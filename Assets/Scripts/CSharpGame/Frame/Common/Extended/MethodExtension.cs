
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

using UnityEngine;

static public class MethodExtension
{
    static public T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T result = go.GetComponent<T>();
        if (null == result)
            result = go.AddComponent<T>();
        return result;
    }

    static public T GetOrAddComponent<T>(this Transform transform) where T : Component
    {
        return GetOrAddComponent<T>(transform.gameObject);
    }

    static public T GetOrAddComponent<T>(this Component component) where T : Component
    {
        return GetOrAddComponent<T>(component.gameObject);
    }
}