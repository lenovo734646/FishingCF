using System;
using System.IO;

namespace JBPROTO
{
    /// <summary>
    /// 房间内玩家信息
    /// </summary>
    public sealed class CLFRRoomPlayerInfo : INetProtocol
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
        /// 玩家等级经验
        /// </summary>
        public Int64 level_exp;
        /// <summary>
        /// 玩家vip等级
        /// </summary>
        public int vip_level;
        /// <summary>
        /// 玩家vip等级经验
        /// </summary>
        public Int64 vip_level_exp;
        /// <summary>
        /// 平台货币
        /// </summary>
        public Int64 currency;
        /// <summary>
        /// 平台绑定货币
        /// </summary>
        public Int64 bind_currency;
        /// <summary>
        /// 平台钻石
        /// </summary>
        public Int64 diamond;
        /// <summary>
        /// 炮台位置：0~3
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 炮台Id
        /// </summary>
        public int gun_id;
        /// <summary>
        /// 当前炮值
        /// </summary>
        public Int64 gun_value;
        /// <summary>
        /// 当前比赛剩余开炮次数
        /// </summary>
        public int left_gun_num;
        /// <summary>
        /// 当前比赛积分
        /// </summary>
        public int integral;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(gender);
            bw.Write(head);
            bw.Write(head_frame);
            bw.Write(level);
            bw.Write(level_exp);
            bw.Write(vip_level);
            bw.Write(vip_level_exp);
            bw.Write(currency);
            bw.Write(bind_currency);
            bw.Write(diamond);
            bw.Write(seat_id);
            bw.Write(gun_id);
            bw.Write(gun_value);
            bw.Write(left_gun_num);
            bw.Write(integral);
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
                level_exp = br.ReadInt64();
                vip_level = br.ReadInt32();
                vip_level_exp = br.ReadInt64();
                currency = br.ReadInt64();
                bind_currency = br.ReadInt64();
                diamond = br.ReadInt64();
                seat_id = br.ReadInt32();
                gun_id = br.ReadInt32();
                gun_value = br.ReadInt64();
                left_gun_num = br.ReadInt32();
                integral = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 鱼游动的时间变化率信息
    /// </summary>
    public sealed class CLFRFishTimeRateInfo : INetProtocol
    {
        /// <summary>
        /// 开始时间，单位毫秒
        /// </summary>
        public UInt64 time;
        /// <summary>
        /// 持续时间，单位毫秒
        /// </summary>
        public UInt64 duration;
        /// <summary>
        /// 时间变化率万分比
        /// </summary>
        public int rate;
        /// <summary>
        /// 变化原因 1冰冻 2鱼潮来临 3装逼悬停 4普通悬停
        /// </summary>
        public int reason;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(time);
            bw.Write(duration);
            bw.Write(rate);
            bw.Write(reason);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                time = br.ReadUInt64();
                duration = br.ReadUInt64();
                rate = br.ReadInt32();
                reason = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 鱼出现信息
    /// </summary>
    public sealed class CLFRFishAppearInfo : INetProtocol
    {
        /// <summary>
        /// 鱼Id
        /// </summary>
        public int fish_id;
        /// <summary>
        /// 鱼配置Id
        /// </summary>
        public int config_id;
        /// <summary>
        /// 开始时间，单位毫秒
        /// </summary>
        public UInt64 start_time;
        /// <summary>
        /// 结束时间，单位毫秒
        /// </summary>
        public UInt64 stop_time;
        /// <summary>
        /// 路径Id
        /// </summary>
        public int path_id;
        /// <summary>
        /// 路径偏移量，单位自己决定
        /// </summary>
        public int offset_x;
        /// <summary>
        /// 路径偏移量，单位自己决定
        /// </summary>
        public int offset_y;
        /// <summary>
        /// 时间变化率数组长度
        /// </summary>
        public int rate_len;
        /// <summary>
        /// 时间变化率数组
        /// </summary>
        public CLFRFishTimeRateInfo[] rates;  //max:20
        /// <summary>
        /// 是否是一网打尽鱼群中的鱼 1是0否
        /// </summary>
        public sbyte is_multiple_group;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(fish_id);
            bw.Write(config_id);
            bw.Write(start_time);
            bw.Write(stop_time);
            bw.Write(path_id);
            bw.Write(offset_x);
            bw.Write(offset_y);
            bw.Write(rate_len);
            for (int i = 0; i < (int)rate_len; i++)
                rates[i].toBinary(bw);
            bw.Write(is_multiple_group);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                fish_id = br.ReadInt32();
                config_id = br.ReadInt32();
                start_time = br.ReadUInt64();
                stop_time = br.ReadUInt64();
                path_id = br.ReadInt32();
                offset_x = br.ReadInt32();
                offset_y = br.ReadInt32();
                rate_len = br.ReadInt32();
                rates = new CLFRFishTimeRateInfo[(int)rate_len];
                for (int i = 0; i < rates.Length; i++)
                {
                    rates[i] = new CLFRFishTimeRateInfo();
                    rates[i].fromBinary(br);
                }
                is_multiple_group = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 世界boss出现信息
    /// </summary>
    public sealed class CLFRBossAppearInfo : INetProtocol
    {
        /// <summary>
        /// boss配置Id，关联FishBoss表的Id
        /// </summary>
        public int config_id;
        /// <summary>
        /// 开始时间，单位毫秒
        /// </summary>
        public UInt64 start_time;
        /// <summary>
        /// 结束时间，单位毫秒
        /// </summary>
        public UInt64 stop_time;
        /// <summary>
        /// 防御罩剩余血量百分比0-100，该值等于0时boss处于落体状态
        /// </summary>
        public sbyte left_blood;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(config_id);
            bw.Write(start_time);
            bw.Write(stop_time);
            bw.Write(left_blood);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                config_id = br.ReadInt32();
                start_time = br.ReadUInt64();
                stop_time = br.ReadUInt64();
                left_blood = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 物品信息结构
    /// </summary>
    public sealed class CLFRItemInfo : INetProtocol
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
    /// 使用中的物品信息
    /// </summary>
    public sealed class CLFRItemUsingInfo : INetProtocol
    {
        /// <summary>
        /// 炮台位置：0~3
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 物品主类型
        /// </summary>
        public int item_id;
        /// <summary>
        /// 物品子类型
        /// </summary>
        public int item_sub_id;
        /// <summary>
        /// 效果结束时间戳
        /// </summary>
        public UInt64 end_time;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(seat_id);
            bw.Write(item_id);
            bw.Write(item_sub_id);
            bw.Write(end_time);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                item_id = br.ReadInt32();
                item_sub_id = br.ReadInt32();
                end_time = br.ReadUInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 进入游戏请求
    /// </summary>
    public sealed class CLFREnterGameReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 0;
        /// <summary>
        /// 房间配置Id，对应房间配置表
        /// </summary>
        public int config_id;
        /// <summary>
        /// 指定房间Id，-1代表随机分配
        /// </summary>
        public int room_id;
        /// <summary>
        /// 指定座位Id，-1代表随机分配
        /// </summary>
        public int seat_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(config_id);
            bw.Write(room_id);
            bw.Write(seat_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                config_id = br.ReadInt32();
                room_id = br.ReadInt32();
                seat_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 进入游戏回应
    /// </summary>
    public sealed class CLFREnterGameAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 1;
        /// <summary>
        /// 0成功 1最大炮倍不足尚未解锁 2房间未开放 3房间已满 4指定的座位已被占用 5系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前时间戳
        /// </summary>
        public UInt64 time_stamp;
        /// <summary>
        /// 房间Id
        /// </summary>
        public int room_id;
        /// <summary>
        /// 当天已完成的比赛次数
        /// </summary>
        public int match_finish_count;
        /// <summary>
        /// 是否正在比赛中 1是0否
        /// </summary>
        public sbyte is_matching;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(time_stamp);
            bw.Write(room_id);
            bw.Write(match_finish_count);
            bw.Write(is_matching);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                time_stamp = br.ReadUInt64();
                room_id = br.ReadInt32();
                match_finish_count = br.ReadInt32();
                is_matching = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 退出游戏请求
    /// </summary>
    public sealed class CLFRExitGameReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 2;

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
    /// 退出游戏回应
    /// </summary>
    public sealed class CLFRExitGameAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 3;
        /// <summary>
        /// 0成功 1不在房间中
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
    /// 我已准备好，可以接收数据了
    /// </summary>
    public sealed class CLFRGetReadyReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 4;

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
    /// 当前房间内玩家信息和鱼的信息
    /// </summary>
    public sealed class CLFRGetReadyAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 5;
        /// <summary>
        /// 0成功 1系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 房间内玩家数量
        /// </summary>
        public int player_len;
        /// <summary>
        /// 房间内玩家信息
        /// </summary>
        public CLFRRoomPlayerInfo[] players;  //max:4
        /// <summary>
        /// 鱼数量
        /// </summary>
        public int fish_count;
        /// <summary>
        /// 房间内所有鱼的路径数组
        /// </summary>
        public CLFRFishAppearInfo[] fishes;  //max:1000
        /// <summary>
        /// 使用中的物品长度
        /// </summary>
        public int item_using_len;
        /// <summary>
        /// 使用中的物品数组
        /// </summary>
        public CLFRItemUsingInfo[] item_using_array;  //max:100
        /// <summary>
        /// 房间背景Id 1~4
        /// </summary>
        public int background_id;
        /// <summary>
        /// 是否正在鱼潮来临中 1是0否
        /// </summary>
        public sbyte in_tide;
        /// <summary>
        /// 距离鱼潮结束剩余秒数
        /// </summary>
        public int tide_left_time;
        /// <summary>
        /// 是否存在世界boss 1是0否
        /// </summary>
        public sbyte has_boss;
        /// <summary>
        /// 世界boss信息
        /// </summary>
        public CLFRBossAppearInfo boss_info;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(player_len);
            for (int i = 0; i < (int)player_len; i++)
                players[i].toBinary(bw);
            bw.Write(fish_count);
            for (int i = 0; i < (int)fish_count; i++)
                fishes[i].toBinary(bw);
            bw.Write(item_using_len);
            for (int i = 0; i < (int)item_using_len; i++)
                item_using_array[i].toBinary(bw);
            bw.Write(background_id);
            bw.Write(in_tide);
            bw.Write(tide_left_time);
            bw.Write(has_boss);
            boss_info.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                player_len = br.ReadInt32();
                players = new CLFRRoomPlayerInfo[(int)player_len];
                for (int i = 0; i < players.Length; i++)
                {
                    players[i] = new CLFRRoomPlayerInfo();
                    players[i].fromBinary(br);
                }
                fish_count = br.ReadInt32();
                fishes = new CLFRFishAppearInfo[(int)fish_count];
                for (int i = 0; i < fishes.Length; i++)
                {
                    fishes[i] = new CLFRFishAppearInfo();
                    fishes[i].fromBinary(br);
                }
                item_using_len = br.ReadInt32();
                item_using_array = new CLFRItemUsingInfo[(int)item_using_len];
                for (int i = 0; i < item_using_array.Length; i++)
                {
                    item_using_array[i] = new CLFRItemUsingInfo();
                    item_using_array[i].fromBinary(br);
                }
                background_id = br.ReadInt32();
                in_tide = br.ReadSByte();
                tide_left_time = br.ReadInt32();
                has_boss = br.ReadSByte();
                boss_info = new CLFRBossAppearInfo();
                boss_info.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 报名比赛请求
    /// </summary>
    public sealed class CLFRJoinMatchReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 6;

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
    /// 报名比赛回应
    /// </summary>
    public sealed class CLFRJoinMatchAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 7;
        /// <summary>
        /// 0成功 1比赛未开放 2比赛即将结束 3未在比赛房间 4已在比赛中 5钻石不足 6已达参赛次数上限 7系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前比赛剩余开炮次数
        /// </summary>
        public int left_gun_num;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(left_gun_num);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                left_gun_num = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 报名比赛通知
    /// </summary>
    public sealed class CLFRJoinMatchNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 8;
        /// <summary>
        /// 炮台位置：0~3
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 当前比赛剩余开炮次数
        /// </summary>
        public int left_gun_num;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(left_gun_num);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                left_gun_num = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 比赛结束通知
    /// </summary>
    public sealed class CLFRMatchOverNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 9;
        /// <summary>
        /// 1正常打完结束 2比赛截止强制结束
        /// </summary>
        public sbyte reason;
        /// <summary>
        /// 比赛获得积分
        /// </summary>
        public int integral;
        /// <summary>
        /// 炮台加成百分比
        /// </summary>
        public int gun_addition;
        /// <summary>
        /// 挑战加成百分比
        /// </summary>
        public int challenge_addition;
        /// <summary>
        /// vip加成百分比
        /// </summary>
        public int vip_addition;
        /// <summary>
        /// 最终结算的积分
        /// </summary>
        public int final_integral;
        /// <summary>
        /// 当前积分所对应的名次
        /// </summary>
        public int rank;
        /// <summary>
        /// 获得钻石奖励数量
        /// </summary>
        public int diamond_reward;
        /// <summary>
        /// 是否已发放过首次完成奖励 1是0否
        /// </summary>
        public sbyte has_give_reward;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(reason);
            bw.Write(integral);
            bw.Write(gun_addition);
            bw.Write(challenge_addition);
            bw.Write(vip_addition);
            bw.Write(final_integral);
            bw.Write(rank);
            bw.Write(diamond_reward);
            bw.Write(has_give_reward);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                reason = br.ReadSByte();
                integral = br.ReadInt32();
                gun_addition = br.ReadInt32();
                challenge_addition = br.ReadInt32();
                vip_addition = br.ReadInt32();
                final_integral = br.ReadInt32();
                rank = br.ReadInt32();
                diamond_reward = br.ReadInt32();
                has_give_reward = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 玩家加入通知
    /// </summary>
    public sealed class CLFRPlayerJoinNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 10;
        /// <summary>
        /// 玩家信息
        /// </summary>
        public CLFRRoomPlayerInfo player;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            player.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                player = new CLFRRoomPlayerInfo();
                player.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 玩家离开通知
    /// </summary>
    public sealed class CLFRPlayerLeaveNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 11;
        /// <summary>
        /// 炮台位置：0~3
        /// </summary>
        public int seat_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 鱼潮来临通知
    /// </summary>
    public sealed class CLFRFishTideStartNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 12;
        /// <summary>
        /// 房间背景Id
        /// </summary>
        public int background_id;
        /// <summary>
        /// 鱼潮持续时间，单位秒
        /// </summary>
        public int duration;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(background_id);
            bw.Write(duration);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                background_id = br.ReadInt32();
                duration = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 新鱼出现通知
    /// </summary>
    public sealed class CLFRFishAppearNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 13;
        /// <summary>
        /// 0普通鱼 1鱼潮
        /// </summary>
        public int type;
        /// <summary>
        /// 鱼信息数组数量
        /// </summary>
        public int fish_count;
        /// <summary>
        /// 房间内所有鱼的信息数组
        /// </summary>
        public CLFRFishAppearInfo[] fishes;  //max:1000

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(type);
            bw.Write(fish_count);
            for (int i = 0; i < (int)fish_count; i++)
                fishes[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                type = br.ReadInt32();
                fish_count = br.ReadInt32();
                fishes = new CLFRFishAppearInfo[(int)fish_count];
                for (int i = 0; i < fishes.Length; i++)
                {
                    fishes[i] = new CLFRFishAppearInfo();
                    fishes[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 鱼的时间变化率改变通知
    /// </summary>
    public sealed class CLFRFishTimeRateChangeNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 14;
        /// <summary>
        /// 鱼Id
        /// </summary>
        public int fish_id;
        /// <summary>
        /// 时间变化率信息
        /// </summary>
        public CLFRFishTimeRateInfo rate;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(fish_id);
            rate.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                fish_id = br.ReadInt32();
                rate = new CLFRFishTimeRateInfo();
                rate.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 改变炮值请求
    /// </summary>
    public sealed class CLFRGunValueChangeReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 15;
        /// <summary>
        /// 炮值
        /// </summary>
        public Int64 gun_value;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(gun_value);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                gun_value = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 改变炮值回应
    /// </summary>
    public sealed class CLFRGunValueChangeAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 16;
        /// <summary>
        /// 0成功 1系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前炮值
        /// </summary>
        public Int64 gun_value;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(gun_value);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                gun_value = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 炮值改变通知
    /// </summary>
    public sealed class CLFRGunValueChangeNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 17;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 当前炮值
        /// </summary>
        public Int64 gun_value;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(gun_value);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                gun_value = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 发炮请求
    /// </summary>
    public sealed class CLFRShootReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 18;
        /// <summary>
        /// 发炮角度
        /// </summary>
        public int angle;
        /// <summary>
        /// 锁定的鱼Id
        /// </summary>
        public int lock_fish;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(angle);
            bw.Write(lock_fish);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                angle = br.ReadInt32();
                lock_fish = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 发炮回应
    /// </summary>
    public sealed class CLFRShootAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 19;
        /// <summary>
        /// 0成功 1金币不足 2子弹数量不足 3系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 锁定的鱼Id
        /// </summary>
        public int lock_fish;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(bullet_id);
            bw.Write(lock_fish);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                bullet_id = br.ReadInt32();
                lock_fish = br.ReadInt32();
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 发炮通知
    /// </summary>
    public sealed class CLFRShootNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 20;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 发炮角度
        /// </summary>
        public int angle;
        /// <summary>
        /// 锁定的鱼Id
        /// </summary>
        public int lock_fish;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(bullet_id);
            bw.Write(angle);
            bw.Write(lock_fish);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                bullet_id = br.ReadInt32();
                angle = br.ReadInt32();
                lock_fish = br.ReadInt32();
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 发炮信息
    /// </summary>
    public sealed class CLFRShootInfo : INetProtocol
    {
        /// <summary>
        /// 发炮角度
        /// </summary>
        public int angle;
        /// <summary>
        /// 锁定的鱼Id
        /// </summary>
        public int lock_fish;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(angle);
            bw.Write(lock_fish);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                angle = br.ReadInt32();
                lock_fish = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 子弹信息
    /// </summary>
    public sealed class CLFRBulletInfo : INetProtocol
    {
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 发炮角度
        /// </summary>
        public int angle;
        /// <summary>
        /// 锁定的鱼Id
        /// </summary>
        public int lock_fish;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(bullet_id);
            bw.Write(angle);
            bw.Write(lock_fish);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bullet_id = br.ReadInt32();
                angle = br.ReadInt32();
                lock_fish = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 多炮台同时发炮请求
    /// </summary>
    public sealed class CLFRMultiShootReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 21;
        /// <summary>
        /// 发炮数量
        /// </summary>
        public sbyte shoot_len;
        /// <summary>
        /// 发炮信息数组
        /// </summary>
        public CLFRShootInfo[] shoot_array;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(shoot_len);
            for (int i = 0; i < (int)shoot_len; i++)
                shoot_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                shoot_len = br.ReadSByte();
                shoot_array = new CLFRShootInfo[(int)shoot_len];
                for (int i = 0; i < shoot_array.Length; i++)
                {
                    shoot_array[i] = new CLFRShootInfo();
                    shoot_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 多炮台同时发炮回应
    /// </summary>
    public sealed class CLFRMultiShootAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 22;
        /// <summary>
        /// 0成功 1金币不足 2子弹数量不足 3系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 子弹数量
        /// </summary>
        public sbyte bullet_len;
        /// <summary>
        /// 子弹信息数组
        /// </summary>
        public CLFRBulletInfo[] bullet_array;  //max:10
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(bullet_len);
            for (int i = 0; i < (int)bullet_len; i++)
                bullet_array[i].toBinary(bw);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                bullet_len = br.ReadSByte();
                bullet_array = new CLFRBulletInfo[(int)bullet_len];
                for (int i = 0; i < bullet_array.Length; i++)
                {
                    bullet_array[i] = new CLFRBulletInfo();
                    bullet_array[i].fromBinary(br);
                }
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 多炮台同时发炮通知
    /// </summary>
    public sealed class CLFRMultiShootNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 23;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 子弹数量
        /// </summary>
        public sbyte bullet_len;
        /// <summary>
        /// 子弹信息数组
        /// </summary>
        public CLFRBulletInfo[] bullet_array;  //max:10
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(bullet_len);
            for (int i = 0; i < (int)bullet_len; i++)
                bullet_array[i].toBinary(bw);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                bullet_len = br.ReadSByte();
                bullet_array = new CLFRBulletInfo[(int)bullet_len];
                for (int i = 0; i < bullet_array.Length; i++)
                {
                    bullet_array[i] = new CLFRBulletInfo();
                    bullet_array[i].fromBinary(br);
                }
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 命中请求
    /// </summary>
    public sealed class CLFRHitReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 24;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 鱼Id
        /// </summary>
        public int fish_id;
        /// <summary>
        /// 关联的鱼数组长度，目前用于黑洞鱼
        /// </summary>
        public sbyte related_fish_len;
        /// <summary>
        /// 关联的鱼Id数组，目前用于黑洞鱼
        /// </summary>
        public int[] related_fish_array;  //max:30

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(bullet_id);
            bw.Write(fish_id);
            bw.Write(related_fish_len);
            for (int i = 0; i < (int)related_fish_len; i++)
                bw.Write(related_fish_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bullet_id = br.ReadInt32();
                fish_id = br.ReadInt32();
                related_fish_len = br.ReadSByte();
                related_fish_array = new int[(int)related_fish_len];
                for (int i = 0; i < related_fish_array.Length; i++)
                    related_fish_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 命中回应
    /// </summary>
    public sealed class CLFRHitAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 25;
        /// <summary>
        /// 0成功 1系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 是否打爆 0否 1是
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 钻石变化量
        /// </summary>
        public Int64 diamond_delta;
        /// <summary>
        /// 比赛积分变化量
        /// </summary>
        public int match_integral_delta;
        /// <summary>
        /// 掉落物品数量
        /// </summary>
        public sbyte item_len;
        /// <summary>
        /// 掉落的物品数组
        /// </summary>
        public CLFRItemInfo[] items;  //max:100
        /// <summary>
        /// 其他关联的死亡鱼的数组长度
        /// </summary>
        public sbyte related_fish_len;
        /// <summary>
        /// 其他关联的死亡鱼Id的数组
        /// </summary>
        public int[] related_fish_array;  //max:100
        /// <summary>
        /// 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
        /// </summary>
        public sbyte related_remove_reason;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(is_boom);
            bw.Write(currency_delta);
            bw.Write(diamond_delta);
            bw.Write(match_integral_delta);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
            bw.Write(related_fish_len);
            for (int i = 0; i < (int)related_fish_len; i++)
                bw.Write(related_fish_array[i]);
            bw.Write(related_remove_reason);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                is_boom = br.ReadSByte();
                currency_delta = br.ReadInt64();
                diamond_delta = br.ReadInt64();
                match_integral_delta = br.ReadInt32();
                item_len = br.ReadSByte();
                items = new CLFRItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLFRItemInfo();
                    items[i].fromBinary(br);
                }
                related_fish_len = br.ReadSByte();
                related_fish_array = new int[(int)related_fish_len];
                for (int i = 0; i < related_fish_array.Length; i++)
                    related_fish_array[i] = br.ReadInt32();
                related_remove_reason = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 命中通知
    /// </summary>
    public sealed class CLFRHitNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 26;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 鱼Id
        /// </summary>
        public int fish_id;
        /// <summary>
        /// 是否打爆 0否 1是
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 钻石变化量
        /// </summary>
        public Int64 diamond_delta;
        /// <summary>
        /// 比赛积分变化量
        /// </summary>
        public int match_integral_delta;
        /// <summary>
        /// 掉落物品数量
        /// </summary>
        public sbyte item_len;
        /// <summary>
        /// 掉落的物品数组
        /// </summary>
        public CLFRItemInfo[] items;  //max:100
        /// <summary>
        /// 其他关联的死亡鱼的数组长度
        /// </summary>
        public sbyte related_fish_len;
        /// <summary>
        /// 其他关联的死亡鱼Id的数组
        /// </summary>
        public int[] related_fish_array;  //max:100
        /// <summary>
        /// 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
        /// </summary>
        public sbyte related_remove_reason;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(bullet_id);
            bw.Write(fish_id);
            bw.Write(is_boom);
            bw.Write(currency_delta);
            bw.Write(diamond_delta);
            bw.Write(match_integral_delta);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
            bw.Write(related_fish_len);
            for (int i = 0; i < (int)related_fish_len; i++)
                bw.Write(related_fish_array[i]);
            bw.Write(related_remove_reason);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                bullet_id = br.ReadInt32();
                fish_id = br.ReadInt32();
                is_boom = br.ReadSByte();
                currency_delta = br.ReadInt64();
                diamond_delta = br.ReadInt64();
                match_integral_delta = br.ReadInt32();
                item_len = br.ReadSByte();
                items = new CLFRItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLFRItemInfo();
                    items[i].fromBinary(br);
                }
                related_fish_len = br.ReadSByte();
                related_fish_array = new int[(int)related_fish_len];
                for (int i = 0; i < related_fish_array.Length; i++)
                    related_fish_array[i] = br.ReadInt32();
                related_remove_reason = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 机器人子弹碰撞通知
    /// </summary>
    public sealed class CLFRRobotHitRpt : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 27;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 鱼Id
        /// </summary>
        public int fish_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(bullet_id);
            bw.Write(fish_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bullet_id = br.ReadInt32();
                fish_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss出现通知
    /// </summary>
    public sealed class CLFRBossAppearNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 28;
        /// <summary>
        /// 世界boss信息
        /// </summary>
        public CLFRBossAppearInfo boss_info;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            boss_info.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                boss_info = new CLFRBossAppearInfo();
                boss_info.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss命中请求
    /// </summary>
    public sealed class CLFRBossHitReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 29;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(bullet_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bullet_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss命中回应
    /// </summary>
    public sealed class CLFRBossHitAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 30;
        /// <summary>
        /// 0成功 1系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 是否爆金 0miss 1爆金 2死亡
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(is_boom);
            bw.Write(currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                is_boom = br.ReadSByte();
                currency_delta = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss命中通知
    /// </summary>
    public sealed class CLFRBossHitNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 31;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 是否爆金 0miss 1爆金 2死亡
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(bullet_id);
            bw.Write(is_boom);
            bw.Write(currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                bullet_id = br.ReadInt32();
                is_boom = br.ReadSByte();
                currency_delta = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss防御罩血量变化通知
    /// </summary>
    public sealed class CLFRBossDefenceBloodChangedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 32;
        /// <summary>
        /// 防御罩剩余血量百分比0-100，该值等于0时boss处于落体状态
        /// </summary>
        public sbyte left_blood;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(left_blood);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                left_blood = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 世界boss被杀死通知
    /// </summary>
    public sealed class CLFRBossKilledNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 33;
        /// <summary>
        /// 你的排名，0代表未上榜
        /// </summary>
        public int rank;
        /// <summary>
        /// 获得的金币数量，仅用于显示
        /// </summary>
        public Int64 gain_currency;
        /// <summary>
        /// 奖励物品数组长度
        /// </summary>
        public sbyte item_length;
        /// <summary>
        /// 奖励物品数组
        /// </summary>
        public CLFRItemInfo[] item_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(rank);
            bw.Write(gain_currency);
            bw.Write(item_length);
            for (int i = 0; i < (int)item_length; i++)
                item_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                rank = br.ReadInt32();
                gain_currency = br.ReadInt64();
                item_length = br.ReadSByte();
                item_array = new CLFRItemInfo[(int)item_length];
                for (int i = 0; i < item_array.Length; i++)
                {
                    item_array[i] = new CLFRItemInfo();
                    item_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// Boss排行榜玩家信息
    /// </summary>
    public sealed class CLFRBossRankPlayerInfo : INetProtocol
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
        /// 打boss过程中所获得的金币数量
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
    /// 获取Boss排行榜请求
    /// </summary>
    public sealed class CLFRBossRankReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
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
    /// 获取Boss排行榜回应
    /// </summary>
    public sealed class CLFRBossRankAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 35;
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
        public CLFRBossRankPlayerInfo[] rank_rows;  //max:100
        /// <summary>
        /// 击杀boss的玩家Id
        /// </summary>
        public int killer_user_id;
        /// <summary>
        /// 世界boss的模板Id
        /// </summary>
        public int boss_config_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(rank_len);
            for (int i = 0; i < (int)rank_len; i++)
                rank_rows[i].toBinary(bw);
            bw.Write(killer_user_id);
            bw.Write(boss_config_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                rank_len = br.ReadInt32();
                rank_rows = new CLFRBossRankPlayerInfo[(int)rank_len];
                for (int i = 0; i < rank_rows.Length; i++)
                {
                    rank_rows[i] = new CLFRBossRankPlayerInfo();
                    rank_rows[i].fromBinary(br);
                }
                killer_user_id = br.ReadInt32();
                boss_config_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 鱼被移除通知
    /// </summary>
    public sealed class CLFRFishRemoveNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 36;
        /// <summary>
        /// 关联的座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 被移除数量
        /// </summary>
        public int len;
        /// <summary>
        /// 被移除的鱼数组
        /// </summary>
        public int[] fish_ids;  //max:100
        /// <summary>
        /// 移除原因 1使用弹头道具
        /// </summary>
        public int reason;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(len);
            for (int i = 0; i < (int)len; i++)
                bw.Write(fish_ids[i]);
            bw.Write(reason);
            bw.Write(delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                len = br.ReadInt32();
                fish_ids = new int[(int)len];
                for (int i = 0; i < fish_ids.Length; i++)
                    fish_ids[i] = br.ReadInt32();
                reason = br.ReadInt32();
                delta = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 显示老虎机转盘
    /// </summary>
    public sealed class CLFRShowWheel1Ntf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 37;
        /// <summary>
        /// 关联的座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 鱼的配置Id
        /// </summary>
        public int fish_config_id;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 delta;
        /// <summary>
        /// 显示倍数
        /// </summary>
        public int multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(fish_config_id);
            bw.Write(delta);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                fish_config_id = br.ReadInt32();
                delta = br.ReadInt64();
                multiple = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 显示双轮盘转盘
    /// </summary>
    public sealed class CLFRShowWheel2Ntf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 38;
        /// <summary>
        /// 关联的座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 鱼的配置Id
        /// </summary>
        public int fish_config_id;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 delta;
        /// <summary>
        /// 鱼倍率
        /// </summary>
        public int multiple;
        /// <summary>
        /// 系数万分比，使用时需除以10000
        /// </summary>
        public int degree;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(fish_config_id);
            bw.Write(delta);
            bw.Write(multiple);
            bw.Write(degree);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                fish_config_id = br.ReadInt32();
                delta = br.ReadInt64();
                multiple = br.ReadInt32();
                degree = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 座位资源变化通知
    /// </summary>
    public sealed class CLFRSeatResourceChangedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 39;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 钻石变化
        /// </summary>
        public Int64 diamond_delta;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 变化原因，详见:resource_log_reason_type
        /// </summary>
        public int reason;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(diamond_delta);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(reason);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                diamond_delta = br.ReadInt64();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                reason = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 座位使用道具通知
    /// </summary>
    public sealed class CLFRSeatItemUsedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 40;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 使用的物品
        /// </summary>
        public CLFRItemInfo item;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            item.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                item = new CLFRItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 炮台解锁请求
    /// </summary>
    public sealed class CLFRGunUnlockReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 41;

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
    /// 炮台解锁回应
    /// </summary>
    public sealed class CLFRGunUnlockAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 42;
        /// <summary>
        /// 0成功 1资源不足 2道具不足 3系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 解锁后最大炮值
        /// </summary>
        public Int64 max_gun_value;
        /// <summary>
        /// 钻石变化量
        /// </summary>
        public Int64 diamond_delta;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(max_gun_value);
            bw.Write(diamond_delta);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                max_gun_value = br.ReadInt64();
                diamond_delta = br.ReadInt64();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 奖金鱼池发生变化通知
    /// </summary>
    public sealed class CLFRBonusPoolChangedNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 43;
        /// <summary>
        /// 最新奖池金币数量
        /// </summary>
        public Int64 bonus_pool;
        /// <summary>
        /// 最新击杀奖金鱼数量
        /// </summary>
        public int bonus_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(bonus_pool);
            bw.Write(bonus_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bonus_pool = br.ReadInt64();
                bonus_count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 奖金抽奖请求
    /// </summary>
    public sealed class CLFRBonusWheelReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 44;

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
    /// 奖金抽奖回应
    /// </summary>
    public sealed class CLFRBonusWheelAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 45;
        /// <summary>
        /// 0成功 1条件不足 2系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 奖励的道具信息
        /// </summary>
        public CLFRItemInfo item;

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
                item = new CLFRItemInfo();
                item.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 添加分身效果通知
    /// </summary>
    public sealed class CLFRAddEffectShadowNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 46;
        /// <summary>
        /// 座位Id：0~3
        /// </summary>
        public int seat_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 召唤鱼通知
    /// </summary>
    public sealed class CLFRFishSummonNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 47;
        /// <summary>
        /// 座位Id：0~3
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 新召唤的鱼
        /// </summary>
        public CLFRFishAppearInfo fish;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            fish.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                fish = new CLFRFishAppearInfo();
                fish.fromBinary(br);
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 炮台切换请求
    /// </summary>
    public sealed class CLFRGunSwitchReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 48;
        /// <summary>
        /// 新的炮台Id
        /// </summary>
        public int gun_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(gun_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                gun_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 炮台切换回应
    /// </summary>
    public sealed class CLFRGunSwitchAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 49;
        /// <summary>
        /// 0成功 1vip等级不足 2系统错误
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
    /// 炮台切换通知
    /// </summary>
    public sealed class CLFRGunSwitchNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 50;
        /// <summary>
        /// 座位Id：0~3
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 炮台Id
        /// </summary>
        public int gun_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(gun_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                gun_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 申请鱼潮来临请求
    /// </summary>
    public sealed class CLFRFishTideForTestReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
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
    /// 申请鱼潮来临回应
    /// </summary>
    public sealed class CLFRFishTideForTestAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 52;
        /// <summary>
        /// 0成功 1还未结束呢 2你不在房间中
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
    /// 查询房间内获取到的积分请求
    /// </summary>
    public sealed class CLFRIntegralGainQueryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 53;

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
    /// 查询房间内获取到的积分回应
    /// </summary>
    public sealed class CLFRIntegralGainQueryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 54;
        /// <summary>
        /// 房间内获取到的积分
        /// </summary>
        public Int64 gain_integral;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(gain_integral);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                gain_integral = br.ReadInt64();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 使用弹头锁定鱼请求
    /// </summary>
    public sealed class CLFRWarheadLockReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 55;
        /// <summary>
        /// 弹头物品Id
        /// </summary>
        public int item_id;
        /// <summary>
        /// 弹头物品子Id
        /// </summary>
        public int item_sub_id;
        /// <summary>
        /// 使用弹头的数量
        /// </summary>
        public int item_count;
        /// <summary>
        /// 弹头想要炸死的鱼Id
        /// </summary>
        public int fish_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(item_id);
            bw.Write(item_sub_id);
            bw.Write(item_count);
            bw.Write(fish_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                item_id = br.ReadInt32();
                item_sub_id = br.ReadInt32();
                item_count = br.ReadInt32();
                fish_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 使用弹头锁定鱼回应
    /// </summary>
    public sealed class CLFRWarheadLockAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 56;
        /// <summary>
        /// 0成功 1物品不足 2已使用弹头锁定过 3鱼不存在 4该鱼不能被炸
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
    /// 弹头锁定通知
    /// </summary>
    public sealed class CLFRWarheadLockNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 57;
        /// <summary>
        /// 炮台位置：0~3
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 弹头物品Id
        /// </summary>
        public int item_id;
        /// <summary>
        /// 弹头物品子Id
        /// </summary>
        public int item_sub_id;
        /// <summary>
        /// 使用弹头的数量
        /// </summary>
        public int item_count;
        /// <summary>
        /// 弹头命中的鱼Id
        /// </summary>
        public int fish_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(item_id);
            bw.Write(item_sub_id);
            bw.Write(item_count);
            bw.Write(fish_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                item_id = br.ReadInt32();
                item_sub_id = br.ReadInt32();
                item_count = br.ReadInt32();
                fish_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 弹头爆炸请求
    /// </summary>
    public sealed class CLFRWarheadBoomReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 58;

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
    /// 弹头爆炸回应
    /// </summary>
    public sealed class CLFRWarheadBoomAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 59;
        /// <summary>
        /// 0成功 1尚未使用弹头锁定过 2该鱼已不存在 3弹头配置错误
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
    /// 弹头爆炸通知
    /// </summary>
    public sealed class CLFRWarheadBoomNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 60;
        /// <summary>
        /// 炮台位置：0~3
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 弹头物品Id
        /// </summary>
        public int item_id;
        /// <summary>
        /// 弹头物品子Id
        /// </summary>
        public int item_sub_id;
        /// <summary>
        /// 使用弹头的数量
        /// </summary>
        public int item_count;
        /// <summary>
        /// 弹头命中的鱼Id
        /// </summary>
        public int fish_id;
        /// <summary>
        /// 爆炸结果 0成功炸死鱼 1鱼已不存在炸失败 2弹头配置错误
        /// </summary>
        public int boom_result;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(item_id);
            bw.Write(item_sub_id);
            bw.Write(item_count);
            bw.Write(fish_id);
            bw.Write(boom_result);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                item_id = br.ReadInt32();
                item_sub_id = br.ReadInt32();
                item_count = br.ReadInt32();
                fish_id = br.ReadInt32();
                boom_result = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 倍击倍数改变请求
    /// </summary>
    public sealed class CLFRMultipleHitChangeReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 61;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public int value;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(value);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                value = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 倍击倍数改变回应
    /// </summary>
    public sealed class CLFRMultipleHitChangeAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 62;
        /// <summary>
        /// 0成功 1vip等级不足
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前倍击倍数
        /// </summary>
        public int value;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(value);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                value = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss下次出现的时间请求
    /// </summary>
    public sealed class CLFRBossNextAppearTimeReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 63;

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
    /// boss下次出现的时间回应
    /// </summary>
    public sealed class CLFRBossNextAppearTimeAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 64;
        /// <summary>
        /// 0成功 1你不在房间中 2该房间不会出现boss
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// bossId
        /// </summary>
        public int boss_id;
        /// <summary>
        /// boss出现的时间戳，单位总秒数
        /// </summary>
        public UInt32 timestamp;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(boss_id);
            bw.Write(timestamp);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                boss_id = br.ReadInt32();
                timestamp = br.ReadUInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 召唤boss请求
    /// </summary>
    public sealed class CLFRBossSummonReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 65;
        /// <summary>
        /// 想要召唤的bossId
        /// </summary>
        public int boss_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(boss_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                boss_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 召唤boss回应
    /// </summary>
    public sealed class CLFRBossSummonAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 66;
        /// <summary>
        /// 0成功 1你不在房间 2boss还活着
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
    /// 穿透发炮请求
    /// </summary>
    public sealed class CLFRAcrossShootReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 67;
        /// <summary>
        /// 发炮角度
        /// </summary>
        public int angle;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(angle);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                angle = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 穿透发炮回应
    /// </summary>
    public sealed class CLFRAcrossShootAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 68;
        /// <summary>
        /// 0成功 1金币不足 2子弹数量不足 3系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(bullet_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                bullet_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 穿透发炮通知
    /// </summary>
    public sealed class CLFRAcrossShootNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 69;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 发炮角度
        /// </summary>
        public int angle;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(bullet_id);
            bw.Write(angle);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                bullet_id = br.ReadInt32();
                angle = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 穿透子弹信息
    /// </summary>
    public sealed class CLFRAcrossBulletInfo : INetProtocol
    {
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 发炮角度
        /// </summary>
        public int angle;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(bullet_id);
            bw.Write(angle);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bullet_id = br.ReadInt32();
                angle = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 多炮台穿透发炮请求
    /// </summary>
    public sealed class CLFRAcrossMultiShootReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 70;
        /// <summary>
        /// 发炮数量
        /// </summary>
        public sbyte shoot_len;
        /// <summary>
        /// 发炮信息数组
        /// </summary>
        public int[] shoot_array;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(shoot_len);
            for (int i = 0; i < (int)shoot_len; i++)
                bw.Write(shoot_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                shoot_len = br.ReadSByte();
                shoot_array = new int[(int)shoot_len];
                for (int i = 0; i < shoot_array.Length; i++)
                    shoot_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 多炮台穿透发炮回应
    /// </summary>
    public sealed class CLFRAcrossMultiShootAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 71;
        /// <summary>
        /// 0成功 1金币不足 2子弹数量不足 3系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 子弹数量
        /// </summary>
        public sbyte bullet_len;
        /// <summary>
        /// 子弹信息数组
        /// </summary>
        public CLFRAcrossBulletInfo[] bullet_array;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(bullet_len);
            for (int i = 0; i < (int)bullet_len; i++)
                bullet_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                bullet_len = br.ReadSByte();
                bullet_array = new CLFRAcrossBulletInfo[(int)bullet_len];
                for (int i = 0; i < bullet_array.Length; i++)
                {
                    bullet_array[i] = new CLFRAcrossBulletInfo();
                    bullet_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 多炮台同时发炮通知
    /// </summary>
    public sealed class CLFRAcrossMultiShootNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 72;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 子弹数量
        /// </summary>
        public sbyte bullet_len;
        /// <summary>
        /// 子弹信息数组
        /// </summary>
        public CLFRAcrossBulletInfo[] bullet_array;  //max:10

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(bullet_len);
            for (int i = 0; i < (int)bullet_len; i++)
                bullet_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                bullet_len = br.ReadSByte();
                bullet_array = new CLFRAcrossBulletInfo[(int)bullet_len];
                for (int i = 0; i < bullet_array.Length; i++)
                {
                    bullet_array[i] = new CLFRAcrossBulletInfo();
                    bullet_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 穿透命中请求
    /// </summary>
    public sealed class CLFRAcrossHitReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 73;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 鱼Id
        /// </summary>
        public int fish_id;
        /// <summary>
        /// 关联的鱼数组长度，目前用于黑洞鱼
        /// </summary>
        public sbyte related_fish_len;
        /// <summary>
        /// 关联的鱼Id数组，目前用于黑洞鱼
        /// </summary>
        public int[] related_fish_array;  //max:30

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(bullet_id);
            bw.Write(fish_id);
            bw.Write(related_fish_len);
            for (int i = 0; i < (int)related_fish_len; i++)
                bw.Write(related_fish_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bullet_id = br.ReadInt32();
                fish_id = br.ReadInt32();
                related_fish_len = br.ReadSByte();
                related_fish_array = new int[(int)related_fish_len];
                for (int i = 0; i < related_fish_array.Length; i++)
                    related_fish_array[i] = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 穿透命中回应
    /// </summary>
    public sealed class CLFRAcrossHitAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 74;
        /// <summary>
        /// 0成功 1金币不足 2子弹数量不足 3系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 是否打爆 0否 1是
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 获得的金币
        /// </summary>
        public Int64 win_currency;
        /// <summary>
        /// 获得的钻石
        /// </summary>
        public Int64 win_diamond;
        /// <summary>
        /// 获得的比赛积分
        /// </summary>
        public int win_match_integral;
        /// <summary>
        /// 掉落物品数量
        /// </summary>
        public sbyte item_len;
        /// <summary>
        /// 掉落的物品数组
        /// </summary>
        public CLFRItemInfo[] items;  //max:100
        /// <summary>
        /// 其他关联的死亡鱼的数组长度
        /// </summary>
        public sbyte related_fish_len;
        /// <summary>
        /// 其他关联的死亡鱼Id的数组
        /// </summary>
        public int[] related_fish_array;  //max:100
        /// <summary>
        /// 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
        /// </summary>
        public sbyte related_remove_reason;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(is_boom);
            bw.Write(win_currency);
            bw.Write(win_diamond);
            bw.Write(win_match_integral);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
            bw.Write(related_fish_len);
            for (int i = 0; i < (int)related_fish_len; i++)
                bw.Write(related_fish_array[i]);
            bw.Write(related_remove_reason);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                is_boom = br.ReadSByte();
                win_currency = br.ReadInt64();
                win_diamond = br.ReadInt64();
                win_match_integral = br.ReadInt32();
                item_len = br.ReadSByte();
                items = new CLFRItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLFRItemInfo();
                    items[i].fromBinary(br);
                }
                related_fish_len = br.ReadSByte();
                related_fish_array = new int[(int)related_fish_len];
                for (int i = 0; i < related_fish_array.Length; i++)
                    related_fish_array[i] = br.ReadInt32();
                related_remove_reason = br.ReadSByte();
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 穿透命中通知
    /// </summary>
    public sealed class CLFRAcrossHitNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 75;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 鱼Id
        /// </summary>
        public int fish_id;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 是否打爆 0否 1是
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 获得的金币
        /// </summary>
        public Int64 win_currency;
        /// <summary>
        /// 获得的钻石
        /// </summary>
        public Int64 win_diamond;
        /// <summary>
        /// 获得的比赛积分
        /// </summary>
        public int win_match_integral;
        /// <summary>
        /// 掉落物品数量
        /// </summary>
        public sbyte item_len;
        /// <summary>
        /// 掉落的物品数组
        /// </summary>
        public CLFRItemInfo[] items;  //max:100
        /// <summary>
        /// 其他关联的死亡鱼的数组长度
        /// </summary>
        public sbyte related_fish_len;
        /// <summary>
        /// 其他关联的死亡鱼Id的数组
        /// </summary>
        public int[] related_fish_array;  //max:100
        /// <summary>
        /// 移除原因 0未知 1炸弹鱼移除 2闪电鱼移除 3鱼王移除 4黑洞鱼移除 5一网打尽
        /// </summary>
        public sbyte related_remove_reason;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(bullet_id);
            bw.Write(fish_id);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(is_boom);
            bw.Write(win_currency);
            bw.Write(win_diamond);
            bw.Write(win_match_integral);
            bw.Write(item_len);
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
            bw.Write(related_fish_len);
            for (int i = 0; i < (int)related_fish_len; i++)
                bw.Write(related_fish_array[i]);
            bw.Write(related_remove_reason);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                bullet_id = br.ReadInt32();
                fish_id = br.ReadInt32();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                is_boom = br.ReadSByte();
                win_currency = br.ReadInt64();
                win_diamond = br.ReadInt64();
                win_match_integral = br.ReadInt32();
                item_len = br.ReadSByte();
                items = new CLFRItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLFRItemInfo();
                    items[i].fromBinary(br);
                }
                related_fish_len = br.ReadSByte();
                related_fish_array = new int[(int)related_fish_len];
                for (int i = 0; i < related_fish_array.Length; i++)
                    related_fish_array[i] = br.ReadInt32();
                related_remove_reason = br.ReadSByte();
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss穿透命中请求
    /// </summary>
    public sealed class CLFRAcrossBossHitReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 76;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(bullet_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                bullet_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss穿透命中回应
    /// </summary>
    public sealed class CLFRAcrossBossHitAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 77;
        /// <summary>
        /// 0成功 1金币不足 2子弹数量不足 3系统错误
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 是否爆金 0miss 1爆金 2死亡
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 获得的金币
        /// </summary>
        public Int64 win_currency;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(is_boom);
            bw.Write(win_currency);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                is_boom = br.ReadSByte();
                win_currency = br.ReadInt64();
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// boss穿透命中通知
    /// </summary>
    public sealed class CLFRAcrossBossHitNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 11;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 78;
        /// <summary>
        /// 座位Id
        /// </summary>
        public int seat_id;
        /// <summary>
        /// 子弹Id
        /// </summary>
        public int bullet_id;
        /// <summary>
        /// 金币变化量
        /// </summary>
        public Int64 currency_delta;
        /// <summary>
        /// 绑定金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 是否爆金 0miss 1爆金 2死亡
        /// </summary>
        public sbyte is_boom;
        /// <summary>
        /// 获得的金币
        /// </summary>
        public Int64 win_currency;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public byte multiple;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(seat_id);
            bw.Write(bullet_id);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(is_boom);
            bw.Write(win_currency);
            bw.Write(multiple);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                seat_id = br.ReadInt32();
                bullet_id = br.ReadInt32();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                is_boom = br.ReadSByte();
                win_currency = br.ReadInt64();
                multiple = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }

    public class ClientFishingRoomResponserBase : INetResponser
    {
        public bool processPackage(BinaryReader br, INetReactor reactor, out INetProtocol responseProto)
        {
            responseProto = null;
            if (br.ReadUInt16() != 11)
                return false;

            switch(br.ReadUInt16())
            {
                case 0:
                    responseProto = new CLFREnterGameReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFREnterGameReq(responseProto as CLFREnterGameReq);
                    break;
                case 1:
                    responseProto = new CLFREnterGameAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFREnterGameAck(responseProto as CLFREnterGameAck);
                    break;
                case 2:
                    responseProto = new CLFRExitGameReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRExitGameReq(responseProto as CLFRExitGameReq);
                    break;
                case 3:
                    responseProto = new CLFRExitGameAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRExitGameAck(responseProto as CLFRExitGameAck);
                    break;
                case 4:
                    responseProto = new CLFRGetReadyReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGetReadyReq(responseProto as CLFRGetReadyReq);
                    break;
                case 5:
                    responseProto = new CLFRGetReadyAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGetReadyAck(responseProto as CLFRGetReadyAck);
                    break;
                case 6:
                    responseProto = new CLFRJoinMatchReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRJoinMatchReq(responseProto as CLFRJoinMatchReq);
                    break;
                case 7:
                    responseProto = new CLFRJoinMatchAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRJoinMatchAck(responseProto as CLFRJoinMatchAck);
                    break;
                case 8:
                    responseProto = new CLFRJoinMatchNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRJoinMatchNtf(responseProto as CLFRJoinMatchNtf);
                    break;
                case 9:
                    responseProto = new CLFRMatchOverNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRMatchOverNtf(responseProto as CLFRMatchOverNtf);
                    break;
                case 10:
                    responseProto = new CLFRPlayerJoinNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRPlayerJoinNtf(responseProto as CLFRPlayerJoinNtf);
                    break;
                case 11:
                    responseProto = new CLFRPlayerLeaveNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRPlayerLeaveNtf(responseProto as CLFRPlayerLeaveNtf);
                    break;
                case 12:
                    responseProto = new CLFRFishTideStartNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRFishTideStartNtf(responseProto as CLFRFishTideStartNtf);
                    break;
                case 13:
                    responseProto = new CLFRFishAppearNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRFishAppearNtf(responseProto as CLFRFishAppearNtf);
                    break;
                case 14:
                    responseProto = new CLFRFishTimeRateChangeNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRFishTimeRateChangeNtf(responseProto as CLFRFishTimeRateChangeNtf);
                    break;
                case 15:
                    responseProto = new CLFRGunValueChangeReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunValueChangeReq(responseProto as CLFRGunValueChangeReq);
                    break;
                case 16:
                    responseProto = new CLFRGunValueChangeAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunValueChangeAck(responseProto as CLFRGunValueChangeAck);
                    break;
                case 17:
                    responseProto = new CLFRGunValueChangeNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunValueChangeNtf(responseProto as CLFRGunValueChangeNtf);
                    break;
                case 18:
                    responseProto = new CLFRShootReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRShootReq(responseProto as CLFRShootReq);
                    break;
                case 19:
                    responseProto = new CLFRShootAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRShootAck(responseProto as CLFRShootAck);
                    break;
                case 20:
                    responseProto = new CLFRShootNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRShootNtf(responseProto as CLFRShootNtf);
                    break;
                case 21:
                    responseProto = new CLFRMultiShootReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRMultiShootReq(responseProto as CLFRMultiShootReq);
                    break;
                case 22:
                    responseProto = new CLFRMultiShootAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRMultiShootAck(responseProto as CLFRMultiShootAck);
                    break;
                case 23:
                    responseProto = new CLFRMultiShootNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRMultiShootNtf(responseProto as CLFRMultiShootNtf);
                    break;
                case 24:
                    responseProto = new CLFRHitReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRHitReq(responseProto as CLFRHitReq);
                    break;
                case 25:
                    responseProto = new CLFRHitAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRHitAck(responseProto as CLFRHitAck);
                    break;
                case 26:
                    responseProto = new CLFRHitNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRHitNtf(responseProto as CLFRHitNtf);
                    break;
                case 27:
                    responseProto = new CLFRRobotHitRpt();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRRobotHitRpt(responseProto as CLFRRobotHitRpt);
                    break;
                case 28:
                    responseProto = new CLFRBossAppearNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossAppearNtf(responseProto as CLFRBossAppearNtf);
                    break;
                case 29:
                    responseProto = new CLFRBossHitReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossHitReq(responseProto as CLFRBossHitReq);
                    break;
                case 30:
                    responseProto = new CLFRBossHitAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossHitAck(responseProto as CLFRBossHitAck);
                    break;
                case 31:
                    responseProto = new CLFRBossHitNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossHitNtf(responseProto as CLFRBossHitNtf);
                    break;
                case 32:
                    responseProto = new CLFRBossDefenceBloodChangedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossDefenceBloodChangedNtf(responseProto as CLFRBossDefenceBloodChangedNtf);
                    break;
                case 33:
                    responseProto = new CLFRBossKilledNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossKilledNtf(responseProto as CLFRBossKilledNtf);
                    break;
                case 34:
                    responseProto = new CLFRBossRankReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossRankReq(responseProto as CLFRBossRankReq);
                    break;
                case 35:
                    responseProto = new CLFRBossRankAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossRankAck(responseProto as CLFRBossRankAck);
                    break;
                case 36:
                    responseProto = new CLFRFishRemoveNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRFishRemoveNtf(responseProto as CLFRFishRemoveNtf);
                    break;
                case 37:
                    responseProto = new CLFRShowWheel1Ntf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRShowWheel1Ntf(responseProto as CLFRShowWheel1Ntf);
                    break;
                case 38:
                    responseProto = new CLFRShowWheel2Ntf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRShowWheel2Ntf(responseProto as CLFRShowWheel2Ntf);
                    break;
                case 39:
                    responseProto = new CLFRSeatResourceChangedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRSeatResourceChangedNtf(responseProto as CLFRSeatResourceChangedNtf);
                    break;
                case 40:
                    responseProto = new CLFRSeatItemUsedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRSeatItemUsedNtf(responseProto as CLFRSeatItemUsedNtf);
                    break;
                case 41:
                    responseProto = new CLFRGunUnlockReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunUnlockReq(responseProto as CLFRGunUnlockReq);
                    break;
                case 42:
                    responseProto = new CLFRGunUnlockAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunUnlockAck(responseProto as CLFRGunUnlockAck);
                    break;
                case 43:
                    responseProto = new CLFRBonusPoolChangedNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBonusPoolChangedNtf(responseProto as CLFRBonusPoolChangedNtf);
                    break;
                case 44:
                    responseProto = new CLFRBonusWheelReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBonusWheelReq(responseProto as CLFRBonusWheelReq);
                    break;
                case 45:
                    responseProto = new CLFRBonusWheelAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBonusWheelAck(responseProto as CLFRBonusWheelAck);
                    break;
                case 46:
                    responseProto = new CLFRAddEffectShadowNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAddEffectShadowNtf(responseProto as CLFRAddEffectShadowNtf);
                    break;
                case 47:
                    responseProto = new CLFRFishSummonNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRFishSummonNtf(responseProto as CLFRFishSummonNtf);
                    break;
                case 48:
                    responseProto = new CLFRGunSwitchReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunSwitchReq(responseProto as CLFRGunSwitchReq);
                    break;
                case 49:
                    responseProto = new CLFRGunSwitchAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunSwitchAck(responseProto as CLFRGunSwitchAck);
                    break;
                case 50:
                    responseProto = new CLFRGunSwitchNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRGunSwitchNtf(responseProto as CLFRGunSwitchNtf);
                    break;
                case 51:
                    responseProto = new CLFRFishTideForTestReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRFishTideForTestReq(responseProto as CLFRFishTideForTestReq);
                    break;
                case 52:
                    responseProto = new CLFRFishTideForTestAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRFishTideForTestAck(responseProto as CLFRFishTideForTestAck);
                    break;
                case 53:
                    responseProto = new CLFRIntegralGainQueryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRIntegralGainQueryReq(responseProto as CLFRIntegralGainQueryReq);
                    break;
                case 54:
                    responseProto = new CLFRIntegralGainQueryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRIntegralGainQueryAck(responseProto as CLFRIntegralGainQueryAck);
                    break;
                case 55:
                    responseProto = new CLFRWarheadLockReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRWarheadLockReq(responseProto as CLFRWarheadLockReq);
                    break;
                case 56:
                    responseProto = new CLFRWarheadLockAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRWarheadLockAck(responseProto as CLFRWarheadLockAck);
                    break;
                case 57:
                    responseProto = new CLFRWarheadLockNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRWarheadLockNtf(responseProto as CLFRWarheadLockNtf);
                    break;
                case 58:
                    responseProto = new CLFRWarheadBoomReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRWarheadBoomReq(responseProto as CLFRWarheadBoomReq);
                    break;
                case 59:
                    responseProto = new CLFRWarheadBoomAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRWarheadBoomAck(responseProto as CLFRWarheadBoomAck);
                    break;
                case 60:
                    responseProto = new CLFRWarheadBoomNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRWarheadBoomNtf(responseProto as CLFRWarheadBoomNtf);
                    break;
                case 61:
                    responseProto = new CLFRMultipleHitChangeReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRMultipleHitChangeReq(responseProto as CLFRMultipleHitChangeReq);
                    break;
                case 62:
                    responseProto = new CLFRMultipleHitChangeAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRMultipleHitChangeAck(responseProto as CLFRMultipleHitChangeAck);
                    break;
                case 63:
                    responseProto = new CLFRBossNextAppearTimeReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossNextAppearTimeReq(responseProto as CLFRBossNextAppearTimeReq);
                    break;
                case 64:
                    responseProto = new CLFRBossNextAppearTimeAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossNextAppearTimeAck(responseProto as CLFRBossNextAppearTimeAck);
                    break;
                case 65:
                    responseProto = new CLFRBossSummonReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossSummonReq(responseProto as CLFRBossSummonReq);
                    break;
                case 66:
                    responseProto = new CLFRBossSummonAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRBossSummonAck(responseProto as CLFRBossSummonAck);
                    break;
                case 67:
                    responseProto = new CLFRAcrossShootReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossShootReq(responseProto as CLFRAcrossShootReq);
                    break;
                case 68:
                    responseProto = new CLFRAcrossShootAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossShootAck(responseProto as CLFRAcrossShootAck);
                    break;
                case 69:
                    responseProto = new CLFRAcrossShootNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossShootNtf(responseProto as CLFRAcrossShootNtf);
                    break;
                case 70:
                    responseProto = new CLFRAcrossMultiShootReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossMultiShootReq(responseProto as CLFRAcrossMultiShootReq);
                    break;
                case 71:
                    responseProto = new CLFRAcrossMultiShootAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossMultiShootAck(responseProto as CLFRAcrossMultiShootAck);
                    break;
                case 72:
                    responseProto = new CLFRAcrossMultiShootNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossMultiShootNtf(responseProto as CLFRAcrossMultiShootNtf);
                    break;
                case 73:
                    responseProto = new CLFRAcrossHitReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossHitReq(responseProto as CLFRAcrossHitReq);
                    break;
                case 74:
                    responseProto = new CLFRAcrossHitAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossHitAck(responseProto as CLFRAcrossHitAck);
                    break;
                case 75:
                    responseProto = new CLFRAcrossHitNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossHitNtf(responseProto as CLFRAcrossHitNtf);
                    break;
                case 76:
                    responseProto = new CLFRAcrossBossHitReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossBossHitReq(responseProto as CLFRAcrossBossHitReq);
                    break;
                case 77:
                    responseProto = new CLFRAcrossBossHitAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossBossHitAck(responseProto as CLFRAcrossBossHitAck);
                    break;
                case 78:
                    responseProto = new CLFRAcrossBossHitNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFRAcrossBossHitNtf(responseProto as CLFRAcrossBossHitNtf);
                    break;
            }
            return responseProto != null;
        }

        public virtual void onRecv_CLFREnterGameReq(CLFREnterGameReq proto) { }
        public virtual void onRecv_CLFREnterGameAck(CLFREnterGameAck proto) { }
        public virtual void onRecv_CLFRExitGameReq(CLFRExitGameReq proto) { }
        public virtual void onRecv_CLFRExitGameAck(CLFRExitGameAck proto) { }
        public virtual void onRecv_CLFRGetReadyReq(CLFRGetReadyReq proto) { }
        public virtual void onRecv_CLFRGetReadyAck(CLFRGetReadyAck proto) { }
        public virtual void onRecv_CLFRJoinMatchReq(CLFRJoinMatchReq proto) { }
        public virtual void onRecv_CLFRJoinMatchAck(CLFRJoinMatchAck proto) { }
        public virtual void onRecv_CLFRJoinMatchNtf(CLFRJoinMatchNtf proto) { }
        public virtual void onRecv_CLFRMatchOverNtf(CLFRMatchOverNtf proto) { }
        public virtual void onRecv_CLFRPlayerJoinNtf(CLFRPlayerJoinNtf proto) { }
        public virtual void onRecv_CLFRPlayerLeaveNtf(CLFRPlayerLeaveNtf proto) { }
        public virtual void onRecv_CLFRFishTideStartNtf(CLFRFishTideStartNtf proto) { }
        public virtual void onRecv_CLFRFishAppearNtf(CLFRFishAppearNtf proto) { }
        public virtual void onRecv_CLFRFishTimeRateChangeNtf(CLFRFishTimeRateChangeNtf proto) { }
        public virtual void onRecv_CLFRGunValueChangeReq(CLFRGunValueChangeReq proto) { }
        public virtual void onRecv_CLFRGunValueChangeAck(CLFRGunValueChangeAck proto) { }
        public virtual void onRecv_CLFRGunValueChangeNtf(CLFRGunValueChangeNtf proto) { }
        public virtual void onRecv_CLFRShootReq(CLFRShootReq proto) { }
        public virtual void onRecv_CLFRShootAck(CLFRShootAck proto) { }
        public virtual void onRecv_CLFRShootNtf(CLFRShootNtf proto) { }
        public virtual void onRecv_CLFRMultiShootReq(CLFRMultiShootReq proto) { }
        public virtual void onRecv_CLFRMultiShootAck(CLFRMultiShootAck proto) { }
        public virtual void onRecv_CLFRMultiShootNtf(CLFRMultiShootNtf proto) { }
        public virtual void onRecv_CLFRHitReq(CLFRHitReq proto) { }
        public virtual void onRecv_CLFRHitAck(CLFRHitAck proto) { }
        public virtual void onRecv_CLFRHitNtf(CLFRHitNtf proto) { }
        public virtual void onRecv_CLFRRobotHitRpt(CLFRRobotHitRpt proto) { }
        public virtual void onRecv_CLFRBossAppearNtf(CLFRBossAppearNtf proto) { }
        public virtual void onRecv_CLFRBossHitReq(CLFRBossHitReq proto) { }
        public virtual void onRecv_CLFRBossHitAck(CLFRBossHitAck proto) { }
        public virtual void onRecv_CLFRBossHitNtf(CLFRBossHitNtf proto) { }
        public virtual void onRecv_CLFRBossDefenceBloodChangedNtf(CLFRBossDefenceBloodChangedNtf proto) { }
        public virtual void onRecv_CLFRBossKilledNtf(CLFRBossKilledNtf proto) { }
        public virtual void onRecv_CLFRBossRankReq(CLFRBossRankReq proto) { }
        public virtual void onRecv_CLFRBossRankAck(CLFRBossRankAck proto) { }
        public virtual void onRecv_CLFRFishRemoveNtf(CLFRFishRemoveNtf proto) { }
        public virtual void onRecv_CLFRShowWheel1Ntf(CLFRShowWheel1Ntf proto) { }
        public virtual void onRecv_CLFRShowWheel2Ntf(CLFRShowWheel2Ntf proto) { }
        public virtual void onRecv_CLFRSeatResourceChangedNtf(CLFRSeatResourceChangedNtf proto) { }
        public virtual void onRecv_CLFRSeatItemUsedNtf(CLFRSeatItemUsedNtf proto) { }
        public virtual void onRecv_CLFRGunUnlockReq(CLFRGunUnlockReq proto) { }
        public virtual void onRecv_CLFRGunUnlockAck(CLFRGunUnlockAck proto) { }
        public virtual void onRecv_CLFRBonusPoolChangedNtf(CLFRBonusPoolChangedNtf proto) { }
        public virtual void onRecv_CLFRBonusWheelReq(CLFRBonusWheelReq proto) { }
        public virtual void onRecv_CLFRBonusWheelAck(CLFRBonusWheelAck proto) { }
        public virtual void onRecv_CLFRAddEffectShadowNtf(CLFRAddEffectShadowNtf proto) { }
        public virtual void onRecv_CLFRFishSummonNtf(CLFRFishSummonNtf proto) { }
        public virtual void onRecv_CLFRGunSwitchReq(CLFRGunSwitchReq proto) { }
        public virtual void onRecv_CLFRGunSwitchAck(CLFRGunSwitchAck proto) { }
        public virtual void onRecv_CLFRGunSwitchNtf(CLFRGunSwitchNtf proto) { }
        public virtual void onRecv_CLFRFishTideForTestReq(CLFRFishTideForTestReq proto) { }
        public virtual void onRecv_CLFRFishTideForTestAck(CLFRFishTideForTestAck proto) { }
        public virtual void onRecv_CLFRIntegralGainQueryReq(CLFRIntegralGainQueryReq proto) { }
        public virtual void onRecv_CLFRIntegralGainQueryAck(CLFRIntegralGainQueryAck proto) { }
        public virtual void onRecv_CLFRWarheadLockReq(CLFRWarheadLockReq proto) { }
        public virtual void onRecv_CLFRWarheadLockAck(CLFRWarheadLockAck proto) { }
        public virtual void onRecv_CLFRWarheadLockNtf(CLFRWarheadLockNtf proto) { }
        public virtual void onRecv_CLFRWarheadBoomReq(CLFRWarheadBoomReq proto) { }
        public virtual void onRecv_CLFRWarheadBoomAck(CLFRWarheadBoomAck proto) { }
        public virtual void onRecv_CLFRWarheadBoomNtf(CLFRWarheadBoomNtf proto) { }
        public virtual void onRecv_CLFRMultipleHitChangeReq(CLFRMultipleHitChangeReq proto) { }
        public virtual void onRecv_CLFRMultipleHitChangeAck(CLFRMultipleHitChangeAck proto) { }
        public virtual void onRecv_CLFRBossNextAppearTimeReq(CLFRBossNextAppearTimeReq proto) { }
        public virtual void onRecv_CLFRBossNextAppearTimeAck(CLFRBossNextAppearTimeAck proto) { }
        public virtual void onRecv_CLFRBossSummonReq(CLFRBossSummonReq proto) { }
        public virtual void onRecv_CLFRBossSummonAck(CLFRBossSummonAck proto) { }
        public virtual void onRecv_CLFRAcrossShootReq(CLFRAcrossShootReq proto) { }
        public virtual void onRecv_CLFRAcrossShootAck(CLFRAcrossShootAck proto) { }
        public virtual void onRecv_CLFRAcrossShootNtf(CLFRAcrossShootNtf proto) { }
        public virtual void onRecv_CLFRAcrossMultiShootReq(CLFRAcrossMultiShootReq proto) { }
        public virtual void onRecv_CLFRAcrossMultiShootAck(CLFRAcrossMultiShootAck proto) { }
        public virtual void onRecv_CLFRAcrossMultiShootNtf(CLFRAcrossMultiShootNtf proto) { }
        public virtual void onRecv_CLFRAcrossHitReq(CLFRAcrossHitReq proto) { }
        public virtual void onRecv_CLFRAcrossHitAck(CLFRAcrossHitAck proto) { }
        public virtual void onRecv_CLFRAcrossHitNtf(CLFRAcrossHitNtf proto) { }
        public virtual void onRecv_CLFRAcrossBossHitReq(CLFRAcrossBossHitReq proto) { }
        public virtual void onRecv_CLFRAcrossBossHitAck(CLFRAcrossBossHitAck proto) { }
        public virtual void onRecv_CLFRAcrossBossHitNtf(CLFRAcrossBossHitNtf proto) { }
    }
}
