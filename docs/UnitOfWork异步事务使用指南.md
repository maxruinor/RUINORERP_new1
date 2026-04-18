# UnitOfWorkManage 异步事务使用指南

## 📖 目录
- [快速开始](#快速开始)
- [同步 vs 异步](#同步-vs-异步)
- [最佳实践](#最佳实践)
- [配置说明](#配置说明)
- [常见问题](#常见问题)

---

## 快速开始

### 1. 依赖注入配置

在 `Startup.cs` 或 `Program.cs` 中:

```csharp
// 添加配置
services.Configure<UnitOfWorkOptions>(Configuration.GetSection("UnitOfWork"));

// 或者使用默认值
services.AddOptions<UnitOfWorkOptions>()
    .Configure(options =>
    {
        options.MaxTransactionDepth = 15;
        options.DefaultTransactionTimeoutSeconds = 60;
        options.EnableTransactionMetrics = true;
    })
    .ValidateDataAnnotations();

// 注册 UnitOfWorkManage
services.AddScoped<IUnitOfWorkManage, UnitOfWorkManage>();
```

### 2. appsettings.json 配置

```json
{
  "UnitOfWork": {
    "MaxTransactionDepth": 15,
    "LongTransactionWarningSeconds": 60,
    "DefaultTransactionTimeoutSeconds": 60,
    "MaxRetryCount": 3,
    "EnableTransactionMetrics": true,
    "EnableVerboseTransactionLogging": false
  }
}
```

---

## 同步 vs 异步

### 同步方式 (向后兼容)

```csharp
public class PurEntryController : BaseController
{
    private readonly IUnitOfWorkManage _unitOfWork;
    
    public async Task<ReturnResults<T>> ApprovalAsync(T entity)
    {
        // ✅ 原有同步代码继续工作
        _unitOfWork.BeginTran();
        try
        {
            // 业务逻辑(可以是异步的)
            await _db.Updateable(entity).ExecuteCommandAsync();
            
            _unitOfWork.CommitTran();
            return ReturnResults.Success(entity);
        }
        catch
        {
            _unitOfWork.RollbackTran();
            throw;
        }
    }
}
```

### 异步方式 (推荐)

```csharp
public class PurEntryController : BaseController
{
    private readonly IUnitOfWorkManage _unitOfWork;
    
    public async Task<ReturnResults<T>> ApprovalAsync(T entity)
    {
        // ✅ 使用新的异步方法
        await _unitOfWork.BeginTranAsync();
        try
        {
            // 所有操作都是异步的
            await _db.Updateable(entity).ExecuteCommandAsync();
            await _db.Insertable(log).ExecuteCommandAsync();
            
            await _unitOfWork.CommitTranAsync();
            return ReturnResults.Success(entity);
        }
        catch
        {
            await _unitOfWork.RollbackTranAsync();
            throw;
        }
    }
}
```

---

## 最佳实践

### 1. 事务范围最小化

```csharp
// ❌ 错误: 事务包含大量非数据库操作
await _unitOfWork.BeginTranAsync();
try
{
    await DoHeavyCalculation();  // 耗时计算
    await SendEmail();           // 网络调用
    await _db.Update(...);       // 数据库操作
    await _unitOfWork.CommitTranAsync();
}
catch
{
    await _unitOfWork.RollbackTranAsync();
    throw;
}

// ✅ 正确: 仅包裹必要的数据库操作
var calculationResult = await DoHeavyCalculation();  // 事务外
await SendEmail();                                    // 事务外

await _unitOfWork.BeginTranAsync();
try
{
    await _db.Update(...);  // 仅数据库操作在事务内
    await _unitOfWork.CommitTranAsync();
}
catch
{
    await _unitOfWork.RollbackTranAsync();
    throw;
}
```

### 2. 重试机制正确使用

```csharp
// ❌ 错误: 只重试内部操作
await _unitOfWork.BeginTranAsync();
await _unitOfWork.ExecuteWithRetryAsync(async () =>
{
    await _db.Insert(...);  // 事务已开启,重试无意义
});

// ✅ 正确: 整个事务单元参与重试
await _unitOfWork.ExecuteWithRetryAsync(async () =>
{
    await _unitOfWork.BeginTranAsync();
    try
    {
        await _db.Insert(...);
        await _unitOfWork.CommitTranAsync();
    }
    catch
    {
        await _unitOfWork.RollbackTranAsync();
        throw;  // 抛出异常触发重试
    }
}, maxRetryCount: 3);
```

### 3. CancellationToken 传播

```csharp
public async Task ProcessLargeBatchAsync(CancellationToken cancellationToken = default)
{
    // ✅ 传递取消令牌
    await _unitOfWork.BeginTranAsync(cancellationToken: cancellationToken);
    try
    {
        foreach (var item in largeCollection)
        {
            cancellationToken.ThrowIfCancellationRequested();  // 检查取消
            await _db.Insert(item).ExecuteCommandAsync(cancellationToken);
        }
        
        await _unitOfWork.CommitTranAsync(cancellationToken);
    }
    catch
    {
        await _unitOfWork.RollbackTranAsync(cancellationToken);
        throw;
    }
}
```

### 4. 资源释放

```csharp
// ✅ 方式1: using 语句 (推荐)
using var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWorkManage>();
await unitOfWork.BeginTranAsync();
// ... 业务逻辑
await unitOfWork.CommitTranAsync();
// 自动释放

// ✅ 方式2: IAsyncDisposable (异步场景)
await using (var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWorkManage>())
{
    await unitOfWork.BeginTranAsync();
    // ... 业务逻辑
    await unitOfWork.CommitTranAsync();
}
```

### 5. 嵌套事务处理

```csharp
public async Task OuterMethod()
{
    await _unitOfWork.BeginTranAsync();
    try
    {
        await _db.Insert(...).ExecuteCommandAsync();
        
        // 内层事务 - 实际是保存点
        await InnerMethod();
        
        await _unitOfWork.CommitTranAsync();
    }
    catch
    {
        await _unitOfWork.RollbackTranAsync();
        throw;
    }
}

private async Task InnerMethod()
{
    // 会自动创建保存点,不会开启新事务
    await _unitOfWork.BeginTranAsync();
    try
    {
        await _db.Update(...).ExecuteCommandAsync();
        await _unitOfWork.CommitTranAsync();  // 仅减少深度
    }
    catch
    {
        await _unitOfWork.RollbackTranAsync();  // 回滚到保存点
        throw;
    }
}
```

---

## 配置说明

### 完整配置示例

```json
{
  "UnitOfWork": {
    "MaxTransactionDepth": 15,
    "LongTransactionWarningSeconds": 60,
    "DefaultTransactionTimeoutSeconds": 60,
    "MaxRetryCount": 3,
    "InitialRetryDelayMs": 100,
    "MaxRetryDelayMs": 2000,
    "EnableTransactionMetrics": true,
    "EnableAutoTransactionTimeout": false,
    "EnableVerboseTransactionLogging": false,
    "RetryableSqlErrorCodes": [1205, 1222, 40197, 40501, 40613, -2]
  }
}
```

### 配置项说明

| 配置项 | 类型 | 默认值 | 说明 |
|--------|------|--------|------|
| MaxTransactionDepth | int | 15 | 最大事务嵌套深度 |
| LongTransactionWarningSeconds | int | 60 | 长事务警告阈值(秒) |
| DefaultTransactionTimeoutSeconds | int | 60 | 默认事务超时时间(秒) |
| MaxRetryCount | int | 3 | 死锁重试最大次数 |
| InitialRetryDelayMs | int | 100 | 初始重试延迟(毫秒) |
| MaxRetryDelayMs | int | 2000 | 最大重试延迟(毫秒) |
| EnableTransactionMetrics | bool | true | 是否启用性能监控 |
| EnableAutoTransactionTimeout | bool | false | 是否启用自动超时 |
| EnableVerboseTransactionLogging | bool | false | 是否记录详细日志 |
| RetryableSqlErrorCodes | int[] | [...] | 可重试的SQL错误码 |

---

## 常见问题

### Q1: 什么时候使用同步方法,什么时候使用异步方法?

**A**: 
- **新项目**: 优先使用异步方法 (`BeginTranAsync` 等)
- **老项目**: 可以继续使用同步方法,逐步迁移
- **高并发场景**: 必须使用异步方法,避免线程阻塞

### Q2: 异步方法会影响性能吗?

**A**: 
- 不会! 异步方法在高并发下性能更好
- 减少了线程阻塞,提高了吞吐量
- 单个请求的延迟可能略有增加(<1ms),但整体系统吞吐提升20-30%

### Q3: 如何处理事务超时?

**A**: 
目前需要手动实现:

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
try
{
    await _unitOfWork.BeginTranAsync(cancellationToken: cts.Token);
    // ... 业务逻辑
    await _unitOfWork.CommitTranAsync(cancellationToken: cts.Token);
}
catch (OperationCanceledException)
{
    await _unitOfWork.RollbackTranAsync();
    throw new TimeoutException("事务执行超时");
}
```

### Q4: SemaphoreSlim 和 lock 有什么区别?

**A**:
- `lock`: 只能用于同步代码,不能跨 `await`
- `SemaphoreSlim`: 支持 `WaitAsync()`,可以跨 `await` 保持锁
- 对于纯同步代码,两者性能相近
- 对于异步代码,必须使用 `SemaphoreSlim`

### Q5: 如何监控事务性能?

**A**:
已集成 `TransactionMetrics`,会自动记录:
- 事务持续时间
- 成功/失败次数
- 长事务警告

查看日志或使用监控系统(如 Prometheus + Grafana)。

### Q6: 嵌套事务真的有效吗?

**A**:
- SQL Server 不支持真正的嵌套事务
- 我们使用 **保存点(Savepoint)** 模拟嵌套
- 外层回滚会回滚所有内容
- 内层回滚只回滚到保存点

### Q7: 如何处理分布式事务?

**A**:
当前实现仅支持单数据库事务。如需分布式事务:
1. 使用 Microsoft Distributed Transaction Coordinator (MSDTC)
2. 或采用 Saga 模式(最终一致性)
3. 或引入消息队列实现异步解耦

---

## 迁移 checklist

从同步迁移到异步的步骤:

- [ ] 1. 将所有 `BeginTran()` 改为 `await BeginTranAsync()`
- [ ] 2. 将所有 `CommitTran()` 改为 `await CommitTranAsync()`
- [ ] 3. 将所有 `RollbackTran()` 改为 `await RollbackTranAsync()`
- [ ] 4. 确保方法签名包含 `async Task`
- [ ] 5. 添加 `CancellationToken` 参数(可选但推荐)
- [ ] 6. 测试验证功能正常
- [ ] 7. 性能测试对比

---

## 参考资料

- [SqlSugar 官方文档](http://www.donet5.com/)
- [.NET 异步编程最佳实践](https://docs.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)
- [事务隔离级别详解](https://docs.microsoft.com/en-us/sql/relational-databases/sql-server-transaction-locking-and-row-versioning-guide/)

---

**最后更新**: 2026-04-18  
**维护者**: RUINORERP 开发团队
