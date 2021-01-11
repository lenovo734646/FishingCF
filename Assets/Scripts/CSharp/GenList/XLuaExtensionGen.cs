using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

namespace XLuaExtension
{
    public static class XLuaExtensionGen
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(UpdateLuaBehaviour),
            typeof(UGUIClickLuaBehaviour),
            typeof(DownloadLuaBehaviour),
            typeof(ColliderLuaBehaviour),
            typeof(DestroyLuaBehaviour),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(Action<LuaTable>),
            typeof(Action<LuaTable,bool>),
            typeof(Action<LuaTable,float>),
            typeof(Action<LuaTable,bool,int,string>),
            typeof(Action<LuaTable,Collision>),
            typeof(Action<LuaTable,Collision2D>),
            typeof(Action<LuaTable,Collider>),
            typeof(Action<LuaTable,Collider2D>),
            typeof(Action<LuaTable,Texture2D>),
            typeof(Action<LuaTable,PointerEventData>),
            typeof(Action<Texture2D>),
            typeof(Action<PointerEventData>),
            typeof(Action<byte[]>),
            typeof(UnityEngine.Events.UnityAction<bool>),
            typeof(UnityEngine.Events.UnityAction<float>),
            typeof(UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>),
        };
    }
}

