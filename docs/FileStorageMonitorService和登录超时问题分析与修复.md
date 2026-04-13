# FileStorageMonitorService和登录超时问题分析与修复

## 问题概述

### 问题1: FileStorageMonitorService 每30分钟报错

**现象**:
```
2026-04-12 0:11:10 ERROR RUINORERP.Server.Network.Services.FileStorageMonitorService
2026-04-12 0:41:10 ERROR RUINORERP.Server.Network.Services.FileStorageMonitorService
...（每30分钟一次）
```

**特点**:
- 固定时间间隔（整点和半点）
- 只有ERROR级别，没有具体错误信息
- 持续一整天

### 问题2: 客户端登录超时

**现象**:
```
2026-04-12 23:07:49 ERROR RUINORERP.UI.Network.ClientCommunicationService 
发送带响应命令失败: CommandId=0x0100:Login, RequestId=Login_230734172_..., 
Error=请求超时（20000ms）
```

**特点**:
- 登录请求超时20秒
- 发生在欢迎流程验证超时后
- 客户端主动断开连接

## 根本原因分析

### FileStorageMonitorService 问题分析

#### 代码流程

1. **定时器触发** (每30分钟)
   ```csharp
   _monitorTimer = new Timer(
       _ => OnTimerElapsed(),  // 第84行
       null,
       TimeSpan.FromMinutes(MonitorIntervalMinutes),
       TimeSpan.FromMinutes(MonitorIntervalMinutes)
   );
   ```

2. **异步执行检查**
   ```csharp
   private void OnTimerElapsed()
   {
       Task.Run(async () =>
       {
           try
           {
               await CheckStorageSpaceAsync();  // 第100行
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "定时检查存储空间失败: {ErrorType} - {ErrorMessage}", 
                   ex.GetType().Name, ex.Message);  // 第105行
           }
       });
   }
   ```

3. **获取统计数据**
   ```csharp
   var stats = await _fileCleanupService.GetCleanupStatisticsAsync();  // 第133行
   ```

4. **数据库查询** (FileCleanupService.cs 第369-396行)
   ```csharp
   public async Task<FileCleanupStatistics> GetCleanupStatisticsAsync()
   {
       var db = _unitOfWorkManage.GetDbClient();
       
       // 查询所有未删除的文件信息
       var files = await db.Queryable<tb_FS_FileStorageInfo>()
           .Where(f => f.isdeleted == false)
           .Select(f => new { f.FileStatus, FileSize = f.FileSize > 0 ? f.FileSize : 0 })
           .ToListAsync();  // 可能在这里卡住或抛出异常
       
       // ...
   }
   ```

#### 可能的原因

**原因1: 数据库连接池耗尽** ⭐⭐⭐⭐⭐
- 服务器长时间运行后，数据库连接可能没有正确释放
- `GetCleanupStatisticsAsync` 尝试获取数据库连接时超时
- 抛出的异常被捕获，但日志格式可能有问题导致详细信息丢失

**原因2: 慢查询**
- `tb_FS_FileStorageInfo` 表数据量过大
- 缺少索引导致查询缓慢
- 超过默认的SQL超时时间

**原因3: 死锁**
- 其他事务持有文件表的锁
- `GetCleanupStatisticsAsync` 等待锁释放超时

**原因4: 日志配置问题**
- 虽然之前修复了日志仓库问题
- 但可能日志输出格式或Appender配置仍有问题
- 导致异常堆栈信息未能完整记录

### 登录超时问题分析

#### 代码流程

1. **客户端发送登录请求**
   ```csharp
   // ClientCommunicationService.cs
   var response = await SendCommandWithResponseAsync<LoginResponse>(
       AuthenticationCommands.Login,
       loginRequest,
       ct,
       20000);  // 20秒超时
   ```

2. **服务器端处理登录** (LoginCommandHandler.cs)
   ```csharp
   protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
   {
       // 第209行
       return await ProcessLoginAsync(loginRequest, cmd.Packet.ExecutionContext, cancellationToken);
   }
   ```

