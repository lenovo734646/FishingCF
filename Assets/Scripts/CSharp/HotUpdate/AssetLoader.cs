using UnityEngine;
using System;
using XLua;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

[LuaCallCSharp]
public class AssetLoader
{
    private readonly Context _context;

    public AssetLoader(Context context)
    {
        _context = context;
    }

    public T LoadAsset<T>(string assetPath, bool logError = true) where T : Object
    {
        return (T)LoadAsset(assetPath, typeof(T), logError);
    }

    public static Object LoadEditorAsset(string assetPath, Type type, bool logError)
    {
#if UNITY_EDITOR
        var importer = AssetImporter.GetAtPath(assetPath);
        if (importer == null)
        {
            if (logError)
                Debug.LogError("No Asset:" + assetPath);
            return null;
        }

        if (string.IsNullOrEmpty(importer.assetBundleName))
        {
            if (logError)
                Debug.LogError("No AssetBundle:" + assetPath);
        }

        return AssetDatabase.LoadAssetAtPath(assetPath, type);
#else
        Debug.LogError("LoadAsset By LocalAssets is invalid in Player mode");
        return null;
#endif
    }

    public Object LoadAsset(string assetPath, Type type, bool logError = false)
    {
        if (string.IsNullOrEmpty(assetPath))
            return null;

        if (_context.Config.IsEditorAssets)
        {
            return LoadEditorAsset(assetPath, type, logError);
        }
        else
        {
            var bundleMgr = _context.BundleMgr;
            var bundleName = bundleMgr.GetAssetBundleName(assetPath);
            if (string.IsNullOrEmpty(bundleName))
            {
                if (logError)
                    Debug.LogError("No bundleName:" + assetPath);
                return null;
            }

            var bundle = bundleMgr.LoadAssetBundle(bundleName);
            if (bundle == null)
            {
                if (logError)
                    Debug.LogError("No bundle:" + bundleName);
                return null;
            }

            return bundle.LoadAsset(assetPath, type);
        }
    }
}
