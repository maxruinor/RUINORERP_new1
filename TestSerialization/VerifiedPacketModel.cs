using MessagePack;
using System;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;

namespace TestSerialization
{
    /// <summary>
    /// 完全修复的PacketModel - 用于验证Key编号问题
    /// </summary>
    [MessagePackObject]
    public class VerifiedPacketModel
    {
        /// <summary>
        /// 保存指令实体数据 - Key(0)
        /// </summary>
        [Key(0)]
        public byte[] CommandData { get; set; }

        /// <summary>
        /// 命令类型 - Key(1)
        /// </summary>
        [Key(1)]
        public CommandId CommandId { get; set; } = new CommandId();

        /// <summary>
        /// 数据包状态 - Key(2)
        /// </summary>
        [Key(2)]
        public PacketStatus Status { get; set; }

        /// <summary>
        /// 包标志位 - Key(3)
        /// </summary>
        [Key(3)]
        public string Flag { get; set; } = "NORMAL";

        /// <summary>
        /// 数据包唯一标识符 - Key(4)
        /// </summary>
        [Key(4)]
        public string PacketId { get; set; }

        /// <summary>
        /// 数据包大小（字节）- Key(5)
        /// </summary>
        [Key(5)]
        public int Size { get; set; }

        /// <summary>
        /// 校验和 - Key(6)
        /// </summary>
        [Key(6)]
        public string Checksum { get; set; } = string.Empty;

        /// <summary>
        /// 是否加密 - Key(7)
        /// </summary>
        [Key(7)]
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// 是否压缩 - Key(8)
        /// </summary>
        [Key(8)]
        public bool IsCompressed { get; set; }

        /// <summary>
        /// 数据包方向 - Key(9)
        /// </summary>
        [Key(9)]
        public PacketDirection Direction { get; set; } = PacketDirection.Request;

        /// <summary>
        /// 模型版本 - Key(10)
        /// </summary>
        [Key(10)]
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 消息类型 - Key(11)
        /// </summary>
        [Key(11)]
        public MessageType MessageType { get; set; } = MessageType.Request;

        /// <summary>
        /// 创建时间（UTC时间）- Key(12)
        /// </summary>
        [Key(12)]
        public DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// 最后更新时间（UTC时间）- Key(13)
        /// </summary>
        [Key(13)]
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）- Key(14)
        /// </summary>
        [Key(14)]
        public DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public VerifiedPacketModel()
        {
            PacketId = Guid.NewGuid().ToString();
            CreatedTimeUtc = DateTime.UtcNow;
            TimestampUtc = CreatedTimeUtc;
            LastUpdatedTime = null;
        }
    }
}