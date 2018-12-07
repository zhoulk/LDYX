//===================================================
//作    者：周连康 
//创建时间：2018-12-06 08:58:45
//备    注：
//===================================================

using LTGame;
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

    GameObject m_SelectedGameObject;

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
        UIEventLisner lisner = obj.AddComponent<UIEventLisner>();
        lisner.PointerClickHandler -= OnPointerClickHandler;
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

    public void OnUpdate()
    {
        if (m_SelectedGameObject == null)
        {
            m_SelectedGameObject = UINaviManager.Instance.DefaultObject;
            m_SelectedGameObject.AddOutLine();
        }
        if (LTInput.GetKeyDown(KeyCode2.Up))
        {
            if (m_SelectedGameObject && m_SelectedGameObject.GetUpNavi())
            {
                m_SelectedGameObject.RemoveOutLine();
                m_SelectedGameObject = m_SelectedGameObject.GetUpNavi();
                m_SelectedGameObject.AddOutLine();
            }
        }
        else if (LTInput.GetKeyDown(KeyCode2.Left))
        {
            if (m_SelectedGameObject && m_SelectedGameObject.GetLeftNavi())
            {
                m_SelectedGameObject.RemoveOutLine();
                m_SelectedGameObject = m_SelectedGameObject.GetLeftNavi();
                m_SelectedGameObject.AddOutLine();
            }
        }
        else if (LTInput.GetKeyDown(KeyCode2.Right))
        {
            if (m_SelectedGameObject && m_SelectedGameObject.GetRightNavi())
            {
                m_SelectedGameObject.RemoveOutLine();
                m_SelectedGameObject = m_SelectedGameObject.GetRightNavi();
                m_SelectedGameObject.AddOutLine();
            }
        }
        else if (LTInput.GetKeyDown(KeyCode2.Down))
        {
            if (m_SelectedGameObject && m_SelectedGameObject.GetDownNavi())
            {
                m_SelectedGameObject.RemoveOutLine();
                m_SelectedGameObject = m_SelectedGameObject.GetDownNavi();
                m_SelectedGameObject.AddOutLine();
            }
        }
        else if (LTInput.GetKeyDown(KeyCode2.A))
        {
            Debug.Log("home");
            if (m_SelectedGameObject)
            {
                if (m_ClickHandlerDic.ContainsKey(m_SelectedGameObject))
                {
                    m_ClickHandlerDic[m_SelectedGameObject](m_SelectedGameObject);
                }
            }
        }
    }
}
