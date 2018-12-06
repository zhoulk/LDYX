//===================================================
//作    者：周连康 
//创建时间：2018-12-05 17:32:11
//备    注：
//===================================================

using System;
using System.Collections.Generic;


/// <summary>
/// 歌曲定义
/// </summary>
/// 
[Serializable]
public class Song {
    /// <summary>
    /// 歌曲id
    /// </summary>
    public int id;

    /// <summary>
    /// 歌曲名称
    /// </summary>
    public string songName;

    /// <summary>
    /// 歌曲资源地址
    /// </summary>
    public string songABUrl;

    /// <summary>
    /// 歌曲显示名称
    /// </summary>
    public string songTitle;

    /// <summary>
    /// 音轨列表
    /// </summary>
    public List<Track> songTracks;
}
