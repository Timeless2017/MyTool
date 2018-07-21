/// <summary>
/// 定时器
/// </summary>
public class Timer {

    //  次数，为-1时为无限次
    private int _count = 0;
    //  定时器时长
    private float _duration = 0.0f;
    //  剩余时间
    private float _leftTime = 0.0f;
    //  定时器委托
    private System.Action<object[]> _callback = null;
    //  参数列表
    private object[] _args = null;
    //是否不被Time.scale影响
    private bool _unScale = false;

    /// <summary>
    /// 初始化函数
    /// </summary>
    public void Initialize(int count, float duration, bool unScale, System.Action<object[]> handler, params object[] args)
    {
        _count = count;
        _duration = duration;
        _unScale = unScale;
        _leftTime = duration;
        _callback = handler;
        _args = args;
    }

    /// <summary>
    /// 执行定时器，返回false通知删除定时器
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="unScaleDelta"></param>
    /// <returns></returns>
    public bool Run(float delta, float unScaleDelta)
    {
        if (_callback == null)
            return false;

        if (_unScale)
            _leftTime -= unScaleDelta;
        else
            _leftTime -= delta;

        if (_leftTime > 0.0f)
            return true;
        //有限次数定时器
        if (_count >= 1)
        {
            _callback(_args);
            _leftTime += _duration;
            _count--;
            if (_count == 0)
                return false;
            return true;
        }
        //无限次数定时器
        _callback(_args);
        _leftTime += _duration;
        return true;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        _count = 0;
        _duration = 0.0f;
        _leftTime = 0.0f;
        _unScale = false;
        _callback = null;
        _args = null;
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        Reset();
    }

    bool isPause = false;
    public bool IsPause
    {
        get
        {
            return isPause;
        }
        set
        {
            isPause = value;
        }
    }
}
