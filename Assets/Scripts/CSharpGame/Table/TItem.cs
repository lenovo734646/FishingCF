using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class TItem
{
    /// <summary>
    /// 主索引
    /// 0平台道具
    /// 1捕鱼道具
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 子索引
    /// </summary>
    public int SubId { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 道具说明，用于客户端显示
    /// （弹头，奖券ID索引位置不可变动）
    /// </summary>
    public string Desc { get; set; }

    /// <summary>
    /// 显示图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 使用CD间隔，单位毫秒
    /// </summary>
    public int Cd { get; set; }

    /// <summary>
    /// 数量上限，-1代表无限制
    /// </summary>
    public int LimitCount { get; set; }

    /// <summary>
    /// 使用物品所需的最低vip等级
    /// </summary>
    public int MinVipLevelForUse { get; set; }

    /// <summary>
    /// 使用效果类型
    /// 1.1锁定 [持续时间]
    /// 1.2冰冻 [持续时间]
    /// 1.3召唤 [[鱼Id,权重],[鱼Id,权重],...]
    /// 1.4狂暴 [持续时间,是否锁定,开炮速度加成]
    /// 1.5分身 [持续时间]
    /// 1.6弹头 [金币价值,随机系数]
    /// </summary>
    public int Effect { get; set; }

    /// <summary>
    /// 效果参数
    /// 1.1锁定 [持续时间]
    /// 1.2冰冻 [持续时间]
    /// 1.3召唤 [[鱼Id,权重],[鱼Id,权重],...]
    /// 1.4狂暴 [持续时间,是否锁定,开炮速度加成]
    /// 1.5分身 [持续时间]
    /// 1.6弹头 [金币价值,随机系数（在现有价值下使用的增加或减少的百分比）]
    /// </summary>
    public JArray EffectParam { get; set; }

    /// <summary>
    /// 背包显示
    /// [填1显示，不填不显示]
    /// </summary>
    public int BagView { get; set; }

    /// <summary>
    /// 购买所需的最低vip等级
    /// </summary>
    public int MinVipLevelForPurchase { get; set; }

    /// <summary>
    /// 购买消耗道具类型：
    /// [暂不销售：0
    /// 钻石：1，
    /// 金币：2 ,
    /// 积分：3]之后尽量不要增加通用货币，可以增加游戏内属性货币
    /// </summary>
    public int PriceType { get; set; }

    /// <summary>
    /// 购买所需单价
    /// </summary>
    public int UnitPrice { get; set; }

    /// <summary>
    /// 是否在公会仓库背包中显示
    /// [显示：1；
    /// 不显示：0。]
    /// </summary>
    public int WarehouseView { get; set; }

    /// <summary>
    /// 公会会员单次贡献最少个数
    /// </summary>
    public int WarehouseLimit { get; set; }
}

public static class TItemHelper
{
    public static readonly string TableName = "Item";
    public static readonly Type TableType = typeof(TItem);

    public static Dictionary<int, Dictionary<int, TItem>> GroupDataMap;

    public static void LoadData(List<object> rows)
    {
        GroupDataMap = new Dictionary<int, Dictionary<int, TItem>>();
        foreach (var t in rows.Cast<TItem>())
        {
            Dictionary<int, TItem> group = null;
            if (GroupDataMap.TryGetValue(t.Id, out group))
                group[t.SubId] = t;
            else
                GroupDataMap[t.Id] = new Dictionary<int, TItem>() { {t.SubId, t} };
        }
    }

    public static Dictionary<int, TItem> GetSubRows(int id)
    {
        Dictionary<int, TItem> r = null;
        return GroupDataMap.TryGetValue(id, out r) ? r : null;
    }

    public static TItem GetRow(int id, int subId)
    {
        TItem r = null;
        Dictionary<int, TItem> group = null;
        if (GroupDataMap.TryGetValue(id, out group))
            group.TryGetValue(subId, out r);
        return r;
    }
}
