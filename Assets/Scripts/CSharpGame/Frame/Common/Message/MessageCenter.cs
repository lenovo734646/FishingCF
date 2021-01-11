
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

using System.Collections.Generic;
public class MessageCenter : Singleton<MessageCenter>
{
    private Dictionary<string, List<MessageEvent>> dicMsgEvents = null;

    public override void Init() => dicMsgEvents = new Dictionary<string, List<MessageEvent>>();

    public void AddListener(string messageName, MessageEvent messageEvent)
    {
        List<MessageEvent> list = null;
        if (dicMsgEvents.ContainsKey(messageName))
            list = dicMsgEvents[messageName];
        else
        {
            list = new List<MessageEvent>();
            dicMsgEvents.Add(messageName, list);
        }
        if (!list.Contains(messageEvent))
            list.Add(messageEvent);
    }

    public void RemoveListener(string messageName, MessageEvent messageEvent)
    {
        if (dicMsgEvents.ContainsKey(messageName))
        {
            List<MessageEvent> list = dicMsgEvents[messageName];
            if (list.Contains(messageEvent))
            {
                list.Remove(messageEvent);
            }
            if (list.Count <= 0)
            {
                dicMsgEvents.Remove(messageName);
            }
        }
    }

    public void RemoveAllListener()
    {
        dicMsgEvents.Clear();
    }

    public void SendMessage(Message message)
    {
        DoMessageDispatcher(message);
    }

    public void SendMessage(string name, object sender, object content = null, params object[] dicParams)
    {
        SendMessage(new Message(name, sender, content, dicParams));

        var copy = new Dictionary<string, object>();
        copy.Add("MsgType", name);
        var para = new Dictionary<string, object>();
        if (!ReferenceEquals(dicParams, null))
        {
            for (int i = 0; i < dicParams.Length; i++)
                para.Add(i.ToString(), dicParams[i]);
        }
        copy.Add("Params", para);
        SendMessage(new Message(MsgType.CSHARP_RECEIVE_DATA, sender, content, copy));
    }

    private void DoMessageDispatcher(Message message)
    {
        if (null == dicMsgEvents || !dicMsgEvents.ContainsKey(message.Name))
            return;
        List<MessageEvent> list = dicMsgEvents[message.Name];
        for (int i = 0; i < list.Count; i++)
            list[i]?.Invoke(message);
    }
}