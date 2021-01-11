using JBPROTO;
using Newtonsoft.Json;
using System;
using UnityEngine;

public class NetReactor : INetReactor
{
    private Message sendMsg;
    public Message SendMsg
    {
        get
        {
            if (null == sendMsg)
                sendMsg = new Message(MsgType.NET_CONNECT, this);
            return sendMsg;
        }
    }

    public void onRecvMessage(INetProtocol proto)
    {
        var msg = string.Format("{0} 接收 {1}:{2}",
            DateTime.Now.ToString("HH:mm:ss:fff"),
            proto.ToString(),
            JsonConvert.SerializeObject(proto, Formatting.Indented)
            );
        Debug.Log(msg);
    }

    public void onSendMessage(INetProtocol proto)
    {
        var msg = string.Format("{0} 发送 {1}:{2}",
            DateTime.Now.ToString("HH:mm:ss:fff"),
            proto.ToString(),
            JsonConvert.SerializeObject(proto, Formatting.Indented)
            );
        Debug.Log(msg);
    }

    /// <summary>
    /// 连接遇到错误 
    /// </summary>
    public void onNetConnectError()
    {
        SendMsg.Content = EnumNetConnectState.Error;
        SendMsg.Send();
    }

    /// <summary>
    /// 连接成功
    /// </summary>
    public void onNetConnectEstablished()
    {
        SendMsg.Content = EnumNetConnectState.Established;
        SendMsg.Send();
    }

    /// <summary>
    /// 连接被断开
    /// </summary>
    public void onNetDisconnect()
    {
        SendMsg.Content = EnumNetConnectState.Disconnect;
        SendMsg.Send();
    }
}
