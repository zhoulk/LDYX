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
    GameObject m_songObj;
    AudioSource m_SongAudioSource;

    /// <summary>
    /// 简单播放器
    /// </summary>
    SimpleMusicPlayer m_simplePlayer;

    public event EventChangeHandler EventChangeHandler;

    /// <summary>
    /// 上一次的取样
    /// </summary>
    int m_PreSample = 0;

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

    Koreography m_Koreography;

    /// <summary>
    /// 延迟播放时间
    /// </summary>
    float m_DelaySeconds = 0;
    bool m_NeedPlay = false;

    public void Init()
    {
        if (m_musicObj == null)
        {
            m_musicObj = new GameObject();
            m_musicObj.name = "Music Player";
            AudioSource source = m_musicObj.AddComponent<AudioSource>();
            source.volume = 0;
            m_musicObj.AddComponent<Koreographer>();
            m_simplePlayer = m_musicObj.AddComponent<SimpleMusicPlayer>();
            GameObject.DontDestroyOnLoad(m_musicObj);
        }
        if (m_songObj == null)
        {
            m_songObj = new GameObject();
            m_songObj.name = "Song Player";
            m_SongAudioSource = m_songObj.AddComponent<AudioSource>();
            m_SongAudioSource.playOnAwake = false;
            GameObject.DontDestroyOnLoad(m_songObj);
        }
    }

    public bool IsPlaying()
    {
        if (m_simplePlayer)
        {
            return m_simplePlayer.IsPlaying;
        }
        return false;
    }

    void LoadGraphy()
    {
        if (m_AudioClip && m_Track)
        {
            Koreographer.Instance.RegisterForEvents(m_Track.EventID, OnEventEasyChange);

            if (m_Koreography == null)
            {
                m_Koreography = Resources.Load<Koreography>("Music/StarlitKoreography");
            }

            // 移除空的音轨
            m_Koreography.CheckTrackListIntegrity();

            m_Koreography.SourceClip = m_AudioClip;
            if (m_Koreography.CanAddTrack(m_Track))
            {
                m_Koreography.AddTrack(m_Track);
            }
            m_simplePlayer.LoadSong(m_Koreography);
            m_SongAudioSource.clip = m_AudioClip;

            m_SongAudioSource.Play((ulong)(2.5 * 44100));
        }
    }

    void OnEventEasyChange(KoreographyEvent evt)
    {
        string txt = evt.GetTextValue();

        if (m_PreSample == evt.StartSample)
        {
            return;
        }
        m_PreSample = evt.StartSample;

        string[] values = txt.Split(',');

        if (values.Length < 3)
        {
            return;
        }

        MusicEventData data = new MusicEventData();
        data.oper = (MusicOper)int.Parse(values[0]);
        data.player = (MusicPlayer)int.Parse(values[1]);
        data.content = values[2];
        data.sampleLen = (evt.EndSample - evt.StartSample) / (float)Koreographer.GetSampleRate(m_AudioClip.name);

        //data.oper = MusicOper.LongUp;
        //data.sampleLen = 1.5f;

        //Debug.Log(txt + "   " + data.sampleLen);

        if (EventChangeHandler != null)
        {
            EventChangeHandler(data);
        }
    }

    public void OnUpdate()
    {
        if (m_NeedPlay)
        {
            m_DelaySeconds -= Time.deltaTime;
            if (m_DelaySeconds < 0)
            {
            }
        }
    }

    public override void Dispose()
    {
        Koreographer.Instance.UnregisterForAllEvents(this);
        m_AudioClip = null;
        m_Track = null;
    }
}
