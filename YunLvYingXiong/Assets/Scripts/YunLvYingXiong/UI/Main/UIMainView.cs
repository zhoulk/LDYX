//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainView : BaseView{
    UIMainViewCtrl _iCtrl;

    GameObject m_chuangGuanBtn;
    GameObject m_PKBtn;
    GameObject m_heZouBtn;
    GameObject m_onLinePKBtn;
    GameObject m_settingBtn;

    public override string PrefabPath()
    {
        return "UIPrefab/Main/UIMainView";
    }

    public override void StartView(params object[] args) {
        _iCtrl = (UIMainViewCtrl)iCtrl;

        InitMenuBtns();
    }

    /// <summary>
    /// 初始化菜单
    /// </summary>
    void InitMenuBtns()
    {
        m_chuangGuanBtn = transform.Find("btns/chuangGuanBtn").gameObject;
        m_PKBtn = transform.Find("btns/pkBtn").gameObject;
        m_heZouBtn = transform.Find("btns/heZouBtn").gameObject;
        m_onLinePKBtn = transform.Find("btns/onLinePKBtn").gameObject;
        m_settingBtn = transform.Find("btns/settingBtn").gameObject;

        UIEventManager.Instance.AddOnClickHandler(m_chuangGuanBtn, OnChuangGuanClick);
        UIEventManager.Instance.AddOnClickHandler(m_PKBtn, OnPKClick);
        UIEventManager.Instance.AddOnClickHandler(m_heZouBtn, OnHeZouClick);
        UIEventManager.Instance.AddOnClickHandler(m_onLinePKBtn, OnOnLinePKClick);
        UIEventManager.Instance.AddOnClickHandler(m_settingBtn, OnSettingClick);
    }

    public override void OnBeforeDestroy()
    {
        UIEventManager.Instance.RemoveClickHandler(m_chuangGuanBtn);
        UIEventManager.Instance.RemoveClickHandler(m_PKBtn);
        UIEventManager.Instance.RemoveClickHandler(m_heZouBtn);
        UIEventManager.Instance.RemoveClickHandler(m_onLinePKBtn);
        UIEventManager.Instance.RemoveClickHandler(m_settingBtn);
    }

    void OnChuangGuanClick(GameObject obj)
    {
        _iCtrl.ShowGuanKaMenu();
    }

    void OnPKClick(GameObject obj)
    {
        Debug.Log(obj.name);
    }

    void OnHeZouClick(GameObject obj)
    {
        Debug.Log(obj.name);
    }

    void OnOnLinePKClick(GameObject obj)
    {
        Debug.Log(obj.name);
    }

    void OnSettingClick(GameObject obj)
    {
        Debug.Log(obj.name);
    }
}
