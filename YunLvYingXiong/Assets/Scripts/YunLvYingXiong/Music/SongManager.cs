//===================================================
//作    者：周连康 
//创建时间：2018-12-05 17:19:59
//备    注：
//===================================================

using System;
using System.ComponentModel;
using System.IO;
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

    public SongManager()
    {
        m_localSongListFilePath = PathConfig.LocalSongPath.AppendFileName("songList.json");
        m_localSongListTempFilePath = PathConfig.LocalTempSongPath.AppendFileName("songList.json");

        DownloadManager.Instance.DownloadFileCompleted += OnDownloadFileCompleted;
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
        Debug.Log("load songList complete!!!");
        if (e.Error == null && !e.Cancelled)
        {
            CheckSongListVersion();
        }
    }

    public override void Dispose()
    {
        DownloadManager.Instance.DownloadFileCompleted -= OnDownloadFileCompleted;
    }
}
