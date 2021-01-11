using System;
using System.Collections.Generic;
using System.IO;

public class NetBinaryReaderProxy
{
    public ushort ModuleId { get; }
    public ushort ProtocolId { get; }
    private BinaryReader br_;

    public NetBinaryReaderProxy(BinaryReader br)
    {
        br_ = br;
        br_.BaseStream.Position = 0;
        ModuleId = br_.ReadUInt16();
        ProtocolId = br_.ReadUInt16();
    }

    public sbyte ReadInt8()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadSByte();
    }

    public byte ReadUInt8()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadByte();
    }

    public short ReadInt16()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadInt16();
    }

    public ushort ReadUInt16()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadUInt16();
    }

    public int ReadInt32()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadInt32();
    }

    public uint ReadUInt32()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadUInt32();
    }

    public long ReadInt64()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadInt64();
    }
    
    public ulong ReadUInt64()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return 0;

        return br_.ReadUInt64();
    }

    public string ReadString()
    {
        if (br_.BaseStream.Position == br_.BaseStream.Length)
            return string.Empty;

        return System.Text.Encoding.UTF8.GetString(br_.ReadBytes(br_.ReadUInt16()));
    }
}
