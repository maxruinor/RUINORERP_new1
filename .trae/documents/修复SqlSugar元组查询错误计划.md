# 修复 SqlSugar 元组查询错误计划

## 问题分析

**错误信息**：
```
System.Data.SqlClient.SqlException: 无法绑定由多个部分组成的标识符 "k.Item1"。
```

**根本原因**：
代码中使用了 C# 元组（ValueTuple）类型作为查询条件，SqlSugar 无法正确解析元组属性（Item1、Item2 等），导致生成的 SQL 语句错误。

**问题代码模式**：
```csharp
// ❌ 错误写法 - 使用元组
var allKeys = new List<(long ProdDetailID, long LocationID)>();
var distinctKeys = allKeys.Distinct().ToList();
var inventoryList = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_Inventory>()
    .Where(i => distinctKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.LocationID == i.Location_ID))
    .ToListAsync();
```

**正确写法（参考销售订单）**：
```csharp
// ✅ 正确写法 - 使用匿名类型
var requiredKeys = allKeys.Select(k => new { k.ProdDetailID, k.Location_ID }).Distinct().ToList();
var inventoryList = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_Inventory>()
    .Where(i => requiredKeys.Any(k => k.ProdDetailID == i.ProdDetailID && k.Location_ID == i.Location_ID))
    .ToListAsync();
```

## 需要修复的文件

根据错误提示和代码扫描，以下文件需要修复：

### 1. tb_ManufacturingOrderControllerPartial.cs
- **位置**：ApprovalAsync 方法（第 406-415 行）
- **问题**：使用元组 `List<(long ProdDetailID, long LocationID)>`
- **修复**：转换为匿名类型

### 2. tb_PurOrderControllerPartial.cs
- **位置**：ApprovalAsync、AntiApprovalAsync、BatchCloseCaseAsync 方法
- **问题**：多处使用元组查询
- **修复**：全部转换为匿名类型

### 3. tb_SaleOutControllerPartial.cs
- **位置**：需要检查并修复类似问题

### 4. 其他业务层文件（全面扫描）
需要扫描所有业务控制器文件，找出类似模式并修复

## 修复步骤

### 步骤 1：修复 tb_ManufacturingOrderControllerPartial.cs
- 找到 ApprovalAsync 方法中的元组查询
- 将 `List<(long ProdDetailID, long LocationID)>` 保持用于收集数据
- 在查询前转换为匿名类型：`allKeys2.Select(k => new { k.ProdDetailID, k.LocationID }).Distinct().ToList()`
- 使用匿名类型进行 Where 查询

### 步骤 2：修复 tb_PurOrderControllerPartial.cs
- 扫描所有方法中的元组查询
- 逐一修复为匿名类型查询
- 保持业务逻辑不变

### 步骤 3：扫描并修复其他文件
- 使用 Grep 搜索所有包含类似模式的文件
- 逐个检查并修复
- 确保不改变业务逻辑

### 步骤 4：验证修复
- 检查所有修改的代码
- 确保匿名类型属性名与数据库字段名一致
- 确认没有使用 dynamic 类型

## 注意事项

1. **不改变业务逻辑**：仅修改查询方式，业务逻辑保持完全一致
2. **属性名称匹配**：确保匿名类型的属性名与数据库字段名一致（注意 Location_ID vs LocationID）
3. **避免 dynamic**：不使用任何 dynamic 类型
4. **全面扫描**：确保所有类似问题都被修复

## 文件清单（待修复）

- [ ] tb_ManufacturingOrderControllerPartial.cs
- [ ] tb_PurOrderControllerPartial.cs
- [ ] tb_SaleOutControllerPartial.cs
- [ ] 其他待扫描发现的文件
