# RUINORERP系统分布式锁定机制与单据状态同步设计方案

## 1. 现有系统分析

### 1.1 现有分布式锁定机制

通过分析RUINORERP系统的代码，发现当前系统的分布式锁定机制存在以下特点和局限性：

1. **分布式锁接口设计**：系统定义了`IDistributedLock`接口，提供了`TryAcquireAsync`、`ReleaseAsync`和`ExistsAsync`三个核心方法。

2. **本地锁实现**：`LocalDistributedLock`基于本地信号量实现，使用`ConcurrentDictionary`存储`SemaphoreSlim`对象，仅适用于单节点部署。

3. **Redis锁框架**：系统提供了`RedisDistributedLock`的框架，但实际实现较为基础，仅包含占位代码，未完整实现Redis分布式锁的核心功能。

4. **锁定请求与响应模型**：系统使用`LockRequest`和`LockResponse`模型描述锁定操作的请求和响应。

5. **锁管理器服务**：`LockManagerService`实现了`ILockManagerService`接口，提供了单据锁定、解锁和检查等功能，但当前基于内存存储锁定信息。

6. **锁命令处理**：`LockCommandHandler`处理各类锁定相关的命令，包括锁定申请、释放、强制解锁等，但缺乏分布式环境下的完整实现。

### 1.2 现有机制局限性

1. **分布式环境适应性差**：当前的锁定机制主要基于内存存储，在多服务器部署环境下无法实现真正的分布式锁定。

2. **Redis锁实现不完整**：虽然有`RedisDistributedLock`类，但实际实现缺乏原子性操作和Lua脚本等关键特性。

3. **锁定粒度控制**：缺乏针对不同类型数据（单表、主子表）的差异化锁定策略。

4. **锁超时和续期**：当前机制在长时间编辑场景下可能导致锁自动失效。

5. **状态同步机制不完善**：特别是在工作台任务清单(UCTodoList)中，缺乏单据状态变更的实时同步机制。

## 2. 基于"先编辑先拥有"原则的分布式锁定机制设计

### 2.1 核心设计原则

1. **"先编辑先拥有"原则**：同一时间仅允许一个用户编辑特定单据或基础数据。

2. **分布式一致性**：确保在多服务器部署环境下，锁定状态在各节点间保持一致。

3. **高可用性**：锁定机制自身应具有高可用性，避免单点故障。

4. **锁自动过期**：为防止死锁，所有锁都必须有过期时间。

5. **锁续期机制**：对于长时间编辑操作，提供自动续期功能。

6. **状态同步**：实现锁定状态和单据状态的实时同步。

### 2.2 分布式锁实现方案

#### 2.2.1 Redis分布式锁增强

完善`RedisDistributedLock`类的实现，利用Redis的特性确保分布式锁的原子性和可靠性：

1. **使用SET NX EX命令**：结合SET命令的NX(不存在才设置)和EX(设置过期时间)选项，实现原子性的锁获取和超时设置。

2. **使用Lua脚本释放锁**：使用Lua脚本确保锁释放的原子性，避免误释放其他线程或服务器持有的锁。

3. **锁续期机制**：提供锁续期API，允许长时间编辑操作保持锁定状态。

#### 2.2.2 分布式锁管理器服务

改进`LockManagerService`，使用分布式存储替代内存存储：

1. **Redis存储锁定信息**：使用Redis Hash结构存储锁定详情，包括锁定用户、时间戳等。

2. **分层锁定策略**：根据数据类型（单表、主子表）实现不同粒度的锁定策略。

3. **锁定冲突处理**：实现基于"先编辑先拥有"原则的锁定冲突处理逻辑。

#### 2.2.3 锁定请求处理流程

优化锁请求处理流程，确保分布式环境下的一致性：

1. **锁请求验证**：验证用户权限、会话有效性等。

2. **分布式锁获取**：尝试通过Redis获取分布式锁。

3. **锁状态更新**：在获取锁成功后，更新锁定信息并返回。

4. **锁冲突处理**：对于锁冲突情况，返回当前锁定信息，便于前端展示。

### 2.3 锁粒度设计

根据不同类型的数据，设计不同粒度的锁定策略：

1. **单表数据锁定**：对单条记录进行锁定，使用表名+主键的组合作为锁键。

2. **主子表数据锁定**：同时锁定主表记录和所有相关子表记录，确保数据完整性。

3. **批量数据锁定**：支持多记录同时锁定，适用于批量操作场景。

