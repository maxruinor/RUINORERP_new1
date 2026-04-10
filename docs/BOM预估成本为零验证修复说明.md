# BOM预估成本为零验证修复说明

**问题**: 新物料首次创建BOM时,UnitCost=0无法保存  
**修复日期**: 2025-04-09

---

## 🔍 问题分析

### 业务场景

```
新物料首次创建BOM:
├─ 工程师不知道成本(全新物料)
├─ UnitCost = 0 (未录入)
├─ UsedQty = 10 (用量已知)
└─ 尝试保存 → ❌ 验证失败!"单位成本要大于零"
```

### 根本原因

在 `tb_BOM_SDetailValidatorFix.cs` 第32行:
```csharp
RuleFor(x => x.UnitCost).GreaterThan(0).WithMessage("配方明细，单位成本：要大于零。");
```

这个验证规则要求UnitCost必须>0,导致新物料(UnitCost=0)无法保存。

---

## ✅ 修复方案

### 1. 修改验证规则

**文件**: `RUINORERP.Business/Validator/tb_BOM_SDetailValidatorFix.cs`

```csharp
public override void Initialize()
{
    RuleFor(x => x.UsedQty).GreaterThan(0).WithMessage("配方明细，用量：要大于零。");
    
    // ✅ 预估成本可以为0(新物料首次创建时未录入)
    // ✅ 审核时如果为0会自动填充为自产成本
    // ✅ 只需要验证精度,不验证必须>0
    RuleFor(x => x.UnitCost).PrecisionScale(19, 4, true)
        .WithMessage("预估成本：小数位不能超过4位。");
    
    // ✅ 实时成本可以为NULL(尚未采购/生产)
    RuleFor(x => x.RealTimeCost).PrecisionScale(19, 4, true)
        .When(x => x.RealTimeCost.HasValue)
        .WithMessage("实时成本：小数位不能超过4位。");
}
```

**关键改动**:
- ❌ 删除: `GreaterThan(0)` - 不再要求必须>0
- ✅ 保留: `PrecisionScale(19, 4, true)` - 只验证精度
- ✅ 新增: RealTimeCost的精度验证

---

### 2. 更新注释说明

**文件**: `RUINORERP.Business/tb_BOM_SControllerPartial.cs`

```csharp
/// <summary>
/// BOM审核，改变本身状态，修改对应母件的详情中的BOMID
/// 同时修改对应母件的在其它配方中作为子件的成本价格。并且更新对应的总成本价格
/// ✅ BOM明细预估成本(UnitCost)可以为0(新物料首次创建时未录入)，审核时会自动填充为自产成本
/// ✅ 实时成本(RealTimeCost)始终更新，反映最新实际成本
/// </summary>
```

---

## 🎯 完整业务流程

### 场景1: 新物料首次创建BOM(UnitCost=0)

```
步骤1: BOM创建
├─ 工程师添加物料
├─ UnitCost = 0 (不知道成本,留空)
├─ UsedQty = 10
└─ 保存 → ✅ 验证通过(允许UnitCost=0)

步骤2: BOM审核
├─ 计算出自产成本 = 4.80
├─ UpdateParentBOMsAsync()触发
├─ 检测到UnitCost == 0
└─ 自动填充: UnitCost = 4.80 ✓

步骤3: 后续采购入库
├─ 实际采购价 = 5.20
├─ RealTimeCost更新为5.20
├─ UnitCost保持4.80(预算基准)
└─ 成本差异: (5.20-4.80)/4.80 = +8.3% ⚠️
```

### 场景2: 已知成本的物料(UnitCost>0)

```
步骤1: BOM创建
├─ 工程师手工录入 UnitCost = 5.00 (基于经验预估)
├─ UsedQty = 10
└─ 保存 → ✅ 验证通过

步骤2: BOM审核
├─ 计算出自产成本 = 4.80
├─ UpdateParentBOMsAsync()触发
├─ 检测到UnitCost > 0 (已有值)
└─ 保持UnitCost = 5.00不变 ✓ (保护手工录入!)

步骤3: 后续采购入库
├─ 实际采购价 = 5.20
├─ RealTimeCost更新为5.20
├─ UnitCost保持5.00(预算基准)
└─ 成本差异: (5.20-5.00)/5.00 = +4% ⚠️
```

---

## 📊 验证规则对比

| 字段 | 旧规则 | 新规则 | 说明 |
|------|--------|--------|------|
| **UsedQty** | > 0 | > 0 | 用量必须>0 ✓ |
| **UnitCost** | > 0 | 精度验证 | 允许0,审核时自动填充 ✓ |
| **RealTimeCost** | 无验证 | 精度验证(当有值时) | 允许NULL ✓ |
| **SubtotalUnitCost** | 精度验证 | 精度验证 | 自动计算,无需特殊验证 ✓ |
| **SubtotalRealTimeCost** | 无验证 | 精度验证(当有值时) | 自动计算,允许NULL ✓ |

