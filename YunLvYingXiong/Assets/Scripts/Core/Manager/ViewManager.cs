//===================================================
//作    者：周连康 
//创建时间：2018-12-04 20:20:45
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ViewManager : Singleton<ViewManager> {

    #region 属性定义
    GameObject m_CanvasObj;

    public GameObject CancasObj
    {
        get
        {
            return m_CanvasObj;
        }
        set
        {
            m_CanvasObj = value;
        }
    }

    Dictionary<string, BaseView> m_ViewDic = new Dictionary<string, BaseView>();
    Dictionary<string, BaseView> m_UpdateViewDic = new Dictionary<string, BaseView>();

    #endregion

    /// <summary>
    /// 创建页面
    /// </summary>
    /// <param name="ctrl"></param>
    /// <param name="viewName"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public BaseView CreateView(BaseCtrl ctrl, string viewName, params object[] args)
    {
        BaseView view;
        if (!m_ViewDic.ContainsKey(viewName))
        {
            string file = Assembly.GetExecutingAssembly().GetName().Name;
            Assembly assembly = Assembly.Load(file);
            view = (BaseView)assembly.CreateInstance(viewName);
            view.viewName = viewName;
            view.iCtrl = ctrl;

            GameObject viewObj = ResourceManager.Instance.LoadPrefabAndInstance(view.PrefabPath());
            viewObj.Attach(m_CanvasObj);
            view.transform = viewObj.transform;

            m_ViewDic.Add(viewName, view);
        }
        else
        {
            view = m_ViewDic[viewName];
            if (view.isDead)
            {
                GameObject viewObj = ResourceManager.Instance.LoadPrefabAndInstance(view.PrefabPath());
                viewObj.Attach(m_CanvasObj);
                view.transform = viewObj.transform;

                view.viewName = viewName;
                view.iCtrl = ctrl;
                view.isDead = false;
            }
        }

        view.StartView(args);

        return view;
    }

    /// <summary>
    /// 移除页面
    /// </summary>
    /// <param name="viewName"></param>
    public void RemoveView(string viewName)
    {
        if (m_ViewDic.ContainsKey(viewName))
        {
            BaseView view = m_ViewDic[viewName];
            view.Destroy();
        }
    }

    public void OnEndOfFrame()
    {
        m_UpdateViewDic.Clear();
        foreach (var itr in m_ViewDic)
        {
            if (!itr.Value.isDead)
            {
                if (m_UpdateViewDic.ContainsKey(itr.Key))
                {
                    m_UpdateViewDic.Remove(itr.Key);
                }
                m_UpdateViewDic.Add(itr.Key, itr.Value);
            }
        }
    }

    public void OnUpdate()
    {
        foreach (var view in m_UpdateViewDic.Values)
        {
            view.OnUpdate();
        }
    }

    public void OnLateUpdate()
    {
        foreach (var view in m_UpdateViewDic.Values)
        {
            view.OnLateUpdate();
        }
    }
}

