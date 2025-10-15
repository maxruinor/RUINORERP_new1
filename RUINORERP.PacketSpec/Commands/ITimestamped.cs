using System;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 时间戳接口 - 定义所有需要时间戳管理的实体的统一契约
    /// </summary>
    public interface ITimestamped
    {
        /// <summary>
        /// 创建时间用于生命周期管理
        /// </summary>
        DateTime CreatedTime { get; set; }

        /// <summary>
        /// 时间戳用于状态跟踪和缓存失效
        /// </summary>
        DateTime TimestampUtc { get; set; }

        /// <summary>
        /// 更新时间戳
        /// </summary>
        void UpdateTimestamp();
    }
}
