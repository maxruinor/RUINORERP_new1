# 数据修复服务架构说明

## 目录结构

```
RUINORERP.Business/DataCorrectionServices/
├── IDataCorrectionService.cs          # 服务接口定义
├── DataCorrectionServiceBase.cs       # 抽象基类
├── DataCorrectionServiceManager.cs    # 服务管理器
├── CostFixService.cs                  # 示例：成本修复服务
└── README.md                          # 本文档
```

## 架构设计

### 核心概念

1. **一个修复项 = 一个服务类**
   - 每个数据修复需求对应一个独立的服务类
   - 服务类放在 `DataCorrectionServices` 目录下
   - 命名规范：`{功能名}FixService.cs`

2. **支持多表预览**
   - `PreviewAsync()` 返回 `List<DataPreviewResult>`
   - 每个表一个 `DataPreviewResult`
   - UI可以展示多个表的数据

3. **标准化接口**
   - 所有服务实现 `IDataCorrectionService` 接口
   - 统一的元数据（问题描述、影响表等）
   - 统一的执行结果格式

### 类图

```
┌─────────────────────────────┐
│  IDataCorrectionService     │  <<interface>>
├─────────────────────────────┤
│ + CorrectionName: string    │
│ + FunctionName: string      │
│ + ProblemDescription: str   │
│ + AffectedTables: List<>    │
│ + FixLogic: string          │
│ + OccurrenceScenario: str   │
│ + PreviewAsync(): Task<>    │
│ + ExecuteAsync(): Task<>    │
│ + ValidateAsync(): Task<>   │
└─────────────────────────────┘
            ▲
            │ implements
            │
┌─────────────────────────────┐
│ DataCorrectionServiceBase   │  <<abstract>>
├─────────────────────────────┤
│ # Db: AppDbContext          │
│ # CreateDataTable()         │
│ # AddLog()                  │
│ # RecordAffectedTable()     │
│ # ExecuteWithTransaction()  │
│ # LimitRows()               │
└─────────────────────────────┘
            ▲
            │ extends
            │
┌─────────────────────────────┐
│    CostFixService           │  <<concrete>>
├─────────────────────────────┤
│ + CorrectionName            │
│ + FunctionName              │
│ + PreviewAsync()            │
│ + ExecuteAsync()            │
│ - PreviewInventoryCost()    │
│ - PreviewAffectedOrders()   │
│ - FixInventoryCost()        │
│ - FixOrderCost()            │
└─────────────────────────────┘
```

## 如何添加新的修复服务

### 步骤1：创建服务类文件

在 `RUINORERP.Business/DataCorrectionServices/` 目录下创建新文件，例如：`PurOrderPriceFixService.cs`

### 步骤2：继承基类并实现接口

```csharp
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RUINORERP.Model;

namespace RUINORERP.Business.DataCorrectionServices
{
    /// <summary>
    /// 采购订单价格修复服务
    /// </summary>
    public class PurOrderPriceFixService : DataCorrectionServiceBase
    {
        // 实现所有抽象属性和方法
    }
}
```

### 步骤3：实现元数据属性

```csharp
public override string CorrectionName => "PurOrderPriceFix";

public override string FunctionName => "采购订单价格修复";

public override string ProblemDescription => 
    "修复采购订单主表与明细表价格不一致的问题。" +
    "当手动修改明细价格后未更新主表、并发操作导致数据不一致或历史数据迁移错误时，" +
    "会导致主表TotalAmount与明细合计不符。";

public override List<string> AffectedTables => new List<string>
{
    "tb_PurOrder",
    "tb_PurOrderDetail"
};

public override string FixLogic => 
    "1. 遍历所有采购订单\n" +
    "2. 计算明细表的金额总和\n" +
    "3. 对比主表TotalAmount字段\n" +
    "4. 如果不一致，更新主表金额\n" +
    "5. 记录修复日志";

public override string OccurrenceScenario => 
    "1. 手动修改明细价格后未同步主表\n" +
    "2. 并发操作导致数据不一致\n" +
    "3. 历史数据迁移时计算错误\n" +
    "4. 退货后价格调整未正确回写";
```

