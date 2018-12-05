//===================================================
//作    者：周连康 
//创建时间：2018-12-05 17:32:48
//备    注：
//===================================================

using System;
using System.Collections.Generic;

/// <summary>
/// 歌曲列表配置定义
/// </summary>

[Serializable]
public class SongConfig {

    /// <summary>
    /// 版本号
    /// </summary>
    public int version;
    /// <summary>
    /// 歌曲列表
    /// </summary>
    public List<Song> songs;

}
