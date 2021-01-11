using UnityEngine;
using System.Collections.Generic;

public class BundleManager
{
    private readonly Context _context;
    private AssetBundleManifest _manifest;
    private readonly Dictionary<string, string> _assetPath_to_bundleName = new Dictionary<string, string>();
    private readonly Dictionary<string, string> _bundleName_to_hashName = new Dictionary<string, string>();
    private readonly Dictionary<string, AssetBundle> _bundleCache = new Dictionary<string, AssetBundle>();

    public BundleManager(Context context)
    {
        _context = context;
    }

    public void Init()
    {
        Clear();
        LoadABFileList();
        LoadManifest();
        LoadAssetsMap();
    }

    private void LoadABFileList()
    {
        var path = _context.Config.GetPath(AssetConfig.File_List_Name, false);
        if (path == null)
        {
            Debug.LogError("No ab_file_list!");
            return;
        }

        var text = AssetUtil.ReadFile(path);
        if (string.IsNullOrEmpty(text))
        {
            Debug.LogError("ab_file_list read error!");
            return;
        }

        var lines = text.Split('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || line.Contains("#"))
                continue;

            var strs = line.Split('|');
            if (strs.Length >= 3)
            {
                var bundleName = strs[0];
                var hashName = StringUtil.Concat(strs[0], "_", strs[1], AssetConfig.Bundle_PostFix);
                if (_bundleName_to_hashName.ContainsKey(bundleName))
                {
                    _bundleName_to_hashName[bundleName] = hashName;
                }
                else
                {
                    _bundleName_to_hashName.Add(bundleName, hashName);
                }
            }
        }
    }

    private void LoadManifest()
    {
        string hashName = GetAssetBundleHashName(AssetConfig.AssetBundleManifest_Name);
        if (string.IsNullOrEmpty(hashName))
        {
            Debug.LogError("No AssetBundleManifest in file list");
            return;
        }

        var fullPath = _context.Config.GetPath(hashName);
        if (string.IsNullOrEmpty(fullPath))
        {
            Debug.LogError("No AssetBundleManifest path");
            return;
        }

        AssetBundle bundle = AssetBundle.LoadFromFile(fullPath);
        if (bundle == null)
        {
            Debug.LogError("AssetBundleManifest bundle load error");
            return;
        }

        _manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        if (_manifest == null)
        {
            Debug.LogError("AssetBundleManifest load error");
            bundle.Unload(true);
            return;
        }

        bundle.Unload(false);
    }

    private void LoadAssetsMap()
    {
        string hashName = GetAssetBundleHashName(AssetConfig.AssetBundle_Build_List_Name);
        if (string.IsNullOrEmpty(hashName))
        {
            Debug.LogError("No AssetBundle_Build_List in file list");
            return;
        }

        var fullPath = _context.Config.GetPath(hashName);
        if (string.IsNullOrEmpty(fullPath))
        {
            Debug.LogError("No AssetBundle_Build_List path");
            return;
        }

        AssetBundle bundle = AssetBundle.LoadFromFile(fullPath);
        if (bundle == null)
        {
            Debug.LogError("AssetBundle_Build_List bundle load error");
            return;
        }

        var textAsset = bundle.LoadAsset<TextAsset>(AssetConfig.AssetBundle_Build_List_Path);
        if (textAsset == null)
        {
            Debug.LogError("AssetBundle_Build_List load error");
            bundle.Unload(true);
            return;
        }

        var lines = textAsset.text.Split('\n');
        bundle.Unload(true);
        string bundleName = null;
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
                continue;

            if (line.StartsWith("\t"))
            {
                if (bundleName != null)
                {
                    var assetPath = line.Substring(1);

                    if (_assetPath_to_bundleName.ContainsKey(assetPath))
                    {
                        _assetPath_to_bundleName[assetPath] = bundleName;
                    }
                    else
                    {
                        _assetPath_to_bundleName.Add(assetPath, bundleName);
                    }
                }
            }
            else
            {
                bundleName = line;
            }
        }
    }

    public string GetAssetBundleHashName(string bundleName)
    {
        string hashName = null;
        _bundleName_to_hashName.TryGetValue(bundleName, out hashName);
        return hashName;
    }

    public string GetAssetBundleName(string assetPath)
    {
        string bundleName = null;
        _assetPath_to_bundleName.TryGetValue(assetPath, out bundleName);
        return bundleName;
    }

    public AssetBundle LoadAssetBundle(string bundleName)
    {
        if (string.IsNullOrEmpty(bundleName))
            return null;

        var hashName = GetAssetBundleHashName(bundleName);
        if (string.IsNullOrEmpty(hashName))
            return null;

        return LoadAssetBundleByHashName(hashName);
    }

    public AssetBundle LoadAssetBundleByHashName(string hashName)
    {
        if (_manifest != null)
        {
            string[] dependencies = _manifest.GetAllDependencies(hashName);
            foreach (var dependency in dependencies)
            {
                _LoadAssetBundleByHashName(dependency);
            }
        }

        return _LoadAssetBundleByHashName(hashName);
    }

    private AssetBundle _LoadAssetBundleByHashName(string hashName)
    {
        if (string.IsNullOrEmpty(hashName))
            return null;

        AssetBundle assetBundle = null;
        if (_bundleCache.TryGetValue(hashName, out assetBundle))
            return assetBundle;

        var fullPath = _context.Config.GetPath(hashName);
        if (string.IsNullOrEmpty(fullPath))
            return null;

        assetBundle = AssetBundle.LoadFromFile(fullPath);
        if (assetBundle == null)
            return null;

        _bundleCache.Add(hashName, assetBundle);
        return assetBundle;
    }

    public AssetBundle GetCachedAssetBundle(string bundleName)
    {
        var hashName = GetAssetBundleHashName(bundleName);
        if (string.IsNullOrEmpty(hashName))
            return null;

        AssetBundle assetBundle = null;
        if (_bundleCache.TryGetValue(hashName, out assetBundle))
            return assetBundle;

        return null;
    }

    public void UnloadAssetBundle(string bundleName)
    {
        var hashName = GetAssetBundleHashName(bundleName);
        if (string.IsNullOrEmpty(hashName))
            return;

        AssetBundle assetBundle = null;
        if (_bundleCache.TryGetValue(hashName, out assetBundle))
        {
            assetBundle.Unload(false);
            _bundleCache.Remove(hashName);
        }
    }

    public void Clear()
    {
        if (_manifest != null)
            Resources.UnloadAsset(_manifest);
        _manifest = null;

        _assetPath_to_bundleName.Clear();
        _bundleName_to_hashName.Clear();
        foreach (var pair in _bundleCache)
        {
            var bundle = pair.Value;
            if (bundle != null)
            {
                bundle.Unload(false);
            }
        }
        _bundleCache.Clear();
    }
}
