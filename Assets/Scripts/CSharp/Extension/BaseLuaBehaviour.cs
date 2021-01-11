using UnityEngine;
using XLua;

namespace XLuaExtension
{
    public abstract class BaseLuaBehaviour<T> : MonoBehaviour where T : BaseLuaBehaviour<T>
    {
        public LuaTable self;

        public virtual void Init() { }

        public static T Bind(GameObject go, LuaTable self)
        {
            T behaviour = go.AddComponent<T>();
            behaviour.self = self;
            behaviour.Init();
            return behaviour;
        }

        public static void Remove(GameObject go)
        {
            if (go == null)
                return;

            foreach (var behaviour in go.GetComponents<T>())
            {
                Remove(behaviour);
            }
        }

        public static void Remove(T behaviour)
        {
            if (behaviour == null)
                return;

            Destroy(behaviour);
        }
    }
}