3. **防暴力破解延迟**
   ```csharp
   private async Task<tb_UserInfo> ValidateUserCredentialsAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
   {
       await Task.Delay(500, cancellationToken);  // 第636行 - 固定500ms延迟
       
       // 数据库查询验证用户
       var user = await Program.AppContextData.Db.CopyNew().Queryable<tb_UserInfo>()
                .Where(u => u.UserName == loginRequest.Username && u.Password == EnPassword)
                .Includes(x => x.tb_employee)
                .Includes(x => x.tb_User_Roles)
                .SingleAsync();  // 第644行 - 可能在这里卡住
       return user;
   }
   ```

4. **生成Token**
   ```csharp
   var tokenInfo = await GenerateTokenInfoAsync(userInfo, sessionInfo.SessionID, loginRequest.ClientIp);  // 第361行
   ```

#### 可能的原因

**原因1: 数据库连接问题** ⭐⭐⭐⭐⭐
- 与FileStorageMonitorService相同的问题
- 登录时需要查询用户信息、员工信息、角色信息（Includes）
- 如果数据库连接池耗尽或查询缓慢，会导致超时

**原因2: Token生成缓慢**
- JWT Token生成涉及加密操作
- 如果系统负载高，可能耗时较长

**原因3: 会话服务阻塞**
- `SessionService.CreateSession` 或 `UpdateSession` 可能阻塞
- 特别是如果有大量并发会话

**原因4: 网络延迟**
- 客户端到服务器的网络延迟过高
- 但考虑到是局域网（192.168.0.99），可能性较低

## 关联性分析

两个问题很可能有**共同的根源**：**数据库连接问题**

### 证据链

1. **时间相关性**
   - FileStorageMonitorService 每30分钟执行一次数据库查询
   - 如果数据库连接有问题，会定期失败
   - 登录也需要数据库查询，同样受影响

2. **症状一致性**
   - 都是超时类问题
   - 都涉及数据库操作
   - 都发生在服务器长时间运行后

3. **资源竞争**
   - FileStorageMonitorService 使用 `_dbLock` 保护数据库操作
   - 但如果锁等待时间过长，会影响其他数据库操作
   - 登录请求可能因为等待数据库连接而超时

## 修复方案

### 短期修复（立即实施）

#### 1. 增强FileStorageMonitorService的异常处理

**文件**: `RUINORERP.Server\Network\Services\FileStorageMonitorService.cs`

**修改点1**: 改进OnTimerElapsed的日志记录

```csharp
private void OnTimerElapsed()
{
    Task.Run(async () =>
    {
        var startTime = DateTime.Now;
        try
        {
            _logger.LogDebug("[FileStorageMonitor] 开始定时检查存储空间");
            await CheckStorageSpaceAsync();
            var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            _logger.LogDebug("[FileStorageMonitor] 定时检查完成，耗时: {ElapsedMs}ms", elapsed);
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "[FileStorageMonitor] 定时检查被取消");
        }
        catch (TimeoutException ex)
        {
            _logger.LogError(ex, "[FileStorageMonitor] 定时检查超时: {ErrorMessage}\n{StackTrace}", 
                ex.Message, ex.StackTrace);
        }
        catch (SqlSugar.SqlSugarException ex)
        {
            _logger.LogError(ex, "[FileStorageMonitor] 数据库操作失败: {ErrorMessage}\n{StackTrace}\nSQL: {Sql}", 
                ex.Message, ex.StackTrace, ex.Sql ?? "N/A");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[FileStorageMonitor] 定时检查存储空间失败: {ErrorType} - {ErrorMessage}\n{StackTrace}", 
                ex.GetType().Name, ex.Message, ex.StackTrace);
        }
    });
}
```

**修改点2**: 为CheckStorageSpaceAsync添加超时保护

