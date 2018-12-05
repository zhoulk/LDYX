//===================================================
//作    者：周连康 
//创建时间：2018-12-05 18:23:46
//备    注：
//===================================================

using System;
using System.ComponentModel;
using System.Net;
using UnityEngine;


public delegate void DownloadFileProgressChangedEventHandler(DownloadProgressChangedEventArgs e);
public delegate void DownloadFileCompleteEventHandler(AsyncCompletedEventArgs e);

public class DownloadManager : Singleton<DownloadManager> {

    private WebClient m_Client;

    public event DownloadFileProgressChangedEventHandler DownloadProgressChanged;
    public event DownloadFileCompleteEventHandler DownloadFileCompleted;

    public DownloadManager()
    {
        m_Client = new WebClient();
        m_Client.DownloadProgressChanged += OnDownLoadProgressChange;
        m_Client.DownloadFileCompleted += OnDownloadFileCompleted;
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="localPath"></param>
    public void DownLoadFile(string url, string localPath)
    {
        Uri uri = new Uri(url);

        if (FileHelper.SafeCreateDictionary(localPath))
        {
            m_Client.DownloadFileAsync(uri, localPath);
        }
    }

    void OnDownLoadProgressChange(object sender, DownloadProgressChangedEventArgs e)
    {
        Debug.Log("loading ... " + e.BytesReceived + " / " + e.TotalBytesToReceive);
        if (DownloadProgressChanged != null)
        {
            DownloadProgressChanged(e);
        }
    }

    void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        if (DownloadFileCompleted != null)
        {
            DownloadFileCompleted(e);
        }
    }

    public override void Dispose()
    {
        m_Client.DownloadProgressChanged -= OnDownLoadProgressChange;
        m_Client.DownloadFileCompleted -= OnDownloadFileCompleted;
    }
}
