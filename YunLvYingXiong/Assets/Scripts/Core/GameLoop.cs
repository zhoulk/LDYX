//===================================================
//作    者：周连康 
//创建时间：2018-12-04 18:40:08
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour {

	// Update is called once per frame
	void Update () {

        StartCoroutine(ExecuteCoroutine());

        ViewManager.Instance.OnUpdate();
    }

    private void LateUpdate()
    {
        ViewManager.Instance.OnLateUpdate();
    }

    IEnumerator ExecuteCoroutine()
    {
        yield return new WaitForEndOfFrame();
        ViewManager.Instance.OnEndOfFrame();
    }
}
