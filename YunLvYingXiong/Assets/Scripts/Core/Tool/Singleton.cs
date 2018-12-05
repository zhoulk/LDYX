//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:38:59
//备    注：
//===================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : IDisposable  where T:new() {

    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new T();
            }
            return _instance;
        }
    }

    public virtual void Dispose()
    {
    }
}
