using UnityEngine;
using System;
using XLua;

namespace XLuaExtension
{
    public class ColliderLuaBehaviour : BaseLuaBehaviour<ColliderLuaBehaviour>
    {
        private Action<LuaTable, Collider> luaOnTriggerEnter;
        private Action<LuaTable, Collider> luaOnTriggerStay;

        public override void Init()
        {
            self.Get("OnTriggerEnter", out luaOnTriggerEnter);
            self.Get("OnTriggerStay", out luaOnTriggerStay);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (luaOnTriggerEnter != null)
                luaOnTriggerEnter(self, other);
        }

        private void OnTriggerStay(Collider other)
        {
            if (luaOnTriggerStay != null)
                luaOnTriggerStay(self, other);
        }
    }
}

