# SqlSugar 事务机制与底层操作详解

**文档版本**: v1.1
**更新日期**: 2026-04-04
**参考官方文档**: https://www.donet5.com/home/doc
**适用范围**: RUINORERP 项目
**核心规范**: 本文档定义了 RUINORERP 项目的数据库操作核心规范，所有涉及事务、连接管理、异步操作的代码必须遵循本文档规范。

---

## ⚠️ 重要声明

> **本文档为 RUINORERP 项目的数据库操作核心规范。所有开发人员在进行数据库操作时，必须严格遵循本文档中的最佳实践和示例代码。文档中带有 ⭐ 标记的内容为特别重要的注意事项。**  

---

## 📋 目录

- [SqlSugar 客户端选择与创建](#sqlSugar-客户端选择与创建)
  - [SqlSugarScope vs SqlSugarClient](#sqlsugarscope-vs-sqlsugarclient)
  - [ConnectionConfig 核心配置](#connectionconfig-核心配置)
- [SqlSugar 事务机制核心原理](#sqlsugar-事务机制核心原理)
  - [事务的本质](#1-sqlsugar-事务的本质)
  - [事务的生命周期](#2-事务的生命周期)
  - [⭐ 上下文一致性要求](#⭐-上下文一致性要求)
- [事务的开启与提交](#事务的开启与提交)
  - [方法一：UseTran 语法糖 (⭐推荐)](#方法一usetran-语法糖-⭐推荐)
  - [方法二：BeginTran/CommitTran](#方法二begintrancommittran)
  - [方法三：UseTran 参数](#方法三usetran-参数)
- [事务的隔离级别](#事务的隔离级别)
- [异步操作与事务上下文](#异步操作与事务上下文)
  - [⭐ 异步事务核心问题](#⭐-异步事务核心问题)
  - [异步语法糖 UseTranAsync](#异步语法糖-usetranasync)
- [多库事务 (跨库事务)](#多库事务-跨库事务)
- [嵌套事务与工作单元模式](#嵌套事务与工作单元模式)
- [常见事务使用误区](#常见事务使用误区)
- [锁机制 (悲观锁与乐观锁)](#锁机制-悲观锁与乐观锁)
  - [With 锁语法](#with-锁语法)
  - [乐观锁配置](#乐观锁配置)
- [AOP 日志与错误处理](#aop-日志与错误处理)
- [⭐ 线程安全与连接管理](#⭐-线程安全与连接管理)
- [常见事务使用误区](#常见事务使用误区)
- [项目代码问题分析](#项目代码问题分析)
- [最佳实践建议](#最佳实践建议)
- [事务检查清单](#事务检查清单)
- [⭐ 官方完整实例代码](#⭐-官方完整实例代码)
  - [1. 联表查询 (Join) 实例](#1-联表查询-join-实例)
  - [2. 导航查询 (Include) 实例](#2-导航查询-include-实例)
  - [3. 导航操作 (InsertNav/UpdateNav/DeleteNav) 实例](#3-导航操作-insertnavupdatenavdeletenav-实例)
  - [4. 批量操作与事务结合实例](#4-批量操作与事务结合实例)
  - [5. Saveable (插入或更新) 实例](#5-saveable-插入或更新-实例)
  - [6. 分组查询 (GroupBy) 实例](#6-分组查询-groupby-实例)
  - [7. 子查询实例](#7-子查询实例)
  - [8. 分页查询实例](#8-分页查询实例)

---

## SqlSugar 客户端选择与创建

### SqlSugarScope vs SqlSugarClient

根据官方文档，SqlSugar 提供两种主要的客户端类型：

| 特性 | SqlSugarClient | SqlSugarScope |
|------|----------------|---------------|
| **生命周期** | 瞬时 (每次创建新实例) | 单例 (长期持有) |
| **线程安全** | 需要配合IOC使用 | 线程安全，可直接单例 |
| **事务传播** | 依赖IOC Scope | 天然支持跨方法事务 |
| **适用场景** | 控制台、桌面应用临时操作 | **Web应用、API服务 (⭐推荐)** |
| **连接管理** | 需手动管理 | 内置连接释放机制 |

#### ⭐ RUINORERP 项目决策

> **根据官方文档和项目实际情况，RUINORERP 选择使用 SqlSugarScope 作为数据库客户端：**
>
> 1. **SqlSugarScope 是线程安全的单例**，适合 WinForms 应用的长期运行特性
> 2. **通过 ScopedContext 管理每个请求的数据库连接**，支持事务自动传播
> 3. **反射方案获取 SqlSugarScope 实例的性能损耗可忽略不计**（仅 1-2 微秒）

---

### ConnectionConfig 核心配置

```csharp
public class ConnectionConfig
{
    /// <summary>
    /// 连接字符串 (⭐必须)
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// 数据库类型 (⭐必须)
    /// </summary>
    public DbType DbType { get; set; }

    /// <summary>
    /// 是否自动关闭连接 (⭐强烈推荐: true)
    /// </summary>
    public bool IsAutoCloseConnection { get; set; } = true;

    /// <summary>
    /// 主键类型设置
    /// </summary>
    public KeyType PrimaryKeyType { get; set; }

    /// <summary>
    /// 超时时间设置 (秒) - ⭐建议设置
    /// </summary>
    public int CommandTimeout { get; set; } = 30;

    /// <summary>
    /// AOP 事件配置
    /// </summary>
    public AopEvents AopEvents { get; set; }

    /// <summary>
    /// 多租户配置ID
    /// </summary>
    public string ConfigId { get; set; }
}
```

---

## SqlSugar 事务机制核心原理

### 1. SqlSugar 事务的本质

SqlSugar 的事务是基于 ADO.NET 的 `DbTransaction` 实现的，它通过以下方式工作:

```csharp
// SqlSugar 内部实现原理
public class SimpleClient : SugarClient
{
    // 当前事务对象
    public DbTransaction CurrentTran { get; set; }
    
    // 当前数据库连接
    public IDbConnection CurrentConnection { get; set; }
}
```

**关键点**:
- ✅ 事务依附于数据库连接 (`IDbConnection`)
- ✅ 同一个连接可以共享同一个事务
- ✅ 不同连接之间的事务是隔离的
- ✅ 异步操作时，事务上下文不会自动传递

---

### 2. 事务的生命周期

```
开始事务 → 执行 SQL → 提交/回滚事务 → 释放连接
   ↓          ↓           ↓            ↓
BeginTran  Insert/Update  CommitTran  Dispose
           Delete/Select   RollbackTran
```

**重要特性**:
1. **原子性**: 事务中的所有操作要么全部成功，要么全部失败
2. **一致性**: 事务前后数据库状态保持一致
3. **隔离性**: 并发事务之间互不干扰 (取决于隔离级别)
4. **持久性**: 事务提交后对数据库的修改是永久的

---

## 事务的开启与提交

### ⭐ 方法一：UseTran 语法糖 (⭐官方推荐 - 5.0.4+)

根据官方文档，这是 **SqlSugar 5.0.4+ 版本推荐的语法糖**，自动处理事务的开启、提交和回滚：

#### 同步版本

```csharp
using var tran = db.UseTran();  // ⭐ using 必须存在
// 业务操作...
tran.CommitTran();  // ⭐ 不提交则自动回滚
```

#### 自动异常处理版本

```csharp
// UseTran 自动处理异常回滚
var result = db.UseTran(() =>
{
    // 业务操作
    db.Insertable(entity1).ExecuteCommand();
    db.Updateable(entity2).ExecuteCommand();
    return true; // 返回业务结果
});

// 通过 result.IsSuccess 判断事务是否成功
if (!result.IsSuccess)
{
    Console.WriteLine($"事务失败: {result.Error}");
}
```

**优点**:
- ✅ **自动回滚**：离开 using 块或发生异常时自动回滚
- ✅ 代码简洁：无需手动 try-catch
- ✅ 避免忘记提交导致连接泄漏

---

### ⭐ 方法二：UseTranAsync 异步语法糖 (⭐官方推荐 - 5.0.3.8+)

```csharp
// ⭐ 异步语法糖 (5.0.3.8+)
var result = await db.UseTranAsync(async () =>
{
    await db.Insertable(entity1).ExecuteCommandAsync();
    await db.Updateable(entity2).ExecuteCommandAsync();
    return true;
});

// 通过 result.IsSuccess 和 result.Data 处理结果
if (!result.IsSuccess)
{
    throw result.Error; // 抛出异常或处理错误
}
```

**⭐ 重要注意事项**:
1. 异步方法必须在 `async Task` 方法内使用
2. `await db.UseTranAsync` 前的 `await` **不可遗漏**
3. 返回值必须是 `Task` 类型，不能是 `void`

---

### 方法三：BeginTran/CommitTran 传统写法

```csharp
try
{
    db.Ado.BeginTran();  // 开启事务

    // 业务操作
    db.Insertable(entity1).ExecuteCommand();
    db.Updateable(entity2).ExecuteCommand();

    db.Ado.CommitTran();  // 提交事务
}
catch (Exception ex)
{
    db.Ado.RollbackTran();  // 回滚事务
    throw;
}
```

#### 设置隔离级别

```csharp
db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted);
```

---

### ⭐ 方法四：隐式事务 (❌禁止使用)

```csharp
// ❌ 错误示例：没有显式开启事务
await db.Insertable(entity1).ExecuteCommandAsync();  // 自动提交
await db.Updateable(entity2).ExecuteCommandAsync();  // 自动提交
// 这两个操作不在同一个事务中!
```

**问题**:
- ❌ 每个操作都是独立的事务
- ❌ 无法保证数据一致性
- ❌ 容易出现部分成功部分失败

---

## ⭐ 上下文一致性要求

> **根据官方文档，这是一个特别重要的注意事项：**
>
> 事务内的所有操作必须使用**同一个 `db` 实例**，否则事务不生效！

```csharp
// ❌ 错误：使用不同的 db 实例
var db1 = GetDb();  // 实例1
var db2 = GetDb();  // 实例2 - 这是错误的!
db1.BeginTran();
db2.Insertable(entity).ExecuteCommand();  // ❌ 不在事务中
db1.CommitTran();

// ✅ 正确：使用同一个 db 实例
var db = GetDb();
db.BeginTran();
db.Insertable(entity).ExecuteCommand();   // ✅ 在事务中
db.CommitTran();
```

---

## 事务的隔离级别

### SqlSugar 支持的隔离级别

```csharp
// 开启事务时指定隔离级别
_unitOfWorkManage.BeginTran(System.Data.IsolationLevel.ReadCommitted);
```

### SQL Server 默认隔离级别对比

| 隔离级别 | 脏读 | 不可重复读 | 幻读 | 锁竞争 | 性能 |
|---------|------|-----------|------|--------|------|
| **Read Uncommitted** | ✅ 可能 | ✅ 可能 | ✅ 可能 | 低 | 最高 |
| **Read Committed** (默认) | ❌ 防止 | ✅ 可能 | ✅ 可能 | 中 | 高 |
| **Repeatable Read** | ❌ 防止 | ❌ 防止 | ✅ 可能 | 高 | 中 |
| **Serializable** | ❌ 防止 | ❌ 防止 | ❌ 防止 | 最高 | 最低 |
| **Snapshot** | ❌ 防止 | ❌ 防止 | ❌ 防止 | 低 | 高 |

### RUINORERP 项目的选择

根据代码分析，项目使用的是 **Read Committed**:

```csharp
// BaseControllerGeneric.cs Line 687
public virtual ISugarQueryable<T> BaseGetQueryable()
{
    return _unitOfWorkManage.GetDbClient().Queryable<T>().With(SqlWith.NoLock);
    // NoLock 相当于 Read Uncommitted，允许脏读
}
```

**问题分析**:
- ⚠️ 大量使用 `With(SqlWith.NoLock)` 可能导致脏读
- ⚠️ 关键业务逻辑应该使用更强的隔离级别

---

## 异步操作与事务上下文

### ⭐ 异步事务核心问题

根据官方文档，异步操作时事务上下文传递是一个核心问题：

#### 错误示例

```csharp
// ❌ 错误：异步操作导致事务上下文丢失
public async Task<bool> ProcessOrderAsync(Order order)
{
    _unitOfWorkManage.BeginTran();  // 线程 A 开启事务

    // 异步操作切换到线程 B
    await db.Insertable(order).ExecuteCommandAsync();  // 线程 B 执行，不在事务中!

    // 回到线程 A
    _unitOfWorkManage.CommitTran();  // 线程 A 提交空事务
    return true;
}
```

**执行流程**:
```
线程 A: BeginTran() → 等待 await → CommitTran() (提交了个寂寞)
            ↓
线程 B:              ExecuteCommandAsync() (没有事务保护!)
```

**后果**:
- ❌ INSERT 操作立即自动提交
- ❌ 如果后续操作失败，无法回滚
- ❌ 数据不一致

---

#### ⭐ 正确示例：UseTranAsync (官方推荐)

```csharp
// ✅ 官方推荐的异步事务模式
public async Task<bool> ProcessOrderAsync(Order order)
{
    var result = await db.UseTranAsync(async () =>
    {
        await db.Insertable(order).ExecuteCommandAsync();
        await db.Updateable(inventory).ExecuteCommandAsync();
        return true;
    });

    if (!result.IsSuccess)
    {
        throw result.Error;
    }
    return true;
}
```

**⭐ 关键点**:
- ✅ `UseTranAsync` 自动处理事务上下文传递
- ✅ 无需手动管理 BeginTran/CommitTran
- ✅ 异常自动回滚

---

## 多库事务 (跨库事务)

> **⭐ 根据官方文档，多库事务是 SqlSugar 的独家功能，稳定性优于 CAP（无队列层）。但跨程序事务推荐使用 CAP（TCC/Saga 模式）。**

### 单库事务 vs 多库事务

| 类型 | 方法 | 说明 |
|------|------|------|
| 单库事务 | `db.Ado.BeginTran()` | 仅限单个数据库 |
| **多库事务** | `db.BeginTran()` ⭐ | **跨多个数据库** |

### 多库事务示例

```csharp
try
{
    db.BeginTran();  // ⭐ 注意：不是 db.Ado.BeginTran()

    // 获取不同数据库连接并操作
    db.GetConnection("1").Insertable(new Order() { ... }).ExecuteCommand();
    db.GetConnection("0").Insertable(new Product() { ... }).ExecuteCommand();

    db.CommitTran();
}
catch (Exception ex)
{
    db.RollbackTran();  // 多库事务回滚有重试机制
    throw;
}
```

### ⭐ 多租户事务注意事项

> **根据官方文档，多租户事务只能使用 `db.BeginTran()`，严禁使用 `db.Ado.BeginTran()`！**

---

## 嵌套事务与工作单元模式

### 工作单元模式 (5.1.2.5-preview03+)

根据官方文档，推荐使用工作单元模式处理嵌套事务：

```csharp
// 共享事务：内部操作使用外部事务
using (var uow = db.CreateContext(db.Ado.IsNoTran()))
{
    // 业务操作...

    using (var uow2 = db.CreateContext(db.Ado.IsNoTran()))
    {
        // 内层业务 - 使用同一个事务
        uow2.Commit();
    }
    uow.Commit(); // 外部提交
}
// 工作单元内抛出异常会自动回滚
```

### 声明式事务

可通过特性注解实现声明式事务：

```csharp
[ServiceFilter(typeof(TransactionFilter))]
public async Task<Result> ProcessAsync()
{
    // 业务逻辑
}
```

### 独立子事务 (❌不推荐 - 易导致死锁)

```csharp
// 外层事务
db.BeginTran();
try
{
    // 业务 A...

    // 内层独立事务（❌不推荐）
    var childDb = db.CopyNew();
    try
    {
        childDb.BeginTran();
        // 业务 B...
        childDb.CommitTran();
    }
    catch
    {
        childDb.RollbackTran();
        throw;
    }

    db.CommitTran();
}
catch
{
    db.RollbackTran();
    throw;
}
```

---

## 锁机制 (悲观锁与乐观锁)

### With 锁语法

```csharp
// 无锁读取（脏读）- 用于非关键查询
var list = db.Queryable<Order>().With(SqlWith.NoLock).ToList();

// 更新锁（避免脏读）- ⭐用于需要更新的查询
var entity = db.Queryable<Order>()
    .With(SqlWith.UpdLock)  // ⭐ 推荐用于审核、更新等操作
    .Where(o => o.Id == id)
    .First();

// 行级锁
var item = db.Queryable<Order>()
    .With(SqlWith.RowLock)
    .Where(o => o.Id == id)
    .First();

// 表锁
db.Queryable<Order>()
    .With(SqlWith.TabLock)
    .Where(...)
    .ToList();
```

### SqlWith 枚举值

| 值 | 说明 | 适用场景 |
|----|------|---------|
| `SqlWith.NoLock` | 无锁 (脏读) | 非关键查询、报表 |
| `SqlWith.RowLock` | 行级锁 | 单行更新 |
| `SqlWith.UpdLock` | 更新锁 ⭐ | **审核、更新等关键操作** |
| `SqlWith.TabLock` | 表锁 | 批量操作 |

### 乐观锁配置

通过实体类的 `[SugarColumn]` 特性标记版本字段：

```csharp
public class Order
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn(IsEnableUpdateVersionValidation = true)]  // ⭐ 启用乐观锁
    public long Version { get; set; }

    public string CustomerName { get; set; }
}

// 更新时自动校验版本，版本不一致会抛出异常
db.Updateable<Order>(order).ExecuteCommand();
```

---

## AOP 日志与错误处理

### OnLogExecuting 事件

在 SQL 执行前触发，用于记录或修改即将执行的 SQL：

```csharp
var db = new SqlSugarScope(new ConnectionConfig()
{
    ConnectionString = "YourConnectionString",
    DbType = DbType.SqlServer,
    IsAutoCloseConnection = true,
    AopEvents = new AopEvents
    {
        OnLogExecuting = (sql, pars) =>
        {
            Console.WriteLine($"SQL: {sql}");
            if (pars != null)
            {
                foreach (var param in pars)
                {
                    Console.WriteLine($"参数: {param.ParameterName} = {param.Value}");
                }
            }
        }
    }
});
```

### OnError 事件

在 SQL 执行发生异常时触发：

```csharp
AopEvents = new AopEvents
{
    OnError = (exp) =>
    {
        Console.WriteLine($"SQL执行错误: {exp.Message}");
        Logger.Error(exp, "数据库操作错误");
    }
}
```

### 性能监控

```csharp
Stopwatch sw = Stopwatch.StartNew();

OnLogExecuted = (sql, pars) =>
{
    sw.Stop();
    Console.WriteLine($"SQL执行耗时: {sw.ElapsedMilliseconds}ms");
}
```

---

## ⭐ 线程安全与连接管理

> **根据官方文档，这是一个特别重要的注意事项：**

### 线程安全要求

1. **SqlSugarScope 是线程安全的**，可在多线程环境下共享使用
2. **SqlSugarClient 每次请求应创建新实例**（配合IOC）

### ⭐ IsAutoCloseConnection 配置

```csharp
// ⭐ 推荐配置
var db = new SqlSugarScope(new ConnectionConfig()
{
    ConnectionString = "YourConnectionString",
    DbType = DbType.SqlServer,
    IsAutoCloseConnection = true,  // ⭐ 强烈推荐开启
    CommandTimeout = 30            // ⭐ 建议设置超时
});
```

| 值 | 说明 | 适用场景 |
|----|------|---------|
| `true` ⭐ | 执行后自动关闭连接 | **大多数场景** |
| `false` | 手动管理连接 | 长事务、批量操作 |

### ⭐ 异步安全注意事项

> **根据官方文档，异步操作时必须注意以下几点：**

1. **异步方法不能遗漏 `await`**
2. **返回值必须是 `Task` 类型，不能是 `void`**
3. **在内部线程并发或异步操作时，必须设置 `IsAutoCloseConnection=true`**

---

## 常见事务使用误区

### 误区 1: 认为 async/await 会自动保持事务

```csharp
// ❌ 错误认知
public async Task ProcessAsync()
{
    _unitOfWorkManage.BeginTran();  // 以为开启了事务

    // 实际上这里已经切换了线程，事务上下文丢失
    await DoSomethingAsync();  // 不在事务中!

    _unitOfWorkManage.CommitTran();  // 提交空事务
}
```

**真相**:
- async/await 会切换线程
- `DbTransaction` 不会自动在新线程中使用
- 必须使用 `UseTranAsync` 或 `TransactionScope` 显式传递

---

## 常见事务使用误区

### 误区 1: 认为 async/await 会自动保持事务

```csharp
// ❌ 错误认知
public async Task ProcessAsync()
{
    _unitOfWorkManage.BeginTran();  // 以为开启了事务
    
    // 实际上这里已经切换了线程，事务上下文丢失
    await DoSomethingAsync();  // 不在事务中!
    
    _unitOfWorkManage.CommitTran();  // 提交空事务
}
```

**真相**:
- async/await 会切换线程
- `DbTransaction` 不会自动在新线程中使用
- 必须使用 `TransactionScope` 显式传递

---

### 误区 2: 嵌套事务会自动合并

```csharp
// ❌ 错误示例
public async Task OuterMethodAsync()
{
    _unitOfWorkManage.BeginTran();  // 外层事务
    
    await InnerMethodAsync();  // 内层也开启事务?
    
    _unitOfWorkManage.CommitTran();
}

public async Task InnerMethodAsync()
{
    _unitOfWorkManage.BeginTran();  // ❌ 这会怎样？
    await db.Insertable(entity).ExecuteCommandAsync();
    _unitOfWorkManage.CommitTran();
}
```

**实际情况**:
- SqlSugar 不支持真正的嵌套事务
- 第二次 `BeginTran()` 会被忽略或抛出异常
- 应该由外层统一管理事务

**正确做法**:
```csharp
// ✅ 正确：由外层统一管理
public async Task OuterMethodAsync()
{
    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        _unitOfWorkManage.BeginTran();
        
        // 内层不再开启事务，直接使用外层的
        await InnerMethodAsync();
        
        _unitOfWorkManage.CommitTran();
        scope.Complete();
    }
}

public async Task InnerMethodAsync()
{
    // 不再开启事务，直接使用当前事务上下文
    await db.Insertable(entity).ExecuteCommandAsync();
}
```

---

### 误区 3: 事务中可以有长时间的业务计算

```csharp
// ❌ 错误示例
public async Task ApprovalAsync()
{
    _unitOfWorkManage.BeginTran();
    
    // 耗时 2 秒的业务计算
    var result = ComplexCalculation();  // 🔴 持有锁 2 秒!
    
    // 数据库更新
    await db.Updateable(entity).ExecuteCommandAsync();
    
    _unitOfWorkManage.CommitTran();
}
```

**问题**:
- 🔴 长时间持有数据库锁
- 🔴 阻塞其他并发操作
- 🔴 增加死锁风险

**正确做法**:
```csharp
// ✅ 正确：先计算，后事务
public async Task ApprovalAsync()
{
    // 在事务外完成所有计算
    var result = ComplexCalculation();  // 无锁状态
    
    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        _unitOfWorkManage.BeginTran();
        
        // 快速完成数据库更新
        await db.Updateable(entity).ExecuteCommandAsync();
        
        _unitOfWorkManage.CommitTran();
        scope.Complete();
    }
}
```

---

### 误区 4: 异常后事务会自动回滚

```csharp
// ❌ 错误示例
public async Task ProcessAsync()
{
    _unitOfWorkManage.BeginTran();
    
    try
    {
        await Operation1Async();
        await Operation2Async();
        // 如果这里发生异常...
    }
    catch (Exception ex)
    {
        _logger.Error(ex);  // 只记录日志
        // ❌ 忘记回滚事务!
    }
    // 事务一直处于未提交状态，连接被占用
}
```

**后果**:
- 🔴 连接池泄漏
- 🔴 数据库资源被长期占用
- 🔴 可能导致死锁

**正确做法**:
```csharp
// ✅ 正确：确保回滚
public async Task ProcessAsync()
{
    _unitOfWorkManage.BeginTran();
    try
    {
        await Operation1Async();
        await Operation2Async();
        _unitOfWorkManage.CommitTran();
    }
    catch
    {
        _unitOfWorkManage.RollbackTran();  // ✅ 必须回滚
        throw;
    }
}
```

---

### 误区 5: Updateable 在事务外也能回滚

```csharp
// ❌ 错误示例
public async Task ProcessAsync()
{
    _unitOfWorkManage.BeginTran();
    
    // 这个 UPDATE 会立即执行并提交!
    await db.Updateable<tb_SaleOut>(entity)
        .UpdateColumns(e => e.Status)
        .ExecuteCommandAsync();
    
    // 如果后面发生异常
    throw new Exception("Oops");
    
    // 前面的 UPDATE 已经提交，无法回滚!
}
```

**真相**:
- `ExecuteCommandAsync()` 会立即执行 SQL
- 如果在事务内执行，会等待事务提交
- 但如果在事务外执行，会立即自动提交

**验证方法**:
```csharp
// 测试是否在事务中
Console.WriteLine(db.Ado.CurrentTran);  // null 表示不在事务中
```

---

## 项目代码问题分析

### 问题案例 1: 平台退款按钮事件处理

#### 原始代码

```csharp
// UCSaleOut.cs Line 101-177
private async void toolStripButton 平台退款_Click(object sender, EventArgs e)
{
    if (EditEntity == null) return;
    
    tb_SaleOut saleOut = EditEntity as tb_SaleOut;
    
    // 检查状态
    if (EditEntity.ApprovalStatus == (int)ApprovalStatus.审核通过)
    {
        // ❌ 问题 1: RefundProcessAsync 内部有事务
        var rrs = await ctr.RefundProcessAsync(EditEntity);
        
        if (rrs.Succeeded)
        {
            // ❌ 问题 2: 这里的 Updateable 在事务外执行!
            await _db.Updateable<tb_SaleOut>(saleout)
                .UpdateColumns(it => new { it.RefundStatus })
                .ExecuteCommandAsync();
        }
    }
}
```

#### RefundProcessAsync 内部实现

```csharp
// tb_SaleOutControllerPartial.cs Line 52-236
public async Task<ReturnResults<tb_SaleOutRe>> RefundProcessAsync(tb_SaleOut entity)
{
    try
    {
        // ✅ 开启事务
        _unitOfWorkManage.BeginTran();
        
        // ... 创建销售退回单逻辑 ...
        
        var reControl = _appContext.GetRequiredService<tb_SaleOutReController<tb_SaleOutRe>>();
        var SaveRs = await reControl.BaseSaveOrUpdateWithChild<tb_SaleOutRe>(entity);
        
        if (SaveRs.Succeeded)
        {
            // ✅ 在事务内更新销售出库单
            saleout.RefundStatus = (int)RefundStatus.已退款等待退货;
            var last = await _unitOfWorkManage.GetDbClient()
                .Updateable<tb_SaleOut>(saleout)
                .UpdateColumns(it => new { it.RefundStatus })
                .ExecuteCommandAsync();
        }
        
        // ✅ 提交事务
        _unitOfWorkManage.CommitTran();
        
        return new ReturnResults<tb_SaleOutRe> { Succeeded = true };
    }
    catch (Exception ex)
    {
        // ✅ 回滚事务
        _unitOfWorkManage.RollbackTran();
        throw;
    }
}
```

#### 问题分析

**问题 1: 双重事务冲突**

```
执行流程:
T0: RefundProcessAsync 开始
T1: _unitOfWorkManage.BeginTran()  ← 内层事务开启
T2: BaseSaveOrUpdateWithChild (可能又开启事务?)
T3: Update tb_SaleOut SET RefundStatus = ...
T4: _unitOfWorkManage.CommitTran()  ← 内层事务提交
T5: RefundProcessAsync 返回
T6: UI 层执行 Updateable<tb_SaleOut>  ← ❌ 这是多余的!
```

**实际影响**:
- ⚠️ `RefundProcessAsync` 已经在事务内更新了 `RefundStatus`
- ⚠️ UI 层的再次更新是多余的
- ⚠️ 如果 UI 层更新失败，会造成数据不一致

**问题 2: 异步事务上下文**

```csharp
// ❌ 没有使用 TransactionScope
private async void toolStripButton 平台退款_Click(object sender, EventArgs e)
{
    // UI 线程的同步上下文
    var rrs = await ctr.RefundProcessAsync(EditEntity);
    // ↑ await 后可能切换线程
    
    // ❌ 这里可能在不同的线程执行
    await _db.Updateable<tb_SaleOut>(saleout)
        .UpdateColumns(it => new { it.RefundStatus })
        .ExecuteCommandAsync();
}
```

**潜在风险**:
- 如果 `RefundProcessAsync` 内部使用了 `TransactionScope`
- UI 层的更新可能不在同一个事务上下文中
- 导致数据不一致

---

#### 正确的实现方式

```csharp
// ✅ 方案 1: 完全在 Controller 层处理
// tb_SaleOutControllerPartial.cs
public async Task<ReturnResults<tb_SaleOutRe>> RefundProcessAsync(tb_SaleOut entity)
{
    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        try
        {
            _unitOfWorkManage.BeginTran();
            
            // 创建销售退回单
            var reControl = _appContext.GetRequiredService<tb_SaleOutReController<tb_SaleOutRe>>();
            var SaveRs = await reControl.BaseSaveOrUpdateWithChild<tb_SaleOutRe>(entity);
            
            if (!SaveRs.Succeeded)
            {
                return new ReturnResults<tb_SaleOutRe> 
                { 
                    Succeeded = false, 
                    ErrorMsg = SaveRs.ErrorMsg 
                };
            }
            
            // ✅ 在事务内更新销售出库单状态
            entity.RefundStatus = (int)RefundStatus.已退款等待退货;
            await _unitOfWorkManage.GetDbClient()
                .Updateable<tb_SaleOut>(entity)
                .UpdateColumns(it => new { it.RefundStatus })
                .ExecuteCommandAsync();
            
            _unitOfWorkManage.CommitTran();
            scope.Complete();
            
            return new ReturnResults<tb_SaleOutRe> 
            { 
                Succeeded = true, 
                ReturnObject = SaveRs.ReturnObject 
            };
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            _logger.Error(ex, "平台退款处理失败");
            return new ReturnResults<tb_SaleOutRe> 
            { 
                Succeeded = false, 
                ErrorMsg = ex.Message 
            };
        }
    }
}

// ✅ UI 层只需要调用一次
private async void toolStripButton 平台退款_Click(object sender, EventArgs e)
{
    if (EditEntity == null) return;
    
    try
    {
        var rrs = await ctr.RefundProcessAsync(EditEntity);
        
        if (rrs.Succeeded)
        {
            MessageBox.Show("成功预生成【销售退回单】");
            // ✅ 刷新 UI 显示最新数据
            Refreshs();
        }
        else
        {
            MessageBox.Show($"平台退款失败：{rrs.ErrorMsg}");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"系统异常：{ex.Message}");
    }
}
```

**关键改进**:
1. ✅ 使用 `TransactionScope` 包装整个操作
2. ✅ 所有数据库操作在同一个事务中
3. ✅ UI 层不再执行额外的数据库操作
4. ✅ 完整的异常处理和回滚

---

### 问题案例 2: BaseSaveOrUpdateWithChild 的事务管理

#### 当前实现

```csharp
// tb_SaleOutController.cs Line 27-49
public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model)
{
    try
    {
        // ✅ 有事务保护
        bool rs = await _unitOfWorkManage.GetDbClient()
            .InsertNav(model)
            .Include(m => m.tb_SaleOutDetails)
            .ExecuteCommandAsync();
            
        _unitOfWorkManage.CommitTran();
        
        return new ReturnMainSubResults<T> { Succeeded = rs, ReturnObject = model };
    }
    catch (Exception ex)
    {
        // ✅ 出错回滚
        _unitOfWorkManage.RollbackTran();
        return new ReturnMainSubResults<T> { Succeeded = false, ErrorMsg = ex.Message };
    }
}
```

#### 问题分析

**问题 1: 没有使用 TransactionScope**

```csharp
// ❌ 异步操作可能导致事务上下文丢失
bool rs = await _unitOfWorkManage.GetDbClient()
    .InsertNav(model)
    .Include(m => m.tb_SaleOutDetails)
    .ExecuteCommandAsync();
    // ↑ await 后可能切换线程
    
_unitOfWorkManage.CommitTran();  // 可能在错误的线程提交
```

**正确实现**:

```csharp
// ✅ 使用 TransactionScope
public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model)
{
    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        try
        {
            _unitOfWorkManage.BeginTran();
            
            bool rs = await _unitOfWorkManage.GetDbClient()
                .InsertNav(model)
                .Include(m => m.tb_SaleOutDetails)
                .ExecuteCommandAsync();
                
            _unitOfWorkManage.CommitTran();
            scope.Complete();
            
            return new ReturnMainSubResults<T> { Succeeded = rs, ReturnObject = model };
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            _logger.Error(ex, "保存失败");
            return new ReturnMainSubResults<T> { Succeeded = false, ErrorMsg = ex.Message };
        }
    }
}
```

---

**问题 2: 调用方可能已经开启事务**

```csharp
// BaseControllerGeneric.cs Save 方法
protected async Task<ReturnMainSubResults<T>> Save(T entity)
{
    // ❌ 如果调用方已经开启事务，这里会冲突
    long pkid = (long)ReflectionHelper.GetPropertyValue(entity, PKCol);
    
    if (pkid == 0)
    {
        BusinessHelper.Instance.InitEntity(entity);
    }
    else
    {
        // ❌ 检查锁定状态 (可能在事务外查询)
        var lockStatus = await CheckLockStatusAndUpdateUI(EditEntity.PrimaryKeyID);
        if (!lockStatus.CanPerformCriticalOperations)
        {
            return new ReturnMainSubResults<T> { Succeeded = false };
        }
    }
    
    // ❌ 又开启一个事务
    rmr = await ctr.BaseSaveOrUpdateWithChild<T>(entity);
    
    // ❌ 保存后管理锁定状态 (可能在事务外更新)
    await PostSaveLockManagement(entity, pkid);
    
    return rmr;
}
```

**执行流程分析**:

```
场景：UCSaleOut.Save() → BaseBillEditGeneric.Save() → BaseController.BaseSaveOrUpdateWithChild()

UCSaleOut.Save():
  └─ 没有开启事务
      └─ BaseBillEditGeneric.Save():
          ├─ CheckLockStatusAndUpdateUI()  ← 查询 (自动提交)
          └─ ctr.BaseSaveOrUpdateWithChild():
              ├─ BeginTran()  ← 新事务
              ├─ InsertNav()  ← 插入
              ├─ CommitTran() ← 提交
              └─ 返回
          └─ PostSaveLockManagement()  ← 更新 (自动提交)
```

**问题**:
- 🔴 三个操作不在同一个事务中
- 🔴 可能出现部分成功部分失败
- 🔴 锁定状态检查和保存之间有竞态条件

**正确实现**:

```csharp
// ✅ 统一事务管理
protected async Task<ReturnMainSubResults<T>> Save(T entity)
{
    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        try
        {
            _unitOfWorkManage.BeginTran();
            
            // 1. 检查锁定状态 (在事务内)
            var lockStatus = await CheckLockStatusAndUpdateUI(EditEntity.PrimaryKeyID);
            if (!lockStatus.CanPerformCriticalOperations)
            {
                return new ReturnMainSubResults<T> { Succeeded = false };
            }
            
            // 2. 保存实体 (使用当前事务)
            var rmr = await ctr.BaseSaveOrUpdateWithChild<T>(entity);
            if (!rmr.Succeeded)
            {
                return rmr;
            }
            
            // 3. 保存后管理 (在事务内)
            await PostSaveLockManagement(entity, rmr.ReturnObject.PrimaryKeyID);
            
            _unitOfWorkManage.CommitTran();
            scope.Complete();
            
            return rmr;
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            _logger.Error(ex, "保存失败");
            return new ReturnMainSubResults<T> { Succeeded = false, ErrorMsg = ex.Message };
        }
    }
}

// ✅ BaseController 修改
public async override Task<ReturnMainSubResults<T>> BaseSaveOrUpdateWithChild<C>(T model)
{
    // 不再开启事务，使用调用方的事务上下文
    bool rs = await _unitOfWorkManage.GetDbClient()
        .InsertNav(model)
        .Include(m => m.tb_SaleOutDetails)
        .ExecuteCommandAsync();
        
    return new ReturnMainSubResults<T> { Succeeded = rs, ReturnObject = model };
}
```

---

### 问题案例 3: 审核过程的事务管理

#### 当前实现

```csharp
// tb_SaleOutControllerPartial.cs ApprovalAsync Line 314-800+
public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
{
    tb_SaleOut entity = ObjectEntity as tb_SaleOut;
    
    try
    {
        // ❌ 先查询 (不在事务中)
        var existingEntity = await _db.Queryable<tb_SaleOut>()
            .Where(c => c.SaleOut_MainID == entity.SaleOut_MainID)
            .FirstAsync();
        
        // ❌ 再加载关联数据 (不在事务中)
        entity.tb_saleorder = await _db.Queryable<tb_SaleOrder>()
            .Includes(...).Where(...).SingleAsync();
        
        // ❌ 预处理后才开启事务
        #region 预加载库存
        var inventoryList = await _db.Queryable<tb_Inventory>()
            .Where(...).ToListAsync();
        #endregion
        
        // ❌ 太晚了！前面已经持有了共享锁
        _unitOfWorkManage.BeginTran();  // Line 492
        
        // ❌ 循环内多次数据库访问
        foreach (var child in entity.tb_SaleOutDetails)
        {
            // 虽然在事务外预加载了
            // 但在事务内仍然有更新操作
        }
        
        // ❌ 批量更新
        await _db.Updateable<tb_Inventory>(invUpdateList).ExecuteCommandAsync();
        await tranController.BatchRecordTransactionsWithRetry(transactionList);
        await _db.Updateable<tb_SaleOutDetail>(UpdateSaleOutCostlist).ExecuteCommandAsync();
        await _db.Updateable<tb_PriceRecord>(priceUpdateList).ExecuteCommandAsync();
        await _db.Updateable<tb_SaleOrder>(entity.tb_saleorder).ExecuteCommandAsync();
        
        _unitOfWorkManage.CommitTran();
        
        return new ReturnResults<T> { Succeeded = true };
    }
    catch (Exception ex)
    {
        _unitOfWorkManage.RollbackTran();
        throw;
    }
}
```

#### 问题分析

**问题 1: 长事务持有过多锁**

```
时间轴:
T0:  BEGIN TRAN  ← 开启事务
T1:  SELECT tb_SaleOut (S 锁)
T2:  SELECT tb_SaleOrder (S 锁)
T3:  SELECT tb_Inventory (S 锁)
T4:  UPDATE tb_Inventory (X 锁) ← 开始持有排他锁
T5:  INSERT tb_InventoryTransaction (X 锁)
T6:  UPDATE tb_SaleOutDetail (X 锁)
T7:  UPDATE tb_PriceRecord (X 锁)
T8:  UPDATE tb_SaleOrder (X 锁)
T9:  COMMIT TRAN  ← 释放所有锁

总耗时：约 2000ms
锁持有时间：T4-T9 ≈ 1500ms
```

**后果**:
- 🔴 长时间持有多个表的排他锁
- 🔴 阻塞其他并发操作
- 🔴 死锁风险极高

---

**问题 2: 没有使用 UpdateLock**

```csharp
// ❌ 普通查询使用 NOLOCK
var existingEntity = await _db.Queryable<tb_SaleOut>()
    .With(SqlWith.NoLock)  // 或者没有 With 子句
    .Where(c => c.SaleOut_MainID == entity.SaleOut_MainID)
    .FirstAsync();

// 然后基于这个数据进行业务判断
if (existingEntity.DataStatus == (int)DataStatus.确认)
{
    // ❌ 如果这是脏数据或被其他事务修改了，会出错
}
```

**正确实现**:

```csharp
// ✅ 使用 UpdateLock 防止并发修改
var entity = await _db.Queryable<tb_SaleOut>()
    .With(SqlWith.UpdLock)  // 更新锁
    .Includes(c => c.tb_SaleOutDetails)
    .Where(d => d.SaleOut_MainID == ObjectEntity.SaleOut_MainID)
    .FirstAsync();
```

---

**问题 3: 事务内包含太多非数据库操作**

```csharp
// ❌ 事务内执行业务计算
foreach (var child in entity.tb_SaleOutDetails)
{
    // 业务逻辑判断
    if (!_appContext.SysConfig.CheckNegativeInventory && 
        (group.Inventory.Quantity - group.OutQtySum) < 0)
    {
        _unitOfWorkManage.RollbackTran();
        return error;
    }
    
    // 内存计算
    group.OutQtySum += currentSaleQty;
}
```

**正确实现**:

```csharp
// ✅ 在事务外完成所有计算
var calculationResult = PreCalculateApproval(entity);

using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
{
    _unitOfWorkManage.BeginTran();
    
    // 快速执行数据库更新
    await ExecuteDatabaseUpdates(calculationResult);
    
    _unitOfWorkManage.CommitTran();
    scope.Complete();
}
```

---

## 最佳实践建议

### 1. 事务使用黄金法则

```
✅ 1. 事务边界要清晰 - 谁开启，谁提交/回滚
✅ 2. 事务范围要最小 - 只包含必要的数据库操作
✅ 3. 事务时间要最短 - 避免在事务内做业务计算
✅ 4. 异步要用 TransactionScope - TransactionScopeAsyncFlowOption.Enabled
✅ 5. 异常必须回滚 - finally 块或使用 using
```

---

### 2. 推荐的事务封装模式

```csharp
/// <summary>
/// 执行带事务的异步操作
/// </summary>
public async Task<T> ExecuteInTransactionAsync<T>(
    Func<Task<T>> operation, 
    string operationName = "未知操作")
{
    using (var scope = new TransactionScope(
        TransactionScopeAsyncFlowOption.Enabled,
        new TransactionOptions 
        { 
            IsolationLevel = System.Data.IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromSeconds(30)  // 30 秒超时
        }))
    {
        try
        {
            _unitOfWorkManage.BeginTran();
            
            T result = await operation();
            
            _unitOfWorkManage.CommitTran();
            scope.Complete();
            
            return result;
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            _logger.Error(ex, $"{operationName}失败");
            throw;
        }
    }
}

// 使用示例
public async Task<ReturnResults<T>> ApprovalAsync(T entity)
{
    return await ExecuteInTransactionAsync(async () =>
    {
        // 所有数据库操作
        await db.Updateable(entity).ExecuteCommandAsync();
        await db.Insertable(log).ExecuteCommandAsync();
        
        return new ReturnResults<T> { Succeeded = true };
    }, "审核操作");
}
```

---

### 3. 避免死锁的实践

```csharp
// ✅ 1. 固定锁获取顺序
public async Task ProcessOrderAsync()
{
    // 总是按这个顺序访问表
    var order = await GetOrderAsync();      // tb_SaleOrder
    var details = await GetDetailsAsync();  // tb_SaleOutDetail
    var inventory = await GetInventoryAsync(); // tb_Inventory
    
    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        _unitOfWorkManage.BeginTran();
        
        // 按相同顺序更新
        await UpdateInventoryAsync(inventory);
        await UpdateDetailsAsync(details);
        await UpdateOrderAsync(order);
        
        _unitOfWorkManage.CommitTran();
        scope.Complete();
    }
}

// ✅ 2. 使用短事务
public async Task QuickUpdateAsync()
{
    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
    {
        _unitOfWorkManage.BeginTran();
        
        // 只包含必要的 UPDATE
        await db.Updateable(entity)
            .UpdateColumns(e => e.Status)
            .ExecuteCommandAsync();
        
        _unitOfWorkManage.CommitTran();
        scope.Complete();
    }
}

// ✅ 3. 设置超时
_db.Ado.CommandTimeout = 10;  // 10 秒超时，快速失败
```

---

### 4. 异步事务的标准模板

```csharp
public async Task<ReturnResults<T>> ProcessAsync(T entity)
{
    // ✅ 1. 在方法最外层使用 TransactionScope
    using (var scope = new TransactionScope(
        TransactionScopeAsyncFlowOption.Enabled,
        new TransactionOptions 
        { 
            IsolationLevel = System.Data.IsolationLevel.ReadCommitted,
            Timeout = TimeSpan.FromSeconds(30)
        }))
    {
        try
        {
            // ✅ 2. 开启 SqlSugar 事务
            _unitOfWorkManage.BeginTran();
            
            // ✅ 3. 所有数据库操作都使用 await
            await db.Insertable(entity).ExecuteCommandAsync();
            await db.Updateable(child).ExecuteCommandAsync();
            
            // ✅ 4. 提交 SqlSugar 事务
            _unitOfWorkManage.CommitTran();
            
            // ✅ 5. 提交 TransactionScope
            scope.Complete();
            
            return new ReturnResults<T> { Succeeded = true };
        }
        catch (Exception ex)
        {
            // ✅ 6. 异常时回滚
            _unitOfWorkManage.RollbackTran();
            _logger.Error(ex);
            return new ReturnResults<T> { Succeeded = false, ErrorMsg = ex.Message };
        }
    }
}
```

---

## 事务检查清单

> **⭐ 根据官方文档，在提交代码前，必须检查以下项目：**

### 必须检查项

- [ ] **异步事务使用 `UseTranAsync`**：异步操作必须使用官方推荐的语法糖
- [ ] **上下文一致性**：所有操作使用同一个 `db` 实例
- [ ] **异常必须回滚**：使用 `UseTranAsync` 或 try-catch 包含 `RollbackTran()`
- [ ] **事务范围最小化**：只包含必要的数据库操作
- [ ] **业务计算在事务外**：复杂计算先完成，再开启事务
- [ ] **设置了合理超时**：`CommandTimeout` 建议设置为 30 秒
- [ ] **使用固定锁顺序**：按相同顺序访问多张表，避免死锁
- [ ] **关键查询用 `UpdLock`**：审核、更新等关键操作使用更新锁

### 禁止事项

- ❌ **禁止隐式事务**：不能在没有开启事务的情况下执行多个操作
- ❌ **禁止嵌套 BeginTran**：第二次 BeginTran 会被忽略或抛异常
- ❌ **禁止在事务外执行后续更新**：所有相关操作必须在同一事务内
- ❌ **多租户事务禁用 `Ado.BeginTran`**：必须使用 `db.BeginTran()`

---

## 总结

### ⭐ SqlSugar 事务的核心要点

根据官方文档，以下是每个 SqlSugar 开发人员必须牢记的核心要点：

1. **⭐ 优先使用 UseTranAsync 语法糖**
   - 官方推荐的异步事务方式
   - 自动处理异常回滚
   - 代码更简洁、更安全

2. **⭐ 上下文一致性是生命线**
   - 事务内所有操作必须使用同一个 `db` 实例
   - 不同实例之间的事务不生效

3. **⭐ 异步操作需要显式传递事务上下文**
   - `UseTranAsync` 自动处理传递
   - 传统方式需要 `TransactionScopeAsyncFlowOption.Enabled`

4. **⭐ 事务范围要尽可能小**
   - 只包含必要的数据库操作
   - 业务计算放在事务外
   - 长时间事务会阻塞其他并发操作

5. **⭐ 异常必须回滚**
   - 使用 `UseTranAsync` 自动处理
   - 或使用 try-catch-finally 确保回滚

6. **⭐ 避免嵌套事务**
   - 由最外层统一管理事务
   - 推荐使用工作单元模式

7. **⭐ 多租户事务注意事项**
   - 只能使用 `db.BeginTran()`
   - 严禁使用 `db.Ado.BeginTran()`

---

### ⭐ 项目当前存在的问题

根据官方文档规范，以下是 RUINORERP 项目当前存在的问题：

1. 🔴 **缺少 UseTranAsync**: 大部分异步操作没有使用官方推荐的语法糖
2. 🔴 **事务边界不清**: 多处开启事务，职责不明
3. 🔴 **长事务问题**: 审核过程事务太长，包含业务计算
4. 🔴 **事务外更新**: UI 层在 Controller 返回后还执行更新
5. 🔴 **缺少超时设置**: 没有设置 CommandTimeout
6. 🔴 **上下文不一致**: 可能在不同 db 实例间操作
7. 🔴 **缺少 UpdLock**: 关键更新操作没有使用更新锁

---

### ⭐ 立即行动项

根据官方文档规范，制定以下优化计划：

1. **[P0] 重构异步事务为 UseTranAsync**
   - 将所有异步事务方法改为使用官方推荐的 `UseTranAsync` 语法糖
   - 确保上下文一致性

2. **[P0] 移除 UI 层的数据库直接操作**
   - 所有数据库操作必须在 Controller 或 Service 层完成
   - UI 层只负责调用和刷新

3. **[P1] 重构审核过程，缩短事务时间**
   - 将业务计算移到事务外
   - 事务内只包含必要的数据库操作

4. **[P1] 设置命令超时**
   - 设置 `CommandTimeout = 30` 秒
   - 快速失败，避免长时间阻塞

5. **[P1] 关键操作添加 UpdLock**
   - 审核、更新等关键操作使用 `With(SqlWith.UpdLock)`
   - 防止脏读和并发问题

6. **[P2] 建立事务使用规范和代码审查清单**
   - 使用本文档作为代码审查标准
   - 建立事务检查清单

---

**文档结束**

---

## ⭐ 官方完整实例代码

### 1. 联表查询 (Join) 实例

#### 基础 INNER JOIN

```csharp
var list = db.Queryable<Order>()
    .InnerJoin<User>((o, u) => o.UserId == u.Id)
    .Select((o, u) => new { OrderId = o.Id, UserName = u.Name })
    .ToList();
```

#### LEFT JOIN (左外连接)

```csharp
var list = db.Queryable<Order>()
    .LeftJoin<User>((o, u) => o.UserId == u.Id)
    .Select((o, u) => new { OrderId = o.Id, UserName = u.Name })
    .ToList();
```

#### 多表 JOIN (链式)

```csharp
var list = db.Queryable<Order>()
    .InnerJoin<User>((o, u) => o.UserId == u.Id)
    .LeftJoin<OrderDetail>((o, u, od) => o.Id == od.OrderId)
    .Select((o, u, od) => new {
        OrderId = o.Id,
        UserName = u.Name,
        ProductName = od.ProductName
    })
    .ToList();
```

#### 完整联表查询实例 (含条件筛选)

```csharp
// 完整的多表联查实例
var result = db.Queryable<Order>()
    .InnerJoin<User>((o, u) => o.UserId == u.Id)
    .LeftJoin<OrderDetail>((o, u, od) => o.Id == od.OrderId)
    .Where((o, u, od) => u.Age > 18 && od.ProductName != "")
    .Select((o, u, od) => new ResultDTO
    {
        OrderId = o.Id,
        UserName = u.Name,
        ProductName = od.ProductName
    })
    .ToList();
```

---

### 2. 导航查询 (Include) 实例

#### 实体配置示例

```csharp
[SugarTable("Order")]
public class Order
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string OrderNo { get; set; }
    public int UserId { get; set; }

    // 一对一关系
    [Navigate(NavigateType.OneToOne, nameof(UserId))]
    public User User { get; set; }

    // 一对多关系
    [Navigate(NavigateType.OneToMany, nameof(OrderDetail.OrderId))]
    public List<OrderDetail> Details { get; set; }
}

[SugarTable("User")]
public class User
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}

[SugarTable("OrderDetail")]
public class OrderDetail
{
    [SugarColumn(IsPrimaryKey = true)]
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}
```

#### 使用 Include 查询

```csharp
// 查询订单并加载用户信息
var orderWithUser = db.Queryable<Order>()
    .Includes(o => o.User)      // 加载一对一关联的用户
    .Includes(o => o.Details)   // 加载一对多的明细
    .Where(o => o.Id == 1)
    .First();

// 链式加载多个导航属性
var orders = db.Queryable<Order>()
    .Include(o => o.User)
    .Include(o => o.Details, d => d.OrderId)
    .ToList();
```

---

### 3. 导航操作 (InsertNav/UpdateNav/DeleteNav) 实例

#### ⭐ InsertNav - 插入主子表

```csharp
// 创建包含子表的主表对象
var order = new Order
{
    OrderNo = "ORD20240101",
    UserId = 1,
    Details = new List<OrderDetail>
    {
        new OrderDetail { ProductName = "产品A", Price = 100 },
        new OrderDetail { ProductName = "产品B", Price = 200 }
    }
};

// 插入主表及所有子表
db.InsertNav(order)
    .Include(z => z.Details)  // 指定要插入的子表
    .ExecuteCommand();
```

#### ⭐ UpdateNav - 更新主子表

```csharp
// 查询并加载子表
var order = db.Queryable<Order>()
    .Includes(o => o.Details)
    .Where(o => o.Id == 1)
    .First();

// 修改主表和子表数据
order.OrderNo = "ORD20240102";
order.Details.First().Price = 150;
order.Details.Add(new OrderDetail { ProductName = "产品C", Price = 300 });

// 更新主表并同步更新子表
db.UpdateNav(order)
    .Include(z => z.Details)
    .ExecuteCommand();
```

#### ⭐ DeleteNav - 删除主子表 (级联删除)

```csharp
var order = db.Queryable<Order>()
    .Includes(o => o.Details)
    .Where(o => o.Id == 1)
    .First();

// 删除主表并级联删除所有子表
db.DeleteNav(order)
    .Include(z => z.Details)
    .ExecuteCommand();
```

#### ⭐ 导航操作与事务结合

```csharp
// 使用 UseTranAsync 确保事务
var result = await db.UseTranAsync(async () =>
{
    var order = new Order
    {
        OrderNo = "ORD20240101",
        UserId = 1,
        Details = new List<OrderDetail>
        {
            new OrderDetail { ProductName = "产品A", Price = 100 }
        }
    };

    await db.InsertNav(order)
        .Include(z => z.Details)
        .ExecuteCommandAsync();

    // 更新用户状态
    await db.Updateable<User>(new User { Id = 1, Status = 1 })
        .ExecuteCommandAsync();

    return true;
});

if (!result.IsSuccess)
{
    throw result.Error;
}
```

---

### 4. 批量操作与事务结合实例

#### Storageable 分组批量操作

```csharp
try
{
    db.Ado.BeginTran();

    // 准备测试数据
    List<Product> list = new List<Product>
    {
        new Product { Id = 1, Name = "产品A", Price = 100 },
        new Product { Id = 2, Name = "产品B", Price = 200 }
    };

    // 分组处理：存在则更新，否则插入
    var x = db.Storageable(list)
        .SplitError(it => string.IsNullOrEmpty(it.Name), "名称不能为空")
        .SplitUpdate(it => it.Any())      // 存在则更新
        .SplitInsert(it => true)          // 其余插入
        .ToStorage();

    Console.WriteLine($"插入: {x.InsertList.Count}, 更新: {x.UpdateList.Count}, 错误: {x.ErrorList.Count}");

    // 在事务中执行批量操作
    x.AsInsertable.ExecuteCommand();      // 批量插入
    x.AsUpdateable.ExecuteCommand();       // 批量更新

    db.Ado.CommitTran();

    // 处理错误
    foreach (var item in x.ErrorList)
    {
        Console.WriteLine($"id={item.Item.Id} : {item.StorageMessage}");
    }
}
catch (Exception ex)
{
    db.Ado.RollbackTran();
    throw;
}
```

#### 分页批量插入 (大数据优化)

```csharp
// 分页批量插入，性能优化
db.Utilities.PageEach(list, 2000, pageList =>
{
    var x = db.Storageable(pageList)
        .SplitUpdate(it => it.Any())      // 存在则更新
        .SplitInsert(it => true)          // 其余插入
        .ToStorage();

    x.BulkCopy();                         // 批量插入
});
```

#### DataTable 批量操作

```csharp
db.Ado.BeginTran();
try
{
    var x = db.Storageable(dt)
        .SplitUpdate(it => it.Any())
        .SplitInsert(it => true)
        .SplitDelete(it => Convert.ToInt32(it["id"]) == 100)
        .WhereColumns("id")
        .ToStorage();

    // 执行顺序：删除 > 更新 > 插入
    x.AsDeleteable.ExecuteCommand();
    x.AsInsertable.IgnoreColumns("id").ExecuteCommand();
    x.AsUpdateable.ExecuteCommand();

    db.Ado.CommitTran();
}
catch
{
    db.Ado.RollbackTran();
    throw;
}
```

---

### 5. Saveable (插入或更新) 实例

#### 基础 Saveable 用法

```csharp
// 如果主键存在则更新，否则插入
var student = new Student { Id = 1, Name = "张三" };
var result = db.Saveable(student).ExecuteCommand();
```

#### 指定列操作

```csharp
// 只更新指定列
var result = db.Saveable(student)
    .UpdateColumns(it => new { it.Name })   // 仅更新Name字段
    .ExecuteCommand();

// 只插入指定列
var result2 = db.Saveable(student)
    .InsertColumns(it => new { it.Name })   // 仅插入Name字段
    .ExecuteCommand();

// 根据非主键字段判断是否存在
var result3 = db.Saveable(student)
    .WhereColumns(it => new { it.Name })    // 根据Name判断
    .ExecuteCommand();
```

#### 批量 Saveable

```csharp
var studentList = new List<Student>
{
    new Student { Id = 1, Name = "李四" },
    new Student { Id = 2, Name = "王五" }
};

// 批量保存，根据主键判断每个对象是插入还是更新
var result = db.Saveable(studentList).ExecuteCommand();
```

#### ⭐ Saveable 与事务结合

```csharp
var result = await db.UseTranAsync(async () =>
{
    var student1 = new Student { Id = 10, Name = "小明" };
    await db.Saveable(student1).ExecuteCommandAsync();

    var student2 = new Student { Id = 11, Name = "小红" };
    await db.Saveable(student2).ExecuteCommandAsync();

    return true;
});

if (!result.IsSuccess)
{
    throw result.Error;
}
```

---

### 6. 分组查询 (GroupBy) 实例

```csharp
// 分组 + 聚合函数
var groupQuery = db.Queryable<Order>()
    .GroupBy(o => o.CustomerId)  // 按客户分组
    .Select(o => new
    {
        CustomerId = o.CustomerId,
        TotalAmount = SqlFunc.AggregateSum(o.Amount),   // 总金额
        AvgAmount = SqlFunc.AggregateAvg(o.Amount),     // 平均金额
        OrderCount = SqlFunc.AggregateCount(o.Id)      // 订单数量
    })
    .ToList();

// Having 子句 (对分组结果筛选)
var havingQuery = db.Queryable<Order>()
    .GroupBy(o => o.CustomerId)
    .Having(o => SqlFunc.AggregateSum(o.Amount) > 1000)  // 总金额>1000
    .Select(o => new
    {
        CustomerId = o.CustomerId,
        TotalAmount = SqlFunc.AggregateSum(o.Amount)
    })
    .ToList();

// 去重查询
var distinctQuery = db.Queryable<Order>()
    .Select(o => o.ProductName)
    .Distinct()  // 去重
    .ToList();

// 分组中计数去重
var groupDistinctQuery = db.Queryable<Order>()
    .GroupBy(o => o.CustomerId)
    .Select(o => new
    {
        CustomerId = o.CustomerId,
        UniqueProductCount = SqlFunc.AggregateCount(SqlFunc.Distinct(o.ProductName))
    })
    .ToList();
```

---

### 7. 子查询实例

#### IN 子查询

```csharp
// 查询年龄大于平均年龄的用户
var list = db.Queryable<User>()
    .Where(u => SqlFunc.Subqueryable<User>()
        .Where(x => x.Age > db.Queryable<User>().Avg(y => y.Age))
        .Select(x => x.Id)
        .Contains(u.Id))
    .ToList();
```

#### EXISTS 子查询

```csharp
// 查询存在订单的用户
var list = db.Queryable<User>()
    .Where(u => SqlFunc.Exists(
        db.Queryable<Order>().Where(o => o.UserId == u.Id)
    ))
    .ToList();
```

#### WHERE 条件中的子查询

```csharp
// 在WHERE条件中使用子查询比较
var list = db.Queryable<User>()
    .Where(u => u.Age > db.Queryable<User>()
        .Where(x => x.Id == 1)
        .Select(x => x.Age)
        .First())
    .ToList();
```

---

### 8. 分页查询实例

```csharp
// 基础分页
var pageResult = db.Queryable<Order>()
    .Where(o => o.CustomerId == 1)
    .OrderBy(o => o.OrderDate, OrderByType.Desc)
    .ToPageList(1, 10);  // 第1页，每页10条

// 带总数的分页
var pageResult = db.Queryable<Order>()
    .Where(o => o.CustomerId == 1)
    .OrderBy(o => o.OrderDate, OrderByType.Desc)
    .ToPageListAsync(1, 10, totalCount);  // 返回总记录数

// 异步分页
RefAsync<int> total = 0;
var list = await db.Queryable<Order>()
    .Where(o => o.CustomerId == 1)
    .ToPageListAsync(1, 10, total);
Console.WriteLine($"总记录数: {total}");
```

---

## 参考资料

### 官方文档 (⭐必读)

- [SqlSugar 官方文档首页](https://www.donet5.com/home/doc)
- [数据事务](https://www.donet5.com/home/Doc?typeId=1183) ⭐
- [单例模式 (SqlSugarScope)](https://www.donet5.com/home/Doc?typeId=1245) ⭐
- [异步查询](https://www.donet5.com/home/Doc?typeId=1184)
- [IOC 注入](https://www.donet5.com/home/Doc?typeId=1224)
- [事务锁](https://www.donet5.com/home/Doc?typeId=1213)
- [入门必看](https://www.donet5.com/home/Doc?typeId=1173)
- [AOP 日志](https://www.donet5.com/home/Doc?typeId=1193)

### 相关资源

- [SqlSugar GitHub 仓库](https://github.com/DotNetNext/SqlSugar)
- [Microsoft - TransactionScope 文档](https://docs.microsoft.com/dotnet/api/system.transactions.transactionscope)
- [SQL Server - 事务隔离级别](https://docs.microsoft.com/sql/relational-databases/sql-server-transaction-locking-and-row-versioning-guide)
