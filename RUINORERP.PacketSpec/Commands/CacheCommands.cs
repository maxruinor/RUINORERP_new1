using System.ComponentModel;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 缓存相关命令
    /// </summary>
    public static class CacheCommands
    {
        #region 缓存命令 (0x02xx)
        /// <summary>
        /// 缓存更新
        /// </summary>
        public static readonly CommandId CacheUpdate = new CommandId(CommandCategory.Cache, 0x00);
        
        /// <summary>
        /// 缓存清理
        /// </summary>
        public static readonly CommandId CacheClear = new CommandId(CommandCategory.Cache, 0x01);
        
        /// <summary>
        /// 缓存统计
        /// </summary>
        public static readonly CommandId CacheStats = new CommandId(CommandCategory.Cache, 0x02);
        
        /// <summary>
        /// 请求缓存
        /// </summary>
        public static readonly CommandId CacheRequest = new CommandId(CommandCategory.Cache, 0x03);
        
        /// <summary>
        /// 发送缓存数据
        /// </summary>
        public static readonly CommandId CacheDataSend = new CommandId(CommandCategory.Cache, 0x04);
        
        /// <summary>
        /// 删除缓存
        /// </summary>
        public static readonly CommandId CacheDelete = new CommandId(CommandCategory.Cache, 0x05);
        
        /// <summary>
        /// 缓存数据列表
        /// </summary>
        public static readonly CommandId CacheDataList = new CommandId(CommandCategory.Cache, 0x06);
        
        /// <summary>
        /// 缓存信息列表
        /// </summary>
        public static readonly CommandId CacheInfoList = new CommandId(CommandCategory.Cache, 0x07);
        
        /// <summary>
        /// 缓存状态查询 - 查询缓存状态信息
        /// </summary>
        public static readonly CommandId CacheStatus = new CommandId(CommandCategory.Cache, 0x04);
        
        /// <summary>
        /// 批量缓存操作 - 执行批量缓存操作
        /// </summary>
        public static readonly CommandId CacheBatchOperation = new CommandId(CommandCategory.Cache, 0x05);
        
        /// <summary>
        /// 缓存同步 - 同步缓存数据
        /// </summary>
        public static readonly CommandId CacheSync = new CommandId(CommandCategory.Cache, 0x06);
        
        /// <summary>
        /// 缓存预热 - 预热缓存数据
        /// </summary>
        public static readonly CommandId CacheWarmup = new CommandId(CommandCategory.Cache, 0x07);
        
        /// <summary>
        /// 缓存失效 - 使缓存数据失效
        /// </summary>
        public static readonly CommandId CacheInvalidate = new CommandId(CommandCategory.Cache, 0x08);
        
        /// <summary>
        /// 缓存统计信息 - 获取缓存统计信息
        /// </summary>
        public static readonly CommandId CacheStatistics = new CommandId(CommandCategory.Cache, 0x09);
        #endregion
    }
}