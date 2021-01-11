using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;

namespace JBPROTO
{
    enum NetConnectState
    {
        None = 0,
        Connecting,
        Established,
    }

    class NetData
    {
        public NetConnectState state;
        public Socket socket;
        public byte[] recvBuffer;
        public MemoryStream msgStream;
        public bool isConnectError;
        public bool isDisconnected;
        public bool isEstablished;
        public List<KeyValuePair<Type, Action<INetProtocol>>> responseQueue;

        public List<byte[]> cacheProtocolDataList;
        public int cacheProtocalValidLength;

        private object netDataLock_;
        private byte[] netDataBuffer_;
        private int netDataBufferLength_;

        public NetData()
        {
            state = NetConnectState.None;
            socket = null;
            recvBuffer = null;
            msgStream = null;
            isConnectError = false;
            isDisconnected = false;
            isEstablished = false;
            responseQueue = new List<KeyValuePair<Type, Action<INetProtocol>>>();

            cacheProtocolDataList = new List<byte[]>();
            cacheProtocalValidLength = 0;

            netDataLock_ = new object();
            netDataBuffer_ = new byte[4096];
            netDataBufferLength_ = 0;
        }

        public void dispatchResponseQueueWithNull()
        {
            if (responseQueue.Count > 0)
            {
                foreach (var p in responseQueue)
                {
                    try
                    {
                        p.Value(null);
                    }
                    catch (System.Exception ex)
                    {
                        UnityEngine.Debug.LogError($"一般情况下这个报错可以忽略。监听的ack对象是可能为空的，这里注意一下：\n{ex.ToString()}");
                    }
                }
                responseQueue.Clear();
            }
        }

        public void dispatchResponse(INetProtocol responseProto)
        {
            if (responseProto != null && responseQueue.Count > 0)
            {
                var t = responseProto.GetType();
                for (int i = 0; i < responseQueue.Count; ++i)
                {
                    var p = responseQueue[i];
                    if (t == p.Key)
                    {
                        responseQueue.RemoveAt(i);
                        p.Value(responseProto);
                        break;
                    }
                }
            }
        }

        public void clearWaitingResponse<T>() where T : class, INetProtocol
        {
            responseQueue.RemoveAll(a => a.Key == typeof(T));
        }

        //接受完整的协议包数据
        public void acceptProtocolData(byte[] buffer, int length)
        {
            if (cacheProtocolDataList.Count <= cacheProtocalValidLength)
                cacheProtocolDataList.Add(new byte[Math.Max(1024, length)]);
            byte[] arr = cacheProtocolDataList[cacheProtocalValidLength++];
            if (arr.Length < length)
            {
                arr = new byte[length];
                cacheProtocolDataList[cacheProtocalValidLength - 1] = arr;
            }
            Array.Copy(buffer, arr, length);
        }

        //派发缓存的协议包数据
        public void dispatchProtocols(INetReactor reactor, List<INetResponser> responsers)
        {
            while (cacheProtocalValidLength > 0)
            {
                byte[] arr = cacheProtocolDataList[0];
                cacheProtocolDataList.RemoveAt(0);
                cacheProtocalValidLength--;

                var sharedStream = new MemoryStream(arr, 4, arr.Length - 4);
                var br = new BinaryReader(sharedStream);
                INetProtocol responseProto = null;
                foreach (var response in responsers)
                {
                    sharedStream.Seek(0, SeekOrigin.Begin);
                    if (response.processPackage(br, reactor, out responseProto))
                    {
                        dispatchResponse(responseProto);
                        break;
                    }
                }

                MessageCenter.Instance.SendMessage(MsgType.NET_RECEIVE_DATA, this, br);

                cacheProtocolDataList.Add(arr);
            }
        }

        //接受网络原始数据
        public void acceptNetData(byte[] buffer, int length)
        {
            lock (netDataLock_)
            {
                if (netDataBuffer_.Length < length + netDataBufferLength_)
                {
                    int l = netDataBuffer_.Length;
                    while (l < length + netDataBufferLength_)
                        l *= 2;

                    byte[] tmpBuffer = new byte[l];
                    Array.Copy(netDataBuffer_, tmpBuffer, netDataBufferLength_);
                    netDataBuffer_ = tmpBuffer;
                }
                Array.Copy(buffer, 0, netDataBuffer_, netDataBufferLength_, length);
                netDataBufferLength_ += length;
            }
        }

