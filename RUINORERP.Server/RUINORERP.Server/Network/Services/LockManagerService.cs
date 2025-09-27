using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Server.Network.Interfaces.Services;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 锁管理服务 - 处理业务单据的锁定逻辑
    /// </summary>
    public class LockManagerService : ILockManagerService
    {
        private readonly ConcurrentDictionary<long, LockedInfo> _lockedDocuments;
        private readonly ILogger<LockManagerService> _logger;

        public LockManagerService(ILogger<LockManagerService> logger)
        {
            _lockedDocuments = new ConcurrentDictionary<long, LockedInfo>();
            _logger = logger;
        }

        /// <summary>
        /// 尝试锁定单据
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定结果</returns>
        public async Task<bool> TryLockDocumentAsync(LockedInfo lockInfo)
        {
            try
            {
                // 检查单据是否已经被锁定
                if (_lockedDocuments.ContainsKey(lockInfo.BillID))
                {
                    _logger.LogInformation($"单据 {lockInfo.BillID} 已被锁定，无法重复锁定");
                    return false;
                }

                // 添加锁定信息
                var added = _lockedDocuments.TryAdd(lockInfo.BillID, lockInfo);
                if (added)
                {
                    _logger.LogInformation($"成功锁定单据 {lockInfo.BillID}，锁定用户: {lockInfo.LockedUserID}");
                }
                else
                {
                    _logger.LogWarning($"锁定单据 {lockInfo.BillID} 失败");
                }

                return added;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"锁定单据 {lockInfo.BillID} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 解锁单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>解锁结果</returns>
        public async Task<bool> UnlockDocumentAsync(long billId, long userId)
        {
            try
            {
                // 检查单据是否存在锁定信息
                if (!_lockedDocuments.TryGetValue(billId, out var lockInfo))
                {
                    _logger.LogInformation($"单据 {billId} 未被锁定，无需解锁");
                    return true;
                }

                // 检查是否是锁定该单据的用户
                if (lockInfo.LockedUserID != userId)
                {
                    _logger.LogWarning($"用户 {userId} 尝试解锁不属于自己的单据 {billId}");
                    return false;
                }

                // 移除锁定信息
                var removed = _lockedDocuments.TryRemove(billId, out _);
                if (removed)
                {
                    _logger.LogInformation($"成功解锁单据 {billId}，解锁用户: {userId}");
                }
                else
                {
                    _logger.LogWarning($"解锁单据 {billId} 失败");
                }

                return removed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"解锁单据 {billId} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 检查单据是否被锁定
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定信息，如果未锁定则返回null</returns>
        public LockedInfo GetLockInfo(long billId)
        {
            _lockedDocuments.TryGetValue(billId, out var lockInfo);
            return lockInfo;
        }

        /// <summary>
        /// 获取所有锁定的单据信息
        /// </summary>
        /// <returns>锁定信息列表</returns>
        public List<LockedInfo> GetAllLockedDocuments()
        {
            return _lockedDocuments.Values.ToList();
        }

        /// <summary>
        /// 根据业务类型解锁单据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="bizName">业务名称</param>
        /// <returns>解锁结果</returns>
        public async Task<bool> UnlockDocumentsByBizNameAsync(long userId, string bizName)
        {
            try
            {
                var removedCount = 0;
                var keysToRemove = new List<long>();

                // 查找需要解锁的单据
                foreach (var kvp in _lockedDocuments)
                {
                    if (kvp.Value.LockedUserID == userId && 
                        kvp.Value.BillData?.BizName == bizName)
                    {
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // 批量移除
                foreach (var key in keysToRemove)
                {
                    if (_lockedDocuments.TryRemove(key, out _))
                    {
                        removedCount++;
                        _logger.LogInformation($"成功按业务类型解锁单据 {key}");
                    }
                }

                _logger.LogInformation($"按业务类型 {bizName} 解锁了 {removedCount} 个单据");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"按业务类型 {bizName} 解锁单据时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 强制解锁单据（管理员操作）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>解锁结果</returns>
        public async Task<bool> ForceUnlockDocumentAsync(long billId)
        {
            try
            {
                var removed = _lockedDocuments.TryRemove(billId, out _);
                if (removed)
                {
                    _logger.LogInformation($"成功强制解锁单据 {billId}");
                }
                else
                {
                    _logger.LogInformation($"单据 {billId} 未被锁定，无需强制解锁");
                }

                return removed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"强制解锁单据 {billId} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 请求解锁单据
        /// </summary>
        /// <param name="requestInfo">请求信息</param>
        /// <returns>请求结果</returns>
        public async Task<bool> RequestUnlockDocumentAsync(RequestUnLockInfo requestInfo)
        {
            try
            {
                // 检查单据是否被锁定
                if (!_lockedDocuments.TryGetValue(requestInfo.BillID, out var lockInfo))
                {
                    _logger.LogInformation($"单据 {requestInfo.BillID} 未被锁定，无法请求解锁");
                    return false;
                }

                // 记录请求信息（可以存储到数据库或缓存中）
                _logger.LogInformation($"用户 {requestInfo.RequestUserID} 请求解锁单据 {requestInfo.BillID}");
                
                // 这里可以添加更多业务逻辑，比如发送通知给锁定用户等
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"请求解锁单据 {requestInfo.BillID} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 拒绝解锁请求
        /// </summary>
        /// <param name="refuseInfo">拒绝信息</param>
        /// <returns>拒绝结果</returns>
        public async Task<bool> RefuseUnlockRequestAsync(RefuseUnLockInfo refuseInfo)
        {
            try
            {
                // 记录拒绝信息（可以存储到数据库或缓存中）
                _logger.LogInformation($"用户 {refuseInfo.RefuseUserID} 拒绝了解锁请求");
                
                // 这里可以添加更多业务逻辑，比如发送通知给请求用户等
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"拒绝解锁请求时发生异常");
                return false;
            }
        }
        
        /// <summary>
        /// 检查用户是否有权限修改单据
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否有权限</returns>
        public bool HasPermissionToModifyDocument(long billId, long userId)
        {
            // 检查单据是否被锁定
            if (!_lockedDocuments.TryGetValue(billId, out var lockInfo))
            {
                // 未被锁定，可以修改
                return true;
            }
            
            // 检查是否是锁定该单据的用户
            return lockInfo.LockedUserID == userId;
        }
        
        /// <summary>
        /// 获取锁定单据的用户ID
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定用户ID，如果未锁定则返回0</returns>
        public long GetLockedUserId(long billId)
        {
            if (_lockedDocuments.TryGetValue(billId, out var lockInfo))
            {
                return lockInfo.LockedUserID;
            }
            return 0;
        }
    }
}