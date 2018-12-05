//===================================================
//作    者：周连康 
//创建时间：2018-12-05 11:30:10
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtention {

	public static void Attach(this GameObject obj, GameObject parent)
    {
        if (parent == null) return;
        obj.transform.SetParent(parent.transform, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
}
