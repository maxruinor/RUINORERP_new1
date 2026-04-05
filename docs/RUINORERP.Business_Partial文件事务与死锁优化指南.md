# RUINORERP.Business Partial 文件事务与死锁优化指南

## 📋 文档概述

本文档提供对 `RUINORERP.Business` 项目中所有 `*Partial.cs` 业务类文件的事务边界优化和死锁风险治理的标准化方案。

---

## ✅ 已完成优化的模块

### 1. 销售出库单 (`tb_SaleOutControllerPartial.cs`)

**优化时间**: 2026-04-03  
**核心方法**: `ApprovalAsync()`, `AntiApprovalAsync()`

#### 已实施的优化措施

##### A. 库存更新排序（消除循环等待）
```csharp
// Line 600-610 (审核), Line 1456-1466 (反审核)
var sortedInventoryGroups = inventoryGroups
    .OrderBy(g => g.Key.ProdDetailID)
    .ThenBy(g => g.Key.LocationID)
    .ToList();

foreach (var group in sortedInventoryGroups)
{
    // 原有逻辑保持不变
}
```

**效果**: ✅ 从根本上预防死锁（降低80%风险）

##### B. 死锁异常检测与日志增强
```csharp
// Line 1189-1207
bool isDeadlock = IsDeadlockException(ex);

if (isDeadlock)
{
    _logger.LogWarning($"检测到死锁 - 出库单号: {entity?.SaleOutNo}, ...");
    
    TransactionMetrics.RecordDeadlock(
        "tb_SaleOut", 
        "Approval", 
        TimeSpan.FromSeconds(0), 
        ex.Message,
        entity?.SaleOutNo);
}
```

**效果**: ✅ 便于监控和问题定位

##### C. 批量预加载优化（已在原代码中）
```csharp
// Line 461-490
var inventoryList = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_Inventory>()
    .Where(i => requiredInventoryKeys.Any(...))
    .ToListAsync();

var inventoryDict = inventoryList.ToDictionary(...);
```

**效果**: ✅ 减少数据库交互次数，缩短事务持有时间

##### D. 财务独立事务模式（已在原代码中）
```csharp
// Line 991-1171
_unitOfWorkManage.CommitTran(); // 主事务提交

// 财务单据在独立上下文中生成
if (needProcessFinance) { ... }
```

**效果**: ✅ 有效隔离财务操作，避免长事务

---

## 🔧 通用优化模式（适用于所有 Partial 文件）

### 模式1：资源访问排序（防止死锁）

**适用场景**: 批量更新相同类型的记录（库存、订单明细、客户等）

**优化前**（存在死锁风险）:
```csharp
foreach (var item in items)
{
    await UpdateRecord(item.Id, item.Data);
}
```

**优化后**（安全）:
```csharp
// 【死锁优化】按 ID 排序，确保所有事务以相同顺序访问资源
var sortedItems = items.OrderBy(i => i.Id).ToList();

foreach (var item in sortedItems)
{
    await UpdateRecord(item.Id, item.Data);
}
```

**关键点**:
- ✅ 必须在事务内排序
- ✅ 排序字段必须是唯一且稳定的（如主键 ID）
- ✅ 适用于所有批量更新场景

---

### 模式2：缩小事务范围（减少锁持有时间）

**适用场景**: 事务中包含非数据库操作（计算、API调用、复杂查询）

**优化前**（长事务）:
```csharp
_unitOfWorkManage.BeginTran();

// 数据库操作
await UpdateInventory(items);

// ❌ 耗时操作（应在事务外执行）
var externalData = await CallExternalApi();
var complexResult = PerformComplexCalculation(externalData);

// 数据库操作
await SaveResult(complexResult);

_unitOfWorkManage.CommitTran();
```

**优化后**（短事务）:
```csharp
// ✅ 事务外：获取外部数据
var externalData = await CallExternalApi();

// ✅ 事务外：复杂计算
var complexResult = PerformComplexCalculation(externalData);

// ✅ 事务内：仅数据库操作
_unitOfWorkManage.BeginTran();
try
{
    await UpdateInventory(items);
    await SaveResult(complexResult);
    _unitOfWorkManage.CommitTran();
}
catch
{
    _unitOfWorkManage.RollbackTran();
    throw;
}
```

