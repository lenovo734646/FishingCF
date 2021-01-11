using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AppRoot : MonoBehaviour
{
    [SerializeField]
    private UpdateType _updateType = UpdateType.CurrentPlatform;
#if HALL
    [SerializeField]
    private AssetSourceType _hallAssetSource = AssetSourceType.LocalAssets;
#endif
    [SerializeField]
    private AssetSourceType _gameAssetSource = AssetSourceType.LocalAssets;

    private static AppRoot _instance;
    public static AppRoot Get()
    {
        return _instance;
    }

    private static string _assetUrl = string.Empty;
    public string AssetUrl()
    {
        return _assetUrl;
    }

    public bool IsRunInHall()
    {
#if HALL
        return true;
#else
        return false;
#endif
    }

    public void HotUpdate(string version)
    {
        InitContext();
        if (Context.Hall.Config.IsUpdate)
            StartUpdate(version);
        else
            OnABUpdateComplete();
    }

    public void AddHotUpdateListener(Action<ProgressType, int, int> onProgress,
        Action<int, string> onError, Action onComplete)
    {
        this.onProgress = onProgress;
        this.onError = onError;
        this.onComplete = onComplete;
    }

    //private int curGameGroup = 0;
    public void HallToGame(int gameId)
    {
#if HALL
        Debug.Log($"HallToGame id = {gameId}");
        var table = THotUpdateHelper.DataMap.Values.Where(t => t.GameId == gameId).ToList().Last();

        ModuleManager.Instance.Get<LoginModule>().AccessServiceReq(table.GameGroupType,EnumAccessServiceType.JoinGame,()=>
        {
            curGameGroup = table.GameGroupType;

            if (!Context.Hall.Config.IsEditorAssets)
                Context.Hall.BundleMgr.Clear();

            Context.Game.Config.SetPathParam($"{SysDefines.OssUrl}/{SysDefines.ZoneId}/{table.Url}", table.NameEN, false);
            Context.Game.BundleMgr.Init();
            Context.Game.LuaClient.Destroy();
            Context.Game.LuaClient.Init(Context.Game);
        });
#endif
    }

    public void GameToHall()
    {
#if HALL
        Debug.Log("GameToHall");

        ModuleManager.Instance.Get<LoginModule>().AccessServiceReq(curGameGroup, EnumAccessServiceType.QuitGame, () => 
        {
            curGameGroup = 0;

            Context.Game.BundleMgr.Clear();
            Context.Game.LuaClient.ClearSceneDelegate();

            if (!Context.Hall.Config.IsEditorAssets)
                Context.Hall.BundleMgr.Init();

            //if (!Context.Hall.Config.IsEditorAssets)
            //{
            //    var bundleMgr = Context.Hall.BundleMgr;
            //    var bundleName = bundleMgr.GetAssetBundleName("Assets/Scenes/MainScene.unity");
            //    bundleMgr.LoadAssetBundle(bundleName);
            //}
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        });
#endif
    }

    private void Start()
    {
        if (!ReferenceEquals(Get(), null))
        {
            GetComponent<XLuaMain>().enabled = false;
            enabled = false;
            return;
        }
        
        _instance = this;

        //InitContext();

#if HALL
        //HotUpdate();      //临时放此处,应由登录代码控制什么时候启动热更新
#endif
    }

    public void Init()
    {
        InitContext();
    }

    private void InitContext()
    {
#if HALL
        Context.Hall.Config = new AssetConfig();
        Context.Hall.Config.SetAssetType(_updateType, _hallAssetSource);
        //Context.Hall.Config.SetPathParam(_assetUrl + "Hall/", "Hall", true);
        Context.Hall.Config.SetPathParam($"{SysDefines.OssUrl}/{SysDefines.ZoneId}/{SysDefines.HotUpdateUrl}", "Hall", true);
        Context.Hall.Loader = new AssetLoader(Context.Hall);
        Context.Hall.BundleMgr = new BundleManager(Context.Hall);

        if (Context.Hall.LuaClient == null)
            Context.Hall.LuaClient = GetComponent<XLuaMain>();

        Context.Game.Config = new AssetConfig();
        if (_gameAssetSource == AssetSourceType.LocalAssets)
            _gameAssetSource = AssetSourceType.UpdateAssetBundle;   //大厅里启动小游戏只能通过热更新
        Context.Game.Config.SetAssetType(_updateType, _gameAssetSource);
        Context.Game.BundleMgr = new BundleManager(Context.Game);
        if (Context.Game.LuaClient == null)
            Context.Game.LuaClient = gameObject.AddComponent<XLuaMain>();
#else
        Context.Game.Config = new AssetConfig();
        Context.Game.Config.SetAssetType(_updateType, _gameAssetSource);
        Context.Game.Config.SetPathParam(_assetUrl + "Game/", "game", true);
        Context.Game.Loader = new AssetLoader(Context.Game);
        Context.Game.BundleMgr = new BundleManager(Context.Game);
        if (!Context.Game.Config.IsEditorAssets)
            Context.Game.BundleMgr.Init();
        Context.Game.LuaClient = GetComponent<XLuaMain>();
        Context.Game.LuaClient.Init(Context.Game);
#endif
    }

    private void StartUpdate(string version)
    {
        var updater = gameObject.AddComponent<AssetUpdater>();
        updater.updateComplete = OnABUpdateComplete;
        updater.onProgress = OnProgress;
        updater.onError = OnError;
        updater.StartUpdate(Context.Hall.Config, version);
    }

    private Action<ProgressType, int, int> onProgress;
    private Action<int, string> onError;
    private Action onComplete;
    private void OnABUpdateComplete()
    {
        if (!Context.Hall.Config.IsEditorAssets)
            Context.Hall.BundleMgr.Init();
        Context.Hall.LuaClient.Init(Context.Hall);

        onComplete?.Invoke();
    }

    private void OnProgress(ProgressType type, int loaded, int total)
    {
        onProgress?.Invoke(type, loaded, total);
    }

    private void OnError(int errorCode, string error)
    {
        onError?.Invoke(errorCode, error);
    }
}
