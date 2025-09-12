using System;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 原始数据包结构 - 用于网络传输
    /// </summary>
    public class OriginalData
    {
        /// <summary>
        /// 主指令
        /// </summary>
        public uint Cmd { get; set; }

        /// <summary>
        /// 第一个数据段
        /// </summary>
        public byte[] One { get; set; }

        /// <summary>
        /// 第二个数据段（子指令或扩展数据）
        /// </summary>
        public byte[] Two { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 数据包长度
        /// </summary>
        public int Length => (One?.Length ?? 0) + (Two?.Length ?? 0) + 4; // Cmd占4字节

        /// <summary>
        /// 创建新的原始数据包
        /// </summary>
        public static OriginalData Create(uint command, byte[] dataOne = null, byte[] dataTwo = null)
        {
            return new OriginalData
            {
                Cmd = command,
                One = dataOne ?? Array.Empty<byte>(),
                Two = dataTwo ?? Array.Empty<byte>(),
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 验证数据包有效性
        /// </summary>
        public bool IsValid()
        {
            // 基本验证规则
            if (Cmd == 0) return false;
            if (One == null) One = Array.Empty<byte>();
            if (Two == null) Two = Array.Empty<byte>();
            
            // 数据长度验证（可根据实际协议调整）
            if (One.Length > 1024 * 1024) return false; // 1MB限制
            if (Two.Length > 1024 * 1024) return false; // 1MB限制

            return true;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            One = Array.Empty<byte>();
            Two = Array.Empty<byte>();
        }
    }
}