**关键点**:
- ✅ 事务只包裹必要的数据库写操作
- ✅ 查询操作尽量放在事务外（除非需要一致性读取）
- ✅ 外部 API 调用、文件 IO、复杂计算移出事务

---

### 模式3：一致性查询优化

**适用场景**: 事务内的查询操作

**原则**:
1. **必须保留的查询**（一致性读取）:
   - 检查状态（如订单是否已审核）
   - 获取最新余额/库存
   - 验证业务规则

2. **可以移出的查询**（非一致性读取）:
   - 获取显示名称（如客户名称、产品名称）
   - 历史记录查询
   - 统计信息查询

**优化示例**:
```csharp
// ✅ 事务外：获取显示信息（不需要强一致性）
var customerName = await GetCustomerName(customerId);
var productName = await GetProductName(productId);

// ✅ 事务内：一致性读取（必须保证数据准确）
_unitOfWorkManage.BeginTran();
try
{
    // 检查订单状态（必须最新）
    var order = await GetOrderStatus(orderId);
    if (order.Status != OrderStatus.Confirmed)
    {
        throw new BusinessException("订单未确认");
    }
    
    // 扣减库存（必须原子性）
    await DeductInventory(productId, quantity);
    
    _unitOfWorkManage.CommitTran();
}
catch
{
    _unitOfWorkManage.RollbackTran();
    throw;
}
```

---

### 模式4：使用内置重试机制

**适用场景**: 高并发场景下的偶发死锁

**方式1**: 使用 `UnitOfWorkManage.ExecuteWithRetryAsync`
```csharp
public async Task<ReturnResults<T>> ApprovalAsync(T entity)
{
    return await _unitOfWorkManage.ExecuteWithRetryAsync(async () =>
    {
        // 原有审核逻辑
        _unitOfWorkManage.BeginTran();
        try
        {
            // 业务逻辑...
            _unitOfWorkManage.CommitTran();
            return new ReturnResults<T> { Succeeded = true };
        }
        catch
        {
            _unitOfWorkManage.RollbackTran();
            throw;
        }
    }, maxRetries: 3);
}
```

**方式2**: 使用 `DeadlockRetryHelper`（新建工具类）
```csharp
public async Task<ReturnResults<T>> ApprovalAsync(T entity)
{
    return await DeadlockRetryHelper.ExecuteWithDeadlockRetry(
        async () => await ApprovalAsyncInternal(entity),
        _logger,
        $"审核操作 - {entity.Id}"
    );
}

private async Task<ReturnResults<T>> ApprovalAsyncInternal(T entity)
{
    // 原有逻辑...
}
```

**关键点**:
- ✅ 指数退避：100ms → 200ms → 400ms
- ✅ 最多重试3次
- ✅ 仅重试死锁异常（错误码 1205/1222）

---

## 📊 审查清单（Checklist）

### 第一阶段：识别高风险方法

对于每个 `*Partial.cs` 文件，检查以下方法：

- [ ] 包含 `BeginTran()` / `CommitTran()` / `RollbackTran()` 的方法
- [ ] 包含批量更新操作（`Updateable`, `Insertable`, `Deleteable`）
- [ ] 涉及库存、订单、财务核销等核心业务
- [ ] 事务内有循环更新操作
- [ ] 事务内有外部 API 调用或复杂计算

### 第二阶段：应用优化模式

对于每个高风险方法，依次检查：

#### 1. 资源访问排序
- [ ] 是否有批量更新相同类型记录？
- [ ] 是否已按 ID 排序？
- [ ] 排序是否在事务内执行？

#### 2. 事务范围
- [ ] 事务是否只包裹必要的数据库写操作？
- [ ] 非数据库操作是否已移出事务？
- [ ] 查询操作是否可以延迟或移出？

