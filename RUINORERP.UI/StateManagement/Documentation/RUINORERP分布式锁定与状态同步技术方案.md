# RUINORERP分布式锁定与状态同步技术方案

## 1. 概述

本文档详细描述RUINORERP系统的分布式锁定机制与单据状态同步功能的优化设计方案。针对多用户通过SOCKET进行服务器-客户端交互时可能出现的单据并发编辑冲突问题，设计并实现基于"先编辑先拥有"原则的分布式锁定机制，确保同一时间仅允许一个用户编辑特定单据或基础数据。同时，实现工作台任务清单中各类业务单据状态变更的实时同步机制，当单据状态发生变化时，通过Socket协议将状态更新推送至服务器，并由服务器分发给所有相关用户。

## 2. 现有系统分析

### 2.1 现有分布式锁定机制

通过对现有代码的分析，RUINORERP系统已经实现了基本的分布式锁定机制，但存在以下局限性：

- **锁粒度不足**：现有锁定机制主要基于单个资源ID，缺乏对主子表结构的支持。
- **锁定冲突处理简单**：当多个用户同时申请锁定时，仅返回锁定失败，缺乏智能的冲突解决策略。
- **状态同步不及时**：当资源锁定状态变更时，其他客户端无法实时感知。
- **锁定续期机制不完善**：缺乏基于用户活动的智能锁定续期策略。
- **异常处理机制简单**：缺乏完善的异常处理和失败恢复机制。

## 3. 分布式锁定机制设计

### 3.1 核心原则

采用"先编辑先拥有"的原则，确保同一时间只有一个用户能够编辑特定资源。锁定机制应具备以下特性：

- **互斥性**：确保资源在同一时间只能被一个用户锁定
- **可重入性**：允许同一用户在不同上下文环境下重复获取同一资源的锁定
- **自动续期**：在用户持续编辑时自动延长锁定时间
- **自动释放**：在用户长时间不活动或会话结束时自动释放锁定
- **冲突检测**：能够检测并解决复杂的锁定冲突

### 3.2 Redis分布式锁增强实现

基于现有的Redis分布式锁实现，进行以下增强：

```csharp
public class EnhancedRedisDistributedLock : IDistributedLock
{
    private readonly IRedisClient _redisClient;
    private readonly ILogger<EnhancedRedisDistributedLock> _logger;
    private readonly IDistributedLockOptions _options;
    private readonly IUserActivityTracker _activityTracker;
    
    public EnhancedRedisDistributedLock(
        IRedisClient redisClient,
        ILogger<EnhancedRedisDistributedLock> logger,
        IDistributedLockOptions options,
        IUserActivityTracker activityTracker)
    {
        _redisClient = redisClient;
        _logger = logger;
        _options = options;
        _activityTracker = activityTracker;
    }
    
    public async Task<LockResult> AcquireLockAsync(LockContext context)
    {
        try
        {
            // 生成锁定键
            string lockKey = GenerateLockKey(context.ResourceType, context.ResourceId);
            
            // 检查资源是否已被锁定
            var currentLockInfo = await GetCurrentLockInfoAsync(lockKey);
            
            if (currentLockInfo != null)
            {
                // 检查是否可重入
                if (IsLockReentrant(currentLockInfo, context.UserId, context.SessionId))
                {
                    // 更新锁定信息（增加计数器、延长时间）
                    return await RenewLockAsync(lockKey, currentLockInfo, context);
                }
                
                // 检查锁定是否已过期（处理死锁情况）
                if (IsLockExpired(currentLockInfo))
                {
                    // 尝试强制获取锁定
                    return await ForceAcquireLockAsync(lockKey, context);
                }
                
                // 锁定冲突，返回当前锁定信息
                return new LockResult
                {
                    Success = false,
                    LockId = null,
                    CurrentLockInfo = currentLockInfo,
                    Message = $"资源已被锁定: 用户={currentLockInfo.UserName}, 锁定时间={currentLockInfo.AcquireTime}"
                };
            }
            
            // 创建新的锁定信息
            var lockInfo = new LockInfo
            {
                LockId = Guid.NewGuid().ToString(),
                ResourceId = context.ResourceId,
                ResourceType = context.ResourceType,
                UserId = context.UserId,
                UserName = context.UserName,
                SessionId = context.SessionId,
                AcquireTime = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddSeconds(_options.DefaultLockTimeoutSeconds),
                LockType = context.LockType,
                ReentrantCount = 1
            };
            
            // 序列化锁定信息
            string lockInfoJson = JsonSerializer.Serialize(lockInfo);
            
            // 设置Redis分布式锁（使用SETNX和过期时间）
            bool acquired = await _redisClient.SetAsync(
                lockKey,
                lockInfoJson,
                _options.DefaultLockTimeoutSeconds,
                When.NotExists);
            
            if (acquired)
            {
                // 记录用户活动
                _activityTracker.RecordUserActivity(
                    context.UserId,
                    context.SessionId,
                    context.ResourceType,
                    context.ResourceId,
                    "LockAcquired");
                
                _logger.LogInformation($"成功获取锁定: 资源={context.ResourceType}:{context.ResourceId}, 用户={context.UserName}");
                
                return new LockResult
                {
                    Success = true,
                    LockId = lockInfo.LockId,
                    CurrentLockInfo = lockInfo,
                    Message = "锁定获取成功"
                };
            }
            
            // 锁定获取失败
            return new LockResult
            {
                Success = false,
                LockId = null,
                CurrentLockInfo = await GetCurrentLockInfoAsync(lockKey), // 再次获取最新锁定信息
                Message = "锁定获取失败，请重试"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"获取锁定时发生异常: 资源={context.ResourceType}:{context.ResourceId}");
            throw;
        }
    }
    
    public async Task<bool> ReleaseLockAsync(ReleaseContext context)
    {
        try
        {
            string lockKey = GenerateLockKey(context.ResourceType, context.ResourceId);
            
            // 获取当前锁定信息
            var currentLockInfo = await GetCurrentLockInfoAsync(lockKey);
            
            if (currentLockInfo == null)
            {
                _logger.LogWarning($"尝试释放不存在的锁定: 资源={context.ResourceType}:{context.ResourceId}");
                return true; // 锁定不存在，视为释放成功
            }
            
            // 验证锁定所有权
            if (!IsLockOwner(currentLockInfo, context.UserId, context.SessionId))
            {
                _logger.LogWarning($"尝试释放非本人锁定: 资源={context.ResourceType}:{context.ResourceId}, 用户={context.UserId}");
                return false;
            }
            
            // 检查是否需要减少重入计数
            if (currentLockInfo.ReentrantCount > 1)
            {
                // 减少重入计数，延长锁定时间
                currentLockInfo.ReentrantCount--;
                currentLockInfo.ExpiryTime = DateTime.UtcNow.AddSeconds(_options.DefaultLockTimeoutSeconds);
                
                string updatedLockInfoJson = JsonSerializer.Serialize(currentLockInfo);
                await _redisClient.SetAsync(
                    lockKey,
                    updatedLockInfoJson,
                    _options.DefaultLockTimeoutSeconds);
                
                _logger.LogInformation($"减少锁定重入计数: 资源={context.ResourceType}:{context.ResourceId}, 当前计数={currentLockInfo.ReentrantCount}");
                return true;
            }
            
            // 删除锁定
            bool deleted = await _redisClient.DeleteAsync(lockKey);
            
            if (deleted)
            {
                // 记录用户活动
                _activityTracker.RecordUserActivity(
                    context.UserId,
                    context.SessionId,
                    context.ResourceType,
                    context.ResourceId,
                    "LockReleased");
                
                _logger.LogInformation($"成功释放锁定: 资源={context.ResourceType}:{context.ResourceId}, 用户={context.UserId}");
            }
            else
            {
                _logger.LogWarning($"锁定释放失败: 资源={context.ResourceType}:{context.ResourceId}, 用户={context.UserId}");
            }
            
            return deleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"释放锁定时发生异常: 资源={context.ResourceType}:{context.ResourceId}");
            throw;
        }
    }
    
    public async Task<LockResult> RenewLockAsync(string lockKey, LockInfo currentLockInfo, LockContext context)
    {
        // 更新锁定信息
        currentLockInfo.ExpiryTime = DateTime.UtcNow.AddSeconds(_options.DefaultLockTimeoutSeconds);
        currentLockInfo.ReentrantCount++;
        
        string updatedLockInfoJson = JsonSerializer.Serialize(currentLockInfo);
        
        // 设置回Redis
        await _redisClient.SetAsync(
            lockKey,
            updatedLockInfoJson,
            _options.DefaultLockTimeoutSeconds);
        
        _logger.LogInformation($"锁定续期成功: 资源={context.ResourceType}:{context.ResourceId}, 重入计数={currentLockInfo.ReentrantCount}");
        
        return new LockResult
        {
            Success = true,
            LockId = currentLockInfo.LockId,
            CurrentLockInfo = currentLockInfo,
            Message = "锁定续期成功"
        };
    }
    
    public async Task<LockResult> ForceAcquireLockAsync(string lockKey, LockContext context)
    {
        // 创建新的锁定信息
        var lockInfo = new LockInfo
        {
            LockId = Guid.NewGuid().ToString(),
            ResourceId = context.ResourceId,
            ResourceType = context.ResourceType,
            UserId = context.UserId,
            UserName = context.UserName,
            SessionId = context.SessionId,
            AcquireTime = DateTime.UtcNow,
            ExpiryTime = DateTime.UtcNow.AddSeconds(_options.DefaultLockTimeoutSeconds),
            LockType = context.LockType,
            ReentrantCount = 1
        };
        
        string lockInfoJson = JsonSerializer.Serialize(lockInfo);
        
        // 强制更新锁定（不检查是否存在）
        await _redisClient.SetAsync(
            lockKey,
            lockInfoJson,
            _options.DefaultLockTimeoutSeconds);
        
        _logger.LogWarning($"强制获取过期锁定: 资源={context.ResourceType}:{context.ResourceId}, 用户={context.UserName}");
        
        return new LockResult
        {
            Success = true,
            LockId = lockInfo.LockId,
            CurrentLockInfo = lockInfo,
            Message = "强制获取锁定成功（原锁定已过期）"
        };
    }
    
    public async Task<LockInfo> GetCurrentLockInfoAsync(string lockKey)
    {
        string lockInfoJson = await _redisClient.GetStringAsync(lockKey);
        
        if (string.IsNullOrEmpty(lockInfoJson))
        {
            return null;
        }
        
        try
        {
            return JsonSerializer.Deserialize<LockInfo>(lockInfoJson);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"反序列化锁定信息失败: 键={lockKey}");
            return null;
        }
    }
    
    private string GenerateLockKey(string resourceType, string resourceId)
    {
        return $"lock:{resourceType}:{resourceId}";
    }
    
    private bool IsLockReentrant(LockInfo lockInfo, string userId, string sessionId)
    {
        return lockInfo.UserId == userId && lockInfo.SessionId == sessionId;
    }
    
    private bool IsLockOwner(LockInfo lockInfo, string userId, string sessionId)
    {
        return lockInfo.UserId == userId && lockInfo.SessionId == sessionId;
    }
    
    private bool IsLockExpired(LockInfo lockInfo)
    {
        return DateTime.UtcNow > lockInfo.ExpiryTime;
    }
}
```

### 3.3 锁管理器服务改进

改进锁管理器服务，提供更完善的锁定管理功能：

