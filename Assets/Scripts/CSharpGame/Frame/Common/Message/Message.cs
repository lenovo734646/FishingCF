
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : IEnumerable<KeyValuePair<string, object>>
{
    public Dictionary<string, object> dicDatas = null;

    public string Name { get; private set; }
    public object Sender { get; private set; }
    public object Content { get; set; }

    public object this[string key]
    {
        get
        {
            return null == dicDatas || !dicDatas.ContainsKey(key) ? null : dicDatas[key];
        }
        set
        {
            if (null == dicDatas)
                dicDatas = new Dictionary<string, object>();
            if (dicDatas.ContainsKey(key))
                dicDatas[key] = value;
            else
                dicDatas.Add(key, value);
        }
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        if (null == dicDatas)
            yield break;
        foreach (KeyValuePair<string, object> kvp in dicDatas)
        {
            yield return kvp;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return dicDatas.GetEnumerator();
    }

    public Message(string name, object sender, object content = null, params object[] dicParams)
    {
        Name = name;
        Sender = sender;
        Content = content;
        foreach (object dicParam in dicParams)
        {
            if (dicParam.GetType() == typeof(Dictionary<string, object>))
            {
                foreach (KeyValuePair<string, object> kvp in dicParam as Dictionary<string, object>)
                    this[kvp.Key] = kvp.Value;
            }
            else
                Debug.LogWarningFormat("Warning: Message dicParam Type Is Not Dictionary. Type: {0}", dicParam.GetType());
        }
    }

    public Message(Message message)
    {
        Name = message.Name;
        Sender = message.Sender;
        Content = message.Content;
        foreach (KeyValuePair<string, object> kvp in message.dicDatas)
            this[kvp.Key] = kvp.Value;
    }

    public void Add(string key, object value)
    {
        this[key] = value;
    }

    public void Remove(string key)
    {
        if (null != dicDatas && dicDatas.ContainsKey(key))
        {
            dicDatas.Remove(key);
        }
    }

    public void Send()
    {
        MessageCenter.Instance.SendMessage(this);
    }
}