## 3. 工作台任务清单实时状态同步机制设计

### 3.1 UCTodoList状态同步架构

1. **事件驱动设计**：基于发布-订阅模式，当单据状态发生变更时，发布状态变更事件。

2. **Socket通信层**：利用现有的Socket通信机制，实现服务器到客户端的实时状态推送。

3. **客户端状态更新**：客户端接收状态更新后，更新UCTodoList中的相应任务状态。

### 3.2 单据状态变更同步流程

1. **状态变更监听**：在`ActionManager`中监听单据状态变更事件。

2. **状态变更事件发布**：当单据状态发生变更时，通过Redis发布订阅机制或消息队列发布状态变更事件。

3. **服务器处理**：服务器接收状态变更事件，查找相关用户，并通过Socket连接推送状态更新。

4. **客户端更新**：客户端接收状态更新，调用UCTodoList的更新方法刷新UI。

### 3.3 基于SYLLABLE协议的状态同步

1. **协议扩展**：扩展SYLLABLE协议，增加状态同步相关的消息类型。

2. **状态数据结构**：定义状态更新的数据结构，包含单据ID、类型、状态等信息。

3. **消息压缩**：对于大批量状态更新，实现消息压缩机制，减少网络传输量。

## 4. 文档处理模块与分布式锁定机制集成

### 4.1 DocumentConverter与分布式锁定集成

1. **转换前锁定检查**：在文档转换前，检查源文档是否被锁定。

2. **转换过程锁定**：在文档转换过程中，临时锁定相关文档，确保转换过程的数据一致性。

3. **转换后状态通知**：文档转换完成后，触发状态变更通知，更新相关用户的工作台任务。

### 4.2 ActionManager与状态同步集成

1. **工作流步骤状态追踪**：在工作流执行过程中，实时追踪每个步骤的执行状态。

2. **状态变更事件发布**：在关键步骤完成后，发布状态变更事件。

3. **错误状态处理**：当工作流执行失败时，发布错误状态事件，确保所有用户得到通知。

## 5. 异常处理策略和失败恢复机制

### 5.1 分布式锁异常处理

1. **锁获取失败处理**：实现重试机制，对于临时性失败自动重试。

2. **锁超时处理**：定期清理过期锁，避免死锁。

3. **强制解锁机制**：提供管理员强制解锁功能，处理特殊情况。

### 5.2 网络异常处理

1. **断网重连**：客户端检测到网络断开时，自动尝试重连。

2. **状态同步重发**：对于状态同步失败的消息，实现重发机制。

3. **离线状态处理**：客户端离线期间的状态变更，在重新连接时进行同步。

### 5.3 故障恢复机制

1. **系统重启恢复**：系统重启后，重建锁定状态和状态同步连接。

2. **数据一致性检查**：定期执行数据一致性检查，修复潜在的数据不一致问题。

3. **备份恢复策略**：实现锁定信息的备份和恢复机制，应对灾难性故障。

## 6. 技术方案实现细节

### 6.1 分布式锁实现代码示例