```csharp
private async Task CheckStorageSpaceAsync()
{
    // ✅ 添加超时保护，避免长时间阻塞
    using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(2)))
    {
        try
        {
            _logger.LogDebug("[FileStorageMonitor] 开始检查文件存储空间");

            // 获取文件统计信息（添加超时保护）
            var stats = await GetCleanupStatisticsWithTimeoutAsync(cts.Token);
            
            // ... 其余逻辑不变
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("[FileStorageMonitor] 检查存储空间超时（2分钟）");
            throw new TimeoutException("检查存储空间操作超时");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[FileStorageMonitor] 检查文件存储空间失败: {ErrorType} - {ErrorMessage}\n{StackTrace}", 
                ex.GetType().Name, ex.Message, ex.StackTrace);
            throw; // 重新抛出，让上层捕获
        }
    }
}

/// <summary>
/// 带超时保护的统计数据获取
/// </summary>
private async Task<FileCleanupStatistics> GetCleanupStatisticsWithTimeoutAsync(CancellationToken ct)
{
    try
    {
        return await _fileCleanupService.GetCleanupStatisticsAsync();
    }
    catch (TaskCanceledException) when (ct.IsCancellationRequested)
    {
        throw new OperationCanceledException("获取清理统计数据超时", ct);
    }
}
```

#### 2. 优化FileCleanupService的查询性能

**文件**: `RUINORERP.Server\Network\Services\FileCleanupService.cs`

**修改**: 为GetCleanupStatisticsAsync添加查询优化和详细日志

```csharp
public async Task<FileCleanupStatistics> GetCleanupStatisticsAsync()
{
    var startTime = DateTime.Now;
    IDbContext db = null;
    
    try
    {
        db = _unitOfWorkManage.GetDbClient();
        
        _logger.LogDebug("[FileCleanup] 开始获取清理统计数据");

        // ✅ 优化查询：使用CountAsync而不是ToList + Count，减少内存占用
        var totalFiles = await db.Queryable<tb_FS_FileStorageInfo>()
            .Where(f => f.isdeleted == false)
            .CountAsync();

        var activeFiles = await db.Queryable<tb_FS_FileStorageInfo>()
            .Where(f => f.isdeleted == false && f.FileStatus == (int)FileStatus.Active)
            .CountAsync();

        var expiredFiles = await db.Queryable<tb_FS_FileStorageInfo>()
            .Where(f => f.isdeleted == false && f.FileStatus == (int)FileStatus.Expired)
            .CountAsync();

        var orphanedFiles = await db.Queryable<tb_FS_FileStorageInfo>()
            .Where(f => f.isdeleted == false && f.FileStatus == (int)FileStatus.Orphaned)
            .CountAsync();

        // 查询总存储大小
        var totalStorageSize = await db.Queryable<tb_FS_FileStorageInfo>()
            .Where(f => f.isdeleted == false)
            .SumAsync(f => f.FileSize > 0 ? f.FileSize : 0);

        // 按方案A：统计已删除的关联记录数
        var deletedRelationsCount = await db.Queryable<tb_FS_BusinessRelation>()
            .Where(r => r.isdeleted == true)
            .CountAsync();

        var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
        _logger.LogDebug("[FileCleanup] 获取清理统计数据完成，耗时: {ElapsedMs}ms", elapsed);

        var stats = new FileCleanupStatistics
        {
            TotalFiles = totalFiles,
            ActiveFiles = activeFiles,
            ExpiredFiles = expiredFiles,
            OrphanedFiles = orphanedFiles,
            DeletedFiles = deletedRelationsCount,
            TotalStorageSize = totalStorageSize
        };

        return stats;
    }
    catch (SqlSugar.SqlSugarException ex)
    {
        var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
        _logger.LogError(ex, "[FileCleanup] 获取清理统计数据失败，耗时: {ElapsedMs}ms\nSQL: {Sql}", 
            elapsed, ex.Sql ?? "N/A");
        throw;
    }
    catch (Exception ex)
    {
        var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
        _logger.LogError(ex, "[FileCleanup] 获取清理统计数据异常，耗时: {ElapsedMs}ms", elapsed);
        throw;
    }
}
```

#### 3. 增强登录处理的日志和超时控制

**文件**: `RUINORERP.Server\Network\CommandHandlers\LoginCommandHandler.cs`

