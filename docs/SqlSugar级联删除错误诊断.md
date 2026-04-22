# SqlSugar 级联删除错误诊断指南

## 错误信息

```
列名 'TotalAmount' 无效。
实体类型：RUINORERP.Model.tb_CustomerVendor
异常位置：DeleteNav.ThenInclude 级联删除操作
```

## 问题原因

SqlSugar 在执行 `DeleteNav` 级联删除时，会先查询导航属性关联的子表数据，然后删除。错误表明：

**某个被级联删除的表中，实体类定义了 `TotalAmount` 字段，但数据库表中不存在该列。**

## 排查步骤

### 步骤1：启用SqlSugar SQL日志

在 `tb_CustomerVendorController.cs` 的 `BaseDeleteByNavAsync` 方法前添加SQL日志：

```csharp
public async override Task<bool> BaseDeleteByNavAsync(T model) 
{
    tb_CustomerVendor entity = model as tb_CustomerVendor;
    
    // 启用SQL日志
    _unitOfWorkManage.GetDbClient().Aop.OnLogExecuting = (sql, pars) =>
    {
        System.Diagnostics.Debug.WriteLine($"[SQL执行] {sql}");
        if (pars != null && pars.Length > 0)
        {
            foreach (var p in pars)
            {
                System.Diagnostics.Debug.WriteLine($"  参数: {p.ParameterName} = {p.Value}");
            }
        }
    };
    
    _unitOfWorkManage.GetDbClient().Aop.OnError = (exp) =>
    {
        System.Diagnostics.Debug.WriteLine($"[SQL错误] {exp.Message}");
        System.Diagnostics.Debug.WriteLine($"[SQL语句] {exp.Sql}");
    };
    
    bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CustomerVendor>(...)
        .Include(m => m.tb_FM_Invoices)
        // ... 其他Include
        .ExecuteCommandAsync();
        
    return rs;
}
```

### 步骤2：检查可能的问题表

根据级联删除列表，以下表最可能有问题：

#### 高优先级检查（包含 TotalAmount 字段）

1. **tb_FM_Invoice** ✅ 已确认有 TotalAmount 字段
   - 检查数据库是否有此列
   
2. **tb_FM_ReceivablePayable**
   - 有 TotalForeignPayableAmount、TotalLocalPayableAmount
   - 但没有 TotalAmount

3. **tb_FM_PaymentRecord**
   - 有 TotalForeignAmount、TotalLocalAmount
   - 但没有 TotalAmount

4. **tb_FM_Statement**（对账单）
5. **tb_FM_PreReceivedPayment**（预收款）
6. **tb_FM_PriceAdjustment**（价格调整）

### 步骤3：执行诊断SQL

在数据库中执行以下SQL，检查哪些表缺少 TotalAmount 列：

```sql
-- 检查 tb_FM_Invoice 表结构
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'tb_FM_Invoice'
ORDER BY ORDINAL_POSITION;

-- 特别检查是否有 TotalAmount 列
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'TotalAmount'
)
BEGIN
    PRINT '❌ tb_FM_Invoice 表缺少 TotalAmount 列'
END
ELSE
BEGIN
    PRINT '✓ tb_FM_Invoice 表有 TotalAmount 列'
END
GO

-- 检查所有 FM 相关表
DECLARE @tables TABLE (TableName NVARCHAR(100))
INSERT INTO @tables VALUES 
    ('tb_FM_Invoice'),
    ('tb_FM_ReceivablePayable'),
    ('tb_FM_PaymentRecord'),
    ('tb_FM_Statement'),
    ('tb_FM_PreReceivedPayment'),
    ('tb_FM_PriceAdjustment'),
    ('tb_FM_PayeeInfo')

SELECT 
    t.TableName,
    CASE 
        WHEN c.COLUMN_NAME IS NOT NULL THEN '✓ 存在'
        ELSE '❌ 缺失'
    END AS TotalAmount列状态
FROM @tables t
LEFT JOIN INFORMATION_SCHEMA.COLUMNS c 
    ON t.TableName = c.TABLE_NAME 
    AND c.COLUMN_NAME = 'TotalAmount'
GO
```

