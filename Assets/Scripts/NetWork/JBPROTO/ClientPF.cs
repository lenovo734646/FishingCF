using System;
using System.IO;

namespace JBPROTO
{
    /// <summary>
    /// 登出请求
    /// </summary>
    public sealed class CLPFLogoutReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 0;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 资源同步通知
    /// </summary>
    public sealed class CLPFResSyncNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 1;
        /// <summary>
        /// 钻石数量
        /// </summary>
        public Int64 diamond;
        /// <summary>
        /// 金币数量
        /// </summary>
        public Int64 currency;
        /// <summary>
        /// 积分数量
        /// </summary>
        public Int64 integral;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(diamond);
            bw.Write(currency);
            bw.Write(integral);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                diamond = br.ReadInt64();
                currency = br.ReadInt64();
                integral = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 资源变化通知
    /// </summary>
    public sealed class CLPFResChangedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 2;
        /// <summary>
        /// 资源类型 1钻石 2金币 3绑定金币 4积分
        /// </summary>
        public sbyte res_type;
        /// <summary>
        /// 资源值
        /// </summary>
        public Int64 res_value;
        /// <summary>
        /// 资源变化量
        /// </summary>
        public int res_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(res_type);
            bw.Write(res_value);
            bw.Write(res_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                res_type = br.ReadSByte();
                res_value = br.ReadInt64();
                res_delta = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 物品信息结构
    /// </summary>
    public sealed class CLPFItemInfo : INetProtocol
    {
        /// <summary>
        /// 物品主类型
        /// </summary>
        public int item_id;
        /// <summary>
        /// 物品子类型
        /// </summary>
        public int item_sub_id;
        /// <summary>
        /// 物品数量
        /// </summary>
        public Int64 item_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(item_id);
            bw.Write(item_sub_id);
            bw.Write(item_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item_id = br.ReadInt32();
                item_sub_id = br.ReadInt32();
                item_count = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取物品列表请求
    /// </summary>
    public sealed class CLPFItemGetListReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 3;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取物品列表回应
    /// </summary>
    public sealed class CLPFItemGetListAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 4;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 使用物品请求
    /// </summary>
    public sealed class CLPFItemUseReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 5;
        /// <summary>
        /// 物品信息
        /// </summary>
        public CLPFItemInfo item;
        /// <summary>
        /// 游戏Id
        /// </summary>
        public int group_id;
        /// <summary>
        /// 玩法Id
        /// </summary>
        public int service_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            item.toBinary(bw);
            bw.Write(group_id);
            bw.Write(service_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item = new CLPFItemInfo();
                item.fromBinary(br);
                group_id = br.ReadInt32();
                service_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 使用物品回应
    /// </summary>
    public sealed class CLPFItemUseAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 6;
        /// <summary>
        /// 0成功 1数量不足 2配置表错误 3使用失败 4鱼潮即将来临禁止使用 5狂暴下不能使用瞄准 6分身下不能使用瞄准 7vip等级不足
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 物品数量变化通知
    /// </summary>
    public sealed class CLPFItemCountChangeNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 7;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 物品购买请求
    /// </summary>
    public sealed class CLPFItemBuyReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 8;
        /// <summary>
        /// 购买的物品
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 物品购买回应
    /// </summary>
    public sealed class CLPFItemBuyAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 9;
        /// <summary>
        /// 0购买成功 1购买数量非法 2物品不存在 3该道具不卖 4资源不足 5vip等级不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 获得的物品，仅用于显示
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 商城购买次数信息
    /// </summary>
    public sealed class CLPFShopBuyCountItem : INetProtocol
    {
        /// <summary>
        /// 商城购买项Id
        /// </summary>
        public int shop_id;
        /// <summary>
        /// 购买次数
        /// </summary>
        public int buy_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(shop_id);
            bw.Write(buy_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                shop_id = br.ReadInt32();
                buy_count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 商城购买次数请求
    /// </summary>
    public sealed class CLPFShopQueryBuyCountReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 10;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 商城购买次数回应
    /// </summary>
    public sealed class CLPFShopQueryBuyCountAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 11;
        /// <summary>
        /// 总购买次数数组长度
        /// </summary>
        public int total_item_len;
        /// <summary>
        /// 总购买次数数组
        /// </summary>
        public CLPFShopBuyCountItem[] total_item_array;  //max:100
        /// <summary>
        /// 当天购买次数数组长度
        /// </summary>
        public int today_item_len;
        /// <summary>
        /// 当天购买次数数组
        /// </summary>
        public CLPFShopBuyCountItem[] today_item_array;  //max:100
        /// <summary>
        /// 当天兑换实物商品数组长度
        /// </summary>
        public int today_real_goods_len;
        /// <summary>
        /// 当天兑换实物商品数组
        /// </summary>
        public CLPFShopBuyCountItem[] today_real_goods_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(total_item_len);
            for (int i = 0; i < (int)total_item_len; i++)
                total_item_array[i].toBinary(bw);
            bw.Write(today_item_len);
            for (int i = 0; i < (int)today_item_len; i++)
                today_item_array[i].toBinary(bw);
            bw.Write(today_real_goods_len);
            for (int i = 0; i < (int)today_real_goods_len; i++)
                today_real_goods_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                total_item_len = br.ReadInt32();
                total_item_array = new CLPFShopBuyCountItem[(int)total_item_len];
                for (int i = 0; i < total_item_array.Length; i++)
                {
                    total_item_array[i] = new CLPFShopBuyCountItem();
                    total_item_array[i].fromBinary(br);
                }
                today_item_len = br.ReadInt32();
                today_item_array = new CLPFShopBuyCountItem[(int)today_item_len];
                for (int i = 0; i < today_item_array.Length; i++)
                {
                    today_item_array[i] = new CLPFShopBuyCountItem();
                    today_item_array[i].fromBinary(br);
                }
                today_real_goods_len = br.ReadInt32();
                today_real_goods_array = new CLPFShopBuyCountItem[(int)today_real_goods_len];
                for (int i = 0; i < today_real_goods_array.Length; i++)
                {
                    today_real_goods_array[i] = new CLPFShopBuyCountItem();
                    today_real_goods_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 商城购买请求
    /// </summary>
    public sealed class CLPFShopBuyReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 12;
        /// <summary>
        /// 商城购买项Id
        /// </summary>
        public int shop_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(shop_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                shop_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 商城购买回应
    /// </summary>
    public sealed class CLPFShopBuyAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 13;
        /// <summary>
        /// 0成功 1无此购买项 2资源不足 3购买次数已达上限 4vip等级不足 5系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 通用充值请求
    /// </summary>
    public sealed class CLPFRechargeReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 14;
        /// <summary>
        /// 购买内容类型 1商城充值 2购买月卡 3首充礼包 4每日充值 5投资炮倍 6出海保险
        /// </summary>
        public int content_type;
        /// <summary>
        /// 购买内容Id
        /// </summary>
        public int content_id;
        /// <summary>
        /// 支付渠道 1微信支付 2支付宝
        /// </summary>
        public int pay_mode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(content_type);
            bw.Write(content_id);
            bw.Write(pay_mode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                content_type = br.ReadInt32();
                content_id = br.ReadInt32();
                pay_mode = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 通用充值回应
    /// </summary>
    public sealed class CLPFRechargeAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 15;
        /// <summary>
        /// 0成功 1无此购买项 2不支持的支付渠道 3购买次数已达上限 4vip等级不足 5系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 支付环境，json格式字符串
        /// </summary>
        public string pay_envir;  //max:4096

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            NetHelper.SafeWriteString(bw, pay_envir, 4096);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                pay_envir = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 充值到账通知
    /// </summary>
    public sealed class CLPFRechargeSuccessNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 16;
        /// <summary>
        /// 购买内容类型 1商城充值 2购买月卡 3首充礼包 4每日充值 5投资炮倍 6出海保险
        /// </summary>
        public int content_type;
        /// <summary>
        /// 购买内容Id
        /// </summary>
        public int content_id;
        /// <summary>
        /// 获得物品数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 获得物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(content_type);
            bw.Write(content_id);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                content_type = br.ReadInt32();
                content_id = br.ReadInt32();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 排行榜玩家信息
    /// </summary>
    public sealed class CLPFRankPlayerInfo : INetProtocol
    {
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 性别 0保密 1男 2女
        /// </summary>
        public int gender;
        /// <summary>
        /// 头像Id
        /// </summary>
        public int head;
        /// <summary>
        /// 头像框Id
        /// </summary>
        public int head_frame;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level;
        /// <summary>
        /// 玩家vip等级
        /// </summary>
        public int vip_level;
        /// <summary>
        /// 平台货币
        /// </summary>
        public Int64 rank_value;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(gender);
            bw.Write(head);
            bw.Write(head_frame);
            bw.Write(level);
            bw.Write(vip_level);
            bw.Write(rank_value);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                gender = br.ReadInt32();
                head = br.ReadInt32();
                head_frame = br.ReadInt32();
                level = br.ReadInt32();
                vip_level = br.ReadInt32();
                rank_value = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取排行榜请求
    /// </summary>
    public sealed class CLPFGetRankListReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 17;
        /// <summary>
        /// 排行榜类型 1金币榜 2弹头榜
        /// </summary>
        public int rank_type;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(rank_type);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                rank_type = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取排行榜回应
    /// </summary>
    public sealed class CLPFGetRankListAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 18;
        /// <summary>
        /// 0成功 1系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int rank_len;
        /// <summary>
        /// 排行榜数据数组
        /// </summary>
        public CLPFRankPlayerInfo[] rank_rows;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(rank_len);
            for (int i = 0; i < (int)rank_len; i++)
                rank_rows[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                rank_len = br.ReadInt32();
                rank_rows = new CLPFRankPlayerInfo[(int)rank_len];
                for (int i = 0; i < rank_rows.Length; i++)
                {
                    rank_rows[i] = new CLPFRankPlayerInfo();
                    rank_rows[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 玩家经验变化通知
    /// </summary>
    public sealed class CLPFLevelExpChangedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 19;
        /// <summary>
        /// 玩家等级经验
        /// </summary>
        public Int64 level_exp;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(level_exp);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                level_exp = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 玩家升级通知
    /// </summary>
    public sealed class CLPFLevelUpNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 20;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level;
        /// <summary>
        /// 升级奖励物品数组长度
        /// </summary>
        public int reward_len;
        /// <summary>
        /// 升级奖励物品数组
        /// </summary>
        public CLPFItemInfo[] reward_array;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(level);
            bw.Write(reward_len);
            for (int i = 0; i < (int)reward_len; i++)
                reward_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                level = br.ReadInt32();
                reward_len = br.ReadInt32();
                reward_array = new CLPFItemInfo[(int)reward_len];
                for (int i = 0; i < reward_array.Length; i++)
                {
                    reward_array[i] = new CLPFItemInfo();
                    reward_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// Vip经验等级发生变化
    /// </summary>
    public sealed class CLPFVipExpChangedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 21;
        /// <summary>
        /// 玩家vip等级
        /// </summary>
        public int vip_level;
        /// <summary>
        /// 玩家vip等级经验
        /// </summary>
        public Int64 vip_level_exp;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(vip_level);
            bw.Write(vip_level_exp);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                vip_level = br.ReadInt32();
                vip_level_exp = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改昵称请求
    /// </summary>
    public sealed class CLPFModifyNicknameReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 22;
        /// <summary>
        /// 新的昵称
        /// </summary>
        public string new_nickname;  //max:32

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, new_nickname, 32);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                new_nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改昵称回应
    /// </summary>
    public sealed class CLPFModifyNicknameAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 23;
        /// <summary>
        /// 0成功 1格式不合法 2包含敏感字符 3昵称已存在 4钻石不足
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改头像请求
    /// </summary>
    public sealed class CLPFModifyHeadReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 24;
        /// <summary>
        /// 新的头像Id
        /// </summary>
        public int new_head;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(new_head);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                new_head = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改头像回应
    /// </summary>
    public sealed class CLPFModifyHeadAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 25;
        /// <summary>
        /// 0成功
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询签到请求
    /// </summary>
    public sealed class CLPFQuerySignReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 26;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询签到回应
    /// </summary>
    public sealed class CLPFQuerySignAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 27;
        /// <summary>
        /// 当月已签次数
        /// </summary>
        public int signed_count;
        /// <summary>
        /// 当天是否已签
        /// </summary>
        public byte today_signed;
        /// <summary>
        /// 当月总天数
        /// </summary>
        public int total_days;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(signed_count);
            bw.Write(today_signed);
            bw.Write(total_days);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                signed_count = br.ReadInt32();
                today_signed = br.ReadByte();
                total_days = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 执行签到请求
    /// </summary>
    public sealed class CLPFActSignReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 28;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 执行签到回应
    /// </summary>
    public sealed class CLPFActSignAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 29;
        /// <summary>
        /// 0成功 1重复签到 2配置表错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询vip抽奖请求
    /// </summary>
    public sealed class CLPFQueryVipWheelReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 30;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询vip抽奖回应
    /// </summary>
    public sealed class CLPFQueryVipWheelAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 31;
        /// <summary>
        /// 当天已抽奖次数
        /// </summary>
        public int used_count;
        /// <summary>
        /// 可抽奖总次数
        /// </summary>
        public int total_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(used_count);
            bw.Write(total_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                used_count = br.ReadInt32();
                total_count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 执行vip抽奖请求
    /// </summary>
    public sealed class CLPFActVipWheelReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 32;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 执行vip抽奖回应
    /// </summary>
    public sealed class CLPFActVipWheelAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 33;
        /// <summary>
        /// 0成功 1抽奖次数不足 2金币不足 3配置表错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 随机到的结果奖项Id
        /// </summary>
        public int reward_id;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(reward_id);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                reward_id = br.ReadInt32();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 邮件信息结构
    /// </summary>
    public sealed class CLPFMailInfo : INetProtocol
    {
        /// <summary>
        /// 邮件唯一Id
        /// </summary>
        public int id;
        /// <summary>
        /// 邮件类型 1系统邮件 2好友赠送邮件
        /// </summary>
        public sbyte type;
        /// <summary>
        /// 邮件标题
        /// </summary>
        public string title;  //max:256
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string content;  //max:512
        /// <summary>
        /// 附件物品长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 附件物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:10
        /// <summary>
        /// 邮件状态 1未读 2已读 3已领取
        /// </summary>
        public sbyte state;
        /// <summary>
        /// 邮件接收时间戳
        /// </summary>
        public UInt32 receive_time;
        /// <summary>
        /// 邮件过期时间戳
        /// </summary>
        public UInt32 expire_time;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(type);
            NetHelper.SafeWriteString(bw, title, 256);
            NetHelper.SafeWriteString(bw, content, 512);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
            bw.Write(state);
            bw.Write(receive_time);
            bw.Write(expire_time);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                id = br.ReadInt32();
                type = br.ReadSByte();
                title = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                content = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
                state = br.ReadSByte();
                receive_time = br.ReadUInt32();
                expire_time = br.ReadUInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 所有邮件Id请求
    /// </summary>
    public sealed class CLPFMailQueryAllIdsReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 34;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 所有邮件Id回应
    /// </summary>
    public sealed class CLPFMailQueryAllIdsAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 35;
        /// <summary>
        /// Id数组长度
        /// </summary>
        public int len;
        /// <summary>
        /// 邮件Id数组
        /// </summary>
        public int[] array;  //max:1000

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(len);
            for (int i = 0; i < (int)len; i++)
                bw.Write(array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                len = br.ReadInt32();
                array = new int[(int)len];
                for (int i = 0; i < array.Length; i++)
                    array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 批量邮件内容请求
    /// </summary>
    public sealed class CLPFMailBatchQueryContentReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 36;
        /// <summary>
        /// Id数组长度
        /// </summary>
        public int len;
        /// <summary>
        /// 邮件Id数组长度
        /// </summary>
        public int[] array;  //max:100
        /// <summary>
        /// 期望显示哪种语言？'CN'为中文
        /// </summary>
        public string language;  //max:16

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(len);
            for (int i = 0; i < (int)len; i++)
                bw.Write(array[i]);
            NetHelper.SafeWriteString(bw, language, 16);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                len = br.ReadInt32();
                array = new int[(int)len];
                for (int i = 0; i < array.Length; i++)
                    array[i] = br.ReadInt32();
                language = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 批量邮件内容回应
    /// </summary>
    public sealed class CLPFMailBatchQueryContentAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 37;
        /// <summary>
        /// 无效Id数组长度
        /// </summary>
        public int invalid_len;
        /// <summary>
        /// 无效Id数组
        /// </summary>
        public int[] invalid_array;  //max:100
        /// <summary>
        /// 结果数组长度
        /// </summary>
        public int result_len;
        /// <summary>
        /// 结果邮件数组
        /// </summary>
        public CLPFMailInfo[] result_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(invalid_len);
            for (int i = 0; i < (int)invalid_len; i++)
                bw.Write(invalid_array[i]);
            bw.Write(result_len);
            for (int i = 0; i < (int)result_len; i++)
                result_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                invalid_len = br.ReadInt32();
                invalid_array = new int[(int)invalid_len];
                for (int i = 0; i < invalid_array.Length; i++)
                    invalid_array[i] = br.ReadInt32();
                result_len = br.ReadInt32();
                result_array = new CLPFMailInfo[(int)result_len];
                for (int i = 0; i < result_array.Length; i++)
                {
                    result_array[i] = new CLPFMailInfo();
                    result_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查看邮件请求
    /// </summary>
    public sealed class CLPFMailAccessReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 38;
        /// <summary>
        /// 邮件唯一Id
        /// </summary>
        public int mail_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(mail_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                mail_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查看邮件回应
    /// </summary>
    public sealed class CLPFMailAccessAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 39;
        /// <summary>
        /// 是否还有未读邮件 1是0否
        /// </summary>
        public sbyte has_unread_mail;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(has_unread_mail);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                has_unread_mail = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取邮件物品请求
    /// </summary>
    public sealed class CLPFMailFetchItemReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 40;
        /// <summary>
        /// 邮件唯一Id
        /// </summary>
        public int mail_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(mail_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                mail_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取邮件物品回应
    /// </summary>
    public sealed class CLPFMailFetchItemAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 41;
        /// <summary>
        /// 0成功 1邮件不存在 2邮件已被领取过
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 获得的物品数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 获得的物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:10
        /// <summary>
        /// 是否还有未读邮件 1是0否
        /// </summary>
        public sbyte has_unread_mail;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
            bw.Write(has_unread_mail);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
                has_unread_mail = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 删除邮件请求
    /// </summary>
    public sealed class CLPFMailRemoveReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 42;
        /// <summary>
        /// 删除类型 1删除指定邮件 2删除已读且无可领取附件的邮件 3清空所有邮件
        /// </summary>
        public sbyte remove_type;
        /// <summary>
        /// 删除邮件数组长度
        /// </summary>
        public int remove_len;
        /// <summary>
        /// 需要删除的邮件Id数组
        /// </summary>
        public int[] remove_ids;  //max:1000

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(remove_type);
            bw.Write(remove_len);
            for (int i = 0; i < (int)remove_len; i++)
                bw.Write(remove_ids[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                remove_type = br.ReadSByte();
                remove_len = br.ReadInt32();
                remove_ids = new int[(int)remove_len];
                for (int i = 0; i < remove_ids.Length; i++)
                    remove_ids[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 删除邮件回应
    /// </summary>
    public sealed class CLPFMailRemoveAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 43;
        /// <summary>
        /// 0成功 1包含错误的邮件Id
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 是否还有未读邮件 1是0否
        /// </summary>
        public sbyte has_unread_mail;
        /// <summary>
        /// 已删除的邮件数组长度
        /// </summary>
        public int removed_len;
        /// <summary>
        /// 已删除的邮件Id数组
        /// </summary>
        public int[] removed_ids;  //max:1000

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(has_unread_mail);
            bw.Write(removed_len);
            for (int i = 0; i < (int)removed_len; i++)
                bw.Write(removed_ids[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                has_unread_mail = br.ReadSByte();
                removed_len = br.ReadInt32();
                removed_ids = new int[(int)removed_len];
                for (int i = 0; i < removed_ids.Length; i++)
                    removed_ids[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 邮件到来通知
    /// </summary>
    public sealed class CLPFMailArriveNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 44;
        /// <summary>
        /// 邮件信息
        /// </summary>
        public CLPFMailInfo mail_info;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            mail_info.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                mail_info = new CLPFMailInfo();
                mail_info.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会信息结构
    /// </summary>
    public sealed class CLPFGuildInfo : INetProtocol
    {
        /// <summary>
        /// 公会Id
        /// </summary>
        public int id;
        /// <summary>
        /// 公会名称
        /// </summary>
        public string name;  //max:64
        /// <summary>
        /// 公会宣言
        /// </summary>
        public string desc;  //max:256
        /// <summary>
        /// 公会徽章
        /// </summary>
        public int icon;
        /// <summary>
        /// 公会等级
        /// </summary>
        public int level;
        /// <summary>
        /// 入会玩家等级限制
        /// </summary>
        public int user_level_limit;
        /// <summary>
        /// 入会贵族等级限制
        /// </summary>
        public int vip_level_limit;
        /// <summary>
        /// 是否允许自动加入 1是0否
        /// </summary>
        public sbyte allow_auto_join;
        /// <summary>
        /// 公会当前成员数量
        /// </summary>
        public int member_count;
        /// <summary>
        /// 公会最大成员限制
        /// </summary>
        public int member_limit;
        /// <summary>
        /// 会长Id
        /// </summary>
        public int president_id;
        /// <summary>
        /// 会长昵称
        /// </summary>
        public string president_name;  //max:32

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(id);
            NetHelper.SafeWriteString(bw, name, 64);
            NetHelper.SafeWriteString(bw, desc, 256);
            bw.Write(icon);
            bw.Write(level);
            bw.Write(user_level_limit);
            bw.Write(vip_level_limit);
            bw.Write(allow_auto_join);
            bw.Write(member_count);
            bw.Write(member_limit);
            bw.Write(president_id);
            NetHelper.SafeWriteString(bw, president_name, 32);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                id = br.ReadInt32();
                name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                desc = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                icon = br.ReadInt32();
                level = br.ReadInt32();
                user_level_limit = br.ReadInt32();
                vip_level_limit = br.ReadInt32();
                allow_auto_join = br.ReadSByte();
                member_count = br.ReadInt32();
                member_limit = br.ReadInt32();
                president_id = br.ReadInt32();
                president_name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 请求加入公会的信息结构
    /// </summary>
    public sealed class CLPFGuildJoinItem : INetProtocol
    {
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 性别 0保密 1男 2女
        /// </summary>
        public int gender;
        /// <summary>
        /// 头像Id
        /// </summary>
        public int head;
        /// <summary>
        /// 头像框Id
        /// </summary>
        public int head_frame;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level;
        /// <summary>
        /// 玩家vip等级
        /// </summary>
        public int vip_level;
        /// <summary>
        /// 申请加入的时间戳
        /// </summary>
        public UInt32 request_time;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(gender);
            bw.Write(head);
            bw.Write(head_frame);
            bw.Write(level);
            bw.Write(vip_level);
            bw.Write(request_time);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                gender = br.ReadInt32();
                head = br.ReadInt32();
                head_frame = br.ReadInt32();
                level = br.ReadInt32();
                vip_level = br.ReadInt32();
                request_time = br.ReadUInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会成员信息结构
    /// </summary>
    public sealed class CLPFGuildMember : INetProtocol
    {
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 性别 0保密 1男 2女
        /// </summary>
        public int gender;
        /// <summary>
        /// 头像Id
        /// </summary>
        public int head;
        /// <summary>
        /// 头像框Id
        /// </summary>
        public int head_frame;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level;
        /// <summary>
        /// 玩家vip等级
        /// </summary>
        public int vip_level;
        /// <summary>
        /// 1会长 2管理员 3会员
        /// </summary>
        public sbyte job;
        /// <summary>
        /// 是否在线 1是0否
        /// </summary>
        public sbyte is_online;
        /// <summary>
        /// 最近登录时间戳
        /// </summary>
        public UInt32 last_login_time;
        /// <summary>
        /// 昨日贡献总金币数量
        /// </summary>
        public Int64 contribute;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(gender);
            bw.Write(head);
            bw.Write(head_frame);
            bw.Write(level);
            bw.Write(vip_level);
            bw.Write(job);
            bw.Write(is_online);
            bw.Write(last_login_time);
            bw.Write(contribute);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                gender = br.ReadInt32();
                head = br.ReadInt32();
                head_frame = br.ReadInt32();
                level = br.ReadInt32();
                vip_level = br.ReadInt32();
                job = br.ReadSByte();
                is_online = br.ReadSByte();
                last_login_time = br.ReadUInt32();
                contribute = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会红包成员信息结构
    /// </summary>
    public sealed class CLPFGuildRedpacketMember : INetProtocol
    {
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 性别 0保密 1男 2女
        /// </summary>
        public int gender;
        /// <summary>
        /// 头像Id
        /// </summary>
        public int head;
        /// <summary>
        /// 头像框Id
        /// </summary>
        public int head_frame;
        /// <summary>
        /// 玩家等级
        /// </summary>
        public int level;
        /// <summary>
        /// 玩家vip等级
        /// </summary>
        public int vip_level;
        /// <summary>
        /// 1会长 2管理员 3会员
        /// </summary>
        public sbyte job;
        /// <summary>
        /// 是否在线 1是0否
        /// </summary>
        public sbyte is_online;
        /// <summary>
        /// 今日领取红包次数
        /// </summary>
        public int grab_count;
        /// <summary>
        /// 今日共计抢到的金币数量
        /// </summary>
        public Int64 total_grab_result;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(gender);
            bw.Write(head);
            bw.Write(head_frame);
            bw.Write(level);
            bw.Write(vip_level);
            bw.Write(job);
            bw.Write(is_online);
            bw.Write(grab_count);
            bw.Write(total_grab_result);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                gender = br.ReadInt32();
                head = br.ReadInt32();
                head_frame = br.ReadInt32();
                level = br.ReadInt32();
                vip_level = br.ReadInt32();
                job = br.ReadSByte();
                is_online = br.ReadSByte();
                grab_count = br.ReadInt32();
                total_grab_result = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会仓库物品信息
    /// </summary>
    public sealed class CLPFGuildBagItem : INetProtocol
    {
        /// <summary>
        /// 物品Id
        /// </summary>
        public int item_id;
        /// <summary>
        /// 物品子Id
        /// </summary>
        public int item_sub_id;
        /// <summary>
        /// 物品数量
        /// </summary>
        public Int64 item_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(item_id);
            bw.Write(item_sub_id);
            bw.Write(item_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item_id = br.ReadInt32();
                item_sub_id = br.ReadInt32();
                item_count = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会仓库日志信息
    /// </summary>
    public sealed class CLPFGuildBagLog : INetProtocol
    {
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 玩家昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 1存入 2取出
        /// </summary>
        public sbyte type;
        /// <summary>
        /// 物品Id
        /// </summary>
        public int item_id;
        /// <summary>
        /// 物品子Id
        /// </summary>
        public int item_sub_id;
        /// <summary>
        /// 物品数量
        /// </summary>
        public Int64 item_count;
        /// <summary>
        /// 时间戳
        /// </summary>
        public UInt32 timestamp;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(type);
            bw.Write(item_id);
            bw.Write(item_sub_id);
            bw.Write(item_count);
            bw.Write(timestamp);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                type = br.ReadSByte();
                item_id = br.ReadInt32();
                item_sub_id = br.ReadInt32();
                item_count = br.ReadInt64();
                timestamp = br.ReadUInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 创建公会请求
    /// </summary>
    public sealed class CLPFGuildCreateReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 45;
        /// <summary>
        /// 公会名称
        /// </summary>
        public string name;  //max:64
        /// <summary>
        /// 公会徽章
        /// </summary>
        public int icon;
        /// <summary>
        /// 入会玩家等级限制
        /// </summary>
        public int user_level_limit;
        /// <summary>
        /// 入会贵族等级限制
        /// </summary>
        public int vip_level_limit;
        /// <summary>
        /// 是否允许自动加入 1是0否
        /// </summary>
        public sbyte allow_auto_join;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, name, 64);
            bw.Write(icon);
            bw.Write(user_level_limit);
            bw.Write(vip_level_limit);
            bw.Write(allow_auto_join);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                icon = br.ReadInt32();
                user_level_limit = br.ReadInt32();
                vip_level_limit = br.ReadInt32();
                allow_auto_join = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 创建公会回应
    /// </summary>
    public sealed class CLPFGuildCreateAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 46;
        /// <summary>
        /// 0成功 1已有公会 2公会名称非法 3公会名字已存在 4等级不足 5贵族等级不足 6钻石不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 公会信息
        /// </summary>
        public CLPFGuildInfo info;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            info.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                info = new CLPFGuildInfo();
                info.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会推荐列表请求
    /// </summary>
    public sealed class CLPFGuildQueryRecommendListReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 47;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取公会推荐列表回应
    /// </summary>
    public sealed class CLPFGuildQueryRecommendListAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 48;
        /// <summary>
        /// 推荐列表数组长度
        /// </summary>
        public int len;
        /// <summary>
        /// 推荐列表数组
        /// </summary>
        public CLPFGuildInfo[] array;  //max:6
        /// <summary>
        /// 申请加入标记数组长度
        /// </summary>
        public int join_flags_len;
        /// <summary>
        /// 申请加入标记数组
        /// </summary>
        public sbyte[] join_flags_array;  //max:6

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(len);
            for (int i = 0; i < (int)len; i++)
                array[i].toBinary(bw);
            bw.Write(join_flags_len);
            for (int i = 0; i < (int)join_flags_len; i++)
                bw.Write(join_flags_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                len = br.ReadInt32();
                array = new CLPFGuildInfo[(int)len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new CLPFGuildInfo();
                    array[i].fromBinary(br);
                }
                join_flags_len = br.ReadInt32();
                join_flags_array = new sbyte[(int)join_flags_len];
                for (int i = 0; i < join_flags_array.Length; i++)
                    join_flags_array[i] = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 搜索公会请求
    /// </summary>
    public sealed class CLPFGuildSearchReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 49;
        /// <summary>
        /// 公会Id
        /// </summary>
        public int guild_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(guild_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                guild_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 搜索公会回应
    /// </summary>
    public sealed class CLPFGuildSearchAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 50;
        /// <summary>
        /// 0成功 1公会不存在
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 公会详细信息
        /// </summary>
        public CLPFGuildInfo info;
        /// <summary>
        /// 申请加入标记
        /// </summary>
        public sbyte join_flag;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            info.toBinary(bw);
            bw.Write(join_flag);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                info = new CLPFGuildInfo();
                info.fromBinary(br);
                join_flag = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 快速加入公会请求
    /// </summary>
    public sealed class CLPFGuildQuickJoinReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 51;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 快速加入公会回应
    /// </summary>
    public sealed class CLPFGuildQuickJoinAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 52;
        /// <summary>
        /// 0成功 1已有公会 2当天退出过公会无法加入 3当日申请次数已达上限 4无满足条件的公会
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 公会详细信息
        /// </summary>
        public CLPFGuildInfo info;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            info.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                info = new CLPFGuildInfo();
                info.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 加入公会请求
    /// </summary>
    public sealed class CLPFGuildJoinReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 53;
        /// <summary>
        /// 申请加入的公会Id
        /// </summary>
        public int guild_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(guild_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                guild_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 加入公会回应
    /// </summary>
    public sealed class CLPFGuildJoinAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 54;
        /// <summary>
        /// 0申请成功 1已有公会 2公会不存在 3等级不满足 4贵族等级不满足 5公会成员已满 6重复申请 7当日主动退出不允许加入 8当日申请次数已达上限
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会申请列表请求
    /// </summary>
    public sealed class CLPFGuildQueryJoinListReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 55;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取公会申请列表回应
    /// </summary>
    public sealed class CLPFGuildQueryJoinListAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 56;
        /// <summary>
        /// 0成功 1没有公会 2权限不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 加入请求数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 加入请求数组
        /// </summary>
        public CLPFGuildJoinItem[] item_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                item_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                item_array = new CLPFGuildJoinItem[(int)item_len];
                for (int i = 0; i < item_array.Length; i++)
                {
                    item_array[i] = new CLPFGuildJoinItem();
                    item_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 处理公会加入请求
    /// </summary>
    public sealed class CLPFGuildHandleJoinReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 57;
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 是否同意加入 1是0否
        /// </summary>
        public int agree;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(user_id);
            bw.Write(agree);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                agree = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 处理公会加入回应
    /// </summary>
    public sealed class CLPFGuildHandleJoinAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 58;
        /// <summary>
        /// 0操作成功 1你没有公会 2权限不足 3找不到该申请纪录 4玩家等级不足 5玩家贵族等级不足 6公会成员已满 7玩家已加入其它公会
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 新加入的成员信息，只有当同意加入并操作成功时该字段才有意义
        /// </summary>
        public CLPFGuildMember new_member;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            new_member.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                new_member = new CLPFGuildMember();
                new_member.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 加入公会应答通知
    /// </summary>
    public sealed class CLPFGuildJoinResponseNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 59;
        /// <summary>
        /// 公会Id
        /// </summary>
        public int guild_id;
        /// <summary>
        /// 公会名称
        /// </summary>
        public string guild_name;  //max:64
        /// <summary>
        /// 操作员玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 操作员玩家昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 是否同意加入公会 1是0否
        /// </summary>
        public int agree;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(guild_id);
            NetHelper.SafeWriteString(bw, guild_name, 64);
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(agree);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                guild_id = br.ReadInt32();
                guild_name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                agree = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 新的申请加入公会的通知
    /// </summary>
    public sealed class CLPFGuildNewJoinRequestNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 60;
        /// <summary>
        /// 申请人玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 申请人玩家昵称
        /// </summary>
        public string nickname;  //max:32

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会信息请求
    /// </summary>
    public sealed class CLPFGuildQueryInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 61;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取公会信息回应
    /// </summary>
    public sealed class CLPFGuildQueryInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 62;
        /// <summary>
        /// 0成功 1你没有公会
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 公会详细信息
        /// </summary>
        public CLPFGuildInfo info;
        /// <summary>
        /// 公会成员数量
        /// </summary>
        public int members_len;
        /// <summary>
        /// 公会成员数组
        /// </summary>
        public CLPFGuildMember[] members_array;  //max:100
        /// <summary>
        /// 你的职务
        /// </summary>
        public int job;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            info.toBinary(bw);
            bw.Write(members_len);
            for (int i = 0; i < (int)members_len; i++)
                members_array[i].toBinary(bw);
            bw.Write(job);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                info = new CLPFGuildInfo();
                info.fromBinary(br);
                members_len = br.ReadInt32();
                members_array = new CLPFGuildMember[(int)members_len];
                for (int i = 0; i < members_array.Length; i++)
                {
                    members_array[i] = new CLPFGuildMember();
                    members_array[i].fromBinary(br);
                }
                job = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改公会信息请求
    /// </summary>
    public sealed class CLPFGuildModifyInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 63;
        /// <summary>
        /// 公会名称
        /// </summary>
        public string name;  //max:64
        /// <summary>
        /// 公会宣言
        /// </summary>
        public string desc;  //max:256
        /// <summary>
        /// 公会徽章
        /// </summary>
        public int icon;
        /// <summary>
        /// 入会玩家等级限制
        /// </summary>
        public int user_level_limit;
        /// <summary>
        /// 入会贵族等级限制
        /// </summary>
        public int vip_level_limit;
        /// <summary>
        /// 是否允许自动加入 1是0否
        /// </summary>
        public sbyte allow_auto_join;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, name, 64);
            NetHelper.SafeWriteString(bw, desc, 256);
            bw.Write(icon);
            bw.Write(user_level_limit);
            bw.Write(vip_level_limit);
            bw.Write(allow_auto_join);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                desc = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                icon = br.ReadInt32();
                user_level_limit = br.ReadInt32();
                vip_level_limit = br.ReadInt32();
                allow_auto_join = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改公会信息回应
    /// </summary>
    public sealed class CLPFGuildModifyInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 64;
        /// <summary>
        /// 0成功 1你没有公会 2权限不足 3公会名称已存在 4钻石不足 5修改次数已达上限
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改成员职务请求
    /// </summary>
    public sealed class CLPFGuildModifyMemberJobReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 65;
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 2管理员 3普通成员
        /// </summary>
        public sbyte job;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(user_id);
            bw.Write(job);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                job = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 修改成员职务回应
    /// </summary>
    public sealed class CLPFGuildModifyMemberJobAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 66;
        /// <summary>
        /// 0成功 1你不是会长 2非法操作
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 踢出成员请求
    /// </summary>
    public sealed class CLPFGuildKickMemberReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 67;
        /// <summary>
        /// 踢出的成员数组长度
        /// </summary>
        public int id_len;
        /// <summary>
        /// 踢出的成员数组
        /// </summary>
        public int[] id_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(id_len);
            for (int i = 0; i < (int)id_len; i++)
                bw.Write(id_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                id_len = br.ReadInt32();
                id_array = new int[(int)id_len];
                for (int i = 0; i < id_array.Length; i++)
                    id_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 踢出成员回应
    /// </summary>
    public sealed class CLPFGuildKickMemberAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 68;
        /// <summary>
        /// 0成功 1你没有公会 2权限不足 3非法操作
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 被踢出通知
    /// </summary>
    public sealed class CLPFGuildKickMemberNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 69;
        /// <summary>
        /// 操作者玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 操作者玩家昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 操作者职务 1会长 2管理员
        /// </summary>
        public sbyte job;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(job);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                job = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 自己退出公会请求
    /// </summary>
    public sealed class CLPFGuildExitReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 70;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 自己退出公会回应
    /// </summary>
    public sealed class CLPFGuildExitAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 71;
        /// <summary>
        /// 0成功 1你没有公会 2会长需先设置管理员才能退出
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会升级请求
    /// </summary>
    public sealed class CLPFGuildUpgradeReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 72;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 公会升级回应
    /// </summary>
    public sealed class CLPFGuildUpgradeAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 73;
        /// <summary>
        /// 0成功 1你不是会长 2钻石不足 3已满级
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 升级后的公会等级
        /// </summary>
        public int guild_level;
        /// <summary>
        /// 升级后公会最大成员数量
        /// </summary>
        public int guild_max_members;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(guild_level);
            bw.Write(guild_max_members);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                guild_level = br.ReadInt32();
                guild_max_members = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询会长福利请求
    /// </summary>
    public sealed class CLPFGuildQueryWelfareReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 74;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询会长福利回应
    /// </summary>
    public sealed class CLPFGuildQueryWelfareAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 75;
        /// <summary>
        /// 0成功 1你不是会长
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前实时额度
        /// </summary>
        public Int64 contribute;
        /// <summary>
        /// 福利金币总数量
        /// </summary>
        public Int64 welfare;
        /// <summary>
        /// 0未领取 1已领取过
        /// </summary>
        public sbyte is_fetched;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(contribute);
            bw.Write(welfare);
            bw.Write(is_fetched);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                contribute = br.ReadInt64();
                welfare = br.ReadInt64();
                is_fetched = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取会长福利请求
    /// </summary>
    public sealed class CLPFGuildFetchWelfareReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 76;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 领取会长福利回应
    /// </summary>
    public sealed class CLPFGuildFetchWelfareAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 77;
        /// <summary>
        /// 0成功 1你不是会长 2已领取过
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 获得了多少金币
        /// </summary>
        public Int64 welfare;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(welfare);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                welfare = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会红包信息请求
    /// </summary>
    public sealed class CLPFGuildQueryRedPacketInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 78;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取公会红包信息回应
    /// </summary>
    public sealed class CLPFGuildQueryRedPacketInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 79;
        /// <summary>
        /// 0成功 1你没有公会
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 今日累积奖池
        /// </summary>
        public Int64 today_pool;
        /// <summary>
        /// 昨日累积奖池
        /// </summary>
        public Int64 past_pool;
        /// <summary>
        /// 今日已派发金币数
        /// </summary>
        public Int64 today_give_out;
        /// <summary>
        /// 剩余红包个数
        /// </summary>
        public int left_packet_count;
        /// <summary>
        /// 红包总数
        /// </summary>
        public int total_packet_count;
        /// <summary>
        /// 今日奖金鱼捐献次数
        /// </summary>
        public int donate_num;
        /// <summary>
        /// 今日已抢红包次数
        /// </summary>
        public int grabed_count;
        /// <summary>
        /// 剩余可抢红包次数
        /// </summary>
        public int left_grab_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(today_pool);
            bw.Write(past_pool);
            bw.Write(today_give_out);
            bw.Write(left_packet_count);
            bw.Write(total_packet_count);
            bw.Write(donate_num);
            bw.Write(grabed_count);
            bw.Write(left_grab_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                today_pool = br.ReadInt64();
                past_pool = br.ReadInt64();
                today_give_out = br.ReadInt64();
                left_packet_count = br.ReadInt32();
                total_packet_count = br.ReadInt32();
                donate_num = br.ReadInt32();
                grabed_count = br.ReadInt32();
                left_grab_count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会红包排行榜请求
    /// </summary>
    public sealed class CLPFGuildQueryRedPacketRankReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 80;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取公会红包排行榜回应
    /// </summary>
    public sealed class CLPFGuildQueryRedPacketRankAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 81;
        /// <summary>
        /// 0成功 1你没有公会
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 排行榜成员数组长度
        /// </summary>
        public int rank_len;
        /// <summary>
        /// 排行榜成员数组
        /// </summary>
        public CLPFGuildRedpacketMember[] rank_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(rank_len);
            for (int i = 0; i < (int)rank_len; i++)
                rank_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                rank_len = br.ReadInt32();
                rank_array = new CLPFGuildRedpacketMember[(int)rank_len];
                for (int i = 0; i < rank_array.Length; i++)
                {
                    rank_array[i] = new CLPFGuildRedpacketMember();
                    rank_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会抢红包请求
    /// </summary>
    public sealed class CLPFGuildActRedPacketReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 82;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 公会抢红包回应
    /// </summary>
    public sealed class CLPFGuildActRedPacketAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 83;
        /// <summary>
        /// 0成功 1你没有公会 2次数不足 3奖池太小还不能领取红包
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 获得的金币数量
        /// </summary>
        public Int64 grab_result;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(grab_result);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                grab_result = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会仓库信息请求
    /// </summary>
    public sealed class CLPFGuildBagQueryInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 84;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取公会仓库信息回应
    /// </summary>
    public sealed class CLPFGuildBagQueryInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 85;
        /// <summary>
        /// 0成功 1你没有公会
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 仓库物品长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 仓库物品数组
        /// </summary>
        public CLPFGuildBagItem[] item_array;  //max:100
        /// <summary>
        /// 仓库日志长度
        /// </summary>
        public int log_len;
        /// <summary>
        /// 仓库日志数组
        /// </summary>
        public CLPFGuildBagLog[] log_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                item_array[i].toBinary(bw);
            bw.Write(log_len);
            for (int i = 0; i < (int)log_len; i++)
                log_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                item_array = new CLPFGuildBagItem[(int)item_len];
                for (int i = 0; i < item_array.Length; i++)
                {
                    item_array[i] = new CLPFGuildBagItem();
                    item_array[i].fromBinary(br);
                }
                log_len = br.ReadInt32();
                log_array = new CLPFGuildBagLog[(int)log_len];
                for (int i = 0; i < log_array.Length; i++)
                {
                    log_array[i] = new CLPFGuildBagLog();
                    log_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会仓库日志请求
    /// </summary>
    public sealed class CLPFGuildBagQueryLogReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 86;
        /// <summary>
        /// 每页数量100，请求第几页？
        /// </summary>
        public int page_index;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(page_index);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                page_index = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取公会仓库日志回应
    /// </summary>
    public sealed class CLPFGuildBagQueryLogAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 87;
        /// <summary>
        /// 0成功 1你没有公会
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 日志数组长度
        /// </summary>
        public int log_len;
        /// <summary>
        /// 仓库日志数组
        /// </summary>
        public CLPFGuildBagLog[] log_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(log_len);
            for (int i = 0; i < (int)log_len; i++)
                log_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                log_len = br.ReadInt32();
                log_array = new CLPFGuildBagLog[(int)log_len];
                for (int i = 0; i < log_array.Length; i++)
                {
                    log_array[i] = new CLPFGuildBagLog();
                    log_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会仓库存入物品请求
    /// </summary>
    public sealed class CLPFGuildBagStoreItemReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 88;
        /// <summary>
        /// 存入的物品
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会仓库存入物品回应
    /// </summary>
    public sealed class CLPFGuildBagStoreItemAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 89;
        /// <summary>
        /// 0成功 1你没有公会 2不能存入该物品 3vip等级不足 4物品不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前存入仓库动作所对应的日志
        /// </summary>
        public CLPFGuildBagLog bag_log;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bag_log.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                bag_log = new CLPFGuildBagLog();
                bag_log.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会仓库取出物品请求
    /// </summary>
    public sealed class CLPFGuildBagFetchItemReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 90;
        /// <summary>
        /// 取出的物品
        /// </summary>
        public CLPFItemInfo item;
        /// <summary>
        /// 物品分配给谁？
        /// </summary>
        public int user_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            item.toBinary(bw);
            bw.Write(user_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item = new CLPFItemInfo();
                item.fromBinary(br);
                user_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会仓库取出物品回应
    /// </summary>
    public sealed class CLPFGuildBagFetchItemAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 91;
        /// <summary>
        /// 0成功 1你不是会长 2玩家不在公会中 3物品数量不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前取出仓库动作所对应的日志
        /// </summary>
        public CLPFGuildBagLog bag_log;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bag_log.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                bag_log = new CLPFGuildBagLog();
                bag_log.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公会领取仓库物品通知
    /// </summary>
    public sealed class CLPFGuildBagFetchItemNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 92;
        /// <summary>
        /// 领取的物品，仅用于显示
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 消息广播通知
    /// </summary>
    public sealed class CLPFMessageBroadcastNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 93;
        /// <summary>
        /// 消息类型 1击杀得金币:昵称-vip等级-鱼配置Id-炮倍-获得金币 2击杀得弹头:昵称-vip等级-鱼配置Id-物品Id-物品子Id-数量 3抽奖得弹头:昵称-vip等级-物品Id-物品子Id-数量
        /// </summary>
        public int type;
        /// <summary>
        /// 字符串参数1
        /// </summary>
        public string str1;  //max:256
        /// <summary>
        /// 字符串参数2
        /// </summary>
        public string str2;  //max:256
        /// <summary>
        /// 整数参数1
        /// </summary>
        public int num1;
        /// <summary>
        /// 整数参数2
        /// </summary>
        public int num2;
        /// <summary>
        /// 整数参数3
        /// </summary>
        public int num3;
        /// <summary>
        /// 整数参数4
        /// </summary>
        public int num4;
        /// <summary>
        /// 整数参数5
        /// </summary>
        public int num5;
        /// <summary>
        /// 整数参数6
        /// </summary>
        public int num6;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(type);
            NetHelper.SafeWriteString(bw, str1, 256);
            NetHelper.SafeWriteString(bw, str2, 256);
            bw.Write(num1);
            bw.Write(num2);
            bw.Write(num3);
            bw.Write(num4);
            bw.Write(num5);
            bw.Write(num6);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                type = br.ReadInt32();
                str1 = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                str2 = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                num1 = br.ReadInt32();
                num2 = br.ReadInt32();
                num3 = br.ReadInt32();
                num4 = br.ReadInt32();
                num5 = br.ReadInt32();
                num6 = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 完成任务信息
    /// </summary>
    public sealed class CLPFTaskInfo : INetProtocol
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int task_id;
        /// <summary>
        /// 达成数量
        /// </summary>
        public int achieve_num;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(task_id);
            bw.Write(achieve_num);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                task_id = br.ReadInt32();
                achieve_num = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询任务请求
    /// </summary>
    public sealed class CLPFTaskQueryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 94;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询任务回应
    /// </summary>
    public sealed class CLPFTaskQueryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 95;
        /// <summary>
        /// 任务信息数组长度
        /// </summary>
        public int task_info_len;
        /// <summary>
        /// 任务信息数组
        /// </summary>
        public CLPFTaskInfo[] task_info_array;  //max:100
        /// <summary>
        /// 已完成（并领取奖励）的任务Id数组长度
        /// </summary>
        public int finish_task_id_len;
        /// <summary>
        /// 已完成（并领取奖励）的任务Id数组
        /// </summary>
        public int[] finish_task_id_array;  //max:100
        /// <summary>
        /// 当前日活跃值
        /// </summary>
        public int daily_active_value;
        /// <summary>
        /// 当前周活跃值
        /// </summary>
        public int weekly_active_value;
        /// <summary>
        /// 已领取奖励的活跃值Id数组长度
        /// </summary>
        public int finish_active_id_len;
        /// <summary>
        /// 已领取奖励的活跃值Id数组
        /// </summary>
        public int[] finish_active_id_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(task_info_len);
            for (int i = 0; i < (int)task_info_len; i++)
                task_info_array[i].toBinary(bw);
            bw.Write(finish_task_id_len);
            for (int i = 0; i < (int)finish_task_id_len; i++)
                bw.Write(finish_task_id_array[i]);
            bw.Write(daily_active_value);
            bw.Write(weekly_active_value);
            bw.Write(finish_active_id_len);
            for (int i = 0; i < (int)finish_active_id_len; i++)
                bw.Write(finish_active_id_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                task_info_len = br.ReadInt32();
                task_info_array = new CLPFTaskInfo[(int)task_info_len];
                for (int i = 0; i < task_info_array.Length; i++)
                {
                    task_info_array[i] = new CLPFTaskInfo();
                    task_info_array[i].fromBinary(br);
                }
                finish_task_id_len = br.ReadInt32();
                finish_task_id_array = new int[(int)finish_task_id_len];
                for (int i = 0; i < finish_task_id_array.Length; i++)
                    finish_task_id_array[i] = br.ReadInt32();
                daily_active_value = br.ReadInt32();
                weekly_active_value = br.ReadInt32();
                finish_active_id_len = br.ReadInt32();
                finish_active_id_array = new int[(int)finish_active_id_len];
                for (int i = 0; i < finish_active_id_array.Length; i++)
                    finish_active_id_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取任务奖励请求
    /// </summary>
    public sealed class CLPFTaskFetchTaskRewardsReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 96;
        /// <summary>
        /// 任务Id
        /// </summary>
        public int task_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(task_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                task_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取任务奖励回应
    /// </summary>
    public sealed class CLPFTaskFetchTaskRewardsAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 97;
        /// <summary>
        /// 0成功 1任务不存在 2任务目标未达成 3任务奖励已领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取活跃度奖励请求
    /// </summary>
    public sealed class CLPFTaskFetchActiveRewardsReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 98;
        /// <summary>
        /// 活跃度奖励Id
        /// </summary>
        public int active_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(active_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                active_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取活跃度奖励回应
    /// </summary>
    public sealed class CLPFTaskFetchActiveRewardsAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 99;
        /// <summary>
        /// 0成功 1不存在 2目标未达成 3已领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 完成的成就任务数据
    /// </summary>
    public sealed class CLPFTaskAchieveData : INetProtocol
    {
        /// <summary>
        /// 1累计登录 2捕鱼能手 3倍率大人 4竞技高手
        /// </summary>
        public int kind;
        /// <summary>
        /// 累计完成数量
        /// </summary>
        public int count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(kind);
            bw.Write(count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                kind = br.ReadInt32();
                count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 获取成就任务信息请求
    /// </summary>
    public sealed class CLPFTaskAchieveQueryInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 100;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 获取成就任务信息回应
    /// </summary>
    public sealed class CLPFTaskAchieveQueryInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 101;
        /// <summary>
        /// 任务数据数组长度
        /// </summary>
        public int data_len;
        /// <summary>
        /// 任务数据数组
        /// </summary>
        public CLPFTaskAchieveData[] data_array;  //max:100
        /// <summary>
        /// 完成的任务Id数组长度
        /// </summary>
        public int finish_id_len;
        /// <summary>
        /// 完成的任务Id数组
        /// </summary>
        public int[] finish_id_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(data_len);
            for (int i = 0; i < (int)data_len; i++)
                data_array[i].toBinary(bw);
            bw.Write(finish_id_len);
            for (int i = 0; i < (int)finish_id_len; i++)
                bw.Write(finish_id_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                data_len = br.ReadInt32();
                data_array = new CLPFTaskAchieveData[(int)data_len];
                for (int i = 0; i < data_array.Length; i++)
                {
                    data_array[i] = new CLPFTaskAchieveData();
                    data_array[i].fromBinary(br);
                }
                finish_id_len = br.ReadInt32();
                finish_id_array = new int[(int)finish_id_len];
                for (int i = 0; i < finish_id_array.Length; i++)
                    finish_id_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取成就任务奖励请求
    /// </summary>
    public sealed class CLPFTaskAchieveFetchRewardReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 102;
        /// <summary>
        /// 成就任务Id
        /// </summary>
        public int task_achieve_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(task_achieve_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                task_achieve_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取成就任务奖励回应
    /// </summary>
    public sealed class CLPFTaskAchieveFetchRewardAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 103;
        /// <summary>
        /// 0成功 1任务不存在 2任务未达成 3奖励已领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取月卡奖励请求
    /// </summary>
    public sealed class CLPFMonthCardFetchRewardReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 104;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 领取月卡奖励回应
    /// </summary>
    public sealed class CLPFMonthCardFetchRewardAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 105;
        /// <summary>
        /// 0成功 1请先购买月卡 2当日已领取 3领取次数不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取救济金请求
    /// </summary>
    public sealed class CLPFReliefGoldFetchReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 106;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 领取救济金回应
    /// </summary>
    public sealed class CLPFReliefGoldFetchAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 107;
        /// <summary>
        /// 0成功 1当前金币不为零 2领取次数已达上限 3你的金库中还有金币不能领取救济金
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 领取到的救济金数量，仅用于显示
        /// </summary>
        public int currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 摇数字获取信息请求
    /// </summary>
    public sealed class CLPFShakeNumberQueryInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 108;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 摇数字获取信息回应
    /// </summary>
    public sealed class CLPFShakeNumberQueryInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 109;
        /// <summary>
        /// 已摇到的数字数组长度
        /// </summary>
        public sbyte shake_number_len;
        /// <summary>
        /// 已经摇到的数字数组，该数字由两部分组成：个位表示当天摇到的数字，十位为0表示当天尚未领取宝箱奖励，十位为1表示当天已领取宝箱奖励
        /// </summary>
        public sbyte[] shake_number_array;  //max:7
        /// <summary>
        /// 当天是否已摇过数字 1是0否
        /// </summary>
        public sbyte shake_number_act_flag;
        /// <summary>
        /// 本轮是否已领取过奖励 1是0否
        /// </summary>
        public sbyte shake_number_fetched;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(shake_number_len);
            for (int i = 0; i < (int)shake_number_len; i++)
                bw.Write(shake_number_array[i]);
            bw.Write(shake_number_act_flag);
            bw.Write(shake_number_fetched);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                shake_number_len = br.ReadSByte();
                shake_number_array = new sbyte[(int)shake_number_len];
                for (int i = 0; i < shake_number_array.Length; i++)
                    shake_number_array[i] = br.ReadSByte();
                shake_number_act_flag = br.ReadSByte();
                shake_number_fetched = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 摇数字请求
    /// </summary>
    public sealed class CLPFShakeNumberActReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 110;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 摇数字回应
    /// </summary>
    public sealed class CLPFShakeNumberActAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 111;
        /// <summary>
        /// 0成功 1今天已经摇过 2有奖励尚未领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 摇到的数字：0~9
        /// </summary>
        public sbyte number;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(number);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                number = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取摇到的金币奖励请求
    /// </summary>
    public sealed class CLPFShakeNumberFetchRewardReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 112;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 领取摇到的金币奖励回应
    /// </summary>
    public sealed class CLPFShakeNumberFetchRewardAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 113;
        /// <summary>
        /// 0成功 1条件未达成 2已领取过
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 领取到的7日摇数字奖励金币数量，仅用于显示
        /// </summary>
        public int currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取摇到数字后的宝箱礼包请求
    /// </summary>
    public sealed class CLPFShakeNumberFetchBoxRewardReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 114;
        /// <summary>
        /// 第几天的宝箱？范围：0-6
        /// </summary>
        public int day;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(day);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                day = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取摇到数字后的宝箱礼包回应
    /// </summary>
    public sealed class CLPFShakeNumberFetchBoxRewardAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 115;
        /// <summary>
        /// 0成功 1参数非法 2当天还未摇数字 3已领取过
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组，仅用于显示
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询每日充值请求
    /// </summary>
    public sealed class CLPFRechargeDailyQueryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 116;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询每日充值回应
    /// </summary>
    public sealed class CLPFRechargeDailyQueryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 117;
        /// <summary>
        /// 已完成的每日充值id数组长度
        /// </summary>
        public int finished_id_len;
        /// <summary>
        /// 已完成的每日充值id数组
        /// </summary>
        public int[] finished_id_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(finished_id_len);
            for (int i = 0; i < (int)finished_id_len; i++)
                bw.Write(finished_id_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                finished_id_len = br.ReadInt32();
                finished_id_array = new int[(int)finished_id_len];
                for (int i = 0; i < finished_id_array.Length; i++)
                    finished_id_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 福利猪获取信息请求
    /// </summary>
    public sealed class CLPFWelfarePigQueryInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 118;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 福利猪获取信息回应
    /// </summary>
    public sealed class CLPFWelfarePigQueryInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 119;
        /// <summary>
        /// 累积的总福利值
        /// </summary>
        public int welfare;
        /// <summary>
        /// 总福利过期时间戳
        /// </summary>
        public UInt32 expire_time;
        /// <summary>
        /// 今日是否已领取锤子碎片 1是0否
        /// </summary>
        public sbyte is_fetched;
        /// <summary>
        /// 今日是否已砸过罐子 1是0否
        /// </summary>
        public sbyte is_broken;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(welfare);
            bw.Write(expire_time);
            bw.Write(is_fetched);
            bw.Write(is_broken);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                welfare = br.ReadInt32();
                expire_time = br.ReadUInt32();
                is_fetched = br.ReadSByte();
                is_broken = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 福利猪领取每日锤子碎片请求
    /// </summary>
    public sealed class CLPFWelfarePigFetchMaterialReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 120;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 福利猪领取每日锤子碎片回应
    /// </summary>
    public sealed class CLPFWelfarePigFetchMaterialAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 121;
        /// <summary>
        /// 0成功 1当日已领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 领取到的物品信息
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 福利猪砸罐子请求
    /// </summary>
    public sealed class CLPFWelfarePigBrokenReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 122;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 福利猪砸罐子回应
    /// </summary>
    public sealed class CLPFWelfarePigBrokenAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 123;
        /// <summary>
        /// 0成功 1当日已砸过 2罐子中金币数量太少 3锤子碎片不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 获得的金币数量，仅用于显示
        /// </summary>
        public int currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 福利猪搜一搜请求
    /// </summary>
    public sealed class CLPFWelfarePigSearchReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 124;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 福利猪搜一搜回应
    /// </summary>
    public sealed class CLPFWelfarePigSearchAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 125;
        /// <summary>
        /// 0搜索成功 1阶段不对 2很遗憾啥也没搜到
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 搜索到的罐子里砸开后获得的金币数量，仅用于显示
        /// </summary>
        public int currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询投资炮倍信息请求
    /// </summary>
    public sealed class CLPFInvestGunQueryInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 126;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询投资炮倍信息回应
    /// </summary>
    public sealed class CLPFInvestGunQueryInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 127;
        /// <summary>
        /// 已完成的最大投资充值Id，关联InvestGunRecharge.xlsx表主键
        /// </summary>
        public int max_recharge_id;
        /// <summary>
        /// 已解锁的最大炮值
        /// </summary>
        public int max_gun_value;
        /// <summary>
        /// 已领取的解锁炮倍数组长度
        /// </summary>
        public int finished_len;
        /// <summary>
        /// 已领取的解锁炮倍数组，对应InvestGunReward.xlsx表主键
        /// </summary>
        public int[] finished_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(max_recharge_id);
            bw.Write(max_gun_value);
            bw.Write(finished_len);
            for (int i = 0; i < (int)finished_len; i++)
                bw.Write(finished_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                max_recharge_id = br.ReadInt32();
                max_gun_value = br.ReadInt32();
                finished_len = br.ReadInt32();
                finished_array = new int[(int)finished_len];
                for (int i = 0; i < finished_array.Length; i++)
                    finished_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取投资炮倍奖励请求
    /// </summary>
    public sealed class CLPFInvestGunFetchRewardReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 128;
        /// <summary>
        /// 领取奖励的解锁炮倍
        /// </summary>
        public int gun_value;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(gun_value);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                gun_value = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取投资炮倍奖励回应
    /// </summary>
    public sealed class CLPFInvestGunFetchRewardAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 129;
        /// <summary>
        /// 0成功 1无此项 2炮值解锁条件不足 3充值条件不足 4奖励已领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组，仅用于显示
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询出海保险信息请求
    /// </summary>
    public sealed class CLPFInvestCostQueryInfoReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 130;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询出海保险信息回应
    /// </summary>
    public sealed class CLPFInvestCostQueryInfoAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 131;
        /// <summary>
        /// 是否已完成充值 1是0否
        /// </summary>
        public sbyte is_recharged;
        /// <summary>
        /// 累计金币总消耗
        /// </summary>
        public Int64 total_currency_cost;
        /// <summary>
        /// 已领取的奖励Id数组长度
        /// </summary>
        public int finished_len;
        /// <summary>
        /// 已领取的奖励Id数组，对应InvestCostReward.xlsx表主键
        /// </summary>
        public int[] finished_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(is_recharged);
            bw.Write(total_currency_cost);
            bw.Write(finished_len);
            for (int i = 0; i < (int)finished_len; i++)
                bw.Write(finished_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                is_recharged = br.ReadSByte();
                total_currency_cost = br.ReadInt64();
                finished_len = br.ReadInt32();
                finished_array = new int[(int)finished_len];
                for (int i = 0; i < finished_array.Length; i++)
                    finished_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取出海保险奖励请求
    /// </summary>
    public sealed class CLPFInvestCostFetchRewardReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 132;
        /// <summary>
        /// 奖励Id
        /// </summary>
        public int reward_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(reward_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                reward_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取出海保险奖励回应
    /// </summary>
    public sealed class CLPFInvestCostFetchRewardAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 133;
        /// <summary>
        /// 0成功 1无此项 2累计金币消耗不足 3尚未充值 4奖励已领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组，仅用于显示
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 领取新手初始礼包请求
    /// </summary>
    public sealed class CLPFFirstPackageFetchReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 134;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 领取新手初始礼包回应
    /// </summary>
    public sealed class CLPFFirstPackageFetchAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 135;
        /// <summary>
        /// 0成功 1已领取
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组，仅用于显示
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 公告变动通知
    /// </summary>
    public sealed class CLPFAnnouncementChangedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 136;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// vip金币补足通知
    /// </summary>
    public sealed class CLPFVipFillUpCurrencyNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 137;
        /// <summary>
        /// 补足的金币数量，仅用于显示
        /// </summary>
        public Int64 currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                currency_delta = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 实物奖励兑换日志
    /// </summary>
    public sealed class CLPFRealGoodsExchangeLog : INetProtocol
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public int goods_id;
        /// <summary>
        /// 商品名称
        /// </summary>
        public string goods_name;  //max:128
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string real_name;  //max:64
        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone;  //max:32
        /// <summary>
        /// 联系地址
        /// </summary>
        public string address;  //max:256
        /// <summary>
        /// 订单状态 0未发货 1已发货 2拒绝发货
        /// </summary>
        public int state;
        /// <summary>
        /// 创建订单时间戳
        /// </summary>
        public UInt32 create_time;
        /// <summary>
        /// 处理订单时间戳
        /// </summary>
        public UInt32 process_time;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(goods_id);
            NetHelper.SafeWriteString(bw, goods_name, 128);
            NetHelper.SafeWriteString(bw, real_name, 64);
            NetHelper.SafeWriteString(bw, phone, 32);
            NetHelper.SafeWriteString(bw, address, 256);
            bw.Write(state);
            bw.Write(create_time);
            bw.Write(process_time);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                goods_id = br.ReadInt32();
                goods_name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                real_name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                address = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                state = br.ReadInt32();
                create_time = br.ReadUInt32();
                process_time = br.ReadUInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询常用的真实地址请求
    /// </summary>
    public sealed class CLPFRealGoodsQueryAddressReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 138;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询常用的真实地址回应
    /// </summary>
    public sealed class CLPFRealGoodsQueryAddressAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 139;
        /// <summary>
        /// 0成功 1不存在
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string real_name;  //max:64
        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone;  //max:32
        /// <summary>
        /// 联系地址
        /// </summary>
        public string address;  //max:256

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            NetHelper.SafeWriteString(bw, real_name, 64);
            NetHelper.SafeWriteString(bw, phone, 32);
            NetHelper.SafeWriteString(bw, address, 256);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                real_name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                address = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 实物奖励下单请求
    /// </summary>
    public sealed class CLPFRealGoodsCreateOrderReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 140;
        /// <summary>
        /// 实物商品Id
        /// </summary>
        public int goods_id;
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string real_name;  //max:64
        /// <summary>
        /// 联系电话
        /// </summary>
        public string phone;  //max:32
        /// <summary>
        /// 联系地址
        /// </summary>
        public string address;  //max:256

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(goods_id);
            NetHelper.SafeWriteString(bw, real_name, 64);
            NetHelper.SafeWriteString(bw, phone, 32);
            NetHelper.SafeWriteString(bw, address, 256);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                goods_id = br.ReadInt32();
                real_name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                address = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 实物奖励下单回应
    /// </summary>
    public sealed class CLPFRealGoodsCreateOrderAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 141;
        /// <summary>
        /// 0成功 1信息填写不完整 2商品不存在 3vip等级不足 4购买次数已达上限 5资源不足
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询实物奖励兑换纪录请求
    /// </summary>
    public sealed class CLPFRealGoodsQueryExchangeLogReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 142;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询实物奖励兑换纪录回应
    /// </summary>
    public sealed class CLPFRealGoodsQueryExchangeLogAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 143;
        /// <summary>
        /// 日志数组长度
        /// </summary>
        public int log_len;
        /// <summary>
        /// 日志数组
        /// </summary>
        public CLPFRealGoodsExchangeLog[] log_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(log_len);
            for (int i = 0; i < (int)log_len; i++)
                log_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                log_len = br.ReadInt32();
                log_array = new CLPFRealGoodsExchangeLog[(int)log_len];
                for (int i = 0; i < log_array.Length; i++)
                {
                    log_array[i] = new CLPFRealGoodsExchangeLog();
                    log_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询已完成的新手引导标记数组请求
    /// </summary>
    public sealed class CLPFGuideDataQueryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 144;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 查询已完成的新手引导标记数组回应
    /// </summary>
    public sealed class CLPFGuideDataQueryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 145;
        /// <summary>
        /// 已完成的新手引导标记数组长度
        /// </summary>
        public int flag_len;
        /// <summary>
        /// 已完成的新手引导标记数组
        /// </summary>
        public int[] flag_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(flag_len);
            for (int i = 0; i < (int)flag_len; i++)
                bw.Write(flag_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                flag_len = br.ReadInt32();
                flag_array = new int[(int)flag_len];
                for (int i = 0; i < flag_array.Length; i++)
                    flag_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 上报完成了某个新手引导标记
    /// </summary>
    public sealed class CLPFGuideDataActRpt : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 146;
        /// <summary>
        /// 新手引导完成标记
        /// </summary>
        public int flag;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(flag);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                flag = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 客户端配置表发布通知
    /// </summary>
    public sealed class CLPFClientConfigPublishNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 147;
        /// <summary>
        /// 最新配置表的md5，里面出现的字母大写
        /// </summary>
        public string md5;  //max:40

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, md5, 40);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                md5 = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 子游戏在线人数信息
    /// </summary>
    public sealed class CLPFSubGamesOnlineCountInfo : INetProtocol
    {
        /// <summary>
        /// 游戏Id
        /// </summary>
        public int group_id;
        /// <summary>
        /// 玩法Id
        /// </summary>
        public int service_id;
        /// <summary>
        /// 在线人数
        /// </summary>
        public int online_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(group_id);
            bw.Write(service_id);
            bw.Write(online_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                group_id = br.ReadInt32();
                service_id = br.ReadInt32();
                online_count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 子游戏在线人数请求
    /// </summary>
    public sealed class CLPFSubGamesOnlineCountReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 148;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 子游戏在线人数回应
    /// </summary>
    public sealed class CLPFSubGamesOnlineCountAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 149;
        /// <summary>
        /// 在线人数数组长度
        /// </summary>
        public byte array_len;
        /// <summary>
        /// 在线人数数组
        /// </summary>
        public CLPFSubGamesOnlineCountInfo[] array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(array_len);
            for (int i = 0; i < (int)array_len; i++)
                array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                array_len = br.ReadByte();
                array = new CLPFSubGamesOnlineCountInfo[(int)array_len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new CLPFSubGamesOnlineCountInfo();
                    array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 激活码领取奖励请求
    /// </summary>
    public sealed class CLPFCdkeyFetchRewardReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 150;
        /// <summary>
        /// 激活码
        /// </summary>
        public string code;  //max:20

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, code, 20);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                code = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 激活码领取奖励回应
    /// </summary>
    public sealed class CLPFCdkeyFetchRewardAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 151;
        /// <summary>
        /// 0成功 1兑换码不存在 2兑换码已被领取 3同一类型的兑换码礼包每个玩家只能领取一次
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组，仅用于显示
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号绑定状态请求
    /// </summary>
    public sealed class CLPFAccountBindStateReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 152;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 账号绑定状态回应
    /// </summary>
    public sealed class CLPFAccountBindStateAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 153;
        /// <summary>
        /// 0成功
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 绑定数组长度
        /// </summary>
        public sbyte bind_type_length;
        /// <summary>
        /// 绑定数组 1游客2手机号3QQ4微信5Facebook6GooglePlay7GameCenter
        /// </summary>
        public sbyte[] bind_type_array;  //max:20

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(bind_type_length);
            for (int i = 0; i < (int)bind_type_length; i++)
                bw.Write(bind_type_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                bind_type_length = br.ReadSByte();
                bind_type_array = new sbyte[(int)bind_type_length];
                for (int i = 0; i < bind_type_array.Length; i++)
                    bind_type_array[i] = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号手机绑定请求
    /// </summary>
    public sealed class CLPFAccountPhoneBindReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 154;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone;  //max:16
        /// <summary>
        /// 短信验证的AppKey
        /// </summary>
        public string sms_app_key;  //max:64
        /// <summary>
        /// 短信验证的区号
        /// </summary>
        public string sms_zone;  //max:10
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string sms_code;  //max:10
        /// <summary>
        /// 登录密码，使用CA3加密，固定秘钥19357
        /// </summary>
        public string password;  //max:64

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, phone, 16);
            NetHelper.SafeWriteString(bw, sms_app_key, 64);
            NetHelper.SafeWriteString(bw, sms_zone, 10);
            NetHelper.SafeWriteString(bw, sms_code, 10);
            NetHelper.SafeWriteString(bw, password, 64);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_app_key = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_zone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_code = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                password = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号手机绑定回应
    /// </summary>
    public sealed class CLPFAccountPhoneBindAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 155;
        /// <summary>
        /// 0成功 1该账号已经绑定过手机号 2密码不合法 3输入的手机号码已绑定其他账号 4短信验证失败
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号手机更换请求1
    /// </summary>
    public sealed class CLPFAccountPhoneChange1Req : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 156;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone;  //max:16
        /// <summary>
        /// 短信验证的AppKey
        /// </summary>
        public string sms_app_key;  //max:64
        /// <summary>
        /// 短信验证的区号
        /// </summary>
        public string sms_zone;  //max:10
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string sms_code;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, phone, 16);
            NetHelper.SafeWriteString(bw, sms_app_key, 64);
            NetHelper.SafeWriteString(bw, sms_zone, 10);
            NetHelper.SafeWriteString(bw, sms_code, 10);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_app_key = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_zone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_code = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号手机更换回应1
    /// </summary>
    public sealed class CLPFAccountPhoneChange1Ack : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 157;
        /// <summary>
        /// 0成功 1请先绑定手机号 2手机号与预留的不一致 3短信验证失败
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号手机更换请求2
    /// </summary>
    public sealed class CLPFAccountPhoneChange2Req : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 158;
        /// <summary>
        /// 新手机号码
        /// </summary>
        public string new_phone;  //max:16
        /// <summary>
        /// 短信验证的AppKey
        /// </summary>
        public string sms_app_key;  //max:64
        /// <summary>
        /// 短信验证的区号
        /// </summary>
        public string sms_zone;  //max:10
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string sms_code;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, new_phone, 16);
            NetHelper.SafeWriteString(bw, sms_app_key, 64);
            NetHelper.SafeWriteString(bw, sms_zone, 10);
            NetHelper.SafeWriteString(bw, sms_code, 10);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                new_phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_app_key = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_zone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_code = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号手机更换回应2
    /// </summary>
    public sealed class CLPFAccountPhoneChange2Ack : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 159;
        /// <summary>
        /// 0成功 1状态不对 2原手机号和新手机号不能一致 3新手机号码已绑定其他账号 4短信验证失败
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号统一绑定请求
    /// </summary>
    public sealed class CLPFAccountUniformBindReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 160;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone;  //max:16
        /// <summary>
        /// 短信验证的AppKey
        /// </summary>
        public string sms_app_key;  //max:64
        /// <summary>
        /// 短信验证的区号
        /// </summary>
        public string sms_zone;  //max:10
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string sms_code;  //max:10
        /// <summary>
        /// 绑定类型 1设备 3QQ 4微信 5Facebook 6GooglePlay 7GameCenter
        /// </summary>
        public byte type;
        /// <summary>
        /// 唯一标识串，使用CA3加密，固定秘钥：19357
        /// </summary>
        public string token;  //max:256

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, phone, 16);
            NetHelper.SafeWriteString(bw, sms_app_key, 64);
            NetHelper.SafeWriteString(bw, sms_zone, 10);
            NetHelper.SafeWriteString(bw, sms_code, 10);
            bw.Write(type);
            NetHelper.SafeWriteString(bw, token, 256);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_app_key = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_zone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_code = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                type = br.ReadByte();
                token = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号统一绑定回应
    /// </summary>
    public sealed class CLPFAccountUniformBindAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 161;
        /// <summary>
        /// 0成功 1请先绑定手机 2手机号与预留的不一致 3绑定类型非法 4token非法 5已绑定该类型，请先解绑 6已关联其他账号 7短信验证失败
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号统一解绑请求
    /// </summary>
    public sealed class CLPFAccountUniformUnbindReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 162;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone;  //max:16
        /// <summary>
        /// 短信验证的AppKey
        /// </summary>
        public string sms_app_key;  //max:64
        /// <summary>
        /// 短信验证的区号
        /// </summary>
        public string sms_zone;  //max:10
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string sms_code;  //max:10
        /// <summary>
        /// 绑定类型 1设备 3QQ 4微信 5Facebook 6GooglePlay 7GameCenter
        /// </summary>
        public byte type;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, phone, 16);
            NetHelper.SafeWriteString(bw, sms_app_key, 64);
            NetHelper.SafeWriteString(bw, sms_zone, 10);
            NetHelper.SafeWriteString(bw, sms_code, 10);
            bw.Write(type);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_app_key = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_zone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_code = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                type = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 账号统一解绑回应
    /// </summary>
    public sealed class CLPFAccountUniformUnbindAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 163;
        /// <summary>
        /// 0成功 1请先绑定手机 2手机号与预留的不一致 3解绑类型非法 4还未绑定该类型 5短信验证失败
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询玩家昵称请求
    /// </summary>
    public sealed class CLPFPlayerNicknameQueryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 164;
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(user_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询玩家昵称回应
    /// </summary>
    public sealed class CLPFPlayerNicknameQueryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 165;
        /// <summary>
        /// 0成功 1玩家不存在
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;  //max:32

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            NetHelper.SafeWriteString(bw, nickname, 32);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码初始化请求 注意：该请求成功后，不用再进行密码验证功能了，可以直接进入金库界面
    /// </summary>
    public sealed class CLPFBankPasswordInitReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 166;
        /// <summary>
        /// 金库初始密码，使用CA3加密，固定秘钥：19357
        /// </summary>
        public string password;  //max:64

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, password, 64);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                password = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码初始化回应
    /// </summary>
    public sealed class CLPFBankPasswordInitAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 167;
        /// <summary>
        /// 0成功 1请先绑定手机 2已设置过初始密码 3密码不合法
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码验证请求
    /// </summary>
    public sealed class CLPFBankPasswordVerifyReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 168;
        /// <summary>
        /// 金库密码，使用CA3加密，固定秘钥：19357
        /// </summary>
        public string password;  //max:64

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, password, 64);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                password = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码验证回应
    /// </summary>
    public sealed class CLPFBankPasswordVerifyAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 169;
        /// <summary>
        /// 0成功 1请先绑定手机 2请先设置密码 3密码验证失败
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码修改请求
    /// </summary>
    public sealed class CLPFBankPasswordModifyReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 170;
        /// <summary>
        /// 原密码，使用CA3加密，固定秘钥：19357
        /// </summary>
        public string origin_password;  //max:64
        /// <summary>
        /// 新密码，使用CA3加密，固定秘钥：19357
        /// </summary>
        public string new_password;  //max:64

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, origin_password, 64);
            NetHelper.SafeWriteString(bw, new_password, 64);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                origin_password = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                new_password = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码修改回应
    /// </summary>
    public sealed class CLPFBankPasswordModifyAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 171;
        /// <summary>
        /// 0成功 1请先绑定手机 2请先设置密码 3原密码不正确 4新密码不合法
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码重置请求 注意：该请求成功后，不用再进行密码验证功能了，可以直接进入金库界面
    /// </summary>
    public sealed class CLPFBankPasswordResetReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 172;
        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone;  //max:16
        /// <summary>
        /// 短信验证的AppKey
        /// </summary>
        public string sms_app_key;  //max:64
        /// <summary>
        /// 短信验证的区号
        /// </summary>
        public string sms_zone;  //max:10
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string sms_code;  //max:10
        /// <summary>
        /// 新密码，使用CA3加密，固定秘钥：19357
        /// </summary>
        public string new_password;  //max:64

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            NetHelper.SafeWriteString(bw, phone, 16);
            NetHelper.SafeWriteString(bw, sms_app_key, 64);
            NetHelper.SafeWriteString(bw, sms_zone, 10);
            NetHelper.SafeWriteString(bw, sms_code, 10);
            NetHelper.SafeWriteString(bw, new_password, 64);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_app_key = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_zone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                sms_code = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                new_password = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库密码重置回应
    /// </summary>
    public sealed class CLPFBankPasswordResetAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 173;
        /// <summary>
        /// 0成功 1请先绑定手机 2手机号与预留的不一致 3还未设置过密码 4新密码不合法 5短信验证失败
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品查询请求
    /// </summary>
    public sealed class CLPFBankItemQueryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 174;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 金库物品查询回应
    /// </summary>
    public sealed class CLPFBankItemQueryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 175;
        /// <summary>
        /// 0成功 1权限不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLPFItemInfo[] items;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                item_len = br.ReadInt32();
                items = new CLPFItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLPFItemInfo();
                    items[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品存入请求
    /// </summary>
    public sealed class CLPFBankItemStoreReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 176;
        /// <summary>
        /// 要存入的物品
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品存入回应
    /// </summary>
    public sealed class CLPFBankItemStoreAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 177;
        /// <summary>
        /// 0成功 1权限不足 2参数无效 3资源数量不足
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品取出请求
    /// </summary>
    public sealed class CLPFBankItemFetchReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 178;
        /// <summary>
        /// 要取出的物品
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品取出回应
    /// </summary>
    public sealed class CLPFBankItemFetchAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 179;
        /// <summary>
        /// 0成功 1权限不足 2参数无效 3资源数量不足
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品赠送请求
    /// </summary>
    public sealed class CLPFBankItemSendReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 180;
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 要赠送的物品
        /// </summary>
        public CLPFItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(user_id);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                user_id = br.ReadInt32();
                item = new CLPFItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品赠送回应
    /// </summary>
    public sealed class CLPFBankItemSendAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 181;
        /// <summary>
        /// 0成功 1权限不足 2参数无效 3资源数量不足 4目标玩家不存在 5对方等级不足 6赠送的资源数量太少
        /// </summary>
        public sbyte errcode;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品日志信息
    /// </summary>
    public sealed class CLPFBankItemLogInfo : INetProtocol
    {
        /// <summary>
        /// 1存入 2取出 3赠送 4接收
        /// </summary>
        public sbyte log_type;
        /// <summary>
        /// 物品信息
        /// </summary>
        public CLPFItemInfo item;
        /// <summary>
        /// 关联的玩家Id
        /// </summary>
        public int refer_user_id;
        /// <summary>
        /// 关联的玩家昵称
        /// </summary>
        public string refer_nickname;  //max:32
        /// <summary>
        /// 时间戳
        /// </summary>
        public UInt32 timestamp;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(log_type);
            item.toBinary(bw);
            bw.Write(refer_user_id);
            NetHelper.SafeWriteString(bw, refer_nickname, 32);
            bw.Write(timestamp);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                log_type = br.ReadSByte();
                item = new CLPFItemInfo();
                item.fromBinary(br);
                refer_user_id = br.ReadInt32();
                refer_nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                timestamp = br.ReadUInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 金库物品日志查询请求
    /// </summary>
    public sealed class CLPFBankItemLogQueryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 182;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }
    /// <summary>
    /// 金库物品日志查询回应
    /// </summary>
    public sealed class CLPFBankItemLogQueryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 2;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 183;
        /// <summary>
        /// 0成功 1权限不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 日志数组长度
        /// </summary>
        public int log_length;
        /// <summary>
        /// 日志数组
        /// </summary>
        public CLPFBankItemLogInfo[] log_array;  //max:15

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(log_length);
            for (int i = 0; i < (int)log_length; i++)
                log_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                log_length = br.ReadInt32();
                log_array = new CLPFBankItemLogInfo[(int)log_length];
                for (int i = 0; i < log_array.Length; i++)
                {
                    log_array[i] = new CLPFBankItemLogInfo();
                    log_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }

    public class ClientPFResponserBase : INetResponser
    {
        public bool processPackage(BinaryReader br, INetReactor reactor, out INetProtocol responseProto)
        {
            responseProto = null;
            if (br.ReadUInt16() != 2)
                return false;

            switch(br.ReadUInt16())
            {
                case 0:
                    responseProto = new CLPFLogoutReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFLogoutReq(responseProto as CLPFLogoutReq);
                    break;
                case 1:
                    responseProto = new CLPFResSyncNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFResSyncNtf(responseProto as CLPFResSyncNtf);
                    break;
                case 2:
                    responseProto = new CLPFResChangedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFResChangedNtf(responseProto as CLPFResChangedNtf);
                    break;
                case 3:
                    responseProto = new CLPFItemGetListReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFItemGetListReq(responseProto as CLPFItemGetListReq);
                    break;
                case 4:
                    responseProto = new CLPFItemGetListAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFItemGetListAck(responseProto as CLPFItemGetListAck);
                    break;
                case 5:
                    responseProto = new CLPFItemUseReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFItemUseReq(responseProto as CLPFItemUseReq);
                    break;
                case 6:
                    responseProto = new CLPFItemUseAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFItemUseAck(responseProto as CLPFItemUseAck);
                    break;
                case 7:
                    responseProto = new CLPFItemCountChangeNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFItemCountChangeNtf(responseProto as CLPFItemCountChangeNtf);
                    break;
                case 8:
                    responseProto = new CLPFItemBuyReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFItemBuyReq(responseProto as CLPFItemBuyReq);
                    break;
                case 9:
                    responseProto = new CLPFItemBuyAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFItemBuyAck(responseProto as CLPFItemBuyAck);
                    break;
                case 10:
                    responseProto = new CLPFShopQueryBuyCountReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShopQueryBuyCountReq(responseProto as CLPFShopQueryBuyCountReq);
                    break;
                case 11:
                    responseProto = new CLPFShopQueryBuyCountAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShopQueryBuyCountAck(responseProto as CLPFShopQueryBuyCountAck);
                    break;
                case 12:
                    responseProto = new CLPFShopBuyReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShopBuyReq(responseProto as CLPFShopBuyReq);
                    break;
                case 13:
                    responseProto = new CLPFShopBuyAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShopBuyAck(responseProto as CLPFShopBuyAck);
                    break;
                case 14:
                    responseProto = new CLPFRechargeReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRechargeReq(responseProto as CLPFRechargeReq);
                    break;
                case 15:
                    responseProto = new CLPFRechargeAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRechargeAck(responseProto as CLPFRechargeAck);
                    break;
                case 16:
                    responseProto = new CLPFRechargeSuccessNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRechargeSuccessNtf(responseProto as CLPFRechargeSuccessNtf);
                    break;
                case 17:
                    responseProto = new CLPFGetRankListReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGetRankListReq(responseProto as CLPFGetRankListReq);
                    break;
                case 18:
                    responseProto = new CLPFGetRankListAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGetRankListAck(responseProto as CLPFGetRankListAck);
                    break;
                case 19:
                    responseProto = new CLPFLevelExpChangedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFLevelExpChangedNtf(responseProto as CLPFLevelExpChangedNtf);
                    break;
                case 20:
                    responseProto = new CLPFLevelUpNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFLevelUpNtf(responseProto as CLPFLevelUpNtf);
                    break;
                case 21:
                    responseProto = new CLPFVipExpChangedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFVipExpChangedNtf(responseProto as CLPFVipExpChangedNtf);
                    break;
                case 22:
                    responseProto = new CLPFModifyNicknameReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFModifyNicknameReq(responseProto as CLPFModifyNicknameReq);
                    break;
                case 23:
                    responseProto = new CLPFModifyNicknameAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFModifyNicknameAck(responseProto as CLPFModifyNicknameAck);
                    break;
                case 24:
                    responseProto = new CLPFModifyHeadReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFModifyHeadReq(responseProto as CLPFModifyHeadReq);
                    break;
                case 25:
                    responseProto = new CLPFModifyHeadAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFModifyHeadAck(responseProto as CLPFModifyHeadAck);
                    break;
                case 26:
                    responseProto = new CLPFQuerySignReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFQuerySignReq(responseProto as CLPFQuerySignReq);
                    break;
                case 27:
                    responseProto = new CLPFQuerySignAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFQuerySignAck(responseProto as CLPFQuerySignAck);
                    break;
                case 28:
                    responseProto = new CLPFActSignReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFActSignReq(responseProto as CLPFActSignReq);
                    break;
                case 29:
                    responseProto = new CLPFActSignAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFActSignAck(responseProto as CLPFActSignAck);
                    break;
                case 30:
                    responseProto = new CLPFQueryVipWheelReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFQueryVipWheelReq(responseProto as CLPFQueryVipWheelReq);
                    break;
                case 31:
                    responseProto = new CLPFQueryVipWheelAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFQueryVipWheelAck(responseProto as CLPFQueryVipWheelAck);
                    break;
                case 32:
                    responseProto = new CLPFActVipWheelReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFActVipWheelReq(responseProto as CLPFActVipWheelReq);
                    break;
                case 33:
                    responseProto = new CLPFActVipWheelAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFActVipWheelAck(responseProto as CLPFActVipWheelAck);
                    break;
                case 34:
                    responseProto = new CLPFMailQueryAllIdsReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailQueryAllIdsReq(responseProto as CLPFMailQueryAllIdsReq);
                    break;
                case 35:
                    responseProto = new CLPFMailQueryAllIdsAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailQueryAllIdsAck(responseProto as CLPFMailQueryAllIdsAck);
                    break;
                case 36:
                    responseProto = new CLPFMailBatchQueryContentReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailBatchQueryContentReq(responseProto as CLPFMailBatchQueryContentReq);
                    break;
                case 37:
                    responseProto = new CLPFMailBatchQueryContentAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailBatchQueryContentAck(responseProto as CLPFMailBatchQueryContentAck);
                    break;
                case 38:
                    responseProto = new CLPFMailAccessReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailAccessReq(responseProto as CLPFMailAccessReq);
                    break;
                case 39:
                    responseProto = new CLPFMailAccessAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailAccessAck(responseProto as CLPFMailAccessAck);
                    break;
                case 40:
                    responseProto = new CLPFMailFetchItemReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailFetchItemReq(responseProto as CLPFMailFetchItemReq);
                    break;
                case 41:
                    responseProto = new CLPFMailFetchItemAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailFetchItemAck(responseProto as CLPFMailFetchItemAck);
                    break;
                case 42:
                    responseProto = new CLPFMailRemoveReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailRemoveReq(responseProto as CLPFMailRemoveReq);
                    break;
                case 43:
                    responseProto = new CLPFMailRemoveAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailRemoveAck(responseProto as CLPFMailRemoveAck);
                    break;
                case 44:
                    responseProto = new CLPFMailArriveNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMailArriveNtf(responseProto as CLPFMailArriveNtf);
                    break;
                case 45:
                    responseProto = new CLPFGuildCreateReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildCreateReq(responseProto as CLPFGuildCreateReq);
                    break;
                case 46:
                    responseProto = new CLPFGuildCreateAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildCreateAck(responseProto as CLPFGuildCreateAck);
                    break;
                case 47:
                    responseProto = new CLPFGuildQueryRecommendListReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryRecommendListReq(responseProto as CLPFGuildQueryRecommendListReq);
                    break;
                case 48:
                    responseProto = new CLPFGuildQueryRecommendListAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryRecommendListAck(responseProto as CLPFGuildQueryRecommendListAck);
                    break;
                case 49:
                    responseProto = new CLPFGuildSearchReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildSearchReq(responseProto as CLPFGuildSearchReq);
                    break;
                case 50:
                    responseProto = new CLPFGuildSearchAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildSearchAck(responseProto as CLPFGuildSearchAck);
                    break;
                case 51:
                    responseProto = new CLPFGuildQuickJoinReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQuickJoinReq(responseProto as CLPFGuildQuickJoinReq);
                    break;
                case 52:
                    responseProto = new CLPFGuildQuickJoinAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQuickJoinAck(responseProto as CLPFGuildQuickJoinAck);
                    break;
                case 53:
                    responseProto = new CLPFGuildJoinReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildJoinReq(responseProto as CLPFGuildJoinReq);
                    break;
                case 54:
                    responseProto = new CLPFGuildJoinAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildJoinAck(responseProto as CLPFGuildJoinAck);
                    break;
                case 55:
                    responseProto = new CLPFGuildQueryJoinListReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryJoinListReq(responseProto as CLPFGuildQueryJoinListReq);
                    break;
                case 56:
                    responseProto = new CLPFGuildQueryJoinListAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryJoinListAck(responseProto as CLPFGuildQueryJoinListAck);
                    break;
                case 57:
                    responseProto = new CLPFGuildHandleJoinReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildHandleJoinReq(responseProto as CLPFGuildHandleJoinReq);
                    break;
                case 58:
                    responseProto = new CLPFGuildHandleJoinAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildHandleJoinAck(responseProto as CLPFGuildHandleJoinAck);
                    break;
                case 59:
                    responseProto = new CLPFGuildJoinResponseNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildJoinResponseNtf(responseProto as CLPFGuildJoinResponseNtf);
                    break;
                case 60:
                    responseProto = new CLPFGuildNewJoinRequestNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildNewJoinRequestNtf(responseProto as CLPFGuildNewJoinRequestNtf);
                    break;
                case 61:
                    responseProto = new CLPFGuildQueryInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryInfoReq(responseProto as CLPFGuildQueryInfoReq);
                    break;
                case 62:
                    responseProto = new CLPFGuildQueryInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryInfoAck(responseProto as CLPFGuildQueryInfoAck);
                    break;
                case 63:
                    responseProto = new CLPFGuildModifyInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildModifyInfoReq(responseProto as CLPFGuildModifyInfoReq);
                    break;
                case 64:
                    responseProto = new CLPFGuildModifyInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildModifyInfoAck(responseProto as CLPFGuildModifyInfoAck);
                    break;
                case 65:
                    responseProto = new CLPFGuildModifyMemberJobReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildModifyMemberJobReq(responseProto as CLPFGuildModifyMemberJobReq);
                    break;
                case 66:
                    responseProto = new CLPFGuildModifyMemberJobAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildModifyMemberJobAck(responseProto as CLPFGuildModifyMemberJobAck);
                    break;
                case 67:
                    responseProto = new CLPFGuildKickMemberReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildKickMemberReq(responseProto as CLPFGuildKickMemberReq);
                    break;
                case 68:
                    responseProto = new CLPFGuildKickMemberAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildKickMemberAck(responseProto as CLPFGuildKickMemberAck);
                    break;
                case 69:
                    responseProto = new CLPFGuildKickMemberNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildKickMemberNtf(responseProto as CLPFGuildKickMemberNtf);
                    break;
                case 70:
                    responseProto = new CLPFGuildExitReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildExitReq(responseProto as CLPFGuildExitReq);
                    break;
                case 71:
                    responseProto = new CLPFGuildExitAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildExitAck(responseProto as CLPFGuildExitAck);
                    break;
                case 72:
                    responseProto = new CLPFGuildUpgradeReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildUpgradeReq(responseProto as CLPFGuildUpgradeReq);
                    break;
                case 73:
                    responseProto = new CLPFGuildUpgradeAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildUpgradeAck(responseProto as CLPFGuildUpgradeAck);
                    break;
                case 74:
                    responseProto = new CLPFGuildQueryWelfareReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryWelfareReq(responseProto as CLPFGuildQueryWelfareReq);
                    break;
                case 75:
                    responseProto = new CLPFGuildQueryWelfareAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryWelfareAck(responseProto as CLPFGuildQueryWelfareAck);
                    break;
                case 76:
                    responseProto = new CLPFGuildFetchWelfareReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildFetchWelfareReq(responseProto as CLPFGuildFetchWelfareReq);
                    break;
                case 77:
                    responseProto = new CLPFGuildFetchWelfareAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildFetchWelfareAck(responseProto as CLPFGuildFetchWelfareAck);
                    break;
                case 78:
                    responseProto = new CLPFGuildQueryRedPacketInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryRedPacketInfoReq(responseProto as CLPFGuildQueryRedPacketInfoReq);
                    break;
                case 79:
                    responseProto = new CLPFGuildQueryRedPacketInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryRedPacketInfoAck(responseProto as CLPFGuildQueryRedPacketInfoAck);
                    break;
                case 80:
                    responseProto = new CLPFGuildQueryRedPacketRankReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryRedPacketRankReq(responseProto as CLPFGuildQueryRedPacketRankReq);
                    break;
                case 81:
                    responseProto = new CLPFGuildQueryRedPacketRankAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildQueryRedPacketRankAck(responseProto as CLPFGuildQueryRedPacketRankAck);
                    break;
                case 82:
                    responseProto = new CLPFGuildActRedPacketReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildActRedPacketReq(responseProto as CLPFGuildActRedPacketReq);
                    break;
                case 83:
                    responseProto = new CLPFGuildActRedPacketAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildActRedPacketAck(responseProto as CLPFGuildActRedPacketAck);
                    break;
                case 84:
                    responseProto = new CLPFGuildBagQueryInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagQueryInfoReq(responseProto as CLPFGuildBagQueryInfoReq);
                    break;
                case 85:
                    responseProto = new CLPFGuildBagQueryInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagQueryInfoAck(responseProto as CLPFGuildBagQueryInfoAck);
                    break;
                case 86:
                    responseProto = new CLPFGuildBagQueryLogReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagQueryLogReq(responseProto as CLPFGuildBagQueryLogReq);
                    break;
                case 87:
                    responseProto = new CLPFGuildBagQueryLogAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagQueryLogAck(responseProto as CLPFGuildBagQueryLogAck);
                    break;
                case 88:
                    responseProto = new CLPFGuildBagStoreItemReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagStoreItemReq(responseProto as CLPFGuildBagStoreItemReq);
                    break;
                case 89:
                    responseProto = new CLPFGuildBagStoreItemAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagStoreItemAck(responseProto as CLPFGuildBagStoreItemAck);
                    break;
                case 90:
                    responseProto = new CLPFGuildBagFetchItemReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagFetchItemReq(responseProto as CLPFGuildBagFetchItemReq);
                    break;
                case 91:
                    responseProto = new CLPFGuildBagFetchItemAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagFetchItemAck(responseProto as CLPFGuildBagFetchItemAck);
                    break;
                case 92:
                    responseProto = new CLPFGuildBagFetchItemNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuildBagFetchItemNtf(responseProto as CLPFGuildBagFetchItemNtf);
                    break;
                case 93:
                    responseProto = new CLPFMessageBroadcastNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMessageBroadcastNtf(responseProto as CLPFMessageBroadcastNtf);
                    break;
                case 94:
                    responseProto = new CLPFTaskQueryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskQueryReq(responseProto as CLPFTaskQueryReq);
                    break;
                case 95:
                    responseProto = new CLPFTaskQueryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskQueryAck(responseProto as CLPFTaskQueryAck);
                    break;
                case 96:
                    responseProto = new CLPFTaskFetchTaskRewardsReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskFetchTaskRewardsReq(responseProto as CLPFTaskFetchTaskRewardsReq);
                    break;
                case 97:
                    responseProto = new CLPFTaskFetchTaskRewardsAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskFetchTaskRewardsAck(responseProto as CLPFTaskFetchTaskRewardsAck);
                    break;
                case 98:
                    responseProto = new CLPFTaskFetchActiveRewardsReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskFetchActiveRewardsReq(responseProto as CLPFTaskFetchActiveRewardsReq);
                    break;
                case 99:
                    responseProto = new CLPFTaskFetchActiveRewardsAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskFetchActiveRewardsAck(responseProto as CLPFTaskFetchActiveRewardsAck);
                    break;
                case 100:
                    responseProto = new CLPFTaskAchieveQueryInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskAchieveQueryInfoReq(responseProto as CLPFTaskAchieveQueryInfoReq);
                    break;
                case 101:
                    responseProto = new CLPFTaskAchieveQueryInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskAchieveQueryInfoAck(responseProto as CLPFTaskAchieveQueryInfoAck);
                    break;
                case 102:
                    responseProto = new CLPFTaskAchieveFetchRewardReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskAchieveFetchRewardReq(responseProto as CLPFTaskAchieveFetchRewardReq);
                    break;
                case 103:
                    responseProto = new CLPFTaskAchieveFetchRewardAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFTaskAchieveFetchRewardAck(responseProto as CLPFTaskAchieveFetchRewardAck);
                    break;
                case 104:
                    responseProto = new CLPFMonthCardFetchRewardReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMonthCardFetchRewardReq(responseProto as CLPFMonthCardFetchRewardReq);
                    break;
                case 105:
                    responseProto = new CLPFMonthCardFetchRewardAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFMonthCardFetchRewardAck(responseProto as CLPFMonthCardFetchRewardAck);
                    break;
                case 106:
                    responseProto = new CLPFReliefGoldFetchReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFReliefGoldFetchReq(responseProto as CLPFReliefGoldFetchReq);
                    break;
                case 107:
                    responseProto = new CLPFReliefGoldFetchAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFReliefGoldFetchAck(responseProto as CLPFReliefGoldFetchAck);
                    break;
                case 108:
                    responseProto = new CLPFShakeNumberQueryInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberQueryInfoReq(responseProto as CLPFShakeNumberQueryInfoReq);
                    break;
                case 109:
                    responseProto = new CLPFShakeNumberQueryInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberQueryInfoAck(responseProto as CLPFShakeNumberQueryInfoAck);
                    break;
                case 110:
                    responseProto = new CLPFShakeNumberActReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberActReq(responseProto as CLPFShakeNumberActReq);
                    break;
                case 111:
                    responseProto = new CLPFShakeNumberActAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberActAck(responseProto as CLPFShakeNumberActAck);
                    break;
                case 112:
                    responseProto = new CLPFShakeNumberFetchRewardReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberFetchRewardReq(responseProto as CLPFShakeNumberFetchRewardReq);
                    break;
                case 113:
                    responseProto = new CLPFShakeNumberFetchRewardAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberFetchRewardAck(responseProto as CLPFShakeNumberFetchRewardAck);
                    break;
                case 114:
                    responseProto = new CLPFShakeNumberFetchBoxRewardReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberFetchBoxRewardReq(responseProto as CLPFShakeNumberFetchBoxRewardReq);
                    break;
                case 115:
                    responseProto = new CLPFShakeNumberFetchBoxRewardAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFShakeNumberFetchBoxRewardAck(responseProto as CLPFShakeNumberFetchBoxRewardAck);
                    break;
                case 116:
                    responseProto = new CLPFRechargeDailyQueryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRechargeDailyQueryReq(responseProto as CLPFRechargeDailyQueryReq);
                    break;
                case 117:
                    responseProto = new CLPFRechargeDailyQueryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRechargeDailyQueryAck(responseProto as CLPFRechargeDailyQueryAck);
                    break;
                case 118:
                    responseProto = new CLPFWelfarePigQueryInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigQueryInfoReq(responseProto as CLPFWelfarePigQueryInfoReq);
                    break;
                case 119:
                    responseProto = new CLPFWelfarePigQueryInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigQueryInfoAck(responseProto as CLPFWelfarePigQueryInfoAck);
                    break;
                case 120:
                    responseProto = new CLPFWelfarePigFetchMaterialReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigFetchMaterialReq(responseProto as CLPFWelfarePigFetchMaterialReq);
                    break;
                case 121:
                    responseProto = new CLPFWelfarePigFetchMaterialAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigFetchMaterialAck(responseProto as CLPFWelfarePigFetchMaterialAck);
                    break;
                case 122:
                    responseProto = new CLPFWelfarePigBrokenReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigBrokenReq(responseProto as CLPFWelfarePigBrokenReq);
                    break;
                case 123:
                    responseProto = new CLPFWelfarePigBrokenAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigBrokenAck(responseProto as CLPFWelfarePigBrokenAck);
                    break;
                case 124:
                    responseProto = new CLPFWelfarePigSearchReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigSearchReq(responseProto as CLPFWelfarePigSearchReq);
                    break;
                case 125:
                    responseProto = new CLPFWelfarePigSearchAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFWelfarePigSearchAck(responseProto as CLPFWelfarePigSearchAck);
                    break;
                case 126:
                    responseProto = new CLPFInvestGunQueryInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestGunQueryInfoReq(responseProto as CLPFInvestGunQueryInfoReq);
                    break;
                case 127:
                    responseProto = new CLPFInvestGunQueryInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestGunQueryInfoAck(responseProto as CLPFInvestGunQueryInfoAck);
                    break;
                case 128:
                    responseProto = new CLPFInvestGunFetchRewardReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestGunFetchRewardReq(responseProto as CLPFInvestGunFetchRewardReq);
                    break;
                case 129:
                    responseProto = new CLPFInvestGunFetchRewardAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestGunFetchRewardAck(responseProto as CLPFInvestGunFetchRewardAck);
                    break;
                case 130:
                    responseProto = new CLPFInvestCostQueryInfoReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestCostQueryInfoReq(responseProto as CLPFInvestCostQueryInfoReq);
                    break;
                case 131:
                    responseProto = new CLPFInvestCostQueryInfoAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestCostQueryInfoAck(responseProto as CLPFInvestCostQueryInfoAck);
                    break;
                case 132:
                    responseProto = new CLPFInvestCostFetchRewardReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestCostFetchRewardReq(responseProto as CLPFInvestCostFetchRewardReq);
                    break;
                case 133:
                    responseProto = new CLPFInvestCostFetchRewardAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFInvestCostFetchRewardAck(responseProto as CLPFInvestCostFetchRewardAck);
                    break;
                case 134:
                    responseProto = new CLPFFirstPackageFetchReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFFirstPackageFetchReq(responseProto as CLPFFirstPackageFetchReq);
                    break;
                case 135:
                    responseProto = new CLPFFirstPackageFetchAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFFirstPackageFetchAck(responseProto as CLPFFirstPackageFetchAck);
                    break;
                case 136:
                    responseProto = new CLPFAnnouncementChangedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAnnouncementChangedNtf(responseProto as CLPFAnnouncementChangedNtf);
                    break;
                case 137:
                    responseProto = new CLPFVipFillUpCurrencyNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFVipFillUpCurrencyNtf(responseProto as CLPFVipFillUpCurrencyNtf);
                    break;
                case 138:
                    responseProto = new CLPFRealGoodsQueryAddressReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRealGoodsQueryAddressReq(responseProto as CLPFRealGoodsQueryAddressReq);
                    break;
                case 139:
                    responseProto = new CLPFRealGoodsQueryAddressAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRealGoodsQueryAddressAck(responseProto as CLPFRealGoodsQueryAddressAck);
                    break;
                case 140:
                    responseProto = new CLPFRealGoodsCreateOrderReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRealGoodsCreateOrderReq(responseProto as CLPFRealGoodsCreateOrderReq);
                    break;
                case 141:
                    responseProto = new CLPFRealGoodsCreateOrderAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRealGoodsCreateOrderAck(responseProto as CLPFRealGoodsCreateOrderAck);
                    break;
                case 142:
                    responseProto = new CLPFRealGoodsQueryExchangeLogReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRealGoodsQueryExchangeLogReq(responseProto as CLPFRealGoodsQueryExchangeLogReq);
                    break;
                case 143:
                    responseProto = new CLPFRealGoodsQueryExchangeLogAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFRealGoodsQueryExchangeLogAck(responseProto as CLPFRealGoodsQueryExchangeLogAck);
                    break;
                case 144:
                    responseProto = new CLPFGuideDataQueryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuideDataQueryReq(responseProto as CLPFGuideDataQueryReq);
                    break;
                case 145:
                    responseProto = new CLPFGuideDataQueryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuideDataQueryAck(responseProto as CLPFGuideDataQueryAck);
                    break;
                case 146:
                    responseProto = new CLPFGuideDataActRpt();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFGuideDataActRpt(responseProto as CLPFGuideDataActRpt);
                    break;
                case 147:
                    responseProto = new CLPFClientConfigPublishNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFClientConfigPublishNtf(responseProto as CLPFClientConfigPublishNtf);
                    break;
                case 148:
                    responseProto = new CLPFSubGamesOnlineCountReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFSubGamesOnlineCountReq(responseProto as CLPFSubGamesOnlineCountReq);
                    break;
                case 149:
                    responseProto = new CLPFSubGamesOnlineCountAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFSubGamesOnlineCountAck(responseProto as CLPFSubGamesOnlineCountAck);
                    break;
                case 150:
                    responseProto = new CLPFCdkeyFetchRewardReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFCdkeyFetchRewardReq(responseProto as CLPFCdkeyFetchRewardReq);
                    break;
                case 151:
                    responseProto = new CLPFCdkeyFetchRewardAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFCdkeyFetchRewardAck(responseProto as CLPFCdkeyFetchRewardAck);
                    break;
                case 152:
                    responseProto = new CLPFAccountBindStateReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountBindStateReq(responseProto as CLPFAccountBindStateReq);
                    break;
                case 153:
                    responseProto = new CLPFAccountBindStateAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountBindStateAck(responseProto as CLPFAccountBindStateAck);
                    break;
                case 154:
                    responseProto = new CLPFAccountPhoneBindReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountPhoneBindReq(responseProto as CLPFAccountPhoneBindReq);
                    break;
                case 155:
                    responseProto = new CLPFAccountPhoneBindAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountPhoneBindAck(responseProto as CLPFAccountPhoneBindAck);
                    break;
                case 156:
                    responseProto = new CLPFAccountPhoneChange1Req();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountPhoneChange1Req(responseProto as CLPFAccountPhoneChange1Req);
                    break;
                case 157:
                    responseProto = new CLPFAccountPhoneChange1Ack();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountPhoneChange1Ack(responseProto as CLPFAccountPhoneChange1Ack);
                    break;
                case 158:
                    responseProto = new CLPFAccountPhoneChange2Req();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountPhoneChange2Req(responseProto as CLPFAccountPhoneChange2Req);
                    break;
                case 159:
                    responseProto = new CLPFAccountPhoneChange2Ack();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountPhoneChange2Ack(responseProto as CLPFAccountPhoneChange2Ack);
                    break;
                case 160:
                    responseProto = new CLPFAccountUniformBindReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountUniformBindReq(responseProto as CLPFAccountUniformBindReq);
                    break;
                case 161:
                    responseProto = new CLPFAccountUniformBindAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountUniformBindAck(responseProto as CLPFAccountUniformBindAck);
                    break;
                case 162:
                    responseProto = new CLPFAccountUniformUnbindReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountUniformUnbindReq(responseProto as CLPFAccountUniformUnbindReq);
                    break;
                case 163:
                    responseProto = new CLPFAccountUniformUnbindAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFAccountUniformUnbindAck(responseProto as CLPFAccountUniformUnbindAck);
                    break;
                case 164:
                    responseProto = new CLPFPlayerNicknameQueryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFPlayerNicknameQueryReq(responseProto as CLPFPlayerNicknameQueryReq);
                    break;
                case 165:
                    responseProto = new CLPFPlayerNicknameQueryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFPlayerNicknameQueryAck(responseProto as CLPFPlayerNicknameQueryAck);
                    break;
                case 166:
                    responseProto = new CLPFBankPasswordInitReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordInitReq(responseProto as CLPFBankPasswordInitReq);
                    break;
                case 167:
                    responseProto = new CLPFBankPasswordInitAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordInitAck(responseProto as CLPFBankPasswordInitAck);
                    break;
                case 168:
                    responseProto = new CLPFBankPasswordVerifyReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordVerifyReq(responseProto as CLPFBankPasswordVerifyReq);
                    break;
                case 169:
                    responseProto = new CLPFBankPasswordVerifyAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordVerifyAck(responseProto as CLPFBankPasswordVerifyAck);
                    break;
                case 170:
                    responseProto = new CLPFBankPasswordModifyReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordModifyReq(responseProto as CLPFBankPasswordModifyReq);
                    break;
                case 171:
                    responseProto = new CLPFBankPasswordModifyAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordModifyAck(responseProto as CLPFBankPasswordModifyAck);
                    break;
                case 172:
                    responseProto = new CLPFBankPasswordResetReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordResetReq(responseProto as CLPFBankPasswordResetReq);
                    break;
                case 173:
                    responseProto = new CLPFBankPasswordResetAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankPasswordResetAck(responseProto as CLPFBankPasswordResetAck);
                    break;
                case 174:
                    responseProto = new CLPFBankItemQueryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemQueryReq(responseProto as CLPFBankItemQueryReq);
                    break;
                case 175:
                    responseProto = new CLPFBankItemQueryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemQueryAck(responseProto as CLPFBankItemQueryAck);
                    break;
                case 176:
                    responseProto = new CLPFBankItemStoreReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemStoreReq(responseProto as CLPFBankItemStoreReq);
                    break;
                case 177:
                    responseProto = new CLPFBankItemStoreAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemStoreAck(responseProto as CLPFBankItemStoreAck);
                    break;
                case 178:
                    responseProto = new CLPFBankItemFetchReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemFetchReq(responseProto as CLPFBankItemFetchReq);
                    break;
                case 179:
                    responseProto = new CLPFBankItemFetchAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemFetchAck(responseProto as CLPFBankItemFetchAck);
                    break;
                case 180:
                    responseProto = new CLPFBankItemSendReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemSendReq(responseProto as CLPFBankItemSendReq);
                    break;
                case 181:
                    responseProto = new CLPFBankItemSendAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemSendAck(responseProto as CLPFBankItemSendAck);
                    break;
                case 182:
                    responseProto = new CLPFBankItemLogQueryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemLogQueryReq(responseProto as CLPFBankItemLogQueryReq);
                    break;
                case 183:
                    responseProto = new CLPFBankItemLogQueryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLPFBankItemLogQueryAck(responseProto as CLPFBankItemLogQueryAck);
                    break;
            }
            return responseProto != null;
        }

        public virtual void onRecv_CLPFLogoutReq(CLPFLogoutReq proto) { }
        public virtual void onRecv_CLPFResSyncNtf(CLPFResSyncNtf proto) { }
        public virtual void onRecv_CLPFResChangedNtf(CLPFResChangedNtf proto) { }
        public virtual void onRecv_CLPFItemGetListReq(CLPFItemGetListReq proto) { }
        public virtual void onRecv_CLPFItemGetListAck(CLPFItemGetListAck proto) { }
        public virtual void onRecv_CLPFItemUseReq(CLPFItemUseReq proto) { }
        public virtual void onRecv_CLPFItemUseAck(CLPFItemUseAck proto) { }
        public virtual void onRecv_CLPFItemCountChangeNtf(CLPFItemCountChangeNtf proto) { }
        public virtual void onRecv_CLPFItemBuyReq(CLPFItemBuyReq proto) { }
        public virtual void onRecv_CLPFItemBuyAck(CLPFItemBuyAck proto) { }
        public virtual void onRecv_CLPFShopQueryBuyCountReq(CLPFShopQueryBuyCountReq proto) { }
        public virtual void onRecv_CLPFShopQueryBuyCountAck(CLPFShopQueryBuyCountAck proto) { }
        public virtual void onRecv_CLPFShopBuyReq(CLPFShopBuyReq proto) { }
        public virtual void onRecv_CLPFShopBuyAck(CLPFShopBuyAck proto) { }
        public virtual void onRecv_CLPFRechargeReq(CLPFRechargeReq proto) { }
        public virtual void onRecv_CLPFRechargeAck(CLPFRechargeAck proto) { }
        public virtual void onRecv_CLPFRechargeSuccessNtf(CLPFRechargeSuccessNtf proto) { }
        public virtual void onRecv_CLPFGetRankListReq(CLPFGetRankListReq proto) { }
        public virtual void onRecv_CLPFGetRankListAck(CLPFGetRankListAck proto) { }
        public virtual void onRecv_CLPFLevelExpChangedNtf(CLPFLevelExpChangedNtf proto) { }
        public virtual void onRecv_CLPFLevelUpNtf(CLPFLevelUpNtf proto) { }
        public virtual void onRecv_CLPFVipExpChangedNtf(CLPFVipExpChangedNtf proto) { }
        public virtual void onRecv_CLPFModifyNicknameReq(CLPFModifyNicknameReq proto) { }
        public virtual void onRecv_CLPFModifyNicknameAck(CLPFModifyNicknameAck proto) { }
        public virtual void onRecv_CLPFModifyHeadReq(CLPFModifyHeadReq proto) { }
        public virtual void onRecv_CLPFModifyHeadAck(CLPFModifyHeadAck proto) { }
        public virtual void onRecv_CLPFQuerySignReq(CLPFQuerySignReq proto) { }
        public virtual void onRecv_CLPFQuerySignAck(CLPFQuerySignAck proto) { }
        public virtual void onRecv_CLPFActSignReq(CLPFActSignReq proto) { }
        public virtual void onRecv_CLPFActSignAck(CLPFActSignAck proto) { }
        public virtual void onRecv_CLPFQueryVipWheelReq(CLPFQueryVipWheelReq proto) { }
        public virtual void onRecv_CLPFQueryVipWheelAck(CLPFQueryVipWheelAck proto) { }
        public virtual void onRecv_CLPFActVipWheelReq(CLPFActVipWheelReq proto) { }
        public virtual void onRecv_CLPFActVipWheelAck(CLPFActVipWheelAck proto) { }
        public virtual void onRecv_CLPFMailQueryAllIdsReq(CLPFMailQueryAllIdsReq proto) { }
        public virtual void onRecv_CLPFMailQueryAllIdsAck(CLPFMailQueryAllIdsAck proto) { }
        public virtual void onRecv_CLPFMailBatchQueryContentReq(CLPFMailBatchQueryContentReq proto) { }
        public virtual void onRecv_CLPFMailBatchQueryContentAck(CLPFMailBatchQueryContentAck proto) { }
        public virtual void onRecv_CLPFMailAccessReq(CLPFMailAccessReq proto) { }
        public virtual void onRecv_CLPFMailAccessAck(CLPFMailAccessAck proto) { }
        public virtual void onRecv_CLPFMailFetchItemReq(CLPFMailFetchItemReq proto) { }
        public virtual void onRecv_CLPFMailFetchItemAck(CLPFMailFetchItemAck proto) { }
        public virtual void onRecv_CLPFMailRemoveReq(CLPFMailRemoveReq proto) { }
        public virtual void onRecv_CLPFMailRemoveAck(CLPFMailRemoveAck proto) { }
        public virtual void onRecv_CLPFMailArriveNtf(CLPFMailArriveNtf proto) { }
        public virtual void onRecv_CLPFGuildCreateReq(CLPFGuildCreateReq proto) { }
        public virtual void onRecv_CLPFGuildCreateAck(CLPFGuildCreateAck proto) { }
        public virtual void onRecv_CLPFGuildQueryRecommendListReq(CLPFGuildQueryRecommendListReq proto) { }
        public virtual void onRecv_CLPFGuildQueryRecommendListAck(CLPFGuildQueryRecommendListAck proto) { }
        public virtual void onRecv_CLPFGuildSearchReq(CLPFGuildSearchReq proto) { }
        public virtual void onRecv_CLPFGuildSearchAck(CLPFGuildSearchAck proto) { }
        public virtual void onRecv_CLPFGuildQuickJoinReq(CLPFGuildQuickJoinReq proto) { }
        public virtual void onRecv_CLPFGuildQuickJoinAck(CLPFGuildQuickJoinAck proto) { }
        public virtual void onRecv_CLPFGuildJoinReq(CLPFGuildJoinReq proto) { }
        public virtual void onRecv_CLPFGuildJoinAck(CLPFGuildJoinAck proto) { }
        public virtual void onRecv_CLPFGuildQueryJoinListReq(CLPFGuildQueryJoinListReq proto) { }
        public virtual void onRecv_CLPFGuildQueryJoinListAck(CLPFGuildQueryJoinListAck proto) { }
        public virtual void onRecv_CLPFGuildHandleJoinReq(CLPFGuildHandleJoinReq proto) { }
        public virtual void onRecv_CLPFGuildHandleJoinAck(CLPFGuildHandleJoinAck proto) { }
        public virtual void onRecv_CLPFGuildJoinResponseNtf(CLPFGuildJoinResponseNtf proto) { }
        public virtual void onRecv_CLPFGuildNewJoinRequestNtf(CLPFGuildNewJoinRequestNtf proto) { }
        public virtual void onRecv_CLPFGuildQueryInfoReq(CLPFGuildQueryInfoReq proto) { }
        public virtual void onRecv_CLPFGuildQueryInfoAck(CLPFGuildQueryInfoAck proto) { }
        public virtual void onRecv_CLPFGuildModifyInfoReq(CLPFGuildModifyInfoReq proto) { }
        public virtual void onRecv_CLPFGuildModifyInfoAck(CLPFGuildModifyInfoAck proto) { }
        public virtual void onRecv_CLPFGuildModifyMemberJobReq(CLPFGuildModifyMemberJobReq proto) { }
        public virtual void onRecv_CLPFGuildModifyMemberJobAck(CLPFGuildModifyMemberJobAck proto) { }
        public virtual void onRecv_CLPFGuildKickMemberReq(CLPFGuildKickMemberReq proto) { }
        public virtual void onRecv_CLPFGuildKickMemberAck(CLPFGuildKickMemberAck proto) { }
        public virtual void onRecv_CLPFGuildKickMemberNtf(CLPFGuildKickMemberNtf proto) { }
        public virtual void onRecv_CLPFGuildExitReq(CLPFGuildExitReq proto) { }
        public virtual void onRecv_CLPFGuildExitAck(CLPFGuildExitAck proto) { }
        public virtual void onRecv_CLPFGuildUpgradeReq(CLPFGuildUpgradeReq proto) { }
        public virtual void onRecv_CLPFGuildUpgradeAck(CLPFGuildUpgradeAck proto) { }
        public virtual void onRecv_CLPFGuildQueryWelfareReq(CLPFGuildQueryWelfareReq proto) { }
        public virtual void onRecv_CLPFGuildQueryWelfareAck(CLPFGuildQueryWelfareAck proto) { }
        public virtual void onRecv_CLPFGuildFetchWelfareReq(CLPFGuildFetchWelfareReq proto) { }
        public virtual void onRecv_CLPFGuildFetchWelfareAck(CLPFGuildFetchWelfareAck proto) { }
        public virtual void onRecv_CLPFGuildQueryRedPacketInfoReq(CLPFGuildQueryRedPacketInfoReq proto) { }
        public virtual void onRecv_CLPFGuildQueryRedPacketInfoAck(CLPFGuildQueryRedPacketInfoAck proto) { }
        public virtual void onRecv_CLPFGuildQueryRedPacketRankReq(CLPFGuildQueryRedPacketRankReq proto) { }
        public virtual void onRecv_CLPFGuildQueryRedPacketRankAck(CLPFGuildQueryRedPacketRankAck proto) { }
        public virtual void onRecv_CLPFGuildActRedPacketReq(CLPFGuildActRedPacketReq proto) { }
        public virtual void onRecv_CLPFGuildActRedPacketAck(CLPFGuildActRedPacketAck proto) { }
        public virtual void onRecv_CLPFGuildBagQueryInfoReq(CLPFGuildBagQueryInfoReq proto) { }
        public virtual void onRecv_CLPFGuildBagQueryInfoAck(CLPFGuildBagQueryInfoAck proto) { }
        public virtual void onRecv_CLPFGuildBagQueryLogReq(CLPFGuildBagQueryLogReq proto) { }
        public virtual void onRecv_CLPFGuildBagQueryLogAck(CLPFGuildBagQueryLogAck proto) { }
        public virtual void onRecv_CLPFGuildBagStoreItemReq(CLPFGuildBagStoreItemReq proto) { }
        public virtual void onRecv_CLPFGuildBagStoreItemAck(CLPFGuildBagStoreItemAck proto) { }
        public virtual void onRecv_CLPFGuildBagFetchItemReq(CLPFGuildBagFetchItemReq proto) { }
        public virtual void onRecv_CLPFGuildBagFetchItemAck(CLPFGuildBagFetchItemAck proto) { }
        public virtual void onRecv_CLPFGuildBagFetchItemNtf(CLPFGuildBagFetchItemNtf proto) { }
        public virtual void onRecv_CLPFMessageBroadcastNtf(CLPFMessageBroadcastNtf proto) { }
        public virtual void onRecv_CLPFTaskQueryReq(CLPFTaskQueryReq proto) { }
        public virtual void onRecv_CLPFTaskQueryAck(CLPFTaskQueryAck proto) { }
        public virtual void onRecv_CLPFTaskFetchTaskRewardsReq(CLPFTaskFetchTaskRewardsReq proto) { }
        public virtual void onRecv_CLPFTaskFetchTaskRewardsAck(CLPFTaskFetchTaskRewardsAck proto) { }
        public virtual void onRecv_CLPFTaskFetchActiveRewardsReq(CLPFTaskFetchActiveRewardsReq proto) { }
        public virtual void onRecv_CLPFTaskFetchActiveRewardsAck(CLPFTaskFetchActiveRewardsAck proto) { }
        public virtual void onRecv_CLPFTaskAchieveQueryInfoReq(CLPFTaskAchieveQueryInfoReq proto) { }
        public virtual void onRecv_CLPFTaskAchieveQueryInfoAck(CLPFTaskAchieveQueryInfoAck proto) { }
        public virtual void onRecv_CLPFTaskAchieveFetchRewardReq(CLPFTaskAchieveFetchRewardReq proto) { }
        public virtual void onRecv_CLPFTaskAchieveFetchRewardAck(CLPFTaskAchieveFetchRewardAck proto) { }
        public virtual void onRecv_CLPFMonthCardFetchRewardReq(CLPFMonthCardFetchRewardReq proto) { }
        public virtual void onRecv_CLPFMonthCardFetchRewardAck(CLPFMonthCardFetchRewardAck proto) { }
        public virtual void onRecv_CLPFReliefGoldFetchReq(CLPFReliefGoldFetchReq proto) { }
        public virtual void onRecv_CLPFReliefGoldFetchAck(CLPFReliefGoldFetchAck proto) { }
        public virtual void onRecv_CLPFShakeNumberQueryInfoReq(CLPFShakeNumberQueryInfoReq proto) { }
        public virtual void onRecv_CLPFShakeNumberQueryInfoAck(CLPFShakeNumberQueryInfoAck proto) { }
        public virtual void onRecv_CLPFShakeNumberActReq(CLPFShakeNumberActReq proto) { }
        public virtual void onRecv_CLPFShakeNumberActAck(CLPFShakeNumberActAck proto) { }
        public virtual void onRecv_CLPFShakeNumberFetchRewardReq(CLPFShakeNumberFetchRewardReq proto) { }
        public virtual void onRecv_CLPFShakeNumberFetchRewardAck(CLPFShakeNumberFetchRewardAck proto) { }
        public virtual void onRecv_CLPFShakeNumberFetchBoxRewardReq(CLPFShakeNumberFetchBoxRewardReq proto) { }
        public virtual void onRecv_CLPFShakeNumberFetchBoxRewardAck(CLPFShakeNumberFetchBoxRewardAck proto) { }
        public virtual void onRecv_CLPFRechargeDailyQueryReq(CLPFRechargeDailyQueryReq proto) { }
        public virtual void onRecv_CLPFRechargeDailyQueryAck(CLPFRechargeDailyQueryAck proto) { }
        public virtual void onRecv_CLPFWelfarePigQueryInfoReq(CLPFWelfarePigQueryInfoReq proto) { }
        public virtual void onRecv_CLPFWelfarePigQueryInfoAck(CLPFWelfarePigQueryInfoAck proto) { }
        public virtual void onRecv_CLPFWelfarePigFetchMaterialReq(CLPFWelfarePigFetchMaterialReq proto) { }
        public virtual void onRecv_CLPFWelfarePigFetchMaterialAck(CLPFWelfarePigFetchMaterialAck proto) { }
        public virtual void onRecv_CLPFWelfarePigBrokenReq(CLPFWelfarePigBrokenReq proto) { }
        public virtual void onRecv_CLPFWelfarePigBrokenAck(CLPFWelfarePigBrokenAck proto) { }
        public virtual void onRecv_CLPFWelfarePigSearchReq(CLPFWelfarePigSearchReq proto) { }
        public virtual void onRecv_CLPFWelfarePigSearchAck(CLPFWelfarePigSearchAck proto) { }
        public virtual void onRecv_CLPFInvestGunQueryInfoReq(CLPFInvestGunQueryInfoReq proto) { }
        public virtual void onRecv_CLPFInvestGunQueryInfoAck(CLPFInvestGunQueryInfoAck proto) { }
        public virtual void onRecv_CLPFInvestGunFetchRewardReq(CLPFInvestGunFetchRewardReq proto) { }
        public virtual void onRecv_CLPFInvestGunFetchRewardAck(CLPFInvestGunFetchRewardAck proto) { }
        public virtual void onRecv_CLPFInvestCostQueryInfoReq(CLPFInvestCostQueryInfoReq proto) { }
        public virtual void onRecv_CLPFInvestCostQueryInfoAck(CLPFInvestCostQueryInfoAck proto) { }
        public virtual void onRecv_CLPFInvestCostFetchRewardReq(CLPFInvestCostFetchRewardReq proto) { }
        public virtual void onRecv_CLPFInvestCostFetchRewardAck(CLPFInvestCostFetchRewardAck proto) { }
        public virtual void onRecv_CLPFFirstPackageFetchReq(CLPFFirstPackageFetchReq proto) { }
        public virtual void onRecv_CLPFFirstPackageFetchAck(CLPFFirstPackageFetchAck proto) { }
        public virtual void onRecv_CLPFAnnouncementChangedNtf(CLPFAnnouncementChangedNtf proto) { }
        public virtual void onRecv_CLPFVipFillUpCurrencyNtf(CLPFVipFillUpCurrencyNtf proto) { }
        public virtual void onRecv_CLPFRealGoodsQueryAddressReq(CLPFRealGoodsQueryAddressReq proto) { }
        public virtual void onRecv_CLPFRealGoodsQueryAddressAck(CLPFRealGoodsQueryAddressAck proto) { }
        public virtual void onRecv_CLPFRealGoodsCreateOrderReq(CLPFRealGoodsCreateOrderReq proto) { }
        public virtual void onRecv_CLPFRealGoodsCreateOrderAck(CLPFRealGoodsCreateOrderAck proto) { }
        public virtual void onRecv_CLPFRealGoodsQueryExchangeLogReq(CLPFRealGoodsQueryExchangeLogReq proto) { }
        public virtual void onRecv_CLPFRealGoodsQueryExchangeLogAck(CLPFRealGoodsQueryExchangeLogAck proto) { }
        public virtual void onRecv_CLPFGuideDataQueryReq(CLPFGuideDataQueryReq proto) { }
        public virtual void onRecv_CLPFGuideDataQueryAck(CLPFGuideDataQueryAck proto) { }
        public virtual void onRecv_CLPFGuideDataActRpt(CLPFGuideDataActRpt proto) { }
        public virtual void onRecv_CLPFClientConfigPublishNtf(CLPFClientConfigPublishNtf proto) { }
        public virtual void onRecv_CLPFSubGamesOnlineCountReq(CLPFSubGamesOnlineCountReq proto) { }
        public virtual void onRecv_CLPFSubGamesOnlineCountAck(CLPFSubGamesOnlineCountAck proto) { }
        public virtual void onRecv_CLPFCdkeyFetchRewardReq(CLPFCdkeyFetchRewardReq proto) { }
        public virtual void onRecv_CLPFCdkeyFetchRewardAck(CLPFCdkeyFetchRewardAck proto) { }
        public virtual void onRecv_CLPFAccountBindStateReq(CLPFAccountBindStateReq proto) { }
        public virtual void onRecv_CLPFAccountBindStateAck(CLPFAccountBindStateAck proto) { }
        public virtual void onRecv_CLPFAccountPhoneBindReq(CLPFAccountPhoneBindReq proto) { }
        public virtual void onRecv_CLPFAccountPhoneBindAck(CLPFAccountPhoneBindAck proto) { }
        public virtual void onRecv_CLPFAccountPhoneChange1Req(CLPFAccountPhoneChange1Req proto) { }
        public virtual void onRecv_CLPFAccountPhoneChange1Ack(CLPFAccountPhoneChange1Ack proto) { }
        public virtual void onRecv_CLPFAccountPhoneChange2Req(CLPFAccountPhoneChange2Req proto) { }
        public virtual void onRecv_CLPFAccountPhoneChange2Ack(CLPFAccountPhoneChange2Ack proto) { }
        public virtual void onRecv_CLPFAccountUniformBindReq(CLPFAccountUniformBindReq proto) { }
        public virtual void onRecv_CLPFAccountUniformBindAck(CLPFAccountUniformBindAck proto) { }
        public virtual void onRecv_CLPFAccountUniformUnbindReq(CLPFAccountUniformUnbindReq proto) { }
        public virtual void onRecv_CLPFAccountUniformUnbindAck(CLPFAccountUniformUnbindAck proto) { }
        public virtual void onRecv_CLPFPlayerNicknameQueryReq(CLPFPlayerNicknameQueryReq proto) { }
        public virtual void onRecv_CLPFPlayerNicknameQueryAck(CLPFPlayerNicknameQueryAck proto) { }
        public virtual void onRecv_CLPFBankPasswordInitReq(CLPFBankPasswordInitReq proto) { }
        public virtual void onRecv_CLPFBankPasswordInitAck(CLPFBankPasswordInitAck proto) { }
        public virtual void onRecv_CLPFBankPasswordVerifyReq(CLPFBankPasswordVerifyReq proto) { }
        public virtual void onRecv_CLPFBankPasswordVerifyAck(CLPFBankPasswordVerifyAck proto) { }
        public virtual void onRecv_CLPFBankPasswordModifyReq(CLPFBankPasswordModifyReq proto) { }
        public virtual void onRecv_CLPFBankPasswordModifyAck(CLPFBankPasswordModifyAck proto) { }
        public virtual void onRecv_CLPFBankPasswordResetReq(CLPFBankPasswordResetReq proto) { }
        public virtual void onRecv_CLPFBankPasswordResetAck(CLPFBankPasswordResetAck proto) { }
        public virtual void onRecv_CLPFBankItemQueryReq(CLPFBankItemQueryReq proto) { }
        public virtual void onRecv_CLPFBankItemQueryAck(CLPFBankItemQueryAck proto) { }
        public virtual void onRecv_CLPFBankItemStoreReq(CLPFBankItemStoreReq proto) { }
        public virtual void onRecv_CLPFBankItemStoreAck(CLPFBankItemStoreAck proto) { }
        public virtual void onRecv_CLPFBankItemFetchReq(CLPFBankItemFetchReq proto) { }
        public virtual void onRecv_CLPFBankItemFetchAck(CLPFBankItemFetchAck proto) { }
        public virtual void onRecv_CLPFBankItemSendReq(CLPFBankItemSendReq proto) { }
        public virtual void onRecv_CLPFBankItemSendAck(CLPFBankItemSendAck proto) { }
        public virtual void onRecv_CLPFBankItemLogQueryReq(CLPFBankItemLogQueryReq proto) { }
        public virtual void onRecv_CLPFBankItemLogQueryAck(CLPFBankItemLogQueryAck proto) { }
    }
}
