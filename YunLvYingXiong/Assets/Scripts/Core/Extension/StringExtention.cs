//===================================================
//作    者：周连康 
//创建时间：2018-12-05 18:51:25
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class StringExtention {

    #region 路径相关

    /// <summary>
    /// 补全文件名
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string AppendFileName(this string path, string fileName)
    {
        return path + Path.DirectorySeparatorChar + fileName;
    }
    #endregion
}