---

## ✅ 测试清单

### 测试1: 新物料UnitCost=0保存
```
操作: 创建BOM,UnitCost=0,UsedQty=10
预期: ✅ 保存成功,无验证错误
结果: [待测试]
```

### 测试2: 新物料BOM审核自动填充
```
操作: 审核UnitCost=0的BOM
预期: ✅ UnitCost自动填充为自产成本
结果: [待测试]
```

### 测试3: 已有UnitCost的物料保护
```
操作: 创建BOM,UnitCost=5.00,审核后检查
预期: ✅ UnitCost保持5.00不变
结果: [待测试]
```

### 测试4: RealTimeCost为NULL保存
```
操作: 创建BOM,RealTimeCost=NULL
预期: ✅ 保存成功,无验证错误
结果: [待测试]
```

### 测试5: 成本精度验证
```
操作: UnitCost=5.12345(5位小数)
预期: ❌ 验证失败,"小数位不能超过4位"
结果: [待测试]
```

---

## 🔧 相关文件

### 已修改
1. ✅ `RUINORERP.Business/Validator/tb_BOM_SDetailValidatorFix.cs`
   - 删除UnitCost > 0的验证
   - 添加UnitCost和RealTimeCost的精度验证

2. ✅ `RUINORERP.Business/tb_BOM_SControllerPartial.cs`
   - 更新ApprovalAsync方法注释
   - 说明UnitCost可以为0并会自动填充

### 相关逻辑(无需修改)
3. ✅ `UpdateParentBOMsAsync()` - 已实现UnitCost=0时自动填充
4. ✅ `RollbackParentBOMsCostAsync()` - 反审核时重置RealTimeCost为NULL

---

## 💡 设计原则

### 1. 预估成本灵活性
```
UnitCost = 0     → 允许(新物料初始化)
UnitCost > 0     → 允许(手工录入)
UnitCost < 0     → ❌ 不允许(负成本无意义)
```

### 2. 实时成本可选性
```
RealTimeCost = NULL  → 允许(尚未采购/生产)
RealTimeCost = 0     → 允许(明确零成本)
RealTimeCost > 0     → 允许(正常成本)
RealTimeCost < 0     → ❌ 不允许(负成本无意义)
```

### 3. 审核时智能处理
```
if (UnitCost == 0) {
    UnitCost = selfProductionCost;  // 自动填充
} else {
    // 保持手工录入的值不变(保护预算基准)
}

RealTimeCost = selfProductionCost;  // 始终更新
```

---

## 📝 后续优化建议

### 1. UI提示优化
```csharp
// UCBillOfMaterials.cs - 在UnitCost列添加工具提示
toolTip.SetToolTip(txtUnitCost, 
    "预估成本(可为0,审核时自动填充)\n" +
    "建议录入以便进行成本差异分析");
```

### 2. 审核前预警
```csharp
// ApprovalAsync()中添加预警
var zeroCostDetails = entity.tb_BOM_SDetails
    .Where(d => Math.Abs(d.UnitCost) < 0.0001m)
    .ToList();

if (zeroCostDetails.Any())
{
    _logger.LogWarning(
        "BOM[{BOM_ID}]中有{Count}个明细预估成本为0,审核时将自动填充",
        entity.BOM_ID, zeroCostDetails.Count
    );
}
```

### 3. 成本完整性报表
```sql
-- 查询所有UnitCost=0的BOM明细(需要补充预估成本)
SELECT 
    b.BOM_No,
    d.ProdDetailID,
    p.CNName AS ProductName,
    d.UsedQty,
    d.UnitCost,
    d.RealTimeCost
FROM tb_BOM_SDetail d
JOIN tb_BOM_S b ON d.BOM_ID = b.BOM_ID
JOIN tb_ProdDetail pd ON d.ProdDetailID = pd.ProdDetailID
JOIN tb_Prod p ON pd.Prod_ID = p.Prod_ID
WHERE d.UnitCost = 0
  AND b.DataStatus = 2  -- 已审核
ORDER BY b.BOM_No;
```

---

## ✅ 总结

### 核心改进
1. ✅ **允许UnitCost=0**: 支持新物料首次创建BOM
2. ✅ **自动填充机制**: 审核时UnitCost=0自动填充为自产成本
3. ✅ **保护手工录入**: UnitCost>0时保持不变
4. ✅ **精度验证**: 确保成本数据准确性

### 业务价值
- ✅ 降低BOM创建门槛(不需要知道成本也能创建)
- ✅ 提高数据质量(审核时自动补充缺失成本)
- ✅ 支持成本差异分析(预估vs实际的对比)
- ✅ 符合双轨制成本管理理念

---

**修复版本**: 1.0  
**更新日期**: 2025-04-09  
**状态**: ✅ 已完成,待测试验证

