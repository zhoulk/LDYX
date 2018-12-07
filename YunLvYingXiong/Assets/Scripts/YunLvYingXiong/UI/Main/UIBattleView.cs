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
using SonicBloom.Koreo;
using LTGame;

class BulletItem
{
    public GameObject obj;
    public GameObject upObj;
    public GameObject longUpObj;
    public GameObject downObj;
    public GameObject longDownObj;

    public RectTransform longUpMaskTrans;
    public RectTransform longDownMaskTrans;

    public MusicEventData musicData;
    public int outScore;
    public int innerScore;

    public Vector2 collisionSize;
    public Vector3 collisionPoint;

    public Vector2 longUpMaskSize;
    public Vector2 longDownMaskSize;
}

enum BulletState{
    Normal,
    IntoOut,
    IntoInner,
    Dead
}

public class UIBattleView : BaseView{
    UIBattleViewCtrl _iCtrl;

    GameObject m_Bullets;
    GameObject m_Bullet;

    GameObject m_Player;
    GameObject m_Enermy;

    GameObject m_OutTarget;
    GameObject m_InnerTarget;

    LinkedList<BulletItem> m_BulletObjs = new LinkedList<BulletItem>();

    float m_OutTargetX1;
    float m_OutTargetX2;
    float m_InnerTargetX1;
    float m_InnerTargetX2;

    GuanKa m_GuanKa;
    public override string PrefabPath()
    {
        return "UIPrefab/Main/UIBattleView";
    }

    public override void StartView(params object[] args) {
        _iCtrl = (UIBattleViewCtrl)iCtrl;
        m_GuanKa = (GuanKa)args[0];

        InitCenterUI();
        InitBullet();

        InitMusic();
    }

    public override void OnUpdate()
    {
        LinkedListNode<BulletItem> temp = m_BulletObjs.First;
        while (temp != null)
        {
            BulletState bulletState = BulletState.Normal;
            BulletItem item = temp.Value;
            MusicEventData musicData = item.musicData;
            float bulletCollisionX1 = item.obj.transform.position.x - item.collisionSize.x;

            if (bulletCollisionX1 <= m_Player.transform.position.x)
            {
                bulletState = BulletState.Dead;
            }
            else
            {
                //进入外圈
                if (bulletCollisionX1 <= m_OutTargetX2 && bulletCollisionX1 >= m_OutTargetX1)
                {
                    bulletState = BulletState.IntoOut;
                    if (bulletCollisionX1 <= m_InnerTargetX2 && bulletCollisionX1 >= m_InnerTargetX1)
                    {
                        bulletState = BulletState.IntoInner;
                    }
                }
            }

            switch (bulletState)
            {
                case BulletState.Dead:
                        m_BulletObjs.Remove(temp);
                        GameObject.Destroy(item.obj);
                    break;
                case BulletState.IntoOut:
                case BulletState.IntoInner:
                    {
                        switch (musicData.oper)
                        {
                            case MusicOper.Up:
                                if (LTInput.GetKeyDown(KeyCode2.Up))
                                {
                                    m_BulletObjs.Remove(temp);
                                    GameObject.Destroy(item.obj);
                                }
                                break;
                            case MusicOper.LongUp:
                                if (LTInput.GetKey(KeyCode2.Up))
                                {
                                    if (item.collisionPoint == Vector3.zero)
                                    {
                                        item.collisionPoint = new Vector3(bulletCollisionX1, 0, 0);
                                    }

                                    float offset = item.collisionPoint.x - bulletCollisionX1;
                                    item.longUpMaskTrans.sizeDelta = new Vector2(item.longUpMaskSize.x - offset, item.longUpMaskSize.y);
                                }
                                break;
                            case MusicOper.Down:
                                if (LTInput.GetKeyDown(KeyCode2.Down))
                                {
                                    m_BulletObjs.Remove(temp);
                                    GameObject.Destroy(item.obj);
                                }
                                break;
                            case MusicOper.LongDown:
                                if (LTInput.GetKey(KeyCode2.Down))
                                {
                                    if (item.collisionPoint == Vector3.zero)
                                    {
                                        item.collisionPoint = new Vector3(bulletCollisionX1, 0, 0);
                                    }
                                    float offset = Mathf.Abs(item.collisionPoint.x - bulletCollisionX1);
                                    item.longDownMaskTrans.sizeDelta = new Vector2(item.longDownMaskSize.x - offset, item.longDownMaskSize.y);
                                }
                                break;
                        }
                    }
                    break;
            }

            temp = temp.Next;
        }
    }

