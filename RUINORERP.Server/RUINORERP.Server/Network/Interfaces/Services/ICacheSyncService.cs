using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 缓存同步服务接口
    /// </summary>
    public interface ICacheSyncService
    {
        /// <summary>
        /// 订阅缓存变更
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="cacheKeys">缓存键列表</param>
        void Subscribe(string sessionId, IEnumerable<string> cacheKeys);

        /// <summary>
        /// 取消订阅缓存变更
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        void Unsubscribe(string sessionId);

        /// <summary>
        /// 获取会话的订阅信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>订阅信息</returns>
        CacheSubscription GetSubscription(string sessionId);

        /// <summary>
        /// 获取所有订阅信息
        /// </summary>
        /// <returns>订阅信息列表</returns>
        List<CacheSubscription> GetAllSubscriptions();

        /// <summary>
        /// 处理客户端缓存更新请求
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="updateRequest">更新请求</param>
        Task ProcessClientUpdateAsync(string sessionId, CacheRequest updateRequest);

        /// <summary>
        /// 向所有订阅者广播缓存同步消息
        /// </summary>
        /// <param name="message">缓存同步消息</param>
        Task BroadcastToAllSubscribersAsync(CacheSyncMessage message);
    }
}