#### RedisDistributedLock 增强实现
```csharp
public class RedisDistributedLock : IDistributedLock
{
    private readonly IDatabase _redisDb;
    private readonly string _lockPrefix;
    private readonly ILogger<RedisDistributedLock> _logger;
    
    // Lua脚本用于原子性释放锁
    private const string ReleaseLockScript = @"
        if redis.call('get', KEYS[1]) == ARGV[1] then
            return redis.call('del', KEYS[1])
        else
            return 0
        end
    ";
    
    // Lua脚本用于锁续期
    private const string ExtendLockScript = @"
        if redis.call('get', KEYS[1]) == ARGV[1] then
            return redis.call('pexpire', KEYS[1], ARGV[2])
        else
            return 0
        end
    ";
    
    public RedisDistributedLock(IConnectionMultiplexer redis, ILogger<RedisDistributedLock> logger, string lockPrefix = "dist_lock:")
    {
        _redisDb = redis.GetDatabase();
        _logger = logger;
        _lockPrefix = lockPrefix;
    }
    
    public async Task<bool> TryAcquireAsync(string lockKey, string lockValue, TimeSpan expiry)
    {
        string redisKey = _lockPrefix + lockKey;
        
        try
        {
            bool acquired = await _redisDb.StringSetAsync(
                redisKey, 
                lockValue, 
                expiry, 
                When.NotExists);
                
            if (acquired)
            {
                _logger.LogInformation($"成功获取分布式锁: {lockKey}");
            }
            else
            {
                _logger.LogInformation($"获取分布式锁失败: {lockKey}");
            }
            
            return acquired;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"尝试获取分布式锁时发生异常: {lockKey}");
            return false;
        }
    }
    
    public async Task<bool> ReleaseAsync(string lockKey, string lockValue)
    {
        string redisKey = _lockPrefix + lockKey;
        
        try
        {
            var result = await _redisDb.ScriptEvaluateAsync(
                ReleaseLockScript,
                new RedisKey[] { redisKey },
                new RedisValue[] { lockValue });
                
            long released = (long)result;
            bool success = released > 0;
            
            if (success)
            {
                _logger.LogInformation($"成功释放分布式锁: {lockKey}");
            }
            else
            {
                _logger.LogWarning($"释放分布式锁失败(可能锁不存在或已被其他进程获取): {lockKey}");
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"尝试释放分布式锁时发生异常: {lockKey}");
            return false;
        }
    }
    
    public async Task<bool> ExistsAsync(string lockKey)
    {
        string redisKey = _lockPrefix + lockKey;
        
        try
        {
            return await _redisDb.KeyExistsAsync(redisKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"检查分布式锁是否存在时发生异常: {lockKey}");
            return false;
        }
    }
    
    public async Task<bool> ExtendLockAsync(string lockKey, string lockValue, TimeSpan expiry)
    {
        string redisKey = _lockPrefix + lockKey;
        
        try
        {
            var result = await _redisDb.ScriptEvaluateAsync(
                ExtendLockScript,
                new RedisKey[] { redisKey },
                new RedisValue[] { lockValue, expiry.TotalMilliseconds });
                
            bool success = (long)result > 0;
            
            if (success)
            {
                _logger.LogInformation($"成功延长分布式锁: {lockKey}, 新过期时间: {expiry}");
            }
            else
            {
                _logger.LogWarning($"延长分布式锁失败(可能锁不存在或已被其他进程获取): {lockKey}");
            }
            
            return success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"尝试延长分布式锁时发生异常: {lockKey}");
            return false;
        }
    }
}
```