```csharp
public class EnhancedLockManagerService : ILockManagerService
{
    private readonly IDistributedLock _distributedLock;
    private readonly ILogger<EnhancedLockManagerService> _logger;
    private readonly IEventBus _eventBus;
    private readonly IUserSessionManager _sessionManager;
    private readonly IResourceTypeRegistry _resourceTypeRegistry;
    
    public EnhancedLockManagerService(
        IDistributedLock distributedLock,
        ILogger<EnhancedLockManagerService> logger,
        IEventBus eventBus,
        IUserSessionManager sessionManager,
        IResourceTypeRegistry resourceTypeRegistry)
    {
        _distributedLock = distributedLock;
        _logger = logger;
        _eventBus = eventBus;
        _sessionManager = sessionManager;
        _resourceTypeRegistry = resourceTypeRegistry;
    }
    
    public async Task<LockResponse> HandleLockRequestAsync(LockRequest request)
    {
        try
        {
            // 验证会话有效性
            if (!await _sessionManager.ValidateSessionAsync(request.SessionId))
            {
                return new LockResponse
                {
                    Success = false,
                    Message = "会话无效或已过期"
                };
            }
            
            // 验证资源类型
            if (!_resourceTypeRegistry.IsValidResourceType(request.ResourceType))
            {
                return new LockResponse
                {
                    Success = false,
                    Message = "无效的资源类型"
                };
            }
            
            // 构建锁定上下文
            var lockContext = new LockContext
            {
                ResourceId = request.ResourceId,
                ResourceType = request.ResourceType,
                UserId = request.UserId,
                UserName = request.UserName,
                SessionId = request.SessionId,
                LockType = request.LockType
            };
            
            // 处理主子表锁定关系
            if (_resourceTypeRegistry.IsMasterDetailResource(request.ResourceType))
            {
                // 为主表和所有子表获取锁定
                var lockResults = new List<LockResult>();
                
                // 获取主表锁定
                lockResults.Add(await _distributedLock.AcquireLockAsync(lockContext));
                
                // 如果需要，获取子表锁定
                if (lockResults[0].Success && request.IncludeChildResources)
                {
                    var childResourceTypes = _resourceTypeRegistry.GetChildResourceTypes(request.ResourceType);
                    
                    foreach (var childResourceType in childResourceTypes)
                    {
                        var childLockContext = new LockContext
                        {
                            ResourceId = request.ResourceId, // 使用相同的ID标识主子表关系
                            ResourceType = childResourceType,
                            UserId = request.UserId,
                            UserName = request.UserName,
                            SessionId = request.SessionId,
                            LockType = request.LockType
                        };
                        
                        lockResults.Add(await _distributedLock.AcquireLockAsync(childLockContext));
                    }
                }
                
                // 检查是否所有锁定都成功
                bool allSuccess = lockResults.All(r => r.Success);
                
                if (!allSuccess)
                {
                    // 如果有失败的锁定，释放已获取的锁定
                    await ReleaseAcquiredLocksAsync(lockContext, lockResults.Where(r => r.Success).ToList());
                    
                    return new LockResponse
                    {
                        Success = false,
                        CurrentLockInfo = lockResults.FirstOrDefault(r => !r.Success)?.CurrentLockInfo,
                        Message = "获取部分锁定失败"
                    };
                }
                
                // 发布锁定获取事件
                await PublishLockAcquiredEvent(lockContext, lockResults[0]);
                
                return new LockResponse
                {
                    Success = true,
                    LockId = lockResults[0].LockId,
                    ExpiryTime = lockResults[0].CurrentLockInfo.ExpiryTime,
                    Message = "锁定获取成功"
                };
            }
            else
            {
                // 处理单表锁定
                var lockResult = await _distributedLock.AcquireLockAsync(lockContext);
                
                if (lockResult.Success)
                {
                    // 发布锁定获取事件
                    await PublishLockAcquiredEvent(lockContext, lockResult);
                }
                
                return new LockResponse
                {
                    Success = lockResult.Success,
                    LockId = lockResult.LockId,
                    ExpiryTime = lockResult.Success ? lockResult.CurrentLockInfo.ExpiryTime : DateTime.MinValue,
                    CurrentLockInfo = lockResult.CurrentLockInfo,
                    Message = lockResult.Message
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"处理锁定请求时发生异常: 资源={request.ResourceType}:{request.ResourceId}");
            return new LockResponse
            {
                Success = false,
                Message = $"处理锁定请求失败: {ex.Message}"
            };
        }
    }
    
    public async Task<LockResponse> HandleReleaseLockRequestAsync(UnlockRequest request)
    {
        try
        {
            // 验证会话有效性
            if (!await _sessionManager.ValidateSessionAsync(request.SessionId))
            {
                return new LockResponse
                {
                    Success = false,
                    Message = "会话无效或已过期"
                };
            }
            
            // 构建释放上下文
            var releaseContext = new ReleaseContext
            {
                ResourceId = request.ResourceId,
                ResourceType = request.ResourceType,
                UserId = request.UserId,
                SessionId = request.SessionId,
                LockId = request.LockId
            };
            
            bool success;
            
            // 处理主子表锁定关系
            if (_resourceTypeRegistry.IsMasterDetailResource(request.ResourceType))
            {
                // 释放主表和所有子表锁定
                success = await ReleaseMasterDetailLocksAsync(releaseContext);
            }
            else
            {
                // 释放单表锁定
                success = await _distributedLock.ReleaseLockAsync(releaseContext);
            }
            
            if (success)
            {
                // 发布锁定释放事件
                await _eventBus.PublishAsync(new LockReleasedEvent
                {
                    ResourceId = request.ResourceId,
                    ResourceType = request.ResourceType,
                    UserId = request.UserId,
                    SessionId = request.SessionId,
                    LockId = request.LockId,
                    Timestamp = DateTime.UtcNow
                });
            }
            
            return new LockResponse
            {
                Success = success,
                Message = success ? "锁定释放成功" : "锁定释放失败"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"处理释放锁定请求时发生异常: 资源={request.ResourceType}:{request.ResourceId}");
            return new LockResponse
            {
                Success = false,
                Message = $"处理释放锁定请求失败: {ex.Message}"
            };
        }
    }
    
    public async Task<LockStatus> CheckLockStatusAsync(string resourceId, string resourceType)
    {
        // 实现锁定状态检查逻辑
        // ...
        return new LockStatus();
    }
    
    private async Task<bool> ReleaseMasterDetailLocksAsync(ReleaseContext releaseContext)
    {
        // 释放主表锁定
        bool masterReleased = await _distributedLock.ReleaseLockAsync(releaseContext);
        
        // 释放子表锁定
        var childResourceTypes = _resourceTypeRegistry.GetChildResourceTypes(releaseContext.ResourceType);
        
        foreach (var childResourceType in childResourceTypes)
        {
            var childReleaseContext = new ReleaseContext
            {
                ResourceId = releaseContext.ResourceId,
                ResourceType = childResourceType,
                UserId = releaseContext.UserId,
                SessionId = releaseContext.SessionId,
                LockId = releaseContext.LockId
            };
            
            await _distributedLock.ReleaseLockAsync(childReleaseContext);
        }
        
        return masterReleased;
    }
    
    private async Task ReleaseAcquiredLocksAsync(LockContext context, List<LockResult> acquiredLocks)
    {
        // 释放已获取的锁定
        foreach (var lockResult in acquiredLocks)
        {
            var releaseContext = new ReleaseContext
            {
                ResourceId = context.ResourceId,
                ResourceType = lockResult.CurrentLockInfo.ResourceType,
                UserId = context.UserId,
                SessionId = context.SessionId,
                LockId = lockResult.LockId
            };
            
            await _distributedLock.ReleaseLockAsync(releaseContext);
        }
    }
    
    private async Task PublishLockAcquiredEvent(LockContext context, LockResult lockResult)
    {
        await _eventBus.PublishAsync(new LockAcquiredEvent
        {
            ResourceId = context.ResourceId,
            ResourceType = context.ResourceType,
            UserId = context.UserId,
            UserName = context.UserName,
            SessionId = context.SessionId,
            LockId = lockResult.LockId,
            LockType = context.LockType,
            ExpiryTime = lockResult.CurrentLockInfo.ExpiryTime,
            Timestamp = DateTime.UtcNow
        });
    }
}
```

### 3.4 锁粒度设计

支持多级锁粒度设计，以满足不同场景的需求：

- **资源级锁定**：锁定整个资源（如整个单据）
- **字段级锁定**：锁定资源的特定字段
- **记录级锁定**：锁定资源中的特定记录
- **主子表锁定**：同时锁定主表和相关子表

```csharp
public interface IResourceTypeRegistry
{
    bool IsValidResourceType(string resourceType);
    bool IsMasterDetailResource(string resourceType);
    List<string> GetChildResourceTypes(string parentResourceType);
    ResourceLockGranularity GetLockGranularity(string resourceType);
    bool SupportsFieldLevelLocking(string resourceType);
    List<string> GetLockableFields(string resourceType);
}
```

## 4. 锁定冲突处理与状态同步优化

### 4.1 多级别冲突检测

实现多级别冲突检测机制，支持对不同类型资源的精细冲突检测：

