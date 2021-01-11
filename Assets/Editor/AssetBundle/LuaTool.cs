using UnityEngine;
using UnityEditor;
using System.IO;

public class LuaTool
{
    public static void CopyLuaFilesToBytes()
    {
        var guids = AssetDatabase.FindAssets("", new string[] { AssetConfig.Lua_Output_Path });
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (File.Exists(assetPath))
                File.Delete(assetPath);
        }

        guids = AssetDatabase.FindAssets("", new string[] { AssetConfig.Lua_Src_Path });
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (assetPath.EndsWith(".lua"))
            {
                var newAssetPath = assetPath.Replace(AssetConfig.Lua_Src_Path, AssetConfig.Lua_Output_Path) + ".bytes";
                var dir = Path.GetDirectoryName(newAssetPath);
                Directory.CreateDirectory(dir);
                byte[] buffer = File.ReadAllBytes(assetPath);
                File.WriteAllBytes(newAssetPath, LuaBundleLoader.Encrypt(buffer));
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Copy lua files over");
    }

    public static void SetLuaAssetBundleName()
    {
        var guids = AssetDatabase.FindAssets("", new string[] { AssetConfig.Lua_Output_Path });
        foreach (var guid in guids)
        {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
#if HALL
                importer.SetAssetBundleNameAndVariant(AssetConfig.Lua_Bundle_Name + AssetConfig.Bundle_PostFix, string.Empty);
#else
                importer.SetAssetBundleNameAndVariant(AssetConfig.Game_Name + "/" + AssetConfig.Lua_Bundle_Name + AssetConfig.Bundle_PostFix, string.Empty);
#endif
            }
        }
    }
}