#### LockManagerService 分布式实现
```csharp
public class DistributedLockManagerService : ILockManagerService
{
    private readonly IDistributedLock _distributedLock;
    private readonly ISessionService _sessionService;
    private readonly IDatabase _redisDb;
    private readonly ILogger<DistributedLockManagerService> _logger;
    
    private const string LockInfoPrefix = "lock_info:";
    
    public DistributedLockManagerService(
        IDistributedLock distributedLock, 
        ISessionService sessionService,
        IConnectionMultiplexer redis,
        ILogger<DistributedLockManagerService> logger)
    {
        _distributedLock = distributedLock;
        _sessionService = sessionService;
        _redisDb = redis.GetDatabase();
        _logger = logger;
    }
    
    public async Task<LockResponse> TryLockDocumentAsync(LockRequest request)
    {
        try
        {
            // 验证会话有效性
            if (!await _sessionService.ValidateSessionAsync(request.SessionId))
            {
                return new LockResponse
                {
                    Success = false,
                    Message = "会话无效，请重新登录"
                };
            }
            
            // 生成锁键和锁值
            string lockKey = GenerateLockKey(request.ResourceId, request.ResourceType);
            string lockValue = $"{request.UserId}:{request.SessionId}:{Guid.NewGuid()}";
            TimeSpan expiry = TimeSpan.FromMinutes(30); // 默认锁定30分钟
            
            // 尝试获取分布式锁
            bool acquired = await _distributedLock.TryAcquireAsync(lockKey, lockValue, expiry);
            
            if (!acquired)
            {
                // 获取锁失败，查询当前锁定信息
                var currentLockInfo = await GetLockInfoAsync(lockKey);
                
                return new LockResponse
                {
                    Success = false,
                    Message = "该单据已被锁定",
                    CurrentLockInfo = currentLockInfo
                };
            }
            
            // 保存锁定信息
            await SaveLockInfoAsync(lockKey, new LockInfo
            {
                LockKey = lockKey,
                LockValue = lockValue,
                ResourceId = request.ResourceId,
                ResourceType = request.ResourceType,
                UserId = request.UserId,
                UserName = request.UserName,
                SessionId = request.SessionId,
                AcquireTime = DateTime.Now,
                ExpiryTime = DateTime.Now.Add(expiry),
                LockType = request.LockType
            });
            
            // 针对主子表结构，同时锁定子表
            if (request.LockType == LockType.MasterDetail)
            {
                await LockDetailTablesAsync(request.ResourceId, request.ResourceType, lockValue);
            }
            
            return new LockResponse
            {
                Success = true,
                Message = "锁定成功",
                LockId = lockValue,
                ExpiryTime = DateTime.Now.Add(expiry)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"尝试锁定单据时发生异常: {request.ResourceId}");
            return new LockResponse
            {
                Success = false,
                Message = "锁定过程中发生错误: " + ex.Message
            };
        }
    }
    
    public async Task<LockResponse> UnlockDocumentAsync(string lockId, string resourceId, string resourceType)
    {
        try
        {
            string lockKey = GenerateLockKey(resourceId, resourceType);
            var lockInfo = await GetLockInfoAsync(lockKey);
            
            if (lockInfo == null || lockInfo.LockValue != lockId)
            {
                return new LockResponse
                {
                    Success = false,
                    Message = "无效的锁定ID或单据未被锁定"
                };
            }
            
            // 释放分布式锁
            bool released = await _distributedLock.ReleaseAsync(lockKey, lockId);
            
            if (released)
            {
                // 删除锁定信息
                await _redisDb.KeyDeleteAsync(LockInfoPrefix + lockKey);
                
                // 释放子表锁定
                if (lockInfo.LockType == LockType.MasterDetail)
                {
                    await UnlockDetailTablesAsync(resourceId, resourceType, lockId);
                }
                
                return new LockResponse
                {
                    Success = true,
                    Message = "解锁成功"
                };
            }
            
            return new LockResponse
            {
                Success = false,
                Message = "解锁失败"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"尝试解锁单据时发生异常: {resourceId}");
            return new LockResponse
            {
                Success = false,
                Message = "解锁过程中发生错误: " + ex.Message
            };
        }
    }
    
    // 其他方法实现...
    
    private string GenerateLockKey(string resourceId, string resourceType)
    {
        return $"{resourceType}:{resourceId}";
    }
    
    private async Task SaveLockInfoAsync(string lockKey, LockInfo lockInfo)
    {
        string redisKey = LockInfoPrefix + lockKey;
        await _redisDb.HashSetAsync(
            redisKey, 
            new HashEntry[]
            {
                new HashEntry("LockKey", lockInfo.LockKey),
                new HashEntry("LockValue", lockInfo.LockValue),
                new HashEntry("ResourceId", lockInfo.ResourceId),
                new HashEntry("ResourceType", lockInfo.ResourceType),
                new HashEntry("UserId", lockInfo.UserId),
                new HashEntry("UserName", lockInfo.UserName),
                new HashEntry("SessionId", lockInfo.SessionId),
                new HashEntry("AcquireTime", lockInfo.AcquireTime.ToString("O")),
                new HashEntry("ExpiryTime", lockInfo.ExpiryTime.ToString("O")),
                new HashEntry("LockType", lockInfo.LockType.ToString())
            });
            
        // 设置过期时间，与锁定时间一致
        await _redisDb.KeyExpireAsync(redisKey, lockInfo.ExpiryTime - DateTime.Now);
    }
    
    private async Task<LockInfo> GetLockInfoAsync(string lockKey)
    {
        string redisKey = LockInfoPrefix + lockKey;
        
        var hashEntries = await _redisDb.HashGetAllAsync(redisKey);
        if (hashEntries.Length == 0)
        {
            return null;
        }
        
        var hash = hashEntries.ToDictionary(entry => entry.Name, entry => entry.Value);
        
        return new LockInfo
        {
            LockKey = hash.ContainsKey("LockKey") ? hash["LockKey"] : string.Empty,
            LockValue = hash.ContainsKey("LockValue") ? hash["LockValue"] : string.Empty,
            ResourceId = hash.ContainsKey("ResourceId") ? hash["ResourceId"] : string.Empty,
            ResourceType = hash.ContainsKey("ResourceType") ? hash["ResourceType"] : string.Empty,
            UserId = hash.ContainsKey("UserId") ? hash["UserId"] : string.Empty,
            UserName = hash.ContainsKey("UserName") ? hash["UserName"] : string.Empty,
            SessionId = hash.ContainsKey("SessionId") ? hash["SessionId"] : string.Empty,
            AcquireTime = hash.ContainsKey("AcquireTime") ? DateTime.Parse(hash["AcquireTime"]).ToLocalTime() : DateTime.MinValue,
            ExpiryTime = hash.ContainsKey("ExpiryTime") ? DateTime.Parse(hash["ExpiryTime"]).ToLocalTime() : DateTime.MinValue,
            LockType = hash.ContainsKey("LockType") ? Enum.Parse<LockType>(hash["LockType"]) : LockType.Single
        };
    }
    
    private async Task LockDetailTablesAsync(string resourceId, string resourceType, string lockValue)
    {
        // 根据主表类型获取相关子表信息，实现略
        var detailTables = GetDetailTablesForResource(resourceType);
        
        foreach (var table in detailTables)
        {
            string detailLockKey = $"{table}:{resourceId}";
            // 为子表记录设置锁定，与主表锁定时间一致
            await _distributedLock.TryAcquireAsync(detailLockKey, lockValue, TimeSpan.FromMinutes(30));
        }
    }
    
    private async Task UnlockDetailTablesAsync(string resourceId, string resourceType, string lockValue)
    {
        var detailTables = GetDetailTablesForResource(resourceType);
        
        foreach (var table in detailTables)
        {
            string detailLockKey = $"{table}:{resourceId}";
            await _distributedLock.ReleaseAsync(detailLockKey, lockValue);
        }
    }
    
    private List<string> GetDetailTablesForResource(string resourceType)
    {
        // 根据实际系统设计实现，返回与指定资源类型相关的子表列表
        // 这里仅为示例
        var detailTables = new Dictionary<string, List<string>>
        {
            { "Order", new List<string> { "OrderItem" } },
            { "Invoice", new List<string> { "InvoiceItem" } }
        };
        
        if (detailTables.TryGetValue(resourceType, out var tables))
        {
            return tables;
        }
        
        return new List<string>();
    }
}
```