**修改**: 在ProcessLoginAsync中添加详细的性能日志

```csharp
private async Task<IResponse> ProcessLoginAsync(
    LoginRequest loginRequest,
    CommandContext executionContext,
    CancellationToken cancellationToken)
{
    var startTime = DateTime.Now;
    var stepTimes = new Dictionary<string, long>();
    
    try
    {
        logger?.LogInformation("[Login] 开始处理登录请求 - Username: {Username}, SessionId: {SessionId}", 
            loginRequest.Username, executionContext.SessionId);

        // 步骤1: 时间验证
        var step1Start = DateTime.Now;
        var serverTime = DateTime.Now;
        var clientTime = loginRequest.LoginTime;
        var timeDifference = Math.Abs((serverTime - clientTime).TotalSeconds);
        stepTimes["TimeValidation"] = (DateTime.Now - step1Start).TotalMilliseconds;

        const double timeDifferenceThreshold = 300.0;
        if (timeDifference > timeDifferenceThreshold)
        {
            return ResponseFactory.CreateSpecificErrorResponse(executionContext, 
                new Exception($"客户端时间与服务器时间差异过大 ({timeDifference:F0}秒)，请校准系统时间"));
        }

        // 步骤2: 参数验证
        var step2Start = DateTime.Now;
        if (!loginRequest.IsValid())
        {
            return ResponseFactory.CreateSpecificErrorResponse(executionContext, 
                new Exception("用户名和密码不能为空"));
        }
        stepTimes["ParameterValidation"] = (DateTime.Now - step2Start).TotalMilliseconds;

        // 步骤3: 并发用户数检查
        var step3Start = DateTime.Now;
        if (SessionService.ActiveSessionCount >= MaxConcurrentUsers)
        {
            return ResponseFactory.CreateSpecificErrorResponse(executionContext, 
                "当前系统用户数已达到上限，请稍后再试");
        }
        stepTimes["ConcurrentCheck"] = (DateTime.Now - step3Start).TotalMilliseconds;

        // 步骤4: 会话管理
        var step4Start = DateTime.Now;
        var sessionInfo = SessionService.GetSession(executionContext.SessionId);
        
        if (sessionInfo == null && loginRequest != null && !string.IsNullOrEmpty(loginRequest.SessionId))
        {
            sessionInfo = SessionService.GetSession(loginRequest.SessionId);
        }
        
        if (sessionInfo == null)
        {
            sessionInfo = SessionService.CreateSession(executionContext.SessionId);
            
            if (sessionInfo == null)
            {
                return ResponseFactory.CreateSpecificErrorResponse(executionContext, "创建会话失败");
            }
        }
        stepTimes["SessionManagement"] = (DateTime.Now - step4Start).TotalMilliseconds;

        sessionInfo.ClientIp = loginRequest.ClientIp;
        if (string.IsNullOrEmpty(sessionInfo.ClientIp))
        {
            sessionInfo.ClientIp = GetClientIp(sessionInfo);
        }

        // 步骤5: 黑名单检查
        var step5Start = DateTime.Now;
        if (IsUserBlacklisted(loginRequest.Username, loginRequest.ClientIp))
        {
            return ResponseFactory.CreateSpecificErrorResponse(executionContext, "用户或IP在黑名单中");
        }
        stepTimes["BlacklistCheck"] = (DateTime.Now - step5Start).TotalMilliseconds;

        // 步骤6: 登录尝试次数检查
        var step6Start = DateTime.Now;
        if (GetLoginAttempts(loginRequest.Username) >= MaxLoginAttempts)
        {
            return ResponseFactory.CreateSpecificErrorResponse(executionContext, 
                "登录尝试次数过多，请稍后再试");
        }
        stepTimes["AttemptCheck"] = (DateTime.Now - step6Start).TotalMilliseconds;

        // 步骤7: 验证用户凭据（最耗时的步骤）
        var step7Start = DateTime.Now;
        logger?.LogDebug("[Login] 开始验证用户凭据 - Username: {Username}", loginRequest.Username);
        var userInfo = await ValidateUserCredentialsAsync(loginRequest, cancellationToken);
        stepTimes["CredentialValidation"] = (DateTime.Now - step7Start).TotalMilliseconds;
        
        if (userInfo == null)
        {
            IncrementLoginAttempts(loginRequest.Username);
            logger?.LogWarning("[Login] 用户凭据验证失败 - Username: {Username}, 耗时: {ElapsedMs}ms", 
                loginRequest.Username, stepTimes["CredentialValidation"]);
            return ResponseFactory.CreateSpecificErrorResponse(executionContext, "用户名或密码错误");
        }
        
        logger?.LogDebug("[Login] 用户凭据验证成功 - Username: {Username}, UserId: {UserId}, 耗时: {ElapsedMs}ms", 
            loginRequest.Username, userInfo.User_ID, stepTimes["CredentialValidation"]);

        ResetLoginAttempts(loginRequest.Username);

        // 步骤8: 检查重复登录
        var step8Start = DateTime.Now;
        var (hasExistingSessions, authorizedSessions, duplicateResult) = 
            CheckUserLoginStatus(loginRequest.Username, executionContext.SessionId);
        stepTimes["DuplicateCheck"] = (DateTime.Now - step8Start).TotalMilliseconds;

        if (duplicateResult.HasDuplicateLogin)
        {
            if (duplicateResult.Type == DuplicateLoginType.LocalOnly && duplicateResult.AllowMultipleLocalSessions)
            {
                logger?.LogDebug($"[Login] 用户 {loginRequest.Username} 本地重复登录，允许多会话");
            }
        }

        // 步骤9: 更新会话信息
        var step9Start = DateTime.Now;
        UpdateSessionInfo(sessionInfo, userInfo);
        sessionInfo.IsAuthenticated = true;
        SessionService.UpdateSession(sessionInfo);
        stepTimes["SessionUpdate"] = (DateTime.Now - step9Start).TotalMilliseconds;

        // 步骤10: 生成Token
        var step10Start = DateTime.Now;
        logger?.LogDebug("[Login] 开始生成Token - Username: {Username}", loginRequest.Username);
        var tokenInfo = await GenerateTokenInfoAsync(userInfo, sessionInfo.SessionID, loginRequest.ClientIp);
        stepTimes["TokenGeneration"] = (DateTime.Now - step10Start).TotalMilliseconds;
        logger?.LogDebug("[Login] Token生成成功 - Username: {Username}, 耗时: {ElapsedMs}ms", 
            loginRequest.Username, stepTimes["TokenGeneration"]);

        // 步骤11: 检查注册状态
        var step11Start = DateTime.Now;
        var registrationStatus = await RegistrationService.GetRegistrationStatusAsync();
        var expirationReminder = await RegistrationService.GetExpirationReminderInfoAsync();
        stepTimes["RegistrationCheck"] = (DateTime.Now - step11Start).TotalMilliseconds;

        if (registrationStatus == RegistrationStatus.Expired)
        {
            return ResponseFactory.CreateSpecificErrorResponse(executionContext, 
                "系统注册已过期，请联系软件提供商续费。续费方式：请联系软件提供商");
        }

        // 构建响应
        var loginResponse = new LoginResponse
        {
            IsSuccess = true,
            Message = "登录成功",
            UserId = userInfo.User_ID,
            Username = userInfo.UserName,
            SessionId = sessionInfo.SessionID,
            Token = tokenInfo,
            HasDuplicateLogin = duplicateResult.HasDuplicateLogin,
            DuplicateLoginResult = duplicateResult,
            RegistrationStatus = registrationStatus,
            ExpirationReminder = expirationReminder
        };

        loginResponse.WithMetadata("HeartbeatIntervalMs", "30000")
            .WithMetadata("ServerInfo", new Dictionary<string, object>
            {
                ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ["ServerVersion"] = "1.0.0",
                ["MaxConcurrentUsers"] = MaxConcurrentUsers.ToString(),
                ["CurrentActiveUsers"] = SessionService.ActiveSessionCount.ToString()
            });

        var totalTime = (DateTime.Now - startTime).TotalMilliseconds;
        logger?.LogInformation("[Login] 登录成功 - Username: {Username}, UserId: {UserId}, 总耗时: {TotalMs}ms, 各步骤耗时: {@StepTimes}", 
            loginRequest.Username, userInfo.User_ID, totalTime, stepTimes);

        return loginResponse;
    }
    catch (OperationCanceledException ex)
    {
        var totalTime = (DateTime.Now - startTime).TotalMilliseconds;
        logger?.LogWarning(ex, "[Login] 登录请求被取消 - Username: {Username}, 已耗时: {TotalMs}ms, 各步骤耗时: {@StepTimes}", 
            loginRequest.Username, totalTime, stepTimes);
        return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "登录请求被取消");
    }
    catch (SqlSugar.SqlSugarException ex)
    {
        var totalTime = (DateTime.Now - startTime).TotalMilliseconds;
        logger?.LogError(ex, "[Login] 数据库操作失败 - Username: {Username}, 已耗时: {TotalMs}ms, SQL: {Sql}, 各步骤耗时: {@StepTimes}", 
            loginRequest.Username, totalTime, ex.Sql ?? "N/A", stepTimes);
        return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "数据库操作失败，请稍后重试");
    }
    catch (Exception ex)
    {
        var totalTime = (DateTime.Now - startTime).TotalMilliseconds;
        logger?.LogError(ex, "[Login] 处理登录异常 - Username: {Username}, 已耗时: {TotalMs}ms, 各步骤耗时: {@StepTimes}", 
            loginRequest.Username, totalTime, stepTimes);
        return ResponseFactory.CreateSpecificErrorResponse(executionContext, ex, "处理登录异常");
    }
}
```

