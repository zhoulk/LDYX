//===================================================
//作    者：周连康 
//创建时间：2018-12-07 15:01:16
//备    注：
//===================================================

using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEngine;

public delegate void LoadSongComplete(SongClient client, Song song);
public delegate void LoadSongProgress(SongClient client, float progress);

public class SongClient {

    public event LoadSongComplete LoadSongComplete;
    public event LoadSongProgress LoadSongProgress;

    AudioClip m_AudioClip;
    public AudioClip AudioClip
    {
        get
        {
            return m_AudioClip;
        }
    }
    Dictionary<int, KoreographyTrack> m_AudioTrackCache = new Dictionary<int, KoreographyTrack>();
    public Dictionary<int, KoreographyTrack> AudioTracks
    {
        get{
            return m_AudioTrackCache;
        }
    }

    AssetBundleCreateRequest m_AssetCreateRequst;
    AssetBundleRequest m_AssetBundleRequest;
    AssetBundle m_AssetBundle;

    Song m_CurrentLoadingSong;
    Track m_CurrentLoadingTrack;

    /// <summary>
    /// 加载歌曲到内存
    /// </summary>
    /// <param name="song"></param>
    public void LoadSongAsync(Song song)
    {
        m_CurrentLoadingSong = song;
        string songABPath = PathConfig.LocalSongPath.AppendFileName(song.songABUrl);
        Debug.Log(songABPath);
        if (File.Exists(songABPath))
        {
            m_AssetCreateRequst = AssetBundle.LoadFromFileAsync(songABPath, 0);
            m_AssetCreateRequst.completed += OnCreateAssetBundleComplete;
        }
        else
        {
            DownloadManager.Instance.DownLoadFile(ServerConfig.SongBaseUrl + "/" + song.songABUrl, songABPath, OnDownLoadABProgress, OnDownLoadABCompleted);
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
        if (m_AudioClip == null)
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
                    LoadSongComplete(this, m_CurrentLoadingSong);
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

        m_AudioClip = clip;

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

    void OnDownLoadABProgress(object sender, DownloadProgressChangedEventArgs e)
    {

    }

    void OnDownLoadABCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (m_CurrentLoadingSong != null)
        {
            LoadSongAsync(m_CurrentLoadingSong);
        }
    }
}
