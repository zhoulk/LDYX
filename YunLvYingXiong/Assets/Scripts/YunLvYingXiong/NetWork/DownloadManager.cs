//===================================================
//作    者：周连康 
//创建时间：2018-12-05 18:23:46
//备    注：
//===================================================

using System;
using System.ComponentModel;
using System.Net;
using UnityEngine;

public class DownloadManager : Singleton<DownloadManager> {

    public DownloadManager()
    {

    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="localPath"></param>
    public void DownLoadFile(string url, string localPath, DownloadProgressChangedEventHandler OnDownLoadProgressChange, AsyncCompletedEventHandler OnDownloadFileCompleted)
    {
        Uri uri = new Uri(url);
        WebClient m_Client = new WebClient();
        m_Client.DownloadProgressChanged += OnDownLoadProgressChange;
        m_Client.DownloadFileCompleted += OnDownloadFileCompleted;

        if (FileHelper.SafeCreateDictionary(localPath))
        {
            m_Client.DownloadFileAsync(uri, localPath);
        }
    }

    public override void Dispose()
    {
        
    }
}
