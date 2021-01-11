using System.IO;

namespace JBPROTO
{
    public interface INetReactor
    {
        void onSendMessage(INetProtocol proto);

        void onRecvMessage(INetProtocol proto);

        void onNetConnectError();

        void onNetConnectEstablished();

        void onNetDisconnect();
    }
}
