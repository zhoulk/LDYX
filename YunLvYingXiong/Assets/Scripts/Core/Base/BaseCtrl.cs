//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:53:17
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCtrl {

    public BaseView view;

    public string ctrlName;

    public virtual void Start(params object[] args) { }

    public virtual void OnDestroy() { }

    public virtual void OnUpdate() { }
}
