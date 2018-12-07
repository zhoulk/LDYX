//===================================================
//作    者：周连康 
//创建时间：2018-12-07 17:54:40
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtentionNavi {

    public static void AddNaviLeft(this GameObject obj, GameObject toObj)
    {
        UINaviManager.Instance.AddNavi(obj, toObj, UINaviNodeRelation.Left);
    }

    public static void AddNaviRight(this GameObject obj, GameObject toObj)
    {
        UINaviManager.Instance.AddNavi(obj, toObj, UINaviNodeRelation.Right);
    }

    public static void AddNaviUp(this GameObject obj, GameObject toObj)
    {
        UINaviManager.Instance.AddNavi(obj, toObj, UINaviNodeRelation.Up);
    }

    public static void AddNaviDown(this GameObject obj, GameObject toObj)
    {
        UINaviManager.Instance.AddNavi(obj, toObj, UINaviNodeRelation.Down);
    }

    public static GameObject GetLeftNavi(this GameObject obj)
    {
        return UINaviManager.Instance.GetNavi(obj, UINaviNodeRelation.Left);
    }

    public static GameObject GetRightNavi(this GameObject obj)
    {
        return UINaviManager.Instance.GetNavi(obj, UINaviNodeRelation.Right);
    }

    public static GameObject GetUpNavi(this GameObject obj)
    {
        return UINaviManager.Instance.GetNavi(obj, UINaviNodeRelation.Up);
    }

    public static GameObject GetDownNavi(this GameObject obj)
    {
        return UINaviManager.Instance.GetNavi(obj, UINaviNodeRelation.Down);
    }

    public static void SetAsDefaultNavi(this GameObject obj)
    {
        UINaviManager.Instance.DefaultObject = obj;
    }
}
