using System.ComponentModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 缓存相关命令 - 简化版
    /// 
    /// 【设计思路】采用"轻量元数据协调 + 重量数据传输"的分层同步策略
    /// 
    /// 业务场景说明：
    /// 1. 服务器启动时查询不常变化的基础数据表并缓存
    /// 2. 客户端请求需要的数据，从服务器获取后缓存在本地
    /// 3. 数据变化由客户端用户操作决定，通过Socket通信保持同步
    /// 
    /// 【为什么需要 CacheSync 和 CacheMetadataSync 两套命令？】
    /// - CacheMetadataSync (0x0204): 传输元数据（几KB），用于登录时快速比对版本，决策需要同步哪些表
    /// - CacheSync (0x0202): 传输实际数据（MB级），用于实时同步和变更推送
    /// - 优势：避免盲目全量同步，实现智能按需加载，提升登录速度和用户体验
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
        /// 
        /// 【职责】：传输实际的缓存数据内容（实体列表）
        /// 【使用场景】：
        ///   1. 客户端上报本地数据变更（增删改）
        ///   2. 服务器推送数据变更给其他订阅的客户端
        ///   3. 客户端请求获取某个表的完整数据列表
        /// 【特点】：数据量大（MB级）、频率高（实时）、关注数据内容本身
        /// 【类比】：书的正文内容
        /// 
        /// 注意：根据"谁先发送谁请求"原则，服务器主动推送时也使用此命令
        /// </summary>
        public static readonly CommandId CacheSync = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheSync & 0xFF), "CacheSync");
        
        /// <summary>
        /// 缓存订阅管理 - 用于缓存订阅操作
        /// 通过CacheRequest的SubscribeAction属性区分订阅/取消订阅
        /// </summary>
        public static readonly CommandId CacheSubscription = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheSubscription & 0xFF), "CacheSubscription");

        /// <summary>
        /// 缓存元数据同步 - 用于同步缓存的元数据信息
        /// 
        /// 【职责】：传输缓存的统计信息（不是实际数据）
        /// 【使用场景】：
        ///   1. 用户登录时，服务器告诉客户端有哪些表的缓存、最后更新时间、数据行数
        ///   2. 客户端根据元数据判断哪些表需要同步、哪些可以跳过
        /// 【特点】：数据量小（KB级）、频率低（登录/定时）、关注决策依据
        /// 【类比】：书的目录和出版信息
        /// 
        /// 优势：避免盲目全量同步，实现智能按需加载
        /// </summary>
        public static readonly CommandId CacheMetadataSync = new CommandId(CommandCategory.Cache, (byte)(CommandCatalog.Cache_CacheMetadataSync & 0xFF), "CacheMetadataSync");

        #endregion
    }
}
