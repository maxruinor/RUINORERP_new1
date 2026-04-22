# 销售出库单未税小计NULL错误修复总结

## 问题描述

**错误信息：**
```
SQL 执行错误：不能将值 NULL 插入列 'SubtotalUntaxedAmount'，表 'erpnew.dbo.tb_SaleOutDetail'；列不允许有 Null 值。INSERT 失败。
```

**发生场景：**
- 提交/审核销售出库单时
- tb_SaleOutDetail表的SubtotalUntaxedAmount字段为NULL
- 数据库约束：该字段IsNullable = false（不允许NULL）

## 根本原因分析

### 1. 模型定义不一致
- **tb_SaleOrderDetail.SubtotalUntaxedAmount**: `decimal` (非nullable)，默认值为0 ✅
- **tb_SaleOutDetail.SubtotalUntaxedAmount**: `decimal?` (nullable) ❌
- **数据库实际约束**: IsNullable = false

### 2. 缺少自动计算逻辑
- **销售订单(UCSaleOrder)**: 有完整的计算公式
  ```csharp
  SubtotalUntaxedAmount = SubtotalTransAmount / (1 + TaxRate)
  ```
- **销售出库(UCSaleOut)**: 缺少此计算公式 ❌

### 3. 转单逻辑缺失
- **SaleOrderToSaleOut方法**: 从订单转为出库单时，没有计算SubtotalUntaxedAmount ❌

### 4. 验证机制缺失
- **tb_SaleOutDetailValidator**: 没有验证SubtotalUntaxedAmount是否为空或为零 ❌

## 修复方案

### 修复1: 修正模型定义
**文件**: `RUINORERP.Model/tb_SaleOutDetail.cs`

```csharp
// 修改前
private decimal? _SubtotalUntaxedAmount;
public decimal? SubtotalUntaxedAmount { ... }
[SugarColumn(..., IsNullable = true, ...)]

// 修改后
private decimal _SubtotalUntaxedAmount = ((0));
public decimal SubtotalUntaxedAmount { ... }
[SugarColumn(..., IsNullable = false, ...)]
```

**影响**: 确保模型与数据库约束一致，设置默认值为0

---

### 修复2: 添加前端自动计算公式
**文件**: `RUINORERP.UI/PSI/SAL/UCSaleOut.cs`

```csharp
// 在SetCol_Formula区域添加
listCols.SetCol_Formula<tb_SaleOutDetail>(
    (a, b) => a.SubtotalTransAmount / (1 + b.TaxRate), 
    c => c.SubtotalUntaxedAmount
);
```

**影响**: 用户编辑明细时，自动计算未税小计，与订单保持一致

---

### 修复3: 添加总额汇总计算
**文件**: `RUINORERP.UI/PSI/SAL/UCSaleOut.cs`

```csharp
// 在Sgh_OnCalculateColumnValue方法中添加
EditEntity.TotalUntaxedAmount = details.Sum(c => c.SubtotalUntaxedAmount);
EditEntity.TotalUntaxedAmount = Math.Round(EditEntity.TotalUntaxedAmount, 
    MainForm.Instance.authorizeController.GetMoneyDataPrecision());
```

**影响**: 主表正确汇总明细的未税金额

---

### 修复4: 完善转单逻辑
**文件**: `RUINORERP.Business/tb_SaleOrderControllerPartial.cs`

在`SaleOrderToSaleOut`方法的两个分支中添加：

```csharp
// 关键修复：计算未税小计，与订单保持一致
details[i].SubtotalUntaxedAmount = details[i].SubtotalTransAmount / (1 + details[i].TaxRate);
details[i].SubtotalUntaxedAmount = Math.Round(details[i].SubtotalUntaxedAmount, 
    authorizeController.GetMoneyDataPrecision());
```

**影响**: 从销售订单转为出库单时，自动计算并填充未税小计

---

### 修复5: 增强验证机制
**文件**: `RUINORERP.Business/Validator/tb_SaleOutDetailValidatorFix.cs`

```csharp
public override void Initialize()
{
    // 关键修复：验证未税小计不能为空或为零
    RuleFor(x => x.SubtotalUntaxedAmount)
        .NotNull().WithMessage("明细中，未税小计：不能为空。")
        .NotEqual(0).When(x => x.SubtotalTransAmount > 0)
            .WithMessage("明细中，成交小计大于零时，未税小计不能为零。请检查税率设置。");
    
    // 验证未税小计的计算公式是否正确
    RuleFor(x => x)
        .Custom((detail, context) =>
        {
            if (detail.SubtotalTransAmount > 0)
            {
                decimal expectedUntaxedAmount = detail.SubtotalTransAmount / (1 + detail.TaxRate);
                expectedUntaxedAmount = Math.Round(expectedUntaxedAmount, 4);
                
                // 允许0.01的误差范围
                if (Math.Abs(detail.SubtotalUntaxedAmount - expectedUntaxedAmount) > 0.01m)
                {
                    context.AddFailure("SubtotalUntaxedAmount", 
                        $"明细中，未税小计计算错误。期望值：{expectedUntaxedAmount}，" +
                        $"实际值：{detail.SubtotalUntaxedAmount}。公式：成交小计/(1+税率)");
                }
            }
        });
    
    // ... 其他验证规则
}
```

