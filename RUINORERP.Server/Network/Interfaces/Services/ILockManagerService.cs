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
        /// <returns>锁定结果</returns>
        Task<bool> TryLockDocumentAsync(LockInfo lockInfo);

        /// <summary>
        /// 解锁单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>解锁结果</returns>
        Task<bool> UnlockDocumentAsync(long billId, long userId);

        /// <summary>
        /// 检查单据是否被锁定
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
        /// <param name="bizName">业务名称</param>
        /// <returns>解锁结果</returns>
        Task<bool> UnlockDocumentsByBizNameAsync(long userId, int BillType);

        /// <summary>
        /// 强制解锁单据（管理员操作）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>解锁结果</returns>
        Task<bool> ForceUnlockDocumentAsync(long billId);

        /// <summary>
        /// 请求解锁单据
        /// </summary>
        /// <param name="request">锁定请求信息</param>
        /// <returns>请求结果</returns>
        Task<bool> RequestUnlockDocumentAsync(LockRequest request);

        /// <summary>
        /// 拒绝解锁请求
        /// </summary>
        /// <param name="refuseInfo">拒绝信息</param>
        /// <returns>拒绝结果</returns>
        Task<bool> RefuseUnlockRequestAsync(LockRequest refuseInfo);
        
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
        /// 获取锁定项列表
        /// </summary>
        /// <returns>锁定项列表</returns>
        List<LockInfo> GetLockItems();

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
    }
}