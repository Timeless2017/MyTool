using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABTest : MonoBehaviour {


	void Start () {
        GameObject cube = AssetBundleManager.Instance.LoadAsset<GameObject>("AssetBundleTest/Cube/Cube");
        Instantiate(cube);
        DebugAllAB();

        GameObject sprite = AssetBundleManager.Instance.LoadAsset<GameObject>("AssetBundleTest/Sprite/Sprite");
        Instantiate(sprite);
        DebugAllAB();

        //-----------------------UnLoad测试-------------------------------------
        AssetBundleManager.Instance.UnloadAssetBundle("AssetBundleTest/Sprite");
        DebugAllAB();

        AssetBundleManager.Instance.UnloadAssetBundle("AssetBundleTest/Cube");
        DebugAllAB();

    }
	
    private void DebugAllAB()
    {

        Debug.Log("---------------------------------------------------------------");
        string[] allLoadAB = AssetBundleManager.Instance.GetAllLoadedAB();
        foreach (string ab in allLoadAB)
        {
            Debug.Log(ab);
        }
    }

}
