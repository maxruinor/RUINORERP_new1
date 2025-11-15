// -----------------------------------------------------------------------
// <copyright file="LockAwareActionManager.cs" company="RUINOR Corp.">
//     Copyright (c) RUINOR Corp. 2023. All rights reserved.
// </copyright>
// <author>RUINOR Team</author>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.Common.LogHelper;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Lock;
using RUINORERP.PacketSpec.Models.Lock;
using SqlSugar;
using RUINORERP.Business.CommService;
using RUINORERP.Model.Context;

namespace RUINORERP.Business.Document
{
    /// <summary>
    /// 锁定感知的联动操作管理器
    /// <remarks>
    /// 增强版ActionManager，负责在执行文档联动操作时处理文档锁定状态，确保并发安全。
    /// 此管理器通过在联动操作前后添加锁定和解锁逻辑，防止多个用户同时操作相同文档导致的数据不一致问题。
    /// 主要功能：
    /// 1. 生成唯一操作ID标识每次联动操作
    /// 2. 自动锁定源文档和相关文档
    /// 3. 执行实际的联动操作
    /// 4. 确保操作完成后释放所有锁资源
    /// 5. 提供详细的日志记录和异常处理
    /// </remarks>
    /// </summary>
    public class LockAwareActionManager : ActionManager
    {
        /// <summary>
        /// 文档锁定管理器
        /// <remarks>负责实际的文档锁定操作，支持分布式环境下的锁定机制</remarks>
        /// </summary>
        private readonly IDocumentLockManager _lockManager;

        /// <summary>
        /// 日志记录器
        /// <remarks>用于记录操作过程中的关键事件、错误和调试信息</remarks>
        /// </summary>
        private readonly ILogger<LockAwareActionManager> _logger;

        /// <summary>
        /// 用户上下文服务
        /// <remarks>提供当前操作的用户信息，用于锁定标识和权限验证</remarks>
        /// </summary>
        private readonly ApplicationContext _appContext;

        /// <summary>
        /// 操作锁映射表
        /// <remarks>记录每个操作ID对应的已获取锁列表，用于统一释放资源</remarks>
        /// </summary>
        private readonly Dictionary<long, List<string>> _operationLocks = new Dictionary<long, List<string>>();

