//===================================================
//作    者：周连康 
//创建时间：2018-12-04 17:42:22
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.DontDestroyOnLoad(this.gameObject);
	}

}
