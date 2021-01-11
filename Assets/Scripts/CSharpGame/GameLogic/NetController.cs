
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
using UnityEngine;
using JBPROTO;
using System.Collections.Generic;
using QL.Core;
using QL.Protocol;

public class NetController : DDOLSingleton<NetController> 
{

    private DefaultQLClient webClient;
    public DefaultQLClient WebClient
    {
        get
        {
            if (null == webClient) {
                webClient = new DefaultQLClient("client", "mbXr8nNL3Gnust17");
                //如果区服Id小于0，修改为测试环境url
                if (SysDefines.ZoneId < 0)
                    webClient.ServerUrl = "http://39.101.175.23:8000/router/rest";
            }
            return webClient;
        }
    }

    public INetComponent netComponent;

    private void Awake()
    {
        netComponent = NetHelper.createNetComponent(new NetReactor());
        netComponent.addResponser(new ClientGTResponser());                 // 解析处理 握手，心跳，连接平台，连接游戏服务器，断开 等消息
        //netComponent.addResponser(new ClientFishingMainResponser());
        //netComponent.addResponser(new ClientFishingRoomResponser());
        netComponent.addResponser(new ClientPFResponser());                // 道具使用相关
    }

    private void Update()
    {
        netComponent.run();
    }

    #region 与web通信
    //获取热更下载地址
    public void GetDownloadUrl(Action<string> onGetUrl)
    {
        ClientGetDownloadUrlRequest webReq = new ClientGetDownloadUrlRequest();
        webReq.ZoneId = SysDefines.ZoneId;
        ClientGetDownloadUrlResponse webRsp = executeWebRequest(webReq);
        SysDefines.OssUrl = webRsp.OssUrl;
        onGetUrl?.Invoke(webRsp.ClientConfigPkgMd5);
    }

    /// <summary>
    /// 获取连接IP 与 Port
    /// </summary>
    /// <param name="connectGame">回调函数</param>
    public void GetIpPort(Action connectGame)
    {
        ClientGetGateConnectionRequest webReq = new ClientGetGateConnectionRequest();
        webReq.ZoneId = SysDefines.ZoneId;
        ClientGetGateConnectionResponse webRsp = executeWebRequest(webReq);
        SysDefines.Ip = webRsp.Ip;
        SysDefines.Port = webRsp.Port;
        //SysDefines.OssUrl = webRsp.OssUrl;
        connectGame?.Invoke();
        Debug.LogFormat("ip: {0}, port: {1}", SysDefines.Ip, SysDefines.Port);
    }

    //获得公告
    public void GetAnnouncement(Action<string, string, string> showAnnouncement)
    {
        ClientAnnouncementQueryRequest webReq = new ClientAnnouncementQueryRequest();
        webReq.ZoneId = SysDefines.ZoneId;
        webReq.Language = "CN";
        ClientAnnouncementQueryResponse webRsp = executeWebRequest(webReq);
        showAnnouncement?.Invoke(webRsp.AnnouncementTitle, webRsp.AnnouncementContent, webRsp.AnnouncementCreateTime);
    }

    //微信授权登录获取appid
    public void GetWechatAppid(Action<string, string> callback)
    {
        ClientWxLoginGetAppInfoRequest webReq = new ClientWxLoginGetAppInfoRequest();
        webReq.ZoneId = SysDefines.ZoneId;
        ClientWxLoginGetAppInfoResponse webRsp = executeWebRequest(webReq);
        callback?.Invoke(webRsp.AppId, webRsp.Scope);
    }

    //微信授权登录code交换openid
    public void GetWechatOpenId(string code, Action<string> callback)
    {
        ClientWxLoginGetOpenIdRequest webReq = new ClientWxLoginGetOpenIdRequest();
        webReq.ZoneId = SysDefines.ZoneId;
        webReq.Code = code;
        ClientWxLoginGetOpenIdResponse webRsp = executeWebRequest(webReq);
        callback?.Invoke(webRsp.OpenId);
    }

    private T executeWebRequest<T>(IQLRequest<T> request) where T : QLResponse
    {
        T webRsp = WebClient.Execute(request);
        return webRsp;
    }

