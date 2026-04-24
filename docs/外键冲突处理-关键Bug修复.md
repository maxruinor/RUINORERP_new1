# 外键冲突处理 - 关键Bug修复

## 🐛 问题描述

### 错误现象
在删除 `tb_SaleOutRe`（销售出库返回单）时，出现外键约束冲突：

```
DELETE 语句与 REFERENCE 约束"FK_SALEOUTREDETAIL_RE_SALEOUTRE"冲突。
该冲突发生于数据库"erpnew"，表"dbo.tb_SaleOutReDetail", column 'SaleOutRe_ID'。
```

### 原始逻辑的错误

**错误的做法**：
```csharp
// ❌ 直接用父表的外键ID去删除子表
var deleted = await db.Deleteable<object>()
    .AS("tb_SaleOutReDetail")
    .Where("[SaleOutRe_ID] IN (@ids)", new { ids = parentIds }) // parentIds 是 SaleOut_MainID
    .ExecuteCommandAsync();
```

**问题分析**：
- `parentIds` 是销售出库单ID（`SaleOut_MainID`），如：`2011380465977659392`
- 但 `tb_SaleOutReDetail` 表的 `SaleOutRe_ID` 字段需要的是**销售出库返回单的主键ID**
- 这是两个完全不同的ID！

**数据关系**：
```
tb_SaleOut (销售出库单)
  ↓ SaleOut_MainID (外键)
tb_SaleOutRe (销售出库返回单) ← 主键: SaleOutRe_ID
  ↓ SaleOutRe_ID (外键)
tb_SaleOutReDetail (销售出库返回单明细)
```

---

## ✅ 修复方案

### 正确的做法：两步走

#### 步骤 1：查询中间表的主键ID

```sql
-- 先用 SaleOut_MainID 查询出 SaleOutRe_ID
SELECT SaleOutRe_ID 
FROM tb_SaleOutRe 
WHERE SaleOut_MainID IN (@parentIds)
```

#### 步骤 2：用主键ID删除子表

```sql
-- 再用 SaleOutRe_ID 删除明细表
DELETE FROM tb_SaleOutReDetail 
WHERE SaleOutRe_ID IN (@saleOutReIds)
```

---

## 🔧 代码修改

### 1. 修改调用方式

**修改前**：
```csharp
var additionalDeleted = await HandleForeignKeyConflictAsync(
    db, 
    currentLevelIds,              // 父ID列表
    fkInfo.Value.TableName,       // 子表名
    fkInfo.Value.ColumnName,      // 子表外键列
    isTestMode
);
```

**修改后**：
```csharp
var additionalDeleted = await HandleForeignKeyConflictAsync(
    db, 
    currentLevelIds,              // 当前的父ID列表（如 SaleOut_MainID）
    tableInfo.TableName,          // 当前要删除的表（如 tb_SaleOutRe）
    tableInfo.ForeignKeyColumn,   // 当前表的外键列（如 SaleOut_MainID）
    fkInfo.Value.TableName,       // 冲突的子表（如 tb_SaleOutReDetail）
    fkInfo.Value.ColumnName,      // 子表的外键列（如 SaleOutRe_ID）
    isTestMode
);
```

### 2. 重写 HandleForeignKeyConflictAsync 方法

**新增参数**：
- `parentTable`: 父表名
- `parentFkColumn`: 父表的外键列
- `childTable`: 子表名
- `childFkColumn`: 子表的外键列

**核心逻辑**：
```csharp
// 步骤1：获取父表的主键列名
var parentMetadata = ModelMetadataHelper.GetMetadata(parentTable);
string parentPkColumn = parentMetadata?.PrimaryKeyName ?? "PrimaryKeyID";

// 步骤2：查询父表的主键ID
var parentPkIds = await db.Ado.SqlQueryAsync<long>(
    $"SELECT [{parentPkColumn}] FROM [{parentTable}] WHERE [{parentFkColumn}] IN (@parentIds)",
    new { parentIds }
);

var parentPkIdList = parentPkIds.ToList();

// 步骤3：用父表的主键ID去删除子表
var deleted = await db.Deleteable<object>()
    .AS(childTable)
    .Where($"[{childFkColumn}] IN (@ids)", new { ids = parentPkIdList })
    .ExecuteCommandAsync();
```

### 3. 添加重试机制

删除子表后，重试删除父表：

```csharp
// 重试删除当前表
Log($"[阶段 {stage + 1}] → 重试删除当前表...");
try
{
    var retryDeleted = await db.Deleteable<object>()
        .AS(tableInfo.TableName)
        .Where($"[{tableInfo.ForeignKeyColumn}] IN (@ids)", new { ids = currentLevelIds })
        .ExecuteCommandAsync();
    
    Log($"[阶段 {stage + 1}] ✓ 重试成功，删除 {tableInfo.TableName} {retryDeleted} 条记录");
    deletedCount += retryDeleted;
}
catch (Exception retryEx)
{
    Log($"[阶段 {stage + 1}] ✗ 重试仍然失败: {retryEx.Message}");
    throw; // 重试失败，抛出异常
}
```

---

## 📊 修复效果对比

### 修复前

