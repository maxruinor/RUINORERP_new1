# BOM成本字段增强 - 简化方案总结

**版本**: 2.0 (简化版)  
**日期**: 2025-04-09  
**核心思路**: 只添加1个字段 `RealTimeCost`,原有 `UnitCost` 重新定义为固定成本

---

## 📊 字段设计(仅2个字段)

### 明细表 (tb_BOM_SDetail)
| 字段 | 说明 | 更新方式 | UI表现 |
|------|------|---------||--------|
| **UnitCost** | 预估成本(标准成本) | ✍️ 手工录入 | ✅ 可编辑 |
| **RealTimeCost** | 实时成本 | 🤖 系统自动更新 | 🔒 只读(灰色) |

### 主表 (tb_BOM_S)
**不需要添加新字段!**

主表的成本字段(`TotalMaterialCost`, `SelfProductionAllCosts`等)是**汇总字段**,通过明细的 `SubtotalUnitCost` 自动计算:

```csharp
// 主表总成本 = SUM(明细.小计)
TotalMaterialCost = SUM(detail.SubtotalUnitCost)

// 明细小计 = 实时成本 * 用量
SubtotalUnitCost = RealTimeCost * UsedQty
```

**成本使用优先级**: `RealTimeCost` > `UnitCost`

---

## 🎯 核心价值

### 解决的问题
✅ **时间滞后**: 缴库时使用 RealTimeCost,避免使用过时的 UnitCost  
✅ **成本准确**: 采购入库自动更新 RealTimeCost,反映最新市场价格  
✅ **成本控制**: UnitCost 作为预估成本,用于预算和差异分析  
✅ **简单清晰**: 只需维护2个字段,用户易理解

### 业务场景
```
场景1: BOM创建
  → 工程师录入 UnitCost=50 (预估标准成本)
  → RealTimeCost 初始化为 50

场景2: 采购入库
  → 原材料采购单价=55
  → 系统自动更新 RealTimeCost=55
  → UnitCost 保持 50 不变

场景3: 制令单创建
  → 优先取 RealTimeCost=55 (最新成本)
  → 若无 RealTimeCost,则用 UnitCost=50

场景4: 缴库审核
  → 使用 RealTimeCost=55 计算库存成本
  → 确保财务报表准确反映实际成本

场景5: 成本分析
  → UnitCost(50) vs RealTimeCost(55)
  → 偏差10%,在正常范围内
  → 若偏差>30%,触发预警
```

---

## 🔧 核心改动点

### 1. 数据库变更
**脚本**: `SQLScripts/BOM_Cost_Enhancement_Migration.sql`

```sql
-- 只添加1个字段
ALTER TABLE tb_BOM_SDetail ADD RealTimeCost MONEY NOT NULL DEFAULT 0;

-- 数据迁移
UPDATE tb_BOM_SDetail SET RealTimeCost = UnitCost;

-- 更新UnitCost说明
EXEC sp_updateextendedproperty 
    @name = N'MS_Description', 
    @value = N'单位成本(固定成本/标准成本,手工录入)', 
    ...
```

### 2. 缴库单审核逻辑改造 ⭐
**文件**: `tb_FinishedGoodsInvControllerPartial.cs` (约第163行)

```csharp
// 获取BOM明细
var bomDetail = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_BOM_SDetail>()
    .Where(d => d.ProdDetailID == child.ProdDetailID 
             && d.BOM_ID == entity.tb_manufacturingorder.BOM_ID)
    .FirstAsync();

// 成本优先级: RealTimeCost > UnitCost
decimal effectiveCost = bomDetail?.RealTimeCost > 0 
    ? bomDetail.RealTimeCost 
    : bomDetail.UnitCost;

CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, effectiveCost);
```

### 3. BOM审核递归更新
**文件**: `tb_BOM_SControllerPartial.cs` (UpdateParentBOMsAsync方法)