    void InitMusic()
    {
        MusicManager.Instance.EventChangeHandler += OnEventChangeHandler;

        AudioClip audioClip = SongManager.Instance.GetAudioClip(m_GuanKa.song.id);
        Track track = null;
        foreach (var tr in m_GuanKa.song.songTracks)
        {
            if (tr.trackLevel == (int)m_GuanKa.level)
            {
                track = tr;
            }
        }
        KoreographyTrack graphyTrack = null;
        if (track != null)
        {
            graphyTrack = SongManager.Instance.GetTrack(track.id);
        }
        MusicManager.Instance.AudioClip = audioClip;
        MusicManager.Instance.Track = graphyTrack;
    }

    void InitCenterUI()
    {
        m_Player = transform.Find("center/player").gameObject;
        m_Enermy = transform.Find("center/enermy").gameObject;
        m_OutTarget = transform.Find("center/target").gameObject;
        m_InnerTarget = transform.Find("center/target/small").gameObject;

        m_OutTargetX1 = m_OutTarget.transform.position.x - m_OutTarget.transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        m_OutTargetX2 = m_OutTarget.transform.position.x + m_OutTarget.transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        m_InnerTargetX1 = m_InnerTarget.transform.position.x - m_InnerTarget.transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;
        m_InnerTargetX2 = m_InnerTarget.transform.position.x + m_InnerTarget.transform.GetComponent<RectTransform>().sizeDelta.x * 0.5f;

        Debug.Log(m_OutTargetX1);
        Debug.Log(m_OutTargetX2);
        Debug.Log(m_InnerTargetX1);
        Debug.Log(m_InnerTargetX2);
    }

    void InitBullet()
    {
        m_Bullets = transform.Find("center/bullets").gameObject;
        m_Bullet = transform.Find("center/bullets/bullet").gameObject;
    }

    bool isCreate = false;
    void OnEventChangeHandler(MusicEventData data)
    {
        if (!isCreate)
        {
            data.oper = MusicOper.LongUp;
            isCreate = true;
        }
        else
        {
            return;
        }
        GameObject bulletObj = GameObject.Instantiate(m_Bullet);
        bulletObj.Attach(m_Bullets);

        bulletObj.transform.position = m_Enermy.transform.position;
        bulletObj.SetActive(true);
        bulletObj.transform.DOMoveX(m_Player.transform.position.x, 5.0f).SetEase(Ease.Linear);

        BulletItem item = new BulletItem();
        item.obj = bulletObj;
        item.upObj = bulletObj.transform.Find("up").gameObject;
        item.longUpObj = bulletObj.transform.Find("longUp").gameObject;
        item.downObj = bulletObj.transform.Find("down").gameObject;
        item.longDownObj = bulletObj.transform.Find("longDown").gameObject;
        item.longUpMaskTrans = bulletObj.transform.Find("longUp").GetComponent<RectTransform>();
        item.longDownMaskTrans = bulletObj.transform.Find("longDown").GetComponent<RectTransform>();
        item.musicData = data;
        item.collisionSize = bulletObj.GetComponent<RectTransform>().sizeDelta;
        item.longUpMaskSize = item.longUpMaskTrans.sizeDelta;
        item.longDownMaskSize = item.longDownMaskTrans.sizeDelta;

        switch (data.oper)
        {
            case MusicOper.Up:
                item.upObj.SetActive(true);
                item.outScore = 100;
                item.innerScore = 150;
                break;
            case MusicOper.LongUp:
                item.longUpObj.SetActive(true);
                item.outScore = 200;
                item.innerScore = 300;
                break;
            case MusicOper.Down:
                item.downObj.SetActive(true);
                item.outScore = 100;
                item.innerScore = 150;
                break;
            case MusicOper.LongDown:
                item.longDownObj.SetActive(true);
                item.outScore = 200;
                item.innerScore = 300;
                break;
        }

        m_BulletObjs.AddLast(item);
    }

    public override void OnBeforeDestroy()
    {
        MusicManager.Instance.EventChangeHandler -= OnEventChangeHandler;
        MusicManager.Instance.Dispose();
    }
}
