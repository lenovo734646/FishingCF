using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AssetUpdater : MonoBehaviour
{
    class FileNode
    {
        public string filePath;
        public string hash;
        public int length;
    }

    public Action updateComplete;
    public Action<ProgressType, int, int> onProgress;
    public Action<int, string> onError;

    private readonly List<string> _updateBundles = new List<string>();
    private readonly List<string> _deleteBundles = new List<string>();
    private int _loadBundleIndex = 0;
    private string _loadFileList = null;
    private AssetConfig _config;

    public void StartUpdate(AssetConfig config, string version)
    {
        if (!config.IsUpdate)
        {
            if (updateComplete != null)
                updateComplete();
            return;
        }

        _config = config;

        if (!Directory.Exists(_config.persistentDataPath))
            Directory.CreateDirectory(_config.persistentDataPath);
        Debug.Log(_config.persistentDataPath);

        //测试中方便起见,暂时关闭版本校验
        //var curFileListPath = _config.GetPath(AssetConfig.File_List_Name, false);
        //if (curFileListPath != null)
        //{
        //    var oldText = AssetUtil.ReadFile(curFileListPath);
        //    var data = oldText.Split('|')[0].Split('#');
        //    if (data[0] == version)
        //    {
        //        _loadFileList = oldText;
        //        OnAllLoadDone();
        //        return;
        //    }
        //}

        StartCheckFileList();
    }

    private void StartCheckFileList()
    {
        if (onProgress != null)
            onProgress(ProgressType.CheckFileList, 0, 0);

        StartCoroutine(Download(_config.url + AssetConfig.File_List_Name, null, OnFileListDone, OnError));
    }

    private void OnError(int errCode, string error)
    {
        Debug.LogError(error);
        if (onError != null)
            onError(errCode, error);
    }

    private void OnFileListDone(string text)
    {
        _loadFileList = text;

        Dictionary<string, FileNode> newFileListMap = new Dictionary<string, FileNode>();
        Dictionary<string, FileNode> oldFileListMap = new Dictionary<string, FileNode>();

        TextToBundleMap(_loadFileList, newFileListMap);

        var curFileListPath = _config.GetPath(AssetConfig.File_List_Name, false);
        if (curFileListPath != null)
        {
            var oldText = AssetUtil.ReadFile(curFileListPath);
            if (!string.IsNullOrEmpty(oldText))
            {
                TextToBundleMap(oldText, oldFileListMap);
            }
        }

        _updateBundles.Clear();
        _deleteBundles.Clear();

        foreach (var pair in newFileListMap)
        {
            var fileName = pair.Key;
            var assetPath = StringUtil.Concat(pair.Value.filePath, "_", pair.Value.hash, AssetConfig.Bundle_PostFix);

            FileNode node;
            if (oldFileListMap.TryGetValue(fileName, out node))
            {
                if (!node.hash.Equals(pair.Value.hash) || !node.length.Equals(pair.Value.length) || _config.GetPath(assetPath) == null)
                    AddUpdateBundle(assetPath);
            }
            else
            {
                AddUpdateBundle(assetPath);
            }
        }

        foreach (var pair in oldFileListMap)
        {
            var fileName = pair.Key;
            var assetPath = StringUtil.Concat(pair.Value.filePath, "_", pair.Value.hash, AssetConfig.Bundle_PostFix);

            FileNode node;
            if (newFileListMap.TryGetValue(fileName, out node))
            {
                if (!node.hash.Equals(pair.Value.hash))
                {
                    _deleteBundles.Add(assetPath);
                }
            }
            else
            {
                _deleteBundles.Add(assetPath);
            }
        }

        if (_updateBundles.Count > 0)
        {
            _loadBundleIndex = 0;

            TryLoadNextBundle();
        }
        else
        {
            OnAllLoadDone();
        }
    }

    private void AddUpdateBundle(string path)
    {
        if (_config.GetPath(path, true) == null)
            _updateBundles.Add(path);
    }

    private void TryLoadNextBundle()
    {
        if (onProgress != null)
            onProgress(ProgressType.Download, _loadBundleIndex, _updateBundles.Count);

        if (_loadBundleIndex >= _updateBundles.Count)
        {
            OnAllLoadDone();
            return;
        }

        string bundle = _updateBundles[_loadBundleIndex];
        _loadBundleIndex++;

        Debug.Log(bundle);

        StartCoroutine(Download(_config.url + bundle, bundle, OnBundleDone, OnError));
    }

    private void OnBundleDone(string file)
    {
        TryLoadNextBundle();
    }

    private void TextToBundleMap(string text, Dictionary<string, FileNode> map)
    {
        var lines = text.Split('\n');
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || line.Contains("#"))
                continue;

            var strs = line.Split('|');
            if (strs.Length >= 3)
            {
                int length;
                if (int.TryParse(strs[2], out length))
                {
                    var node = new FileNode();
                    node.filePath = strs[0];
                    node.hash = strs[1];
                    node.length = length;
                    map.Add(node.filePath, node);
                }
            }
        }
    }

    private void OnAllLoadDone()
    {
        foreach (var bundle in _deleteBundles)
        {
            var path = StringUtil.Concat(_config.persistentDataPath, bundle);
            if (File.Exists(path))
                File.Delete(path);
        }

        File.WriteAllText(StringUtil.Concat(_config.persistentDataPath, AssetConfig.File_List_Name), _loadFileList);

        if (updateComplete != null)
            updateComplete();
    }

    private IEnumerator Download(string url, string savePath, Action<string> onDone, Action<int, string> onError)
    {
        var request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || !string.IsNullOrEmpty(request.error))
        {
            if (onError != null)
                onError(1, StringUtil.Concat(url, "\n", request.error));
        }
        else if (request.responseCode != 200)
        {
            if (onError != null)
                onError(2, StringUtil.Concat(url, "\nresponseCode:", request.responseCode.ToString()));
        }
        else
        {
            if (!string.IsNullOrEmpty(savePath))
            {
                string path = _config.persistentDataPath + savePath;
                string dir = Path.GetDirectoryName(path);
                if (dir != null && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllBytes(path, request.downloadHandler.data);

                if (onDone != null)
                    onDone(savePath);
            }
            else
            {
                if (onDone != null)
                    onDone(request.downloadHandler.text);
            }
        }

        request.Dispose();
    }
}

public enum ProgressType
{
    CheckFileList,
    Download,
}
