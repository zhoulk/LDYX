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

class GuanKaItem
{
    public GuanKa guanKa;
    public GameObject obj;
    public Image image;
    public Text nameText;
}

public class UIGuanKaMenuView : BaseView{
    UIGuanKaMenuViewCtrl _iCtrl;

    GameObject m_BackBtn;

    ScrollRect m_GuanKaListScrollRect;
    GameObject m_GuanKaListItem;

    GameObject m_LeftOperBtn;
    GameObject m_RightOperBtn;

    GameObject m_CurrentGuanKaObj;

    /// <summary>
    /// 关卡数量
    /// </summary>
    int m_guanKaCount = 0;
    int m_guanKaItemWidth = 600;
    int m_guanKaMaxWidth = 0;
    int m_guanKaIndex = 1;

    Color m_NormalColor = new Color(1.0f, 1.0f, 1.0f);
    Color m_DisableColor = new Color(0.5f, 0.5f, 0.5f);

    GuanKa[] m_guanKaItems;
    List<GuanKaItem> m_guanKaItemCache = new List<GuanKaItem>();

    public override string PrefabPath()
    {
        return "UIPrefab/Main/UIGuanKaMenuView";
    }

    public override void StartView(params object[] args) {
        _iCtrl = (UIGuanKaMenuViewCtrl)iCtrl;

        InitTopUI();
        InitCenterUI();

        InitNavi();

        IntiData();
    }

    public override void OnResume(params object[] args)
    {
        IntiData();
    }

    void IntiData()
    {
        Debug.Log("guankaMenuView IntiData");

        m_guanKaItems = GuanKaLogic.Instance.GetGuanKaList().ToArray();
        m_guanKaCount = m_guanKaItems.Length;
        m_guanKaMaxWidth = (m_guanKaCount > 0 ? m_guanKaCount - 1 : 0)* m_guanKaItemWidth;

        if (m_guanKaCount > 0)
        {
            m_guanKaItemCache.Clear();
            foreach (var guanKa in m_guanKaItems)
            {
                GameObject gkObj = GameObject.Instantiate(m_GuanKaListItem);
                gkObj.Attach(m_GuanKaListScrollRect.content.gameObject);
                gkObj.SetActive(true);
                gkObj.name = guanKa.id.ToString();

                Image image = gkObj.transform.Find("image").GetComponent<Image>();
                image.color = m_DisableColor;
                image.raycastTarget = false;
                UIEventManager.Instance.AddOnClickHandler(image.gameObject, OnClickGuanKaItem);
                Text nameText = gkObj.transform.Find("name").GetComponent<Text>();

                GuanKaItem item = new GuanKaItem();
                item.guanKa = guanKa;
                item.image = image;
                item.nameText = nameText;
                item.obj = gkObj;
                m_guanKaItemCache.Add(item);

                nameText.text = guanKa.name;
            }
        }

        LoadSong();

        ChangeGuanKaObjNavi();
        m_CurrentGuanKaObj.SetAsDefaultNavi();
    }

    void OnClickGuanKaItem(GameObject obj)
    {
        GuanKa gk = GetCurrentGuanKa();
        if (gk != null)
        {
            _iCtrl.ShowGuanKaDetail(gk);
        }
    }

    void InitTopUI()
    {
        m_BackBtn = transform.Find("top/backBtn").gameObject;
        UIEventManager.Instance.AddOnClickHandler(m_BackBtn, OnBackClick);
    }

    void InitCenterUI()
    {
        m_GuanKaListScrollRect = transform.Find("center/guanKaList").GetComponent<ScrollRect>();
        m_GuanKaListItem = transform.Find("center/guanKaList/item").gameObject;

        m_LeftOperBtn = transform.Find("center/operBtns/leftBtn").gameObject;
        m_RightOperBtn = transform.Find("center/operBtns/rightBtn").gameObject;

        UIEventManager.Instance.AddOnClickHandler(m_LeftOperBtn, OnLeftClick);
        UIEventManager.Instance.AddOnClickHandler(m_RightOperBtn, OnRightClick);
    }

