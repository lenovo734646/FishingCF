using System;
using XLua;

namespace XLuaExtension
{
    [LuaCallCSharp]
    public class UpdateLuaBehaviour : BaseLuaBehaviour<UpdateLuaBehaviour>
    {
        private Action<LuaTable> luaUpdate;
        private Action<LuaTable> luaFixedUpdate;

        public override void Init()
        {
            self.Get("Update", out luaUpdate);
            self.Get("FixedUpdate", out luaFixedUpdate);
        }

        private void Update()
        {
            luaUpdate?.Invoke(self);
        }

        private void FixedUpdate()
        {
            luaFixedUpdate?.Invoke(self);
        }
    }
}


