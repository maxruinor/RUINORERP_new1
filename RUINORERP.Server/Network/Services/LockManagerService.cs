using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Lock;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;

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
        /// <param name="billId">单据ID</param>
        /// <returns>锁定信息，如果未找到则返回null</returns>
        public LockInfo GetLockInfo(long billId)
        {
            if (billId <= 0)
            {
                _logger.LogWarning("获取锁定信息失败：无效的单据ID={BillID}", billId);
                return null;
            }

            try
            {
                var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
                if (lockInfo != null)
                {
                    _logger.LogDebug("获取锁定信息成功: BillID={BillID}, UserID={UserID}, LockId={LockId}", 
                        lockInfo.BillID, 
                        lockInfo.UserId,
                        lockInfo.LockId);
                }
                else
                {
                    _logger.LogDebug("未找到锁定信息: BillID={BillID}", billId);
                }

                return lockInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定信息时发生异常: BillID={BillID}", billId);
                return null;
            }
        }

        /// <summary>
        /// 获取所有锁定的单据信息
        /// </summary>
        /// <returns>锁定单据列表</returns>
        public List<LockInfo> GetAllLockedDocuments()
        {
            try
            {
                var lockedDocs = _lockInfoManager.GetLockInfoByStatus(LockStatus.Locked);
                _logger.LogDebug("获取所有锁定单据: 数量={Count}", lockedDocs?.Count ?? 0);
                return lockedDocs ?? new List<LockInfo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有锁定单据时发生异常");
                return new List<LockInfo>();
            }
        }

        /// <summary>
        /// 根据业务类型解锁单据
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="billType">单据类型</param>
        /// <returns>是否全部解锁成功</returns>
        public async Task<bool> UnlockDocumentsByBizNameAsync(long userId, int billType)
        {
            // 参数验证
            if (userId <= 0 || billType <= 0)
            {
                _logger.LogError("无效的解锁参数: UserID={UserID}, BillType={BillType}", userId, billType);
                return false;
            }

            try
            {
                // 获取用户的所有锁定
                var userLocks = _lockInfoManager.GetLockInfoByUserId(userId);
                if (userLocks == null || !userLocks.Any())
                {
                    _logger.LogInformation("用户没有锁定任何单据: UserID={UserID}", userId);
                    return true;
                }

                var bizType = (BizType)billType;
                var toUnlock = userLocks.Where(lockInfo => 
                    lockInfo.BillData?.BizType == bizType).ToList();

                if (!toUnlock.Any())
                {
                    _logger.LogInformation("用户没有锁定指定业务类型的单据: UserID={UserID}, BizType={BizType}", 
                        userId, bizType);
                    return true;
                }

                _logger.LogInformation("开始批量解锁业务类型单据: UserID={UserID}, BizType={BizType}, Count={Count}", 
                    userId, bizType, toUnlock.Count);

                var successCount = 0;
                foreach (var lockInfo in toUnlock)
                {
                    var unlocked = await UnlockDocumentAsync(lockInfo.BillID, userId);
                    if (unlocked)
                    {
                        successCount++;
                    }
                    else
                    {
                        _logger.LogWarning("解锁单个单据失败: BillID={BillID}, UserID={UserID}", 
                            lockInfo.BillID, userId);
                    }
                }

                _logger.LogInformation("批量解锁完成: UserID={UserID}, BizType={BizType}, Success={Success}, Total={Total}", 
                    userId, bizType, successCount, toUnlock.Count);

                return successCount == toUnlock.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "按业务类型解锁时发生异常: BillType={BillType}, UserID={UserID}", 
                    billType.ToString(), 
                    userId);
                return false;
            }
        }

        /// <summary>
        /// 强制解锁单据（管理员操作）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>是否强制解锁成功</returns>
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

                // 执行强制解锁操作
                var unlocked = await PerformUnlockAsync(billId, lockInfo.LockId, "管理员强制解锁");
                
                if (unlocked)
                {
                    _logger.LogInformation("成功强制解锁单据: BillID={BillID}, LockId={LockId}", 
                        billId, 
                        lockInfo.LockId);
                }
                else
                {
                    _logger.LogWarning("强制解锁单据失败: BillID={BillID}", billId);
                }

                return unlocked;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "强制解锁单据时发生异常: BillID={BillID}", billId);
                return false;
            }
        }

        /// <summary>
        /// 执行解锁操作（通用解锁方法）
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="lockId">锁ID</param>
        /// <param name="unlockReason">解锁原因</param>
        /// <returns>是否解锁成功</returns>
        private async Task<bool> PerformUnlockAsync(long billId, string lockId, string unlockReason)
        {
            // 参数验证
            if (billId <= 0 || string.IsNullOrEmpty(lockId))
            {
                _logger.LogError("无效的解锁参数: BillID={BillID}, LockId={LockId}", billId, lockId);
                return false;
            }

            try
            {
                // 生成分布式锁键
                string lockKey = GetLockKey(billId);

                // 首先释放分布式锁，确保分布式层面的解锁
                try
                {
                    await _distributedLock.ReleaseAsync(lockKey, lockId);
                    _logger.LogDebug("分布式锁释放成功: BillID={BillID}, LockId={LockId}", billId, lockId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "释放分布式锁时发生异常，继续执行内存解锁: BillID={BillID}, LockId={LockId}", billId, lockId);
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
                    _logger.LogInformation("成功解锁单据: BillID={BillID}, LockId={LockId}, Reason={Reason}", 
                        billId, 
                        lockId,
                        unlockReason);
                    
                    // 记录解锁统计
                    _lockInfoManager.RecordUnlock(billId, 0, unlockReason); // 用户ID设为0，因为是系统或管理员操作
                }
                else
                {
                    _logger.LogWarning("解锁单据失败: BillID={BillID}, LockId={LockId}", billId, lockId);
                }

                return removed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行解锁操作时发生异常: BillID={BillID}, LockId={LockId}", billId, lockId);
                return false;
            }
        }

        /// <summary>
        /// 请求解锁单据
        /// </summary>
        /// <param name="request">解锁请求信息</param>
        /// <returns>是否请求成功</returns>
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

                // 验证请求者是否就是被锁定者
                if (request.RequesterUserId == lockInfo.UserId)
                {
                    _logger.LogInformation("请求者就是被锁定者，无需请求解锁: BillID={BillID}, UserID={UserID}", 
                        request.LockInfo.BillID, request.RequesterUserId);
                    return true;
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
                _logger.LogInformation("解锁请求已记录，等待通知机制实现: BillID={BillID}", request.LockInfo.BillID);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "请求解锁单据时发生异常: BillID={BillID}, RequestUserID={RequestUserID}", 
                    request.LockInfo.BillID, request.RequesterUserId);
                return false;
            }
        } 

        /// <summary>
        /// 拒绝解锁请求
        /// </summary>
        /// <param name="refuseInfo">拒绝解锁信息</param>
        /// <returns>是否拒绝成功</returns>
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
                // 检查是否存在解锁请求
                var lockInfo = _lockInfoManager.GetLockInfoByBillId(refuseInfo.LockInfo.BillID);
                if (lockInfo == null)
                {
                    _logger.LogInformation("单据未被锁定，无法拒绝解锁请求: BillID={BillID}", refuseInfo.LockInfo.BillID);
                    return false;
                }

                // 验证拒绝者是否就是被锁定者
                if (refuseInfo.LockInfo.UserId != lockInfo.UserId)
                {
                    _logger.LogWarning("拒绝者不是被锁定者，无权拒绝解锁请求: BillID={BillID}, RefuseUserID={RefuseUserID}, LockedUserID={LockedUserID}", 
                        refuseInfo.LockInfo.BillID, refuseInfo.LockInfo.UserId, lockInfo.UserId);
                    return false;
                }

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
                _logger.LogInformation("拒绝解锁请求已记录，等待通知机制实现: BillID={BillID}", refuseInfo.LockInfo.BillID);
                
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
        /// <returns>锁定统计信息</returns>
        public LockInfoStatistics GetLockStatistics()
        {
            try
            {
                var statistics = _lockInfoManager.GetStatistics();
                if (statistics == null)
                {
                    _logger.LogWarning("获取锁定统计信息为空，返回默认统计对象");
                    return new LockInfoStatistics();
                }

                _logger.LogDebug("获取锁定统计信息: 总数={Total}, 活跃={Active}, 成功={Success}, 失败={Failed}, 平均锁定时间={AvgLockTime:F2}ms", 
                    statistics.TotalLocks, 
                    statistics.ActiveLocks, 
                    statistics.SuccessfulLocks, 
                    statistics.FailedLocks,
                    statistics.AverageLockTime);
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
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>是否有修改权限</returns>
        public bool HasPermissionToModifyDocument(long billId, long userId)
        {
            // 参数验证
            if (billId <= 0 || userId <= 0)
            {
                _logger.LogWarning("无效的参数: BillID={BillID}, UserID={UserID}", billId, userId);
                return false;
            }

            try
            {
                // 获取锁定信息
                var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
                if (lockInfo == null)
                {
                    // 未被锁定，可以修改
                    _logger.LogDebug("单据未被锁定，用户有修改权限: BillID={BillID}, UserID={UserID}", billId, userId);
                    return true;
                }

                // 检查是否是锁定该单据的用户
                bool hasPermission = lockInfo.UserId == userId;
                if (hasPermission)
                {
                    _logger.LogDebug("用户是锁定者，有修改权限: BillID={BillID}, UserID={UserID}, LockUserID={LockUserID}", 
                        billId, userId, lockInfo.UserId);
                }
                else
                {
                    _logger.LogInformation("用户不是锁定者，无修改权限: BillID={BillID}, UserID={UserID}, LockUserID={LockUserID}", 
                        billId, userId, lockInfo.UserId);
                }
                
                return hasPermission;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查用户修改权限时发生异常: BillID={BillID}, UserID={UserID}", billId, userId);
                return false;
            }
        }

        /// <summary>
        /// 获取锁定单据的用户ID
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定用户的ID，如果未锁定则返回0</returns>
        public long GetLockedUserId(long billId)
        {
            // 参数验证
            if (billId <= 0)
            {
                _logger.LogWarning("无效的单据ID: BillID={BillID}", billId);
                return 0;
            }

            try
            {
                var lockInfo = _lockInfoManager.GetLockInfoByBillId(billId);
                long lockedUserId = lockInfo?.UserId ?? 0;
                
                if (lockedUserId > 0)
                {
                    _logger.LogDebug("获取到锁定用户信息: BillID={BillID}, LockedUserID={LockedUserID}", billId, lockedUserId);
                }
                else
                {
                    _logger.LogDebug("单据未被锁定: BillID={BillID}", billId);
                }
                
                return lockedUserId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定用户信息时发生异常: BillID={BillID}", billId);
                return 0;
            }
        }

        /// <summary>
        /// 获取锁定历史记录（新功能）
        /// </summary>
        /// <param name="maxRecords">最大记录数，默认100</param>
        /// <returns>锁定历史记录列表</returns>
        public List<LockHistoryRecord> GetLockHistory(int maxRecords = 100)
        {
            // 参数验证
            if (maxRecords <= 0)
            {
                _logger.LogWarning("无效的最大记录数参数: MaxRecords={MaxRecords}", maxRecords);
                return new List<LockHistoryRecord>();
            }

            try
            {
                var history = _lockInfoManager.GetLockHistory(maxRecords);
                if (history == null)
                {
                    _logger.LogWarning("获取锁定历史记录返回空值");
                    return new List<LockHistoryRecord>();
                }

                _logger.LogDebug("获取锁定历史记录成功: Count={Count}, MaxRecords={MaxRecords}", history.Count, maxRecords);
                return history;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定历史记录时发生异常: MaxRecords={MaxRecords}", maxRecords);
                return new List<LockHistoryRecord>();
            }
        }

        /// <summary>
        /// 获取锁定项数量
        /// </summary>
        /// <returns>锁定项数量</returns>
        public int GetLockItemCount()
        {
            try
            {
                var lockInfos = _lockInfoManager.GetAllLockInfo();
                int count = lockInfos?.Count ?? 0;
                
                _logger.LogDebug("获取锁定项数量: Count={Count}", count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取锁定项数量时发生异常");
                return 0;
            }
        }

        /// <summary>
        /// 更新处理器状态
        /// </summary>
        /// <param name="handler">处理器实例</param>
        /// <param name="status">处理器状态</param>
        /// <param name="additionalInfo">附加信息</param>
        public void UpdateHandlerState(ICommandHandler handler, HandlerStatus status, string additionalInfo = null)
        {
            if (handler == null)
            {
                _logger.LogWarning("处理器实例为空，无法更新状态");
                return;
            }

            try
            {
                _lockInfoManager.UpdateHandlerState(handler, status, additionalInfo);
                _logger.LogDebug("更新处理器状态成功: HandlerId={HandlerId}, Status={Status}", 
                    handler.HandlerId, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新处理器状态时发生异常: HandlerId={HandlerId}, Status={Status}", 
                    handler?.HandlerId ?? "Unknown", status);
            }
        }

        /// <summary>
        /// 获取处理器状态
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <returns>处理器状态信息</returns>
        public HandlerStateInfo GetHandlerState(string handlerId)
        {
            // 参数验证
            if (string.IsNullOrEmpty(handlerId))
            {
                _logger.LogWarning("处理器ID为空或无效");
                return null;
            }

            try
            {
                var state = _lockInfoManager.GetHandlerState(handlerId);
                if (state == null)
                {
                    _logger.LogDebug("未找到处理器状态: HandlerId={HandlerId}", handlerId);
                }
                else
                {
                    _logger.LogDebug("获取处理器状态成功: HandlerId={HandlerId}, Status={Status}", 
                        handlerId, state.Status);
                }
                
                return state;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取处理器状态时发生异常: HandlerId={HandlerId}", handlerId);
                return null;
            }
        }

        /// <summary>
        /// 获取所有处理器状态
        /// </summary>
        /// <returns>所有处理器状态列表</returns>
        public List<HandlerStateInfo> GetAllHandlerStates()
        {
            try
            {
                var states = _lockInfoManager.GetAllHandlerStates();
                if (states == null)
                {
                    _logger.LogWarning("获取所有处理器状态返回空值");
                    return new List<HandlerStateInfo>();
                }

                _logger.LogDebug("获取所有处理器状态成功: Count={Count}", states.Count);
                return states;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有处理器状态时发生异常");
                return new List<HandlerStateInfo>();
            }
        }

        /// <summary>
        /// 更新会话状态
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="sessionInfo">会话信息</param>
        public void UpdateSessionState(string sessionId, SessionInfo sessionInfo)
        {
            // 参数验证
            if (string.IsNullOrEmpty(sessionId))
            {
                _logger.LogWarning("会话ID为空或无效");
                return;
            }

            try
            {
                _lockInfoManager.UpdateSessionState(sessionId, sessionInfo);
                _logger.LogInformation("更新会话状态: SessionId={SessionId}, UserId={UserId}", sessionId, sessionInfo?.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新会话状态时发生异常: SessionId={SessionId}", sessionId);
            }
        }

        /// <summary>
        /// 获取会话状态
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话信息</returns>
        public SessionInfo GetSessionState(string sessionId)
        {
            // 参数验证
            if (string.IsNullOrEmpty(sessionId))
            {
                _logger.LogWarning("会话ID为空或无效");
                return null;
            }

            try
            {
                var session = _lockInfoManager.GetSessionState(sessionId);
                if (session == null)
                {
                    _logger.LogDebug("未找到会话状态: SessionId={SessionId}", sessionId);
                }
                else
                {
                    _logger.LogDebug("获取会话状态成功: SessionId={SessionId}, UserId={UserId}", 
                        sessionId, session.UserId);
                }
                
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取会话状态时发生异常: SessionId={SessionId}", sessionId);
                return null;
            }
        }

        /// <summary>
        /// 获取所有会话状态
        /// </summary>
        /// <returns>所有会话状态列表</returns>
        public List<SessionInfo> GetAllSessionStates()
        {
            try
            {
                var sessions = _lockInfoManager.GetAllSessionStates();
                if (sessions == null)
                {
                    _logger.LogWarning("获取所有会话状态返回空值");
                    return new List<SessionInfo>();
                }

                _logger.LogDebug("获取所有会话状态成功: Count={Count}", sessions.Count);
                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取所有会话状态时发生异常");
                return new List<SessionInfo>();
            }
        }
         

        /// <summary>
        /// 获取活跃会话数
        /// </summary>
        public int GetActiveSessionCount()
        {
            return _lockInfoManager.GetActiveSessionCount();
        }

        /// <summary>
        /// 获取会话统计信息
        /// </summary>
        public SessionStatistics GetSessionStatistics()
        {
            return _lockInfoManager.GetSessionStatistics();
        }

        /// <summary>
        /// 获取系统状态报告
        /// </summary>
        public string GetSystemStateReport()
        {
            return _lockInfoManager.GetSystemStateReport();
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

    
}