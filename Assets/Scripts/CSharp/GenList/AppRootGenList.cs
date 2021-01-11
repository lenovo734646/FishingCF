using System;
using System.Collections.Generic;
using XLua;

namespace XLuaExtension
{
    public static class AppRootGenList
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(AppRoot),
            typeof(Context),
            typeof(AssetLoader),
            typeof(BundleManager),
            typeof(AssetConfig),
            //typeof(Client),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {

        };
    }
}
