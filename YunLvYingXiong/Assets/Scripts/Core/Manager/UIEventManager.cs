//===================================================
//作    者：周连康 
//创建时间：2018-12-06 08:58:45
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// UIEvent 管理
/// </summary>


public delegate void OnClick(GameObject obj);

public class UIEventManager : Singleton<UIEventManager> {

    Dictionary<GameObject, OnClick> m_ClickHandlerDic = new Dictionary<GameObject, OnClick>();

    /// <summary>
    /// 添加点击事件处理
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="onClick"></param>
    public void AddOnClickHandler(GameObject obj, OnClick onClick)
    {
        if (m_ClickHandlerDic.ContainsKey(obj))
        {
            m_ClickHandlerDic.Remove(obj);
        }
        m_ClickHandlerDic.Add(obj, onClick);
        UIEventLisner lisner = obj.AddComponent<UIEventLisner>();
        lisner.PointerClickHandler += OnPointerClickHandler;
    }

    /// <summary>
    /// 移除点击事件处理
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveClickHandler(GameObject obj)
    {
        if (m_ClickHandlerDic.ContainsKey(obj))
        {
            m_ClickHandlerDic.Remove(obj);
        }
    }

    void OnPointerClickHandler(PointerEventData data)
    {
        if (data.pointerPress)
        {
            if (m_ClickHandlerDic.ContainsKey(data.pointerPress))
            {
                m_ClickHandlerDic[data.pointerPress](data.pointerPress);
            }
        }
    }
}
