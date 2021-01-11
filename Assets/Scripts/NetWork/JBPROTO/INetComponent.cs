using System;
using System.IO;

namespace JBPROTO
{
    public interface INetComponent
    {
        void addResponser(INetResponser responser);

        void connectWithTimeout(string ip, int port, int timeoutInMillionSeconds);

        void disconnect();

        void send(INetProtocol proto);

        void send(MemoryStream ms);

        void asyncRequest<T>(INetProtocol proto, Action<T> callback) where T : class, INetProtocol;

        void asyncRequestWithLock<T>(INetProtocol proto, Action<T> callback) where T : class, INetProtocol;

        void clearWaitingResponse<T>() where T : class, INetProtocol;

        void run();
    }
}
