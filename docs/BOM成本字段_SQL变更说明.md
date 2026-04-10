# BOM成本字段增强 - SQL变更说明

**版本**: 2.1 (支持NULL)  
**日期**: 2025-04-09

---

## 📋 核心变更

### RealTimeCost 字段定义

```sql
RealTimeCost MONEY NULL DEFAULT 0
```

**特性**:
- ✅ **允许NULL**: 新记录或未更新过的记录可以为NULL
- ✅ **默认值0**: 插入时如未指定,默认为0
- ✅ **非负约束**: `(RealTimeCost IS NULL OR RealTimeCost >= 0)`

---

## 🔧 完整迁移脚本

执行文件: `SQLScripts/BOM_Cost_Enhancement_Migration.sql`

### 关键SQL语句

```sql
-- 1. 添加字段(允许NULL,默认0)
ALTER TABLE tb_BOM_SDetail 
ADD RealTimeCost MONEY NULL DEFAULT 0;

-- 2. 数据迁移(将现有UnitCost同步到RealTimeCost)
UPDATE tb_BOM_SDetail 
SET RealTimeCost = UnitCost
WHERE RealTimeCost IS NULL OR RealTimeCost = 0;

-- 3. 更新UnitCost说明
EXEC sp_updateextendedproperty 
    @name = N'MS_Description', 
    @value = N'单位成本(固定成本/标准成本,手工录入)', 
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'tb_BOM_SDetail',
    @level2type = N'COLUMN', @level2name = N'UnitCost';

-- 4. 创建索引
CREATE NONCLUSTERED INDEX IX_BOM_SDetail_RealTimeCost 
ON tb_BOM_SDetail(RealTimeCost)
INCLUDE (ProdDetailID, BOM_ID, UsedQty);

-- 5. 添加约束(允许NULL)
ALTER TABLE tb_BOM_SDetail 
ADD CONSTRAINT CK_BOM_SDetail_Cost_NonNegative 
CHECK ((RealTimeCost IS NULL OR RealTimeCost >= 0) AND UnitCost >= 0);
```

---

## 💡 C#模型定义

```csharp
private decimal? _RealTimeCost;

[SugarColumn(
    ColumnDataType = "money", 
    SqlParameterDbType = "Decimal", 
    ColumnName = "RealTimeCost", 
    DecimalDigits = 4, 
    IsNullable = true,  // ← 允许NULL
    ColumnDescription = "实时成本(系统自动更新,允许NULL)"
)]
public decimal? RealTimeCost
{
    get { return _RealTimeCost; }
    set { SetProperty(ref _RealTimeCost, value); }
}
```

---

## 🎯 使用示例

### 1. 查询时处理NULL

```csharp
// 优先级: RealTimeCost(不为null且>0) > UnitCost
decimal effectiveCost = (bomDetail?.RealTimeCost.HasValue == true && bomDetail.RealTimeCost.Value > 0)
    ? bomDetail.RealTimeCost.Value
    : bomDetail?.UnitCost ?? 0;
```

### 2. 更新时赋值

```csharp
// 采购入库触发更新
detail.RealTimeCost = purchaseUnitCost;  // 直接赋值,可为任何>=0的值或null
```

### 3. SQL查询

```sql
-- 查询有实时成本的记录
SELECT * FROM tb_BOM_SDetail WHERE RealTimeCost IS NOT NULL AND RealTimeCost > 0;

-- 查询需要更新实时成本的记录
SELECT * FROM tb_BOM_SDetail WHERE RealTimeCost IS NULL OR RealTimeCost = 0;
```

---

## ⚠️ 注意事项

### 1. NULL vs 0 的区别

| 值 | 含义 | 使用场景 |
|----|------|---------|
| `NULL` | 从未更新过实时成本 | 新建BOM明细,尚未有采购记录 |
| `0` | 明确设置为0成本 | 免费物料、样品等特殊情况 |
| `>0` | 正常的实时成本 | 已有采购或缴库记录 |

### 2. 代码中的判断逻辑

```csharp
// ❌ 错误写法(无法区分NULL和0)
if (bomDetail.RealTimeCost > 0)

// ✅ 正确写法(先检查HasValue)
if (bomDetail.RealTimeCost.HasValue && bomDetail.RealTimeCost.Value > 0)
```

### 3. 数据库约束

```sql
-- 允许NULL,但如果有值则必须>=0
CHECK ((RealTimeCost IS NULL OR RealTimeCost >= 0) AND UnitCost >= 0)
```

---

## 📊 数据迁移策略

### 迁移前状态
```
SubID | UnitCost | RealTimeCost
------|----------|-------------
1     | 10.00    | (不存在)
2     | 20.00    | (不存在)
```

### 迁移后状态
```
SubID | UnitCost | RealTimeCost
------|----------|-------------
1     | 10.00    | 10.00       ← 从UnitCost复制
2     | 20.00    | 20.00       ← 从UnitCost复制
```

### 新增记录
```
SubID | UnitCost | RealTimeCost
------|----------|-------------
3     | 15.00    | NULL        ← 默认为NULL(或0,取决于INSERT语句)
```

---

## ✅ 验收检查

执行迁移脚本后,验证以下内容:

```sql
-- 1. 检查字段是否存在且允许NULL
SELECT COLUMN_NAME, IS_NULLABLE, COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'tb_BOM_SDetail' 
AND COLUMN_NAME = 'RealTimeCost';
-- 预期: IS_NULLABLE='YES', COLUMN_DEFAULT='((0))'

-- 2. 检查数据迁移是否正确
SELECT TOP 10 SubID, UnitCost, RealTimeCost
FROM tb_BOM_SDetail;
-- 预期: RealTimeCost = UnitCost

-- 3. 检查约束是否生效
INSERT INTO tb_BOM_SDetail (SubID, ProdDetailID, BOM_ID, UnitCost, RealTimeCost)
VALUES (999999, 1, 1, 10, -5);
-- 预期: 失败,违反CHECK约束

-- 4. 检查NULL是否允许
INSERT INTO tb_BOM_SDetail (SubID, ProdDetailID, BOM_ID, UnitCost, RealTimeCost)
VALUES (999998, 1, 1, 10, NULL);
-- 预期: 成功
```

---

## 🔄 回滚方案

如需回滚,执行以下SQL:

```sql
BEGIN TRANSACTION;

-- 1. 删除约束
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_BOM_SDetail_Cost_NonNegative')
    ALTER TABLE tb_BOM_SDetail DROP CONSTRAINT CK_BOM_SDetail_Cost_NonNegative;

-- 2. 删除索引
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_BOM_SDetail_RealTimeCost')
    DROP INDEX IX_BOM_SDetail_RealTimeCost ON tb_BOM_SDetail;

-- 3. 删除字段
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('tb_BOM_SDetail') AND name = 'RealTimeCost')
    ALTER TABLE tb_BOM_SDetail DROP COLUMN RealTimeCost;

COMMIT TRANSACTION;
```

---

**总结**: RealTimeCost字段设计为`MONEY NULL DEFAULT 0`,既保证了灵活性(允许NULL),又提供了默认值保障,同时通过CHECK约束确保数据有效性。
