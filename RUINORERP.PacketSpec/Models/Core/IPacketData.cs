using RUINORERP.PacketSpec.Enums.Core;
using System;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 数据包接口 - 定义数据包的基本结构
    /// </summary>
    public interface IPacketData
    {
        // 传输数据
        byte[] CommandData { get; set; }

        /// <summary>
        /// 包标志位
        /// </summary>
        string Flag { get; set; }

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
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        bool IsValid();

        /// <summary>
        /// 更新时间戳
        /// </summary>
        void UpdateTimestamp();

        /// <summary>
        /// 会话ID
        /// </summary>
        string SessionId { get; set; }

        ///// <summary>
        ///// 获取包大小
        ///// </summary>
       int GetPackageSize();

        ///// <summary>
        ///// 安全清理敏感数据
        ///// </summary>
        void ClearSensitiveData();
    }

}
