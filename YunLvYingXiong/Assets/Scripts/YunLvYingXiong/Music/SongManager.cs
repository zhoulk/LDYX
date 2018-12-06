//===================================================
//作    者：周连康 
//创建时间：2018-12-05 17:19:59
//备    注：
//===================================================

using SonicBloom.Koreo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using UnityEngine;

public delegate void LoadSongComplete(Song song);

public class SongManager : Singleton<SongManager> {

    /// <summary>
    /// 歌单本地路径
    /// </summary>
    private string m_localSongListFilePath;
    /// <summary>
    /// 歌单本地临时路径
    /// </summary>
    private string m_localSongListTempFilePath;

    AssetBundleCreateRequest m_AssetCreateRequst;
    AssetBundleRequest m_AssetBundleRequest;
    AssetBundle m_AssetBundle;

    Dictionary<int, AudioClip> m_AudioClipCache = new Dictionary<int, AudioClip>();
    Dictionary<int, KoreographyTrack> m_AudioTrackCache = new Dictionary<int, KoreographyTrack>();

    Song m_CurrentLoadingSong;
    Track m_CurrentLoadingTrack;
    public event LoadSongComplete LoadSongComplete;

    public SongManager()
    {
        m_localSongListFilePath = PathConfig.LocalSongPath.AppendFileName("songList.json");
        m_localSongListTempFilePath = PathConfig.LocalTempSongPath.AppendFileName("songList.json");

        DownloadManager.Instance.DownloadFileCompleted += OnDownloadFileCompleted;
    }

    public void OnUpdate()
    {
        
    }

    /// <summary>
    /// 获取音频
    /// </summary>
    /// <param name="songId"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(int songId)
    {
        AudioClip audio;
        m_AudioClipCache.TryGetValue(songId, out audio);
        return audio;
    }

    /// <summary>
    /// 获取音轨
    /// </summary>
    /// <param name="trackId"></param>
    /// <returns></returns>
    public KoreographyTrack GetTrack(int trackId)
    {
        KoreographyTrack track;
        m_AudioTrackCache.TryGetValue(trackId, out track);
        return track;
    }

    /// <summary>
    /// 获取本地歌曲列表
    /// </summary>
    /// <returns></returns>
    public List<Song> GetLocalSongList()
    {
        SongConfig localConfig = null;
        try
        {
            string localSongListTxt = File.ReadAllText(m_localSongListFilePath);
            localConfig = JsonUtility.FromJson<SongConfig>(localSongListTxt);
        }
        catch (Exception)
        {
        }

        return localConfig != null ? localConfig.songs : null;
    }

    /// <summary>
    /// 歌曲是否已经加载
    /// </summary>
    /// <param name="song"></param>
    /// <returns></returns>
    public bool IsSongLoaded(Song song)
    {
        return m_AudioClipCache.ContainsKey(song.id);
    }

    /// <summary>
    /// 加载歌曲
    /// </summary>
    public void LoadSong(Song song)
    {
        m_CurrentLoadingSong = song;
        string songABPath = PathConfig.LocalSongPath.AppendFileName(song.songABUrl);
        //string songABPath = Application.dataPath + "/StreamAssets/01.ab";
        Debug.Log(songABPath);
        if (File.Exists(songABPath))
        {
            m_AssetCreateRequst = AssetBundle.LoadFromFileAsync(songABPath, 0);
            m_AssetCreateRequst.completed += OnCreateAssetBundleComplete;
        }
        else
        {
            DownloadManager.Instance.DownLoadFile(ServerConfig.SongBaseUrl + "/" + song.songABUrl, songABPath);
        }
    }

    void LoadSongToMemery()
    {
        m_AssetBundleRequest = m_AssetBundle.LoadAssetAsync<AudioClip>(m_CurrentLoadingSong.songName);
        m_AssetBundleRequest.completed += OnLoadAudioClipComplete;
    }

    void LoadTrackToMemery()
    {
        m_AssetBundleRequest = m_AssetBundle.LoadAssetAsync<KoreographyTrack>(m_CurrentLoadingTrack.trackName);
        m_AssetBundleRequest.completed += OnLoadKoreographyTrackComplete;
    }

