using System;
using RUINORERP.PacketSpec.Security;

namespace RUINORERP.PacketSpec.Protocol
{
    /// <summary>
    /// 网络传输层 - 原始数据包结构
    /// 轻量级结构体，专用于网络数据传输，保持高性能
    /// </summary>
    public struct OriginalData
    {
        /// <summary>
        /// 命令字节（支持256个基础命令）
        /// </summary>
        public byte Cmd;
        
        /// <summary>
        /// 第一数据段
        /// </summary>
        public byte[] One;
        
        /// <summary>
        /// 第二数据段（子命令或扩展数据）
        /// </summary>
        public byte[] Two;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cmd">命令字节</param>
        /// <param name="one">第一数据段</param>
        /// <param name="two">第二数据段</param>
        public OriginalData(byte cmd, byte[] one, byte[] two)
        {
            Cmd = cmd;
            One = one ?? Array.Empty<byte>();
            Two = two ?? Array.Empty<byte>();
        }
        
        /// <summary>
        /// 获取数据包总长度
        /// </summary>
        public readonly int Length => 1 + (One?.Length ?? 0) + (Two?.Length ?? 0);
        
        /// <summary>
        /// 检查数据包是否有效
        /// </summary>
        public readonly bool IsValid => Cmd != 0;
        
        /// <summary>
        /// 创建空数据包
        /// </summary>
        public static OriginalData Empty => new OriginalData(0, Array.Empty<byte>(), Array.Empty<byte>());
        
        
    }

    /// <summary>
    /// 网络传输层 - 加密后的数据包结构
    /// 用于在网络中传输已加密的数据
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