```csharp
public class EnhancedLockConflictDetector : ILockConflictDetector
{
    private readonly IResourceTypeRegistry _resourceTypeRegistry;
    private readonly ILogger<EnhancedLockConflictDetector> _logger;
    
    public EnhancedLockConflictDetector(
        IResourceTypeRegistry resourceTypeRegistry,
        ILogger<EnhancedLockConflictDetector> logger)
    {
        _resourceTypeRegistry = resourceTypeRegistry;
        _logger = logger;
    }
    
    public async Task<ConflictDetectionResult> DetectConflictsAsync(LockRequest request, LockInfo currentLockInfo)
    {
        try
        {
            // 基本冲突检测
            if (currentLockInfo == null)
            {
                return new ConflictDetectionResult
                {
                    HasConflicts = false
                };
            }
            
            // 检查是否为同一用户
            if (currentLockInfo.UserId == request.UserId && currentLockInfo.SessionId == request.SessionId)
            {
                return new ConflictDetectionResult
                {
                    HasConflicts = false,
                    IsReentrant = true
                };
            }
            
            // 根据资源类型进行特定的冲突检测
            ResourceLockGranularity granularity = _resourceTypeRegistry.GetLockGranularity(request.ResourceType);
            
            switch (granularity)
            {
                case ResourceLockGranularity.Resource:
                    // 资源级锁定 - 任何锁定都视为冲突
                    return await DetectResourceLevelConflict(request, currentLockInfo);
                    
                case ResourceLockGranularity.Field:
                    // 字段级锁定 - 仅当锁定相同字段时才视为冲突
                    return await DetectFieldLevelConflict(request, currentLockInfo);
                    
                case ResourceLockGranularity.Record:
                    // 记录级锁定 - 仅当锁定相同记录时才视为冲突
                    return await DetectRecordLevelConflict(request, currentLockInfo);
                    
                case ResourceLockGranularity.MasterDetail:
                    // 主子表锁定 - 需要特殊处理
                    return await DetectMasterDetailConflict(request, currentLockInfo);
                    
                default:
                    _logger.LogWarning($"未知的锁定粒度: {granularity}");
                    return await DetectResourceLevelConflict(request, currentLockInfo);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"检测锁定冲突时发生异常: 资源={request.ResourceType}:{request.ResourceId}");
            return new ConflictDetectionResult
            {
                HasConflicts = true,
                ErrorMessage = ex.Message
            };
        }
    }
    
    private async Task<ConflictDetectionResult> DetectResourceLevelConflict(LockRequest request, LockInfo currentLockInfo)
    {
        return new ConflictDetectionResult
        {
            HasConflicts = true,
            ConflictType = ConflictType.Resource,
            CurrentLockInfo = currentLockInfo,
            ConflictDetails = $"资源 {request.ResourceType}:{request.ResourceId} 已被用户 {currentLockInfo.UserName} 锁定"
        };
    }
    
    private async Task<ConflictDetectionResult> DetectFieldLevelConflict(LockRequest request, LockInfo currentLockInfo)
    {
        // 检查是否都指定了字段信息
        if (string.IsNullOrEmpty(request.LockedFields) || string.IsNullOrEmpty(currentLockInfo.LockedFields))
        {
            // 如果任何一方没有指定字段信息，则视为资源级锁定
            return await DetectResourceLevelConflict(request, currentLockInfo);
        }
        
        // 解析锁定的字段列表
        var requestedFields = ParseFieldList(request.LockedFields);
        var currentFields = ParseFieldList(currentLockInfo.LockedFields);
        
        // 检查是否有重叠的字段
        var conflictingFields = requestedFields.Intersect(currentFields).ToList();
        
        if (conflictingFields.Count > 0)
        {
            return new ConflictDetectionResult
            {
                HasConflicts = true,
                ConflictType = ConflictType.Field,
                CurrentLockInfo = currentLockInfo,
                ConflictingFields = conflictingFields,
                ConflictDetails = $"字段 {string.Join(", ", conflictingFields)} 已被用户 {currentLockInfo.UserName} 锁定"
            };
        }
        
        return new ConflictDetectionResult
        {
            HasConflicts = false,
            NonConflictingFields = requestedFields.Except(currentFields).ToList()
        };
    }
    
    private async Task<ConflictDetectionResult> DetectRecordLevelConflict(LockRequest request, LockInfo currentLockInfo)
    {
        // 检查是否都指定了记录信息
        if (string.IsNullOrEmpty(request.RecordIds) || string.IsNullOrEmpty(currentLockInfo.RecordIds))
        {
            // 如果任何一方没有指定记录信息，则视为资源级锁定
            return await DetectResourceLevelConflict(request, currentLockInfo);
        }
        
        // 解析锁定的记录ID列表
        var requestedRecords = ParseRecordIdList(request.RecordIds);
        var currentRecords = ParseRecordIdList(currentLockInfo.RecordIds);
        
        // 检查是否有重叠的记录
        var conflictingRecords = requestedRecords.Intersect(currentRecords).ToList();
        
        if (conflictingRecords.Count > 0)
        {
            return new ConflictDetectionResult
            {
                HasConflicts = true,
                ConflictType = ConflictType.Record,
                CurrentLockInfo = currentLockInfo,
                ConflictingRecords = conflictingRecords,
                ConflictDetails = $"记录 {string.Join(", ", conflictingRecords)} 已被用户 {currentLockInfo.UserName} 锁定"
            };
        }
        
        return new ConflictDetectionResult
        {
            HasConflicts = false,
            NonConflictingRecords = requestedRecords.Except(currentRecords).ToList()
        };
    }
    
    private async Task<ConflictDetectionResult> DetectMasterDetailConflict(LockRequest request, LockInfo currentLockInfo)
    {
        // 检查主表锁定
        if (currentLockInfo.ResourceType == request.ResourceType && currentLockInfo.ResourceId == request.ResourceId)
        {
            // 主表已被锁定
            return new ConflictDetectionResult
            {
                HasConflicts = true,
                ConflictType = ConflictType.MasterTable,
                CurrentLockInfo = currentLockInfo,
                ConflictDetails = $"主表 {request.ResourceType}:{request.ResourceId} 已被用户 {currentLockInfo.UserName} 锁定"
            };
        }
        
        // 检查是否锁定了相关的子表
        var childResourceTypes = _resourceTypeRegistry.GetChildResourceTypes(request.ResourceType);
        
        if (childResourceTypes.Contains(currentLockInfo.ResourceType) && 
            currentLockInfo.ResourceId == request.ResourceId)
        {
            // 子表已被锁定
            return new ConflictDetectionResult
            {
                HasConflicts = true,
                ConflictType = ConflictType.ChildTable,
                CurrentLockInfo = currentLockInfo,
                ConflictDetails = $"子表 {currentLockInfo.ResourceType}:{currentLockInfo.ResourceId} 已被用户 {currentLockInfo.UserName} 锁定"
            };
        }
        
        return new ConflictDetectionResult
        {
            HasConflicts = false
        };
    }
    
    private List<string> ParseFieldList(string fieldList)
    {
        return fieldList.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(f => f.Trim())
            .ToList();
    }
    
    private List<string> ParseRecordIdList(string recordIdList)
    {
        return recordIdList.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim())
            .ToList();
    }
}
```

### 4.2 智能冲突解决策略

实现智能冲突解决策略，根据资源类型、用户权限和系统负载自动选择最优的冲突解决策略：

```csharp
public class AdaptiveLockConflictResolver : ILockConflictResolver
{
    private readonly ILogger<AdaptiveLockConflictResolver> _logger;
    private readonly IUserPermissionService _permissionService;
    private readonly ISystemLoadMonitor _loadMonitor;
    
    public AdaptiveLockConflictResolver(
        ILogger<AdaptiveLockConflictResolver> logger,
        IUserPermissionService permissionService,
        ISystemLoadMonitor loadMonitor)
    {
        _logger = logger;
        _permissionService = permissionService;
        _loadMonitor = loadMonitor;
    }
    
    public async Task<ConflictResolutionResult> ResolveConflictAsync(LockRequest request, ConflictDetectionResult conflictResult)
    {
        try
        {
            if (!conflictResult.HasConflicts)
            {
                return new ConflictResolutionResult
                {
                    ResolutionStrategy = ResolutionStrategy.None,
                    CanProceed = true
                };
            }
            
            // 获取当前系统负载
            SystemLoadLevel loadLevel = await _loadMonitor.GetCurrentSystemLoadAsync();
            
            // 检查请求用户是否有高级权限
            bool hasAdminPermission = await _permissionService.HasPermissionAsync(
                request.UserId,
                "Lock:ForceAcquire");
            
            // 根据冲突类型和系统状态选择解决策略
            switch (conflictResult.ConflictType)
            {
                case ConflictType.Resource:
                    return await ResolveResourceConflict(request, conflictResult, loadLevel, hasAdminPermission);
                    
                case ConflictType.Field:
                    return await ResolveFieldConflict(request, conflictResult);
                    
                case ConflictType.Record:
                    return await ResolveRecordConflict(request, conflictResult);
                    
                case ConflictType.MasterDetail:
                    return await ResolveMasterDetailConflict(request, conflictResult, hasAdminPermission);
                    
                default:
                    return new ConflictResolutionResult
                    {
                        ResolutionStrategy = ResolutionStrategy.Reject,
                        CanProceed = false,
                        Message = "不支持的冲突类型"
                    };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"解决锁定冲突时发生异常: 资源={request.ResourceType}:{request.ResourceId}");
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.Reject,
                CanProceed = false,
                Message = $"冲突解决失败: {ex.Message}"
            };
        }
    }
    
    private async Task<ConflictResolutionResult> ResolveResourceConflict(
        LockRequest request, 
        ConflictDetectionResult conflictResult, 
        SystemLoadLevel loadLevel, 
        bool hasAdminPermission)
    {
        var currentLockInfo = conflictResult.CurrentLockInfo;
        
        // 检查锁定是否已过期
        if (DateTime.UtcNow > currentLockInfo.ExpiryTime)
        {
            // 锁定已过期，允许获取锁定
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.TakeExpiredLock,
                CanProceed = true,
                Message = "锁定已过期，允许获取锁定"
            };
        }
        
        // 检查锁定持有时间
        TimeSpan lockHoldTime = DateTime.UtcNow - currentLockInfo.AcquireTime;
        
        if (lockHoldTime > TimeSpan.FromMinutes(30) && hasAdminPermission)
        {
            // 如果锁定持有时间超过30分钟且用户有管理员权限，可以强制获取锁定
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.ForceTakeLock,
                CanProceed = true,
                Message = "锁定持有时间过长，管理员可以强制获取锁定"
            };
        }
        
        // 系统负载较低时，可以提供等待选项
        if (loadLevel == SystemLoadLevel.Low)
        {
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.Wait,
                CanProceed = true,
                Message = "资源当前被锁定，可以等待锁定释放",
                WaitTimeout = TimeSpan.FromSeconds(30)
            };
        }
        
        // 其他情况，拒绝请求
        return new ConflictResolutionResult
        {
            ResolutionStrategy = ResolutionStrategy.Reject,
            CanProceed = false,
            Message = $"资源 {request.ResourceType}:{request.ResourceId} 已被用户 {currentLockInfo.UserName} 锁定，锁定将在 {currentLockInfo.ExpiryTime} 过期",
            CurrentLockInfo = currentLockInfo
        };
    }
    
    private async Task<ConflictResolutionResult> ResolveFieldConflict(
        LockRequest request, 
        ConflictDetectionResult conflictResult)
    {
        // 字段级冲突可以部分授权
        if (conflictResult.NonConflictingFields != null && conflictResult.NonConflictingFields.Count > 0)
        {
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.PartialPermission,
                CanProceed = true,
                Message = "部分字段可锁定",
                AllowedFields = conflictResult.NonConflictingFields,
                CurrentLockInfo = conflictResult.CurrentLockInfo
            };
        }
        
        // 所有字段都有冲突
        return new ConflictResolutionResult
        {
            ResolutionStrategy = ResolutionStrategy.Reject,
            CanProceed = false,
            Message = $"请求的所有字段已被用户 {conflictResult.CurrentLockInfo.UserName} 锁定",
            CurrentLockInfo = conflictResult.CurrentLockInfo
        };
    }
    
    private async Task<ConflictResolutionResult> ResolveRecordConflict(
        LockRequest request, 
        ConflictDetectionResult conflictResult)
    {
        // 记录级冲突可以部分授权
        if (conflictResult.NonConflictingRecords != null && conflictResult.NonConflictingRecords.Count > 0)
        {
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.PartialPermission,
                CanProceed = true,
                Message = "部分记录可锁定",
                AllowedRecords = conflictResult.NonConflictingRecords,
                CurrentLockInfo = conflictResult.CurrentLockInfo
            };
        }
        
        // 所有记录都有冲突
        return new ConflictResolutionResult
        {
            ResolutionStrategy = ResolutionStrategy.Reject,
            CanProceed = false,
            Message = $"请求的所有记录已被用户 {conflictResult.CurrentLockInfo.UserName} 锁定",
            CurrentLockInfo = conflictResult.CurrentLockInfo
        };
    }
    
    private async Task<ConflictResolutionResult> ResolveMasterDetailConflict(
        LockRequest request, 
        ConflictDetectionResult conflictResult, 
        bool hasAdminPermission)
    {
        var currentLockInfo = conflictResult.CurrentLockInfo;
        
        // 检查锁定是否已过期
        if (DateTime.UtcNow > currentLockInfo.ExpiryTime)
        {
            // 锁定已过期，允许获取锁定
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.TakeExpiredLock,
                CanProceed = true,
                Message = "锁定已过期，允许获取锁定"
            };
        }
        
        // 管理员可以强制获取锁定
        if (hasAdminPermission)
        {
            return new ConflictResolutionResult
            {
                ResolutionStrategy = ResolutionStrategy.ForceTakeLock,
                CanProceed = true,
                Message = "管理员可以强制获取锁定"
            };
        }
        
        // 其他情况，拒绝请求
        return new ConflictResolutionResult
        {
            ResolutionStrategy = ResolutionStrategy.Reject,
            CanProceed = false,
            Message = $"{GetConflictTypeText(conflictResult.ConflictType)} 已被用户 {currentLockInfo.UserName} 锁定",
            CurrentLockInfo = currentLockInfo
        };
    }
    
    private string GetConflictTypeText(ConflictType conflictType)
    {
        switch (conflictType)
        {
            case ConflictType.MasterTable:
                return "主表";
            case ConflictType.ChildTable:
                return "子表";
            default:
                return "资源";
        }
    }
}
```