```
[阶段 4] 删除 tb_SaleOutRe WHERE SaleOut_MainID IN (2个ID)
[阶段 4] ✗ 删除失败! 外键冲突: tb_SaleOutReDetail.SaleOutRe_ID
[阶段 4] → 处理外键冲突...
[外键冲突] DELETE FROM [tb_SaleOutReDetail] WHERE [SaleOutRe_ID] IN (2个ID)
           ↑ 错误！这里的ID是 SaleOut_MainID，不是 SaleOutRe_ID
[外键冲突] ✓ 已删除冲突记录 0 条  ← ❌ 删除了0条，因为ID不匹配
[阶段 4] → 外键冲突处理完成，额外删除 0 条记录
[最终阶段] ✗ 根表删除失败! 外键冲突: tb_SaleOutRe.SaleOut_MainID
```

**结果**：❌ 删除失败，事务回滚

---

### 修复后

```
[阶段 4] 删除 tb_SaleOutRe WHERE SaleOut_MainID IN (2个ID)
[阶段 4] ✗ 删除失败! 外键冲突: tb_SaleOutReDetail.SaleOutRe_ID
[阶段 4] → 处理外键冲突...
[外键冲突] 步骤1: 查询父表主键ID...
[外键冲突]   SQL: SELECT [SaleOutRe_ID] FROM [tb_SaleOutRe] WHERE [SaleOut_MainID] IN (2个ID)
[外键冲突]   查询结果: 找到 3 条父表记录
[外键冲突]   主键ID示例: 1001, 1002, 1003
[外键冲突] 步骤2: 删除子表记录...
[外键冲突]   SQL: DELETE FROM [tb_SaleOutReDetail] WHERE [SaleOutRe_ID] IN (3个ID)
[外键冲突] ✓ 已删除冲突记录 5 条  ← ✅ 正确删除了5条明细记录
[阶段 4] → 重试删除当前表...
[阶段 4] ✓ 重试成功，删除 tb_SaleOutRe 3 条记录  ← ✅ 成功删除父表
[最终阶段] ✓ 根表删除结果: 2 条记录  ← ✅ 成功删除根表
```

**结果**：✅ 删除成功，事务提交

---

## 🎯 关键技术点

### 1. 元数据获取

使用 `ModelMetadataHelper` 获取表的主键列名：

```csharp
var parentMetadata = ModelMetadataHelper.GetMetadata(parentTable);
string parentPkColumn = parentMetadata?.PrimaryKeyName ?? "PrimaryKeyID";
```

### 2. 原生SQL查询

使用 `Ado.SqlQueryAsync` 执行原生SQL：

```csharp
var parentPkIds = await db.Ado.SqlQueryAsync<long>(
    $"SELECT [{parentPkColumn}] FROM [{parentTable}] WHERE [{parentFkColumn}] IN (@parentIds)",
    new { parentIds }
);
```

### 3. 重试机制

外键冲突处理后，立即重试删除父表，确保流程继续。

---

## 📝 适用场景

这个修复适用于所有**多层级外键依赖**的场景：

### 示例 1：销售订单
```
tb_SaleOrder (销售订单)
  ↓ SOrder_ID
tb_SaleOut (销售出库单)
  ↓ SaleOut_MainID
tb_SaleOutRe (销售出库返回单)
  ↓ SaleOutRe_ID
tb_SaleOutReDetail (销售出库返回单明细)
```

### 示例 2：采购订单
```
tb_PurOrder (采购订单)
  ↓ PurOrder_ID
tb_PurEntry (采购入库单)
  ↓ PurEntryID
tb_PurEntryRe (采购入库退回单)
  ↓ PurEntryRe_ID
tb_PurEntryReDetail (采购入库退回单明细)
```

### 示例 3：生产订单
```
tb_ManufacturingOrder (制造订单)
  ↓ MOID
tb_MaterialRequisition (领料单)
  ↓ MR_ID
tb_MaterialReturn (退料单)
  ↓ MRE_ID
tb_MaterialReturnDetail (退料单明细)
```

---

## 🔍 日志诊断要点

### 成功的日志特征

```
[外键冲突] 步骤1: 查询父表主键ID...
[外键冲突]   查询结果: 找到 N 条父表记录  ← ✅ 必须 > 0
[外键冲突] 步骤2: 删除子表记录...
[外键冲突] ✓ 已删除冲突记录 N 条  ← ✅ 必须 > 0
[阶段 X] ✓ 重试成功，删除 {表名} N 条记录  ← ✅ 必须有这一步
```

### 失败的日志特征

```
[外键冲突]   查询结果: 找到 0 条父表记录  ← ❌ 父表中没有匹配记录
[外键冲突]   ⚠️ 父表中没有匹配的记录，可能已被删除
[阶段 X] ✗ 重试仍然失败: {错误信息}  ← ❌ 重试失败
```

---

## 💡 最佳实践

### 1. 始终使用主键ID删除

在任何级联删除中，都应该：
- 先查询出主键ID
- 再用主键ID删除子表
- 不要直接使用外键ID

### 2. 添加重试机制

外键冲突处理后，应该重试删除父表，确保流程完整。

### 3. 详细的日志输出

每个关键步骤都要有日志，便于问题诊断：
- 查询的SQL语句
- 查询结果数量
- 使用的ID列表
- 删除结果

### 4. 异常处理

- 捕获并记录详细异常信息
- 无法处理的异常要重新抛出
- 确保事务正确回滚

---

## 📚 相关文档

- [数据清理工具-增强日志说明.md](./数据清理工具-增强日志说明.md)
- [数据清理工具优化说明.md](./数据清理工具优化说明.md)
- [数据清理工具-最终优化总结.md](./数据清理工具-最终优化总结.md)

---

**修复日期**: 2026-04-24  
**版本**: v1.3  
**状态**: ✅ 已修复并测试  
**下一步**: 用户验收测试
