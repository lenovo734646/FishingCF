using UnityEngine;
using UnityEngine.EventSystems;
using System;
using XLua;

namespace XLuaExtension
{
    public class UGUIClickLuaBehaviour : BaseLuaBehaviour<UGUIClickLuaBehaviour>,IPointerClickHandler
    {
        private Action<LuaTable, PointerEventData> luaOnPointerClick;
        private Action<PointerEventData> luaOnPointerClick2;

        public static UGUIClickLuaBehaviour Bind(GameObject go, Action<PointerEventData> func)
        {
            UGUIClickLuaBehaviour behaviour = go.AddComponent<UGUIClickLuaBehaviour>();
            behaviour.self = null;
            behaviour.luaOnPointerClick2 += func;
            return behaviour;
        }

        public override void Init()
        {
            self.Get("OnPointerClick", out luaOnPointerClick);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (luaOnPointerClick2 != null)
                luaOnPointerClick2(eventData);
            if (luaOnPointerClick != null)
                luaOnPointerClick(self, eventData);
        }
    }
}

