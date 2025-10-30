using System.ComponentModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 缓存相关命令 - 简化版
    /// 
    /// 业务场景说明：
    /// 1. 服务器启动时查询不常变化的基础数据表并缓存
    /// 2. 客户端请求需要的数据，从服务器获取后缓存在本地
    /// 3. 数据变化由客户端用户操作决定，通过Socket通信保持同步
    /// </summary>
    public static class CacheCommands
    {
        #region 核心缓存命令 (0x02xx)
        /// <summary>
        /// 缓存操作 - 统一的缓存操作命令
        /// 通过CacheRequest.Operation区分具体操作类型（Get/Set/Update/Remove/Clear/BatchUpdate）
        /// </summary>
        public static readonly CommandId CacheOperation = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheOperation & 0xFF), "CacheOperation");

        /// <summary>
        /// 缓存同步 - 用于缓存数据的双向同步
        /// 服务器向客户端推送缓存变更，客户端向服务器报告本地变更 通过这个来处理是不中同步到其它用户
        /// </summary>
        public static readonly CommandId CacheSync = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheSync & 0xFF), "CacheSync");
        
        /// <summary>
        /// 缓存订阅管理 - 用于缓存订阅和取消订阅操作
        /// 通过CacheRequest的SubscribeAction属性区分订阅/取消订阅
        /// </summary>
        public static readonly CommandId CacheSubscription = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheSubscription & 0xFF), "CacheSubscription");
        #endregion
    }
}
