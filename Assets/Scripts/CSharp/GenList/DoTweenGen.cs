using System;
using System.Collections.Generic;
using XLua;

namespace XLuaExtension
{
    public static class DoTweenGen
    {
        [LuaCallCSharp]
        public static List<Type> LuaCallCSharp = new List<Type>()
        {
            typeof(DG.Tweening.DOTween),
            typeof(DG.Tweening.Tween),
            typeof(DG.Tweening.Sequence),
            typeof(DG.Tweening.Tweener),
            typeof(DG.Tweening.TweenCallback),
            typeof(DG.Tweening.Ease),
            typeof(DG.Tweening.LoopType),
            typeof(DG.Tweening.PathMode),
            typeof(DG.Tweening.PathType),
            typeof(DG.Tweening.RotateMode),
            typeof(DG.Tweening.ScrambleMode),
            typeof(DG.Tweening.TweenExtensions),
            typeof(DG.Tweening.TweenSettingsExtensions),
            typeof(DG.Tweening.ShortcutExtensions),
            typeof(DG.Tweening.Plugins.Options.PathOptions),
            typeof(DG.Tweening.Plugins.PathPlugin),
            typeof(DG.Tweening.Plugins.Options.OrientType),
            typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,
                DG.Tweening.Plugins.Core.PathCore.Path,
                DG.Tweening.Plugins.Options.PathOptions>),
        };

        [CSharpCallLua]
        public static List<Type> CSharpCallLua = new List<Type>()
        {
            typeof(DG.Tweening.TweenCallback),
            typeof(DG.Tweening.TweenCallback<>),
        };
    }
}

