
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

public class MsgType
{
    #region client message
    public const string CLIENT_PLAYER_CURRENCY_CHANGED      = "CLIENT_PLAYER_CURRENCY_CHANGED";     //金币变化
    public const string CLIENT_PLAYER_BINDCURRENCY_CHANGED  = "CLIENT_PLAYER_BINDCURRENCY_CHANGED"; //绑金变化
    public const string CLIENT_PLAYER_DIAMOND_CHANGED       = "CLIENT_PLAYER_DIAMOND_CHANGED";      //钻石变化
    public const string CLIENT_PLAYER_INTEGRAL_CHANGED      = "CLIENT_PLAYER_INTEGRAL_CHANGED";     //积分变化
    public const string CLIENT_PLAYER_ITEM_CHANGED          = "CLIENT_PLAYER_ITEM_CHANGED";         //道具变化
    public const string CLIENT_PLAYER_UNLOCKGUN_CHANGED     = "CLIENT_PLAYER_UNLOCKGUN_CHANGED";    //解锁最大炮倍变化
    public const string CLIENT_PLAYER_FORGE                 = "CLIENT_PLAYER_FORGE";                //锻造
    public const string CLIENT_PLAYER_HEADID                = "CLIENT_PLAYER_HEADID";               //头像
    public const string CLIENT_PLAYER_NICKNAME              = "CLIENT_PLAYER_NICKNAME";             //昵称
    public const string CLIENT_PLAYER_VIPLEVEL              = "CLIENT_PLAYER_VIPLEVEL";             //vip等级变化
    public const string CLIENT_PLAYER_GUILDID               = "CLIENT_PLAYER_GUILDID";              //公会id变动
    public const string CLIENT_SIGN_QUERY_ACK               = "CLIENT_SIGN_QUERY_ACK";              //查询签到数据回应   
    public const string CLIENT_SIGNED_SHOW                  = "CLIENT_SIGNED_SHOW";                 //签到成功
    public const string CLIENT_PLAYER_VIPLOTTERY_QUERY      = "CLIENT_PLAYER_VIPLOTTERY_CHANGED";   //vip抽奖次数变化
    public const string CLIENT_PLAYER_VIPLOTTERY_ACK        = "CLIENT_PLAYER_VIPLOTTERY_ACK";       //vip抽奖结果
    public const string CLIENT_RANKINFO_REFRESH             = "CLIENT_RANKINFO_REFRESH";            //排行榜界面刷新
    public const string CLIENT_ISMATCHING_CHANGE            = "CLIENT_ISMATCHING_CHANGE";           //比赛状态改变
    public const string CLIENT_ALL_MAIL_ACK                 = "CLIENT_ALL_MAIL_ACK";                //查询所有邮件ID回应
    public const string CLIENT_MAIL_REFRESH                 = "CLIENT_MAIL_REFRESH";                //邮件刷新
    public const string CLIENT_MAIL_OPEN                    = "CLIENT_MAIL_OPEN";                   //点击打开邮件
    public const string CLIENT_MAIL_FETCH                   = "CLIENT_MAIL_FETCH";                  //领取邮件物品
    public const string CLIENT_GUILD_LIST_REFRESH           = "CLIENT_GUILD_LIST_REFRESH";          //刷新公会推荐列表
    //public const string CLIENT_GUILD_LIST_CLOSE             = "CLIENT_GUILD_LIST_CLOSE";            //加入/创建公会后关闭公会推荐列表
    public const string CLIENT_GUILD_BADGE_REFRESH          = "CLIENT_GUILD_BADGE_REFRESH";         //公会徽章变动
    public const string CLIENT_GUILD_MEMBER_REFRESH         = "CLIENT_GUILD_MEMBER_REFRESH";        //公会成员列表刷新
    public const string CLIENT_GUILD_SEARCH_REFRESH         = "CLIENT_GUILD_SEARCH_REFRESH";        //查询公会信息刷新
    public const string CLIENT_GUILD_BAGITEM_REFRESH        = "CLIENT_GUILD_BAGITEM_REFRESH";       //刷新仓库物品
    public const string CLIENT_GUILD_BAGLOG_REFRESH         = "CLIENT_GUILD_BAGLOG_REFRESH";        //刷新仓库日志
    public const string CLIENT_GUILD_JOINLIST_REFRESH       = "CLIENT_GUILD_JOINLIST_REFRESH";      //公会申请列表刷新
    public const string CLIENT_GUILD_UI_REFRESH             = "CLIENT_GUILD_UI_REFRESH";            //公会界面刷新
    public const string CLIENT_GUILD_TRYHANDLEJOIN          = "CLIENT_GUILD_TRYHANDLEJOIN";         //界面点击处理公会加入请求
    public const string CLIENT_GUILD_HANDLEJOIN_ACK         = "CLIENT_GUILD_HANDLEJOIN_ACK";        //处理公会请求回应
    public const string CLIENT_GUILD_EXIT_ACK               = "CLIENT_GUILD_EXIT_ACK";              //公会退出成功回应
    public const string CLIENT_GUILD_MODIFYINFO_ACK         = "CLIENT_GUILD_MODIFYINFO_ACK";        //修改公会信息成功回应
    public const string CLIENT_GUILD_QUERYREDPACKET_ACK     = "CLIENT_GUILD_QUERYREDPACKET_ACK";    //获取公会红包信息
    public const string CLIENT_GUILD_PACKETRANK_REFRESH     = "CLIENT_GUILD_PACKETRANK_REFRESH";    //公会红包排行榜刷新
    public const string CLIENT_GUILD_ACTREDPACKET_ACK       = "CLIENT_GUILD_ACTREDPACKET_ACK";      //公会抢红包回应
    public const string CLIENT_GUILD_QUERYWELFARE_ACK       = "CLIENT_GUILD_QUERYWELFARE_ACK";      //查询会长福利回应
    public const string CLIENT_GUILD_FETCHWELFARE_ACK       = "CLIENT_GUILD_FETCHWELFARE_ACK";      //领取会长福利回应
    public const string CLIENT_PLAYER_LEVEL                 = "CLIENT_PLAYER_LEVEL";                //玩家等级变化
    public const string CLIENT_PLAYER_LEVELEXP              = "CLIENT_PLAYER_LEVELEXP";             //玩家经验变化
    public const string CLIENT_SHOP_RECHARGE                = "CLIENT_SHOP_RECHARGE";               //商城充值事件
    public const string CLIENT_PAY_REQ                      = "CLIENT_PAY_REQ";                     //调用SDK支付请求
    public const string CLIENT_AUTHORIZE_REQ                = "CLIENT_AUTHORIZE_REQ";               //调用SDK授权请求
    public const string CLIENT_AUTHORIZE_ACK                = "CLIENT_AUTHORIZE_ACK";               //SDK授权回调
    public const string CLIENT_MONTHCARD_REFRESH            = "CLIENT_MONTHCARD_REFRESH";           //月卡界面刷新
    public const string CLIENT_FIRSTCHARGE_REFRESH          = "CLIENT_FIRSTCHARGE_REFRESH";         //首充界面刷新
    public const string CLIENT_SHOP_REFRESH                 = "CLIENT_SHOP_REFRESH";                //商城界面刷新
    public const string CLIENT_DAILYCHARGE_REFRESH          = "CLIENT_DAILYCHARGE_REFRESH";         //每日充值界面刷新
    public const string CLIENT_RELIEF_REFRESH               = "CLIENT_RELIEF_REFRESH";              //救济金界面刷新
    public const string CLIENT_TASK_QUERY_ACK               = "CLIENT_TASK_QUERY_ACK";              //查询任务回应
    public const string CLIENT_TASK_ACHIEVE_QUERY_ACK       = "CLIENT_TASK_ACHIEVE_QUERY_ACK";      //查询成就任务回应
    public const string CLIENT_TASK_REFRESH                 = "CLIENT_TASK_REFRESH";                //任务界面刷新
    public const string CLIENT_TASK_ACHIEVE_REFRESH         = "CLIENT_TASK_ACHIEVE_REFRESH";        //成就任务界面刷新
    public const string CLIENT_WELFARE_GOTOPAGE             = "CLIENT_WELFARE_GOTOPAGE";            //福利中心页面跳转
    public const string CLIENT_WELFARE_GOTOOTHER            = "CLIENT_WELFARE_GOTOOTHER";           //福利中心任务跳转
    public const string CLIENT_SHAKENUMBER_QUERY_ACK        = "CLIENT_SHAKENUMBER_QUERY_ACK";       //摇数字查询回应
    public const string CLIENT_SHAKENUMBER_REFRESH          = "CLIENT_SHAKENUMBER_REFRESH";         //摇数字界面刷新
    public const string CLIENT_REDDOT_FORGE_REFRESH         = "CLIENT_REDDOT_FORGE_REFRESH";        //锻造红点刷新
    public const string CLIENT_REDDOT_MAIL_REFRESH          = "CLIENT_REDDOT_MAIL_REFRESH";         //邮件红点刷新
    public const string CLIENT_REDDOT_ANNOUNCEMENT_REFRESH  = "CLIENT_REDDOT_ANNOUNCEMENT_REFRESH"; //公告红点刷新
    public const string CLIENT_WELFAREPIG_QUERY_ACK         = "CLIENT_WELFAREPIG_QUERY_ACK";        //福利猪获取信息回应
    public const string CLIENT_WELFAREPIG_FETCHMATERIAL_ACK = "CLIENT_WELFAREPIG_FETCHMATERIAL_ACK";//福利猪领取每日锤子碎片回应
    public const string CLIENT_WELFAREPIG_BROKEN_ACK        = "CLIENT_WELFAREPIG_BROKEN_ACK";       //福利猪砸罐子回应
    public const string CLIENT_WELFAREPIG_SEARCH_ACK        = "CLIENT_WELFAREPIG_SEARCH_ACK";       //福利猪搜一搜回应
    public const string CLIENT_INVEST_GUN_QUERY_ACK         = "CLIENT_INVEST_GUN_QUERY_ACK";        //查询投资炮倍信息回应
    public const string CLIENT_INVEST_GUN_BUY_NTF           = "CLIENT_INVEST_GUN_BUY_NTF";          //投资炮倍购买通知
    public const string CLIENT_INVEST_GUN_FETCH_ACK         = "CLIENT_INVEST_GUN_FETCH_ACK";        //领取投资炮倍奖励回应
    public const string CLIENT_INVEST_COST_QUERY_ACK        = "CLIENT_INVEST_COST_QUERY_ACK";       //查询出海保险信息回应
    public const string CLIENT_INVEST_COST_BUY_NTF          = "CLIENT_INVEST_COST_BUY_NTF";         //出海保险购买通知
    public const string CLIENT_INVEST_COST_FETCH_ACK        = "CLIENT_INVEST_COST_FETCH_ACK";       //领取出海保险奖励回应
    public const string CLIENT_REALGOODS_ADD_QUERY_ACK      = "CLIENT_REALGOODS_ADD_QUERY_ACK";     //查询常用的真实地址回应
    public const string CLIENT_REALGOODS_CREATEORDER_ACK    = "CLIENT_REALGOODS_CREATEORDER_ACK";   //实物奖励下单回应
    public const string CLIENT_REALGOODS_LOG_QUERY_ACK      = "CLIENT_REALGOODS_LOG_QUERY_ACK";     //实物奖励兑换纪录回应
    public const string CLIENT_GUIDE_QUERY_ACK              = "CLIENT_GUIDE_QUERY_ACK";             //查询已完成的新手引导标记数组回应
    public const string CLIENT_BOMB_TARGET_SELECT           = "CLIENT_BOMB_TARGET_SELECT";          //弹头选中目标
    public const string CLIENT_ROOM_USERCOUNT_SUMMARY_ACK   = "CLIENT_ROOM_USERCOUNT_SUMMARY_ACK";  //查询房间总人数回应
    public const string CLIENT_ROOM_USERCOUNT_DETAIL_ACK    = "CLIENT_ROOM_USERCOUNT_DETAIL_ACK";   //查询房间详细人数回应
    public const string CLIENT_JSON_UPDATE_ACK              = "CLIENT_JSON_UPDATE_ACK";             //从线上更新配置完毕
    public const string CLIENT_CANCEL_AUTO_LOGIN            = "CLIENT_CANCEL_AUTO_LOGIN";           //取消自动登录
    public const string CLIENT_CDKEY_FETCH_REWARD_ACK       = "CLIENT_CDKEY_FETCH_REWARD_ACK";      //激活码领取奖励回应
    public const string CLIENT_ACCOUNT_BIND_STATE_ACK       = "CLIENT_ACCOUNT_BIND_STATE_ACK";      //账号绑定状态回应
    public const string CLIENT_ACCOUNT_PHONE_BIND_ACK       = "CLIENT_ACCOUNT_PHONE_BIND_ACK";      //账号手机绑定回应
    public const string CLIENT_ACCOUNT_PHONE_CHANGE1_ACK    = "CLIENT_ACCOUNT_PHONE_CHANGE1_ACK";   //账号手机更换回应1
    public const string CLIENT_ACCOUNT_PHONE_CHANGE2_ACK    = "CLIENT_ACCOUNT_PHONE_CHANGE2_ACK";   //账号手机更换回应2
    public const string CLIENT_ACCOUNT_UNIFORM_BIND_ACK     = "CLIENT_ACCOUNT_UNIFORM_BIND_ACK";    //账号统一绑定回应
    public const string CLIENT_ACCOUNT_UNIFORM_UNBIND_ACK   = "CLIENT_ACCOUNT_UNIFORM_UNBIND_ACK";  //账号统一解绑回应
    public const string CLIENT_BANK_PASSWORD_INIT_ACK       = "CLIENT_BANK_PASSWORD_INIT_ACK";      //金库密码初始化回应
    public const string CLIENT_BANK_PASSWORD_VERIFY_ACK     = "CLIENT_BANK_PASSWORD_VERIFY_ACK";    //金库密码验证回应
    public const string CLIENT_BANK_PASSWORD_MODIFY_ACK     = "CLIENT_BANK_PASSWORD_MODIFY_ACK";    //金库密码修改回应
    public const string CLIENT_BANK_PASSWORD_RESET_ACK      = "CLIENT_BANK_PASSWORD_RESET_ACK";     //金库密码重置回应
    public const string CLIENT_BANK_ITEM_QUERY_ACK          = "CLIENT_BANK_ITEM_QUERY_ACK";         //金库物品查询回应
    public const string CLIENT_BANK_ITEM_STORE_ACK          = "CLIENT_BANK_ITEM_STORE_ACK";         //金库物品存入回应
    public const string CLIENT_BANK_ITEM_FETCH_ACK          = "CLIENT_BANK_ITEM_FETCH_ACK";         //金库物品取出回应
    public const string CLIENT_BANK_ITEM_SEND_ACK           = "CLIENT_BANK_ITEM_SEND_ACK";          //金库物品赠送回应
    public const string CLIENT_BANK_ITEM_LOG_QUERY_ACK      = "CLIENT_BANK_ITEM_LOG_QUERY_ACK";     //金库物品日志查询回应
    public const string CLIENT_PLAYER_NICKNAME_QUERY_ACK    = "CLIENT_PLAYER_NICKNAME_QUERY_ACK";   //查询玩家昵称回应
    public const string CLIENT_INNERGAME_OPENCLOSE          = "CLIENT_INNERGAME_OPENCLOSE";         //打开关闭小游戏
    #endregion

