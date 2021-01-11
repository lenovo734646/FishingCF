using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetConfig
{
    public const string Game_Name = "AmazingFish";
    public const string Version = "1.0.0.0";
    public const string Bundle_PostFix = ".bundle";
    public const string Lua_Src_Path = "Assets/Scripts/Lua";
    public const string Lua_Output_Path = "Assets/Lua";
    public const string Lua_Bundle_Name = "Lua";
    public const string AssetBundle_Build_List_Path = "Assets/Editor/BundleToAssetsMap.txt";
    public const string AssetBundle_Build_List_Name = "bundle_to_asset_map";
    public const string AssetBundleManifest_Name = "assetbundle_manifest";
    public const string File_List_Name = "ab_file_list.ftxt";

    private AssetSourceType _assetSource = AssetSourceType.LocalAssets;
    private UpdateType _updatePlatform = UpdateType.CurrentPlatform;

#if UNITY_ANDROID && !UNITY_EDITOR
    private string[] _allAssets = null;
    private string streamingAssetsPath_Unity;
#endif

    public string url { get; private set; }
    public string streamingAssetsPath { get; private set; }
    public string persistentDataPath { get; private set; }
    public bool IsUpdate { get { return _assetSource == AssetSourceType.UpdateAssetBundle; } }
    public bool IsEditorAssets { get { return _assetSource == AssetSourceType.LocalAssets; } }

    public AssetConfig Clone()
    {
        var ret = new AssetConfig();
        ret._assetSource = _assetSource;
        ret._updatePlatform = _updatePlatform;
        ret.url = url;
        ret.streamingAssetsPath = streamingAssetsPath;
        ret.persistentDataPath = persistentDataPath;
        return ret;
    }

    public void SetAssetType(UpdateType type, AssetSourceType assetSource)
    {
        _assetSource = assetSource;
#if !UNITY_EDITOR
        if (_assetSource == AssetSourceType.LocalAssets)
            _assetSource = AssetSourceType.UpdateAssetBundle;
#endif
        _updatePlatform = type;
    }

    public string GetPlatform()
    {
#if UNITY_EDITOR
        if (_updatePlatform == UpdateType.CurrentPlatform)
        {
#if UNITY_ANDROID
            return "Android";
#elif UNITY_IOS
            return "iOS";
#else
            return "Win";
#endif
        }
        else if (_updatePlatform == UpdateType.Android)
        {
            return "Android";
        }
        else if (_updatePlatform == UpdateType.iOS)
        {
            return "iOS";
        }
        else
        {
            return "Win";
        }
#elif UNITY_ANDROID
        return "Android";
#elif UNITY_IOS
        return "iOS";
#else
        return "Win";
#endif
    }

    public void SetPathParam(string url_origin, string dataDir, bool isMainProject)
    {
        if (string.IsNullOrEmpty(dataDir))
        {
            dataDir = string.Empty;
        }
        else
        {
            if (!dataDir.StartsWith("/"))
                dataDir = "/" + dataDir;
            if (dataDir.EndsWith("/"))
                dataDir = dataDir.Substring(0, dataDir.Length - 1);
        }
        persistentDataPath = StringUtil.Concat(Application.persistentDataPath, dataDir, "/");

#if UNITY_EDITOR
        if (_updatePlatform == UpdateType.CurrentPlatform)
        {
#if UNITY_ANDROID
            url = url_origin + "Android/";
            streamingAssetsPath = isMainProject ? Application.dataPath + "/../StreamingAssets/Android/" :
                Application.dataPath + "/../StreamingAssets" + dataDir + "/Android/";
#elif UNITY_IOS
            url = url_origin + "iOS/";
            streamingAssetsPath = isMainProject ? Application.dataPath + "/../StreamingAssets/iOS/" :
                Application.dataPath + "/../StreamingAssets" + dataDir + "/iOS/";
#else
            url = url_origin + "Win/";
            streamingAssetsPath = isMainProject ? Application.dataPath + "/../StreamingAssets/Win/" :
                Application.dataPath + "/../StreamingAssets" + dataDir + "/Win/";
#endif
        }
        else if (_updatePlatform == UpdateType.Android)
        {
            url = url_origin + "Android/";
            streamingAssetsPath = isMainProject ? Application.dataPath + "/../StreamingAssets/Android/" :
                Application.dataPath + "/../StreamingAssets" + dataDir + "/Android/";
        }
        else if (_updatePlatform == UpdateType.iOS)
        {
            url = url_origin + "iOS/";
            streamingAssetsPath = isMainProject ? Application.dataPath + "/../StreamingAssets/iOS/" :
                Application.dataPath + "/../StreamingAssets" + dataDir + "/iOS/";
        }
        else
        {
            url = url_origin + "Win/";
            streamingAssetsPath = isMainProject ? Application.dataPath + "/../StreamingAssets/Win/" :
                Application.dataPath + "/../StreamingAssets" + dataDir + "/Win/";
        }
#elif UNITY_ANDROID
        url = url_origin + "Android/";
        streamingAssetsPath = isMainProject ? Application.streamingAssetsPath + "/" :
            Application.streamingAssetsPath + dataDir + "/";
        streamingAssetsPath_Unity = isMainProject ? Application.dataPath + "!assets/" :
            Application.dataPath + "!assets" + dataDir + "/";
#elif UNITY_IOS
        url = url_origin + "iOS/";
        streamingAssetsPath = isMainProject ? Application.streamingAssetsPath + "/" :
            Application.streamingAssetsPath + dataDir + "/";
#else
        url = url_origin + "Win/";
        streamingAssetsPath = isMainProject ? Application.streamingAssetsPath + "/" :
            Application.streamingAssetsPath + dataDir + "/";
#endif
    }

    public string StreamingAssetsPath(bool readByUnityAPI)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (readByUnityAPI)
            return streamingAssetsPath_Unity;
        return streamingAssetsPath;
#else
        return streamingAssetsPath;
#endif
    }

    public string GetPath(string asset, bool readByUnityAPI = true)
    {
        if (IsEditorAssets)
            return null;

        string path = null;

        if (IsUpdate)
        {
            path = StringUtil.Concat(persistentDataPath, asset);
            if (File.Exists(path))
                return path;
        }

        if (IsFileExistInStreamingAssets(asset))
        {
            path = StreamingAssetsPath(readByUnityAPI) + asset;
            return path;
        }

        return null;
    }

    public bool IsFileExistInStreamingAssets(string assetPath)
    {
        if (streamingAssetsPath == null)
            return false;

#if UNITY_ANDROID && !UNITY_EDITOR
        if (_allAssets == null)
        {
            string ret = AssetUtil.ReadFile(streamingAssetsPath + File_List_Name);
            if (ret != null)
            {
                var list = new List<string>();
                list.Add(File_List_Name);
                string[] lines = ret.Split('\n');
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    string[] s = line.Split('|');
                    if (s != null && s.Length > 2)
                    {
                        list.Add(StringUtil.Concat(s[0], "_", s[1], Bundle_PostFix));
                    }
                }
                _allAssets = list.ToArray();
            }
            else
            {
                _allAssets = new string[0];
            }
        }

        if (_allAssets != null)
        {
            foreach (var asset in _allAssets)
            {
                if (asset.Equals(assetPath))
                    return true;
            }
        }
#else
        string path = StreamingAssetsPath(true) + assetPath;

        if (File.Exists(path))
            return true;
#endif
        return false;
    }
}

public enum AssetSourceType
{
    LocalAssets,
    LocalAssetBundle,
    UpdateAssetBundle,
}

public enum UpdateType
{
    CurrentPlatform,
    Android,
    iOS,
    Win,
}
