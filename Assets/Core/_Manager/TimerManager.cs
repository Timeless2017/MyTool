using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerManager : Singleton<TimerManager>
{

    private LinkedList<Timer> m_TimerList = new LinkedList<Timer>();
    protected LinkedListNode<Timer> m_curTimer = null;

    /// <summary>
    /// 更新定时器
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <param name="unscaledDeltaTime"></param>
    public void Update(float deltaTime, float unscaledDeltaTime)
    {
        m_curTimer = m_TimerList.First;
        while (m_curTimer != null)
        {
            try
            {
                if (m_curTimer.Value.IsPause)
                    continue;
                if (!m_curTimer.Value.Run(deltaTime, unscaledDeltaTime))
                    DeleteCurrentTimer();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                DeleteCurrentTimer();
            }
        }
    }

    #region 删除定时器

    /// <summary>
    /// 删除当前定时器并指向下一个定时器
    /// </summary>
    private void DeleteCurrentTimer()
    {
        LinkedListNode<Timer> nextTimer = m_curTimer.Next;
        RemoveTimer(m_curTimer.Value);
        m_curTimer = nextTimer;
    }

    public void RemoveTimer(Timer timer)
    {
        if (timer == null)
            return;
        m_TimerList.Remove(timer);
        timer.Dispose();
        timer = null;
    }

    public void ClearTimer()
    {
        Debug.Log("总共计时器个数为：" + m_TimerList.Count);
        m_curTimer = m_TimerList.First;
        var cur_timer = m_curTimer;
        while (m_curTimer != null)
        {
            cur_timer = m_curTimer;
            m_curTimer = m_curTimer.Next;
            if (m_TimerList == cur_timer.List)
            {
                Debug.Log("从TimerManager中移除计时器=====");
                m_TimerList.Remove(cur_timer);
                cur_timer.Value.Dispose();
            }
        }
        Debug.Log("移除后总共计时器个数为：" + m_TimerList.Count);
    }

    #endregion

    #region 添加定时器

    private Timer InternalAddTimer(int count, float duration, bool unScale, System.Action<object[]> handler, params object[] args)
    {
        if (duration < 0.0f)
        {
            Debug.LogError("duration不能小于0");
            return null;
        }

        Timer timer = CreateObj();
        if (timer == null)
            return null;

        timer.Initialize(count, duration, unScale, handler, args);
        m_TimerList.AddFirst(timer);
        return timer;
    }

    /// <summary>
    /// 增加定时器
    /// <param name="unScale">True:不受Time.scale影响,false相反</param>
    /// </summary>
    public Timer AddOnceTimer(float duration, bool unScale, System.Action<object[]> handler, params object[] args)
    {
        return InternalAddTimer(1, duration, unScale, handler, args);
    }

    /// <summary>
    /// 增加计数定时器
    /// <param name="unScale">True:不受Time.scale影响,false相反</param>
    /// </summary>
    public Timer AddCountTimer(float duration, bool unScale, System.Action<object[]> handler, uint count, params object[] args)
    {
        return InternalAddTimer((int)count, duration, unScale, handler, args);
    }

    /// <summary>
    /// 增加持续定时器
    /// <param name="unScale">True:不受Time.scale影响,false相反</param>
    /// </summary>
    public Timer AddRepeatTimer(float duration, bool unScale, System.Action<object[]> handler, params object[] args)
    {
        return InternalAddTimer(-1, duration, unScale, handler, args);
    }

    #endregion

    /// <summary>
    /// 重置指定定时器
    /// </summary>
    public void ResetTimer(Timer timer)
    {
        if (timer == null)
            return;
        timer.Reset();
    }

    /// <summary>
    /// 从对象池创建时间对象
    /// </summary>
    private Timer CreateObj()
    {
        return new Timer();
    }

    /// <summary>
    /// 是否在运行
    /// </summary>
    public bool IsRunning(Timer timer)
    {
        var timerNode = m_TimerList.First;
        while (null != timerNode)
        {
            var curTimerNode = timerNode;
            timerNode = timerNode.Next;

            if (curTimerNode.Value == timer)
            {
                return true;
            }
        }
        return false;
    }

}
