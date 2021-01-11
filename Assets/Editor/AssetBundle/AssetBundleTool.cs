using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AssetBundleTool
{
    public static string AssetBundle_Output_Path = "StreamingAssets";

    static string GetBuildTargetOutputPath(BuildTarget target)
    {
        if (target == BuildTarget.Android)
            return AssetBundle_Output_Path + "/Android";

        if (target == BuildTarget.iOS)
            return AssetBundle_Output_Path + "/iOS";

        return AssetBundle_Output_Path + "/Win";
    }

    [MenuItem("AssetBundle/Build/Current")]
    static void Build_Current()
    {
#if UNITY_ANDROID
        BuildAllAssetBundles(BuildTarget.Android);
#elif UNITY_IOS
        BuildAllAssetBundles(BuildTarget.iOS);
#else
        BuildAllAssetBundles(BuildTarget.StandaloneWindows);
#endif
    }

    [MenuItem("AssetBundle/Build/Android")]
    static void Build_Android()
    {
        BuildAllAssetBundles(BuildTarget.Android);
    }

    [MenuItem("AssetBundle/Build/iOS")]
    static void Build_iOS()
    {
        BuildAllAssetBundles(BuildTarget.iOS);
    }

    [MenuItem("AssetBundle/Build/Windows")]
    static void Build_Win()
    {
        BuildAllAssetBundles(BuildTarget.StandaloneWindows);
    }

    static void BuildAllAssetBundles(BuildTarget target)
    {
        LuaTool.CopyLuaFilesToBytes();
        LuaTool.SetLuaAssetBundleName();

        StringBuilder sb = new StringBuilder();
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();
        foreach (var abName in abNames)
        {
            var abNameNoHashPostfix = abName.EndsWith(AssetConfig.Bundle_PostFix) ?
                abName.Substring(0, abName.Length - AssetConfig.Bundle_PostFix.Length) : abName;
            var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(abName);

            if (assetPaths != null && assetPaths.Length > 0 && !abName.EndsWith(AssetConfig.Bundle_PostFix))
            {
                throw new Exception("No .bundle postfix for AssetBundle " + abName);
            }

            if (!abNameNoHashPostfix.Equals(AssetConfig.AssetBundle_Build_List_Name) && assetPaths != null && assetPaths.Length > 0)
            {
                sb.Append(abNameNoHashPostfix).Append("\n");
                foreach (var assetPath in assetPaths)
                {
                    sb.Append("\t").Append(assetPath).Append("\n");
                }
            }
        }

        var dir = Path.GetDirectoryName(AssetConfig.AssetBundle_Build_List_Path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllText(AssetConfig.AssetBundle_Build_List_Path, sb.ToString());

        AssetDatabase.Refresh();

        var importer = AssetImporter.GetAtPath(AssetConfig.AssetBundle_Build_List_Path);
        importer.SetAssetBundleNameAndVariant(AssetConfig.AssetBundle_Build_List_Name + AssetConfig.Bundle_PostFix, string.Empty);

        var output_path = GetBuildTargetOutputPath(target);

        if (!Directory.Exists(output_path))
            Directory.CreateDirectory(output_path);

        var manifest = BuildPipeline.BuildAssetBundles(output_path,
            BuildAssetBundleOptions.AppendHashToAssetBundleName |
            BuildAssetBundleOptions.ChunkBasedCompression |
            BuildAssetBundleOptions.DeterministicAssetBundle |
            BuildAssetBundleOptions.StrictMode, target);

        sb.Length = 0;

        string[] abs = manifest.GetAllAssetBundles();
        foreach (var ab in abs)
        {
            var hash = manifest.GetAssetBundleHash(ab).ToString();
            var ab_no_hash = ab.Replace("_" + hash + AssetConfig.Bundle_PostFix, string.Empty);
            var len = new FileInfo(output_path + "/" + ab).Length;
            sb.Append(ab_no_hash).Append("|").Append(hash).Append("|").Append(len).Append("\n");
        }

        var manifestName = Path.GetFileName(output_path);
        var md5 = Util.md5(sb.ToString());
        var newFile = output_path + "/" + AssetConfig.AssetBundleManifest_Name + "_" + md5 + AssetConfig.Bundle_PostFix;
        if (File.Exists(newFile))
            File.Delete(newFile);
        File.Move(output_path + "/" + manifestName, newFile);

        var manifest_len = new FileInfo(newFile).Length;
        var new_sb = new StringBuilder();
        TimeSpan timeSpan = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        new_sb.Append(AssetConfig.Version)
            .Append("#")
            .Append(abs.Length + 1)
            .Append("#")
            .Append(Convert.ToInt64(timeSpan.TotalMilliseconds))
            .Append("\n");
        new_sb.Append(sb);
        new_sb.Append(AssetConfig.AssetBundleManifest_Name)
            .Append("|")
            .Append(md5)
            .Append("|")
            .Append(manifest_len)
            .Append("\n");

        File.WriteAllText(output_path + "/" + AssetConfig.File_List_Name, new_sb.ToString());

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        Debug.Log("done!");
    }
}