### 4.3 智能锁定续期

实现基于用户活动感知的智能锁定续期机制：

```csharp
public class SmartLockRenewer : ILockRenewer
{
    private readonly IDistributedLock _distributedLock;
    private readonly IUserActivityTracker _activityTracker;
    private readonly ILogger<SmartLockRenewer> _logger;
    private readonly ConcurrentDictionary<string, LockRenewalInfo> _activeRenewals;
    private readonly Timer _activityCheckTimer;
    private readonly TimeSpan _activityThreshold = TimeSpan.FromMinutes(5);
    
    public SmartLockRenewer(
        IDistributedLock distributedLock,
        IUserActivityTracker activityTracker,
        ILogger<SmartLockRenewer> logger)
    {
        _distributedLock = distributedLock;
        _activityTracker = activityTracker;
        _logger = logger;
        _activeRenewals = new ConcurrentDictionary<string, LockRenewalInfo>();
        
        // 初始化活动检查定时器，每30秒检查一次
        _activityCheckTimer = new Timer(CheckUserActivity, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
    }
    
    public void RegisterLockForRenewal(string lockId, string resourceId, string resourceType, string userId, string sessionId, TimeSpan initialExpiry)
    {
        try
        {
            var renewalInfo = new LockRenewalInfo
            {
                LockId = lockId,
                ResourceId = resourceId,
                ResourceType = resourceType,
                UserId = userId,
                SessionId = sessionId,
                LastRenewalTime = DateTime.UtcNow,
                ExpectedExpiryTime = DateTime.UtcNow + initialExpiry,
                LastActivityTime = DateTime.UtcNow
            };
            
            // 注册到活动续期列表
            _activeRenewals[lockId] = renewalInfo;
            
            _logger.LogInformation($"注册锁定续期: 资源={resourceType}:{resourceId}, 锁定ID={lockId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"注册锁定续期失败: 资源={resourceType}:{resourceId}, 锁定ID={lockId}");
        }
    }
    
    public void UnregisterLockForRenewal(string lockId)
    {
        try
        {
            if (_activeRenewals.TryRemove(lockId, out var renewalInfo))
            {
                _logger.LogInformation($"取消锁定续期: 资源={renewalInfo.ResourceType}:{renewalInfo.ResourceId}, 锁定ID={lockId}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"取消锁定续期失败: 锁定ID={lockId}");
        }
    }
    
    public async Task<RenewalStatus> ManualRenewLockAsync(string lockId, string userId, string sessionId)
    {
        try
        {
            if (!_activeRenewals.TryGetValue(lockId, out var renewalInfo))
            {
                return new RenewalStatus
                {
                    Success = false,
                    Message = "未找到锁定续期信息"
                };
            }
            
            // 验证用户身份
            if (renewalInfo.UserId != userId || renewalInfo.SessionId != sessionId)
            {
                return new RenewalStatus
                {
                    Success = false,
                    Message = "无权续期此锁定"
                };
            }
            
            // 执行锁定续期
            bool renewed = await RenewLockInternalAsync(renewalInfo);
            
            if (renewed)
            {
                renewalInfo.LastRenewalTime = DateTime.UtcNow;
                renewalInfo.LastActivityTime = DateTime.UtcNow;
                renewalInfo.ExpectedExpiryTime = DateTime.UtcNow + TimeSpan.FromSeconds(600); // 续期10分钟
                
                _activeRenewals[lockId] = renewalInfo;
                
                return new RenewalStatus
                {
                    Success = true,
                    Message = "锁定续期成功",
                    NewExpiryTime = renewalInfo.ExpectedExpiryTime
                };
            }
            
            return new RenewalStatus
            {
                Success = false,
                Message = "锁定续期失败"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"手动续期锁定时发生异常: 锁定ID={lockId}");
            return new RenewalStatus
            {
                Success = false,
                Message = $"续期失败: {ex.Message}"
            };
        }
    }
    
    public void UpdateUserActivity(string lockId)
    {
        try
        {
            if (_activeRenewals.TryGetValue(lockId, out var renewalInfo))
            {
                renewalInfo.LastActivityTime = DateTime.UtcNow;
                _activeRenewals[lockId] = renewalInfo;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"更新用户活动时发生异常: 锁定ID={lockId}");
        }
    }
    
    private void CheckUserActivity(object state)
    {
        try
        {
            var now = DateTime.UtcNow;
            var expiredRenewals = new List<string>();
            
            foreach (var renewalInfo in _activeRenewals.Values)
            {
                // 检查用户活动
                TimeSpan timeSinceLastActivity = now - renewalInfo.LastActivityTime;
                
                if (timeSinceLastActivity > _activityThreshold)
                {
                    // 用户长时间不活动，停止续期
                    _logger.LogInformation($"用户长时间不活动，停止续期: 资源={renewalInfo.ResourceType}:{renewalInfo.ResourceId}, 用户={renewalInfo.UserId}, 最后活动时间={renewalInfo.LastActivityTime}");
                    expiredRenewals.Add(renewalInfo.LockId);
                }
                else
                {
                    // 检查是否需要续期
                    TimeSpan timeUntilExpiry = renewalInfo.ExpectedExpiryTime - now;
                    
                    if (timeUntilExpiry < TimeSpan.FromMinutes(3))
                    {
                        // 自动续期
                        _ = Task.Run(async () => 
                        {
                            try
                            {
                                await RenewLockInternalAsync(renewalInfo);
                                
                                // 更新续期信息
                                renewalInfo.LastRenewalTime = now;
                                renewalInfo.ExpectedExpiryTime = now + TimeSpan.FromSeconds(600); // 续期10分钟
                                _activeRenewals[renewalInfo.LockId] = renewalInfo;
                                
                                _logger.LogInformation($"自动续期成功: 资源={renewalInfo.ResourceType}:{renewalInfo.ResourceId}, 锁定ID={renewalInfo.LockId}");
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"自动续期失败: 资源={renewalInfo.ResourceType}:{renewalInfo.ResourceId}, 锁定ID={renewalInfo.LockId}");
                                expiredRenewals.Add(renewalInfo.LockId);
                            }
                        });
                    }
                }
            }
            
            // 移除过期的续期
            foreach (var lockId in expiredRenewals)
            {
                _activeRenewals.TryRemove(lockId, out _);
                
                // 通知用户锁定可能丢失
                // 实现通知逻辑
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "检查用户活动时发生异常");
        }
    }
    
    private async Task<bool> RenewLockInternalAsync(LockRenewalInfo renewalInfo)
    {
        try
        {
            // 构建锁定上下文
            var lockContext = new LockContext
            {
                ResourceId = renewalInfo.ResourceId,
                ResourceType = renewalInfo.ResourceType,
                UserId = renewalInfo.UserId,
                SessionId = renewalInfo.SessionId
            };
            
            // 执行锁定续期（通过重新获取锁定实现）
            var lockResult = await _distributedLock.AcquireLockAsync(lockContext);
            return lockResult.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"续期锁定时发生异常: 资源={renewalInfo.ResourceType}:{renewalInfo.ResourceId}");
            return false;
        }
    }
    
    private class LockRenewalInfo
    {
        public string LockId { get; set; }
        public string ResourceId { get; set; }
        public string ResourceType { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public DateTime LastRenewalTime { get; set; }
        public DateTime ExpectedExpiryTime { get; set; }
        public DateTime LastActivityTime { get; set; }
    }
}
```

## 5. 工作台任务清单实时状态同步机制

### 5.1 架构设计

工作台任务清单的实时状态同步架构如下：

1. **服务端状态变更检测**：监听单据状态变更事件
2. **状态变更消息队列**：使用消息队列解耦状态变更和通知处理
3. **WebSocket实时推送**：通过WebSocket将状态变更推送给客户端
4. **客户端状态更新**：客户端接收状态更新并刷新UI

### 5.2 状态同步流程

1. 当单据状态发生变化时（如等待审核、等待归还等），通过文档处理模块触发状态变更事件
2. 状态变更处理服务接收事件，生成状态更新消息
3. 消息通过SupperScoket协议序列化后发送到消息队列
4. 推送服务从消息队列获取状态更新，通过WebSocket推送给相关客户端
5. 客户端接收状态更新，更新本地任务清单显示

### 5.3 Socket协议扩展

扩展Socket协议，支持任务状态同步：

```csharp
public class TaskStatusUpdateMessage : MessageBase
{
    public TaskStatusUpdateMessage()
    {
        MessageType = MessageType.TaskStatusUpdate;
    }
    
    public string TaskId { get; set; }
    public string DocumentId { get; set; }
    public string DocumentType { get; set; }
    public string CurrentStatus { get; set; }
    public string PreviousStatus { get; set; }
    public DateTime StatusChangeTime { get; set; }
    public string ChangedBy { get; set; }
    public Dictionary<string, object> AdditionalInfo { get; set; } = new Dictionary<string, object>();
}
```

### 5.4 服务端实现

