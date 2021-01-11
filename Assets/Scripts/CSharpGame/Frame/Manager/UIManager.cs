
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *         1：管理UI界面的管理类
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// UI窗体信息
    /// </summary>
    struct UIInfoData
    {
        public EnumUIType UIType { get; private set; }
        public string Path { get; private set; }
        public Type ScriptType { get; private set; }
        public object[] UIparams { get; private set; }
        public UIInfoData GetUIInfoData(EnumUIType uiType, string componentType, params object[] uiParams)
        {
            UIType = uiType;
            Path = UIPathDefines.GetPrefabPathByType(uiType, componentType);
            UIparams = uiParams;
            ScriptType = UIPathDefines.GetUIScriptByType(uiType, componentType);
            return this;
        }
    }

    private Dictionary<EnumUIType, GameObject> dicOpenUIs = null;
    private Stack<UIInfoData> stackOpenUIs = null;

    public override void Init()
    {
        dicOpenUIs = new Dictionary<EnumUIType, GameObject>();
        stackOpenUIs = new Stack<UIInfoData>();
    }

    public T GetUI<T>(EnumUIType uiType) where T : BaseUI
    {
        GameObject retObj = GetUIObject(uiType);
        if (null != retObj)
            return retObj.GetComponent<T>();
        return null;
    }

    public GameObject GetUIObject(EnumUIType uiType)
    {
        GameObject retObj = null;
        if (!dicOpenUIs.TryGetValue(uiType, out retObj))
        {
            var msg = string.Format("dicOpenUIs TryGetValue Failure! uiType :{0}", uiType);
            throw new Exception(msg);
        }
        return retObj;
    }

    public void PreloadUI(EnumUIType[] uiTypes, string[] componentType)
    {
        var len = uiTypes.Length;
        if (len != componentType.Length)
        {
            Debug.LogErrorFormat($"uiTypes Length != componentType Length.uiTypes Length{len},componentType Length{componentType.Length}");
            return;
        }
        for (int i = 0; i < len; i++)
        {
            PreloadUI(uiTypes[i], componentType[i]);
        }
    }

    public void PreloadUI(EnumUIType[] uITypes)
    {
        var len = uITypes.Length;
        for (int i = 0; i < len; i++)
        {
            PreloadUI(uITypes[i]);
        }
    }

    /// <summary>
    /// 预加载
    /// </summary>
    /// <param name="uiType">UI类型</param>
    /// <param name="componentType">组件类型</param>
    public void PreloadUI(EnumUIType uiType,string componentType)
    {
        string path = UIPathDefines.GetPrefabPathByType(uiType, componentType);
        ResManager.Instance.LoadAsync<GameObject>(path, null);
    }

    /// <summary>
    /// 预加载
    /// </summary>
    /// <param name="uiType">UI类型</param>
    public void PreloadUI(EnumUIType uiType)
    {
        string path = UIPathDefines.GetPrefabPathByType(uiType, string.Empty);
        ResManager.Instance.LoadPrefab(path);
    }

    public void OpenMessageBoxUI(string content, int countTime = 10, EnumMessageBoxType type = EnumMessageBoxType.OK_CANCEL,
                                 MethodAction btnOK = null, object btnOKParam = null,
                                 MethodAction btnRelease = null, object btnReleaseParam = null,
                                 params object[] uiParams)
    {
        OpenMessageBoxUI(null, content, countTime, type, btnOK, btnOKParam, btnRelease, btnReleaseParam, uiParams);
    }


    //打开两个按钮的弹窗
    public void OpenMessageBoxUI(string title, string content, int countTime = 10, EnumMessageBoxType type = EnumMessageBoxType.OK_CANCEL,
                                 MethodAction btnOK = null, object btnOKParam = null,
                                 MethodAction btnRelease = null,object btnReleaseParam = null,
                                 params object[] uiParams)
    {
        var module = ModuleManager.Instance.Get<MessageBoxModule>();
        module.Title = string.IsNullOrEmpty(title) ? "温馨提示" : title;
        module.Content = content;
        module.CountTime = countTime;
        module.btnOK = btnOK;
        module.btnRelease = btnRelease;
        module.btnOKParam = btnOKParam;
        module.btnReleaseParam = btnReleaseParam;
        module.MessageType = type;
        OpenUI(EnumUIType.MessageBoxUI, uiParams);
    }

    /// <summary>
    /// 打开多个UI面板不关闭已打开的UI面板
    /// </summary>
    /// <param name="uITypes">打开面板的UI类型数组</param>
    public void OpenUI(EnumUIType[] uITypes)
    {
        OpenUI(false, uITypes, string.Empty, null);
    }

    /// <summary>
    /// 打开UI面板不关闭已打开的UI面板
    /// </summary>
    /// <param name="uiType">UI类型</param>
    /// <param name="uiParams">可变参数</param>
    public void OpenUI(EnumUIType uiType, params object[] uiParams)
    {
        EnumUIType[] uiTypes = new EnumUIType[] { uiType };
        OpenUI(false, uiTypes, string.Empty, uiParams);
    }

    /// <summary>
    /// 打开UI面板不关闭已打开的UI面板
    /// </summary>
    /// <param name="uiType">UI类型</param>
    /// <param name="componentType">组件类型</param>
    /// <param name="uiParams">可变参数</param>
    public void OpenUI(EnumUIType uiType, string componentType, params object[] uiParams)
    {
        EnumUIType[] uiTypes = new EnumUIType[] { uiType };
        OpenUI(false, uiTypes, componentType, uiParams);
    }

    /// <summary>
    /// 打开多个UI面板兵关闭其他面板
    /// </summary>
    /// <param name="uiTypes">打开面板的UI类型数组</param>
    public void OpenUICloseOthers(EnumUIType[] uiTypes)
    {
        OpenUI(true, uiTypes, string.Empty, null);
    }

    /// <summary>
    /// 打开多个UI面板兵关闭其他面板
    /// </summary>
    /// <param name="uiType">UI类型</param>
    /// <param name="uiParams">可变参数</param>
    public void OpenUICloseOthers(EnumUIType uiType, params object[] uiParams)
    {
        EnumUIType[] uiTypes = new EnumUIType[] { uiType };
        OpenUI(true, uiTypes, string.Empty, uiParams);
    }

    /// <summary>
    /// 打开多个UI面板兵关闭其他面板
    /// </summary>
    /// <param name="uiType">UI类型</param>
    /// <param name="componentType">组件类型</param>
    /// <param name="uiParams">可变参数</param>
    public void OpenUICloseOthers(EnumUIType uiType, string componentType, params object[] uiParams)
    {
        EnumUIType[] uiTypes = new EnumUIType[] { uiType };
        OpenUI(true, uiTypes, componentType, uiParams);
    }

    /// <summary>
    /// 打开UI面板
    /// </summary>
    /// <param name="isCloseOthers">是否关闭已打开的UI的面板</param>
    /// <param name="uiTypes">UI类型数组</param>
    /// <param name="componentType">组件类型</param>
    /// <param name="uiParams">可变参数</param>
    public void OpenUI(bool isCloseOthers, EnumUIType[] uiTypes, string componentType, params object[] uiParams)
    {
        if (isCloseOthers)
            CloseUIAll();
        for (int i = 0; i < uiTypes.Length; i++)
        {
            EnumUIType uiType = uiTypes[i];
            if (!dicOpenUIs.ContainsKey(uiType))
                stackOpenUIs.Push(new UIInfoData().GetUIInfoData(uiType, componentType, uiParams));
        }
        if (stackOpenUIs.Count > 0)
        {
            CoroutineController.Instance.StartCoroutine(AsyncLoadData());
        }
    }

    private IEnumerator<int> AsyncLoadData()
    {
        UIInfoData uiInfoData;
        UnityEngine.Object prefabObj = null;
        GameObject uiObj = null;
        if (!ReferenceEquals(stackOpenUIs, null) && stackOpenUIs.Count > 0)
        {
            do
            {
                uiInfoData = stackOpenUIs.Pop();
                prefabObj = ResManager.Instance.LoadPrefab(uiInfoData.Path);
                if (!ReferenceEquals(prefabObj, null))
                {
                    uiObj = UnityEngine.Object.Instantiate(prefabObj) as GameObject;
                    BaseUI baseUI = uiObj.GetComponent<BaseUI>();
                    if (ReferenceEquals(baseUI, null))
                        baseUI = uiObj.AddComponent(uiInfoData.ScriptType) as BaseUI;
                    baseUI.SetUIWhenOpening(uiInfoData.UIparams);
                    dicOpenUIs.Add(uiInfoData.UIType, uiObj);
                }
            } while (stackOpenUIs.Count > 0);
        }
        yield return 0;
    }

    public void CloseUIAll()
    {
        List<EnumUIType> listKey = new List<EnumUIType>(dicOpenUIs.Keys);
        for (int i = 0; i < listKey.Count; i++)
        {
            CloseUI(listKey[i]);
        }
        dicOpenUIs.Clear();
    }

    public void CloseUI(EnumUIType[] uiTypes)
    {
        for (int i = 0; i < uiTypes.Length; i++)
        {
            CloseUI(uiTypes[i]);
        }
    }

    public void CloseUI(EnumUIType uiType)
    {
        GameObject uiObj = GetUIObject(uiType);
        if (null == uiObj)
        {
            dicOpenUIs.Remove(uiType);
        }
        else
        {
            BaseUI baseUI = uiObj.GetComponent<BaseUI>();
            if (null == baseUI)
            {
                UnityEngine.Object.Destroy(uiObj);
                dicOpenUIs.Remove(uiType);
            }
            else
            {
                baseUI.StateChanged += CloseUIHandle;
                baseUI.Release();
            }
        }
    }

    public void CloseUIHandle(object sender, EnumObjectState newState, EnumObjectState oldState)
    {
        if (newState == EnumObjectState.Closing)
        {
            BaseUI baseUI = sender as BaseUI;
            dicOpenUIs.Remove(baseUI.GetUIType());
            baseUI.StateChanged -= CloseUIHandle;
        }
    }

    //获得所有的打开的面板
    public Dictionary<EnumUIType, GameObject> GetDicOpenUIs()
    {
        return dicOpenUIs;
    }

    //获取最上层的UI面板
    public EnumUIType GetCurrentUI()
    {
        EnumUIType curUIType = EnumUIType.None;
        if (dicOpenUIs.Count >0 )
            curUIType = dicOpenUIs.Last().Key;
        return curUIType;
    }

    //打开的UI面板中可有此类型的UI
    public bool FindUIByUIType(EnumUIType uiType)
    {
        return dicOpenUIs.ContainsKey(uiType);
    }

    #region HotUpdate
    private Dictionary<string, GameObject> dicOpenUIsLua = new Dictionary<string, GameObject>();
    public void BindLuaUIObject(GameObject uiObj, string name)
    {
        var script = uiObj.GetOrAddComponent<LuaBaseUI>();
        script.UIName = name;
        uiObj.transform.SetParent(GameController.Instance.UIParent, false);
        dicOpenUIsLua.Add(name, uiObj);
    }

    public void CloseLuaUIObject(string name)
    {
        var script = dicOpenUIsLua[name].GetComponent<LuaBaseUI>();
        script.Release();
        dicOpenUIsLua.Remove(name);
    }
    #endregion
}