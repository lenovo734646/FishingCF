
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

using System;
using System.Collections;
using UnityEngine;

public class AssetInfo<T> where T: UnityEngine.Object
{
    private T loadObj;
    public string Path { get; set; }
    public int RefCount { get; set; }

    public bool IsLoaded
    {
        get
        {
            return !ReferenceEquals(loadObj, null);
        }
    }
    public T AssetObject
    {
        get
        {
            if (ReferenceEquals(loadObj, null))
                resourcesLoad();
            return loadObj;
        }
    }

    #region public function
    //协程加载
    public IEnumerator GetObjectByCoroutine(Action<T> loaded)
    {
        while (ReferenceEquals(loadObj, null))
        {
            yield return null;
            resourcesLoad();
        }
        loaded?.Invoke(loadObj);
    }

    //异步加载
    public IEnumerator GetObjectAsync(Action<T> loaded)
    {
        return GetObjectAsync(loaded, null);
    }

    public IEnumerator GetObjectAsync(Action<T> loaded, Action<float> progress)
    {
        if (!ReferenceEquals(loadObj, null))
        {
            loaded?.Invoke(loadObj);
            yield break;
        }
        var request = Resources.LoadAsync(Path);
        if (!ReferenceEquals(progress, null))
        {
            while (!request.isDone)
            {
                progress(request.progress);
            }
        }
        yield return request;
        if (ReferenceEquals(request.asset, null))
            Debug.LogErrorFormat($"Resources Load Failure! Path:{Path}");
        else
        {
            loadObj = request.asset as T;
            loaded?.Invoke(loadObj);
            yield return request;
        }
    }
    #endregion

    #region private function
    private void resourcesLoad()
    {
        try
        {
            loadObj = Resources.Load<T>(Path);
            if (ReferenceEquals(loadObj, null))
                Debug.LogErrorFormat($"Resources Load Failure! Path:{Path}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    #endregion
}


public class ResManager : Singleton<ResManager>
{
    //资源缓存集合
    private Hashtable hashTable;

    //初始化
    public override void Init()
    {
        hashTable = new Hashtable();
    }

    #region public function

    public bool HasCache<T>(string path)where T:UnityEngine.Object
    {
        string hashKey = string.Format($"{path}{typeof(T)}");
        return hashTable.ContainsKey(hashKey);
    }

    //load
    public GameObject LoadPrefab(string path)
    {
        return Load<GameObject>(path);
    }

    public UnityEngine.UI.Image LoadImage(string path)
    {
        return Load<UnityEngine.UI.Image>(path);
    }

    public Font LoadFont(string path)
    {
        return Load<Font>(path);
    }

    public T Load<T>(string path) where T : UnityEngine.Object
    {
        AssetInfo<T> info = getAssetInfo<T>(path);
        if (!ReferenceEquals(info, null))
            return info.AssetObject;
        return default(T);
    }

    //Instance
    public T LoadInstance<T>(string path) where T : UnityEngine.Object
    {
        var obj = Load<T>(path);
        return Instantiate(obj);
    }

    //Instance & Coroutine
    public void LoadInstanceCoroutine(string path, Action<GameObject> loaded)
    {
        LoadInstanceCoroutine<GameObject>(path, loaded);
    }

    public void LoadInstanceCoroutine<T>(string path, Action<T> loaded) where T : UnityEngine.Object
    {
        LoadCoroutine<T>(path, obj => { Instantiate<T>(obj, loaded); });
    }

    public void LoadCoroutine<T>(string path, Action<T> loaded) where T : UnityEngine.Object
    {
        var info = getAssetInfo(path, loaded);
        if (!ReferenceEquals(info, null))
            CoroutineController.Instance.StartCoroutine(info.GetObjectByCoroutine(loaded));
    }

    //Instance & Async
    public void LoadInstanceAsync(string path, Action<GameObject> loaded)
    {
        LoadInstanceAsync<GameObject>(path, loaded);
    }

    public void LoadInstanceAsync<T>(string path, Action<T> loaded) where T : UnityEngine.Object
    {
        LoadAsync<T>(path, obj => { Instantiate<T>(obj, loaded); });
    }

    public void LoadInstanceAsync(string path, Action<GameObject> loaded, Action<float> progress)
    {
        LoadInstanceAsync<GameObject>(path, loaded, progress);
    }

    public void LoadInstanceAsync<T>(string path, Action<T> loaded, Action<float> progress) where T : UnityEngine.Object
    {
        LoadAsync<T>(path, obj => { Instantiate<T>(obj, loaded); }, progress);
    }

    public void LoadAsync<T>(string path, Action<T> loaded) where T : UnityEngine.Object
    {
        LoadAsync<T>(path, loaded, null);
    }

    public void LoadAsync<T>(string path, Action<T> loaded, Action<float> progress) where T : UnityEngine.Object
    {
        var info = getAssetInfo<T>(path, loaded);
        if (!ReferenceEquals(info, null))
            CoroutineController.Instance.StartCoroutine(info.GetObjectAsync(loaded, progress));
    }

    //释放资源
    public void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }
    #endregion

    #region private function

    private AssetInfo<T> getAssetInfo<T>(string path) where T : UnityEngine.Object
    {
        return getAssetInfo<T>(path, null);
    }
    private AssetInfo<T> getAssetInfo<T>(string path, Action<T> loaded) where T : UnityEngine.Object
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("Error: null path name.");
            loaded?.Invoke(null);
        }
        else
        {
            AssetInfo<T> info = null;
            string hashKey = string.Format($"{path}{typeof(T)}");
            if (!hashTable.ContainsKey(hashKey))
            {
                info = new AssetInfo<T>();
                info.Path = path;
                hashTable.Add(hashKey, info);
            }
            else
                info = hashTable[hashKey] as AssetInfo<T>;
            info.RefCount++;
            return info;
        }
        return null;
    }

    private T Instantiate<T>(T obj) where T : UnityEngine.Object
    {
        return Instantiate<T>(obj, null);
    }
    private T Instantiate<T>(T obj, Action<T> loaded) where T : UnityEngine.Object
    {
        T retObj = default(T);
        if (!ReferenceEquals(obj, null))
        {
            retObj = MonoBehaviour.Instantiate(obj);
            if (!ReferenceEquals(retObj, null))
                loaded?.Invoke(retObj);
            else
                Debug.LogError("Error: null Instantiate retObj.");
        }
        else
            Debug.LogError("Error: null Resources Load return obj.");
        return retObj;
    }
    #endregion
}