```csharp
public class WorkbenchTaskSyncService : IWorkbenchTaskSyncService
{
    private readonly IEventBus _eventBus;
    private readonly IMessageQueue _messageQueue;
    private readonly ILogger<WorkbenchTaskSyncService> _logger;
    private readonly IUserNotificationService _notificationService;
    
    public WorkbenchTaskSyncService(
        IEventBus eventBus,
        IMessageQueue messageQueue,
        ILogger<WorkbenchTaskSyncService> logger,
        IUserNotificationService notificationService)
    {
        _eventBus = eventBus;
        _messageQueue = messageQueue;
        _logger = logger;
        _notificationService = notificationService;
        
        // 订阅文档状态变更事件
        _eventBus.Subscribe<DocumentStatusChangedEvent>(HandleDocumentStatusChanged);
        // 订阅工作流状态变更事件
        _eventBus.Subscribe<WorkflowStatusChangedEvent>(HandleWorkflowStatusChanged);
    }
    
    private async Task HandleDocumentStatusChanged(DocumentStatusChangedEvent @event)
    {
        try
        {
            // 构建任务状态更新消息
            var updateMessage = new TaskStatusUpdateMessage
            {
                TaskId = @event.DocumentId,
                DocumentId = @event.DocumentId,
                DocumentType = @event.DocumentType,
                CurrentStatus = @event.NewStatus,
                PreviousStatus = @event.OldStatus,
                StatusChangeTime = @event.Timestamp,
                ChangedBy = @event.UserName
            };
            
            // 添加额外信息
            if (!string.IsNullOrEmpty(@event.WorkflowInstanceId))
            {
                updateMessage.AdditionalInfo["WorkflowInstanceId"] = @event.WorkflowInstanceId;
            }
            if (!string.IsNullOrEmpty(@event.NextApproverId))
            {
                updateMessage.AdditionalInfo["NextApproverId"] = @event.NextApproverId;
                updateMessage.AdditionalInfo["NextApproverName"] = @event.NextApproverName;
            }
            
            // 序列化消息
            string messageJson = JsonSerializer.Serialize(updateMessage);
            
            // 发送到消息队列
            await _messageQueue.PublishAsync("TaskStatusUpdates", messageJson);
            
            // 记录日志
            _logger.LogInformation($"文档状态更新消息已发送: 文档ID={@event.DocumentId}, 状态={@event.NewStatus}");
            
            // 发送用户通知（如果需要）
            if (!string.IsNullOrEmpty(@event.NextApproverId))
            {
                await _notificationService.SendNotificationAsync(
                    @event.NextApproverId,
                    $"有新的待审批任务: {GetDocumentTypeName(@event.DocumentType)} #{@event.DocumentNumber}",
                    NotificationType.TaskAssigned,
                    new Dictionary<string, string>
                    {
                        { "DocumentId", @event.DocumentId },
                        { "DocumentType", @event.DocumentType },
                        { "WorkflowInstanceId", @event.WorkflowInstanceId }
                    });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"处理文档状态变更事件时发生异常: 文档ID={@event.DocumentId}");
        }
    }
    
    private async Task HandleWorkflowStatusChanged(WorkflowStatusChangedEvent @event)
    {
        try
        {
            // 构建任务状态更新消息
            var updateMessage = new TaskStatusUpdateMessage
            {
                TaskId = @event.WorkflowInstanceId,
                DocumentId = @event.DocumentId,
                DocumentType = @event.DocumentType,
                CurrentStatus = @event.NewStatus,
                PreviousStatus = @event.OldStatus,
                StatusChangeTime = @event.Timestamp,
                ChangedBy = @event.UserName
            };
            
            // 添加工作流相关信息
            updateMessage.AdditionalInfo["WorkflowInstanceId"] = @event.WorkflowInstanceId;
            updateMessage.AdditionalInfo["CurrentStepId"] = @event.CurrentStepId;
            updateMessage.AdditionalInfo["CurrentStepName"] = @event.CurrentStepName;
            
            if (!string.IsNullOrEmpty(@event.NextApproverId))
            {
                updateMessage.AdditionalInfo["NextApproverId"] = @event.NextApproverId;
                updateMessage.AdditionalInfo["NextApproverName"] = @event.NextApproverName;
            }
            
            // 序列化消息
            string messageJson = JsonSerializer.Serialize(updateMessage);
            
            // 发送到消息队列
            await _messageQueue.PublishAsync("TaskStatusUpdates", messageJson);
            
            _logger.LogInformation($"工作流状态更新消息已发送: 工作流ID={@event.WorkflowInstanceId}, 状态={@event.NewStatus}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"处理工作流状态变更事件时发生异常: 工作流ID={@event.WorkflowInstanceId}");
        }
    }
    
    private string GetDocumentTypeName(string documentType)
    {
        // 根据文档类型获取显示名称
        // 实现略
        return documentType;
    }
}
```

### 5.5 客户端实现

```csharp
public class WorkbenchTaskSyncManager : IWorkbenchTaskSyncManager, IDisposable
{
    private readonly ISocketClient _socketClient;
    private readonly ITaskListManager _taskListManager;
    private readonly ILogger<WorkbenchTaskSyncManager> _logger;
    private readonly ConcurrentDictionary<string, TaskInfo> _taskCache;
    private bool _isInitialized;
    
    public event EventHandler<TaskStatusChangedEventArgs> TaskStatusChanged;
    public event EventHandler<TaskAddedEventArgs> TaskAdded;
    public event EventHandler<TaskRemovedEventArgs> TaskRemoved;
    
    public WorkbenchTaskSyncManager(
        ISocketClient socketClient,
        ITaskListManager taskListManager,
        ILogger<WorkbenchTaskSyncManager> logger)
    {
        _socketClient = socketClient;
        _taskListManager = taskListManager;
        _logger = logger;
        _taskCache = new ConcurrentDictionary<string, TaskInfo>();
    }
    
    public async Task InitializeAsync()
    {
        if (_isInitialized)
            return;
        
        try
        {
            // 订阅Socket消息
            _socketClient.OnMessageReceived += HandleSocketMessage;
            
            // 加载初始任务数据
            await LoadInitialTasksAsync();
            
            _isInitialized = true;
            _logger.LogInformation("工作台任务同步管理器初始化成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "工作台任务同步管理器初始化失败");
            throw;
        }
    }
    
    public async Task LoadInitialTasksAsync()
    {
        try
        {
            // 从服务端加载初始任务列表
            var tasks = await _taskListManager.GetUserTasksAsync();
            
            // 更新缓存
            foreach (var task in tasks)
            {
                _taskCache[task.TaskId] = task;
            }
            
            _logger.LogInformation($"成功加载 {tasks.Count} 个初始任务");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "加载初始任务列表失败");
            throw;
        }
    }
    
    private async void HandleSocketMessage(object sender, MessageReceivedEventArgs e)
    {
        try
        {
            if (e.MessageType == MessageType.TaskStatusUpdate)
            {
                // 解析任务状态更新消息
                var updateMessage = JsonSerializer.Deserialize<TaskStatusUpdateMessage>(e.MessageData);
                
                await ProcessTaskStatusUpdateAsync(updateMessage);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理Socket消息时发生异常");
        }
    }
    
    private async Task ProcessTaskStatusUpdateAsync(TaskStatusUpdateMessage updateMessage)
    {
        // 检查任务是否已存在
        if (_taskCache.TryGetValue(updateMessage.TaskId, out var existingTask))
        {
            // 更新现有任务
            string oldStatus = existingTask.Status;
            
            // 更新任务属性
            existingTask.Status = updateMessage.CurrentStatus;
            existingTask.StatusChangeTime = updateMessage.StatusChangeTime;
            existingTask.ChangedBy = updateMessage.ChangedBy;
            
            // 更新额外信息
            foreach (var kvp in updateMessage.AdditionalInfo)
            {
                existingTask.AdditionalInfo[kvp.Key] = kvp.Value;
            }
            
            // 更新缓存
            _taskCache[updateMessage.TaskId] = existingTask;
            
            // 触发状态变更事件
            OnTaskStatusChanged(new TaskStatusChangedEventArgs
            {
                Task = existingTask,
                OldStatus = oldStatus,
                NewStatus = updateMessage.CurrentStatus
            });
            
            _logger.LogInformation($"任务状态已更新: 任务ID={updateMessage.TaskId}, 旧状态={oldStatus}, 新状态={updateMessage.CurrentStatus}");
        }
        else
        {
            // 检查是否需要添加新任务
            bool shouldAddTask = await ShouldAddNewTaskAsync(updateMessage);
            
            if (shouldAddTask)
            {
                // 获取完整的任务信息
                var newTask = await _taskListManager.GetTaskByIdAsync(updateMessage.TaskId);
                
                if (newTask != null)
                {
                    // 添加到缓存
                    _taskCache[updateMessage.TaskId] = newTask;
                    
                    // 触发任务添加事件
                    OnTaskAdded(new TaskAddedEventArgs { Task = newTask });
                    
                    _logger.LogInformation($"新任务已添加: 任务ID={updateMessage.TaskId}, 状态={updateMessage.CurrentStatus}");
                }
            }
        }
    }
    
    private async Task<bool> ShouldAddNewTaskAsync(TaskStatusUpdateMessage updateMessage)
    {
        // 判断是否应该将任务添加到当前用户的任务列表
        // 例如，检查是否是分配给当前用户的审批任务
        // 实现略
        return true;
    }
    
    public void Dispose()
    {
        // 取消订阅事件
        _socketClient.OnMessageReceived -= HandleSocketMessage;
    }
    
    protected virtual void OnTaskStatusChanged(TaskStatusChangedEventArgs e)
    {
        TaskStatusChanged?.Invoke(this, e);
    }
    
    protected virtual void OnTaskAdded(TaskAddedEventArgs e)
    {
        TaskAdded?.Invoke(this, e);
    }
    
    protected virtual void OnTaskRemoved(TaskRemovedEventArgs e)
    {
        TaskRemoved?.Invoke(this, e);
    }
}
```

## 6. 文档处理模块集成

### 6.1 与文档转换器的集成

扩展文档转换器，在文档转换过程中正确处理锁定状态：

```csharp
public class LockAwareDocumentConverterBase : DocumentConverterBase
{
    protected readonly ILockManagerService _lockManagerService;
    protected readonly ILogger<LockAwareDocumentConverterBase> _logger;
    
    public LockAwareDocumentConverterBase(
        ILockManagerService lockManagerService,
        ILogger<LockAwareDocumentConverterBase> logger)
    {
        _lockManagerService = lockManagerService;
        _logger = logger;
    }
    
    public override async Task<ConversionResult> ConvertAsync(ConversionContext context)
    {
        // 检查源文档锁定状态
        var sourceLockStatus = await _lockManagerService.CheckLockStatusAsync(
            context.SourceDocumentId,
            context.SourceDocumentType);
        
        if (sourceLockStatus.IsLocked && sourceLockStatus.CurrentLockInfo.UserId != context.UserId)
        {
            return new ConversionResult
            {
                Success = false,
                ErrorMessage = $"源文档已被用户 {sourceLockStatus.CurrentLockInfo.UserName} 锁定，无法进行转换",
                ErrorCode = "DOCUMENT_LOCKED"
            };
        }
        
        try
        {
            // 尝试锁定源文档
            await LockSourceDocumentAsync(context);
            
            // 执行转换
            var result = await PerformConversionAsync(context);
            
            if (result.Success && result.TargetDocumentId != null)
            {
                // 如果转换成功且生成了目标文档，更新任务状态
                await NotifyTaskStatusChangedAsync(context, result);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"文档转换失败: 源文档ID={context.SourceDocumentId}, 类型={context.SourceDocumentType}");
            return new ConversionResult
            {
                Success = false,
                ErrorMessage = $"文档转换失败: {ex.Message}",
                ErrorCode = "CONVERSION_FAILED"
            };
        }
        finally
        {
            // 释放源文档锁定
            await UnlockSourceDocumentAsync(context);
        }
    }
    
    protected virtual async Task LockSourceDocumentAsync(ConversionContext context)
    {
        var lockRequest = new LockRequest
        {
            ResourceId = context.SourceDocumentId,
            ResourceType = context.SourceDocumentType,
            UserId = context.UserId,
            UserName = context.UserName,
            SessionId = context.SessionId,
            LockType = LockType.Conversion
        };
        
        var lockResponse = await _lockManagerService.HandleLockRequestAsync(lockRequest);
        
        if (!lockResponse.Success && lockResponse.CurrentLockInfo != null)
        {
            throw new DocumentLockedException(
                context.SourceDocumentId,
                context.SourceDocumentType,
                lockResponse.CurrentLockInfo.UserId,
                lockResponse.CurrentLockInfo.UserName);
        }
    }
    
    protected virtual async Task UnlockSourceDocumentAsync(ConversionContext context)
    {
        var unlockRequest = new UnlockRequest
        {
            ResourceId = context.SourceDocumentId,
            ResourceType = context.SourceDocumentType,
            UserId = context.UserId,
            SessionId = context.SessionId
        };
        
        await _lockManagerService.HandleReleaseLockRequestAsync(unlockRequest);
    }
    
    protected virtual async Task NotifyTaskStatusChangedAsync(ConversionContext context, ConversionResult result)
    {
        try
        {
            // 构建任务状态更新事件
            var statusChangedEvent = new DocumentStatusChangedEvent
            {
                DocumentId = context.SourceDocumentId,
                DocumentType = context.SourceDocumentType,
                NewStatus = "Converted",
                OldStatus = context.SourceDocumentStatus,
                Timestamp = DateTime.UtcNow,
                UserId = context.UserId,
                UserName = context.UserName,
                TargetDocumentId = result.TargetDocumentId,
                TargetDocumentType = GetTargetDocumentType(context)
            };
            
            // 发布事件
            // 这里需要注入IEventBus
            // await _eventBus.PublishAsync(statusChangedEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"通知任务状态变更失败: 文档ID={context.SourceDocumentId}");
        }
    }
    
    protected virtual string GetTargetDocumentType(ConversionContext context)
    {
        // 根据转换上下文获取目标文档类型
        // 实现略
        return string.Empty;
    }
}
```

