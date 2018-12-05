//===================================================
//作    者：周连康 
//创建时间：2018-12-05 15:44:09
//备    注：
//===================================================

using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class MusicManager : Singleton<MusicManager> {

    /// <summary>
    /// 挂载根节点
    /// </summary>
    private GameObject m_musicObj;

    /// <summary>
    /// 简单播放器
    /// </summary>
    SimpleMusicPlayer m_simplePlayer;

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

        Koreographer.Instance.RegisterForEvents("Bump", OnBumpChange);

        //Koreography graphy = Resources.Load<Koreography>("Music/StarlitKoreography");
        //AudioClip clip = Resources.Load<AudioClip>("Music/Starlit Black - Stem - Melody");
        //graphy.SourceClip = clip;
        //KoreographyTrack track = Resources.Load<KoreographyTrack>("Music/StarlitTrackBump");
        //if (graphy.CanAddTrack(track))
        //{
        //    Debug.Log("can add");
        //    graphy.AddTrack(track);
        //}
        //m_simplePlayer.LoadSong(graphy);
    }

    void OnBumpChange(KoreographyEvent evt)
    {
        string txt = evt.GetTextValue();
        float val = evt.GetFloatValue();

        Debug.Log(txt + "   " + val);
    }
}
