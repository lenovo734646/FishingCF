using System;
using System.Collections.Generic;

public class TableLoadConfig
{
    public string LocalJsonDirectory { get; set; }

    public List<Type> TableHelperTypeList { get; set; }

    public TableLoadConfig()
    {
        LocalJsonDirectory = "Table/";

        TableHelperTypeList = new List<Type>()
        {
//            typeof(TFishHelper),
            typeof(TItemHelper),
//            typeof(TFishRoomHelper),
//            typeof(TUserLevelHelper),
            typeof(TFishGunForgeHelper),
//            typeof(TVIPHelper),
            typeof(TLanguageHelper),
//            typeof(TFishBonusLevelHelper),
//            typeof(TFishBonusRewardHelper),
//            typeof(TFishGunHelper),
//            typeof(TSignInHelper),
//            typeof(TFishMatchHelper),
//            typeof(TVipWheelHelper),
//            typeof(TGlobalHelper),
//            typeof(TPathWayPointsHelper),
//            typeof(TPathNameHelper),
//            typeof(TFishGlobalHelper),
//            typeof(TRechargeDailyHelper),
//            typeof(TTaskHelper),
//            typeof(TTaskAchieveHelper),
//            typeof(TTaskActiveHelper),
//            typeof(TInvestCostRewardHelper),
//            typeof(TInvestGunRechargeHelper),
//            typeof(TInvestGunRewardHelper),
//            typeof(TShopHelper),
//            typeof(TRealGoodsHelper),
//            typeof(TShopViewHelper),
//            typeof(THotUpdateHelper),
        };
    }
}
