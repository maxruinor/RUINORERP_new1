using System.ComponentModel;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存相关命令
    /// 
    /// 业务场景说明：
    /// 1. 服务器启动时查询不常变化的基础数据表并缓存
    /// 2. 客户端请求需要的数据，从服务器获取后缓存在本地
    /// 3. 数据变化由客户端用户操作决定，通过Socket通信保持同步
    /// </summary>
    public static class CacheCommands
    {
        #region 缓存命令 (0x02xx)
        /// <summary>
        /// 缓存更新/刷新 - 更新服务器缓存数据并通知客户端
        /// </summary>
        public static readonly CommandId CacheUpdate = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheUpdate & 0xFF));

        /// <summary>
        /// 缓存清理 - 清理服务器或客户端的缓存数据
        /// </summary>
        public static readonly CommandId CacheClear = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheClear & 0xFF));

        /// <summary>
        /// 缓存统计信息 - 获取缓存统计信息
        /// </summary>
        public static readonly CommandId CacheStatistics = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheStats & 0xFF));

        /// <summary>
        /// 请求缓存 - 客户端向服务器请求特定缓存数据
        /// </summary>
        public static readonly CommandId CacheRequest = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheRequest & 0xFF));

        /// <summary>
        /// 发送缓存数据 - 服务器向客户端发送请求的缓存数据
        /// </summary>
        public static readonly CommandId CacheDataSend = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheDataSend & 0xFF));

        /// <summary>
        /// 删除缓存 - 删除指定的缓存项
        /// </summary>
        public static readonly CommandId CacheDelete = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheDelete & 0xFF));

        /// <summary>
        /// 缓存数据列表 - 获取缓存数据列表信息
        /// </summary>
        public static readonly CommandId CacheDataList = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheDataList & 0xFF));

        /// <summary>
        /// 缓存状态查询 - 查询缓存状态信息
        /// </summary>
        public static readonly CommandId CacheStatus = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheStatus & 0xFF));

        /// <summary>
        /// 批量缓存操作 - 执行批量缓存操作
        /// </summary>
        public static readonly CommandId CacheBatchOperation = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheBatchOperation & 0xFF));

        /// <summary>
        /// 缓存预热 - 服务器启动时预热缓存数据
        /// </summary>
        public static readonly CommandId CacheWarmup = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheWarmup & 0xFF));

        /// <summary>
        /// 缓存失效 - 使缓存数据失效
        /// </summary>
        public static readonly CommandId CacheInvalidate = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheInvalidate & 0xFF));
        
        /// <summary>
        /// 缓存订阅 - 订阅缓存变更通知
        /// </summary>
        public static readonly CommandId CacheSubscribe = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheSubscribe & 0xFF));

        /// <summary>
        /// 缓存取消订阅 - 取消订阅缓存变更通知
        /// </summary>
        public static readonly CommandId CacheUnsubscribe = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheUnsubscribe & 0xFF));
        #endregion
    }
}
