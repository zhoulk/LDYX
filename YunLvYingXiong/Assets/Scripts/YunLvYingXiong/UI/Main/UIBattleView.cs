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

public class UIBattleView : BaseView{
    UIBattleViewCtrl _iCtrl;

    GameObject m_Bullets;
    GameObject m_Bullet;
    GameObject m_Player;
    GameObject m_Enermy;

    LinkedList<GameObject> m_BulletObjs = new LinkedList<GameObject>();

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
        LinkedListNode<GameObject> temp = m_BulletObjs.First;
        while (temp != null)
        {
            Debug.Log(temp.Value.transform.position.x + "   "+ m_Player.transform.position.x);
            if (temp.Value.transform.position.x <= m_Player.transform.position.x)
            {
                m_BulletObjs.Remove(temp.Value);
                GameObject.Destroy(temp.Value);
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
    }

    void InitBullet()
    {
        m_Bullets = transform.Find("center/bullets").gameObject;
        m_Bullet = transform.Find("center/bullets/bullet").gameObject;
    }

    void OnEventChangeHandler(MusicEventData data)
    {
        GameObject bulletObj = GameObject.Instantiate(m_Bullet);
        bulletObj.Attach(m_Bullets);
        switch (data.oper)
        {
            case MusicOper.Up:
                bulletObj.transform.Find("up").gameObject.SetActive(true);
                break;
            case MusicOper.LongUp:
                bulletObj.transform.Find("longUp").gameObject.SetActive(true);
                break;
            case MusicOper.Down:
                bulletObj.transform.Find("down").gameObject.SetActive(true);
                break;
            case MusicOper.LongDown:
                bulletObj.transform.Find("longDown").gameObject.SetActive(true);
                break;
        }
        bulletObj.transform.position = m_Enermy.transform.position;
        bulletObj.SetActive(true);
        bulletObj.transform.DOMoveX(m_Player.transform.position.x, 5.0f);

        m_BulletObjs.AddLast(bulletObj);
    }

    public override void OnBeforeDestroy()
    {
        MusicManager.Instance.EventChangeHandler -= OnEventChangeHandler;
        MusicManager.Instance.Dispose();
    }
}
