using System;
using System.Collections.Generic;
using XLua;

namespace XLuaExtension
{
    public static class SpineGen
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(Spine.Unity.SkeletonGraphic),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {

        };
    }
}