        /// <summary>
        /// 构造函数
        /// <remarks>通过依赖注入获取所需服务</remarks>
        /// </summary>
        /// <param name="lockManager">文档锁定管理器</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="appContext">应用上下文</param>
        /// <exception cref="ArgumentNullException">当任何必要的依赖为空时抛出</exception>
        public LockAwareActionManager(
            IDocumentLockManager lockManager,
            DocumentConverterFactory converterFactory,
            ISqlSugarClient db,
            ILogger<LockAwareActionManager> logger,
            IAuditLogService auditLogService,
            ApplicationContext appContext)
            : base(converterFactory, db, logger, auditLogService)
        {
            _lockManager = lockManager ?? throw new ArgumentNullException(nameof(lockManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        }

        /// <summary>
        /// 执行联动操作（带锁定处理）
        /// <remarks>
        /// 此方法是核心功能实现，在执行基类的联动操作前自动添加锁定逻辑，操作完成后自动解锁。
        /// 执行流程：
        /// 1. 生成操作ID标识此次操作
        /// 2. 获取当前用户ID
        /// 3. 锁定相关文档
        /// 4. 执行基类的联动操作
        /// 5. 释放所有锁定资源
        /// 
        /// 安全设计：
        /// - 使用try-finally确保无论操作成功或失败，锁都能被释放
        /// - 详细的日志记录，便于问题排查
        /// - 异常处理和包装，提供更友好的错误信息
        /// </remarks>
        /// </summary>
        /// <param name="actionId">操作ID，标识要执行的联动操作类型</param>
        /// <param name="sourceDoc">源单据数据，作为联动操作的输入</param>
        /// <exception cref="InvalidOperationException">当锁定失败或执行操作时发生错误</exception>
        public override async Task ExecuteActionAsync(string actionId, object sourceDoc)
        {
            long operationId = 0;
            List<string> locksAcquired = new List<string>();
            
            try
            {
                // 生成操作ID
                operationId = GenerateOperationId();
                // 获取用户ID
                long userId = _appContext.CurrentUser?.UserID ?? 0;
                
                _logger.LogInformation("开始执行联动操作: {actionId}, 操作ID: {operationId}, 用户ID: {userId}", 
                    actionId, operationId, userId);
                
                // 锁定相关文档
                await LockRelatedDocumentsAsync(operationId, sourceDoc, locksAcquired, userId);
                
                // 调用基类的非泛型版本方法
                await base.ExecuteActionAsync(actionId, sourceDoc);
                
                _logger.LogInformation("联动操作执行成功: {actionId}", actionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "执行联动操作失败: {actionId}, 错误: {errorMessage}", 
                    actionId, ex.Message);
                throw;
            }
            finally
            {
                // 释放锁定
                await ReleaseOperationLocksAsync(operationId, locksAcquired);
            }
        }

        /// <summary>
        /// 锁定相关文档
        /// <remarks>
        /// 负责对联动操作涉及的文档进行锁定，确保并发安全。
        /// 此方法不仅锁定文档，还维护锁与操作之间的映射关系，并提供异常处理确保在锁定失败时已获取的锁能被释放。
        /// 使用与LockManagerService统一的锁定键格式，确保系统中锁定机制的一致性。
        /// </remarks>
        /// </summary>
        /// <param name="operationId">操作ID</param>
        /// <param name="sourceDoc">源单据</param>
        /// <param name="locksAcquired">已获取的锁列表</param>
        /// <param name="userId">用户ID</param>
        /// <exception cref="InvalidOperationException">当锁定文档失败时抛出</exception>
        private async Task LockRelatedDocumentsAsync(long operationId, object sourceDoc, List<string> locksAcquired, long userId)
        {
            try
            {
                if (sourceDoc is BaseEntity baseEntity)
                {
                    // 使用与LockManagerService统一的锁定键格式：lock:document:{billId}
                    string lockKey = GetLockKey(baseEntity.PrimaryKeyID);
                    bool isLocked = await _lockManager.TryLockAsync(lockKey, userId, operationId);
                    
                    if (isLocked)
                    {
                        locksAcquired.Add(lockKey);
                        
                        // 记录操作锁关系
                        lock (_operationLocks)
                        {
                            if (!_operationLocks.ContainsKey(operationId))
                            {
                                _operationLocks[operationId] = new List<string>();
                            }
                            _operationLocks[operationId].Add(lockKey);
                        }
                        
                        _logger.LogDebug("成功锁定文档: {lockKey}, 操作ID: {operationId}", 
                            lockKey, operationId);
                    }
                    else
                    {
                        // 获取锁定信息，提供更详细的错误信息
                        var lockInfo = await _lockManager.GetLockInfoAsync(lockKey);
                        if (lockInfo != null)
                        {
                            // 使用统一的锁定信息结构，提供更详细的锁定状态信息
                            string errorMsg = string.IsNullOrEmpty(lockInfo.UserName) 
                                ? $"无法锁定文档 {baseEntity.GetType().Name} ID={baseEntity.PrimaryKeyID}，已被用户ID={lockInfo.UserId}锁定" 
                                : $"无法锁定文档 {baseEntity.GetType().Name} ID={baseEntity.PrimaryKeyID}，已被用户{lockInfo.UserName}(ID={lockInfo.UserId})锁定";
                            
                            if (lockInfo.IsExpired())
                            {
                                errorMsg += $"（锁定已于{lockInfo.ExpireTime}过期）";
                            }
                            
                            throw new InvalidOperationException(errorMsg);
                        }
                        else
                        {
                            throw new InvalidOperationException($"无法锁定文档 {baseEntity.GetType().Name} ID={baseEntity.PrimaryKeyID}，锁定失败");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 增强异常处理：确保在锁定过程中出现异常时，已获取的锁也能被释放
                _logger.LogError(ex, "锁定文档失败，操作ID: {operationId}, 错误: {errorMessage}", 
                    operationId, ex.Message);
                
                // 尝试释放已获取的锁
                if (locksAcquired.Count > 0)
                {
                    await ReleaseOperationLocksAsync(operationId, locksAcquired);
                }
                
                throw new InvalidOperationException("锁定文档失败: " + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// 获取锁定键
        /// <remarks>
        /// 生成与LockManagerService统一的锁定键，确保系统中锁定机制的一致性
        /// </remarks>
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定键</returns>
        private string GetLockKey(long billId) => $"lock:document:{billId}";

        /// <summary>
        /// 释放操作相关的所有锁定
        /// <remarks>
        /// 负责释放操作过程中获取的所有锁资源，防止资源泄露。
        /// 此方法确保锁的释放不影响主操作流程，即使释放失败也只记录日志而不抛出异常。
        /// </remarks>
        /// </summary>
        /// <param name="operationId">操作ID</param>
        /// <param name="locksToRelease">需要释放的锁列表</param>
        private async Task ReleaseOperationLocksAsync(long operationId, List<string> locksToRelease)
        {
            if (locksToRelease == null || locksToRelease.Count == 0)
            {
                return;
            }
            
            try
            {
                long userId = _appContext.CurrentUser?.UserID ?? 0; // 确保正确保存和使用用户ID
                
                foreach (var lockKey in locksToRelease)
                {
                    await _lockManager.UnlockAsync(lockKey, userId);
                    
                    _logger.LogDebug("成功释放文档锁: {lockKey}, 操作ID: {operationId}", 
                        lockKey, operationId);
                }
                
                // 清理记录
                lock (_operationLocks)
                {
                    if (_operationLocks.ContainsKey(operationId))
                    {
                        _operationLocks.Remove(operationId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放文档锁失败，操作ID: {operationId}, 错误: {errorMessage}", 
                    operationId, ex.Message);
                // 记录错误但不抛出异常，避免影响主流程
            }
        }

        /// <summary>
        /// 生成操作ID
        /// <remarks>
        /// 使用当前时间的Ticks作为唯一操作ID，确保在分布式环境中的唯一性。
        /// 此方法在每个联动操作开始时调用，用于标识本次操作并关联所有相关的锁资源。
        /// </remarks>
        /// </summary>
        /// <returns>唯一的操作ID</returns>
        private long GenerateOperationId()
        {
            return DateTime.Now.Ticks;
        }
    }
}