### 6.2 工作台任务清单状态同步实现

#### 服务端状态变更广播
```csharp
public class BillStatusChangeNotifier : IBillStatusChangeNotifier
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<BillStatusChangeNotifier> _logger;
    private readonly ConcurrentDictionary<string, HashSet<string>> _userConnections;
    
    public BillStatusChangeNotifier(IConnectionMultiplexer redis, ILogger<BillStatusChangeNotifier> logger)
    {
        _redis = redis;
        _logger = logger;
        _userConnections = new ConcurrentDictionary<string, HashSet<string>>();
        
        // 订阅Redis状态变更事件
        var subscriber = _redis.GetSubscriber();
        subscriber.Subscribe("bill:status:change", async (channel, message) => 
        {
            try
            {
                var statusChange = JsonSerializer.Deserialize<BillStatusChange>(message);
                if (statusChange != null)
                {
                    await BroadcastStatusChangeToUsers(statusChange);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理状态变更消息时发生异常");
            }
        });
    }
    
    public void RegisterUserConnection(string userId, string connectionId)
    {
        _userConnections.AddOrUpdate(userId, 
            _ => new HashSet<string> { connectionId }, 
            (_, connections) => 
            {
                connections.Add(connectionId);
                return connections;
            });
        
        _logger.LogInformation($"用户 {userId} 的连接 {connectionId} 已注册");
    }
    
    public void UnregisterUserConnection(string userId, string connectionId)
    {
        if (_userConnections.TryGetValue(userId, out var connections))
        {
            connections.Remove(connectionId);
            
            if (connections.Count == 0)
            {
                _userConnections.TryRemove(userId, out _);
            }
            
            _logger.LogInformation($"用户 {userId} 的连接 {connectionId} 已注销");
        }
    }
    
    public async Task NotifyStatusChangeAsync(BillStatusChange statusChange)
    {
        // 发布状态变更事件到Redis
        string message = JsonSerializer.Serialize(statusChange);
        await _redis.GetSubscriber().PublishAsync("bill:status:change", message);
        
        _logger.LogInformation($"已发布单据状态变更事件: 单据类型={statusChange.BillType}, 单据ID={statusChange.BillId}, 状态={statusChange.Status}");
    }
    
    private async Task BroadcastStatusChangeToUsers(BillStatusChange statusChange)
    {
        // 获取需要通知的用户列表
        var notifiedUsers = await GetUsersToNotifyAsync(statusChange);
        
        foreach (var userId in notifiedUsers)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                foreach (var connectionId in connections.ToList()) // ToList避免并发修改异常
                {
                    try
                    {
                        // 通过Socket连接推送状态更新
                        await SendStatusUpdateToConnection(connectionId, statusChange);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"向连接 {connectionId} 推送状态更新失败");
                        // 移除失败的连接
                        UnregisterUserConnection(userId, connectionId);
                    }
                }
            }
        }
    }
    
    private async Task<List<string>> GetUsersToNotifyAsync(BillStatusChange statusChange)
    {
        // 根据状态变更内容，确定需要通知的用户列表
        // 实现逻辑包括：获取相关用户、审核人等
        // 这里仅为示例实现
        var users = new List<string>();
        
        // 添加当前处理用户
        if (!string.IsNullOrEmpty(statusChange.HandlerUserId))
        {
            users.Add(statusChange.HandlerUserId);
        }
        
        // 获取相关审核人
        var reviewers = await GetBillReviewersAsync(statusChange.BillType, statusChange.Status);
        users.AddRange(reviewers);
        
        return users.Distinct().ToList();
    }
    
    private async Task SendStatusUpdateToConnection(string connectionId, BillStatusChange statusChange)
    {
        // 通过Socket管理器发送消息
        var message = new SocketMessage
        {
            Type = MessageType.BillStatusChange,
            Data = statusChange
        };
        
        await SocketManager.SendToConnectionAsync(connectionId, message);
    }
    
    private async Task<List<string>> GetBillReviewersAsync(string billType, string status)
    {
        // 实现获取审核人列表的逻辑
        // 例如从数据库查询相关审核配置
        return new List<string>(); // 示例返回空列表
    }
}
```

