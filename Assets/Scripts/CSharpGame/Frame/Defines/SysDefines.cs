
/******************************************************************************
 * 
 *  Title:  捕鱼项目
 *
 *  Version:  1.0版
 *
 *  Description:
 *    1：系统常量
 *    2：全局枚举对象
 *    3：全局委托
 *
 *  Author:  WangXingXing
 *       
 *  Date:  2018
 * 
 ******************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

//全局委托
public delegate void StateChangeEvent(object sender, EnumObjectState newState, EnumObjectState oldState);
public delegate void MessageEvent(Message msg);
public delegate void OnTouchEventHandle(GameObject listener, object eventData, params object[] args);
public delegate void MethodAction(object args);


/// <summary>
/// 对象当前状态
/// </summary>
public enum EnumObjectState
{
    None,
    Initial,                 //初始化
    Loading,                 //装载中
    Ready,                   //准备结束
    Disabled,                //过去的
    Closing,                 //关闭
}

//ui界面类型
public enum EnumUIType
{
    None = -1,

    LuaUI,                              //所有热更界面共用

    AwardUI,                            //获奖界面
    ChangeGunUI,                        //切换炮台界面
    FirstChargeUI,                      //首充界面
    FishFreeDrawUI,                     //赏金鱼抽奖界面
    FishingSelectUI,                    //竞技场渔场选择界面
    FishingCommonUI,                    //普通竞技场界面
    FishingFreeMatchUI,                 //免费赛界面
    FishingRewardMatchUI,               //大奖赛界面
    ForgeUI,                            //锻造界面
    KnapsackUI,                         //背包界面
    LevelUpUI,                          //升级界面
    LoadingUI,                          //过度界面
    LoginUI,                            //登录界面
    MainUI,                             //主界面
    MessageBoxUI,                       //消息弹窗界面
    MonthCardUI,                        //月卡界面
    PersonalInfoUI,                     //个人信息界面
    RechargeUI,                         //商城界面
    SettingUI,                          //设置界面
    VIPUI,                              //VIP特权界面
    WelfareUI,                          //福利中心界面
    RankUI,                             //排行榜
    ArenaUI,                            //比赛场选择
    FreeMatchUI,                        //免费赛报名排行
    RewardMatchUI,                      //大奖赛报名排行
    VIPLottery,                         //VIP转盘
    MailUI,                             //邮件
    GuildListUI,                        //工会列表
    GuildHomeUI,                        //工会主界面
    GuildHallUI,                        //工会大厅
    GuildSetupUI,                       //创建工会设置
    GuildSelectBadgeUI,                 //选择工会徽章
    GuildMemberManagementUI,            //工会成员管理
    GuildApplicationListUI,             //工会申请界面
    GuildSetupUpgradeUI,                //工会修改界面
    GuildRedPacketMainUI,               //工会红包主界面
    GuildWarehouseUI,                   //工会仓库
    GuildRedPacketHelpUI,               //工会红包说明界面
    GuildRedPacketRankUI,               //工会红包排行
    GuildRedPacketGrabUI,               //工会红包拆开界面
    GuildChairmanWelfareUI,             //工会会长福利
    GuildChairmanWelfareHelpUI,         //工会会长福利说明
    GuildWarheadAllotUI,                //工会物品分配界面
    GuildWarehouseJournalUI,            //工会仓库日志
    NoticeUI,                           //公告
    InputBoxUI,                         //物品输入通用弹窗
    FishHandBookUI,                     //鱼百科
    FishingFreeMatchResultUI,           //免费赛成绩
    FishingRewardMatchResultUI,         //大奖赛成绩
    DailyChargeUI,                      //每日充值礼包
    SystemNoticeUI,                     //系统消息
    WelfarePigUI,                       //砸金猪
    WelfarePigAwardUI,                  //砸金猪奖励界面
    InvestUI,                           //投资界面
    ExchangePropDetailsUI,              //兑换实物详情
    ExchangeFillOrderUI,                //兑换实物玩家信息
    ExchangeRecordUI,                   //兑换记录
    NoviceGuideUI,                      //新手引导
    VIPPrivilegeUI,                     //VIP特权界面
    NuclearBombUI,                      //弹头目标选择界面
    RoomSelectUI,                       //选房间界面
    WaitResponseUI,                     //等待回应界面
    MessageLeaveFishUI,                 //退出渔场等待界面
    VIPLevelUpUI,                       //VIP升级界面
    SailGiftUI,                         //起航礼包
    UserAgreementUI,                    //用户协议
    PrivacyGuidelinesUI,                //用户隐私
    HotUpdateUI,                        //热更界面
}

//event事件类型
public enum EnumTouchEventType
{
    OnBeginDrag,
    OnCancel,
    OnDeselect,
    OnDrag,
    OnDrop,
    OnEndDrag,
    OnInitializePotentialDrag,
    OnMove,
    OnClick,
    OnDoubleClick,
    OnDown,
    OnEnter,
    OnExit,
    OnUp,
    OnScroll,
    OnSelect,
    OnSubmit,
    OnUpdateSelected,
}

//场景类型
public enum EnumSceneType
{
    None = 0,
    LoginScene,
    LoadingScene,
    MainScene,
    FishScene,
}

//UI打开效果
public enum EnumAnimationType
{
    None,
    Scale,
}

//服务器连接状态
public enum EnumNetConnectState
{
    None = 0,
    Error,
    Established,
    Disconnect,
}

//弹窗类型
public enum EnumMessageBoxType
{
    OK,
    OK_CANCEL,
}

//鱼的类型
public enum EnumFishType
{
    None = 0,
    Small,                   //小
    Midle,                   //中
    Big,                     //大
    Special,                 //特殊
    Gold,                    //金
    Boss,                    //Boss
    Max,
}

public enum EnumFishPicType
{
    None = 0,
    Common,                  //普通
    Bonus,                   //赏金
    Special,                 //特殊
    Max,                     //最大
}

//链接游戏的服务组Id
public enum EnumGameGroupType
{
    None = 0,
    Fish,                    //捕鱼
}

public enum EnumAccessServiceType
{
    None = 0,
    JoinGame,                //加入游戏
    QuitGame,                //退出游戏
}

//item的效果类型
public enum EnumItemEffectType
{
    None = 0,
    Lock,                    //锁定
    Frozen,                  //冰冻
    Summon,                  //召唤葫芦
    Rage,                    //狂暴卡
    Cloned,                  //分身卡
    Bomb,                    //弹头
}

//item的大类型分类
public enum EnumItemType
{
    None = -1,
    Platform,                //平台
    Fish,                    //捕鱼
}

//游戏玩法
public enum EnumSiteType
{
    None = 0,
    Fishing,
}

//捕鱼玩法的类型
public enum EnumFishingMatchType
{
    None = -1,
    Common,
    FreeMatch,
    RewardMatch,
}

//捕鱼玩法的房间类型
public enum EnumFishingRoomType
{
    None = 0,
    Common,
    FreeMatch,
    RewardMatch,
}

//按钮点击可变参数的键值类型
public enum EnumHashtableParamsType
{
    None = 0,
    Audio,
    LockAllClick,
    LockSelfClick,
}

public enum EnumNoticeType
{
    KillGetGold = 1,         //击杀得金币
    KillGetBomb,             //击杀得弹头
    DrawGetBomb,             //抽奖得弹头
}

//鱼死亡原因
public enum EnumCreateBombType
{
    CommonFish,              //普通鱼
    SlotMachineFish,         //老虎机鱼
    TurntableFish,           //转盘鱼
}

//爆金效果
public enum EnumFishEffect
{
    Common = 0,              //普通
    Roulette,                //轮盘
    Rich,                    //发财了
    Zillionaire,             //大富翁
}

/// <summary>
/// 玩家性别
/// </summary>
public enum EnumPlayerGenderType
{
    Unknown = 0,              //保密
    Male,                     //男
    Female,                   //女
}

//鱼死亡后需处理的效果
public enum EnumFishDieEvent
{
    None = -1,
    Die,                    //鱼死亡特效
    GoldPop,                //金币(积分)和数字弹出
    Bonus,                  //爆金
}

public class UIPathDefines
{
    public static string GetPrefabPathByType(EnumUIType uiType, string componentType)
    {
        var path = string.Format($"{SysDefines.UIPREFAB}{(string.IsNullOrEmpty(componentType) ? uiType.ToString() : componentType)}");
        var msg = string.Empty;
        if (uiType == EnumUIType.None)
            msg = string.Format($"没有该类型的预制:{uiType.ToString()}");
        if (!string.IsNullOrEmpty(msg))
            Debug.LogWarning(msg);
        return path;
    }

    public static Type GetUIScriptByType(EnumUIType uiType, string componentType)
    {
        var msg = string.Empty;
        var scriptType = Type.GetType(string.IsNullOrEmpty(componentType) ? uiType.ToString() : componentType);
        if (uiType == EnumUIType.None)
            msg = string.Format($"没有该类型对应的脚本:{uiType.ToString()}");
        if (!string.IsNullOrEmpty(msg))
            Debug.LogWarning(msg);
        return scriptType;
    }
}

public class SysDefines
{
    #region 只读变量
    //UI预设
    public const string UIPREFAB = "UIPrefab/";
    //UI小控件预设
    public const string UICONTROLSPREFAB = "UIControl/";
    //UI子页面预设
    public const string UISUBUIPREFAB = "UIPrefab/SubUI/";
    //icon 路径
    public const string UIICONPATH = "UI/";
    //item路径
    public const string UIITEM = UIICONPATH + "Icon/Item/";
    //头像路径
    public const string UIHEAD = UIICONPATH + "Icon/Head/";
    //排行标志
    public const string UIRANK = UIICONPATH + "Icon/Rank/";
    //徽章
    public const string UIBADGE = UIICONPATH + "Icon/Badge/";
    //box路径
    public const string UIBOX = UIICONPATH + "Icon/Box/";
    //鱼图鉴
    public const string UIHANDBOOK = UIICONPATH + "HandBook/";
    //鱼预制路径
    public const string FISH = "Fish/";
    //音频路径
    public const string AUDIO = "Audio/";
    //渔场背景图
    public const string FISHBACKGROUD = "FishBackgroud/";
    //单个角色渔场中存在最大的子弹数
    public const int BulletLimitNum = 40;
    //子弹发射CD，单位：毫秒/发
    public const ulong BulletShootCD = 250;
    //子弹速度，单位：单位/秒
    public const float BulletSpeed = 1000;
    //炮台长度，单位：像素
    public const float CannoGunLength = 130;
    //区服ID 1开发 2正式 3佩奇
    public const int ZoneId = 1;
    //自定义的主场景的按钮标签
    public const string MainBtnTag = "mainBtn";
    //自定义的捕鱼场景的按钮标签
    public const string FishingBtnTag = "fishingBtn";
    //自定义的主场景的红点标签
    public const string MainRedDotTag = "mainRedDot";
    //自定义的捕鱼场景的红点标签
    public const string FishingRedDotTag = "fishingRedDot";
    //产品版本号
    public const uint Version = 1;
    //昵称的默认的字符长度
    public const int NickNameLength = 12;
    //积分显示权重
    public const long ScoreViewPara = 10000;
    //大厅的游戏ID(热更新)
    public const int GameID_Hall = 1001;
    //捕鱼服务组Id
    public const int GroupId_Fish = 1;
    //小游戏读取线上配置表
    public const bool IsInnerGameLoadFromNet = true;
    #endregion

    #region 静态变量
    //射线获得指定的层级
    public static int FishLayer = 1 << LayerMask.NameToLayer("Fish");
    //是否检测过版本信息
    public static bool IsCheckVersion = false;
    public static string Ip = string.Empty;
    public static long Port;
    public static string OssUrl = string.Empty;
    public static string HotUpdateUrl = string.Empty;
    // 运行平台 1:IOS 2:ANDRIOD 3:WINDOWS 4:LINUX 5:MAC
    public static uint Platform;
    //登录方式 1游客 2三方平台 3QQ 4微信 5Facebook 6GooglePlay 7GameCenter
    public static byte LoginType = 0;
    //登录标识
    public static string LoginToken;
    //加入玩法
    //public static EnumSiteType SiteId = EnumSiteType.None;
    public static int SiteId = -1;
    //加入房间的ID
    public static int RoomConfigID = 0;
    //是否断开连接
    public static bool IsDisconnect;
    //预加载界面 大小大于1M的都加入预加载
    public static EnumUIType[] preloadUIArray = new EnumUIType[]
    {
        EnumUIType.FishingSelectUI,
        EnumUIType.InvestUI,
        EnumUIType.RechargeUI,
        EnumUIType.RankUI,
        EnumUIType.WelfarePigUI,
        EnumUIType.WelfareUI
    };

    public static string[] preloadPrefab = new string[]
    {
        "eff_fishdie",
        "eff_fishdie_small",
        "fishBigBomb",
        "fishBossBomb",
        "fishBossBomb_Contest",
        "fishGoldBomb",
        "fishingSiteSelf",
        "fishingSmallBell",
        "fishingSiteOther_0_1",
        "fishingSiteOther_2_3",
        "Effect_Boom_UI",
        "Effect_Ice_UI",
        "dropBomb",
        "dropBombOther"
    };

    #endregion

    #region 游戏内提示文字
    public const string DiamondInsufficient = "钻石不足";
    public const string QuitGame = "确定要退出游戏？";
    public const string QuitFishScene = "确定离开捕鱼界面";
    public const string QuitMainScene = "确定返回登录界面";
    public const string UnLock = "至少解锁{0}倍炮台才可进入游戏！";
    public const string ShortGold = "金币不足";
    public const string ForgeUnLock = "解锁到{0}倍即可开启锻造功能";
    public const string ForgeEssenceCount = "水晶精华不足";
#endregion
}
