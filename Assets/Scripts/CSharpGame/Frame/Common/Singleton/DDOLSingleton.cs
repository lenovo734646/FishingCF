
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
public abstract class DDOLSingleton<T> : MonoBehaviour where T : DDOLSingleton<T>//Component
{
    public static GameObject go { get; set; }
    private static T _Instance = null;
    public static T Instance
    {
        get
        {
            if (null == _Instance)
            {
                if (null == go)
                {
                    go = GameObject.Find("DDOLGameObject");
                    if (null == go)
                    {
                        go = new GameObject("DDOLGameObject");
                    }
                }
                DontDestroyOnLoad(go);
                _Instance = go.GetOrAddComponent<T>();
            }
            return _Instance;
        }
    }

    public void OnApplicationQuit()
    {
        _Instance = null;
    }

    public virtual void Init() { }
}