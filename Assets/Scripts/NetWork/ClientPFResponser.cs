
using JBPROTO;

public class ClientPFResponser : ClientPFResponserBase
{
    //玩家道具更新
    public override void onRecv_CLPFItemCountChangeNtf(CLPFItemCountChangeNtf proto)
    {
        GameController.Instance.Player.SetItem(proto.items);
    }

    //资源强同步
    public override void onRecv_CLPFResSyncNtf(CLPFResSyncNtf proto)
    {
        GameController.Instance.Player.SyncResource(proto.diamond, proto.currency, proto.integral);
    }

    //玩家个人资源信息更新
    public override void onRecv_CLPFResChangedNtf(CLPFResChangedNtf proto)
    {
        //1 钻石 2 金币
        switch (proto.res_type)
        {
            case 1:
                //GameController.Instance.Player.SetDiamond(proto.res_value);
                GameController.Instance.Player.DeltaDiamond(proto.res_delta);
                break;
            case 2:
                GameController.Instance.Player.DeltaCurrency(proto.res_delta);
                //GameController.Instance.Player.SetCurrency(proto.res_value);
                break;
            case 3:
                GameController.Instance.Player.DeltaIntegral(proto.res_delta);
                break;
        }
    }

    //vip经验变动通知
    public override void onRecv_CLPFVipExpChangedNtf(CLPFVipExpChangedNtf proto)
    {
        GameController.Instance.Player.SetVipLevel(proto.vip_level, proto.vip_level_exp);
    }

    //充值到账通知
    public override void onRecv_CLPFRechargeSuccessNtf(CLPFRechargeSuccessNtf proto)
    {
        //ItemInfo[] itemInfos = null;
        //if (proto.item_len > 0)
        //{
        //    itemInfos = new ItemInfo[proto.item_len];
        //    for (int i = 0; i < proto.item_len; i++)
        //    {
        //        var item = proto.items[i];
        //        itemInfos[i] = new ItemInfo(item.item_id, item.item_sub_id, item.item_count);
        //    }
        //    ModuleManager.Instance.Get<CommonModule>().SetItemData(itemInfos);
        //}
        //switch (proto.content_type)
        //{
        //    case 1:
        //        ModuleManager.Instance.Get<MainModule>().SetShopBuyCountData(proto.content_id);
        //        break;
        //    case 2:
        //        ModuleManager.Instance.Get<MainModule>().MonthCardBuySuccess();
        //        break;
        //    case 3:
        //        ModuleManager.Instance.Get<MainModule>().FirstChargeBuySuccess();
        //        break;
        //    case 4:
        //        ModuleManager.Instance.Get<MainModule>().DailyChargeBuySuccess();
        //        break;
        //    case 5:
        //        ModuleManager.Instance.Get<MainModule>().InvestGunBuySuccess();
        //        break;
        //    case 6:
        //        ModuleManager.Instance.Get<MainModule>().InvestCostBuySuccess();
        //        break;
        //    default:
        //        break;
        //}
    }

    //新邮件通知
    public override void onRecv_CLPFMailArriveNtf(CLPFMailArriveNtf proto)
    {
        MessageCenter.Instance.SendMessage(MsgType.NET_MAIL_ARRIVE_NTF, this, proto);
    }

    //加入公会应答通知
    public override void onRecv_CLPFGuildJoinResponseNtf(CLPFGuildJoinResponseNtf proto)
    {
        var player = GameController.Instance.Player;
        player.SetGuildId(proto.guild_id);
    }

    //玩家经验变动通知
    public override void onRecv_CLPFLevelExpChangedNtf(CLPFLevelExpChangedNtf proto)
    {
        GameController.Instance.Player.SetLevelExp(proto.level_exp);
    }

    //玩家等级变动通知
    public override void onRecv_CLPFLevelUpNtf(CLPFLevelUpNtf proto)
    {
        GameController.Instance.Player.SetLevel(proto.level, proto.reward_array);
    }

    //系统消息
    public override void onRecv_CLPFMessageBroadcastNtf(CLPFMessageBroadcastNtf proto)
    {
        MessageCenter.Instance.SendMessage(MsgType.NET_MESSAGEBROADCAST_NTF, this, proto);
    }

    //公告变动通知
    public override void onRecv_CLPFAnnouncementChangedNtf(CLPFAnnouncementChangedNtf proto)
    {
        MessageCenter.Instance.SendMessage(MsgType.NET_ANNOUNCEMENT_CHANGE_NTF, this);
    }

    //客户端配置表变化通知
    public override void onRecv_CLPFClientConfigPublishNtf(CLPFClientConfigPublishNtf proto)
    {
        TableLoadHelper.LoadFromNet(SysDefines.OssUrl, proto.md5);
    }
}
