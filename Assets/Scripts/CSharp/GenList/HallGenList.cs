using System;
using System.Collections.Generic;
using XLua;

namespace XLuaExtension
{
    public static class HallGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(SysDefines),
            typeof(UIManager),
            typeof(GameController),
            typeof(UnityHelper)
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(Message),
        };
    }
}
