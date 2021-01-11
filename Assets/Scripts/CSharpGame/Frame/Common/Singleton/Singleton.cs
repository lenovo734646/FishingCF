
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *         1：单列基类
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

using System;

public abstract class Singleton<T> where T : class, new()
{
    protected static T _Instance = null;
    public static T Instance
    {
        get
        {
            if (null == _Instance)
                _Instance = Activator.CreateInstance<T>();
            return _Instance;
        }
    }
    protected Singleton()
    {
        if (null != _Instance)
        {
            var msg = string.Format("This {0} Singleton Instance is not null !!!", typeof(T));
            throw new SingletonException(msg);
        }
        Init();
    }

    ~Singleton()
    {

    }

    public virtual void Init() { }

}