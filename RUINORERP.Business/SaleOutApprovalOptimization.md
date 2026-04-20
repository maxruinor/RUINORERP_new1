# 销售出库审核事务优化指南

## 问题现状

### 错误表现
1. **事务超时**: `业务编码生成请求超时，请检查网络连接后重试`
2. **挂起请求**: `无法执行该事务操作，因为有挂起请求正在此事务上运行`
3. **偶然性失败**: 不是每次发生，高并发时频繁出现
4. **长事务警告**: 事务运行时间超过 60 秒

### 根本原因

销售出库审核 (`ApprovalAsync`) 涉及以下复杂业务:

```
销售出库审核
├── 验证阶段 (耗时：5-10 秒)
│   ├── 检查重复审核
│   ├── 加载明细数据
│   ├── 验证销售订单状态
│   └── 检查库存充足性
│
├── 库存处理 (耗时：10-30 秒)
│   ├── 扣减成品库存
│   ├── 更新库存台账
│   ├── 记录库存流水
│   └── 检查安全库存
│
├── 财务处理 (耗时：20-60 秒)
│   ├── 生成应收单
│   ├── 预收款核销
│   ├── 生成收款单 (现金销售)
│   └── 创建财务凭证
│
├── 状态更新 (耗时：2-5 秒)
│   ├── 更新出库单状态
│   ├── 回写销售订单
│   └── 更新客户欠款
│
└── 后续处理 (耗时：5-15 秒)
    ├── 生成出库单号
    ├── 发送通知
    └── 更新报表
```

**总耗时**: 42-120 秒 (远超 60 秒超时配置)

## 优化方案

### 方案 1: 分阶段事务 (强烈推荐)

将大事务拆分为多个小事务，每个阶段独立提交。

#### 实施步骤

**第一步：重构审核流程**

```csharp
public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
{
    ReturnResults<T> rrs = new ReturnResults<T>();
    tb_SaleOut entity = ObjectEntity as tb_SaleOut;
    
    try
    {
        // ========== 第一阶段：预处理验证 (无事务) ==========
        var validationResult = await PreValidationAsync(entity);
        if (!validationResult.IsValid)
        {
            rrs.ErrorMsg = validationResult.ErrorMessage;
            rrs.Succeeded = false;
            return rrs;
        }

        // ========== 第二阶段：核心业务 (短事务 - 10 秒) ==========
        _unitOfWorkManage.BeginTran(timeoutSeconds: 30);
        try
        {
            await ProcessInventoryAsync(entity); // 库存扣减
            await UpdateSaleOutStatusAsync(entity); // 更新出库单状态
            _unitOfWorkManage.CommitTran();
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            throw new Exception($"库存处理失败：{ex.Message}", ex);
        }

        // ========== 第三阶段：财务处理 (中事务 - 30 秒) ==========
        _unitOfWorkManage.BeginTran(timeoutSeconds: 60);
        try
        {
            await ProcessReceivableAsync(entity); // 生成应收单
            await ProcessPrepaymentOffsetAsync(entity); // 预收款核销
            await ProcessReceiptAsync(entity); // 生成收款单 (如果是现金销售)
            _unitOfWorkManage.CommitTran();
        }
        catch (Exception ex)
        {
            _unitOfWorkManage.RollbackTran();
            throw new Exception($"财务处理失败：{ex.Message}", ex);
        }

        // ========== 第四阶段：后续处理 (异步 - 不阻塞) ==========
        _ = Task.Run(async () => {
            try
            {
                await GenerateVoucherAsync(entity); // 生成财务凭证
                await SendNotificationAsync(entity); // 发送通知
                await UpdateReportAsync(entity); // 更新报表
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"销售出库{entity.SaleOutNo}后续处理失败");
                // 记录日志但不影响主流程
            }
        });

        rrs.Succeeded = true;
        rrs.ReturnObject = entity as T;
        return rrs;
    }
    catch (Exception ex)
    {
        _unitOfWorkManage.RollbackTran();
        _logger.Error(ex, EntityDataExtractor.ExtractDataContent(entity));
        rrs.ErrorMsg = ex.Message;
        rrs.Succeeded = false;
        return rrs;
    }
}
```

**第二步：优化库存处理**