### 中期优化（1-2周内实施）

#### 1. 数据库连接池优化

**检查项**:
- 当前连接池大小配置
- 连接泄漏检测
- 连接超时设置

**建议配置**:
```csharp
// 在数据库连接字符串中添加
"Max Pool Size=200;Min Pool Size=5;Connection Lifetime=300;Connection Timeout=30;"
```

#### 2. 添加数据库查询监控

创建中间件或拦截器，记录慢查询：

```csharp
public class SlowQueryLogger : SqlSugar.IAopService
{
    private readonly ILogger<SlowQueryLogger> _logger;
    private const int SlowQueryThresholdMs = 1000; // 1秒

    public void OnExecuting(System.Data.Common.DbCommand command, AopEventType eventType)
    {
        var startTime = DateTime.Now;
        
        // 在命令执行完成后记录
        command.ExecuteNonQueryAsync().ContinueWith(task =>
        {
            var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
            if (elapsed > SlowQueryThresholdMs)
            {
                _logger.LogWarning("[慢查询] 耗时: {ElapsedMs}ms, SQL: {Sql}", 
                    elapsed, command.CommandText);
            }
        });
    }
}
```

#### 3. 优化FileStorageMonitorService的执行策略

**改进**:
- 将监控间隔从30分钟调整为60分钟（如果不需要频繁监控）
- 添加指数退避重试机制
- 在服务器低峰期执行（如凌晨2-4点）

