using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池管理器，管理所有对象池
/// </summary>
public class PoolManager : Singleton<PoolManager> {

    //用string作为key要注意不同的预制体不能同名
    Dictionary<string, SubPool> poolDict = new Dictionary<string, SubPool>();

    /// <summary>
    /// 创建池
    /// </summary>
    /// <param name="item"></param>
    public void CreateSubPool(GameObject item)
    {
        //安全校验
        if (poolDict.ContainsKey(item.name))
        {
            Debug.LogError(item.name + "池已存在，检查是否重名或重复创建");
            return;
        }
        SubPool subPool = new SubPool(item);
        poolDict.Add(item.name, subPool);
    }

    /// <summary>
    /// 从池中取物体
    /// </summary>
    /// <returns></returns>
    public GameObject Spawn(string objName)
    {
        //安全校验
        if (!poolDict.ContainsKey(objName))
        {
            Debug.LogError(objName + "池不存在，检查名字或是否已创建该池");
            return null;
        }
        return poolDict[objName].Spawn();
    }

    public void UnSpawn(GameObject obj)
    {
        foreach (string poolName in poolDict.Keys)
        {
            if(obj.name == poolName)
            {
                poolDict[obj.name].UnSpawn(obj);
                return;
            }
        }
        Debug.LogError("对象池中没有" + obj.name);
    }

    #region 调试用

    public string[] GetAllPoolName()
    {
        List<string> poolName = new List<string>();
        foreach(string name in poolDict.Keys)
        {
            poolName.Add(name);
        }
        return poolName.ToArray();
    }

    #endregion


}
