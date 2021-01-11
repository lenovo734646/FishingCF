
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

using JBPROTO;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LoginModule : BaseModule 
{
    public LoginModule()
    {
        AutoRegister = true;
    }

    protected override void OnLoad()
    {
        MessageCenter.Instance.AddListener(MsgType.NET_CONNECT, netConnectState);
        MessageCenter.Instance.AddListener(MsgType.NET_DISCONNECT_NTF, disconnectNtf);
        MessageCenter.Instance.AddListener(MsgType.NET_HAND_ACK, handAck);
        MessageCenter.Instance.AddListener(MsgType.NET_LOGIN_PLATFORM_ACK, loginPlatformAck);
        MessageCenter.Instance.AddListener(MsgType.NET_ACCESSSERVICE_ACK, accessServiceAck);
    }

    protected override void OnRelease()
    {
        MessageCenter.Instance.RemoveListener(MsgType.NET_CONNECT, netConnectState);
        MessageCenter.Instance.RemoveListener(MsgType.NET_DISCONNECT_NTF, disconnectNtf);
        MessageCenter.Instance.RemoveListener(MsgType.NET_HAND_ACK, handAck);
        MessageCenter.Instance.RemoveListener(MsgType.NET_LOGIN_PLATFORM_ACK, loginPlatformAck);
        MessageCenter.Instance.RemoveListener(MsgType.NET_ACCESSSERVICE_ACK, accessServiceAck);
    }

    //连接或断开服务器请求
    private Action accessServiceAction = null;
    private int siteId = -1;     
    public void AccessServiceReq(int groupId, EnumAccessServiceType actionType, Action action)
    {
        accessServiceAction = action;
        siteId = groupId;
        NetController.Instance.SendAccessServiceReq(groupId, actionType);
    }

    //连接或断开服务器回应
    private void accessServiceAck(Message msg)
    {
        var rsp = msg.Content as CLGTAccessServiceAck;
        switch (rsp.errcode)
        {
            case 0:
                var actionType = (EnumAccessServiceType)msg["ActionType"];
                switch (actionType)
                {
                    case EnumAccessServiceType.JoinGame:
                        SysDefines.SiteId = siteId;
                        siteId = -1;
                        accessServiceAction?.Invoke();
                        accessServiceAction = null;
                        break;
                    case EnumAccessServiceType.QuitGame:
                        //NetController.Instance.SendLogoutReq();
                        accessServiceAction?.Invoke();
                        accessServiceAction = null;
                        break;
                }
                var str = 1 == (int)actionType ? "加入" : "断开";
                Debug.LogFormat($"{str} 服务组 {SysDefines.SiteId} 成功");
                break;
            default:
                var hintMsg = TLanguageHelper.DataMap.Values.ToList().Find(a => a.key == $"CLGTAccessServiceAck_{rsp.errcode}").CN;
                GameController.Instance.CreateHintMessage(hintMsg);
                break;
        }
    }

    //连接或断开服务器回应
    //private void accessServiceAck(Message msg)
    //{
    //    var rsp = msg.Content as CLGTAccessServiceAck;
    //    switch (rsp.errcode)
    //    {
    //        case 0:
    //            var actionType = (EnumAccessServiceType)msg["ActionType"];
    //            switch (actionType)
    //            {
    //                case EnumAccessServiceType.JoinGame:
    //                    UIManager.Instance.OpenUI(EnumUIType.LoadingUI, true, EnumUIType.MainUI, EnumSceneType.MainScene);
    //                    SysDefines.SiteId = EnumSiteType.Fishing;
    //                    break;
    //                case EnumAccessServiceType.QuitGame:
    //                    NetController.Instance.SendLogoutReq();
    //                    break;
    //            }
    //            var str = 1 == (int)actionType ? "加入" : "断开";
    //            Debug.LogFormat($"{str} 服务组成功");
    //            break;
    //        default:
    //            var hintMsg = TLanguageHelper.DataMap.Values.ToList().Find(a => a.key == $"CLGTAccessServiceAck_{rsp.errcode}").CN;
    //            GameController.Instance.CreateHintMessage(hintMsg);
    //            break;
    //    }
    //}

    //登录回应
    private void loginPlatformAck(Message msg)
    {
        var rsp = msg.Content as CLGTLoginAck;
        switch (rsp.errcode)
        {
            case 0:
                if(SysDefines.LoginType == 4)
                    PlayerPrefs.SetString("WechatOpenId", SysDefines.LoginToken);
                GameController.Instance.Player = new GamePlayer(rsp);
                CoroutineController.Instance.StartAliveCor();
                SysDefines.IsDisconnect = false;
                AccessServiceReq(1, EnumAccessServiceType.JoinGame, () =>
                {
                    AppRoot.Get().Init();
                });
                //UIManager.Instance.OpenUI(EnumUIType.LoadingUI, true, EnumUIType.MainUI, EnumSceneType.MainScene);
                //这里默认链接捕鱼大厅，目前只有捕鱼游戏
                //NetController.Instance.SendAccessServiceReq(EnumGameGroupType.Fish, EnumAccessServiceType.JoinGame);
                break;
            default:
                var hintMsg = TLanguageHelper.DataMap.Values.ToList().Find(a => a.key == $"TextLoginPrompt_{rsp.errcode:d2}").CN;
                GameController.Instance.CreateHintMessage(hintMsg);
                break;
        }
    }

    //握手回应
    private void handAck(Message msg)
    {
        var rsp = msg.Content as CLGTHandAck;
        switch (rsp.errcode)
        {
            case 0:
                NetController.Instance.SendLoginPlatformReq(rsp.random_key);
                break;
            default:
                var hintMsg = TLanguageHelper.DataMap.Values.ToList().Find(a => a.key == $"TextHandPrompt_{rsp.errcode:d2}").CN;
                GameController.Instance.CreateHintMessage(hintMsg);
                break;
        }
    }

    public void SendNetConnect(byte loginType)
    {
        SysDefines.LoginType = loginType;
        NetController.Instance.GetIpPort(netConnect);
    }

    private void netConnect()
    {
        if (string.IsNullOrEmpty(SysDefines.Ip))
            GameController.Instance.CreateHintMessage("连接服务器出了点问题，请重新连接");
        else
            NetController.Instance.netComponent.connectWithTimeout(SysDefines.Ip, (int)SysDefines.Port, 5000);
    }

    //网络连接状态
    private void netConnectState(Message msg)
    {
        EnumNetConnectState state = (EnumNetConnectState)Enum.Parse(typeof(EnumNetConnectState), msg.Content.ToString(), true);
        string stateMsg = DateTime.Now.ToString("HH:mm:ss:fff");
        switch (state)
        {
            case EnumNetConnectState.Error:
                stateMsg = string.Format("[{0}]连接遇到错误!IP:{1},Port:{2}", stateMsg, SysDefines.Ip, SysDefines.Port);
                break;
            case EnumNetConnectState.Established:
                stateMsg = string.Format("[{0}]连接建立成功！IP:{1},Port:{2}", stateMsg, SysDefines.Ip, SysDefines.Port);
                NetController.Instance.SendHandReq();
                break;
            case EnumNetConnectState.Disconnect:
                if (!SysDefines.IsDisconnect)
                {
                    SysDefines.IsDisconnect = true;
                    CoroutineController.Instance.StopAliveCor();
                    GameController.Instance.ReturnToLogin();
                }
                stateMsg = string.Format("[{0}]连接被断开！IP:{1},Port:{2}", stateMsg, SysDefines.Ip, SysDefines.Port);
                break;
            default:
                stateMsg = string.Format("[{0}]未知消息连接状态", stateMsg);
                break;
        }
        Debug.Log(stateMsg);
    }

    //网络断开通知
    private void disconnectNtf(Message msg)
    {
        if (!SysDefines.IsDisconnect)
        {
            SysDefines.IsDisconnect = true;
            CoroutineController.Instance.StopAliveCor();
            var rsp = msg.Content as CLGTDisconnectNtf;
            var hintMsg = string.Empty;
            switch (rsp.code)
            {
                case 0:
                    GameController.Instance.ReturnToLogin();
                    break;
                default:
                    hintMsg = TLanguageHelper.DataMap.Values.ToList().Find(a => a.key == $"TextSystemPrompt_{rsp.code:d2}").CN;
                    UIManager.Instance.OpenMessageBoxUI(hintMsg, 10, EnumMessageBoxType.OK, GameController.Instance.ReturnToLogin);
                    break;
            }
        }
    }
}

