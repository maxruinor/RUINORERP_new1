using System.ComponentModel;

namespace RUINORERP.PacketSpec.Enums.DataSync
{
    /// <summary>
    /// 数据同步命令枚举
    /// </summary>
    public enum DataSyncCommand : uint
    {
        /// <summary>
        /// 数据同步请求
        /// </summary>
        [Description("数据同步请求")]
        DataSync = 0x0700,

        /// <summary>
        /// 实体同步
        /// </summary>
        [Description("实体同步")]
        EntitySync = 0x0701,

        /// <summary>
        /// 增量同步
        /// </summary>
        [Description("增量同步")]
        IncrementalSync = 0x0702,

        /// <summary>
        /// 同步状态查询
        /// </summary>
        [Description("同步状态查询")]
        SyncStatus = 0x0703,

        /// <summary>
        /// 数据同步
        /// </summary>
        [Description("数据同步")]
        DataSyncRequest = 0x0704,

        /// <summary>
        /// 实体数据传输
        /// </summary>
        [Description("实体数据传输")]
        EntityDataTransfer = 0x0705,

        /// <summary>
        /// 更新动态配置
        /// </summary>
        [Description("更新动态配置")]
        UpdateDynamicConfig = 0x0706,

        /// <summary>
        /// 转发更新动态配置
        /// </summary>
        [Description("转发更新动态配置")]
        ForwardUpdateDynamicConfig = 0x0707,

        /// <summary>
        /// 更新全局配置
        /// </summary>
        [Description("更新全局配置")]
        UpdateGlobalConfig = 0x0708
    }
}