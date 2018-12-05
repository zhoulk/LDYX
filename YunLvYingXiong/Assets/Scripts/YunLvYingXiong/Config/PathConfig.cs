//===================================================
//作    者：周连康 
//创建时间：2018-12-05 18:35:52
//备    注：
//===================================================

using System.IO;
using UnityEngine;

public class PathConfig {
    /// <summary>
    /// 本地歌曲列表路径
    /// </summary>
    public static string LocalSongPath = Application.persistentDataPath + Path.DirectorySeparatorChar + "song";
    /// <summary>
    /// 本地歌曲列表临时路径
    /// </summary>
    public static string LocalTempSongPath = Application.temporaryCachePath + Path.DirectorySeparatorChar + "song";
}