### 步骤4：修复方案

#### 方案A：添加缺失的数据库列（推荐）

如果 `tb_FM_Invoice` 确实缺少 `TotalAmount` 列：

```sql
-- 为 tb_FM_Invoice 添加 TotalAmount 列
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'tb_FM_Invoice' AND COLUMN_NAME = 'TotalAmount'
)
BEGIN
    ALTER TABLE [tb_FM_Invoice] 
    ADD [TotalAmount] [money] NULL DEFAULT 0
    
    EXEC sys.sp_addextendedproperty 
        @name=N'MS_Description', 
        @value=N'发票总金额（不含税）',
        @level0type=N'SCHEMA',@level0name=N'dbo', 
        @level1type=N'TABLE',@level1name=N'tb_FM_Invoice', 
        @level2type=N'COLUMN',@level2name=N'TotalAmount'
        
    PRINT '✓ 已成功添加 tb_FM_Invoice.TotalAmount 列'
END
GO
```

#### 方案B：从实体类中移除字段

如果数据库中不应该有这个字段，从实体类中删除：

```csharp
// 在 tb_FM_Invoice.cs 中注释或删除
/*
private decimal? _TotalAmount= ((0));
[SugarColumn(...)]
public decimal? TotalAmount { ... }
*/
```

#### 方案C：使用 SugarColumn.IsIgnore

如果只是临时解决，可以标记为忽略：

```csharp
[SugarColumn(IsIgnore = true)]
public decimal? TotalAmount { ... }
```

## 快速定位技巧

### 方法1：二分法排查

注释掉一半的 `.Include()`，看错误是否消失：

```csharp
bool rs = await _unitOfWorkManage.GetDbClient().DeleteNav<tb_CustomerVendor>(...)
    // 先只保留前几个
    .Include(m => m.tb_FM_Invoices)
    .Include(m => m.tb_AS_AfterSaleDeliveries)
    .Include(m => m.tb_ManufacturingOrders)
    // 注释掉其他的
    //.Include(m => m.tb_ManufacturingOrdersByCustomerVendor)
    //.Include(m => m.tb_FM_OtherExpenseDetails)
    //...
    .ExecuteCommandAsync();
```

逐步缩小范围，找到具体是哪个 `.Include()` 导致错误。

### 方法2：单独测试每个导航属性

```csharp
// 单独测试 tb_FM_Invoices
try 
{
    await _db.DeleteNav<tb_CustomerVendor>(m => m.CustomerVendor_ID == id)
        .Include(m => m.tb_FM_Invoices)
        .ExecuteCommandAsync();
    Console.WriteLine("✓ tb_FM_Invoices 正常");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ tb_FM_Invoices 错误: {ex.Message}");
}
```

## 最可能的原因

根据经验，**90%的可能性是 `tb_FM_Invoice` 表缺少 `TotalAmount` 列**。

因为：
1. 实体类中有 `[SugarColumn(ColumnName = "TotalAmount")]`
2. 之前我们刚修改过这个实体，添加了新字段
3. 可能忘记执行数据库迁移脚本

## 立即执行的修复

```sql
-- 1. 检查列是否存在
SELECT COLUMN_NAME 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'tb_FM_Invoice' 
AND COLUMN_NAME = 'TotalAmount';

-- 2. 如果不存在，添加列
ALTER TABLE [tb_FM_Invoice] ADD [TotalAmount] [money] NULL DEFAULT 0;

-- 3. 验证
SELECT TOP 1 TotalAmount FROM tb_FM_Invoice;
```

## 预防措施

1. **实体类变更后，立即同步数据库**
2. **使用数据库迁移脚本管理表结构变更**
3. **在开发环境启用SqlSugar的SQL日志**
4. **单元测试覆盖级联删除场景**