### 6.2 与ActionManager的集成

扩展ActionManager，在执行联动操作时正确处理锁定状态：

```csharp
public class LockAwareActionManager : ActionManager
{
    private readonly ILockManagerService _lockManagerService;
    private readonly ILogger<LockAwareActionManager> _logger;
    private readonly ConcurrentDictionary<string, List<string>> _operationLocks;
    
    public LockAwareActionManager(
        ILockManagerService lockManagerService,
        ILogger<LockAwareActionManager> logger,
        // 注入其他依赖...
        ) : base(
            // 传递基础依赖...
            )
    {
        _lockManagerService = lockManagerService;
        _logger = logger;
        _operationLocks = new ConcurrentDictionary<string, List<string>>();
    }
    
    public override async Task<ActionResult> ExecuteActionAsync(ActionContext actionContext, CancellationToken cancellationToken = default)
    {
        string operationId = Guid.NewGuid().ToString();
        var acquiredLocks = new List<string>();
        
        try
        {
            // 记录操作ID
            _operationLocks[operationId] = acquiredLocks;
            
            // 在执行操作前，锁定相关文档
            await LockRelatedDocumentsAsync(actionContext, acquiredLocks);
            
            // 执行原始操作
            return await base.ExecuteActionAsync(actionContext, cancellationToken);
        }
        catch (DocumentLockedException ex)
        {
            _logger.LogWarning(ex, $"文档锁定异常: 文档={ex.DocumentType}:{ex.DocumentId}, 用户={ex.LockedByUserId}");
            return new ActionResult
            {
                Success = false,
                ErrorMessage = $"操作无法执行，{ex.DocumentType} {ex.DocumentId} 已被用户 {ex.LockedByUserName} 锁定",
                ErrorCode = "DOCUMENT_LOCKED"
            };
        }
        finally
        {
            // 释放所有获取的锁定
            await ReleaseOperationLocksAsync(operationId);
        }
    }
    
    private async Task LockRelatedDocumentsAsync(ActionContext actionContext, List<string> acquiredLocks)
    {
        try
        {
            // 获取需要锁定的文档列表
            var documentsToLock = await GetDocumentsToLockAsync(actionContext);
            
            foreach (var document in documentsToLock)
            {
                var lockRequest = new LockRequest
                {
                    ResourceId = document.DocumentId,
                    ResourceType = document.DocumentType,
                    UserId = actionContext.UserId,
                    UserName = actionContext.UserName,
                    SessionId = actionContext.SessionId,
                    LockType = LockType.ActionExecution
                };
                
                var lockResponse = await _lockManagerService.HandleLockRequestAsync(lockRequest);
                
                if (!lockResponse.Success)
                {
                    if (lockResponse.CurrentLockInfo != null)
                    {
                        // 文档已被锁定，抛出异常
                        throw new DocumentLockedException(
                            document.DocumentId,
                            document.DocumentType,
                            lockResponse.CurrentLockInfo.UserId,
                            lockResponse.CurrentLockInfo.UserName);
                    }
                    else
                    {
                        // 其他锁定失败情况
                        throw new LockOperationException(
                            document.DocumentId,
                            document.DocumentType,
                            lockResponse.Message);
                    }
                }
                
                // 记录已获取的锁定
                acquiredLocks.Add($"{document.DocumentType}:{document.DocumentId}");
            }
        }
        catch (Exception ex)
        {
            // 发生异常，释放已获取的锁定
            foreach (var lockInfo in acquiredLocks)
            {
                var parts = lockInfo.Split(':');
                if (parts.Length == 2)
                {
                    await _lockManagerService.HandleReleaseLockRequestAsync(new UnlockRequest
                    {
                        ResourceId = parts[1],
                        ResourceType = parts[0],
                        UserId = actionContext.UserId,
                        SessionId = actionContext.SessionId
                    });
                }
            }
            
            throw;
        }
    }
    
    private async Task ReleaseOperationLocksAsync(string operationId)
    {
        try
        {
            if (_operationLocks.TryRemove(operationId, out var acquiredLocks))
            {
                foreach (var lockInfo in acquiredLocks)
                {
                    var parts = lockInfo.Split(':');
                    if (parts.Length == 2)
                    {
                        try
                        {
                            // 释放锁定（忽略错误）
                            await _lockManagerService.HandleReleaseLockRequestAsync(new UnlockRequest
                            {
                                ResourceId = parts[1],
                                ResourceType = parts[0],
                                UserId = string.Empty, // 这里需要改进，保存操作的用户ID
                                SessionId = string.Empty
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"释放锁定失败: {lockInfo}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"释放操作锁定时发生异常: 操作ID={operationId}");
        }
    }
    
    private async Task<List<DocumentReference>> GetDocumentsToLockAsync(ActionContext actionContext)
    {
        // 获取需要锁定的文档列表
        // 根据操作类型和上下文确定
        // 实现略
        return new List<DocumentReference>();
    }
}
```

## 7. 异常处理策略

### 7.1 异常类型定义

定义专门的异常类型，用于处理分布式锁定相关的异常：

```csharp
public class LockOperationException : Exception
{
    public string ResourceId { get; }
    public string ResourceType { get; }
    public string OperationType { get; }
    
    public LockOperationException(string resourceId, string resourceType, string message)
        : base(message)
    {
        ResourceId = resourceId;
        ResourceType = resourceType;
        OperationType = "LockOperation";
    }
    
    public LockOperationException(string resourceId, string resourceType, string message, Exception innerException)
        : base(message, innerException)
    {
        ResourceId = resourceId;
        ResourceType = resourceType;
        OperationType = "LockOperation";
    }
}

public class DocumentLockedException : LockOperationException
{
    public string LockedByUserId { get; }
    public string LockedByUserName { get; }
    public DateTime LockAcquireTime { get; }
    
    public DocumentLockedException(string documentId, string documentType, string lockedByUserId, string lockedByUserName)
        : base(documentId, documentType, $"文档已被用户 {lockedByUserName} 锁定")
    {
        LockedByUserId = lockedByUserId;
        LockedByUserName = lockedByUserName;
        LockAcquireTime = DateTime.UtcNow;
        OperationType = "DocumentLocked";
    }
}

public class LockTimeoutException : LockOperationException
{
    public TimeSpan Timeout { get; }
    
    public LockTimeoutException(string resourceId, string resourceType, TimeSpan timeout)
        : base(resourceId, resourceType, $"获取锁定超时: {timeout.TotalSeconds}秒")
    {
        Timeout = timeout;
        OperationType = "LockTimeout";
    }
}

public class LockLostException : LockOperationException
{
    public string LockId { get; }
    
    public LockLostException(string resourceId, string resourceType, string lockId)
        : base(resourceId, resourceType, $"锁定丢失: 锁定ID={lockId}")
    {
        LockId = lockId;
        OperationType = "LockLost";
    }
}
```

### 7.2 全局异常处理器

实现全局异常处理器，统一处理分布式锁定相关的异常：

```csharp
public class LockExceptionHandler : IExceptionHandler
{
    private readonly ILogger<LockExceptionHandler> _logger;
    private readonly IEventBus _eventBus;
    
    public LockExceptionHandler(
        ILogger<LockExceptionHandler> logger,
        IEventBus eventBus)
    {
        _logger = logger;
        _eventBus = eventBus;
    }
    
    public async Task<ExceptionResult> HandleAsync(ExceptionContext context, CancellationToken cancellationToken)
    {
        // 记录异常
        _logger.LogError(context.Exception, $"处理锁定异常: {context.Exception.Message}");
        
        // 根据异常类型返回不同的处理结果
        if (context.Exception is DocumentLockedException lockedEx)
        {
            return await HandleDocumentLockedException(context, lockedEx, cancellationToken);
        }
        else if (context.Exception is LockTimeoutException timeoutEx)
        {
            return await HandleLockTimeoutException(context, timeoutEx, cancellationToken);
        }
        else if (context.Exception is LockLostException lostEx)
        {
            return await HandleLockLostException(context, lostEx, cancellationToken);
        }
        else if (context.Exception is LockOperationException lockEx)
        {
            return await HandleGeneralLockException(context, lockEx, cancellationToken);
        }
        
        // 其他异常返回默认处理
        return null;
    }
    
    private async Task<ExceptionResult> HandleDocumentLockedException(
        ExceptionContext context,
        DocumentLockedException exception,
        CancellationToken cancellationToken)
    {
        // 发布锁定冲突事件
        await _eventBus.PublishAsync(new LockConflictEvent
        {
            ResourceId = exception.ResourceId,
            ResourceType = exception.ResourceType,
            UserId = GetCurrentUserId(context),
            SessionId = GetCurrentSessionId(context),
            LockedByUserId = exception.LockedByUserId,
            LockedByUserName = exception.LockedByUserName,
            Timestamp = DateTime.UtcNow
        });
        
        // 返回友好的错误响应
        return new ExceptionResult(
            new JsonResult(new
            {
                success = false,
                errorCode = "DOCUMENT_LOCKED",
                errorMessage = exception.Message,
                lockedByUser = exception.LockedByUserName,
                lockedByUserId = exception.LockedByUserId,
                resourceId = exception.ResourceId,
                resourceType = exception.ResourceType
            })
            {
                StatusCode = StatusCodes.Status409Conflict
            });
    }
    
    private async Task<ExceptionResult> HandleLockTimeoutException(
        ExceptionContext context,
        LockTimeoutException exception,
        CancellationToken cancellationToken)
    {
        // 发布锁定超时事件
        await _eventBus.PublishAsync(new LockTimeoutEvent
        {
            ResourceId = exception.ResourceId,
            ResourceType = exception.ResourceType,
            UserId = GetCurrentUserId(context),
            SessionId = GetCurrentSessionId(context),
            TimeoutSeconds = (int)exception.Timeout.TotalSeconds,
            Timestamp = DateTime.UtcNow
        });
        
        // 返回友好的错误响应
        return new ExceptionResult(
            new JsonResult(new
            {
                success = false,
                errorCode = "LOCK_TIMEOUT",
                errorMessage = exception.Message,
                timeoutSeconds = (int)exception.Timeout.TotalSeconds,
                resourceId = exception.ResourceId,
                resourceType = exception.ResourceType
            })
            {
                StatusCode = StatusCodes.Status408RequestTimeout
            });
    }
    
    private async Task<ExceptionResult> HandleLockLostException(
        ExceptionContext context,
        LockLostException exception,
        CancellationToken cancellationToken)
    {
        // 发布锁定丢失事件
        await _eventBus.PublishAsync(new LockLostEvent
        {
            ResourceId = exception.ResourceId,
            ResourceType = exception.ResourceType,
            LockId = exception.LockId,
            UserId = GetCurrentUserId(context),
            SessionId = GetCurrentSessionId(context),
            Timestamp = DateTime.UtcNow
        });
        
        // 返回友好的错误响应
        return new ExceptionResult(
            new JsonResult(new
            {
                success = false,
                errorCode = "LOCK_LOST",
                errorMessage = exception.Message,
                resourceId = exception.ResourceId,
                resourceType = exception.ResourceType,
                lockId = exception.LockId,
                actionRequired = "refresh"
            })
            {
                StatusCode = StatusCodes.Status409Conflict
            });
    }
    
    private async Task<ExceptionResult> HandleGeneralLockException(
        ExceptionContext context,
        LockOperationException exception,
        CancellationToken cancellationToken)
    {
        // 记录通用锁定操作异常
        _logger.LogError(exception, $"通用锁定操作异常: {exception.OperationType}");
        
        // 返回友好的错误响应
        return new ExceptionResult(
            new JsonResult(new
            {
                success = false,
                errorCode = "LOCK_OPERATION_FAILED",
                errorMessage = exception.Message,
                resourceId = exception.ResourceId,
                resourceType = exception.ResourceType
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            });
    }
    
    private string GetCurrentUserId(ExceptionContext context)
    {
        // 从上下文中获取当前用户ID
        // 实现略
        return string.Empty;
    }
    
    private string GetCurrentSessionId(ExceptionContext context)
    {
        // 从上下文中获取当前会话ID
        // 实现略
        return string.Empty;
    }
}
```