#### 客户端UCTodoList状态更新
```csharp
public partial class UCTodoList : UserControl
{
    // 现有代码...
    
    private readonly ISocketClient _socketClient;
    
    public UCTodoList(IEntityMappingService mapper, EntityLoader loader, ILogger<UCTodoList> logger, ISocketClient socketClient)
    {
        InitializeComponent();
        _logger = logger;
        _mapper = mapper;
        _loader = loader;
        _socketClient = socketClient;
        // 通过依赖注入获取服务实例
        _menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        _conditionBuilderFactory = new ConditionBuilderFactory();
        
        // 注册状态更新事件处理器
        _socketClient.OnBillStatusChanged += HandleBillStatusChanged;
    }
    
    private async void HandleBillStatusChanged(object sender, BillStatusChangeEventArgs e)
    {
        try
        {
            // 确保在UI线程执行
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => HandleBillStatusChanged(sender, e)));
                return;
            }
            
            _logger.LogInformation($"收到单据状态变更通知: 单据类型={e.StatusChange.BillType}, 单据ID={e.StatusChange.BillId}");
            
            // 更新任务列表
            await UpdateTaskInTreeView(e.StatusChange);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "处理单据状态变更时发生错误");
        }
    }
    
    private async Task UpdateTaskInTreeView(BillStatusChange statusChange)
    {
        // 查找对应的树节点
        TreeNode nodeToUpdate = FindNodeByBillInfo(statusChange.BillType, statusChange.BillId);
        
        if (nodeToUpdate != null && nodeToUpdate.Tag is QueryParameter parameter)
        {
            // 更新节点状态
            UpdateNodeStatus(nodeToUpdate, statusChange.Status);
            
            // 可能需要刷新整个节点或分支
            if (ShouldRefreshEntireBranch(statusChange))
            {
                await RefreshTaskBranch(nodeToUpdate.Parent);
            }
        }
        else
        {
            // 如果找不到对应节点，可能需要添加新任务或刷新整个列表
            if (ShouldAddNewTask(statusChange))
            {
                await AddNewTaskToTreeView(statusChange);
            }
            else if (ShouldRemoveTask(statusChange))
            {
                // 某些状态变更可能需要从列表中移除任务
                RemoveTaskFromTreeView(statusChange);
            }
        }
    }
    
    private TreeNode FindNodeByBillInfo(string billType, string billId)
    {
        // 遍历树节点查找匹配的单据
        foreach (TreeNode node in kryptonTreeViewJobList.Nodes)
        {
            var foundNode = FindNodeByBillInfoRecursive(node, billType, billId);
            if (foundNode != null)
            {
                return foundNode;
            }
        }
        
        return null;
    }
    
    private TreeNode FindNodeByBillInfoRecursive(TreeNode node, string billType, string billId)
    {
        if (node.Tag is QueryParameter parameter && 
            parameter.tableType.Name == billType && 
            parameter.conditionals != null &&
            parameter.conditionals.Any(c => 
                c.FieldName == "ID" && 
                c.FieldValue == billId))
        {
            return node;
        }
        
        foreach (TreeNode childNode in node.Nodes)
        {
            var foundNode = FindNodeByBillInfoRecursive(childNode, billType, billId);
            if (foundNode != null)
            {
                return foundNode;
            }
        }
        
        return null;
    }
    
    private void UpdateNodeStatus(TreeNode node, string status)
    {
        // 更新节点的状态显示，例如改变节点文本、图标等
        if (node.Text.Contains("状态:"))
        {
            int statusIndex = node.Text.IndexOf("状态:");
            string prefix = node.Text.Substring(0, statusIndex);
            node.Text = $"{prefix}状态:{status}";
        }
        else
        {
            node.Text += $" (状态:{status})";
        }
        
        // 更新节点图标
        node.ImageIndex = GetStatusImageIndex(status);
        node.SelectedImageIndex = node.ImageIndex;
    }
    
    private int GetStatusImageIndex(string status)
    {
        // 根据状态返回对应的图标索引
        // 实现略
        return 0; // 默认图标
    }
    
    private bool ShouldRefreshEntireBranch(BillStatusChange statusChange)
    {
        // 判断是否需要刷新整个分支
        // 例如状态变为"已完成"可能需要从待办列表中移除
        return statusChange.Status == "已完成" || statusChange.Status == "已取消";
    }
    
    private async Task RefreshTaskBranch(TreeNode parentNode)
    {
        if (parentNode == null) return;
        
        // 保存父节点的展开状态
        bool wasExpanded = parentNode.IsExpanded;
        
        // 移除所有子节点
        parentNode.Nodes.Clear();
        
        // 重新加载该分支的数据
        await LoadTasksForBranch(parentNode);
        
        // 恢复展开状态
        parentNode.Expand();
    }
    
    private bool ShouldAddNewTask(BillStatusChange statusChange)
    {
        // 判断是否应该添加新任务
        // 例如新创建的待审核单据
        return statusChange.Status == "待审核" || statusChange.Status == "等待确认";
    }
    
    private bool ShouldRemoveTask(BillStatusChange statusChange)
    {
        // 判断是否应该移除任务
        // 例如状态变为已完成或已取消的单据
        return statusChange.Status == "已完成" || statusChange.Status == "已取消";
    }
    
    private async Task AddNewTaskToTreeView(BillStatusChange statusChange)
    {
        // 添加新任务到树视图
        // 实现略
    }
    
    private void RemoveTaskFromTreeView(BillStatusChange statusChange)
    {
        // 从树视图中移除任务
        // 实现略
    }
    
    private async Task LoadTasksForBranch(TreeNode parentNode)
    {
        // 加载指定分支的任务数据
        // 实现略，类似于现有的BuilderToDoListTreeView方法
    }
}
```

