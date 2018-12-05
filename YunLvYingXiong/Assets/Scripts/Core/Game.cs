//===================================================
//作    者：周连康 
//创建时间：2018-12-04 18:41:26
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Singleton<Game> {

    private GameObject m_gameObj;
    private GameObject m_GameLoopObj;


	public void Init()
    {
        if (m_gameObj == null)
        {
            m_gameObj = new GameObject();
            m_gameObj.name = "Game";
            m_gameObj.AddComponent<GameLoop>();
            GameObject.DontDestroyOnLoad(m_gameObj);
        }

        GameObject canvasObj = GameObject.Find("Canvas");
        if (canvasObj)
        {
            canvasObj.AddComponent<DontDestroy>();
            ViewManager.Instance.CancasObj = canvasObj;
        }
    }
}