### 步骤4：实现预览方法（支持多表）

```csharp
public override async Task<List<DataPreviewResult>> PreviewAsync(Dictionary<string, object> parameters = null)
{
    var results = new List<DataPreviewResult>();
    
    // 表1：采购订单主表
    var orderResult = await PreviewPurOrdersAsync();
    results.Add(orderResult);
    
    // 表2：采购订单明细（可选）
    if (orderResult.NeedFixCount > 0)
    {
        var detailResult = await PreviewPurOrderDetailsAsync();
        results.Add(detailResult);
    }
    
    return results;
}

private async Task<DataPreviewResult> PreviewPurOrdersAsync()
{
    var result = new DataPreviewResult
    {
        TableName = "tb_PurOrder",
        Description = "主表金额与明细合计不一致的订单",
        Data = CreateDataTable("tb_PurOrder")
    };
    
    // 添加列
    result.Data.Columns.Add("订单号", typeof(string));
    result.Data.Columns.Add("主表金额", typeof(decimal));
    result.Data.Columns.Add("明细合计", typeof(decimal));
    result.Data.Columns.Add("差异", typeof(decimal));
    result.Data.Columns.Add("是否一致", typeof(string));
    
    // 查询数据
    var orders = await Db.Queryable<tb_PurOrder>()
        .Includes(o => o.tb_PurOrderDetails)
        .ToListAsync();
    
    int needFixCount = 0;
    
    foreach (var order in orders.Take(100))
    {
        if (order.tb_PurOrderDetails == null || order.tb_PurOrderDetails.Count == 0)
            continue;
        
        decimal detailTotal = order.tb_PurOrderDetails.Sum(d => d.Amount);
        decimal diff = Math.Abs(order.TotalAmount - detailTotal);
        bool isConsistent = diff < 0.01m;
        
        if (!isConsistent)
            needFixCount++;
        
        var row = result.Data.NewRow();
        row["订单号"] = order.PurOrderNo;
        row["主表金额"] = order.TotalAmount;
        row["明细合计"] = detailTotal;
        row["差异"] = diff;
        row["是否一致"] = isConsistent ? "是" : "否";
        
        result.Data.Rows.Add(row);
    }
    
    result.TotalCount = orders.Count;
    result.NeedFixCount = needFixCount;
    result.Data = LimitRows(result.Data, 100);
    
    return result;
}
```

### 步骤5：实现执行方法

```csharp
public override async Task<DataFixExecutionResult> ExecuteAsync(bool testMode = true, Dictionary<string, object> parameters = null)
{
    var stopwatch = Stopwatch.StartNew();
    var result = new DataFixExecutionResult
    {
        Success = false,
        ExecutionTime = DateTime.Now
    };
    
    try
    {
        AddLog(result, $"开始执行采购订单价格修复（{(testMode ? "测试模式" : "正式模式")}）...");
        
        // 验证
        var (isValid, message) = await ValidateAsync();
        if (!isValid)
        {
            result.ErrorMessage = message;
            return result;
        }
        
        // 执行修复
        await ExecuteWithTransactionAsync(async () =>
        {
            var fixCount = await FixPurOrderPricesAsync(result, testMode);
            RecordAffectedTable(result, "tb_PurOrder", fixCount);
            
            result.Success = true;
            return result;
            
        }, testMode);
        
        if (testMode)
        {
            AddLog(result, "测试模式：未实际修改数据库");
        }
        
        AddLog(result, $"修复完成，共影响 {result.AffectedTables.Count} 个表");
    }
    catch (Exception ex)
    {
        result.Success = false;
        result.ErrorMessage = ex.Message;
        AddLog(result, $"执行失败：{ex.Message}");
    }
    finally
    {
        stopwatch.Stop();
        result.ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        AddLog(result, $"总耗时：{result.ElapsedMilliseconds}ms");
    }
    
    return result;
}

private async Task<int> FixPurOrderPricesAsync(DataFixExecutionResult result, bool testMode)
{
    AddLog(result, "正在分析采购订单价格...");
    
    var orders = await Db.Queryable<tb_PurOrder>()
        .Includes(o => o.tb_PurOrderDetails)
        .ToListAsync();
    
    int fixedCount = 0;
    var updateList = new List<tb_PurOrder>();
    
    foreach (var order in orders)
    {
        if (order.tb_PurOrderDetails == null || order.tb_PurOrderDetails.Count == 0)
            continue;
        
        decimal detailTotal = order.tb_PurOrderDetails.Sum(d => d.Amount);
        decimal diff = Math.Abs(order.TotalAmount - detailTotal);
        
        if (diff >= 0.01m)
        {
            order.TotalAmount = detailTotal;
            
            if (!testMode)
            {
                updateList.Add(order);
            }
            
            fixedCount++;
            AddLog(result, $"修复订单 {order.PurOrderNo}: {order.TotalAmount} -> {detailTotal}");
        }
    }
    
    if (!testMode && updateList.Count > 0)
    {
        await Db.Updateable(updateList)
            .UpdateColumns(o => new { o.TotalAmount })
            .ExecuteCommandAsync();
        
        AddLog(result, $"已更新 {updateList.Count} 个订单");
    }
    
    return fixedCount;
}
```

