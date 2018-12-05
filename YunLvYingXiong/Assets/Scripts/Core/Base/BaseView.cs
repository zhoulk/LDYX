//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:53:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseView {

    /// <summary>
    /// 对应的 ctrl 引用
    /// </summary>
    public BaseCtrl iCtrl;

    /// <summary>
    /// 视图名称
    /// </summary>
    public string viewName;

    /// <summary>
    /// 对应根节点的 transform
    /// </summary>
    public Transform transform;

    /// <summary>
    /// 标记被销毁
    /// </summary>
    public bool isDead;

    /// <summary>
    /// 预设的路径
    /// </summary>
    /// <returns></returns>
    public abstract string PrefabPath();

    public virtual void StartView(params object[] args) { }

    /// <summary>
    /// 销毁
    /// </summary>
    public void Destroy()
    {
        if (transform != null)
        {
            OnBeforeDestroy();
            isDead = true;
            GameObject.Destroy(transform.gameObject);
        }
    }

    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnBeforeDestroy() { }
}
