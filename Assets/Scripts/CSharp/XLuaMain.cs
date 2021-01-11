using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using XLua;
using XLua.LuaDLL;

public class XLuaMain : MonoBehaviour
{
    [Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

    internal const float GCInterval = 1;    //second
    private float lastGCTime = 0;
    public LuaEnv luaEnv;

    [SerializeField]
    private string luaBoot = "Main";
    [SerializeField]
    private string luaAssetPath = "Assets/Scripts/Lua/";
    [SerializeField]
    private Injection[] injections;

    private bool _inited = false;

    public bool InitOnAwake = false;

    private Action _luaUpdate;
    private Action _luaFixedUpdate;
    private Action _luaLateUpdate;
    private UnityAction<Scene, LoadSceneMode> _luaSceneLoaded;
    private UnityAction<Scene> _luaSceneUnloaded;
    private UnityAction<Scene, Scene> _luaActiveSceneChanged;
    private Action<NetBinaryReaderProxy> _luaReceiveNetData;
    private Action<Message> _luaReceiveCSharpData;


    private void Awake()
    {
        if (InitOnAwake)
            Init(null);
    }

    public void SetLuaParam(string boot, string assetPath)
    {
        luaBoot = boot;
        luaAssetPath = assetPath;
    }

    public void Init(Context context)
    {
        if (_inited)
            return;

        _inited = true;
        
        Context.Game.Loader = new AssetLoader(context);

        luaEnv = new LuaEnv();
        luaEnv.AddBuildin("rapidjson", Lua.LoadRapidJson);
        luaEnv.AddBuildin("lpeg", Lua.LoadLpeg);

        luaEnv.AddBuildin("protobuf.c", XLua.LuaDLL.Lua.LoadProtobufC);

        if (context == null || context.Config.IsEditorAssets)
        {
            luaEnv.AddLoader(new LuaFileLoader(luaAssetPath).LoadFile);
        }
        else
        {
            luaEnv.AddLoader(new LuaBundleLoader(AssetConfig.Lua_Output_Path,context).LoadFile);
        }

        if (injections != null)
        {
            foreach (var injection in injections)
            {
                luaEnv.Global.Set(injection.name, injection.value);
            }
        }
        luaEnv.DoString("require '" + luaBoot + "'", "XLuaMain");

        luaEnv.Global.Get("Update", out _luaUpdate);
        luaEnv.Global.Get("FixedUpdate", out _luaFixedUpdate);
        luaEnv.Global.Get("LateUpdate", out _luaLateUpdate);
        luaEnv.Global.Get("OnSceneLoaded", out _luaSceneLoaded);
        luaEnv.Global.Get("OnSceneUnloaded", out _luaSceneUnloaded);
        luaEnv.Global.Get("OnActiveSceneChanged", out _luaActiveSceneChanged);
        luaEnv.Global.Get("OnReceiveNetData", out _luaReceiveNetData);
        luaEnv.Global.Get("OnReceiveCSharpData", out _luaReceiveCSharpData);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;

        MessageCenter.Instance.AddListener(MsgType.NET_RECEIVE_DATA, OnReceiveNetData);
        MessageCenter.Instance.AddListener(MsgType.CSHARP_RECEIVE_DATA, OnReceiveCSharpData);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_luaSceneLoaded != null)
            _luaSceneLoaded(scene, mode);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (_luaSceneUnloaded != null)
            _luaSceneUnloaded(scene);
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        if (_luaActiveSceneChanged != null)
            _luaActiveSceneChanged(previousScene, newScene);
    }

    private void OnReceiveNetData(Message msg)
    {
        if (_luaReceiveNetData != null)
        {
            var br = new NetBinaryReaderProxy(msg.Content as System.IO.BinaryReader);
            _luaReceiveNetData.Invoke(br);
        }
    }

    private void OnReceiveCSharpData(Message msg)
    {
        if (_luaReceiveCSharpData != null)
        {
            _luaReceiveCSharpData.Invoke(msg);
        }
    }

    private void Update()
    {
        if (luaEnv == null)
            return;

        if (_luaUpdate != null)
            _luaUpdate();

        if (Time.time - lastGCTime > GCInterval)
        {
            luaEnv.Tick();
            lastGCTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        if (luaEnv == null)
            return;

        if (_luaFixedUpdate != null)
            _luaFixedUpdate();
    }

    private void LateUpdate()
    {
        if (luaEnv == null)
            return;

        if (_luaLateUpdate != null)
            _luaLateUpdate();
    }

    public void ClearSceneDelegate()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    public void Destroy()
    {
        MessageCenter.Instance.RemoveListener(MsgType.NET_RECEIVE_DATA, OnReceiveNetData);

        //luaBoot = null;
        //luaAssetPath = null;
        injections = null;
        _luaUpdate = null;
        _luaFixedUpdate = null;
        _luaLateUpdate = null;
        _luaSceneLoaded = null;
        _luaSceneUnloaded = null;
        _luaActiveSceneChanged = null;
        _luaReceiveNetData = null;

        if (luaEnv != null)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
        
        _inited = false;
    }
}