### 步骤6：实现验证方法（可选）

```csharp
public override async Task<(bool IsValid, string Message)> ValidateAsync()
{
    // 检查是否有足够权限
    // 检查是否在业务高峰期
    // 检查数据库连接等
    
    // 示例：检查是否有未完成的采购订单
    var pendingOrders = await Db.Queryable<tb_PurOrder>()
        .Where(o => o.Status == "Pending")
        .CountAsync();
    
    if (pendingOrders > 1000)
    {
        return (false, $"当前有 {pendingOrders} 个待处理订单，建议在业务低峰期执行");
    }
    
    return (true, "验证通过");
}
```

### 步骤7：自动注册

服务类创建完成后，会自动被 `DataCorrectionServiceManager` 发现并注册，无需手动配置。

## 在UI中使用服务

### 修改 UCDataCorrectionCenter.cs

```csharp
using RUINORERP.Business.DataCorrectionServices;

// 在 btnPreview_Click 中
private async void btnPreview_Click(object sender, EventArgs e)
{
    if (string.IsNullOrEmpty(_currentCorrectionItem))
    {
        MessageBox.Show("请先选择一个数据修复项！");
        return;
    }
    
    // 获取对应的服务
    var service = DataCorrectionServiceManager.GetService(_currentCorrectionItem);
    if (service == null)
    {
        richTextBoxLog.AppendText($"未找到修复服务：{_currentCorrectionItem}\r\n");
        return;
    }
    
    richTextBoxLog.Clear();
    richTextBoxLog.AppendText($"正在预览【{service.FunctionName}】...\r\n\r\n");
    
    try
    {
        // 调用服务的预览方法
        var previewResults = await service.PreviewAsync();
        
        // 显示多表预览结果
        foreach (var previewResult in previewResults)
        {
            richTextBoxLog.AppendText($"表：{previewResult.TableName}\r\n");
            richTextBoxLog.AppendText($"说明：{previewResult.Description}\r\n");
            richTextBoxLog.AppendText($"总记录数：{previewResult.TotalCount}\r\n");
            richTextBoxLog.AppendText($"需要修复：{previewResult.NeedFixCount}\r\n");
            richTextBoxLog.AppendText("---\r\n");
            
            // 在DataGridView中显示第一个表的数据
            if (dataGridView1.DataSource == null)
            {
                dataGridView1.DataSource = previewResult.Data;
            }
        }
        
        richTextBoxLog.AppendText($"\r\n共预览 {previewResults.Count} 个表的数据。\r\n");
    }
    catch (Exception ex)
    {
        richTextBoxLog.AppendText($"预览失败：{ex.Message}\r\n");
    }
}

// 在 btnExecute_Click 中
private async void btnExecute_Click(object sender, EventArgs e)
{
    if (string.IsNullOrEmpty(_currentCorrectionItem))
    {
        MessageBox.Show("请先选择一个数据修复项！");
        return;
    }
    
    var service = DataCorrectionServiceManager.GetService(_currentCorrectionItem);
    if (service == null)
    {
        MessageBox.Show($"未找到修复服务：{_currentCorrectionItem}");
        return;
    }
    
    // 二次确认
    DialogResult result = MessageBox.Show(
        $"确定要执行【{service.FunctionName}】吗？\r\n\r\n" +
        $"{(chkTestMode.Checked ? "当前为测试模式，不会真正修改数据。" : "⚠️ 警告：将真正修改数据库！")}",
        "确认执行",
        MessageBoxButtons.YesNo,
        chkTestMode.Checked ? MessageBoxIcon.Question : MessageBoxIcon.Warning);
    
    if (result != DialogResult.Yes)
        return;
    
    richTextBoxLog.Clear();
    richTextBoxLog.AppendText($"开始执行【{service.FunctionName}】...\r\n");
    
    try
    {
        // 调用服务的执行方法
        var executionResult = await service.ExecuteAsync(chkTestMode.Checked);
        
        // 显示执行结果
        richTextBoxLog.AppendText($"\r\n执行结果：{(executionResult.Success ? "成功" : "失败")}\r\n");
        richTextBoxLog.AppendText($"影响表数：{executionResult.AffectedTables.Count}\r\n");
        
        foreach (var kvp in executionResult.AffectedRows)
        {
            richTextBoxLog.AppendText($"  - {kvp.Key}: {kvp.Value} 条记录\r\n");
        }
        
        richTextBoxLog.AppendText($"\r\n详细日志：\r\n");
        foreach (var log in executionResult.Logs)
        {
            richTextBoxLog.AppendText($"{log}\r\n");
        }
        
        if (!string.IsNullOrEmpty(executionResult.ErrorMessage))
        {
            richTextBoxLog.AppendText($"\r\n错误信息：{executionResult.ErrorMessage}\r\n");
        }
        
        richTextBoxLog.AppendText($"\r\n耗时：{executionResult.ElapsedMilliseconds}ms\r\n");
    }
    catch (Exception ex)
    {
        richTextBoxLog.AppendText($"执行失败：{ex.Message}\r\n");
    }
}
```

