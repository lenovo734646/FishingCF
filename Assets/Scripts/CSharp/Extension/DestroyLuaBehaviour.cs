using System;
using XLua;

namespace XLuaExtension
{
    [LuaCallCSharp]
    public class DestroyLuaBehaviour : BaseLuaBehaviour<DestroyLuaBehaviour>
    {
        private Action<LuaTable> luaDestroy;

        public override void Init()
        {
            self.Get("OnDestroy", out luaDestroy);
        }

        private void OnDestroy()
        {
            luaDestroy?.Invoke(self);
        }
    }
}