```csharp
public int MonitorIntervalMinutes { get; set; } = 60; // 改为60分钟

// 或者使用智能调度
private async Task SmartScheduleMonitoringAsync()
{
    var now = DateTime.Now;
    var hour = now.Hour;
    
    // 在低峰期（凌晨2-6点）更频繁监控
    if (hour >= 2 && hour <= 6)
    {
        MonitorIntervalMinutes = 30;
    }
    else
    {
        MonitorIntervalMinutes = 60;
    }
}
```

### 长期优化（1个月内实施）

#### 1. 实现数据库健康检查服务

定期检查数据库连接状态，提前发现问题：

```csharp
public class DatabaseHealthCheckService : IHostedService
{
    private readonly IUnitOfWorkManage _unitOfWorkManage;
    private readonly ILogger<DatabaseHealthCheckService> _logger;
    private Timer _healthCheckTimer;

    public void Start()
    {
        _healthCheckTimer = new Timer(
            _ => CheckDatabaseHealthAsync(),
            null,
            TimeSpan.FromMinutes(5),
            TimeSpan.FromMinutes(5)
        );
    }

    private async Task CheckDatabaseHealthAsync()
    {
        try
        {
            var db = _unitOfWorkManage.GetDbClient();
            var result = await db.Ado.GetStringAsync("SELECT 1");
            
            if (result == "1")
            {
                _logger.LogDebug("[健康检查] 数据库连接正常");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[健康检查] 数据库连接异常");
            // 可以触发告警
        }
    }
}
```

