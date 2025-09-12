using System;

namespace RUINORERP.PacketSpec.Protocol
{
    /// <summary>
    /// 原始数据包结构（加密前）
    /// </summary>
    public struct OriginalData
    {
        public byte Cmd;
        public byte[] One;
        public byte[] Two;
        
        public OriginalData(byte cmd, byte[] one, byte[] two)
        {
            Cmd = cmd;
            One = one;
            Two = two;
        }
    }

    /// <summary>
    /// 加密后的数据包结构
    /// </summary>
    public struct EncryptedData
    {
        public byte[] Head;
        public byte[] One;
        public byte[] Two;
        
        public EncryptedData(byte[] head, byte[] one, byte[] two)
        {
            Head = head;
            One = one;
            Two = two;
        }
    }
}