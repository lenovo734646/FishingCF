using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class TFishGunForge
{
    /// <summary>
    /// 唯一索引
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 炮值
    /// </summary>
    public long GunValue { get; set; }

    /// <summary>
    /// 解锁所需红宝石
    /// </summary>
    public int DiamondNeed { get; set; }

    /// <summary>
    /// 解锁所需道具
    /// </summary>
    public JArray ItemNeed { get; set; }

    /// <summary>
    /// 金币获得
    /// </summary>
    public int CoinGet { get; set; }

    /// <summary>
    /// 大赛积分提升
    /// </summary>
    public double AddPoints { get; set; }

    /// <summary>
    /// 升级类型判断（0为升级，1为锻造）
    /// </summary>
    public int UpgradeJudge { get; set; }
}

public static class TFishGunForgeHelper
{
    public static readonly string TableName = "FishGunForge";
    public static readonly Type TableType = typeof(TFishGunForge);

    public static Dictionary<int, TFishGunForge> DataMap;

    public static void LoadData(List<object> rows)
    {
        DataMap = new Dictionary<int, TFishGunForge>();
        foreach (var t in rows.Cast<TFishGunForge>())
            DataMap[t.Id] = t;
    }

    public static TFishGunForge GetRow(int id)
    {
        TFishGunForge r = null;
        return DataMap.TryGetValue(id, out r) ? r : null;
    }
}
