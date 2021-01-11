using System.IO;

namespace JBPROTO
{
    public interface INetResponser
    {
        bool processPackage(BinaryReader br, INetReactor reactor, out INetProtocol responseProto);
    }
}