    #region net message
    public const string NET_RECEIVE_DATA                    = "NET_RECEIVE_DATA";                   //网络接收到数据
    public const string NET_CONNECT                         = "NET_CONNECT";                        //网络链接状态
    public const string NET_DISCONNECT_NTF                  = "NET_DISCONNECT_NTF";                 //网络断开通知
    public const string NET_HAND_ACK                        = "NET_HAND_ACK";                       //握手回应
    public const string NET_LOGIN_PLATFORM_ACK              = "NET_LOGIN_PLATFORM_ACK";             //登录大厅回应
    public const string NET_ACCESSSERVICE_ACK               = "NET_ACCESSSERVICE_ACK";              //连接或断开服务器回应
    public const string NET_ENTERSITE_ACK                   = "NET_ENTERSITE_ACK";                  //加入玩法
    public const string NET_EXITSITE_ACK                    = "NET_EXITSITE_ACK";                   //退出玩法
    public const string NET_SHOP_BUYCOUNT_QUERY             = "NET_SHOP_BUYCOUNT_QUERY";            //商城购买次数查询
    public const string NET_ITEM_BUY_ACK                    = "NET_ITEM_BUY_ACK";                   //单独购买物品
    public const string NET_SHOP_BUY                        = "NET_SHOP_BUY";                       //商城购买
    public const string NET_QUERY_SIGN                      = "NET_QUERY_SIGN";                     //签到查询
    public const string NET_ACT_SIGN                        = "NET_ACT_SIGN";                       //签到
    public const string NET_VIPLOTTERY_QUERY                = "NET_VIPWHEEL_QUERY";                 //查询VIP抽奖请求
    public const string NET_VIPLOTTERY_ACT                  = "NET_VIPWHEEL_ACT";                   //执行VIP抽奖请求
    public const string NET_FORGE                           = "NET_FORGE";                          //锻造
    public const string NET_RANKINFO_ACK                    = "NET_RANKINFO_ACK";                   //请求排行榜数据回应
    public const string NET_MATCHRANK                       = "NET_MATCHRANK";                      //比赛排行榜信息
    public const string NET_QUERY_ALL_MAIL                  = "NET_QUERY_ALL_MAIL";                 //查询所有邮件ID
    public const string NET_MAIL_CONTENT_ACK                = "NET_MAIL_CONTENT_ACK";               //批量邮件内容回应
    public const string NET_MAIL_ACCESS_ACK                 = "NET_MAIL_ACCESS_ACK";                //查看邮件回应
    public const string NET_MAIL_FETCH_ACK                  = "NET_MAIL_FETCH_ACK";                 //领取邮件物品回应
    public const string NET_MAIL_REMOVE_ACK                 = "NET_MAIL_REMOVE_ACK";                //删除邮件回应
    public const string NET_MAIL_ARRIVE_NTF                 = "NET_MAIL_ARRIVE_NTF";                //新邮件通知
    public const string NET_GUILD_RECOMMENDLIST_ACK         = "NET_GUILD_RECOMMENDLIST_ACK";        //获取公会推荐列表
    public const string NET_GUILD_JOIN_ACK                  = "NET_GUILD_JOIN_ACK";                 //加入公会
    public const string NET_GUILD_QUICKJOIN_ACK             = "NET_GUILD_QUICKJOIN_ACK";            //快速加入公会
    public const string NET_GUILD_SEARCH_ACK                = "NET_GUILD_SEARCH_ACK";               //查询公会
    public const string NET_GUILD_CREATE_ACK                = "NET_GUILD_CREATE_ACK";               //创建公会
    public const string NET_CUILD_BAGQUERY_ACK              = "NET_CUILD_BAGQUERY_ACK";             //获取公会仓库信息
    public const string NET_GUILD_BAGQUERYLOG_ACK           = "NET_GUILD_BAGQUERYLOG_ACK";          //获取公会仓库日志信息
    public const string NET_GUILD_BAGSTORE_ACK              = "NET_GUILD_BAGSTORE_ACK";             //仓库捐赠
    public const string NET_GUILD_BAGFETCH_ACK              = "NET_GUILD_BAGFETCH_ACK";             //仓库分配
    public const string NET_GUILD_QUERYINFO_ACK             = "NET_GUILD_QUERYINFO_ACK";            //获取公会信息回应
    public const string NET_GUILD_MODIFYINFO_ACK            = "NET_GUILD_MODIFYINFO_ACK";           //修改公会信息回应
    public const string NET_GUILD_KICK_ACK                  = "NET_GUILD_KICK_ACK";                 //公会踢出成员回应
    public const string NET_GUILD_QUERYJOIN_ACK             = "NET_GUILD_QUERYJOIN_ACK";            //获取公会申请列表回应
    public const string NET_GUILD_HANDLEJOIN_ACK            = "NET_GUILD_HANDLEJOIN_ACK";           //处理公会申请回应
    public const string NET_GUILD_EXIT_ACK                  = "NET_GUILD_EXIT_ACK";                 //退出公会成功回应
    public const string NET_GUILD_UPGRADE_ACK               = "NET_GUILD_UPGRADE_ACK";              //公会升级回应
    public const string NET_GUILD_QUERY_REDPACKETINFO_ACK   = "NET_GUILD_QUERY_REDPACKETINFO_ACK";  //获取公会红包信息回应
    public const string NET_GUILD_QUERY_REDPACKETRANK_ACK   = "NET_GUILD_QUERY_REDPACKETRANK_ACK";  //获取公会红包排行榜回应
    public const string NET_GUILD_ACTREDPACKET_ACK          = "NET_GUILD_ACTREDPACKET_ACK";         //公会抢红包回应
    public const string NET_GUILD_QUERYWELFARE_ACK          = "NET_GUILD_QUERYWELFARE_ACK";         //查询会长福利回应
    public const string NET_GUILD_FETCHWELFARE_ACK          = "NET_GUILD_FETCHWELFARE_ACK";         //领取会长福利回应
    public const string NET_ENTERSERVER_NTF                 = "NET_ENTERSERVER_NTF";                //登录服务器
    public const string NET_BONUSWHEEL_ACK                  = "NET_BONUSWHEEL_ACK";                 //赏金鱼抽奖回应
    public const string NET_MONTHCARD_FETCH_ACK             = "NET_MONTHCARD_FETCH_ACK";            //领取月卡奖励回应
    public const string NET_RECHARGEDAILY_QUERY_ACK         = "NET_RECHARGEDAILY_QUERY_ACK";        //每日充值查询回应
    public const string NET_RELIEFGOLD_FETCH_ACK            = "NET_RELIEFGOLD_FETCH_ACK";           //领取救济金回应
	public const string NET_MESSAGEBROADCAST_NTF            = "NET_MESSAGEBROADCAST_NTF";           //消息广播
	public const string NET_TASK_QUERY_ACK                  = "NET_TASK_QUERY_ACK";                 //查询任务回应
    public const string NET_TASK_ACHIEVE_QUERY_ACK          = "NET_TASK_ACHIEVE_QUERY_ACK";         //查询成就任务回应
    public const string NET_TASK_FETCH_ACK                  = "NET_TASK_FETCH_ACK";                 //领取任务奖励回应
    public const string NET_TASK_ACHIEVE_FETCH_ACK          = "NET_TASK_ACHIEVE_FETCH_ACK";         //领取成就任务奖励回应
    public const string NET_TASK_ACTIVE_FETCH_ACK           = "NET_TASK_ACTIVE_FETCH_ACK";          //领取活跃值奖励回应
    public const string NET_MODIFYNICKNAME_ACK              = "NET_MODIFYNICKNAME_ACK";             //设置昵称
    public const string NET_SHAKENUMBER_QUERY_ACK           = "NET_SHAKENUMBER_QUERY_ACK";          //摇数字获取信息回应
    public const string NET_SHAKENUMBER_ACT_ACK             = "NET_SHAKENUMBER_ACT_ACK";            //摇数字回应
    public const string NET_SHAKENUMBER_FETCHREWARD_ACK     = "NET_SHAKENUMBER_FETCHREWARD_ACK";    //领取摇到的金币奖励回应
    public const string NET_SHAKENUMBER_FETCHBOX_ACK        = "NET_SHAKENUMBER_FETCHBOX_ACK";       //领取摇到数字后的宝箱礼包回应
    public const string NET_ANNOUNCEMENT_CHANGE_NTF         = "NET_ANNOUNCEMENT_CHANGE_NTF";        //公告变动通知
    public const string NET_WELFAREPIG_QUERY_ACK            = "NET_WELFAREPIG_QUERY_ACK";           //福利猪获取信息回应
    public const string NET_WELFAREPIG_FETCHMATERIAL_ACK    = "NET_WELFAREPIG_FETCHMATERIAL_ACK";   //福利猪领取每日锤子碎片回应
    public const string NET_WELFAREPIG_BROKEN_ACK           = "NET_WELFAREPIG_BROKEN_ACK";          //福利猪砸罐子回应
    public const string NET_WELFAREPIG_SEARCH_ACK           = "NET_WELFAREPIG_SEARCH_ACK";          //福利猪搜一搜回应
    public const string NET_INVEST_GUN_QUERY_ACK            = "NET_INVEST_GUN_QUERY_ACK";           //查询投资炮倍信息回应
    public const string NET_INVEST_GUN_FETCH_ACK            = "NET_INVEST_GUN_FETCH_ACK";           //领取投资炮倍奖励回应
    public const string NET_INVEST_COST_QUERY_ACK           = "NET_INVEST_COST_QUERY_ACK";          //查询出海保险信息回应
    public const string NET_INVEST_COST_FETCH_ACK           = "NET_INVEST_COST_FETCH_ACK";          //领取出海保险奖励回应
    public const string NET_REALGOODS_ADD_QUERY_ACK         = "NET_REALGOODS_ADD_QUERY_ACK";        //查询常用的真实地址回应
    public const string NET_REALGOODS_CREATEORDER_ACK       = "NET_REALGOODS_CREATEORDER_ACK";      //实物奖励下单回应
    public const string NET_REALGOODS_LOG_QUERY_ACK         = "NET_REALGOODS_LOG_QUERY_ACK";        //实物奖励兑换纪录回应
    public const string NET_GUIDE_QUERY_ACK                 = "NET_GUIDE_QUERY_ACK";                //查询已完成的新手引导标记数组回应
    public const string NET_WARHEAD_LOCK_ACK                = "NET_WARHEAD_LOCK_ACK";               //使用弹头锁定鱼回应
    public const string NET_WARHEAD_BOOM_ACK                = "NET_WARHEAD_BOOM_ACK";               //弹头爆炸回应
    public const string NET_ROOM_USERCOUNT_SUMMARY_ACK      = "NET_ROOM_USERCOUNT_SUMMARY_ACK";     //查询房间总人数回应
    public const string NET_ROOM_USERCOUNT_DETAIL_ACK       = "NET_ROOM_USERCOUNT_DETAIL_ACK";      //查询房间详细人数回应
    public const string NET_INTEGRAL_GAIN_QUERY_ACK         = "NET_INTEGRAL_GAIN_QUERY_ACK";        //查询房间内获取到的积分回应
    public const string NET_FIRST_PACKAGE_FETCH_ACK         = "NET_FIRST_PACKAGE_FETCH_ACK";        //领取新手初始礼包回应
    public const string NET_MUILTIPLE_HIT_CHANGE_ACK        = "NET_MULTIPLE_HIT_CHANGE_ACK";        //倍击倍率切换回应
    public const string NET_BOSS_NEXT_APPEAR_TIME_ACK       = "NET_BOSS_NEXT_APPEAR_TIME_ACK";      //boss下次出现的时间回应
    public const string NET_BOSS_RANK_ACK                   = "NET_BOSS_RANK_ACK";                  //获取Boss排行榜回应
    public const string NET_CDKEY_FETCH_REWARD_ACK          = "NET_CDKEY_FETCH_REWARD_ACK";         //激活码领取奖励回应
    public const string NET_ACCOUNT_BIND_STATE_ACK          = "NET_ACCOUNT_BIND_STATE_ACK";         //账号绑定状态回应
    public const string NET_ACCOUNT_PHONE_BIND_ACK          = "NET_ACCOUNT_PHONE_BIND_ACK";         //账号手机绑定回应
    public const string NET_ACCOUNT_PHONE_CHANGE1_ACK       = "NET_ACCOUNT_PHONE_CHANGE1_ACK";      //账号手机更换回应1
    public const string NET_ACCOUNT_PHONE_CHANGE2_ACK       = "NET_ACCOUNT_PHONE_CHANGE2_ACK";      //账号手机更换回应2
    public const string NET_ACCOUNT_UNIFORM_BIND_ACK        = "NET_ACCOUNT_UNIFORM_BIND_ACK";       //账号统一绑定回应
    public const string NET_ACCOUNT_UNIFORM_UNBIND_ACK      = "NET_ACCOUNT_UNIFORM_UNBIND_ACK";     //账号统一解绑回应
    public const string NET_BANK_PASSWORD_INIT_ACK          = "NET_BANK_PASSWORD_INIT_ACK";         //金库密码初始化回应
    public const string NET_BANK_PASSWORD_VERIFY_ACK        = "NET_BANK_PASSWORD_VERIFY_ACK";       //金库密码验证回应
    public const string NET_BANK_PASSWORD_MODIFY_ACK        = "NET_BANK_PASSWORD_MODIFY_ACK";       //金库密码修改回应
    public const string NET_BANK_PASSWORD_RESET_ACK         = "NET_BANK_PASSWORD_RESET_ACK";        //金库密码重置回应
    public const string NET_BANK_ITEM_QUERY_ACK             = "NET_BANK_ITEM_QUERY_ACK";            //金库物品查询回应
    public const string NET_BANK_ITEM_STORE_ACK             = "NET_BANK_ITEM_STORE_ACK";            //金库物品存入回应
    public const string NET_BANK_ITEM_FETCH_ACK             = "NET_BANK_ITEM_FETCH_ACK";            //金库物品取出回应
    public const string NET_BANK_ITEM_SEND_ACK              = "NET_BANK_ITEM_SEND_ACK";             //金库物品赠送回应
    public const string NET_BANK_ITEM_LOG_QUERY_ACK         = "NET_BANK_ITEM_LOG_QUERY_ACK";        //金库物品日志查询回应
    public const string NET_PLAYER_NICKNAME_QUERY_ACK       = "NET_PLAYER_NICKNAME_QUERY_ACK";      //查询玩家昵称回应
    #endregion

