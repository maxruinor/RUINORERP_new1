using MessagePack;
using System;

namespace TestSerialization
{
    /// <summary>
    /// 修复Key编号连续性的PacketModel测试版本
    /// </summary>
    [MessagePackObject]
    public class FixedPacketModel
    {
        /// <summary>
        /// 保存指令实体数据 - Key(100)
        /// </summary>
        [Key(100)]
        public byte[] CommandData { get; set; }

        /// <summary>
        /// 简单的命令标识 - Key(101)
        /// </summary>
        [Key(101)]
        public string CommandId { get; set; }

        /// <summary>
        /// 数据包状态 - Key(102)
        /// </summary>
        [Key(102)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// 包标志位 - Key(103)
        /// </summary>
        [Key(103)]
        public string Flag { get; set; } = "NORMAL";

        /// <summary>
        /// 数据包唯一标识符 - Key(104)
        /// </summary>
        [Key(104)]
        public string PacketId { get; set; }

        /// <summary>
        /// 数据包大小（字节）- Key(105)
        /// </summary>
        [Key(105)]
        public int Size { get; set; }

        /// <summary>
        /// 校验和 - Key(106)
        /// </summary>
        [Key(106)]
        public string Checksum { get; set; }

        /// <summary>
        /// 是否加密 - Key(107)
        /// </summary>
        [Key(107)]
        public bool IsEncrypted { get; set; } = false;

        /// <summary>
        /// 是否压缩 - Key(108)
        /// </summary>
        [Key(108)]
        public bool IsCompressed { get; set; } = false;

        /// <summary>
        /// 数据包方向 - Key(109)
        /// </summary>
        [Key(109)]
        public string Direction { get; set; } = "Request";

        /// <summary>
        /// 模型版本 - Key(110)
        /// </summary>
        [Key(110)]
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 消息类型 - Key(111)
        /// </summary>
        [Key(111)]
        public string MessageType { get; set; } = "Request";

        /// <summary>
        /// 创建时间 - Key(112)
        /// </summary>
        [Key(112)]
        public DateTime CreatedTimeUtc { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FixedPacketModel()
        {
            PacketId = Guid.NewGuid().ToString();
        }
    }
}