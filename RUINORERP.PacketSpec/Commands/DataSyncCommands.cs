using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 数据同步相关命令
    /// </summary>
    public static class DataSyncCommands
    {
        #region 数据同步命令 (0x07xx)
        /// <summary>
        /// 数据请求 - 请求同步数据
        /// </summary>
        public static readonly CommandId DataRequest = new CommandId(CommandCategory.DataSync, 0x00);
        
        /// <summary>
        /// 全量同步 - 执行全量数据同步
        /// </summary>
        public static readonly CommandId FullSync = new CommandId(CommandCategory.DataSync, 0x01);
        
        /// <summary>
        /// 增量同步 - 执行增量数据同步
        /// </summary>
        public static readonly CommandId IncrementalSync = new CommandId(CommandCategory.DataSync, 0x02);
        
        /// <summary>
        /// 同步状态查询 - 查询数据同步状态
        /// </summary>
        public static readonly CommandId SyncStatus = new CommandId(CommandCategory.DataSync, 0x03);
        
        /// <summary>
        /// 数据同步 - 执行数据同步操作
        /// </summary>
        public static readonly CommandId DataSyncRequest = new CommandId(CommandCategory.DataSync, 0x04);
        
        /// <summary>
        /// 实体数据传输 - 传输业务实体数据
        /// </summary>
        public static readonly CommandId EntityDataTransfer = new CommandId(CommandCategory.DataSync, 0x05);
        
        /// <summary>
        /// 更新动态配置 - 更新系统动态配置
        /// </summary>
        public static readonly CommandId UpdateDynamicConfig = new CommandId(CommandCategory.DataSync, 0x06);
        
        /// <summary>
        /// 转发更新动态配置 - 转发动态配置更新请求
        /// </summary>
        public static readonly CommandId ForwardUpdateDynamicConfig = new CommandId(CommandCategory.DataSync, 0x07);
        
        /// <summary>
        /// 更新全局配置 - 更新系统全局配置
        /// </summary>
        public static readonly CommandId UpdateGlobalConfig = new CommandId(CommandCategory.DataSync, 0x08);
        #endregion
    }
}