    #region room message
    public const string ROOM_FISH_CREATE                    = "ROOM_FISH_CREATE";                   //生成鱼
    public const string ROOM_FISH_DESTROY                   = "ROOM_FISH_DESTROY";                  //销毁鱼
    public const string ROOM_REMOVE_FISH                    = "ROOM_REMOVE_FISH";                   //移除鱼
    public const string ROOM_FUNCTIONAL_REMOVE_FISH         = "ROOM_FUNCTIONAL_REMOVE_FISH";        //特殊功能移除鱼
    public const string ROOM_FISH_CREATE_GOLD               = "ROOM_FISH_CREATE_GOLD";              //特殊功能弹出金币效果
    public const string ROOM_PLAYER_JOIN                    = "ROOM_PLAYER_JOIN";                   //玩家加入
    public const string ROOM_PLAYER_LEAVE                   = "ROOM_PLAYER_LEAVE";                  //玩家离开
    public const string ROOM_BULLET_CREATE                  = "ROOM_BULLET_CREATE";                 //生成子弹
    public const string ROOM_BULLET_DESTROY                 = "ROOM_BULLET_DESTROY";                //销毁子弹
    public const string ROOM_BULLET_UPDATE_INFO             = "ROOM_BULLET_UPDATE_INFO";            //虚拟子弹转变为实体子弹
    public const string ROOM_PLAYER_CURRENCY_CHANGED        = "ROOM_PLAYER_CURRENCY_CHANGED";       //渔场金币变化
    public const string ROOM_PLAYER_DIAMOND_CHANGED         = "ROOM_PLAYER_DIAMOND_CHANGED";        //渔场钻石变化
    public const string ROOM_GUN_ROTATION_CHANGED           = "ROOM_GUN_ROTATION_CHANGED";          //炮台的转向
    public const string ROOM_PLAYER_GUNVALUE_CHANGED        = "ROOM_PLAYER_GUNVALUE_CHANGED";       //炮台值变化
    public const string ROOM_PLAYER_USEITEM                 = "ROOM_PLAYER_USEITEM";                //玩家使用物品
    public const string ROOM_FISH_WHEEL1                    = "ROOM_FISH_WHEEL1";                   //单转盘鱼死亡
    public const string ROOM_FISH_WHEEL2                    = "ROOM_FISH_WHEEL2";                   //双转盘鱼死亡
    public const string ROOM_PLAYER_SKILL                   = "ROOM_PLAYER_SKILL";                  //技能
    public const string ROOM_BONUS_POOL_CHANGED             = "ROOM_BONUS_POOL_CHANGED";            //奖金鱼的鱼池变化
    public const string ROOM_BONUS_WHEEL_ACK                = "ROOM_BONUS_WHEEL_ACK";               //奖金鱼抽奖
    public const string ROOM_PLAYER_GUN_CHANGED             = "ROOM_PLAYER_GUN_CHANGED";            //玩家切换炮台
    public const string ROOM_MATCH_OVER                     = "ROOM_MATCH_OVER";                    //比赛结束
    public const string ROOM_MATCH_BULLETS_LEFT             = "ROOM_MATCH_BULLETS_LEFT";            //比赛子弹变化
    public const string ROOM_MATCH_GAME_INTEGRAL            = "ROOM_MATCH_GAME_INTEGRAL";           //比赛积分变化
    public const string ROOM_SWITCHBACKGROUND_NTF           = "ROOM_SWITCHBACKGROUND_NTF";          //切换背景
    public const string ROOM_FISHTIDE_NTF                   = "ROOM_FISHTIDE_NTF";                  //鱼潮来袭
    public const string ROOM_TRYGET_RELIEF                  = "ROOM_TRYGET_RELIEF";                 //渔场内尝试领取救济金
    public const string ROOM_FISH_TALK                      = "ROOM_FISH_TALK";                     //鱼说话气泡
    public const string ROOM_BOSS_CREATE                    = "ROOM_BOSS_CREATE";                   //生成Boss
    public const string ROOM_BOSS_DESTROY                   = "ROOM_BOSS_DESTROY";                  //销毁Boss
    public const string ROOM_BOSS_CREATE_GOLD               = "ROOM_BOSS_CREATE_GOLD";              //Boss掉落金币效果
    public const string ROOM_BOSS_BLOOD_CHANGE              = "ROOM_BOSS_BLOOD_CHANGE";             //世界Boss血量变化
    public const string ROOM_AUTO_FIRE_CHANGE               = "ROOM_AUTO_FIRE_CHANGE";              //自动发炮切换
    public const string ROOM_MUILTIPLE_HIT_CHANGE           = "ROOM_MULTIPLE_HIT_CHANGE";           //倍击倍率切换
    public const string ROOM_BOSS_RANK_ACK                  = "ROOM_BOSS_RANK_ACK";                 //获取Boss排行榜回应
    public const string ROOM_BOSS_KILL_NTF                  = "ROOM_BOSS_KILL_NTF";                 //世界Boss击杀通知
    public const string ROOM_PENETRATE_FIRE_CHANGE          = "ROOM_PENETRATE_FIRE_CHANGE";         //穿透模式切换
    public const string ROOM_PENETRATE_CREATE_WEB           = "ROOM_PENETRATE_CREATE_WEB";          //穿透模式创建渔网
    public const string ROOM_MULTIPLE_FIRE_CHANGE           = "ROOM_MULTIPLE_FIRE_CHANGE";          //倍击模式切换
    public const string ROOM_CLONE_FIRE_CHANGE              = "ROOM_CLONE_FIRE_CHANGE";             //分身模式切换
    public const string ROOM_CLONE_FIRE_OTHER_CHANGE        = "ROOM_CLONE_FIRE_OTHER_CHANGE";       //其他玩家分身模式切换
    public const string ROOM_TRY_USE_LASER                  = "ROOM_TRY_USE_LASER";                 //尝试使用激光炮
    public const string ROOM_TRY_USE_LASER_SUCCESS          = "ROOM_TRY_USE_LASER_SUCCESS";         //尝试使用激光炮成功
    public const string ROOM_SET_BG_COLOR                   = "ROOM_SET_BG_COLOR";                  //设置背景颜色
    public const string ROOM_PLAY_LASER_UI                  = "ROOM_PLAY_LASER_UI";                 //播放激光炮提示
    public const string ROOM_PLAYER_LASER_ENERGY_CHANGE     = "ROOM_PLAYER_LASER_ENERGY_CHANGE";    //激光炮能量变化
    public const string ROOM_OTHER_USE_LAYER                = "ROOM_OTHER_USE_LAYER";               //其他玩家使用激光炮
    #endregion

    public const string CSHARP_RECEIVE_DATA                 = "CSHARP_RECEIVE_DATA";                //所有C#需转发至Lua的消息
}