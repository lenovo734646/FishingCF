using System;
using System.IO;

namespace JBPROTO
{
    /// <summary>
    /// 登入服务应答
    /// </summary>
    public sealed class CLFMEnterServerNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 0;
        /// <summary>
        /// 当前的炮台Id
        /// </summary>
        public int gun_id;
        /// <summary>
        /// 解锁的最大炮值
        /// </summary>
        public Int64 max_gun_value;
        /// <summary>
        /// 个人累积的奖金池数量
        /// </summary>
        public Int64 bonus_pool;
        /// <summary>
        /// 个人累积的打死奖金鱼数量
        /// </summary>
        public int bonus_count;
        /// <summary>
        /// 倍击倍数
        /// </summary>
        public int multiple_hit;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(gun_id);
            bw.Write(max_gun_value);
            bw.Write(bonus_pool);
            bw.Write(bonus_count);
            bw.Write(multiple_hit);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                gun_id = br.ReadInt32();
                max_gun_value = br.ReadInt64();
                bonus_pool = br.ReadInt64();
                bonus_count = br.ReadInt32();
                multiple_hit = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 加入玩法请求
    /// </summary>
    public sealed class CLFMEnterSiteReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 1;
        /// <summary>
        /// 玩法Id 1捕鱼3D 2捕鱼2D
        /// </summary>
        public int site_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(site_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                site_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 加入玩法回应
    /// </summary>
    public sealed class CLFMEnterSiteAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 2;
        /// <summary>
        /// 0成功 1无可用服务器 2系统错误
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
    /// 退出玩法请求
    /// </summary>
    public sealed class CLFMExitSiteReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 3;
        /// <summary>
        /// 玩法Id 1捕鱼3D 2捕鱼2D
        /// </summary>
        public int site_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(site_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                site_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 退出玩法回应
    /// </summary>
    public sealed class CLFMExitSiteAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 4;
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
    /// 炮台锻造请求
    /// </summary>
    public sealed class CLFMGunForgeReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 5;
        /// <summary>
        /// 是否使用100个水晶保证成功
        /// </summary>
        public sbyte use_crystal;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(use_crystal);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                use_crystal = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 炮台锻造回应
    /// </summary>
    public sealed class CLFMGunForgeAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 6;
        /// <summary>
        /// 0成功 1锻造失败 2资源不足 3道具不足 4系统错误
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
        /// 金币变化量
        /// </summary>
        public Int64 bind_currency_delta;
        /// <summary>
        /// 水晶物品Id
        /// </summary>
        public int crystal_id;
        /// <summary>
        /// 水晶物品子Id
        /// </summary>
        public int crystal_sub_id;
        /// <summary>
        /// 锻造失败时返还的水晶数量
        /// </summary>
        public int crystal_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(max_gun_value);
            bw.Write(diamond_delta);
            bw.Write(currency_delta);
            bw.Write(bind_currency_delta);
            bw.Write(crystal_id);
            bw.Write(crystal_sub_id);
            bw.Write(crystal_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                max_gun_value = br.ReadInt64();
                diamond_delta = br.ReadInt64();
                currency_delta = br.ReadInt64();
                bind_currency_delta = br.ReadInt64();
                crystal_id = br.ReadInt32();
                crystal_sub_id = br.ReadInt32();
                crystal_count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 比赛排行信息
    /// </summary>
    public sealed class CLFMMatchRankInfo : INetProtocol
    {
        /// <summary>
        /// 排名段起始
        /// </summary>
        public int min_rank;
        /// <summary>
        /// 排名段结束
        /// </summary>
        public int max_rank;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 排名段结束名次所对应积分
        /// </summary>
        public int integral;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(min_rank);
            bw.Write(max_rank);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(integral);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                min_rank = br.ReadInt32();
                max_rank = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                integral = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 比赛排行榜请求
    /// </summary>
    public sealed class CLFMMatchRankReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 7;
        /// <summary>
        /// 1免费赛 2大奖赛
        /// </summary>
        public int match_type;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(match_type);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                match_type = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 比赛排行榜回应
    /// </summary>
    public sealed class CLFMMatchRankAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 8;
        /// <summary>
        /// 我的今日积分
        /// </summary>
        public int my_day_integral;
        /// <summary>
        /// 我的今日排名
        /// </summary>
        public int my_day_rank;
        /// <summary>
        /// 我的本周积分
        /// </summary>
        public int my_week_integral;
        /// <summary>
        /// 预计周冠军的玩家Id
        /// </summary>
        public int top_week_user_id;
        /// <summary>
        /// 预计周冠军的昵称
        /// </summary>
        public string top_week_nickname;  //max:32
        /// <summary>
        /// 预计周冠军的总积分
        /// </summary>
        public int top_week_integral;
        /// <summary>
        /// 排行榜分段数组长度
        /// </summary>
        public int rank_section_len;
        /// <summary>
        /// 排行榜分段数组
        /// </summary>
        public CLFMMatchRankInfo[] rank_section_array;  //max:20

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(my_day_integral);
            bw.Write(my_day_rank);
            bw.Write(my_week_integral);
            bw.Write(top_week_user_id);
            NetHelper.SafeWriteString(bw, top_week_nickname, 32);
            bw.Write(top_week_integral);
            bw.Write(rank_section_len);
            for (int i = 0; i < (int)rank_section_len; i++)
                rank_section_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                my_day_integral = br.ReadInt32();
                my_day_rank = br.ReadInt32();
                my_week_integral = br.ReadInt32();
                top_week_user_id = br.ReadInt32();
                top_week_nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                top_week_integral = br.ReadInt32();
                rank_section_len = br.ReadInt32();
                rank_section_array = new CLFMMatchRankInfo[(int)rank_section_len];
                for (int i = 0; i < rank_section_array.Length; i++)
                {
                    rank_section_array[i] = new CLFMMatchRankInfo();
                    rank_section_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 房间人数信息
    /// </summary>
    public sealed class CLFMRoomUserCountInfo : INetProtocol
    {
        /// <summary>
        /// 房间配置Id
        /// </summary>
        public int config_id;
        /// <summary>
        /// 在线人数
        /// </summary>
        public int total_count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(config_id);
            bw.Write(total_count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                config_id = br.ReadInt32();
                total_count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询房间总人数请求
    /// </summary>
    public sealed class CLFMRoomUserCountSummaryReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 9;
        /// <summary>
        /// 玩法Id 1捕鱼3D 2捕鱼2D
        /// </summary>
        public int site_id;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(site_id);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                site_id = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询房间总人数回应
    /// </summary>
    public sealed class CLFMRoomUserCountSummaryAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 10;
        /// <summary>
        /// 房间人数信息数组长度
        /// </summary>
        public int info_len;
        /// <summary>
        /// 房间人数信息数组
        /// </summary>
        public CLFMRoomUserCountInfo[] info_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(info_len);
            for (int i = 0; i < (int)info_len; i++)
                info_array[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                info_len = br.ReadInt32();
                info_array = new CLFMRoomUserCountInfo[(int)info_len];
                for (int i = 0; i < info_array.Length; i++)
                {
                    info_array[i] = new CLFMRoomUserCountInfo();
                    info_array[i].fromBinary(br);
                }
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询房间详细人数请求
    /// </summary>
    public sealed class CLFMRoomUserCountDetailReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 11;
        /// <summary>
        /// 玩法Id 1捕鱼3D 2捕鱼2D
        /// </summary>
        public int site_id;
        /// <summary>
        /// 房间配置Id
        /// </summary>
        public int config_id;
        /// <summary>
        /// 请求房间的起始编号
        /// </summary>
        public int start_room_id;
        /// <summary>
        /// 请求数量
        /// </summary>
        public int count;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(site_id);
            bw.Write(config_id);
            bw.Write(start_room_id);
            bw.Write(count);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                site_id = br.ReadInt32();
                config_id = br.ReadInt32();
                start_room_id = br.ReadInt32();
                count = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 查询房间详细人数回应
    /// </summary>
    public sealed class CLFMRoomUserCountDetailAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 10;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 12;
        /// <summary>
        /// 0成功 1配置id非法
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 房间人数数组长度
        /// </summary>
        public int amount_len;
        /// <summary>
        /// 房间人数数组
        /// </summary>
        public sbyte[] amount_array;  //max:100

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(amount_len);
            for (int i = 0; i < (int)amount_len; i++)
                bw.Write(amount_array[i]);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                amount_len = br.ReadInt32();
                amount_array = new sbyte[(int)amount_len];
                for (int i = 0; i < amount_array.Length; i++)
                    amount_array[i] = br.ReadSByte();
            } catch (EndOfStreamException) { }
        }
    }

    public class ClientFishingMainResponserBase : INetResponser
    {
        public bool processPackage(BinaryReader br, INetReactor reactor, out INetProtocol responseProto)
        {
            responseProto = null;
            if (br.ReadUInt16() != 10)
                return false;

            switch(br.ReadUInt16())
            {
                case 0:
                    responseProto = new CLFMEnterServerNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMEnterServerNtf(responseProto as CLFMEnterServerNtf);
                    break;
                case 1:
                    responseProto = new CLFMEnterSiteReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMEnterSiteReq(responseProto as CLFMEnterSiteReq);
                    break;
                case 2:
                    responseProto = new CLFMEnterSiteAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMEnterSiteAck(responseProto as CLFMEnterSiteAck);
                    break;
                case 3:
                    responseProto = new CLFMExitSiteReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMExitSiteReq(responseProto as CLFMExitSiteReq);
                    break;
                case 4:
                    responseProto = new CLFMExitSiteAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMExitSiteAck(responseProto as CLFMExitSiteAck);
                    break;
                case 5:
                    responseProto = new CLFMGunForgeReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMGunForgeReq(responseProto as CLFMGunForgeReq);
                    break;
                case 6:
                    responseProto = new CLFMGunForgeAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMGunForgeAck(responseProto as CLFMGunForgeAck);
                    break;
                case 7:
                    responseProto = new CLFMMatchRankReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMMatchRankReq(responseProto as CLFMMatchRankReq);
                    break;
                case 8:
                    responseProto = new CLFMMatchRankAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMMatchRankAck(responseProto as CLFMMatchRankAck);
                    break;
                case 9:
                    responseProto = new CLFMRoomUserCountSummaryReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMRoomUserCountSummaryReq(responseProto as CLFMRoomUserCountSummaryReq);
                    break;
                case 10:
                    responseProto = new CLFMRoomUserCountSummaryAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMRoomUserCountSummaryAck(responseProto as CLFMRoomUserCountSummaryAck);
                    break;
                case 11:
                    responseProto = new CLFMRoomUserCountDetailReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMRoomUserCountDetailReq(responseProto as CLFMRoomUserCountDetailReq);
                    break;
                case 12:
                    responseProto = new CLFMRoomUserCountDetailAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLFMRoomUserCountDetailAck(responseProto as CLFMRoomUserCountDetailAck);
                    break;
            }
            return responseProto != null;
        }

        public virtual void onRecv_CLFMEnterServerNtf(CLFMEnterServerNtf proto) { }
        public virtual void onRecv_CLFMEnterSiteReq(CLFMEnterSiteReq proto) { }
        public virtual void onRecv_CLFMEnterSiteAck(CLFMEnterSiteAck proto) { }
        public virtual void onRecv_CLFMExitSiteReq(CLFMExitSiteReq proto) { }
        public virtual void onRecv_CLFMExitSiteAck(CLFMExitSiteAck proto) { }
        public virtual void onRecv_CLFMGunForgeReq(CLFMGunForgeReq proto) { }
        public virtual void onRecv_CLFMGunForgeAck(CLFMGunForgeAck proto) { }
        public virtual void onRecv_CLFMMatchRankReq(CLFMMatchRankReq proto) { }
        public virtual void onRecv_CLFMMatchRankAck(CLFMMatchRankAck proto) { }
        public virtual void onRecv_CLFMRoomUserCountSummaryReq(CLFMRoomUserCountSummaryReq proto) { }
        public virtual void onRecv_CLFMRoomUserCountSummaryAck(CLFMRoomUserCountSummaryAck proto) { }
        public virtual void onRecv_CLFMRoomUserCountDetailReq(CLFMRoomUserCountDetailReq proto) { }
        public virtual void onRecv_CLFMRoomUserCountDetailAck(CLFMRoomUserCountDetailAck proto) { }
    }
}