**影响**: 
1. 保存前验证SubtotalUntaxedAmount不为空
2. 当成交小计>0时，验证未税小计不为0
3. 验证计算公式的正确性，允许0.01的浮点误差
4. 提供清晰的错误提示，指导用户排查问题

## 修复效果

### 1. 预防层面
- ✅ 前端自动计算，用户无需手动输入
- ✅ 转单时自动填充，避免遗漏
- ✅ 实时验证，保存前发现问题

### 2. 检测层面
- ✅ 验证器捕获NULL值
- ✅ 验证器捕获错误的计算结果
- ✅ 提供明确的错误提示

### 3. 数据一致性
- ✅ 模型定义与数据库约束一致
- ✅ 计算公式与销售订单保持一致
- ✅ 支持多币种、多税率场景

## 测试建议

### 1. 基本功能测试
- [ ] 新建销售出库单，录入明细，验证SubtotalUntaxedAmount自动计算
- [ ] 修改税率，验证SubtotalUntaxedAmount重新计算
- [ ] 修改成交小计，验证SubtotalUntaxedAmount重新计算

### 2. 转单功能测试
- [ ] 从销售订单转为出库单，验证SubtotalUntaxedAmount正确填充
- [ ] 部分出库场景，验证SubtotalUntaxedAmount按比例计算
- [ ] 多次出库场景，验证每次出库的SubtotalUntaxedAmount正确

### 3. 边界条件测试
- [ ] 税率为0时，SubtotalUntaxedAmount = SubtotalTransAmount
- [ ] 赠品(Gift=true)且价格为0时，SubtotalUntaxedAmount = 0
- [ ] 外币订单，验证汇率不影响SubtotalUntaxedAmount计算

### 4. 验证机制测试
- [ ] 手动将SubtotalUntaxedAmount设为NULL，保存时应被拦截
- [ ] 手动将SubtotalUntaxedAmount设为错误值，保存时应被拦截
- [ ] 验证错误提示信息是否清晰易懂

## 相关文件清单

### 修改的文件
1. `RUINORERP.Model/tb_SaleOutDetail.cs` - 模型定义修复
2. `RUINORERP.UI/PSI/SAL/UCSaleOut.cs` - 前端计算逻辑
3. `RUINORERP.Business/tb_SaleOrderControllerPartial.cs` - 转单逻辑
4. `RUINORERP.Business/Validator/tb_SaleOutDetailValidatorFix.cs` - 验证增强

### 参考的文件
1. `RUINORERP.UI/PSI/SAL/UCSaleOrder.cs` - 销售订单的实现参考
2. `RUINORERP.Model/tb_SaleOrderDetail.cs` - 订单明细模型参考
3. `RUINORERP.Business/Validator/tb_SaleOrderValidatorFix.cs` - 订单验证参考

## 注意事项

### 1. 数据库迁移
如果数据库中已存在SubtotalUntaxedAmount为NULL的历史数据，需要执行数据修复：

```sql
-- 修复历史数据：根据公式重新计算
UPDATE tb_SaleOutDetail 
SET SubtotalUntaxedAmount = SubtotalTransAmount / (1 + TaxRate)
WHERE SubtotalUntaxedAmount IS NULL 
  AND SubtotalTransAmount > 0;

-- 对于成交小计为0的记录，设置为0
UPDATE tb_SaleOutDetail 
SET SubtotalUntaxedAmount = 0
WHERE SubtotalUntaxedAmount IS NULL 
  AND SubtotalTransAmount = 0;
```

### 2. 编译检查
修改后需要重新编译以下项目：
- RUINORERP.Model
- RUINORERP.Business
- RUINORERP.UI

### 3. 兼容性
- 本次修复向后兼容，不会影响现有功能
- 验证规则采用渐进式策略，不会阻断正常业务流程
- 允许0.01的计算误差，避免浮点数精度问题

## 总结

本次修复采用了**多层次防御策略**：
1. **模型层**: 修正数据类型，确保与数据库一致
2. **UI层**: 添加自动计算，从源头避免NULL值
3. **业务层**: 完善转单逻辑，保证数据完整性
4. **验证层**: 增强校验规则，提前发现并阻止错误

通过这种分层修复的方式，既解决了当前的NULL值错误，也预防了未来可能出现的类似问题，提升了系统的健壮性和用户体验。
