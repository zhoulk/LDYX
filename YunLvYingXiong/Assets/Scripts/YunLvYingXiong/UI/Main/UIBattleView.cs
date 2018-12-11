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
    public Image bgImage;
    public Text contentText;

    public RectTransform maskTrans;

    public MusicEventData musicData;
    public int outScore;
    public int innerScore;

    public Vector2 collisionSize;
    public Vector3 collisionPoint;

    public Vector2 maskSize;
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

    Color32 m_upColor = new Color32(0,255,255,255);
    Color32 m_longUpColor = new Color32(0, 0, 255, 255);
    Color32 m_downColor = new Color32(255, 0, 255, 255);
    Color32 m_longDownColor = new Color32(255, 255, 0, 255);

    float m_Speed = 5.0f;
    float m_OneSmapleLength = 200;

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
        if (!MusicManager.Instance.IsPlaying())
        {
            _iCtrl.Close();
            return;
        }

        LinkedListNode<BulletItem> temp = m_BulletObjs.First;
        while (temp != null)
        {
            BulletState bulletState = BulletState.Normal;
            BulletItem item = temp.Value;
            MusicEventData musicData = item.musicData;
            float bulletCollisionX1 = item.obj.transform.position.x; // - item.collisionSize.x * 0.5f;

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

            //Debug.Log(bulletState);
            switch (bulletState)
            {
                case BulletState.Dead:
                        m_BulletObjs.Remove(temp);
                        GameObject.Destroy(item.obj);
                    break;
                case BulletState.Normal:
                    switch (musicData.oper)
                    {
                        case MusicOper.LongUp:
                            if (LTInput.GetKey(KeyCode2.Left))
                            {
                                if (item.collisionPoint != Vector3.zero)
                                {
                                    float offset = item.collisionPoint.x - bulletCollisionX1;
                                    item.maskTrans.sizeDelta = new Vector2(item.maskSize.x - offset, item.maskTrans.sizeDelta.y);
                                }
                            }
                            break;
                        case MusicOper.LongDown:
                            if (LTInput.GetKey(KeyCode2.Right))
                            {
                                if (item.collisionPoint != Vector3.zero)
                                {
                                    float offset = Mathf.Abs(item.collisionPoint.x - bulletCollisionX1);
                                    item.maskTrans.sizeDelta = new Vector2(item.maskSize.x - offset, item.maskTrans.sizeDelta.y);
                                }
                            }
                            break;
                    }
                    break;
                case BulletState.IntoOut:
                case BulletState.IntoInner:
                    {
                        switch (musicData.oper)
                        {
                            case MusicOper.Up:
                                if (LTInput.GetKeyDown(KeyCode2.Left))
                                {
                                    m_BulletObjs.Remove(temp);
                                    GameObject.Destroy(item.obj);
                                }
                                break;
                            case MusicOper.LongUp:
                                if (LTInput.GetKey(KeyCode2.Left))
                                {
                                    if (item.collisionPoint == Vector3.zero)
                                    {
                                        item.collisionPoint = new Vector3(bulletCollisionX1, 0, 0);
                                    }

                                    float offset = item.collisionPoint.x - bulletCollisionX1;
                                    item.maskTrans.sizeDelta = new Vector2(item.maskSize.x - offset, item.maskTrans.sizeDelta.y);
                                }
                                break;
                            case MusicOper.Down:
                                if (LTInput.GetKeyDown(KeyCode2.Right))
                                {
                                    m_BulletObjs.Remove(temp);
                                    GameObject.Destroy(item.obj);
                                }
                                break;
                            case MusicOper.LongDown:
                                if (LTInput.GetKey(KeyCode2.Right))
                                {
                                    if (item.collisionPoint == Vector3.zero)
                                    {
                                        item.collisionPoint = new Vector3(bulletCollisionX1, 0, 0);
                                    }
                                    float offset = Mathf.Abs(item.collisionPoint.x - bulletCollisionX1);
                                    item.maskTrans.sizeDelta = new Vector2(item.maskSize.x - offset, item.maskTrans.sizeDelta.y);
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

        Debug.Log("load music complete");
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

    }

    void InitBullet()
    {
        m_Bullets = transform.Find("center/bullets").gameObject;
        m_Bullet = transform.Find("center/bullets/bullet").gameObject;
    }

    //bool isCreate = false;
    void OnEventChangeHandler(MusicEventData data)
    {
        //if (!isCreate)
        //{
        //    data.oper = MusicOper.LongUp;
        //    isCreate = true;
        //}
        //else
        //{
        //    return;
        //}
        GameObject bulletObj = GameObject.Instantiate(m_Bullet);
        bulletObj.Attach(m_Bullets);

        bulletObj.transform.position = m_Enermy.transform.position;
        bulletObj.SetActive(true);
        bulletObj.transform.DOMoveX(m_Player.transform.position.x, m_Speed).SetEase(Ease.Linear);

        BulletItem item = new BulletItem();
        item.obj = bulletObj;
        item.bgImage = bulletObj.transform.Find("bg").GetComponent<Image>();
        item.contentText = bulletObj.transform.Find("bg/Text").GetComponent<Text>();
        item.maskTrans = bulletObj.transform.Find("bg").GetComponent<RectTransform>();
        item.musicData = data;
        item.collisionSize = bulletObj.GetComponent<RectTransform>().sizeDelta;

        switch (data.oper)
        {
            case MusicOper.Up:
                item.outScore = 100;
                item.innerScore = 150;
                item.bgImage.color = m_upColor;
                break;
            case MusicOper.LongUp:
                item.outScore = 200;
                item.innerScore = 300;
                item.bgImage.color = m_longUpColor;
                break;
            case MusicOper.Down:
                item.outScore = 100;
                item.innerScore = 150;
                item.bgImage.color = m_downColor;
                break;
            case MusicOper.LongDown:
                item.outScore = 200;
                item.innerScore = 300;
                item.bgImage.color = m_longDownColor;
                break;
        }

        item.contentText.text = data.content;

        float width = item.contentText.TextWidth();
        if (data.sampleLen > 0)
        {
            if (data.sampleLen * m_OneSmapleLength > width)
            {
                width = data.sampleLen * m_OneSmapleLength;
            }
        }

        item.maskSize = new Vector2(width, 0);
        item.bgImage.GetComponent<RectTransform>().offsetMax = new Vector2(width, 0);
        item.bgImage.GetComponent<RectTransform>().sizeDelta = new Vector2(width, 36);

        m_BulletObjs.AddLast(item);
    }

    public override void OnBeforeDestroy()
    {
        MusicManager.Instance.EventChangeHandler -= OnEventChangeHandler;
        MusicManager.Instance.Dispose();

        m_BulletObjs.Clear();
    }
}
