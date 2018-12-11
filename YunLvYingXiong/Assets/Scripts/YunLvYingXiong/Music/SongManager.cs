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
using System.Net;
using UnityEngine;

public class SongManager : Singleton<SongManager> {

    /// <summary>
    /// 歌单本地路径
    /// </summary>
    private string m_localSongListFilePath;
    /// <summary>
    /// 歌单本地临时路径
    /// </summary>
    private string m_localSongListTempFilePath;

    Dictionary<int, AudioClip> m_AudioClipCache = new Dictionary<int, AudioClip>();
    Dictionary<int, KoreographyTrack> m_AudioTrackCache = new Dictionary<int, KoreographyTrack>();

    public SongManager()
    {
        m_localSongListFilePath = PathConfig.LocalSongPath.AppendFileName("songList.json");
        m_localSongListTempFilePath = PathConfig.LocalTempSongPath.AppendFileName("songList.json");
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
    public void LoadSong(Song song, LoadSongProgress progressHandler, LoadSongComplete completeHandler)
    {
        SongClient client = new SongClient();

        client.LoadSongProgress += progressHandler;
        client.LoadSongComplete += completeHandler;

        client.LoadSongProgress += OnLoadSongProgressHandler;
        client.LoadSongComplete += OnLoadSongCompleteHandler;

        client.LoadSongAsync(song);
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
        }
        catch(Exception e)
        {
            Debug.Log("check update error !!!" + e.Message);
        }
        try
        {
            string localSongListTxt = File.ReadAllText(m_localSongListFilePath);
            localConfig = JsonUtility.FromJson<SongConfig>(localSongListTxt);
        }
        catch (Exception e)
        {
            Debug.Log("check update error !!!" + e.Message);
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
        DownloadManager.Instance.DownLoadFile(ServerConfig.SongListUrl, m_localSongListTempFilePath, OnDownloadSongProgress, OnDownloadSongCompleted);
    }

    /// <summary>
    /// 更新本地歌单
    /// </summary>
    private void UpdateLocalSongList(SongConfig config)
    {
        if (config == null)
        {
            Debug.LogError("update local songList error!!!   SongConfig is null");
            return;
        }
        FileHelper.SafeCreateDictionary(m_localSongListFilePath);
        File.WriteAllText(m_localSongListFilePath, JsonUtility.ToJson(config));

        Debug.Log("update local songList complete!!!");
    }

    void OnDownloadSongProgress(object sender, DownloadProgressChangedEventArgs e)
    {
       
    }

    void OnDownloadSongCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error == null && !e.Cancelled)
        {
            Debug.Log("load songList complete!!!");

            CheckSongListVersion();
        }
        else
        {
            Debug.Log("load songList error!!!" + e.Error);
        }
    }

    void OnLoadSongProgressHandler(SongClient client, float progress)
    {

    }

    void OnLoadSongCompleteHandler(SongClient client, Song song)
    {
        if (m_AudioClipCache.ContainsKey(song.id))
        {
            m_AudioClipCache.Remove(song.id);
        }
        m_AudioClipCache.Add(song.id, client.AudioClip);

        foreach (var track in client.AudioTracks)
        {
            if (m_AudioTrackCache.ContainsKey(track.Key))
            {
                m_AudioTrackCache.Remove(track.Key);
            }
            m_AudioTrackCache.Add(track.Key, track.Value);
        }
    }

    public override void Dispose()
    {
    }
}
