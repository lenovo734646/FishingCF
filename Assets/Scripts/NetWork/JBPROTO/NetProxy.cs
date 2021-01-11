using System;
using System.Collections.Generic;
using System.Threading;

namespace JBPROTO
{
    public class NetProxy
    {
        private INetComponent netComponent_;
        private List<Action> actions_;

        public NetProxy(INetReactor reactor)
        {
            netComponent_ = NetHelper.createNetComponent(reactor);
            actions_ = new List<Action>();
        }

        private void _addAction(Action action)
        {
            lock (actions_)
            {
                actions_.Add(action);
            }
        }

        private void _executeActions()
        {
            lock (actions_)
            {
                foreach (var action in actions_)
                {
                    action();
                }
                actions_.Clear();
            }
        }

        public void addResponser(INetResponser responser)
        {
            _addAction(() => netComponent_.addResponser(responser));
        }

        public void connectWithTimeout(string ip, int port, int timeoutInMillionSeconds)
        {
            _addAction(() => netComponent_.connectWithTimeout(ip, port, timeoutInMillionSeconds));
        }

        public void disconnect()
        {
            _addAction(() => netComponent_.disconnect());
        }

        public void send(INetProtocol proto)
        {
            _addAction(() => netComponent_.send(proto));
        }
        
        //该函数只允许其他线程调用，决不能主线程调用！！！
        public void syncRequestByOtherThread<T>(INetProtocol proto, Action<T> callback) where T : class, INetProtocol
        {
            var obj = new object();
            Monitor.Enter(obj);

            T rsp = null;
            Action<T> tcallback = new Action<T>((responseProto) =>
            {
                rsp = responseProto;
                Monitor.Exit(obj);
            });
            asyncRequest(proto, tcallback);

            Monitor.Enter(obj);
            Monitor.Exit(obj);
            callback(rsp);
        }

        public void asyncRequest<T>(INetProtocol proto, Action<T> callback) where T : class, INetProtocol
        {
            _addAction(() => netComponent_.asyncRequest<T>(proto, callback));
        }

        public void run()
        {
            _executeActions();
            netComponent_.run();
        }
    }
}