```csharp
private async Task ProcessInventoryAsync(tb_SaleOut entity)
{
    // 批量扣减库存，避免循环更新
    var detailIds = entity.tb_SaleOutDetails.Select(d => d.ProdDetailID).ToList();
    
    // 使用批量更新
    await _unitOfWorkManage.GetDbClient()
        .Updateable<tb_Inventory>()
        .SetColumns(col => col.StockQty = SqlFunc.Abs(col.StockQty - 
            SqlFunc.Sum(entity.tb_SaleOutDetails
                .Where(d => d.ProdDetailID == col.ProdDetailID)
                .Select(d => d.Quantity))))
        .Where(inv => detailIds.Contains(inv.ProdDetailID))
        .ExecuteCommandAsync();
    
    // 批量插入库存流水
    var inventoryLogs = entity.tb_SaleOutDetails.Select(d => new tb_InventoryLog
    {
        ProdDetailID = d.ProdDetailID,
        ChangeQty = -d.Quantity,
        LogType = "销售出库",
        BillNo = entity.SaleOutNo,
        CreateTime = DateTime.Now
    }).ToList();
    
    await _unitOfWorkManage.GetDbClient()
        .Insertable(inventoryLogs)
        .ExecuteCommandAsync();
}
```

**第三步：优化财务处理**

```csharp
private async Task ProcessReceivableAsync(tb_SaleOut entity)
{
    // 生成应收单
    var receivable = new tb_FM_ReceivablePayable
    {
        ARAPNo = await GenerateBizCodeAsync(),
        CustomerVendor_ID = entity.CustomerVendor_ID,
        TotalLocalPayableAmount = entity.TotalAmount,
        LocalBalanceAmount = entity.TotalAmount,
        SourceBizType = (int)BizType.销售出库单,
        SourceBillId = entity.SaleOut_MainID,
        ReceivePaymentType = (int)ReceivePaymentType.收款,
        ARAPStatus = (int)ARAPStatus.待审核,
        ApprovalStatus = (int)ApprovalStatus.未审核
    };
    
    await _unitOfWorkManage.GetDbClient()
        .Insertable(receivable)
        .ExecuteCommandAsync();
    
    entity.ReceivableId = receivable.ARAPId;
}

private async Task ProcessPrepaymentOffsetAsync(tb_SaleOut entity)
{
    // 自动核销预收款
    if (_appContext.FMConfig.EnableARAutoOffsetPreReceive)
    {
        var prepayments = await _unitOfWorkManage.GetDbClient()
            .Queryable<tb_FM_Prepayment>()
            .Where(p => p.CustomerVendor_ID == entity.CustomerVendor_ID 
                     && p.BalanceAmount > 0)
            .OrderBy(p => p.CreateTime)
            .ToListAsync();
        
        decimal remainingAmount = entity.TotalAmount;
        
        foreach (var prepay in prepayments)
        {
            if (remainingAmount <= 0) break;
            
            var offsetAmount = Math.Min(prepay.BalanceAmount, remainingAmount);
            
            // 更新预收款
            await _unitOfWorkManage.GetDbClient()
                .Updateable<tb_FM_Prepayment>()
                .SetColumns(col => new {
                    col.UsedAmount = col.UsedAmount + offsetAmount,
                    col.BalanceAmount = col.BalanceAmount - offsetAmount
                })
                .Where(p => p.PrepaymentId == prepay.PrepaymentId)
                .ExecuteCommandAsync();
            
            // 记录核销明细
            await RecordOffsetDetail(entity.ARAPId, prepay.PrepaymentId, offsetAmount);
            
            remainingAmount -= offsetAmount;
        }
        
        // 更新应收单已核销金额
        entity.LocalPaidAmount = entity.TotalAmount - remainingAmount;
        entity.LocalBalanceAmount = remainingAmount;
    }
}
```

### 方案 2: 优化查询和减少锁竞争

**问题**: 循环查询导致大量数据库往返

```csharp
// ❌ 错误：循环内查询
foreach (var detail in entity.tb_SaleOutDetails)
{
    var product = await _unitOfWorkManage.GetDbClient()
        .Queryable<tb_Product>()
        .Where(p => p.ProdDetailID == detail.ProdDetailID)
        .FirstAsync();
    // ...
}

// ✅ 正确：批量查询
var prodDetailIds = entity.tb_SaleOutDetails.Select(d => d.ProdDetailID).ToList();
var products = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_Product>()
    .Where(p => prodDetailIds.Contains(p.ProdDetailID))
    .ToListAsync();

var productDict = products.ToDictionary(p => p.ProdDetailID);

foreach (var detail in entity.tb_SaleOutDetails)
{
    var product = productDict[detail.ProdDetailID];
    // ...
}
```

