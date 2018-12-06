//===================================================
//作    者：周连康 
//创建时间：2018-12-04 15:37:44
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIGuanKaDetailView : BaseView{
    UIGuanKaDetailViewCtrl _iCtrl;

    GameObject m_BackBtn;

    GameObject m_EasyBtn;
    GameObject m_MidBtn;
    GameObject m_HardBtn;

    GameObject m_GoBtn;

    GuanKa m_GuanKa;

    public override string PrefabPath()
    {
        return "UIPrefab/Main/UIGuanKaDetailView";
    }

    public override void StartView(params object[] args) {
        _iCtrl = (UIGuanKaDetailViewCtrl)iCtrl;

        InitTopUI();
        InitCenterUI();

        m_GuanKa = (GuanKa)args[0];
        m_GuanKa.level = GuanKaLevel.Easy;
    }
    
    void InitTopUI()
    {
        m_BackBtn = transform.Find("top/backBtn").gameObject;
        UIEventManager.Instance.AddOnClickHandler(m_BackBtn, OnBackClick);
    }

    void InitCenterUI()
    {
        m_EasyBtn = transform.Find("center/levelBtns/easyBtn").gameObject;
        m_MidBtn = transform.Find("center/levelBtns/midBtn").gameObject;
        m_HardBtn = transform.Find("center/levelBtns/hardBtn").gameObject;
        m_GoBtn = transform.Find("center/goBtn").gameObject;

        UIEventManager.Instance.AddOnClickHandler(m_EasyBtn, OnEasyClick);
        UIEventManager.Instance.AddOnClickHandler(m_MidBtn, OnMidClick);
        UIEventManager.Instance.AddOnClickHandler(m_HardBtn, OnHardClick);
        UIEventManager.Instance.AddOnClickHandler(m_GoBtn, OnGoClick);
    }

    void OnBackClick(GameObject obj)
    {
        _iCtrl.Close();
    }

    void OnEasyClick(GameObject obj)
    {
        m_GuanKa.level = GuanKaLevel.Easy;
    }

    void OnMidClick(GameObject obj)
    {
        m_GuanKa.level = GuanKaLevel.Mid;
    }

    void OnHardClick(GameObject obj)
    {
        m_GuanKa.level = GuanKaLevel.Hard;
    }

    void OnGoClick(GameObject obj)
    {
        _iCtrl.ShowBattleView(m_GuanKa);
    }

    public override void OnBeforeDestroy()
    {
        UIEventManager.Instance.RemoveClickHandler(m_BackBtn);
        UIEventManager.Instance.RemoveClickHandler(m_EasyBtn);
        UIEventManager.Instance.RemoveClickHandler(m_MidBtn);
        UIEventManager.Instance.RemoveClickHandler(m_HardBtn);
        UIEventManager.Instance.RemoveClickHandler(m_GoBtn);
    }
}