        //拆分网络数据，组装协议包
        public void splitNetDataToProtocols()
        {
            lock (netDataLock_)
            {
                while (true)
                {
                    if (netDataBufferLength_ < 4)
                        break;
                    
                    int len = BitConverter.ToInt32(netDataBuffer_, 0);
                    if (netDataBufferLength_ < len)
                        break;

                    if (len == 0)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        for (int i = 0; i < Math.Min(netDataBufferLength_, 100); ++i)
                        {
                            byte b = netDataBuffer_[i];
                            sb.Append(b.ToString("X"));
                            sb.Append(' ');
                        }
                        throw new Exception($"len==0 ms.Length={netDataBufferLength_} content={sb.ToString()}");
                    }

                    acceptProtocolData(netDataBuffer_, len);
                    if (len < netDataBufferLength_)
                    {
                        int count = netDataBufferLength_ - len;
                        for (int i = 0; i < count; i++)
                            netDataBuffer_[i] = netDataBuffer_[len + i];
                    }
                    netDataBufferLength_ -= len;
                }
            }
        }
    }

    class NetComponentImpl : INetComponent
    {
        private INetReactor reactor_;
        private List<INetResponser> responsers_;
        private NetData netData_;   //attention: netData_的赋值必须发生在主线程

        public NetComponentImpl(INetReactor reactor)
        {
            reactor_ = reactor;
            responsers_ = new List<INetResponser>();
            netData_ = null;
        }

        public void addResponser(INetResponser responser)
        {
            responsers_.Add(responser);
        }

        public void connectWithTimeout(string ip, int port, int timeoutInMillionSeconds)
        {
            disconnect();

            var addr = IPAddress.Parse(ip);
            var endPoint = new IPEndPoint(addr, port);

            var data = new NetData();
            data.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            data.recvBuffer = new byte[4096];
            data.msgStream = new MemoryStream();

            data.state = NetConnectState.Connecting;
            data.socket.BeginConnect(endPoint, new AsyncCallback(_beginConnectCallback), data);
            netData_ = data;

            if (timeoutInMillionSeconds > 0)
                checkConnectWithTimeout(data, timeoutInMillionSeconds);
        }

        private async void checkConnectWithTimeout(NetData data, int timeoutInMillionSeconds)
        {
            await Task.Delay(timeoutInMillionSeconds);

            if (data.state == NetConnectState.Connecting)
            {
                data.isConnectError = true;
            }
        }

        public void disconnect()
        {
            var data = netData_;
            if (data != null)
            {
                netData_ = null;
                if (data.state == NetConnectState.Established)
                {
                    data.dispatchResponseQueueWithNull();
                    try
                    {
                        netData_.socket.Close();
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("net disconnect error: {0}", ex.Message);
                    }
                }
            }
        }

        private MemoryStream sendMemoryStream_ = new MemoryStream();
        public void send(INetProtocol proto)
        {
            var data = netData_;
            if (data != null && data.state == NetConnectState.Established)
            {
                sendMemoryStream_.Seek(0, SeekOrigin.Begin);
                sendMemoryStream_.SetLength(0);

                var bw = new BinaryWriter(sendMemoryStream_);
                bw.Write((int)0);
                proto.toBinary(bw);

                var len = (int)sendMemoryStream_.Length;
                bw.Seek(0, SeekOrigin.Begin);
                bw.Write(len);
                
                try
                {
                    data.socket.Send(sendMemoryStream_.GetBuffer(), len, SocketFlags.None);
                    if (reactor_ != null)
                        reactor_.onSendMessage(proto);
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError($"一般情况下这个报错可以忽略。网络可能已断开，套接字无效了。\n{ex.ToString()}");
                }
            }
        }

        public void send(MemoryStream ms)
        {
            var data = netData_;
            if (data != null && data.state == NetConnectState.Established)
            {
                try
                {
                    data.socket.Send(ms.GetBuffer(), (int)ms.Length, SocketFlags.None);
                }
                catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError($"一般情况下这个报错可以忽略。网络可能已断开，套接字无效了。\n{ex.ToString()}");
                }
            }
        }

        public void asyncRequest<T>(INetProtocol proto, Action<T> callback) where T : class, INetProtocol
        {
            var data = netData_;
            if (data != null && data.state == NetConnectState.Established)
            {
                send(proto);
                var action = new Action<INetProtocol>((_p) => callback(_p as T));
                data.responseQueue.Add(new KeyValuePair<Type, Action<INetProtocol>>(typeof(T), action));
            }
        }

        public void asyncRequestWithLock<T>(INetProtocol proto, Action<T> callback) where T : class, INetProtocol
        {
            //ModuleManager.Instance.Get<CommonModule>().WaitLockCount++;
            asyncRequest<T>(proto, ack =>
            {
                //ModuleManager.Instance.Get<CommonModule>().WaitLockCount--;
                callback(ack);
            });
        }

        public void clearWaitingResponse<T>() where T : class, INetProtocol
        {
            var data = netData_;
            data.clearWaitingResponse<T>();
        }

        public void run()
        {
            var data = netData_;
            if (data != null)
            {
                if (data.isConnectError)
                {
                    netData_ = null;
                    if (reactor_ != null)
                        reactor_.onNetConnectError();
                    return;
                }

                if (data.isDisconnected)
                {
                    netData_ = null;
                    data.dispatchResponseQueueWithNull();
                    if (reactor_ != null)
                        reactor_.onNetDisconnect();
                    return;
                }

                switch (data.state)
                {
                    case NetConnectState.Connecting:
                        if (data.isEstablished)
                        {
                            data.isEstablished = false;
                            data.state = NetConnectState.Established;
                            if (reactor_ != null)
                                reactor_.onNetConnectEstablished();
                        }
                        break;

                    case NetConnectState.Established:
                        data.splitNetDataToProtocols();
                        break;
                }

                //派发协议
                data.dispatchProtocols(reactor_, responsers_);
            }
        }

        private void _beginConnectCallback(IAsyncResult ar)
        {
            var data = ar.AsyncState as NetData;
            if (netData_ == data && !data.isConnectError && !data.isEstablished)
            {
                try
                {
                    data.socket.EndConnect(ar);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("net connect error: {0}", ex.Message);
                    data.isConnectError = true;
                    return;
                }

                data.isEstablished = true;
                _beginReceive(data);
            }
        }

        private void _beginReceive(NetData data)
        {
            if (netData_ == data)
            {
                data.socket.BeginReceive(data.recvBuffer, 0, data.recvBuffer.Length, SocketFlags.None, _beginReceiveCallback, data);
            }
        }

        private void _beginReceiveCallback(IAsyncResult ar)
        {
            var data = ar.AsyncState as NetData;
            if (netData_ == data && !data.isDisconnected)
            {
                try
                {
                    int len = data.socket.EndReceive(ar);
                    if (len > 0)
                    {
                        data.acceptNetData(data.recvBuffer, len);
                    }
                    _beginReceive(data);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("net receive error: {0}", ex.Message);
                    data.isDisconnected = true;
                    return;
                }
            }
        }
    }
}