    void InitNavi()
    {
        m_BackBtn.AddNaviDown(m_LeftOperBtn);
        m_BackBtn.AddNaviRight(m_LeftOperBtn);

        m_LeftOperBtn.AddNaviUp(m_BackBtn);
        m_RightOperBtn.AddNaviUp(m_BackBtn);
        m_LeftOperBtn.AddNaviLeft(m_RightOperBtn);
        m_RightOperBtn.AddNaviRight(m_LeftOperBtn);
    }

    void OnBackClick(GameObject obj)
    {
        _iCtrl.Close();
    }

    void OnLeftClick(GameObject obj)
    {
        Vector3 curPosition = m_GuanKaListScrollRect.content.localPosition;
        //移动到最左端
        if (m_guanKaIndex == 1)
        {
            return;
        }
        m_guanKaIndex--;
        m_GuanKaListScrollRect.content.DOLocalMoveX(-1 * (m_guanKaIndex - 1) * m_guanKaItemWidth, 0.25f, true);

        LoadSong();

        ChangeGuanKaObjNavi();
    }

    void OnRightClick(GameObject obj)
    {
        Vector3 curPosition = m_GuanKaListScrollRect.content.localPosition;
        //移动到最左端
        if (m_guanKaIndex == m_guanKaCount)
        {
            return;
        }
        m_guanKaIndex++;
        m_GuanKaListScrollRect.content.DOLocalMoveX(-1 * (m_guanKaIndex-1) * m_guanKaItemWidth, 0.25f, true);

        LoadSong();

        ChangeGuanKaObjNavi();
    }

    void LoadSong()
    {
        GuanKa gk = GetCurrentGuanKa();
        if (gk != null)
        {
            if (!SongManager.Instance.IsSongLoaded(gk.song))
            {
                SongManager.Instance.LoadSong(gk.song, OnLoadSongProgress, OnLoadSongComplete);
            }
            else
            {
                OnLoadSongComplete(null, gk.song);
            }
        }
    }

    void OnLoadSongProgress(SongClient client, float progress)
    {

    }

    void OnLoadSongComplete(SongClient client, Song song)
    {
        GuanKaItem item = null;
        foreach (var gkItem in m_guanKaItemCache)
        {
            if (gkItem.guanKa.song.id == song.id)
            {
                item = gkItem;
                break;
            }
        }
        if (item != null)
        {
            item.image.color = m_NormalColor;
            item.image.raycastTarget = true;
        }
    }

    void UpdateGKState()
    {

    }

    GuanKa GetCurrentGuanKa()
    {
        if (m_guanKaIndex < 1 || m_guanKaIndex > m_guanKaItems.Length)
        {
            Debug.LogError("关卡计数异常 m_guanKaIndex = " + m_guanKaIndex);
            return null;
        }
        else
        {
            return m_guanKaItems[m_guanKaIndex - 1];
        }
    }

    void ChangeGuanKaObjNavi()
    {
        if (m_guanKaIndex < 1 || m_guanKaIndex > m_guanKaItemCache.Count)
        {
            Debug.LogError("关卡计数异常 m_guanKaIndex = " + m_guanKaIndex);
        }
        else
        {
            m_CurrentGuanKaObj = m_guanKaItemCache[m_guanKaIndex - 1].image.gameObject;
            Debug.Log(m_CurrentGuanKaObj);
            m_LeftOperBtn.AddNaviRight(m_CurrentGuanKaObj);
            m_RightOperBtn.AddNaviLeft(m_CurrentGuanKaObj);
            m_CurrentGuanKaObj.AddNaviLeft(m_LeftOperBtn);
            m_CurrentGuanKaObj.AddNaviRight(m_RightOperBtn);
        }
    }

    public override void OnBeforeDestroy()
    {
        UIEventManager.Instance.RemoveClickHandler(m_BackBtn);
        UIEventManager.Instance.RemoveClickHandler(m_LeftOperBtn);
        UIEventManager.Instance.RemoveClickHandler(m_RightOperBtn);

        m_guanKaCount = 0;
        m_guanKaItems = null;
        m_guanKaIndex = 1;
    }
}
