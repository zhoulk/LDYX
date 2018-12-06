//===================================================
//作    者：周连康 
//创建时间：2018-12-06 10:11:57
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GuanKaLevel
{
    Easy = 1,
    Mid = 2,
    Hard = 3
}

/// <summary>
/// 关卡丁定义
/// </summary>
public class GuanKa {
    /// <summary>
    /// 关卡Id
    /// </summary>
    public int id;
    /// <summary>
    /// 关卡图片
    /// </summary>
    public string imageUrl;
    /// <summary>
    /// 关卡名称
    /// </summary>
    public string name;
    /// <summary>
    /// 歌曲信息
    /// </summary>
    public Song song;
    /// <summary>
    /// 关卡难度
    /// </summary>
    public GuanKaLevel level;
}
