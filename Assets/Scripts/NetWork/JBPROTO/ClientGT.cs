using System;
using System.IO;

namespace JBPROTO
{
    /// <summary>
    /// 握手请求
    /// </summary>
    public sealed class CLGTHandReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 0;
        /// <summary>
        /// 运行平台 1:IOS 2:ANDRIOD 3:WINDOWS 4:LINUX 5:MAC
        /// </summary>
        public UInt32 platform;
        /// <summary>
        /// 产品代号 0:未知的产品 1:游戏平台
        /// </summary>
        public UInt32 product;
        /// <summary>
        /// 产品版本号
        /// </summary>
        public UInt32 version;
        /// <summary>
        /// 机器设备码
        /// </summary>
        public string device;  //max:64
        /// <summary>
        /// 渠道
        /// </summary>
        public string channel;  //max:32
        /// <summary>
        /// 国家标识
        /// </summary>
        public string country;  //max:16
        /// <summary>
        /// 语言标识
        /// </summary>
        public string language;  //max:16

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(platform);
            bw.Write(product);
            bw.Write(version);
            NetHelper.SafeWriteString(bw, device, 64);
            NetHelper.SafeWriteString(bw, channel, 32);
            NetHelper.SafeWriteString(bw, country, 16);
            NetHelper.SafeWriteString(bw, language, 16);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                platform = br.ReadUInt32();
                product = br.ReadUInt32();
                version = br.ReadUInt32();
                device = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                channel = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                country = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                language = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 握手回应
    /// </summary>
    public sealed class CLGTHandAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 1;
        /// <summary>
        /// 0成功 1无法识别的平台 2无法识别的产品 3版本太老需强更 4拒绝访问 5你的IP已被封禁 6你的设备已被封禁
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 当前网关负载
        /// </summary>
        public int payload;
        /// <summary>
        /// 随机数秘钥
        /// </summary>
        public int random_key;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(payload);
            bw.Write(random_key);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                payload = br.ReadInt32();
                random_key = br.ReadInt32();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 网络中断通知
    /// </summary>
    public sealed class CLGTDisconnectNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 2;
        /// <summary>
        /// 0断开通知 1连接超时 2被踢下线 3被挤下线 4网关维护 5平台维护 6游戏维护 7与平台服务器断开连接 8与游戏服务器断开连接 9系统错误 10离线挂机
        /// </summary>
        public byte code;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(code);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                code = br.ReadByte();
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 物品信息结构
    /// </summary>
    public sealed class CLGTItemInfo : INetProtocol
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
    /// 登录平台请求
    /// </summary>
    public sealed class CLGTLoginReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 3;
        /// <summary>
        /// 登录方式 1游客 2手机登录 3QQ 4微信 5Facebook 6GooglePlay 7GameCenter
        /// </summary>
        public byte login_type;
        /// <summary>
        /// 唯一标识串，CA3加密
        /// </summary>
        public string token;  //max:256

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(login_type);
            NetHelper.SafeWriteString(bw, token, 256);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                login_type = br.ReadByte();
                token = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 管理员登录平台请求
    /// </summary>
    public sealed class CLGTAdminLoginReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 4;
        /// <summary>
        /// 目标玩家Id
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
    /// 登录平台应答
    /// </summary>
    public sealed class CLGTLoginAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 5;
        /// <summary>
        /// 0成功 1平台服务器不可用 2账号被封禁 3系统繁忙 4系统错误 5系统暂未开放
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 玩家Id
        /// </summary>
        public int user_id;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;  //max:32
        /// <summary>
        /// 昵称是否修改过 1是 0否
        /// </summary>
        public sbyte nickname_mdf;
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
        /// 绑定手机
        /// </summary>
        public string phone;  //max:20
        /// <summary>
        /// 平台钻石
        /// </summary>
        public Int64 diamond;
        /// <summary>
        /// 平台货币
        /// </summary>
        public Int64 currency;
        /// <summary>
        /// 平台绑定货币
        /// </summary>
        public Int64 bind_currency;
        /// <summary>
        /// 平台积分
        /// </summary>
        public Int64 integral;
        /// <summary>
        /// 数组长度
        /// </summary>
        public int item_len;
        /// <summary>
        /// 物品数组
        /// </summary>
        public CLGTItemInfo[] items;  //max:100
        /// <summary>
        /// 物品数组（最大长度）
        /// </summary>
        public const int items_max_length = 100;
        /// <summary>
        /// 服务器时间戳
        /// </summary>
        public UInt32 server_timestamp;
        /// <summary>
        /// 额外附加参数
        /// </summary>
        public string extra_params;  //max:4096

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            bw.Write(user_id);
            NetHelper.SafeWriteString(bw, nickname, 32);
            bw.Write(nickname_mdf);
            bw.Write(gender);
            bw.Write(head);
            bw.Write(head_frame);
            bw.Write(level);
            bw.Write(level_exp);
            bw.Write(vip_level);
            bw.Write(vip_level_exp);
            NetHelper.SafeWriteString(bw, phone, 20);
            bw.Write(diamond);
            bw.Write(currency);
            bw.Write(bind_currency);
            bw.Write(integral);
            bw.Write(item_len);
            if (item_len > items_max_length)
                throw new Exception($"CLGTLoginAck.items数组长度超过规定限制，期望:100 实际:{item_len}");
            for (int i = 0; i < (int)item_len; i++)
                items[i].toBinary(bw);
            bw.Write(server_timestamp);
            NetHelper.SafeWriteString(bw, extra_params, 4096);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                user_id = br.ReadInt32();
                nickname = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                nickname_mdf = br.ReadSByte();
                gender = br.ReadInt32();
                head = br.ReadInt32();
                head_frame = br.ReadInt32();
                level = br.ReadInt32();
                level_exp = br.ReadInt64();
                vip_level = br.ReadInt32();
                vip_level_exp = br.ReadInt64();
                phone = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
                diamond = br.ReadInt64();
                currency = br.ReadInt64();
                bind_currency = br.ReadInt64();
                integral = br.ReadInt64();
                item_len = br.ReadInt32();
                items = new CLGTItemInfo[(int)item_len];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new CLGTItemInfo();
                    items[i].fromBinary(br);
                }
                server_timestamp = br.ReadUInt32();
                extra_params = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 访问游戏服务请求
    /// </summary>
    public sealed class CLGTAccessServiceReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 6;
        /// <summary>
        /// 服务组Id
        /// </summary>
        public int group_id;
        /// <summary>
        /// 1加入服务 2离开服务
        /// </summary>
        public int action;
        /// <summary>
        /// 精确指定要加入的服务唯一名称，目前用于客户端加入上次未结束的游戏
        /// </summary>
        public string app_id;  //max:64

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(group_id);
            bw.Write(action);
            NetHelper.SafeWriteString(bw, app_id, 64);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                group_id = br.ReadInt32();
                action = br.ReadInt32();
                app_id = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 访问游戏服务应答
    /// </summary>
    public sealed class CLGTAccessServiceAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 7;
        /// <summary>
        /// 0成功 1服务不存在 2拒绝访问
        /// </summary>
        public sbyte errcode;
        /// <summary>
        /// 上次未结束的游戏数据，json格式
        /// </summary>
        public string game_data;  //max:4096

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(errcode);
            NetHelper.SafeWriteString(bw, game_data, 4096);
        }
        public void fromBinary(BinaryReader br)
        {
            try {
                errcode = br.ReadSByte();
                game_data = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            } catch (EndOfStreamException) { }
        }
    }
    /// <summary>
    /// 心跳包，客户端应当每间隔10秒发一个心跳包，证明你还活着
    /// </summary>
    public sealed class CLGTKeepAlive : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public const ushort mid = 1;
        /// <summary>
        /// 协议ID
        /// </summary>
        public const ushort pid = 8;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
        }
        public void fromBinary(BinaryReader br)
        {
        }
    }

    public class ClientGTResponserBase : INetResponser
    {
        public bool processPackage(BinaryReader br, INetReactor reactor, out INetProtocol responseProto)
        {
            responseProto = null;
            if (br.ReadUInt16() != 1)
                return false;

            switch(br.ReadUInt16())
            {
                case 0:
                    responseProto = new CLGTHandReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTHandReq(responseProto as CLGTHandReq);
                    break;
                case 1:
                    responseProto = new CLGTHandAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTHandAck(responseProto as CLGTHandAck);
                    break;
                case 2:
                    responseProto = new CLGTDisconnectNtf();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTDisconnectNtf(responseProto as CLGTDisconnectNtf);
                    break;
                case 3:
                    responseProto = new CLGTLoginReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTLoginReq(responseProto as CLGTLoginReq);
                    break;
                case 4:
                    responseProto = new CLGTAdminLoginReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTAdminLoginReq(responseProto as CLGTAdminLoginReq);
                    break;
                case 5:
                    responseProto = new CLGTLoginAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTLoginAck(responseProto as CLGTLoginAck);
                    break;
                case 6:
                    responseProto = new CLGTAccessServiceReq();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTAccessServiceReq(responseProto as CLGTAccessServiceReq);
                    break;
                case 7:
                    responseProto = new CLGTAccessServiceAck();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTAccessServiceAck(responseProto as CLGTAccessServiceAck);
                    break;
                case 8:
                    responseProto = new CLGTKeepAlive();
                    responseProto.fromBinary(br);
                    reactor?.onRecvMessage(responseProto);
                    onRecv_CLGTKeepAlive(responseProto as CLGTKeepAlive);
                    break;
            }
            return responseProto != null;
        }

        public virtual void onRecv_CLGTHandReq(CLGTHandReq proto) { }
        public virtual void onRecv_CLGTHandAck(CLGTHandAck proto) { }
        public virtual void onRecv_CLGTDisconnectNtf(CLGTDisconnectNtf proto) { }
        public virtual void onRecv_CLGTLoginReq(CLGTLoginReq proto) { }
        public virtual void onRecv_CLGTAdminLoginReq(CLGTAdminLoginReq proto) { }
        public virtual void onRecv_CLGTLoginAck(CLGTLoginAck proto) { }
        public virtual void onRecv_CLGTAccessServiceReq(CLGTAccessServiceReq proto) { }
        public virtual void onRecv_CLGTAccessServiceAck(CLGTAccessServiceAck proto) { }
        public virtual void onRecv_CLGTKeepAlive(CLGTKeepAlive proto) { }
    }
}