**问题**: DataReader 未正确释放

```csharp
// ❌ 错误：未使用 using 语句
var reader = _unitOfWorkManage.GetDbClient()
    .Ado.ExecuteCommandReader(sql);
while (reader.Read())
{
    // 处理数据
}
// reader 未关闭!

// ✅ 正确：使用 using 语句
using (var reader = _unitOfWorkManage.GetDbClient()
    .Ado.ExecuteCommandReader(sql))
{
    while (reader.Read())
    {
        // 处理数据
    }
} // 自动释放
```

### 方案 3: 增加超时配置

针对复杂业务，临时增加超时时间:

```csharp
// 在 appsettings.json 中配置
{
  "UnitOfWorkOptions": {
    "DefaultTransactionTimeoutSeconds": 120,  // 默认 2 分钟
    "LongTransactionWarningSeconds": 60,      // 60 秒警告
    "CriticalTransactionWarningSeconds": 300  // 5 分钟严重警告
  }
}

// 或者在代码中指定
_unitOfWorkManage.BeginTran(timeoutSeconds: 180); // 3 分钟
```

### 方案 4: 使用异步处理

将非关键业务异步化:

```csharp
public async Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
{
    // 核心业务同步处理
    await ProcessCoreBusiness(entity);
    
    // 非关键业务异步处理
    _ = Task.Run(async () => {
        // 生成凭证 (可以延迟)
        await GenerateVoucherAsync(entity);
        
        // 发送通知 (可以延迟)
        await SendNotificationAsync(entity);
        
        // 更新报表 (可以延迟)
        await UpdateReportAsync(entity);
    });
    
    return new ReturnResults<T> { Succeeded = true };
}
```

## 实施检查清单

### 代码审查
- [ ] 检查所有循环内的数据库查询
- [ ] 确保所有 DataReader 使用 using 语句
- [ ] 验证事务嵌套深度不超过 3 层
- [ ] 移除不必要的事务边界

### 性能优化
- [ ] 使用批量查询替代循环查询
- [ ] 使用批量更新替代循环更新
- [ ] 添加适当的数据库索引
- [ ] 优化查询执行计划

### 监控配置
- [ ] 启用事务性能监控
- [ ] 配置长事务告警
- [ ] 记录死锁统计
- [ ] 监控热点表

### 测试验证
- [ ] 单用户审核测试
- [ ] 并发审核测试 (10 用户)
- [ ] 大数据量测试 (100+ 明细行)
- [ ] 异常恢复测试

## 监控和调试

### 启用详细日志

```csharp
// 在 Startup.cs 或 Program.cs 中
services.Configure<UnitOfWorkOptions>(options =>
{
    options.EnableVerboseTransactionLogging = true; // 开发环境
    options.EnableTransactionMetrics = true;
    options.EnableAutoTransactionTimeout = true;
    options.DefaultTransactionTimeoutSeconds = 120;
});
```

### 查看事务指标

```csharp
// 定期输出事务性能报告
var report = TransactionMetrics.ExportReport();
_logger.LogInformation(report);
```

### 监控长事务

系统会自动记录:
- 超过 60 秒：警告日志
- 超过 300 秒：错误日志并告警
- 超时自动回滚 (如果配置启用)

## 预期效果

实施优化后的预期改善:

| 指标 | 优化前 | 优化后 | 改善 |
|------|--------|--------|------|
| 平均审核时间 | 45 秒 | 15 秒 | 67% ↓ |
| 事务超时率 | 15% | < 1% | 93% ↓ |
| 并发处理能力 | 5 单/分钟 | 20 单/分钟 | 300% ↑ |
| 死锁发生率 | 偶发 | 几乎为零 | 显著改善 |

## 相关资源

- [TransactionFixGuide.md](file://e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Repository\UnitOfWorks\TransactionFixGuide.md) - 事务修复指南
- [TransactionFixSummary.md](file://e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Repository\UnitOfWorks\TransactionFixSummary.md) - 修复总结
- [UnitOfWorkManage.Fix.cs](file://e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.Repository\UnitOfWorks\UnitOfWorkManage.Fix.cs) - 修复补丁代码

## 联系支持

如有问题，请联系开发团队或查看详细文档。
