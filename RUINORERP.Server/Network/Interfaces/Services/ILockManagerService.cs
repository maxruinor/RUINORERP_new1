using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.Server.Network.Services;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 锁管理服务接口
    /// </summary>
    public interface ILockManagerService
    {
        /// <summary>
        /// 尝试锁定单据
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定结果，包含成功状态和详细信息</returns>
        Task<LockResponse> TryLockDocumentAsync(LockInfo lockInfo);

        /// <summary>
        /// 解锁单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>解锁结果，包含成功状态和详细信息</returns>
        Task<LockResponse> UnlockDocumentAsync(long billId, long userId);

        /// <summary>
        /// 获取指定单据的锁定信息
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定信息，如果未锁定则返回null</returns>
        LockInfo GetLockInfo(long billId);

        /// <summary>
        /// 获取所有锁定的单据信息
        /// </summary>
        /// <returns>锁定信息列表</returns>
        List<LockInfo> GetAllLockedDocuments();

        /// <summary>
        /// 根据业务类型解锁单据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bizType">业务类型</param>
        /// <returns>解锁结果，包含成功状态和详细信息</returns>
        Task<LockResponse> UnlockDocumentsByBizNameAsync(long userId, int bizType);

        /// <summary>
        /// 强制解锁单据（管理员操作）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>解锁结果，包含成功状态和详细信息</returns>
        Task<LockResponse> ForceUnlockDocumentAsync(long billId);

        /// <summary>
        /// 请求解锁单据
        /// </summary>
        /// <param name="request">锁定请求信息</param>
        /// <returns>请求结果，包含成功状态和详细信息</returns>
        Task<LockResponse> RequestUnlockDocumentAsync(LockRequest request);

        /// <summary>
        /// 拒绝解锁请求
        /// </summary>
        /// <param name="refuseInfo">拒绝信息</param>
        /// <returns>拒绝结果，包含成功状态和详细信息</returns>
        Task<LockResponse> RefuseUnlockRequestAsync(LockRequest request);
        
        /// <summary>
        /// 检查用户是否有权限修改单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否有权限</returns>
        bool HasPermissionToModifyDocument(long billId, long userId);
        
        /// <summary>
        /// 获取锁定单据的用户ID
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定用户ID，如果未锁定则返回0</returns>
        long GetLockedUserId(long billId);

        /// <summary>
        /// 获取锁定项数量
        /// </summary>
        /// <returns>锁定项数量</returns>
        int GetLockItemCount();

        /// <summary>
        /// 获取锁定统计信息
        /// </summary>
        /// <returns>锁定统计信息</returns>
        LockInfoStatistics GetLockStatistics();
        
        /// <summary>
        /// 广播锁定状态变化给所有客户端
        /// </summary>
        /// <param name="lockedDocument">锁定文档信息</param>
        Task BroadcastLockStatusAsync(LockInfo lockedDocument);

        /// <summary>
        /// 广播锁定状态变化给所有客户端
        /// </summary>
        /// <param name="lockedDocuments">锁定文档信息列表</param>
        Task BroadcastLockStatusAsync(IEnumerable<LockInfo> lockedDocuments, bool NeedReponse = false);

        /// <summary>
        /// 广播锁定状态变化给所有客户端（与BroadcastLockStatusAsync相同功能）
        /// </summary>
        /// <param name="lockedDocuments">锁定文档信息列表</param>
        Task BroadcastLockStatusToAllClientsAsync(IEnumerable<LockInfo> lockedDocuments);

        /// <summary>
        /// 启动锁管理服务
        /// </summary>
        Task StartAsync();
        
        /// <summary>
        /// 停止锁管理服务
        /// </summary>
        Task StopAsync();
        
        /// <summary>
        /// 释放所有锁
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns>释放的锁数量</returns>
        Task<int> ReleaseAllLocksBySessionIdAsync(long? userID);
        
        /// <summary>
        /// 处理心跳锁信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="activeLocks">活跃锁列表</param>
        Task ProcessHeartbeatLocksAsync(string sessionId, long[] activeLocks);
        
        /// <summary>
        /// 检查锁状态
        /// </summary>
        /// <param name="request">锁请求信息</param>
        /// <returns>锁状态检查结果</returns>
        Task<LockResponse> CheckLockStatusAsync(LockRequest request);
        
        /// <summary>
        /// 强制解锁
        /// </summary>
        /// <param name="request">解锁请求信息</param>
        /// <returns>解锁结果</returns>
        Task<LockResponse> ForceUnlockAsync(LockRequest request);
        
        /// <summary>
        /// 获取用户锁定的单据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户锁定的单据列表</returns>
        Task<List<LockInfo>> GetUserLocksAsync(long userId);
        
        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <param name="userId">用户ID</param>
        /// <returns>解锁结果</returns>
        Task<LockResponse> UnlockAsync(string lockKey, long userId);
        
        /// <summary>
        /// 强制解锁
        /// </summary>
        /// <param name="lockKey">锁定键</param>
        /// <param name="userId">用户ID</param>
        /// <returns>解锁结果</returns>
        Task<LockResponse> ForceUnlockAsync(string lockKey, long userId);
        
        /// <summary>
        /// 获取孤儿锁数量
        /// </summary>
        /// <returns>孤儿锁数量</returns>
        int GetOrphanedLockCount();
    }
}