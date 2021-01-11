using System;
using System.IO;

namespace JBPROTO
{
    /// <summary>
    /// 连接断开通知
    /// </summary>
    public sealed class WDBSDisconnectNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 8;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 0;
        /// <summary>
        /// 0断开通知 1系统维护 2系统错误
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
            code = br.ReadByte();
        }
    }
    /// <summary>
    /// 应用程序数据结构
    /// </summary>
    public sealed class WDBSServerApplicationData : INetProtocol
    {
        /// <summary>
        /// 应用程序唯一Id
        /// </summary>
        public string app_id;  //max:32
        /// <summary>
        /// 服务器类型 1:GATE 2:PLATFORM 3:GAME
        /// </summary>
        public int server_type;
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string server_name;  //max:32
        /// <summary>
        /// 游戏组Id
        /// </summary>
        public int group_id;
        /// <summary>
        /// 游戏子服务Id
        /// </summary>
        public int service_id;
        /// <summary>
        /// 公网IP地址
        /// </summary>
        public string public_ip;  //max:32
        /// <summary>
        /// 内网IP地址
        /// </summary>
        public string private_ip;  //max:32
        /// <summary>
        /// 服务端口号1
        /// </summary>
        public int port1;
        /// <summary>
        /// 服务端口号2
        /// </summary>
        public int port2;
        /// <summary>
        /// 处理的协议模块Id列表，用逗号隔开
        /// </summary>
        public string modules;  //max:256
        /// <summary>
        /// 服务器版本号
        /// </summary>
        public string version;  //max:32
        /// <summary>
        /// 服务器编译时间
        /// </summary>
        public string compile_time;  //max:32
        /// <summary>
        /// 应用程序启动时间
        /// </summary>
        public string startup_time;  //max:32

        public void toBinary(BinaryWriter bw)
        {
            bw.Write((ushort)app_id.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(app_id));
            bw.Write(server_type);
            bw.Write((ushort)server_name.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(server_name));
            bw.Write(group_id);
            bw.Write(service_id);
            bw.Write((ushort)public_ip.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(public_ip));
            bw.Write((ushort)private_ip.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(private_ip));
            bw.Write(port1);
            bw.Write(port2);
            bw.Write((ushort)modules.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(modules));
            bw.Write((ushort)version.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(version));
            bw.Write((ushort)compile_time.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(compile_time));
            bw.Write((ushort)startup_time.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(startup_time));
        }
        public void fromBinary(BinaryReader br)
        {
            app_id = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            server_type = br.ReadInt32();
            server_name = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            group_id = br.ReadInt32();
            service_id = br.ReadInt32();
            public_ip = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            private_ip = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            port1 = br.ReadInt32();
            port2 = br.ReadInt32();
            modules = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            version = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            compile_time = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
            startup_time = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
        }
    }
    /// <summary>
    /// 请求应用程序列表
    /// </summary>
    public sealed class WDBSServerApplicationListReq : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 8;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 1;

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
    /// 应用程序列表回应
    /// </summary>
    public sealed class WDBSServerApplicationListAck : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 8;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 2;
        /// <summary>
        /// 应用数据列表长度
        /// </summary>
        public int applen;
        /// <summary>
        /// 应用程序列表数据
        /// </summary>
        public WDBSServerApplicationData[] apps;  //max:512

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write(applen);
            for (int i = 0; i < (int)applen; i++)
                apps[i].toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            applen = br.ReadInt32();
            apps = new WDBSServerApplicationData[(int)applen];
            for (int i = 0; i < apps.Length; i++)
            {
                apps[i] = new WDBSServerApplicationData();
                apps[i].fromBinary(br);
            }
        }
    }
    /// <summary>
    /// 应用程序接入通知
    /// </summary>
    public sealed class WDBSServerApplicationJoinNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 8;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 3;
        /// <summary>
        /// 应用程序数据
        /// </summary>
        public WDBSServerApplicationData app;

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            app.toBinary(bw);
        }
        public void fromBinary(BinaryReader br)
        {
            app = new WDBSServerApplicationData();
            app.fromBinary(br);
        }
    }
    /// <summary>
    /// 应用程序退出通知
    /// </summary>
    public sealed class WDBSServerApplicationExitNtf : INetProtocol
    {
        /// <summary>
        /// 协议模块ID
        /// </summary>
        public ushort mid = 8;
        /// <summary>
        /// 协议ID
        /// </summary>
        public ushort pid = 4;
        /// <summary>
        /// 应用程序唯一Id
        /// </summary>
        public string app_id;  //max:32

        public void toBinary(BinaryWriter bw)
        {
            bw.Write(mid);
            bw.Write(pid);
            bw.Write((ushort)app_id.Length);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(app_id));
        }
        public void fromBinary(BinaryReader br)
        {
            app_id = System.Text.Encoding.UTF8.GetString(br.ReadBytes(br.ReadUInt16()));
        }
    }

    public class WorldBSResponserBase : INetResponser
    {
        public bool processPackage(BinaryReader br, out INetProtocol responseProto)
        {
            responseProto = null;
            if (br.ReadUInt16() != 8)
                return false;

            switch(br.ReadUInt16())
            {
                case 0:
                    responseProto = new WDBSDisconnectNtf();
                    responseProto.fromBinary(br);
                    onRecv_WDBSDisconnectNtf(responseProto as WDBSDisconnectNtf);
                    break;
                case 1:
                    responseProto = new WDBSServerApplicationListReq();
                    responseProto.fromBinary(br);
                    onRecv_WDBSServerApplicationListReq(responseProto as WDBSServerApplicationListReq);
                    break;
                case 2:
                    responseProto = new WDBSServerApplicationListAck();
                    responseProto.fromBinary(br);
                    onRecv_WDBSServerApplicationListAck(responseProto as WDBSServerApplicationListAck);
                    break;
                case 3:
                    responseProto = new WDBSServerApplicationJoinNtf();
                    responseProto.fromBinary(br);
                    onRecv_WDBSServerApplicationJoinNtf(responseProto as WDBSServerApplicationJoinNtf);
                    break;
                case 4:
                    responseProto = new WDBSServerApplicationExitNtf();
                    responseProto.fromBinary(br);
                    onRecv_WDBSServerApplicationExitNtf(responseProto as WDBSServerApplicationExitNtf);
                    break;
            }
            return responseProto != null;
        }

        public virtual void onRecv_WDBSDisconnectNtf(WDBSDisconnectNtf proto) { }
        public virtual void onRecv_WDBSServerApplicationListReq(WDBSServerApplicationListReq proto) { }
        public virtual void onRecv_WDBSServerApplicationListAck(WDBSServerApplicationListAck proto) { }
        public virtual void onRecv_WDBSServerApplicationJoinNtf(WDBSServerApplicationJoinNtf proto) { }
        public virtual void onRecv_WDBSServerApplicationExitNtf(WDBSServerApplicationExitNtf proto) { }

        public bool processPackage(BinaryReader br, INetReactor reactor, out INetProtocol responseProto)
        {
            throw new NotImplementedException();
        }
    }
}