### 7.3 失败恢复机制

实现失败恢复机制，确保在分布式环境中锁定状态的一致性：

```csharp
public class LockRecoveryService : ILockRecoveryService, IHostedService
{
    private readonly IDistributedLock _distributedLock;
    private readonly IRedisClient _redisClient;
    private readonly ILogger<LockRecoveryService> _logger;
    private readonly IEventBus _eventBus;
    private readonly Timer _recoveryTimer;
    private readonly TimeSpan _recoveryInterval = TimeSpan.FromMinutes(5);
    
    public LockRecoveryService(
        IDistributedLock distributedLock,
        IRedisClient redisClient,
        ILogger<LockRecoveryService> logger,
        IEventBus eventBus)
    {
        _distributedLock = distributedLock;
        _redisClient = redisClient;
        _logger = logger;
        _eventBus = eventBus;
        _recoveryTimer = new Timer(ExecuteRecovery, null, _recoveryInterval, _recoveryInterval);
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("锁定恢复服务已启动");
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _recoveryTimer.Dispose();
        _logger.LogInformation("锁定恢复服务已停止");
        return Task.CompletedTask;
    }
    
    private async void ExecuteRecovery(object state)
    {
        try
        {
            _logger.LogInformation("开始执行锁定恢复任务");
            
            // 查找孤立的锁定
            var orphanedLocks = await FindOrphanedLocksAsync();
            
            _logger.LogInformation($"发现 {orphanedLocks.Count} 个孤立的锁定");
            
            // 处理每个孤立的锁定
            foreach (var lockInfo in orphanedLocks)
            {
                try
                {
                    // 记录锁定恢复事件
                    await _eventBus.PublishAsync(new LockRecoveredEvent
                    {
                        ResourceId = lockInfo.ResourceId,
                        ResourceType = lockInfo.ResourceType,
                        LockId = lockInfo.LockId,
                        PreviousOwnerUserId = lockInfo.UserId,
                        PreviousOwnerUserName = lockInfo.UserName,
                        RecoverTime = DateTime.UtcNow
                    });
                    
                    // 清理孤立的锁定
                    await CleanupOrphanedLockAsync(lockInfo);
                    
                    _logger.LogInformation($"已恢复孤立锁定: 资源={lockInfo.ResourceType}:{lockInfo.ResourceId}, 锁定ID={lockInfo.LockId}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"恢复孤立锁定失败: 资源={lockInfo.ResourceType}:{lockInfo.ResourceId}");
                }
            }
            
            // 检查并修复不一致的锁定状态
            await CheckAndRepairLockConsistencyAsync();
            
            _logger.LogInformation("锁定恢复任务执行完成");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "执行锁定恢复任务时发生异常");
        }
    }
    
    private async Task<List<LockInfo>> FindOrphanedLocksAsync()
    {
        var orphanedLocks = new List<LockInfo>();
        
        try
        {
            // 使用Redis SCAN命令查找所有锁定键
            var lockKeys = await _redisClient.ScanKeysAsync("lock:*");
            
            foreach (var lockKey in lockKeys)
            {
                string lockInfoJson = await _redisClient.GetStringAsync(lockKey);
                
                if (!string.IsNullOrEmpty(lockInfoJson))
                {
                    try
                    {
                        var lockInfo = JsonSerializer.Deserialize<LockInfo>(lockInfoJson);
                        
                        // 检查锁定是否已过期
                        if (DateTime.UtcNow > lockInfo.ExpiryTime)
                        {
                            orphanedLocks.Add(lockInfo);
                        }
                        else
                        {
                            // 检查锁定用户会话是否仍然活跃
                            bool isSessionActive = await CheckSessionActiveAsync(lockInfo.UserId, lockInfo.SessionId);
                            
                            if (!isSessionActive)
                            {
                                orphanedLocks.Add(lockInfo);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"解析锁定信息失败: 键={lockKey}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "查找孤立锁定失败");
        }
        
        return orphanedLocks;
    }
    
    private async Task<bool> CheckSessionActiveAsync(string userId, string sessionId)
    {
        try
        {
            // 检查会话是否活跃
            // 这里需要查询会话服务
            // 实现略
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"检查会话活跃状态失败: 用户ID={userId}, 会话ID={sessionId}");
            return false;
        }
    }
    
    private async Task CleanupOrphanedLockAsync(LockInfo lockInfo)
    {
        try
        {
            string lockKey = $"lock:{lockInfo.ResourceType}:{lockInfo.ResourceId}";
            
            // 删除锁定
            await _redisClient.DeleteAsync(lockKey);
            
            // 记录锁定清理日志
            _logger.LogInformation($"已清理孤立锁定: 资源={lockInfo.ResourceType}:{lockInfo.ResourceId}, 锁定ID={lockInfo.LockId}, 用户={lockInfo.UserName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"清理孤立锁定失败: 资源={lockInfo.ResourceType}:{lockInfo.ResourceId}");
            throw;
        }
    }
    
    private async Task CheckAndRepairLockConsistencyAsync()
    {
        try
        {
            // 检查系统中的锁定状态一致性
            // 实现略
            _logger.LogInformation("锁定状态一致性检查完成");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "检查锁定状态一致性失败");
        }
    }
}
```

## 8. 集成与部署方案

### 8.1 依赖注入配置

```csharp
public static class DistributedLockServiceCollectionExtensions
{
    public static IServiceCollection AddDistributedLockServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 配置分布式锁定选项
        services.Configure<DistributedLockOptions>(configuration.GetSection("DistributedLock"));
        
        // 注册分布式锁定实现
        services.AddSingleton<IDistributedLock, EnhancedRedisDistributedLock>();
        
        // 注册锁定管理器服务
        services.AddSingleton<ILockManagerService, EnhancedLockManagerService>();
        
        // 注册锁定冲突检测器和解决器
        services.AddSingleton<ILockConflictDetector, EnhancedLockConflictDetector>();
        services.AddSingleton<ILockConflictResolver, AdaptiveLockConflictResolver>();
        
        // 注册锁定续期服务
        services.AddSingleton<ILockRenewer, SmartLockRenewer>();
        
        // 注册资源类型注册表
        services.AddSingleton<IResourceTypeRegistry, ResourceTypeRegistry>();
        
        // 注册用户活动跟踪器
        services.AddSingleton<IUserActivityTracker, UserActivityTracker>();
        
        // 注册锁定恢复服务
        services.AddHostedService<LockRecoveryService>();
        services.AddSingleton<ILockRecoveryService, LockRecoveryService>();
        
        // 注册工作台任务同步服务
        services.AddSingleton<IWorkbenchTaskSyncService, WorkbenchTaskSyncService>();
        services.AddSingleton<IWorkbenchTaskSyncManager, WorkbenchTaskSyncManager>();
        
        // 注册异常处理器
        services.AddExceptionHandler<LockExceptionHandler>();
        
        return services;
    }
}
```

### 8.2 配置文件示例

```json
{
  "DistributedLock": {
    "DefaultLockTimeoutSeconds": 600,
    "LockRenewalIntervalSeconds": 300,
    "MaxLockHoldTimeMinutes": 120,
    "UserInactivityThresholdSeconds": 300,
    "EnableLockRecovery": true,
    "RecoveryIntervalMinutes": 5,
    "Redis": {
      "ConnectionString": "localhost:6379",
      "Database": 0,
      "KeyPrefix": "ruinor:lock:"
    },
    "WebSocket": {
      "Enabled": true,
      "HeartbeatIntervalSeconds": 30
    }
  }
}
```

### 8.3 客户端集成指南

1. **锁定操作集成**

在客户端表单加载时尝试获取锁定：

```csharp
public async Task InitializeDocumentAsync(string documentId, string documentType)
{
    try
    {
        // 尝试获取文档锁定
        var lockRequest = new LockRequest
        {
            ResourceId = documentId,
            ResourceType = documentType,
            UserId = CurrentUser.Id,
            UserName = CurrentUser.Name,
            SessionId = CurrentSession.Id,
            LockType = LockType.Edit
        };
        
        var lockResponse = await _lockManagerService.HandleLockRequestAsync(lockRequest);
        
        if (!lockResponse.Success)
        {
            // 显示锁定冲突提示
            if (lockResponse.CurrentLockInfo != null)
            {
                ShowLockConflictMessage(lockResponse.CurrentLockInfo);
            }
            else
            {
                ShowErrorMessage(lockResponse.Message);
            }
            
            // 设置为只读模式
            SetReadOnlyMode(true);
            return;
        }
        
        // 锁定成功，保存锁定ID
        CurrentLockId = lockResponse.LockId;
        
        // 注册锁定续期
        _lockRenewer.RegisterLockForRenewal(
            lockResponse.LockId,
            documentId,
            documentType,
            CurrentUser.Id,
            CurrentSession.Id,
            lockResponse.ExpiryTime - DateTime.UtcNow);
        
        // 加载文档数据
        await LoadDocumentDataAsync(documentId, documentType);
        
        // 设置为编辑模式
        SetReadOnlyMode(false);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "初始化文档失败");
        ShowErrorMessage($"初始化文档失败: {ex.Message}");
        SetReadOnlyMode(true);
    }
}
```

在表单关闭时释放锁定：

```csharp
protected override async Task OnFormClosingAsync(FormClosingEventArgs e)
{
    if (!string.IsNullOrEmpty(CurrentLockId) && !IsReadOnlyMode)
    {
        try
        {
            // 释放锁定
            var unlockRequest = new UnlockRequest
            {
                ResourceId = CurrentDocumentId,
                ResourceType = CurrentDocumentType,
                UserId = CurrentUser.Id,
                SessionId = CurrentSession.Id,
                LockId = CurrentLockId
            };
            
            await _lockManagerService.HandleReleaseLockRequestAsync(unlockRequest);
            
            // 取消锁定续期
            _lockRenewer.UnregisterLockForRenewal(CurrentLockId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "释放锁定失败");
            // 继续关闭，不阻塞用户
        }
    }
    
    await base.OnFormClosingAsync(e);
}
```

2. **任务状态同步集成**

在工作台任务清单控件中集成实时状态同步：

