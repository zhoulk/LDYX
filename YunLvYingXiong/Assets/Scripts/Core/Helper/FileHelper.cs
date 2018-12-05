//===================================================
//作    者：周连康 
//创建时间：2018-12-05 19:12:24
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class FileHelper {

    /// <summary>
    /// 根据文件路径创建文件目录
    /// </summary>
    /// <param name="filePath"></param>
    public static bool SafeCreateDictionary(string filePath)
    {
        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            DirectoryInfo info = Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            return info.Exists;
        }
        else
        {
            return true;
        }
    }
}