#### 2. 添加熔断器模式

防止数据库问题导致级联故障：

```csharp
public class DatabaseCircuitBreaker
{
    private CircuitState _state = CircuitState.Closed;
    private int _failureCount = 0;
    private DateTime _lastFailureTime;
    private readonly int _threshold = 5;
    private readonly TimeSpan _resetTimeout = TimeSpan.FromMinutes(5);

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
    {
        if (_state == CircuitState.Open)
        {
            if (DateTime.Now - _lastFailureTime > _resetTimeout)
            {
                _state = CircuitState.HalfOpen;
            }
            else
            {
                throw new CircuitBreakerOpenException("数据库熔断器已打开");
            }
        }

        try
        {
            var result = await operation();
            
            if (_state == CircuitState.HalfOpen)
            {
                _state = CircuitState.Closed;
                _failureCount = 0;
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _failureCount++;
            _lastFailureTime = DateTime.Now;
            
            if (_failureCount >= _threshold)
            {
                _state = CircuitState.Open;
            }
            
            throw;
        }
    }
}
```

## 验证方法

### 1. 监控日志输出

修复后，应该能看到：

**FileStorageMonitorService**:
```
[FileStorageMonitor] 开始定时检查存储空间
[FileCleanup] 开始获取清理统计数据
[FileCleanup] 获取清理统计数据完成，耗时: 123ms
[FileStorageMonitor] 定时检查完成，耗时: 145ms
```

**如果出现错误**:
```
[FileStorageMonitor] 定时检查超时（2分钟）
[FileCleanup] 获取清理统计数据失败，耗时: 120000ms
SQL: SELECT COUNT(*) FROM tb_FS_FileStorageInfo WHERE isdeleted = 0
```

**LoginCommandHandler**:
```
[Login] 开始处理登录请求 - Username: admin, SessionId: xxx
[Login] 开始验证用户凭据 - Username: admin
[Login] 用户凭据验证成功 - Username: admin, UserId: 1, 耗时: 234ms
[Login] 开始生成Token - Username: admin
[Login] Token生成成功 - Username: admin, 耗时: 45ms
[Login] 登录成功 - Username: admin, UserId: 1, 总耗时: 567ms, 各步骤耗时: {...}
```

### 2. 压力测试

模拟多用户同时登录，观察：
- 登录成功率
- 平均响应时间
- 数据库连接使用情况

### 3. 长时间运行测试

让服务器连续运行24-48小时，观察：
- FileStorageMonitorService是否还定期报错
- 数据库连接池是否稳定
- 内存使用情况

## 总结

### 核心问题

两个问题的根本原因很可能是**数据库连接管理不当**：
1. 连接池配置不合理
2. 缺少连接泄漏检测
3. 慢查询未优化
4. 缺少超时保护和熔断机制

### 优先级

1. **P0 - 立即修复**: 增强日志记录，添加超时保护
2. **P1 - 本周内**: 优化数据库查询，调整连接池配置
3. **P2 - 本月内**: 实现健康检查和熔断器

### 预期效果

修复后应该能够：
- ✅ FileStorageMonitorService不再定期报错
- ✅ 登录响应时间在2秒以内
- ✅ 即使数据库暂时不可用，也能优雅降级而不是超时
- ✅ 能够通过日志快速定位问题根因