```csharp
public partial class UCTodoList : UserControl
{
    private readonly IWorkbenchTaskSyncManager _taskSyncManager;
    private readonly ILogger<UCTodoList> _logger;
    private bool _isInitialized;
    
    public UCTodoList(IWorkbenchTaskSyncManager taskSyncManager, ILogger<UCTodoList> logger)
    {
        InitializeComponent();
        _taskSyncManager = taskSyncManager;
        _logger = logger;
        
        // 订阅任务状态变更事件
        _taskSyncManager.TaskStatusChanged += OnTaskStatusChanged;
        _taskSyncManager.TaskAdded += OnTaskAdded;
        _taskSyncManager.TaskRemoved += OnTaskRemoved;
    }
    
    public async Task InitializeAsync()
    {
        if (_isInitialized)
            return;
        
        try
        {
            // 初始化任务同步管理器
            await _taskSyncManager.InitializeAsync();
            
            // 构建初始任务树
            await BuilderToDoListTreeView();
            
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "初始化任务清单失败");
        }
    }
    
    private void OnTaskStatusChanged(object sender, TaskStatusChangedEventArgs e)
    {
        try
        {
            // 在UI线程更新任务状态
            this.Invoke((MethodInvoker)delegate
            {
                UpdateTaskStatusInTree(e.Task, e.OldStatus, e.NewStatus);
                
                // 更新任务计数
                UpdateTaskCounters();
                
                // 显示状态变更通知
                ShowStatusChangeNotification(e.Task, e.NewStatus);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"更新任务状态失败: 任务ID={e.Task.TaskId}");
        }
    }
    
    private void OnTaskAdded(object sender, TaskAddedEventArgs e)
    {
        try
        {
            // 在UI线程添加新任务
            this.Invoke((MethodInvoker)delegate
            {
                AddTaskToTree(e.Task);
                UpdateTaskCounters();
                
                // 显示新任务通知
                ShowNewTaskNotification(e.Task);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"添加新任务失败: 任务ID={e.Task.TaskId}");
        }
    }
    
    private void OnTaskRemoved(object sender, TaskRemovedEventArgs e)
    {
        try
        {
            // 在UI线程移除任务
            this.Invoke((MethodInvoker)delegate
            {
                RemoveTaskFromTree(e.TaskId);
                UpdateTaskCounters();
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"移除任务失败: 任务ID={e.TaskId}");
        }
    }
    
    // 其他方法...
}
```

## 9. 测试与验证

### 9.1 单元测试

为核心组件编写单元测试，确保功能正确性：

```csharp
public class EnhancedRedisDistributedLockTests
{
    private Mock<IRedisClient> _redisClientMock;
    private Mock<ILogger<EnhancedRedisDistributedLock>> _loggerMock;
    private Mock<IDistributedLockOptions> _optionsMock;
    private Mock<IUserActivityTracker> _activityTrackerMock;
    private EnhancedRedisDistributedLock _distributedLock;
    
    [SetUp]
    public void Setup()
    {
        _redisClientMock = new Mock<IRedisClient>();
        _loggerMock = new Mock<ILogger<EnhancedRedisDistributedLock>>();
        _optionsMock = new Mock<IDistributedLockOptions>();
        _activityTrackerMock = new Mock<IUserActivityTracker>();
        
        // 设置默认超时时间为10分钟
        _optionsMock.SetupGet(o => o.DefaultLockTimeoutSeconds).Returns(600);
        
        _distributedLock = new EnhancedRedisDistributedLock(
            _redisClientMock.Object,
            _loggerMock.Object,
            _optionsMock.Object,
            _activityTrackerMock.Object);
    }
    
    [Test]
    public async Task AcquireLockAsync_ShouldSucceedWhenLockNotExists()
    {
        // 配置模拟行为
        _redisClientMock.Setup(r => r.GetStringAsync(It.IsAny<string>())).ReturnsAsync((string)null);
        _redisClientMock.Setup(r => r.SetAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<When>())).ReturnsAsync(true);
        
        // 创建锁定上下文
        var lockContext = new LockContext
        {
            ResourceId = "doc123",
            ResourceType = "SalesOrder",
            UserId = "user1",
            UserName = "张三",
            SessionId = "session1",
            LockType = LockType.Edit
        };
        
        // 执行测试
        var result = await _distributedLock.AcquireLockAsync(lockContext);
        
        // 验证结果
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.LockId);
        Assert.IsNotNull(result.CurrentLockInfo);
        Assert.AreEqual("user1", result.CurrentLockInfo.UserId);
        
        // 验证Redis调用
        _redisClientMock.Verify(r => r.SetAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            600,
            When.NotExists), Times.Once);
        
        // 验证活动跟踪
        _activityTrackerMock.Verify(a => a.RecordUserActivity(
            "user1",
            "session1",
            "SalesOrder",
            "doc123",
            "LockAcquired"), Times.Once);
    }
    
    [Test]
    public async Task AcquireLockAsync_ShouldSupportReentrancy()
    {
        // 配置模拟行为 - 返回现有锁定信息
        var existingLockInfo = new LockInfo
        {
            LockId = "lock1",
            ResourceId = "doc123",
            ResourceType = "SalesOrder",
            UserId = "user1",
            UserName = "张三",
            SessionId = "session1",
            AcquireTime = DateTime.UtcNow.AddMinutes(-5),
            ExpiryTime = DateTime.UtcNow.AddMinutes(5),
            LockType = LockType.Edit,
            ReentrantCount = 1
        };
        
        string lockInfoJson = JsonSerializer.Serialize(existingLockInfo);
        _redisClientMock.Setup(r => r.GetStringAsync(It.IsAny<string>())).ReturnsAsync(lockInfoJson);
        _redisClientMock.Setup(r => r.SetAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>())).ReturnsAsync(true);
        
        // 创建锁定上下文（相同用户和会话）
        var lockContext = new LockContext
        {
            ResourceId = "doc123",
            ResourceType = "SalesOrder",
            UserId = "user1",
            UserName = "张三",
            SessionId = "session1",
            LockType = LockType.Edit
        };
        
        // 执行测试
        var result = await _distributedLock.AcquireLockAsync(lockContext);
        
        // 验证结果
        Assert.IsTrue(result.Success);
        Assert.AreEqual("lock1", result.LockId);
        Assert.AreEqual(2, result.CurrentLockInfo.ReentrantCount);
        
        // 验证Redis调用
        _redisClientMock.Verify(r => r.SetAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            600), Times.Once);
    }
    
    [Test]
    public async Task ReleaseLockAsync_ShouldDecrementReentrantCount()
    {
        // 配置模拟行为 - 返回现有锁定信息（重入计数为2）
        var existingLockInfo = new LockInfo
        {
            LockId = "lock1",
            ResourceId = "doc123",
            ResourceType = "SalesOrder",
            UserId = "user1",
            UserName = "张三",
            SessionId = "session1",
            AcquireTime = DateTime.UtcNow.AddMinutes(-5),
            ExpiryTime = DateTime.UtcNow.AddMinutes(5),
            LockType = LockType.Edit,
            ReentrantCount = 2
        };
        
        string lockInfoJson = JsonSerializer.Serialize(existingLockInfo);
        _redisClientMock.Setup(r => r.GetStringAsync(It.IsAny<string>())).ReturnsAsync(lockInfoJson);
        _redisClientMock.Setup(r => r.SetAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>())).ReturnsAsync(true);
        
        // 创建释放上下文
        var releaseContext = new ReleaseContext
        {
            ResourceId = "doc123",
            ResourceType = "SalesOrder",
            UserId = "user1",
            SessionId = "session1",
            LockId = "lock1"
        };
        
        // 执行测试
        bool result = await _distributedLock.ReleaseLockAsync(releaseContext);
        
        // 验证结果
        Assert.IsTrue(result);
        
        // 验证Redis调用（应该是SET而不是DELETE，因为重入计数>1）
        _redisClientMock.Verify(r => r.SetAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            600), Times.Once);
        _redisClientMock.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Never);
    }
    
    // 更多测试用例...
}
```

### 9.2 集成测试

编写集成测试，验证各个组件之间的交互：

```csharp
public class LockManagerServiceIntegrationTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly TestWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly IServiceScopeFactory _scopeFactory;
    
    public LockManagerServiceIntegrationTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    }
    
    [Fact]
    public async Task HandleLockRequest_ShouldAcquireLockAndPublishEvent()
    {
        using var scope = _scopeFactory.CreateScope();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        var eventCollector = scope.ServiceProvider.GetRequiredService<TestEventCollector>();
        
        // 清除事件收集器
        eventCollector.ClearEvents();
        
        // 发送锁定请求
        var lockRequest = new LockRequest
        {
            ResourceId = "integration-test-doc",
            ResourceType = "SalesOrder",
            UserId = "test-user",
            UserName = "测试用户",
            SessionId = "test-session",
            LockType = LockType.Edit
        };
        
        var response = await _client.PostAsJsonAsync("/api/lock/acquire", lockRequest);
        
        // 验证响应
        response.EnsureSuccessStatusCode();
        
        var lockResponse = await response.Content.ReadFromJsonAsync<LockResponse>();
        Assert.NotNull(lockResponse);
        Assert.True(lockResponse.Success);
        Assert.NotNull(lockResponse.LockId);
        
        // 验证事件发布
        var lockAcquiredEvents = eventCollector.GetEvents<LockAcquiredEvent>();
        Assert.Single(lockAcquiredEvents);
        Assert.Equal("integration-test-doc", lockAcquiredEvents[0].ResourceId);
        Assert.Equal("test-user", lockAcquiredEvents[0].UserId);
        
        // 发送释放请求
        var unlockRequest = new UnlockRequest
        {
            ResourceId = "integration-test-doc",
            ResourceType = "SalesOrder",
            UserId = "test-user",
            SessionId = "test-session",
            LockId = lockResponse.LockId
        };
        
        var unlockResponse = await _client.PostAsJsonAsync("/api/lock/release", unlockRequest);
        unlockResponse.EnsureSuccessStatusCode();
        
        // 验证锁定释放事件
        var lockReleasedEvents = eventCollector.GetEvents<LockReleasedEvent>();
        Assert.Single(lockReleasedEvents);
        Assert.Equal("integration-test-doc", lockReleasedEvents[0].ResourceId);
    }
    
    // 更多集成测试...
}
```

## 10. 性能优化建议

1. **缓存优化**
   - 缓存锁定状态信息，减少Redis访问频率
   - 使用本地锁减少对分布式锁的竞争

2. **异步处理**
   - 使用异步操作减少线程阻塞
   - 采用并行处理提高并发性能

3. **消息队列优化**
   - 使用高性能消息队列（如RabbitMQ）
   - 实现消息批处理减少网络往返

4. **WebSocket优化**
   - 实现消息压缩减少网络带宽
   - 使用心跳机制保持连接活跃
   - 断线重连机制确保消息可靠性

5. **数据库优化**
   - 使用索引优化锁定状态查询
   - 优化事务隔离级别减少锁竞争

## 11. 结论与下一步

本技术方案提供了一个完整的RUINORERP系统分布式锁定与状态同步的优化设计。通过实现基于"先编辑先拥有"原则的分布式锁定机制，解决了多用户并发编辑冲突问题；通过WebSocket实时推送机制，实现了工作台任务清单的实时状态同步。

下一步建议：

1. 按照本方案实现核心组件
2. 进行全面的单元测试和集成测试
3. 在测试环境中进行性能测试和压力测试
4. 逐步在生产环境中部署和推广
5. 持续监控和优化系统性能

通过这些优化，可以显著提高系统的并发处理能力和用户体验，确保在多用户环境下的数据一致性和操作流畅性。