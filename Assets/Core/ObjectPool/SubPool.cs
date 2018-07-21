using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool {

    private GameObject prefab;
    private List<GameObject> objList = new List<GameObject>();

    public SubPool(GameObject pfb)
    {
        this.prefab = pfb;
    }

    public string PoolName
    {
        get { return prefab.name; }
    }

    public GameObject Spawn()
    {
        GameObject obj = null;
        foreach (GameObject item in objList)
        {
            if (!item.activeSelf)
            {
                obj = item;
                obj.SetActive(true);
                break;
            }
        }
        if(obj == null)
        {
            obj = GameObject.Instantiate(prefab);
            objList.Add(obj);
        }
        return obj;
    }

    public void UnSpawn(GameObject gameObject)
    {
        foreach (GameObject item in objList)
        {
            if (gameObject == item)
            {
                item.SetActive(false);
                return;
            }
        }
    }


}
