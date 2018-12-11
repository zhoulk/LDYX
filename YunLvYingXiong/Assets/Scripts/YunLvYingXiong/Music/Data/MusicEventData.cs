//===================================================
//作    者：周连康 
//创建时间：2018-12-06 20:45:28
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicOper
{
    Up = 1,
    LongUp = 2,
    Down = 3,
    LongDown = 4
}

public enum MusicPlayer
{
    P1 = 1,
    P2 = 2
}

/// <summary>
/// 音乐事件数据
/// </summary>
public class MusicEventData {
    /// <summary>
    /// 操作
    /// </summary>
    public MusicOper oper;
    /// <summary>
    /// 角色编号
    /// </summary>
    public MusicPlayer player;
    /// <summary>
    /// 歌词
    /// </summary>
    public string content;
    /// <summary>
    /// 取样长度
    /// </summary>
    public float sampleLen;
}
