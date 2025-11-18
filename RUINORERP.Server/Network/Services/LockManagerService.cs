using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Commands.Lock;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.Server.Network.Interfaces.Services;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 锁管理服务 - 企业级单据锁定管理
    /// 重构版本：使用LockInfoManager进行高效信息管理，提供更好的性能和功能
    /// </summary>
    public class LockManagerService : ILockManagerService, IDisposable
    {
        // 锁信息管理器 - 核心组件
        private readonly LockInfoManager _lockInfoManager;
        
        // 分布式锁接口
        private readonly ILocalDistributedLock _distributedLock;
        
        // 智能锁续期管理器
        private readonly SmartLockRenewer _lockRenewer;
        
        // 日志记录器
        private readonly ILogger<LockManagerService> _logger;
        
        // 默认配置
        private const int DefaultLockTimeoutSeconds = 300; // 5分钟
        private const string LockKeyPrefix = "lock:document:";

        /// <summary>
        /// 构造函数
        /// </summary>
        public LockManagerService(
            ILogger<LockManagerService> logger,
            ILocalDistributedLock distributedLock,
            SmartLockRenewer lockRenewer)
        {
            var lockInfoManagerLogger = Program.ServiceProvider?.GetService(typeof(ILogger<LockInfoManager>)) as ILogger<LockInfoManager> ?? 
                                       new Microsoft.Extensions.Logging.Logger<LockInfoManager>(new Microsoft.Extensions.Logging.LoggerFactory());
            var lockInfoManagerConfig = Program.ServiceProvider?.GetService(typeof(LockInfoManagerConfig)) as LockInfoManagerConfig ?? new LockInfoManagerConfig();
            
            _lockInfoManager = new LockInfoManager(lockInfoManagerLogger, lockInfoManagerConfig);
            _distributedLock = distributedLock ?? throw new ArgumentNullException(nameof(distributedLock));
            _lockRenewer = lockRenewer ?? throw new ArgumentNullException(nameof(lockRenewer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _logger.LogInformation("锁管理服务(重构版)初始化完成");
        }

        /// <summary>
        /// 尝试锁定单据
        /// </summary>
        public async Task<bool> TryLockDocumentAsync(LockInfo lockInfo)
        {
            // 参数验证
            if (lockInfo == null || lockInfo.BillID <= 0 || lockInfo.UserId <= 0)
            {
                _logger.LogError("无效的锁定信息: BillID={BillID}, UserID={UserID}", 
                    lockInfo?.BillID ?? 0, 
                    lockInfo?.UserId ?? 0);
                return false;
            }

            try
            {
                // 生成分布式锁键
                string lockKey = GetLockKey(lockInfo.BillID);
                
                // 首先检查是否已存在锁定（内存级别检查，快速失败）
                var existingLock = _lockInfoManager.GetLockInfoByBillId(lockInfo.BillID);
                if (existingLock != null)
                {
                    _logger.LogInformation("单据已被锁定: BillID={BillID}, LockedUserID={LockedUserID}", 
                        lockInfo.BillID, 
                        existingLock.UserId);
                    
                    // 记录失败统计
                    _lockInfoManager.RecordFailedLock(lockInfo.BillID, lockInfo.UserId, "单据已被锁定");
                    return false;
                }

                // 尝试获取分布式锁
                bool distributedLockAcquired = await _distributedLock.TryAcquireAsync(
                    lockKey, 
                    lockInfo.LockId,
                    TimeSpan.FromSeconds(DefaultLockTimeoutSeconds));
                
                if (!distributedLockAcquired)
                {
                    _logger.LogWarning("分布式锁获取失败: BillID={BillID}", lockInfo.BillID);
                    
                    // 记录失败统计
                    _lockInfoManager.RecordFailedLock(lockInfo.BillID, lockInfo.UserId, "分布式锁获取失败");
                    return false;
                }

                // 再次检查是否已存在锁定（双重检查，防止并发问题）
                existingLock = _lockInfoManager.GetLockInfoByBillId(lockInfo.BillID);
                if (existingLock != null)
                {
                    _logger.LogInformation("并发检查：单据已被锁定: BillID={BillID}, LockedUserID={LockedUserID}", 
                        lockInfo.BillID, 
                        existingLock.UserId);
                    
                    // 释放分布式锁
                    await _distributedLock.ReleaseAsync(lockKey, lockInfo.LockId);
                    
                    // 记录失败统计
                    _lockInfoManager.RecordFailedLock(lockInfo.BillID, lockInfo.UserId, "并发检查失败");
                    return false;
                }

                // 设置锁定详细信息
                lockInfo.LockTime = DateTime.Now;
                lockInfo.ExpireTime = lockInfo.LockTime.AddSeconds(DefaultLockTimeoutSeconds);
                lockInfo.LockId = Guid.NewGuid().ToString();

                // 添加锁定信息
                var added = _lockInfoManager.AddLockInfo(lockInfo);
                if (added)
                {
                    _logger.LogInformation("成功锁定单据: BillID={BillID}, UserID={UserID}, LockId={LockId}", 
                        lockInfo.BillID, 
                        lockInfo.UserId, 
                        lockInfo.LockId);
                    
                    // 注册锁续期
                    _lockRenewer.RegisterLock(lockKey, lockInfo.LockId, TimeSpan.FromSeconds(DefaultLockTimeoutSeconds));
                    
                    // 记录成功统计
                    _lockInfoManager.RecordSuccessfulLock(lockInfo.BillID, lockInfo.UserId);
                }
                else
                {
                    _logger.LogWarning("锁定单据失败: BillID={BillID}, UserID={UserID}", 
                        lockInfo.BillID, 
                        lockInfo.UserId);
                    
                    // 释放分布式锁
                    await _distributedLock.ReleaseAsync(lockKey, lockInfo.LockId);
                    
                    // 记录失败统计
                    _lockInfoManager.RecordFailedLock(lockInfo.BillID, lockInfo.UserId, "内存锁定失败");
                }

                return added;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "锁定单据时发生异常: BillID={BillID}, UserID={UserID}", 
                    lockInfo.BillID, 
                    lockInfo.UserId);
                
                // 记录异常统计
                _lockInfoManager.RecordFailedLock(lockInfo.BillID, lockInfo.UserId, $"异常: {ex.Message}");
                
                // 尝试清理分布式锁
                string lockKey = GetLockKey(lockInfo.BillID);
                try
                {
                    await _distributedLock.ReleaseAsync(lockKey, lockInfo.LockId);
                }
                catch (Exception releaseEx)
                {
                    // 记录释放锁时的异常，但不中断主流程
                    _logger.LogWarning(releaseEx, "释放分布式锁时发生异常: BillID={BillID}, LockId={LockId}", 
                        lockInfo.BillID, 
                        lockInfo.LockId);
                }
                
                return false;
            }
        }

        /// <summary>
        /// 解锁单据
        /// </summary>
        public async Task<bool> UnlockDocumentAsync(long billId, long userId)
        {
            // 参数验证
            if (billId <= 0 || userId <= 0)
            {
                _logger.LogError("无效的解锁参数: BillID={BillID}, UserID={UserID}", billId, userId);
                return false;
            }

            try
            {
                // 获取锁定信息
                var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
                if (lockInfo == null)
                {
                    _logger.LogInformation("单据未被锁定，无需解锁: BillID={BillID}", billId);
                    return true;
                }

                // 验证解锁权限
                if (lockInfo.UserId != userId)
                {
                    _logger.LogWarning("用户尝试解锁不属于自己的单据: BillID={BillID}, AttemptedUserID={UserID}, ActualUserID={ActualUserID}", 
                        billId, 
                        userId, 
                        lockInfo.UserId);
                    return false;
                }

                // 生成分布式锁键
                string lockKey = GetLockKey(billId);

                // 首先释放分布式锁，确保分布式层面的解锁
                try
                {
                    await _distributedLock.ReleaseAsync(lockKey, lockInfo.LockId);
                    _logger.LogDebug("分布式锁释放成功: BillID={BillID}, LockId={LockId}", billId, lockInfo.LockId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "释放分布式锁时发生异常，继续执行内存解锁: BillID={BillID}, LockId={LockId}", billId, lockInfo.LockId);
                }

                // 取消锁续期
                try
                {
                    _lockRenewer.UnregisterLock(lockKey);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "取消锁续期时发生异常: BillID={BillID}", billId);
                }

                // 移除内存中的锁定信息
                var removed = _lockInfoManager.RemoveLockInfo(billId);
                if (removed)
                {
                    _logger.LogInformation("成功解锁单据: BillID={BillID}, UserID={UserID}, LockId={LockId}", 
                        billId, 
                        userId, 
                        lockInfo.LockId);
                    
                    // 记录解锁统计
                    _lockInfoManager.RecordUnlock(billId, userId, "用户主动解锁");
                }
                else
                {
                    _logger.LogWarning("解锁单据失败: BillID={BillID}, UserID={UserID}", billId, userId);
                }

                return removed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解锁单据时发生异常: BillID={BillID}, UserID={UserID}", billId, userId);
                return false;
            }
        }

        /// <summary>
        /// 获取指定单据的锁定信息
        /// </summary>
        public LockInfo GetLockInfo(long billId)
        {
            if (billId <= 0)
                return null;

            var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
            if (lockInfo != null)
            {
                _logger.LogDebug("获取锁定信息成功: BillID={BillID}, UserID={UserID}", 
                lockInfo.BillID, 
                lockInfo.UserId);
            }
            else
            {
                _logger.LogDebug("未找到锁定信息: BillID={BillID}", billId);
            }

            return lockInfo;
        }

        /// <summary>
        /// 获取所有锁定的单据信息
        /// </summary>
        public List<LockInfo> GetAllLockedDocuments()
        {
            var lockedDocs = _lockInfoManager.GetLockInfoByStatus(LockStatus.Locked);
            _logger.LogDebug("获取所有锁定单据: 数量={Count}", lockedDocs.Count);
            return lockedDocs;
        }

        /// <summary>
        /// 根据业务类型解锁单据
        /// </summary>
        public async Task<bool> UnlockDocumentsByBizNameAsync(long userId, int BillType)
        {
            // 参数验证
            if (userId <= 0 || BillType <= 0)
            {
                _logger.LogError("无效的解锁参数: UserID={UserID}, BillType={BillType}", userId, BillType);
                return false;
            }

            try
            {
                // 获取用户的所有锁定
                var userLocks = _lockInfoManager.GetLockInfoByUserId(userId);
                var bizType = (BizType)BillType;
                
                var toUnlock = userLocks.Where(lockInfo => 
                    lockInfo.BillData?.BizType == bizType).ToList();

                var successCount = 0;
                foreach (var lockInfo in toUnlock)
                {
                    var unlocked = await UnlockDocumentAsync(lockInfo.BillID, userId);
                    if (unlocked)
                    {
                        successCount++;
                    }
                }

                return successCount == toUnlock.Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "按业务类型解锁时发生异常: BillType={BillType}, UserID={UserID}", 
                    BillType.ToString(), 
                    userId);
                return false;
            }
        }

        /// <summary>
        /// 强制解锁单据（管理员操作）
        /// </summary>
        public async Task<bool> ForceUnlockDocumentAsync(long billId)
        {
            // 参数验证
            if (billId <= 0)
            {
                _logger.LogError("无效的强制解锁参数: BillID={BillID}", billId);
                return false;
            }

            try
            {
                // 获取锁定信息
                var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
                if (lockInfo == null)
                {
                    _logger.LogInformation("单据未被锁定，无需强制解锁: BillID={BillID}", billId);
                    return true;
                }

                _logger.LogInformation("开始强制解锁单据: BillID={BillID}, LockedUserID={LockedUserID}, LockId={LockId}", 
                    billId, 
                    lockInfo.UserId, 
                    lockInfo.LockId);

                // 生成分布式锁键
                string lockKey = GetLockKey(billId);

                // 首先释放分布式锁，确保分布式层面的解锁
                try
                {
                    await _distributedLock.ReleaseAsync(lockKey, lockInfo.LockId);
                    _logger.LogDebug("强制解锁：分布式锁释放成功: BillID={BillID}, LockId={LockId}", billId, lockInfo.LockId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "强制解锁：释放分布式锁时发生异常，继续执行内存解锁: BillID={BillID}, LockId={LockId}", billId, lockInfo.LockId);
                }

                // 取消锁续期
                try
                {
                    _lockRenewer.UnregisterLock(lockKey);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "强制解锁：取消锁续期时发生异常: BillID={BillID}", billId);
                }

                // 移除内存中的锁定信息
                var removed = _lockInfoManager.RemoveLockInfo(billId);
                if (removed)
                {
                    _logger.LogInformation("成功强制解锁单据: BillID={BillID}, LockId={LockId}", 
                        billId, 
                        lockInfo.LockId);
                    
                    // 记录强制解锁统计
                    _lockInfoManager.RecordUnlock(billId, lockInfo.UserId, "管理员强制解锁");
                }
                else
                {
                    _logger.LogWarning("强制解锁单据失败: BillID={BillID}", billId);
                }

                return removed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "强制解锁单据时发生异常: BillID={BillID}", billId);
                return false;
            }
        }

        /// <summary>
        /// 请求解锁单据
        /// </summary>
        public async Task<bool> RequestUnlockDocumentAsync(LockRequest request)
        {
            // 参数验证
            if (request == null || request.LockInfo?.BillID <= 0 || request.RequesterUserId <= 0)
            {
                _logger.LogError("无效的解锁请求信息: BillID={BillID}, RequestUserID={RequestUserID}", 
                    request?.LockInfo?.BillID ?? 0, 
                    request?.RequesterUserId ?? 0);
                return false;
            }

            try
            {
                // 检查单据是否被锁定
                var lockInfo = _lockInfoManager.GetLockInfoByBillId(request.LockInfo.BillID);
                if (lockInfo == null)
                {
                    _logger.LogInformation("单据未被锁定，无法请求解锁: BillID={BillID}", request.LockInfo.BillID);
                    return false;
                }

                // 记录请求信息
                _logger.LogInformation("用户请求解锁单据: BillID={BillID}, RequestUserID={RequestUserID}, LockedUserID={LockedUserID}, RequestReason={RequestReason}", 
                    request.LockInfo.BillID, 
                    request.RequesterUserId, 
                    lockInfo.UserId, 
                    request.Reason);

                // 记录解锁请求历史
                _lockInfoManager.RecordUnlockRequest(request.LockInfo.BillID, request.RequesterUserId, 
                    lockInfo.UserId, request.Reason);

                // TODO: 实现解锁请求通知机制
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "请求解锁时发生异常: BillID={BillID}, RequestUserID={RequestUserID}", 
                    request.LockInfo.BillID, 
                    request.RequesterUserId);
                return false;
            }
        }

        /// <summary>
        /// 拒绝解锁请求
        /// </summary>
        public async Task<bool> RefuseUnlockRequestAsync(LockRequest refuseInfo)
        {
            // 参数验证
            if (refuseInfo == null || refuseInfo.LockInfo?.BillID <= 0 || refuseInfo.LockInfo?.UserId <= 0)
            {
                _logger.LogError("无效的拒绝解锁信息: BillID={BillID}, RefuseUserID={RefuseUserID}", 
                    refuseInfo?.LockInfo?.BillID ?? 0, 
                    refuseInfo?.LockInfo?.UserId ?? 0);
                return false;
            }

            try
            {
                // 记录拒绝信息
                _logger.LogInformation("用户拒绝了解锁请求: BillID={BillID}, RefuseUserID={RefuseUserID}, RequestUserID={RequestUserID}, RefuseReason={RefuseReason}", 
                    refuseInfo.LockInfo.BillID, 
                    refuseInfo.LockInfo.UserId, 
                    refuseInfo.RequesterUserId, 
                    refuseInfo.Reason);

                // 记录拒绝解锁历史
                _lockInfoManager.RecordUnlockRefuse(refuseInfo.LockInfo.BillID, refuseInfo.LockInfo.UserId, 
                    refuseInfo.RequesterUserId, refuseInfo.Reason);

                // TODO: 实现拒绝解锁通知机制
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "拒绝解锁请求时发生异常: BillID={BillID}, RefuseUserID={RefuseUserID}", 
                    refuseInfo.LockInfo.BillID, 
                    refuseInfo.LockInfo.UserId);
                return false;
            }
        }

        /// <summary>
        /// 获取锁定统计信息
        /// </summary>
        public LockInfoStatistics GetLockStatistics()
        {
            try
            {
                var statistics = _lockInfoManager.GetStatistics();
                _logger.LogDebug("获取锁定统计信息: 总数={Total}, 活跃={Active}, 成功={Success}, 失败={Failed}", 
                    statistics.TotalLocks, 
                    statistics.ActiveLocks, 
                    statistics.SuccessfulLocks, 
                    statistics.FailedLocks);
                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定统计信息时发生异常");
                return new LockInfoStatistics();
            }
        }

        /// <summary>
        /// 检查用户是否有权限修改单据
        /// </summary>
        public bool HasPermissionToModifyDocument(long billId, long userId)
        {
            if (billId <= 0 || userId <= 0)
                return false;

            // 获取锁定信息
            var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
            if (lockInfo == null)
            {
                // 未被锁定，可以修改
                return true;
            }

            // 检查是否是锁定该单据的用户
            return lockInfo.UserId == userId;
        }

        /// <summary>
        /// 获取锁定单据的用户ID
        /// </summary>
        public long GetLockedUserId(long billId)
        {
            var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
            return lockInfo?.UserId ?? 0;
        }

        /// <summary>
        /// 获取增强统计信息（新功能）
        /// </summary>
        public LockInfoStatistics GetEnhancedStatistics()
        {
            try
            {
                return _lockInfoManager.GetStatistics();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取增强统计信息时发生异常");
                return new LockInfoStatistics();
            }
        }

        /// <summary>
        /// 获取锁定历史记录（新功能）
        /// </summary>
        public List<LockHistoryRecord> GetLockHistory(int maxRecords = 100)
        {
            try
            {
                return _lockInfoManager.GetLockHistory(maxRecords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定历史记录时发生异常");
                return new List<LockHistoryRecord>();
            }
        }

        /// <summary>
        /// 获取锁定项列表（新功能）
        /// </summary>
        public List<LockInfo> GetLockItems()
        {
            try
            {
                var lockInfos = _lockInfoManager.GetAllLockInfo();
                return lockInfos.Select(lockInfo => lockInfo.Clone()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定项列表时发生异常");
                return new List<LockInfo>();
            }
        }

        /// <summary>
        /// 获取锁定项数量
        /// </summary>
        public int GetLockItemCount()
        {
            try
            {
                var lockInfos = _lockInfoManager.GetAllLockInfo();
                return lockInfos?.Count ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定项数量时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                _logger.LogInformation("锁管理服务释放资源");
                _lockInfoManager?.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放资源时发生异常");
            }
        }

        /// <summary>
        /// 生成分布式锁键
        /// </summary>
        private string GetLockKey(long billId) => $"{LockKeyPrefix}{billId}";
    }

    /// <summary>
    /// 原LockManagerService标记为过时 - 请使用重构后的版本
    /// </summary>
    [Obsolete("此类已过时，请直接使用重构后的LockManagerService，功能已完全替换并增强")]
    public class LockManagerServiceLegacy
    {
        // 此类仅用于标识旧代码已过时，不包含实际实现
    }
}