    #endregion
    /// <summary>
    /// 握手协议
    /// </summary>
    public void SendHandReq()
    {
        CLGTHandReq req = new CLGTHandReq();
        req.platform = SysDefines.Platform;
        req.product = 1;
        req.version = SysDefines.Version;
        req.device = SystemInfo.deviceUniqueIdentifier;
        req.channel = "com.game.fishing.android";
        req.country = "ZH-CN";
        req.language = "CN";
        netComponent.asyncRequest<CLGTHandAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_HAND_ACK, this, rsp);
         }
        );
    }

    /// <summary>
    /// 登录大厅请求
    /// </summary>
    /// <param name="randomKey">随机密钥</param>
    public void SendLoginPlatformReq(int randomKey)
    {
        CLGTLoginReq req = new CLGTLoginReq();
        req.login_type = SysDefines.LoginType;
        if (SysDefines.LoginType == 1)
            //req.token = UnityHelper.CA3Encode("zcj132", randomKey);
            req.token = UnityHelper.CA3Encode(SystemInfo.deviceUniqueIdentifier, randomKey);
        else
            req.token = UnityHelper.CA3Encode(SysDefines.LoginToken, randomKey);
        netComponent.asyncRequestWithLock<CLGTLoginAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_LOGIN_PLATFORM_ACK, this, rsp);

             //打开app到登录完成需要一定时间，这段时间内有可能发布了客户端配置表
             //为了在这种情况下也能加载到最新配置，这时调用一下LoadFromNet ^_^
             //下面还是先注释掉吧，2019年7月13日11:01:30 client_config_md5参数需要从json串里查找
             //if (rsp.errcode == 0)
             //{
             //    TableLoadHelper.LoadFromNet(SysDefines.OssUrl, rsp.client_config_md5, null);
             //}
         });
    }

    /// <summary>
    /// 登出大厅请求
    /// </summary>
    public void SendLogoutReq()
    {
        CLPFLogoutReq req = new CLPFLogoutReq();
        netComponent.send(req);
    }

    /// <summary>
    /// 连接或断开服务器请求
    /// </summary>
    /// <param name="groupId">服务组Id</param>
    /// <param name="action">1 加入服务 2 离开服务</param>
    public void SendAccessServiceReq(EnumGameGroupType groupId, EnumAccessServiceType action)
    {
        CLGTAccessServiceReq req = new CLGTAccessServiceReq();
        req.group_id = (int)groupId;
        req.action = (int)action;
        netComponent.asyncRequest<CLGTAccessServiceAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_ACCESSSERVICE_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"ActionType", action}
            });
        });
    }

    /// <summary>
    /// 连接或断开服务器请求
    /// </summary>
    /// <param name="groupId">服务组Id</param>
    /// <param name="action">1 加入服务 2 离开服务</param>
    public void SendAccessServiceReq(int groupId, EnumAccessServiceType action)
    {
        CLGTAccessServiceReq req = new CLGTAccessServiceReq();
        req.group_id = (int)groupId;
        req.action = (int)action;
        netComponent.asyncRequest<CLGTAccessServiceAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_ACCESSSERVICE_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"ActionType", action},
            });
        });
    }

    /// <summary>
    /// 加入玩法请求
    /// </summary>
    /// <param name="">游戏玩法的ID</param>
    public void SendEnterSiteReq(int siteId, int roomId = -1, int seatId = -1)
    {
        //清理上次渔场内的ack协议缓存
        netComponent.clearWaitingResponse<JBPROTO.CLFREnterGameAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRGetReadyAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRJoinMatchAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRGunValueChangeAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRShootAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRMultiShootAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRHitAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRGunUnlockAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRBonusWheelAck>();
        netComponent.clearWaitingResponse<JBPROTO.CLFRGunSwitchAck>();

        //发送进入渔场协议
        CLFMEnterSiteReq req = new CLFMEnterSiteReq();
        req.site_id = siteId;
        netComponent.asyncRequestWithLock<CLFMEnterSiteAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_ENTERSITE_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"SiteId", siteId},
                {"RoomId", roomId},
                {"SeatId", seatId},
            });
        });
    }

    /// <summary>
    /// 退出玩法请求
    /// </summary>
    /// <param name="siteId">游戏玩法的ID</param>
    /// <param name="reason">退出原因 0正常渔场退出 1选座失败</param>
    public void SendExitSiteReq(int siteId, int reason = 0)
    {
        CLFMExitSiteReq req = new CLFMExitSiteReq();
        req.site_id = siteId;
        netComponent.asyncRequestWithLock<CLFMExitSiteAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_EXITSITE_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"SiteId", siteId},
                {"Reason", reason},
            });
        });
    }

    /// <summary>
    /// 进入捕鱼游戏
    /// </summary>
    /// <param name="configID">配置的房间号</param>
    public void SendEnterFishingGameReq(int configID, int roomId, int seatId)
    {
        Debug.LogError("SendEnterFishingGameReq.....");
        //CLFREnterGameReq req = new CLFREnterGameReq();
        //req.config_id = configID;
        //req.room_id = roomId;
        //req.seat_id = seatId;
        //netComponent.asyncRequestWithLock<CLFREnterGameAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessEnterFishingRoomAck(rsp, configID);
        //});
    }

    /// <summary>
    /// 退出捕鱼房间
    /// </summary>
    public void SendExitGameReq(Action action)
    {
        Debug.LogError("SendExitGameReq.....");
        //CLFRExitGameReq req = new CLFRExitGameReq();
        //netComponent.asyncRequestWithLock<CLFRExitGameAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessExitGameAck(rsp, action);
        //});
    }

    /// <summary>
    /// 表示客户端已准备可以接收服务器的推送
    /// </summary>
    public void SendGetReadyReq()
    {
        Debug.LogError("SendGetReadyReq.....");
        //CLFRGetReadyReq req = new CLFRGetReadyReq();
        //netComponent.asyncRequest<CLFRGetReadyAck>(req, rsp =>
        // {
        //     FishingRoomController.ProcessGetReadyAck(rsp);
        // });
    }

    /// <summary>
    /// 开火
    /// </summary>
    /// <param name="angle">炮台角度</param>
    /// <param name="lockFishID">锁定鱼的id</param>
    public void SendShootReq(int angle, int lockFishID)
    {
        Debug.LogError("SendShootReq.....");
        //CLFRShootReq req = new CLFRShootReq();
        //req.angle = angle;
        //req.lock_fish = lockFishID;
        //netComponent.asyncRequest<CLFRShootAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessShootAck(rsp, angle);
        //});
    }

    /// <summary>
    /// 分身开火
    /// </summary>
    /// <param name="angle">炮台角度</param>
    /// <param name="locakFishID">锁定鱼的id</param>
    public void SendCloneShootReq(int[] angles, int[] lockFishIDs)
    {
        Debug.LogError("SendCloneShootReq.....");
        //CLFRMultiShootReq req = new CLFRMultiShootReq();
        //req.shoot_len = (SByte)angles.Length;
        //var info = new CLFRShootInfo[angles.Length];
        //for (int i = 0; i < angles.Length; i++)
        //{
        //    info[i] = new CLFRShootInfo();
        //    info[i].angle = angles[i];
        //    info[i].lock_fish = lockFishIDs[i];
        //}
        //req.shoot_array = info;

        //netComponent.asyncRequest<CLFRMultiShootAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessCloneShootAck(rsp);
        //});
    }

    /// <summary>
    /// 碰撞请求
    /// </summary>
    /// <param name="bulletId">子弹ID</param>
    /// <param name="fishId">鱼ID</param>
    public void SendHitReq(int bulletId, int fishId)
    {
        Debug.LogError("SendHitReq.....");
        //CLFRHitReq req = new CLFRHitReq();
        //req.bullet_id = bulletId;
        //req.fish_id = fishId;
        //netComponent.asyncRequest<CLFRHitAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessHitAck(rsp, bulletId, fishId);
        //});
    }

    /// <summary>
    /// 上报机器人子弹碰撞信息
    /// </summary>
    /// <param name="bulletId">子弹Id</param>
    /// <param name="fishId">鱼Id</param>
    public void SendRobotHitRpt(int bulletId, int fishId)
    {
        CLFRRobotHitRpt rpt = new CLFRRobotHitRpt();
        rpt.bullet_id = bulletId;
        rpt.fish_id = fishId;
        netComponent.send(rpt);
    }

    /// <summary>
    /// 改变炮值请求
    /// </summary>
    /// <param name="gunValue">改变的炮值</param>
    public void SendGunValueChangeReq(long gunValue)
    {
        Debug.LogError("SendGunValueChangeReq.....");
        //CLFRGunValueChangeReq req = new CLFRGunValueChangeReq();
        //req.gun_value = gunValue;
        //netComponent.asyncRequest<CLFRGunValueChangeAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessGunValueChangeAck(rsp);
        //});
    }

    /// <summary>
    /// 使用物品请求
    /// </summary>
    /// <param name="itemId">物品类型ID</param>
    /// <param name="itemSubId">物品子ID</param>
    /// <param name="count">使用物品的数量[默认使用1个]</param>
    public void SendItemUseReq(int itemId, int itemSubId, int count = 1)
    {
        Debug.LogError("SendItemUseReq.....");
        //CLPFItemUseReq req = new CLPFItemUseReq();
        //req.item = new CLPFItemInfo();
        //req.item.item_id = itemId;
        //req.item.item_sub_id = itemSubId;
        //req.item.item_count = count;
        //netComponent.asyncRequest<CLPFItemUseAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessItemUseAck(rsp, itemId, itemSubId, count);
        //});
    }

    /// <summary>
    /// 购买物品请求
    /// </summary>
    /// <param name="itemId">物品类型ID</param>
    /// <param name="itemSubId">物品子ID</param>
    /// <param name="count">购买物品的数量[默认购买1个]</param>
    public void SendItemBuyReq(int itemId, int itemSubId, int count = 1, Action action = null)
    {
        CLPFItemBuyReq req = new CLPFItemBuyReq();
        req.item = new CLPFItemInfo();
        req.item.item_id = itemId;
        req.item.item_sub_id = itemSubId;
        req.item.item_count = count;
        netComponent.asyncRequest<CLPFItemBuyAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_ITEM_BUY_ACK, this, rsp, new Dictionary<string, object>()
            {
                { "action",action}
            });
        });
    }

    /// <summary>
    /// 解锁炮台请求
    /// </summary>
    public void SendGunUnlockReq()
    {
        Debug.LogError("SendGunUnlockReq.....");
        //CLFRGunUnlockReq req = new CLFRGunUnlockReq();
        //netComponent.asyncRequest<CLFRGunUnlockAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessGunUnlockAck(rsp);
        //});
    }

    /// <summary>
    /// 锻造炮台请求
    /// </summary>
    /// <param name="use_crystal">是否使用水晶</param>
    public void SendGunForgeReq(sbyte useCrystal)
    {
        CLFMGunForgeReq req = new CLFMGunForgeReq();
        req.use_crystal = useCrystal;
        netComponent.asyncRequest<CLFMGunForgeAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_FORGE, this, rsp);
        });
    }

    /// <summary>
    /// 奖金抽奖请求
    /// </summary>
    public void SendBonusWheelReq()
    {
        CLFRBonusWheelReq req = new CLFRBonusWheelReq();
        netComponent.asyncRequest<CLFRBonusWheelAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_BONUSWHEEL_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 切换炮台请求
    /// </summary>
    public void SendGunSwitchReq(int gunId)
    {
        Debug.LogError("SendGunSwitchReq.....");
        //CLFRGunSwitchReq req = new CLFRGunSwitchReq();
        //req.gun_id = gunId;
        //netComponent.asyncRequest<CLFRGunSwitchAck>(req, rsp =>
        //{
        //    FishingRoomController.ProcessGunSwitchAck(rsp, gunId);
        //});
    }

    /// <summary>
    /// 修改头像
    /// </summary>
    /// <param name="headId">新头像id</param>
    public void SendModifyHeadReq(int headId)
    {
        CLPFModifyHeadReq req = new CLPFModifyHeadReq();
        req.new_head = headId;
        netComponent.asyncRequest<CLPFModifyHeadAck>(req, rsp =>
        {
            switch (rsp.errcode)
            {
                case 0:
                    GameController.Instance.Player.SetNewHead(headId);
                    break;
                default:
                    Debug.Log($"修改头像失败：{rsp.errcode}");
                    break;
            }
        });
    }

    /// <summary>
    /// 设置昵称
    /// </summary>
    /// <param name="nickName">新昵称</param>
    public void SendModifyNicknameReq(string nickName)
    {
        CLPFModifyNicknameReq req = new CLPFModifyNicknameReq();
        req.new_nickname = nickName;
        netComponent.asyncRequest<CLPFModifyNicknameAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_MODIFYNICKNAME_ACK, this, rsp, new Dictionary<string, object>()
            {
                { "NickName",nickName}
            });
        });
    }

    /// <summary>
    /// 商城购买次数请求
    /// </summary>
    public void SendShopQueryBuyCountReq()
    {
        CLPFShopQueryBuyCountReq req = new CLPFShopQueryBuyCountReq();
        netComponent.asyncRequest<CLPFShopQueryBuyCountAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_SHOP_BUYCOUNT_QUERY, this, rsp);
        });
    }

    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="shopId">商品id</param>
    public void SendFShopBuyReq(int shopId)
    {
        CLPFShopBuyReq req = new CLPFShopBuyReq();
        req.shop_id = shopId;
        netComponent.asyncRequest<CLPFShopBuyAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_SHOP_BUY, this, rsp, new Dictionary<string, object>()
            {
                { "shopId", shopId }
            });
        });
    }
    /// <summary>
    /// 商城充值请求
    /// </summary>
    /// <param name="payMode"></param>
    /// <param name="shopId"></param>
    public void SendShopRechargeReq(int payMode, int shopId, int contentType = 1)
    {
        CLPFRechargeReq req = new CLPFRechargeReq();
        req.pay_mode = payMode;
        req.content_id = shopId;
        req.content_type = contentType;
        netComponent.asyncRequest<CLPFRechargeAck>(req, rsp =>
        {
            if (rsp.errcode == 0)
            {
                MessageCenter.Instance.SendMessage(MsgType.CLIENT_PAY_REQ, this, rsp);
            }
        });
    }
    //签到查询
    public void SendQuerySignReq()
    {
        CLPFQuerySignReq req = new CLPFQuerySignReq();
        netComponent.asyncRequest<CLPFQuerySignAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_QUERY_SIGN, this, rsp);
        });
    }

    //签到请求
    public void SendActSignReq()
    {
        CLPFActSignReq req = new CLPFActSignReq();
        netComponent.asyncRequest<CLPFActSignAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_ACT_SIGN, this, rsp);
        });
    }

    /// <summary>
    /// 查询VIP抽奖请求
    /// </summary>
    public void SendQueryVipWheelReq()
    {
        CLPFQueryVipWheelReq req = new CLPFQueryVipWheelReq();
        netComponent.asyncRequest<CLPFQueryVipWheelAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_VIPLOTTERY_QUERY, this, rsp);
        });
    }

    /// <summary>
    /// 执行VIP抽奖请求
    /// </summary>
    public void SendActVipWheelReq()
    {
        CLPFActVipWheelReq req = new CLPFActVipWheelReq();
        netComponent.asyncRequest<CLPFActVipWheelAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_VIPLOTTERY_ACT, this, rsp);
         });
    }

    /// <summary>
    /// 获取排行榜请求
    /// </summary>
    /// <param name="rank_type">排行榜类型 1金币榜 2弹头榜</param>
    public void SendGetRankListReq(int rankType)
    {
        CLPFGetRankListReq req = new CLPFGetRankListReq();
        req.rank_type = rankType;
        netComponent.asyncRequest<CLPFGetRankListAck>(req, rsp =>
         {
             switch (rsp.errcode)
             {
                 case 0:
                     //var para = new object[1] { rankType };
                     MessageCenter.Instance.SendMessage(MsgType.NET_RANKINFO_ACK, this, rsp, new Dictionary<string, object>()
                     {
                         {"rankType",rankType}
                     });
                     break;
                 default:
                     Debug.Log($"获取排行榜失败：{rsp.errcode}");
                     break;
             }
         });
    }

    /// <summary>
    /// 比赛排行榜请求
    /// </summary>
    /// <param name="matchType">排行榜类型 1免费赛 2大奖赛</param>
    public void SendMatchRankReq(int matchType)
    {
        CLFMMatchRankReq req = new CLFMMatchRankReq();
        req.match_type = matchType;
        netComponent.asyncRequest<CLFMMatchRankAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_MATCHRANK, this, rsp, new Dictionary<string, object>()
            {
                {"matchType",matchType}
            });
        });
    }

    /// <summary>
    /// 加入比赛请求
    /// </summary>
    public void SendJoinMatchReq()
    {
        Debug.LogError("SendJoinMatchReq.....");
        //CLFRJoinMatchReq req = new CLFRJoinMatchReq();
        //netComponent.asyncRequest<CLFRJoinMatchAck>(req, rsp =>
        //{
        //    FishingRoomController.PrecessJoinMatchAck(rsp);
        //});
    }

    /// <summary>
    /// 查询所有邮件ID请求
    /// </summary>
    public void SendQueryAllMailId()
    {
        CLPFMailQueryAllIdsReq req = new CLPFMailQueryAllIdsReq();
        netComponent.asyncRequest<CLPFMailQueryAllIdsAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_QUERY_ALL_MAIL, this, rsp);
         });
    }

    /// <summary>
    /// 批量邮件内容请求
    /// </summary>
    public void SendMailBatchQueryContentReq(int[] reqIds, string language = "CN")
    {
        CLPFMailBatchQueryContentReq req = new CLPFMailBatchQueryContentReq();
        req.len = reqIds.Length;
        req.array = reqIds;
        req.language = language;
        netComponent.asyncRequest<CLPFMailBatchQueryContentAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_MAIL_CONTENT_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查看邮件请求
    /// </summary>
    public void SendMailAccessReq(int mailId)
    {
        CLPFMailAccessReq req = new CLPFMailAccessReq();
        req.mail_id = mailId;
        netComponent.asyncRequest<CLPFMailAccessAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_MAIL_ACCESS_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"accessId",mailId}
            });
         });
    }

    /// <summary>
    /// 领取邮件物品请求
    /// </summary>
    public void SendMailFetchItem(int mailId)
    {
        CLPFMailFetchItemReq req = new CLPFMailFetchItemReq();
        req.mail_id = mailId;
        netComponent.asyncRequest<CLPFMailFetchItemAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_MAIL_FETCH_ACK, this, rsp, new Dictionary<string, object>()
                    {
                        {"fetchId",mailId}
                    });
        });
    }

    /// <summary>
    /// 删除邮件请求
    /// </summary>
    public void SendMailRemoveReq(sbyte removeType, int[] remove_ids)
    {
        CLPFMailRemoveReq req = new CLPFMailRemoveReq();
        req.remove_type = removeType;
        req.remove_len = remove_ids == null ? 0 : remove_ids.Length;
        req.remove_ids = remove_ids;
        netComponent.asyncRequest<CLPFMailRemoveAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_MAIL_REMOVE_ACK, this, rsp);
        });
    }

    #region 公会
    /// <summary>
    /// 获取公会推荐列表
    /// </summary>
    /// <param name="operationType">操作类型 1打开 2刷新</param>
    public void SendGuildQueryRecommendListReq(int operationType)
    {
        CLPFGuildQueryRecommendListReq req = new CLPFGuildQueryRecommendListReq();
        netComponent.asyncRequest<CLPFGuildQueryRecommendListAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_RECOMMENDLIST_ACK, this, rsp, new Dictionary<string, object>
            {
                { "OperationType",operationType}
            });
        });
    }

    /// <summary>
    /// 加入公会
    /// </summary>
    /// <param name="guildId">公会id</param>
    public void SendGuildJoinReq(int guildId)
    {
        CLPFGuildJoinReq req = new CLPFGuildJoinReq();
        req.guild_id = guildId;
        netComponent.asyncRequest<CLPFGuildJoinAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_JOIN_ACK, this, rsp, new Dictionary<string, object>
            {
                { "GuildId",guildId}
            });
        });
    }

    /// <summary>
    /// 快读加入公会
    /// </summary>
    public void SendGuildQuickJoinReq()
    {
        CLPFGuildQuickJoinReq req = new CLPFGuildQuickJoinReq();
        netComponent.asyncRequest<CLPFGuildQuickJoinAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_QUICKJOIN_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查找公会
    /// </summary>
    /// <param name="guildId">公会id</param>
    public void SendGuildSearchReq(int guildId)
    {
        CLPFGuildSearchReq req = new CLPFGuildSearchReq();
        req.guild_id = guildId;
        netComponent.asyncRequest<CLPFGuildSearchAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_SEARCH_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 创建公会
    /// </summary>
    /// <param name="guildName">公会名</param>
    /// <param name="badge">公会图标</param>
    /// <param name="userLevelLimit">限制玩家等级</param>
    /// <param name="vipLevelLimit">限制玩家vip等级</param>
    /// <param name="allowAutoJoin">是否允许自动加入</param>
    public void SendGuildCreateReq(string guildName,int badge, int userLevelLimit,int vipLevelLimit,sbyte allowAutoJoin)
    {
        CLPFGuildCreateReq req = new CLPFGuildCreateReq();
        req.name = guildName;
        req.icon = badge;
        req.user_level_limit = userLevelLimit;
        req.vip_level_limit = vipLevelLimit;
        req.allow_auto_join = allowAutoJoin;
        netComponent.asyncRequest<CLPFGuildCreateAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_CREATE_ACK, this, rsp);
        });

    }

    /// <summary>
    /// 获取公会仓库信息请求
    /// </summary>
    /// <param name="operationType">操作类型 1打开 2刷新</param>
    public void SendGuildBagQueryInfoReq(int operationType)
    {
        CLPFGuildBagQueryInfoReq req = new CLPFGuildBagQueryInfoReq();
        netComponent.asyncRequest<CLPFGuildBagQueryInfoAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_CUILD_BAGQUERY_ACK, this, rsp, new Dictionary<string, object>
            {
                { "OperationType",operationType}
            });
        });
    }

    /// <summary>
    /// 公会仓库日志信息
    /// </summary>
    /// <param name="Page">日志页数</param>
    /// <param name="operationType">操作类型 1打开 2刷新</param>
    public void SendGuildBagQueryLogReq(int page, int operationType)
    {
        CLPFGuildBagQueryLogReq req = new CLPFGuildBagQueryLogReq();
        req.page_index = page;
        netComponent.asyncRequestWithLock<CLPFGuildBagQueryLogAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_BAGQUERYLOG_ACK, this, rsp, new Dictionary<string, object>
            {
                {"OperationType",operationType },
                {"Page", page}
            });
        });
    }

    /// <summary>
    /// 捐赠物品
    /// </summary>
    /// <param name="info">捐赠物品信息</param>
    public void SendGuildBagStoreItemReq(ItemInfo info)
    {
        CLPFGuildBagStoreItemReq req = new CLPFGuildBagStoreItemReq();
        var item = new CLPFItemInfo();
        item.item_id = info.ItemID;
        item.item_sub_id = info.ItemSubID;
        item.item_count = (int)info.ItemCount;
        req.item = item;
        netComponent.asyncRequest<CLPFGuildBagStoreItemAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_BAGSTORE_ACK, this, rsp, new Dictionary<string, object>
            {
                { "ItemInfo",info}
            });
        });
    }

    /// <summary>
    /// 分配物品
    /// </summary>
    /// <param name="info">分配物品信息</param>
    /// <param name="userId">分配给某人的id</param>
    public void SendGuildBagFetchItemReq(ItemInfo info,int userId)
    {
        CLPFGuildBagFetchItemReq req = new CLPFGuildBagFetchItemReq();
        var item = new CLPFItemInfo();
        item.item_id = info.ItemID;
        item.item_sub_id = info.ItemSubID;
        item.item_count = (int)info.ItemCount;
        req.item = item;
        req.user_id = userId;
        netComponent.asyncRequest<CLPFGuildBagFetchItemAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_BAGFETCH_ACK, this, rsp, new Dictionary<string, object>
            {
                { "ItemInfo",info},
                { "UserId",userId}
            });
        });
    }


    /// <summary>
    /// 获取公会信息请求
    /// </summary>
    public void SendGuildQueryInfoReq(bool isOpenUI)
    {
        CLPFGuildQueryInfoReq req = new CLPFGuildQueryInfoReq();
        netComponent.asyncRequest<CLPFGuildQueryInfoAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_QUERYINFO_ACK, this, rsp, new Dictionary<string, object>
                     {
                         {"isOpenUI",isOpenUI}
                     });
         });
    }

    /// <summary>
    /// 公会踢出成员请求
    /// </summary>
    /// <param name="idArr">踢出的成员数组</param>
    public void SendGuildKickMemberReq(int[] idArr)
    {
        CLPFGuildKickMemberReq req = new CLPFGuildKickMemberReq();
        req.id_len = idArr.Length;
        req.id_array = idArr;
        netComponent.asyncRequest<CLPFGuildKickMemberAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_KICK_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 获取公会申请列表
    /// </summary>
    public void SendGuildQueryJoinListReq()
    {
        CLPFGuildQueryJoinListReq req = new CLPFGuildQueryJoinListReq();
        netComponent.asyncRequest<CLPFGuildQueryJoinListAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_QUERYJOIN_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 处理公会加入请求
    /// </summary>
    /// <param name="id">玩家Id</param><param name="agree">是否同意加入 1是0否</param>
    public void SendGuildHandleJoinReq(int id, int agree)
    {
        CLPFGuildHandleJoinReq req = new CLPFGuildHandleJoinReq();
        req.user_id = id;
        req.agree = agree;
        netComponent.asyncRequest<CLPFGuildHandleJoinAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_HANDLEJOIN_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 退出公会请求
    /// </summary>
    public void SendGuildExitReq()
    {
        CLPFGuildExitReq req = new CLPFGuildExitReq();
        netComponent.asyncRequest<CLPFGuildExitAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_EXIT_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 修改公会信息请求
    /// </summary>
    public void SendGuildModifyInfoReq(Dictionary<string, object> data)
    {
        CLPFGuildModifyInfoReq req = new CLPFGuildModifyInfoReq();
        req.name = data["name"].ToString();
        req.desc = data["desc"].ToString();
        req.icon = int.Parse(data["icon"].ToString());
        req.user_level_limit = int.Parse(data["user_level_limit"].ToString());
        req.vip_level_limit = int.Parse(data["vip_level_limit"].ToString());
        req.allow_auto_join = sbyte.Parse(data["allow_auto_join"].ToString());
        netComponent.asyncRequest<CLPFGuildModifyInfoAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_MODIFYINFO_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 公会升级请求
    /// </summary>
    public void SendGuildUpgradeReq()
    {
        CLPFGuildUpgradeReq req = new CLPFGuildUpgradeReq();
        netComponent.asyncRequest<CLPFGuildUpgradeAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_UPGRADE_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 获取公会红包信息
    /// </summary>
    public void SendGuildQueryRedPacketInfo()
    {
        CLPFGuildQueryRedPacketInfoReq req = new CLPFGuildQueryRedPacketInfoReq();
        netComponent.asyncRequest<CLPFGuildQueryRedPacketInfoAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_QUERY_REDPACKETINFO_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 获取公会红包排行榜信息
    /// </summary>
    public void SendGuildQueryRedPacketRankReq()
    {
        CLPFGuildQueryRedPacketRankReq req = new CLPFGuildQueryRedPacketRankReq();
        netComponent.asyncRequest<CLPFGuildQueryRedPacketRankAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_QUERY_REDPACKETRANK_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 公会抢红包请求
    /// </summary>
    public void SendGuildActRedPacketReq()
    {
        CLPFGuildActRedPacketReq req = new CLPFGuildActRedPacketReq();
        netComponent.asyncRequest<CLPFGuildActRedPacketAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_ACTREDPACKET_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 查询会长福利
    /// </summary>
    public void SendGuildQueryWelfareReq()
    {
        CLPFGuildQueryWelfareReq req = new CLPFGuildQueryWelfareReq();
        netComponent.asyncRequest<CLPFGuildQueryWelfareAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_QUERYWELFARE_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 领取会长福利
    /// </summary>
    public void SendGuildFetchWelfareReq()
    {
        CLPFGuildFetchWelfareReq req = new CLPFGuildFetchWelfareReq();
        netComponent.asyncRequest<CLPFGuildFetchWelfareAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUILD_FETCHWELFARE_ACK, this, rsp);
        });
    }
    #endregion

    /// <summary>
    /// 心跳包
    /// </summary>
    public IEnumerator SendTKeepAlive()
    {
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            CLGTKeepAlive alive = new CLGTKeepAlive();
            netComponent.send(alive);
        }
    }

    /// <summary>
    /// 领取月卡奖励
    /// </summary>
    public void SendMonthCardFetchRewardReq()
    {
        CLPFMonthCardFetchRewardReq req = new CLPFMonthCardFetchRewardReq();
        netComponent.asyncRequest<CLPFMonthCardFetchRewardAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_MONTHCARD_FETCH_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 查询每日充值请求
    /// </summary>
    public void SendRechargeDailyQueryReq()
    {
        CLPFRechargeDailyQueryReq req = new CLPFRechargeDailyQueryReq();
        netComponent.asyncRequest<CLPFRechargeDailyQueryAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_RECHARGEDAILY_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 领取救济金请求
    /// </summary>
    public void SendReliefGoldFetchReq()
    {
        CLPFReliefGoldFetchReq req = new CLPFReliefGoldFetchReq();
        netComponent.asyncRequest<CLPFReliefGoldFetchAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_RELIEFGOLD_FETCH_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查询任务状态
    /// </summary>
    public void SendTaskQueryReq()
    {
        CLPFTaskQueryReq req = new CLPFTaskQueryReq();
        netComponent.asyncRequest<CLPFTaskQueryAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_TASK_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查询新手成就任务状态
    /// </summary>
    public void SendTaskAchieveQueryReq()
    {
        CLPFTaskAchieveQueryInfoReq req = new CLPFTaskAchieveQueryInfoReq();
        netComponent.asyncRequest<CLPFTaskAchieveQueryInfoAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_TASK_ACHIEVE_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 领取任务奖励请求
    /// </summary>
    public void SendTaskFetchTaskRewardsReq(int taskId)
    {
        CLPFTaskFetchTaskRewardsReq req = new CLPFTaskFetchTaskRewardsReq();
        req.task_id = taskId;
        netComponent.asyncRequest<CLPFTaskFetchTaskRewardsAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_TASK_FETCH_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"taskId", taskId},
            });
        });
    }

    /// <summary>
    /// 领取成就任务奖励请求
    /// </summary>
    public void SendTaskAchieveFetchRewardReq(int taskId)
    {
        CLPFTaskAchieveFetchRewardReq req = new CLPFTaskAchieveFetchRewardReq();
        req.task_achieve_id = taskId;
        netComponent.asyncRequest<CLPFTaskAchieveFetchRewardAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_TASK_ACHIEVE_FETCH_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"taskId", taskId},
            });
        });
    }

    /// <summary>
    /// 领取活跃度奖励请求
    /// </summary>
    public void SendTaskFetchActiveRewardsReq(int taskId)
    {
        CLPFTaskFetchActiveRewardsReq req = new CLPFTaskFetchActiveRewardsReq();
        req.active_id = taskId;
        netComponent.asyncRequest<CLPFTaskFetchActiveRewardsAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_TASK_ACTIVE_FETCH_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"taskId", taskId},
            });
        });
    }

    /// <summary>
    /// 摇数字获取信息请求
    /// </summary>
    public void SendShakeNumberQueryInfoReq()
    {
        CLPFShakeNumberQueryInfoReq req = new CLPFShakeNumberQueryInfoReq();
        netComponent.asyncRequest<CLPFShakeNumberQueryInfoAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_SHAKENUMBER_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 摇数字请求
    /// </summary>
    public void SendShakeNumberActReq()
    {
        CLPFShakeNumberActReq req = new CLPFShakeNumberActReq();
        netComponent.asyncRequest<CLPFShakeNumberActAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_SHAKENUMBER_ACT_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 领取摇到的金币奖励请求
    /// </summary>
    public void SendShakeNumberFetchRewardReq()
    {
        CLPFShakeNumberFetchRewardReq req = new CLPFShakeNumberFetchRewardReq();
        netComponent.asyncRequest<CLPFShakeNumberFetchRewardAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_SHAKENUMBER_FETCHREWARD_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 领取摇到数字后的宝箱礼包请求
    /// </summary>
    /// <param name="day">第几天的宝箱？范围：0-6</param>
    public void SendShakeNumberFetchBoxRewardReq(int day)
    {
        CLPFShakeNumberFetchBoxRewardReq req = new CLPFShakeNumberFetchBoxRewardReq();
        req.day = day;
        netComponent.asyncRequest<CLPFShakeNumberFetchBoxRewardAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_SHAKENUMBER_FETCHBOX_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"day", day},
            });
        });
    }

    /// <summary>
    /// 福利猪获取信息请求
    /// </summary>
    public void SendWelfarePigQueryReq()
    {
        CLPFWelfarePigQueryInfoReq req = new CLPFWelfarePigQueryInfoReq();
        netComponent.asyncRequest<CLPFWelfarePigQueryInfoAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_WELFAREPIG_QUERY_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 福利猪领取每日锤子碎片请求
    /// </summary>
    public void SendWelfarePigFetchMaterialReq()
    {
        CLPFWelfarePigFetchMaterialReq req = new CLPFWelfarePigFetchMaterialReq();
        netComponent.asyncRequest<CLPFWelfarePigFetchMaterialAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_WELFAREPIG_FETCHMATERIAL_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 福利猪砸罐子请求
    /// </summary>
    public void SendWelfarePigBrokenReq()
    {
        CLPFWelfarePigBrokenReq req = new CLPFWelfarePigBrokenReq();
        netComponent.asyncRequest<CLPFWelfarePigBrokenAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_WELFAREPIG_BROKEN_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 福利猪搜一搜请求
    /// </summary>
    public void SendWelfarePigSearchReq()
    {
        CLPFWelfarePigSearchReq req = new CLPFWelfarePigSearchReq();
        netComponent.asyncRequest<CLPFWelfarePigSearchAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_WELFAREPIG_SEARCH_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查询投资炮倍信息请求
    /// </summary>
    public void SendInvestGunQueryInfoReq()
    {
        CLPFInvestGunQueryInfoReq req = new CLPFInvestGunQueryInfoReq();
        netComponent.asyncRequest<CLPFInvestGunQueryInfoAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_INVEST_GUN_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 领取投资炮倍奖励请求
    /// </summary>
    public void SendInvestGunFetchRewardReq(int gunValue)
    {
        CLPFInvestGunFetchRewardReq req = new CLPFInvestGunFetchRewardReq();
        req.gun_value = gunValue;
        netComponent.asyncRequest<CLPFInvestGunFetchRewardAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_INVEST_GUN_FETCH_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"gunValue", gunValue},
            });
        });
    }

    /// <summary>
    /// 查询出海保险信息请求
    /// </summary>
    public void SendInvestCostQueryInfoReq()
    {
        CLPFInvestCostQueryInfoReq req = new CLPFInvestCostQueryInfoReq();
        netComponent.asyncRequest<CLPFInvestCostQueryInfoAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_INVEST_COST_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 领取出海保险奖励请求
    /// </summary>
    public void SendInvestCostFetchRewardReq(int rewardId)
    {
        CLPFInvestCostFetchRewardReq req = new CLPFInvestCostFetchRewardReq();
        req.reward_id = rewardId;
        netComponent.asyncRequest<CLPFInvestCostFetchRewardAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_INVEST_COST_FETCH_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"rewardId", rewardId},
            });
        });
    }

    /// <summary>
    /// 查询常用的真实地址请求
    /// </summary>
    public void SendRealGoodsQueryAddressReq()
    {
        CLPFRealGoodsQueryAddressReq req = new CLPFRealGoodsQueryAddressReq();
        netComponent.asyncRequest<CLPFRealGoodsQueryAddressAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_REALGOODS_ADD_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 实物奖励下单请求
    /// </summary>
    public void SendRealGoodsCreateOrderReq(int goodsId, string name, string phone, string address)
    {
        CLPFRealGoodsCreateOrderReq req = new CLPFRealGoodsCreateOrderReq();
        req.goods_id = goodsId;
        req.real_name = name;
        req.phone = phone;
        req.address = address;
        netComponent.asyncRequest<CLPFRealGoodsCreateOrderAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_REALGOODS_CREATEORDER_ACK, this, rsp, new Dictionary<string, object>()
            {
                {"GoodsId", goodsId},
            });
        });
    }

    /// <summary>
    /// 查询实物奖励兑换纪录请求
    /// </summary>
    public void SendRealGoodsQueryExchangeLogReq()
    {
        CLPFRealGoodsQueryExchangeLogReq req = new CLPFRealGoodsQueryExchangeLogReq();
        netComponent.asyncRequest<CLPFRealGoodsQueryExchangeLogAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_REALGOODS_LOG_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查询已完成的新手引导标记数组请求
    /// </summary>
    public void SendGuideDataQueryReq()
    {
        CLPFGuideDataQueryReq req = new CLPFGuideDataQueryReq();
        netComponent.asyncRequest<CLPFGuideDataQueryAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_GUIDE_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 上报完成了某个新手引导标记
    /// </summary>
    public void SendGuideDataActRpt(int flag)
    {
        CLPFGuideDataActRpt rpt = new CLPFGuideDataActRpt();
        rpt.flag = flag;
        netComponent.send(rpt);
    }

    /// <summary>
    /// 使用弹头锁定鱼请求
    /// </summary>
    /// <param name="bombInfo">弹头信息</param>
    /// <param name="fishId">鱼ID</param>
    public void SendWarheadLockReq(ItemInfo bombInfo, int fishId)
    {
        CLFRWarheadLockReq req = new CLFRWarheadLockReq();
        req.item_id = bombInfo.ItemID;
        req.item_sub_id = bombInfo.ItemSubID;
        req.item_count = (int)bombInfo.ItemCount;
        req.fish_id = fishId;
        netComponent.asyncRequest<CLFRWarheadLockAck>(req, rsp =>
         {
             MessageCenter.Instance.SendMessage(MsgType.NET_WARHEAD_LOCK_ACK, this, rsp);
         });
    }

    /// <summary>
    /// 弹头爆炸请求
    /// </summary>
    public void SendWarheadBoomReq()
    {
        CLFRWarheadBoomReq req = new CLFRWarheadBoomReq();
        netComponent.asyncRequest<CLFRWarheadBoomAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_WARHEAD_BOOM_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查询房间总人数请求
    /// </summary>
    public void SendRoomUserCountSummaryReq()
    {
        CLFMRoomUserCountSummaryReq req = new CLFMRoomUserCountSummaryReq();
        netComponent.asyncRequest<CLFMRoomUserCountSummaryAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_ROOM_USERCOUNT_SUMMARY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查询房间详细人数请求
    /// </summary>
    public void SendRoomUserCountDetailReq(int configId, int startId, int count)
    {
        CLFMRoomUserCountDetailReq req = new CLFMRoomUserCountDetailReq();
        req.config_id = configId;
        req.start_room_id = startId;
        req.count = count;
        netComponent.asyncRequest<CLFMRoomUserCountDetailAck>(req, rsp =>
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_ROOM_USERCOUNT_DETAIL_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 查询房间内获取到的积分请求
    /// </summary>
    public void SendIntegralGainQueryReq()
    {
        CLFRIntegralGainQueryReq req = new CLFRIntegralGainQueryReq();
        netComponent.asyncRequest<CLFRIntegralGainQueryAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_INTEGRAL_GAIN_QUERY_ACK, this, rsp);
        });
    }

    /// <summary>
    /// 领取新手初始礼包请求
    /// </summary>
    public void SendFirstPackageFetchReq()
    {
        CLPFFirstPackageFetchReq req = new CLPFFirstPackageFetchReq();
        netComponent.asyncRequest<CLPFFirstPackageFetchAck>(req, rsp => 
        {
            MessageCenter.Instance.SendMessage(MsgType.NET_FIRST_PACKAGE_FETCH_ACK, this, rsp);
        });
    }
    #region 测试!!!
    //!!!测试用!!!召唤鱼潮
    public void SendFishTideForTestReq()
    {
        CLFRFishTideForTestReq req = new CLFRFishTideForTestReq();
        netComponent.asyncRequest<CLFRFishTideForTestAck>(req, rsp =>
         {
             Debug.Log($"召唤鱼潮：{rsp.errcode}");
         });
    }
    #endregion
}