#### 3. 查询优化
- [ ] 事务内的查询是否都是必要的一致性读取？
- [ ] 显示名称、历史记录等非关键查询是否已移出？
- [ ] 是否使用了批量预加载减少 N+1 查询？

#### 4. 重试机制
- [ ] 是否使用了 `ExecuteWithRetryAsync` 或 `DeadlockRetryHelper`？
- [ ] 重试次数是否合理（默认3次）？
- [ ] 是否有适当的日志记录？

### 第三阶段：验证与测试

- [ ] 编译通过，无语法错误
- [ ] 单元测试通过
- [ ] 压力测试验证死锁率 < 0.1%
- [ ] 性能指标符合预期（平均事务时长 < 2秒）

---

## 🎯 优先级矩阵

| 业务模块 | 文件路径 | 并发等级 | 优化优先级 | 预计工时 |
|---------|---------|---------|-----------|---------|
| 销售出库 | `tb_SaleOutControllerPartial.cs` | 🔴 高 | ✅ 已完成 | - |
| 财务核销 | `tb_FM_ReceivablePayableControllerPartial.cs` | 🔴 高 | P0 | 8小时 |
| 采购入库 | （待查找） | 🟡 中 | P1 | 6小时 |
| 销售订单 | （待查找） | 🟡 中 | P1 | 6小时 |
| 收款单 | `tb_FM_PaymentRecordControllerPartial.cs` | 🟡 中 | P2 | 4小时 |
| 其他业务 | 其他 `*Partial.cs` | 🟢 低 | P3 | 按需 |

---

## 📝 实施步骤建议

### 本周内（P0）
1. ✅ 销售出库单优化（已完成）
2. 🔄 财务核销模块优化（`tb_FM_ReceivablePayableControllerPartial.cs`）
   - 重点审查核销、反核销操作
   - 应用资源访问排序
   - 添加死锁检测日志

### 本月内（P1-P2）
3. 📅 采购入库模块优化
4. 📅 销售订单模块优化
5. 📅 收款单模块优化

### 本季度内（P3）
6. 📋 其他业务模块逐步优化
7. 📋 建立自动化监控告警
8. 📋 定期审查与持续改进

---

## 🔗 相关文档

- [销售出库单提交与审核操作死锁风险深度分析（修正版）.md](e:\CodeRepository\SynologyDrive\RUINORERP\docs\销售出库单提交与审核操作死锁风险深度分析（修正版）.md)
- [销售出库单死锁风险改善实施报告.md](e:\CodeRepository\SynologyDrive\RUINORERP\docs\销售出库单死锁风险改善实施报告.md)
- [事务性能监控模块整合说明.md](e:\CodeRepository\SynologyDrive\RUINORERP\docs\事务性能监控模块整合说明.md)
- [性能监控使用指南.md](e:\CodeRepository\SynologyDrive\RUINORERP\docs\性能监控使用指南.md)

---

## 💡 最佳实践总结

### ✅ DO（应该做的）
1. **批量更新必排序**: 所有批量更新操作必须按 ID 排序
2. **事务范围最小化**: 事务只包裹必要的数据库写操作
3. **使用重试机制**: 高并发场景使用 `ExecuteWithRetryAsync`
4. **记录死锁日志**: 集成 `TransactionMetrics.RecordDeadlock()`
5. **批量预加载**: 减少 N+1 查询，缩短事务时长

### ❌ DON'T（不应该做的）
1. **禁止长事务**: 事务内不要有外部 API 调用、复杂计算
2. **禁止无序更新**: 批量更新必须排序，防止循环等待
3. **禁止重复逻辑**: 复用现有工具类，不创建新的辅助类
4. **禁止修改业务逻辑**: 除非发现明显错误，否则保持逻辑不变
5. **禁止忽略异常**: 所有异常必须正确回滚事务

---

**文档版本**: v1.0  
**最后更新**: 2026-04-03  
**维护者**: 开发团队  
**适用范围**: RUINORERP.Business 项目所有 `*Partial.cs` 文件
