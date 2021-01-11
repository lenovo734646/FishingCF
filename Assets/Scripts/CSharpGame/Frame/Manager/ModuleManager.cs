
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
using System.Collections.Generic;

public class ModuleManager : Singleton<ModuleManager>
{
    private Dictionary<string, BaseModule> dicModules = null;

    public override void Init() => dicModules = new Dictionary<string, BaseModule>();

    public BaseModule Get(string key)
    {
        return dicModules.ContainsKey(key) ? dicModules[key] : null;
    }

    public T Get<T>() where T : BaseModule
    {
        Type t = typeof(T);
        return dicModules.ContainsKey(t.ToString()) ? dicModules[t.ToString()] as T : null;
    }

    public void RegisterAllModules()
    {
        LoadModule(typeof(LoginModule));
        LoadModule(typeof(MessageBoxModule));
//        LoadModule(typeof(FishingRoomModule));
//        LoadModule(typeof(MainModule));
//        LoadModule(typeof(CommonModule));
//        LoadModule(typeof(FishingMainModule));
//        LoadModule(typeof(ThirdDataModule));
    }

    private void LoadModule(Type moduleType)
    {
        BaseModule bm = Activator.CreateInstance(moduleType) as BaseModule;
        bm.Load();
    }

    public void Register(BaseModule module)
    {
        Type t = module.GetType();
        Register(t.ToString(), module);
    }

    public void Register(string key, BaseModule module)
    {
        if (!dicModules.ContainsKey(key))
            dicModules.Add(key, module);
    }

    public void UnRegisterAll()
    {
        List<string> keyList = new List<string>(dicModules.Keys);
        for (int i = 0; i < keyList.Count; i++)
        {
            UnRegister(keyList[i]);
        }
        dicModules.Clear();
    }

    public void UnRegister(BaseModule module)
    {
        Type t = module.GetType();
        UnRegister(t.ToString());
    }
    public void UnRegister(string key)
    {
        if (dicModules.ContainsKey(key))
        {
            BaseModule module = dicModules[key];
            module.Release();
            dicModules.Remove(key);
            module = null;
        }
    }
}