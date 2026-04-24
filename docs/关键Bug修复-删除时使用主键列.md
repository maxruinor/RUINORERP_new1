# 关键Bug修复 - 删除时使用主键列而非外键列

## 🐛 Bug描述

**问题现象**：预查询成功获取了ID，但实际删除时返回0条记录，最终导致外键约束冲突。

**错误日志**：
```
[预查询] tb_SaleOut.SaleOut_MainID → tb_SaleOutDetail.SaleOut_MainID (主键: SaleOutDetail_ID)
[预查询]   结果: 17 条记录

[阶段 3] 表名: tb_SaleOutDetail
[阶段 3] 待删除ID数量: 17
[阶段 3] SQL: DELETE FROM [tb_SaleOutDetail] WHERE [SaleOut_MainID] IN (17个ID)
[阶段 3] ✓ 删除 0 条记录  ← ❌ 应该是17条！

[根表] SQL: DELETE FROM [tb_SaleOut] WHERE [SaleOut_MainID] IN (11个ID)
✗ 异常: DELETE 语句与 REFERENCE 约束"FK_SALEOUTDETAIL_REF_SALEOUT"冲突
```

---

## 🔍 根本原因

### 错误的代码逻辑

```csharp
// ❌ 错误：使用外键列作为WHERE条件
var idsToDelete = idMap[tableInfo.TableName]; // 这是主键ID列表（如 SaleOutDetail_ID）

await db.Deleteable<object>()
    .AS(tableInfo.TableName)
    .Where($"[{tableInfo.ForeignKeyColumn}] IN (@ids)", new { ids = idsToDelete })
    // 生成SQL: DELETE FROM [tb_SaleOutDetail] WHERE [SaleOut_MainID] IN (...)
    //                                                  ↑ 错误！应该用 SaleOutDetail_ID
    .ExecuteCommandAsync();
```

### 问题分析

1. **预查询阶段**：正确获取了子表的**主键ID**
   ```sql
   SELECT [SaleOutDetail_ID] FROM [tb_SaleOutDetail] WHERE [SaleOut_MainID] IN (@ids)
   -- 得到: [1001, 1002, 1003, ...]  (这些是 SaleOutDetail_ID)
   ```

2. **删除阶段**：错误地使用了**外键列名**作为WHERE条件
   ```sql
   DELETE FROM [tb_SaleOutDetail] WHERE [SaleOut_MainID] IN (1001, 1002, 1003, ...)
   --                                                  ↑ 这里应该是 SaleOutDetail_ID，不是 SaleOut_MainID
   -- 结果: 找不到匹配记录，删除0条
   ```

3. **最终结果**：因为子表没有被删除，删除根表时触发外键约束冲突

---

## ✅ 修复方案

### 修复后的代码

```csharp
// ✅ 正确：获取该表的主键列名
var tableMetadata = ModelMetadataHelper.GetMetadata(tableInfo.TableName);
string primaryKeyColumn = tableMetadata?.PrimaryKeyName ?? "PrimaryKeyID";

// 使用主键列作为WHERE条件
await db.Deleteable<object>()
    .AS(tableInfo.TableName)
    .Where($"[{primaryKeyColumn}] IN (@ids)", new { ids = idsToDelete })
    // 生成SQL: DELETE FROM [tb_SaleOutDetail] WHERE [SaleOutDetail_ID] IN (...)
    //                                                  ↑ 正确！
    .ExecuteCommandAsync();
```

### 完整的修复逻辑

```csharp
for (int stage = 0; stage < deleteOrder.Count; stage++)
{
    var tableInfo = deleteOrder[stage];
    
    // 跳过根表
    if (tableInfo.TableName == rootTableName)
        continue;
    
    // 从 idMap 中获取该表的主键ID列表
    var idsToDelete = idMap.ContainsKey(tableInfo.TableName) 
        ? idMap[tableInfo.TableName] 
        : new List<long>();
    
    // ⚠️ 关键修复：获取该表的主键列名（不是外键列！）
    var tableMetadata = ModelMetadataHelper.GetMetadata(tableInfo.TableName);
    string primaryKeyColumn = tableMetadata?.PrimaryKeyName ?? "PrimaryKeyID";
    
    Log($"[阶段 {stage + 1}] 表名: {tableInfo.TableName}");
    Log($"[阶段 {stage + 1}] 主键列: {primaryKeyColumn}");  // ← 显示主键列
    Log($"[阶段 {stage + 1}] 待删除ID数量: {idsToDelete.Count}");
    
    if (idsToDelete.Count == 0)
        continue;
    
    // ✅ 使用主键列进行删除
    var deleted = await db.Deleteable<object>()
        .AS(tableInfo.TableName)
        .Where($"[{primaryKeyColumn}] IN (@ids)", new { ids = idsToDelete })
        .ExecuteCommandAsync();

    Log($"[阶段 {stage + 1}] ✓ 删除 {deleted} 条记录");
}
```

---

## 📊 修复前后对比

### 修复前（错误）