## 最佳实践

### 1. 命名规范
- 文件名：`{功能名}FixService.cs`
- 类名：`{功能名}FixService`
- CorrectionName：使用英文驼峰命名，如 `"CostFix"`

### 2. 多表预览
- 主要表放在列表第一个
- 相关表按重要性排序
- 每个表都要有清晰的Description

### 3. 性能优化
- 预览时使用 `Take()` 限制返回行数
- 使用 `LimitRows()` 辅助方法
- 避免在预览中执行复杂计算

### 4. 事务处理
- 使用 `ExecuteWithTransactionAsync()` 包装
- 测试模式不启用事务
- 正式模式启用事务保证原子性

### 5. 日志记录
- 每个关键步骤都记录日志
- 包含时间戳便于追踪
- 记录受影响的表和记录数

### 6. 错误处理
- 捕获所有异常
- 记录详细的错误信息
- 返回明确的错误消息

## 示例服务清单

目前已实现的服务：

1. **CostFixService** - 成本修复服务
   - 支持多表预览（库存、订单）
   - 完整的执行逻辑
   - 可作为模板参考

## 后续扩展

可以添加的服务：

1. PurOrderPriceFixService - 采购订单价格修复
2. SaleOrderCostFixService - 销售订单成本修复
3. InventoryStatisticsFixService - 库存统计修复
4. MenuEnumFixService - 菜单枚举修复
5. ... （根据实际需求添加）

## 注意事项

⚠️ **重要提醒**：
1. 每个服务都是独立的，不要相互依赖
2. 预览方法不能修改数据库
3. 执行方法必须支持测试模式
4. 所有数据库操作都要考虑性能
5. 大批量操作要分批处理
6. 记得记录详细的执行日志

---

**版本**: v1.0  
**更新日期**: 2026-04-19  
**维护者**: RUINORERP开发团队
