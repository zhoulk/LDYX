//===================================================
//作    者：周连康 
//创建时间：2018-12-05 15:44:09
//备    注：
//===================================================

using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public delegate void EventChangeHandler(MusicEventData data);

public class MusicManager : Singleton<MusicManager> {

    /// <summary>
    /// 挂载根节点
    /// </summary>
    private GameObject m_musicObj;

    /// <summary>
    /// 简单播放器
    /// </summary>
    SimpleMusicPlayer m_simplePlayer;

    public event EventChangeHandler EventChangeHandler;

    AudioClip m_AudioClip;
    public AudioClip AudioClip
    {
        get
        {
            return m_AudioClip;
        }
        set
        {
            m_AudioClip = value;
            LoadGraphy();
        }
    }

    KoreographyTrack m_Track;
    public KoreographyTrack Track
    {
        get
        {
            return m_Track;
        }
        set
        {
            m_Track = value;
            LoadGraphy();
        }
    }

    public void Init()
    {
        if (m_musicObj == null)
        {
            m_musicObj = new GameObject();
            m_musicObj.name = "Music Player";
            m_musicObj.AddComponent<AudioSource>();
            m_musicObj.AddComponent<Koreographer>();
            m_simplePlayer = m_musicObj.AddComponent<SimpleMusicPlayer>();
            GameObject.DontDestroyOnLoad(m_musicObj);
        }

        Koreographer.Instance.RegisterForEvents(MusicEvent.Easy, OnEventEasyChange);
    }

    void LoadGraphy()
    {
        if (m_AudioClip && m_Track)
        {
            Koreography graphy = Resources.Load<Koreography>("Music/StarlitKoreography");
            graphy.SourceClip = m_AudioClip;
            if (graphy.CanAddTrack(m_Track))
            {
                graphy.AddTrack(m_Track);
            }
            m_simplePlayer.LoadSong(graphy);
        }
    }

    void OnEventEasyChange(KoreographyEvent evt)
    {
        string txt = evt.GetTextValue();
        float val = evt.GetFloatValue();


        MusicEventData data = new MusicEventData();
        int oper = Random.Range(1,5);
        data.oper = (MusicOper)oper;
        data.player = MusicPlayer.P1;
        data.content = "haha";

        if (EventChangeHandler != null)
        {
            EventChangeHandler(data);
        }
    }

    public override void Dispose()
    {
        m_AudioClip = null;
        m_Track = null;
        Koreographer.Instance.UnregisterForAllEvents(this);
    }
}
