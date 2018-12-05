//===================================================
//作    者：周连康 
//创建时间：2018-12-05 08:54:04
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager> {

    /// <summary>
    /// 加载预设
    /// </summary>
    public GameObject LoadPrefab(string path)
    {
        GameObject prefabObj = Resources.Load<GameObject>(path);
        return prefabObj;
    }

    public GameObject LoadPrefabAndInstance(string path)
    {
        GameObject obj = LoadPrefab(path);
        return GameObject.Instantiate(obj);
    }
}
