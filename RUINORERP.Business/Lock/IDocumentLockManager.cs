using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Lock;

namespace RUINORERP.Business.Lock
{
    /// <summary>
    /// 文档锁定管理器接口
    /// 负责管理文档的锁定状态，确保并发操作的安全性
    /// </summary>
    public interface IDocumentLockManager
    {
        /// <summary>
        /// 尝试锁定文档
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <param name="userId">用户ID</param>
        /// <param name="operationId">操作ID</param>
        /// <param name="expireSeconds">过期时间（秒），默认300秒</param>
        /// <returns>是否锁定成功</returns>
        Task<bool> TryLockAsync(string lockKey, long userId, long operationId, int expireSeconds = 300);
        
        /// <summary>
        /// 解锁文档
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否解锁成功</returns>
        Task<bool> UnlockAsync(string lockKey, long userId);
        
        /// <summary>
        /// 检查文档是否被锁定
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <returns>是否被锁定</returns>
        Task<bool> IsLockedAsync(string lockKey);
        
        /// <summary>
        /// 获取锁定信息
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <returns>锁定信息，如果未锁定则返回null</returns>
        Task<LockInfo> GetLockInfoAsync(string lockKey);
    }
}