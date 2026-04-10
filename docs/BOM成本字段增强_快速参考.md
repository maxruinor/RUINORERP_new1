# BOM成本字段增强 - 快速参考指南

## 📋 新增字段速查

### tb_BOM_SDetail 表

| 字段 | 类型 | 说明 | 更新方式 |
|------|------|------|---------||
| `UnitCost` | MONEY | 固定成本(标准成本) | 手工录入 |
| `RealTimeCost` | MONEY | 实时成本 | 系统自动更新 |

**成本使用优先级**: `RealTimeCost` > `UnitCost`

---

## 🔧 核心代码改动位置

### 1. 缴库单审核 (最关键)
**文件**: `RUINORERP.Business/tb_FinishedGoodsInvControllerPartial.cs`  
**行号**: 约163行

```csharp
// 改动前
CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, child.UnitCost);

// 改动后
var bomDetail = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_BOM_SDetail>()
    .Where(d => d.ProdDetailID == child.ProdDetailID 
             && d.BOM_ID == entity.tb_manufacturingorder.BOM_ID)
    .FirstAsync();

// 优先级: RealTimeCost(不为null且>0) > UnitCost
decimal effectiveCost = (bomDetail?.RealTimeCost.HasValue == true && bomDetail.RealTimeCost.Value > 0)
    ? bomDetail.RealTimeCost.Value
    : bomDetail?.UnitCost ?? 0;

CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, effectiveCost);
```

### 2. BOM审核递归更新
**文件**: `RUINORERP.Business/tb_BOM_SControllerPartial.cs`  
**方法**: `UpdateParentBOMsAsync` (第267行)

```csharp
// 更新实时成本
detail.RealTimeCost = selfProductionCost;
// UnitCost 保持不自动更新,作为固定成本保留
```

### 3. 采购入库触发更新
**新建方法**: 在采购入库Controller中添加

```csharp
public async Task UpdateBOMRealTimeCostAfterPurchase(long prodDetailId, decimal purchaseUnitCost)
{
    // 查询所有包含该产品的BOM明细
    // 更新 RealTimeCost = purchaseUnitCost
    // 重新计算 SubtotalUnitCost 和上级BOM总成本
}
```

---

## 🗄️ 数据库迁移

**执行脚本**: `SQLScripts/BOM_Cost_Enhancement_Migration.sql`

```sql
-- 主要操作(允许NULL,默认0)
ALTER TABLE tb_BOM_SDetail ADD RealTimeCost MONEY NULL DEFAULT 0;

-- 数据迁移
UPDATE tb_BOM_SDetail SET RealTimeCost = UnitCost;
```

---

## 🎨 UI调整要点

### BOM维护界面 (`UCBillOfMaterials.cs`)

```csharp
// 1. 添加列显示
newSumDataGridViewBOM.FieldNameList.TryAdd("RealTimeCost", true);

// 2. 设置属性
var realTimeCostCol = newSumDataGridViewBOM.Columns["RealTimeCost"];
realTimeCostCol.ReadOnly = true; // 只读,系统自动更新
realTimeCostCol.DefaultCellStyle.BackColor = Color.LightGray;

// UnitCost 列保持可编辑(作为固定成本)
var unitCostCol = newSumDataGridViewBOM.Columns["UnitCost"];
unitCostCol.ReadOnly = false;   // 可编辑
unitCostCol.HeaderText = "固定成本"; // 修改列标题
```

---

## ✅ 测试检查清单

- [ ] **TC01**: BOM编辑时UnitCost(固定成本)可手工修改
- [ ] **TC02**: RealTimeCost显示为只读(灰色)
- [ ] **TC03**: 采购入库后RealTimeCost自动更新
- [ ] **TC04**: 制令单创建时优先使用RealTimeCost
- [ ] **TC05**: 缴库单审核使用RealTimeCost
- [ ] **TC06**: BOM审核递归更新RealTimeCost
- [ ] **TC07**: 成本波动>20%时记录警告

---

## ⚠️ 常见问题

### Q1: UnitCost和RealTimeCost有什么区别?
**A**: 
- `UnitCost`: 固定成本(标准成本),手工录入,用于预算控制和成本基准
- `RealTimeCost`: 实时成本,系统根据采购/缴库自动更新,用于业务执行,允许NULL

### Q2: RealTimeCost为NULL或0怎么办?
**A**: 降级使用UnitCost(固定成本)。代码示例:
```csharp
decimal effectiveCost = (bomDetail?.RealTimeCost.HasValue == true && bomDetail.RealTimeCost.Value > 0)
    ? bomDetail.RealTimeCost.Value
    : bomDetail?.UnitCost ?? 0;
```

### Q3: 采购入库如何触发BOM更新?
**A**: 在采购入库单审核成功后,调用 `UpdateBOMRealTimeCostAfterPurchase()` 方法。

### Q4: 性能会受影响吗?
**A**: 已添加索引优化,正常场景下影响可忽略。大批量更新建议异步处理。

---

## 📞 联系人

如有疑问,请联系项目实施团队。

**文档版本**: 1.0  
**更新日期**: 2025-04-09
