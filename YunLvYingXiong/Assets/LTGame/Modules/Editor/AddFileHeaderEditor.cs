/*-------------------------------------------------------------------------------
 * 创建者：#AUTHERNAME#
 * 修改者列表：
 * 创建日期：#CREATEDATE#
 * 模块描述：
 * 
 * ------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEditor;

/// <summary>
/// 添加脚本文件头模板信息
/// \Unity\Editor\Data\Resources\ScriptTemplates
/// </summary>
public class AddFileHeaderEditor : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");

        if (index < 0) return;

        string file = path.Substring(index);
        if (file != ".cs" && file != ".js" && file != ".boo") return;
        string fileExtension = file;

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);

        file = file.Replace("#AUTHERNAME#", "huangyechuan");
        file = file.Replace("#CREATEDATE#", System.DateTime.Now.ToString("d"));

        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }
}