```csharp
// 只更新 RealTimeCost,不动 UnitCost
detail.RealTimeCost = selfProductionCost;
// UnitCost 保持手工录入的固定成本值
```

### 4. 采购入库自动更新
**新建方法**: 在采购入库Controller中

```csharp
public async Task UpdateBOMRealTimeCostAfterPurchase(long prodDetailId, decimal purchaseUnitCost)
{
    // 查询所有包含该产品的BOM明细
    var bomDetails = await _unitOfWorkManage.GetDbClient()
        .Queryable<tb_BOM_SDetail>()
        .Where(d => d.ProdDetailID == prodDetailId)
        .ToListAsync();

    foreach (var detail in bomDetails)
    {
        detail.RealTimeCost = purchaseUnitCost; // 只更新实时成本
        detail.SubtotalUnitCost = detail.RealTimeCost * detail.UsedQty;
    }

    await _unitOfWorkManage.GetDbClient()
        .Updateable(bomDetails)
        .UpdateColumns(d => new { d.RealTimeCost, d.SubtotalUnitCost })
        .ExecuteCommandAsync();
}
```

### 5. UI界面调整
**文件**: `UCBillOfMaterials.cs`

```csharp
// 添加 RealTimeCost 列
newSumDataGridViewBOM.FieldNameList.TryAdd("RealTimeCost", true);

// 设置为只读
var realTimeCostCol = newSumDataGridViewBOM.Columns["RealTimeCost"];
realTimeCostCol.ReadOnly = true;
realTimeCostCol.DefaultCellStyle.BackColor = Color.LightGray;

// UnitCost 列标题改为"固定成本"
var unitCostCol = newSumDataGridViewBOM.Columns["UnitCost"];
unitCostCol.HeaderText = "固定成本";
```

---

## 📈 实施计划(简化版)

| 阶段 | 任务 | 工时 |
|------|------|------|
| **Day 1** | 执行数据库迁移脚本 | 0.5天 |
| **Day 2-3** | 后端逻辑改造(缴库单、BOM审核、采购入库) | 2天 |
| **Day 4** | UI界面调整 | 0.5天 |
| **Day 5** | 集成测试与上线 | 1天 |

**总工期**: 5个工作日 (比原方案节省3天!)

---

## ✅ 验收标准

### 功能测试
- [ ] BOM编辑时 UnitCost 可手工修改
- [ ] RealTimeCost 显示为只读(灰色背景)
- [ ] 采购入库后 RealTimeCost 自动更新
- [ ] 制令单创建时优先使用 RealTimeCost
- [ ] 缴库单审核使用 RealTimeCost 计算库存成本
- [ ] BOM审核递归更新 RealTimeCost
- [ ] 成本波动>20%时记录警告日志

### 性能测试
- [ ] 1000条BOM更新响应时间 < 3秒
- [ ] 缴库单审核响应时间 < 2秒

---

## 💡 关键优势(相比三字段方案)

| 维度 | 三字段方案 | 两字段方案(当前) |
|------|-----------|----------------|
| **字段数量** | 3个(UnitCost+FixedCost+RealTimeCost) | 2个(UnitCost+RealTimeCost) |
| **复杂度** | 高,用户需理解3个概念 | 低,只需理解2个概念 |
| **开发工作量** | 8天 | 5天 |
| **UI复杂度** | 需显示3列 | 只需显示2列 |
| **维护成本** | 高 | 低 |
| **功能完整性** | ✅ 完整 | ✅ 完整(无损失) |

**结论**: 两字段方案更简洁高效,功能完全满足需求!

---

## 📝 相关文档

- 📘 详细实施方案: `docs/BOM配方表成本字段增强实施方案.md`
- 📗 快速参考: `docs/BOM成本字段增强_快速参考.md`
- 📙 任务清单: `docs/BOM成本字段增强_实施任务清单.md`
- 🗄️ 迁移脚本: `SQLScripts/BOM_Cost_Enhancement_Migration.sql`

---

**核心理念**: 简单即美,够用就好! ✨