### 6.3 文档处理模块与分布式锁定集成

#### ActionManager增强实现
```csharp
public class DistributedActionManager : ActionManager
{
    private readonly ILockManagerService _lockManagerService;
    private readonly IBillStatusChangeNotifier _statusNotifier;
    
    public DistributedActionManager(
        DocumentConverterFactory converterFactory,
        ISqlSugarClient db,
        ILogger<DistributedActionManager> logger,
        ILockManagerService lockManagerService,
        IBillStatusChangeNotifier statusNotifier)
        : base(converterFactory, db, logger)
    {
        _lockManagerService = lockManagerService;
        _statusNotifier = statusNotifier;
    }
    
    public override async Task<ActionResult<TTarget>> ExecuteActionAsync<TSource, TTarget>(
        TSource source, 
        ActionOptions options = null)
    {
        // 获取源单据的锁定信息
        string resourceId = source.GetPropertyValue("ID")?.ToString();
        string resourceType = typeof(TSource).Name;
        
        // 在执行操作前，临时锁定相关单据
        LockRequest lockRequest = new LockRequest
        {
            ResourceId = resourceId,
            ResourceType = resourceType,
            UserId = options?.UserId ?? "system",
            UserName = options?.UserName ?? "系统",
            SessionId = options?.SessionId ?? Guid.NewGuid().ToString(),
            LockType = LockType.Temporary
        };
        
        LockResponse lockResponse = null;
        string lockId = null;
        
        try
        {
            // 尝试锁定源单据
            lockResponse = await _lockManagerService.TryLockDocumentAsync(lockRequest);
            if (!lockResponse.Success)
            {
                return new ActionResult<TTarget>(false, "操作失败: 单据已被锁定", lockResponse.Message);
            }
            
            lockId = lockResponse.LockId;
            
            // 执行原始的联动操作
            var result = await base.ExecuteActionAsync<TSource, TTarget>(source, options);
            
            if (result.Success)
            {
                // 发布状态变更通知
                await _statusNotifier.NotifyStatusChangeAsync(new BillStatusChange
                {
                    BillId = resourceId,
                    BillType = resourceType,
                    Status = GetNewStatus(result.Data),
                    HandlerUserId = options?.UserId ?? "system",
                    HandlerUserName = options?.UserName ?? "系统",
                    Timestamp = DateTime.Now
                });
                
                // 如果是转换操作，也通知目标单据的状态变更
                if (typeof(TSource) != typeof(TTarget))
                {
                    string targetResourceId = result.Data.GetPropertyValue("ID")?.ToString();
                    string targetResourceType = typeof(TTarget).Name;
                    
                    await _statusNotifier.NotifyStatusChangeAsync(new BillStatusChange
                    {
                        BillId = targetResourceId,
                        BillType = targetResourceType,
                        Status = GetNewStatus(result.Data),
                        HandlerUserId = options?.UserId ?? "system",
                        HandlerUserName = options?.UserName ?? "系统",
                        Timestamp = DateTime.Now
                    });
                }
            }
            
            return result;
        }
        catch (Exception ex)
        {
            return new ActionResult<TTarget>(false, "操作执行失败: " + ex.Message);
        }
        finally
        {
            // 无论操作成功与否，都释放临时锁定
            if (lockResponse?.Success == true && !string.IsNullOrEmpty(lockId))
            {
                await _lockManagerService.UnlockDocumentAsync(lockId, resourceId, resourceType);
            }
        }
    }
    
    private string GetNewStatus(BaseEntity entity)
    {
        // 获取实体的状态属性值
        return entity.GetPropertyValue("Status")?.ToString() ?? "未知";
    }
}
```

