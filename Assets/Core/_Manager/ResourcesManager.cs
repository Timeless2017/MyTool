using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourcesManager : Singleton<ResourcesManager>
{

    public bool useAssetsBundleInEditor = false;

    /// <summary>
    /// 通过EditorResources后的完整路径(需后缀名)加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path">EditorResources后的完整路径(需后缀名)</param>
    /// <returns></returns>
    public T LoadAssetByFullName<T>(string path) where T : Object
    {
#if (UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE) && !UNITY_EDITOR
        path = path.Remove(path.LastIndexOf('.'));
        return AssetBundleManager.Instance.LoadAsset<T>(path);
#else
        if (useAssetsBundleInEditor)
        {
            path = path.Remove(path.LastIndexOf('.'));
            return AssetBundleManager.Instance.LoadAsset<T>(path);
        }
        else
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/EditorResources/" + path);
        }
#endif
    }

    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="pathName">EditorResources/Prefab后的路径</param>
    /// <returns></returns>
    public GameObject LoadPrefab(string pathName)
    {
#if (UNITY_IOS || UNITY_ANDROID || UNITY_STANDALONE) && !UNITY_EDITOR
        return AssetBundleManager.Instance.LoadAsset<GameObject>(string.Format("prefab/{0}", pathName));
#else
        if (useAssetsBundleInEditor)
        {
            return AssetBundleManager.Instance.LoadAsset<GameObject>(string.Format("prefab/{0}", pathName));
        }
        else
        {
            string path = string.Format("Assets/EditorResources/Prefab/{0}.prefab", pathName);
            return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
#endif
    }

    string directory = Application.dataPath + @"/StreamingAssets/Json/";
    public string LoadJson(string path)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(Application.dataPath + @"/StreamingAssets/Json");

        string unityPath = directory + path + ".json";
        if (!File.Exists(unityPath))
        {
            Debug.LogError(unityPath + "不存在");
            return "";
        }
        StreamReader sr = new StreamReader(unityPath);
        string jsonStr = sr.ReadToEnd();
        sr.Close();
        return jsonStr;
    }

    public void SaveJson(string path, string jsonStr)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(Application.dataPath + @"/StreamingAssets/Json");

        int index = path.LastIndexOf('/');
        //如果path有文件夹，先判断文件夹是否存在，不存在则创建
        if (index != -1)
        {
            string dirName = directory + path.Remove(index);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
        }

        StreamWriter sw = new StreamWriter(directory + path + ".json");
        sw.Write(jsonStr);
        sw.Close();
    }

}
