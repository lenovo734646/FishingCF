using System.IO;

namespace JBPROTO
{
    public interface INetProtocol
    {
        void toBinary(BinaryWriter bw);

        void fromBinary(BinaryReader br);
    }
}