## 7. 集成与部署方案

### 7.1 与现有系统的集成

1. **依赖注入配置**：在系统的依赖注入容器中注册新的分布式锁实现和相关服务。

2. **现有代码适配**：对现有代码进行最小化修改，确保无缝集成新的分布式锁定机制。

3. **数据库迁移**：确保Redis数据库正确配置，包括连接池大小、超时设置等。

4. **服务启动顺序**：确保Redis服务在应用服务之前启动，避免启动时出现连接错误。

### 7.2 部署建议

1. **Redis配置**：
   - 使用Redis集群确保高可用性
   - 配置合理的内存策略和持久化选项
   - 设置适当的连接池参数

2. **应用服务器配置**：
   - 调整应用服务器的连接池设置
   - 配置适当的超时参数
   - 实现健康检查和自动重启机制

3. **监控与告警**：
   - 监控Redis性能指标
   - 监控分布式锁使用情况
   - 设置异常告警机制

## 8. 测试计划

### 8.1 单元测试

1. **分布式锁单元测试**：测试锁获取、释放、续期等核心功能。

2. **锁管理器服务测试**：测试锁定、解锁、强制解锁等服务方法。

3. **状态同步测试**：测试状态变更通知和同步机制。

### 8.2 集成测试

1. **分布式环境测试**：在多服务器环境下测试锁定机制的一致性。

2. **高并发测试**：模拟大量并发请求，测试锁竞争情况下的表现。

3. **故障恢复测试**：测试系统在网络中断、服务重启等情况下的恢复能力。

### 8.3 性能测试

1. **锁定操作性能**：测试锁定和解锁操作的响应时间。

2. **状态同步延迟**：测试状态变更到客户端更新的延迟。

3. **系统负载测试**：测试系统在高负载下的性能表现。

## 9. 总结与展望

本设计方案提供了一套完整的分布式锁定机制和单据状态同步解决方案，基于"先编辑先拥有"原则，确保多用户环境下的并发编辑安全。通过使用Redis分布式锁、事件驱动设计和WebSocket实时通信，解决了现有系统在分布式环境下的局限性。

未来，可以进一步优化以下方面：

1. **锁粒度的动态调整**：根据数据访问模式动态调整锁的粒度。

2. **智能锁定预测**：基于用户行为模式预测可能的锁定冲突，提前优化。

3. **更完善的监控系统**：实现分布式锁使用情况的可视化监控。

4. **边缘缓存优化**：在客户端实现智能缓存，减少网络请求和锁定竞争。