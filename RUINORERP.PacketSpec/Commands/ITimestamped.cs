using System;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 时间戳接口 - 定义所有需要时间戳管理的实体的统一契约
    /// </summary>
    public interface ITimestamped
    {
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        DateTime CreatedTimeUtc { get; set; }

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        void UpdateTimestamp();
    }
}