| 步骤 | 操作 | 使用的列 | 结果 |
|------|------|---------|------|
| 预查询 | `SELECT [SaleOutDetail_ID] FROM ...` | SaleOutDetail_ID（主键） | ✅ 17条 |
| 删除 | `DELETE WHERE [SaleOut_MainID] IN (...)` | SaleOut_MainID（外键） | ❌ 0条 |
| 根表删除 | `DELETE FROM tb_SaleOut` | SaleOut_MainID | ❌ 外键冲突 |

### 修复后（正确）

| 步骤 | 操作 | 使用的列 | 结果 |
|------|------|---------|------|
| 预查询 | `SELECT [SaleOutDetail_ID] FROM ...` | SaleOutDetail_ID（主键） | ✅ 17条 |
| 删除 | `DELETE WHERE [SaleOutDetail_ID] IN (...)` | SaleOutDetail_ID（主键） | ✅ 17条 |
| 根表删除 | `DELETE FROM tb_SaleOut` | SaleOut_MainID | ✅ 成功 |

---

## 🎯 核心原则

### 数据库删除的基本原则

```
预查询：用外键列查询 → 得到主键ID
删除：用主键列删除 → 使用主键ID
```

### 具体示例

```
tb_SaleOut (根表)
  └─ tb_SaleOutDetail (子表)
       外键列: SaleOut_MainID
       主键列: SaleOutDetail_ID

步骤1：预查询
  SELECT [SaleOutDetail_ID]          ← 查询主键
  FROM [tb_SaleOutDetail]
  WHERE [SaleOut_MainID] IN (@rootIds)  ← 用外键过滤
  结果: [1001, 1002, 1003, ...]      ← 得到主键ID列表

步骤2：删除
  DELETE FROM [tb_SaleOutDetail]
  WHERE [SaleOutDetail_ID] IN (1001, 1002, 1003, ...)  ← 用主键删除
                                                        ↑ 这才是正确的！
```

---

## 📝 修改的文件

**文件**: `DataCleanupEngine.cs`  
**方法**: `ExecuteCascadeDeleteByDbAsync`  
**修改位置**: 第855-894行

### 主要变更

1. **新增**：获取表的主键列名
   ```csharp
   var tableMetadata = ModelMetadataHelper.GetMetadata(tableInfo.TableName);
   string primaryKeyColumn = tableMetadata?.PrimaryKeyName ?? "PrimaryKeyID";
   ```

2. **修改**：日志输出
   ```csharp
   // 修改前
   Log($"[阶段 {stage + 1}] 外键列: {tableInfo.ForeignKeyColumn}");
   
   // 修改后
   Log($"[阶段 {stage + 1}] 主键列: {primaryKeyColumn}");
   ```

3. **修改**：测试模式查询
   ```csharp
   // 修改前
   .Where($"[{tableInfo.ForeignKeyColumn}] IN (@ids)", ...)
   
   // 修改后
   .Where($"[{primaryKeyColumn}] IN (@ids)", ...)
   ```

4. **修改**：正式删除
   ```csharp
   // 修改前
   .Where($"[{tableInfo.ForeignKeyColumn}] IN (@ids)", ...)
   
   // 修改后
   .Where($"[{primaryKeyColumn}] IN (@ids)", ...)
   ```

---

## ✅ 预期效果

修复后，日志应该显示：

```
========== [步骤3] 预查询所有表的主键ID ==========
[预查询] tb_SaleOut.SaleOut_MainID → tb_SaleOutDetail.SaleOut_MainID (主键: SaleOutDetail_ID)
[预查询]   结果: 17 条记录

========== [步骤4] 执行删除 ==========

---------- [阶段 3/5] ----------
[阶段 3] 表名: tb_SaleOutDetail
[阶段 3] 主键列: SaleOutDetail_ID  ← ✅ 显示主键列
[阶段 3] 待删除ID数量: 17
[阶段 3] 🔴 正式删除
[阶段 3] SQL: DELETE FROM [tb_SaleOutDetail] WHERE [SaleOutDetail_ID] IN (17个ID)  ← ✅ 使用主键
[阶段 3] ✓ 删除 17 条记录  ← ✅ 成功删除！

---------- [阶段 5/5] ----------
[根表] SQL: DELETE FROM [tb_SaleOut] WHERE [SaleOut_MainID] IN (11个ID)
[根表] ✓ 删除 11 条记录  ← ✅ 无外键冲突

========== [删除汇总] ==========
[汇总] 总删除数: 29
[汇总] 涉及表数: 5
[事务] ✓ 已提交
```

---

## 🎉 总结

这是一个**典型的列名混淆Bug**：

- ❌ **错误思维**：idMap存的是ID，直接用 `ForeignKeyColumn` 删除
- ✅ **正确思维**：idMap存的是**主键ID**，必须用**主键列**删除

**教训**：在数据库操作中，要清楚区分：
1. **外键列**：用于建立表间关系，用于查询过滤
2. **主键列**：用于唯一标识记录，用于删除/更新

---

**修复日期**: 2026-04-24  
**版本**: v2.1  
**状态**: ✅ 已完成  
**下一步**: 用户重新测试验证
