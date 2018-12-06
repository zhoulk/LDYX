//===================================================
//作    者：周连康 
//创建时间：2018-12-06 14:53:24
//备    注：
//===================================================

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Packager : MonoBehaviour {

    private static string m_SongPath = Application.dataPath + "/Resources/Music";
    private static string m_OutPutPath = Application.dataPath + "/StreamAssets";

    [MenuItem("Packager/Create AssetBunldes Android")]
    static void CreateAssetBunldesMain()
    {
        BuildAssetBundle(BuildTarget.Android);
    }

    static void BuildAssetBundle(BuildTarget target)
    {
        string[] directories = Directory.GetDirectories(m_SongPath);
        foreach (var dir in directories)
        {
            string bundleName = Path.GetFileName(dir);
            string[] buildfiles = Directory.GetFiles(dir);

            List<string> fileNames = new List<string>();
            foreach (var f in buildfiles)
            {
                if (f.EndsWith(".meta")) continue;
                if (f.EndsWith(".DS_Store")) continue;

                string fileName = f.Replace('\\', '/');

                fileName = "Assets/" + fileName.Substring(Application.dataPath.Length + 1);
                fileNames.Add(fileName);
            }
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = bundleName + ".ab";
            build.assetNames = fileNames.ToArray();

            AssetBundleBuild[] maps = new AssetBundleBuild[] { build };

            if (!Directory.Exists(m_OutPutPath))
            {
                Directory.CreateDirectory(m_OutPutPath);
            }
            BuildPipeline.BuildAssetBundles(m_OutPutPath, maps, BuildAssetBundleOptions.None, target);

            AssetDatabase.Refresh();
        }
    }
}