    void OnCreateAssetBundleComplete(UnityEngine.AsyncOperation oper)
    {
        m_AssetBundle = m_AssetCreateRequst.assetBundle;
        LoadAllToMemery();
    }

    void LoadAllToMemery()
    {
        if (!m_AudioClipCache.ContainsKey(m_CurrentLoadingSong.id))
        {
            LoadSongToMemery();
        }
        else
        {
            bool isAllLoad = true;
            foreach (var track in m_CurrentLoadingSong.songTracks)
            {
                if (!m_AudioTrackCache.ContainsKey(track.id))
                {
                    m_CurrentLoadingTrack = track;
                    LoadTrackToMemery();
                    isAllLoad = false;
                }
            }
            if (isAllLoad)
            {
                if (LoadSongComplete != null)
                {
                    LoadSongComplete(m_CurrentLoadingSong);
                }
            }
        }
    }

    void OnLoadAudioClipComplete(UnityEngine.AsyncOperation oper)
    {
        AudioClip clip = null;
        if (m_AssetBundleRequest.asset.GetType().Equals(typeof(AudioClip)))
        {
            clip = (AudioClip)m_AssetBundleRequest.asset;
        }

        if (m_AudioClipCache.ContainsKey(m_CurrentLoadingSong.id))
        {
            m_AudioClipCache.Remove(m_CurrentLoadingSong.id);
        }
        m_AudioClipCache.Add(m_CurrentLoadingSong.id, clip);

        LoadAllToMemery();
    }

    void OnLoadKoreographyTrackComplete(UnityEngine.AsyncOperation oper)
    {
        KoreographyTrack track = null;
        if (m_AssetBundleRequest.asset.GetType().Equals(typeof(KoreographyTrack)))
        {
            track = (KoreographyTrack)m_AssetBundleRequest.asset;
        }

        if (m_AudioTrackCache.ContainsKey(m_CurrentLoadingTrack.id))
        {
            m_AudioTrackCache.Remove(m_CurrentLoadingTrack.id);
        }
        m_AudioTrackCache.Add(m_CurrentLoadingTrack.id, track);

        LoadAllToMemery();
    }

    /// <summary>
    /// 更新歌单
    /// </summary>
    public void UpdateSongList()
    {
        DownLoadSongList();
    }

    /// <summary>
    /// 检查歌单版本
    /// </summary>
    private void CheckSongListVersion()
    {
        Debug.Log("check update start !!!");

        SongConfig localConfig = null;
        SongConfig tempConfig = null;
        try
        {
            string tempSongListTxt = File.ReadAllText(m_localSongListTempFilePath);
            tempConfig = JsonUtility.FromJson<SongConfig>(tempSongListTxt);

            string localSongListTxt = File.ReadAllText(m_localSongListFilePath);
            localConfig = JsonUtility.FromJson<SongConfig>(localSongListTxt);
        }
        catch(Exception)
        {
        }
        if (localConfig != null)
        {
            if (tempConfig != null)
            {
                if (tempConfig.version > localConfig.version)
                {
                    UpdateLocalSongList(tempConfig);
                }
                else
                {
                    Debug.Log("dont need update local songList !!!");
                }
            }
        }
        else
        {
            UpdateLocalSongList(tempConfig);
        }
    }

    /// <summary>
    /// 拉取服务器歌单
    /// </summary>
    private void DownLoadSongList()
    {
        DownloadManager.Instance.DownLoadFile(ServerConfig.SongListUrl, m_localSongListTempFilePath);
    }

    /// <summary>
    /// 更新本地歌单
    /// </summary>
    private void UpdateLocalSongList(SongConfig config)
    {
        if (config == null) return;
        FileHelper.SafeCreateDictionary(m_localSongListFilePath);
        File.WriteAllText(m_localSongListFilePath, JsonUtility.ToJson(config));

        Debug.Log("update local songList complete!!!");
    }

    void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
    {
        if (e.Error == null && !e.Cancelled)
        {
            Debug.Log("load songList complete!!!");

            CheckSongListVersion();
        }
        else
        {
            Debug.Log("load songList error!!!");
        }

        if (m_CurrentLoadingSong != null)
        {
            LoadSong(m_CurrentLoadingSong);
        }
    }

    public override void Dispose()
    {
        DownloadManager.Instance.DownloadFileCompleted -= OnDownloadFileCompleted;
    }
}
