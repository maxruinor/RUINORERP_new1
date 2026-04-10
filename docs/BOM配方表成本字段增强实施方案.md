# BOM配方表成本字段增强实施方案

**文档版本**: 1.0  
**生成日期**: 2025-04-09  
**关联文档**: [成本计算分析报告.md](./成本计算分析报告.md)

---

## 一、需求背景

### 1.1 当前问题

根据《成本计算分析报告》,现有BOM成本设计存在以下核心问题:

1. **时间滞后性** (中等风险)
   - 缴库单审核时使用 `child.UnitCost`,该值来源于制令单创建时的静态快照
   - 如果制令单提前数月创建,BOM成本可能已过时
   - 导致库存成本与实际生产成本脱节

2. **冷启动问题** (高风险)
   - 半成品/成品首次缴库前可能没有历史成本
   - 直接使用缴库明细中的 UnitCost,若为0或异常值会导致库存成本异常

3. **缺少成本版本管理**
   - 无法区分手工录入的标准成本和系统自动更新的实时成本
   - 成本变更无追溯机制

### 1.2 解决方案

在 `tb_BOM_SDetail` 表中新增两个成本字段:

| 字段名 | 类型 | 说明 | 更新时机 |
|--------|------|------|---------|
| **FixedCost** | MONEY | 固定成本(标准成本) | 初建BOM时手工录入,后续可手动调整 |
| **RealTimeCost** | MONEY | 实时成本 | 采购入库/缴库时自动更新 |

**成本使用策略**:
- **BOM编辑阶段**: 显示 FixedCost 供用户参考和修改
- **制令单创建**: 默认使用 RealTimeCost(如有),否则使用 FixedCost
- **缴库单审核**: 使用 RealTimeCost 作为成本依据(解决时间滞后问题)
- **成本分析**: 对比 FixedCost vs RealTimeCost,识别成本偏差

---

## 二、数据库变更

### 2.1 迁移脚本

执行文件: `SQLScripts/BOM_Cost_Enhancement_Migration.sql`

**主要操作**:
1. 添加 `FixedCost` 和 `RealTimeCost` 字段
2. 将现有 `UnitCost` 数据同步到新字段(保证向后兼容)
3. 创建索引优化查询性能
4. 添加非负约束保证数据完整性

### 2.2 字段关系图

```
tb_BOM_SDetail
├── UnitCost        (保留,用于兼容性,建议逐步废弃)
├── FixedCost       (新增,手工录入的标准成本)
├── RealTimeCost    (新增,系统自动更新的实时成本)
└── SubtotalUnitCost (计算字段 = UnitCost * UsedQty)

成本优先级:
  缴库时: RealTimeCost > FixedCost > UnitCost
  制令单: RealTimeCost > FixedCost
  BOM编辑: 显示 FixedCost 供参考
```

---

## 三、业务逻辑改造

### 3.1 BOM编辑界面改造

**文件**: `RUINORERP.UI/MRP/BOM/UCBillOfMaterials.cs`

#### 3.1.1 增加FixedCost列显示

在BOM明细表格中增加"固定成本"列:

```csharp
// 在 InitListData() 或列初始化处添加
newSumDataGridViewBOM.FieldNameList.TryAdd("FixedCost", true);
newSumDataGridViewBOM.FieldNameList.TryAdd("RealTimeCost", true);

// 设置列属性
var fixedCostCol = newSumDataGridViewBOM.Columns["FixedCost"];
if (fixedCostCol != null)
{
    fixedCostCol.HeaderText = "固定成本";
    fixedCostCol.DefaultCellStyle.Format = "N4";
    fixedCostCol.ReadOnly = false; // 允许手工编辑
}

var realTimeCostCol = newSumDataGridViewBOM.Columns["RealTimeCost"];
if (realTimeCostCol != null)
{
    realTimeCostCol.HeaderText = "实时成本";
    realTimeCostCol.DefaultCellStyle.Format = "N4";
    realTimeCostCol.ReadOnly = true; // 只读,系统自动更新
}
```

#### 3.1.2 成本输入验证

```csharp
private void newSumDataGridViewBOM_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
{
    if (newSumDataGridViewBOM.Columns[e.ColumnIndex].Name == "FixedCost")
    {
        if (decimal.TryParse(e.FormattedValue.ToString(), out decimal cost))
        {
            if (cost < 0)
            {
                e.Cancel = true;
                MessageBox.Show("固定成本不能为负数!", "验证错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        else
        {
            e.Cancel = true;
            MessageBox.Show("请输入有效的数值!", "验证错误", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
```

### 3.2 制令单创建逻辑优化

**文件**: `RUINORERP.Business/tb_ManufacturingOrderControllerPartial.cs`

在创建制令单明细时,优先使用实时成本:

```csharp
// 原逻辑(假设在第XXX行):
// child.UnitCost = bomDetail.UnitCost;

// 新逻辑:
decimal effectiveCost = 0;

// 优先级: RealTimeCost > FixedCost > UnitCost
if (bomDetail.RealTimeCost > 0)
{
    effectiveCost = bomDetail.RealTimeCost;
}
else if (bomDetail.FixedCost > 0)
{
    effectiveCost = bomDetail.FixedCost;
}
else
{
    effectiveCost = bomDetail.UnitCost;
    _logger.LogWarning($"产品{bomDetail.ProdDetailID}的成本均为0,请检查BOM配置");
}

child.UnitCost = effectiveCost;
child.SubtotalUnitCost = effectiveCost * child.RequirementQty;
```

### 3.3 缴库单审核逻辑改造 ⭐ 核心改动

**文件**: `RUINORERP.Business/tb_FinishedGoodsInvControllerPartial.cs`

#### 3.3.1 修改成本获取逻辑(第163行附近)

```csharp
// 原代码:
// CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, child.UnitCost);

// 新代码:
// 从BOM明细获取最新实时成本
var bomDetail = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_BOM_SDetail>()
    .Where(d => d.ProdDetailID == child.ProdDetailID 
             && d.BOM_ID == entity.tb_manufacturingorder.BOM_ID)
    .FirstAsync();

decimal effectiveCost = 0;

if (bomDetail != null)
{
    // 优先使用实时成本
    if (bomDetail.RealTimeCost > 0)
    {
        effectiveCost = bomDetail.RealTimeCost;
    }
    else if (bomDetail.FixedCost > 0)
    {
        effectiveCost = bomDetail.FixedCost;
        _logger.LogInformation($"产品{child.ProdDetailID}无实时成本,使用固定成本:{effectiveCost}");
    }
    else
    {
        effectiveCost = child.UnitCost;
        _logger.LogWarning($"产品{child.ProdDetailID}的BOM成本均为0,使用制令单成本:{effectiveCost}");
    }
}
else
{
    effectiveCost = child.UnitCost;
    _logger.LogWarning($"未找到产品{child.ProdDetailID}的BOM明细,使用制令单成本:{effectiveCost}");
}

// 成本异常波动检测(参考成本计算分析报告第222-228行)
if (inv.Quantity > 0)
{
    decimal costChangeRate = Math.Abs((inv.Inv_Cost - effectiveCost) / inv.Inv_Cost);
    if (costChangeRate > 0.2m) // 超过20%波动
    {
        _logger.LogWarning(
            $"产品{child.ProdDetailID}成本波动超过20%: 原成本={inv.Inv_Cost}, 新成本={effectiveCost}, 波动率={costChangeRate:P2}");
        
        // 可选: 根据配置决定是否阻断
        // if (_appContext.SysConfig.EnableCostChangeBlock)
        // {
        //     throw new Exception($"成本波动过大,请核实后重新缴库!");
        // }
    }
}

CommService.CostCalculations.CostCalculation(_appContext, inv, child.Qty, effectiveCost);
```

#### 3.3.2 反审核逻辑同步修改(第593行附近)

```csharp
// 原代码:
// CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Qty, child.UnitCost);

// 新代码: 同样使用实时成本
var bomDetail = await _unitOfWorkManage.GetDbClient()
    .Queryable<tb_BOM_SDetail>()
    .Where(d => d.ProdDetailID == child.ProdDetailID 
             && d.BOM_ID == entity.tb_manufacturingorder.BOM_ID)
    .FirstAsync();

decimal effectiveCost = bomDetail?.RealTimeCost > 0 
    ? bomDetail.RealTimeCost 
    : (bomDetail?.FixedCost > 0 ? bomDetail.FixedCost : child.UnitCost);

CommService.CostCalculations.AntiCostCalculation(_appContext, inv, child.Qty, effectiveCost);
```

### 3.4 采购入库自动更新RealTimeCost

**文件**: 需新建或在现有采购入库Controller中添加

当采购入库审核时,自动更新相关BOM明细的实时成本:

```csharp
/// <summary>
/// 采购入库审核后,更新BOM明细的实时成本
/// </summary>
public async Task UpdateBOMRealTimeCostAfterPurchase(long prodDetailId, decimal purchaseUnitCost)
{
    try
    {
        // 查询所有包含该产品的BOM明细
        var bomDetails = await _unitOfWorkManage.GetDbClient()
            .Queryable<tb_BOM_SDetail>()
            .Where(d => d.ProdDetailID == prodDetailId)
            .ToListAsync();

        if (bomDetails == null || bomDetails.Count == 0)
        {
            return; // 该产品未被任何BOM引用
        }

        foreach (var detail in bomDetails)
        {
            // 更新实时成本
            detail.RealTimeCost = purchaseUnitCost;
            
            // 重新计算小计
            detail.SubtotalUnitCost = detail.RealTimeCost * detail.UsedQty;
            
            // 更新主表的总物料成本
            var parentBOM = await _unitOfWorkManage.GetDbClient()
                .Queryable<tb_BOM_S>()
                .Where(b => b.BOM_ID == detail.BOM_ID)
                .FirstAsync();
                
            if (parentBOM != null)
            {
                // 重新计算总物料成本
                var allDetails = await _unitOfWorkManage.GetDbClient()
                    .Queryable<tb_BOM_SDetail>()
                    .Where(d => d.BOM_ID == detail.BOM_ID)
                    .ToListAsync();
                    
                parentBOM.TotalMaterialCost = allDetails.Sum(d => d.SubtotalUnitCost);
                parentBOM.SelfProductionAllCosts = parentBOM.TotalMaterialCost 
                    + parentBOM.TotalSelfManuCost 
                    + parentBOM.SelfApportionedCost;
                parentBOM.OutProductionAllCosts = parentBOM.TotalMaterialCost 
                    + parentBOM.TotalOutManuCost 
                    + parentBOM.OutApportionedCost;
                    
                await _unitOfWorkManage.GetDbClient()
                    .Updateable(parentBOM)
                    .UpdateColumns(b => new { 
                        b.TotalMaterialCost, 
                        b.SelfProductionAllCosts, 
                        b.OutProductionAllCosts 
                    })
                    .ExecuteCommandAsync();
            }
        }

        // 批量更新BOM明细
        await _unitOfWorkManage.GetDbClient()
            .Updateable(bomDetails)
            .UpdateColumns(d => new { d.RealTimeCost, d.SubtotalUnitCost })
            .ExecuteCommandAsync();

        _logger.LogInformation($"已更新{bomDetails.Count}条BOM明细的实时成本,产品ID={prodDetailId}, 新成本={purchaseUnitCost}");
    }
    catch (Exception ex)
    {
        _logger.Error(ex, $"更新BOM实时成本失败,产品ID={prodDetailId}");
        throw;
    }
}
```

**调用时机**: 在采购入库单(`tb_PurchaseInbound`)审核成功后调用。

### 3.5 BOM审核递归更新逻辑优化

**文件**: `RUINORERP.Business/tb_BOM_SControllerPartial.cs` (第267-342行)

当前 `UpdateParentBOMsAsync` 方法已经实现了递归更新,需要微调以支持双成本字段:

```csharp
public async Task UpdateParentBOMsAsync(long prodDetailId, decimal selfProductionCost, HashSet<long> processedProdDetailIds = null)
{
    // ... 原有循环检测逻辑保持不变 ...

    try
    {
        var parentBomList = await _appContext.Db.Queryable<tb_BOM_S>()
                              .Includes(x => x.tb_BOM_SDetails)
                              .Where(x => x.tb_BOM_SDetails.Any(z => z.ProdDetailID == prodDetailId))
                              .ToListAsync();

        if (parentBomList == null || parentBomList.Count == 0)
        {
            return;
        }

        foreach (var parentBom in parentBomList)
        {
            bool hasChanges = false;

            foreach (var detail in parentBom.tb_BOM_SDetails)
            {
                if (detail.ProdDetailID == prodDetailId)
                {
                    // 同时更新三个成本字段
                    detail.UnitCost = selfProductionCost;      // 兼容性保留
                    detail.RealTimeCost = selfProductionCost;  // 实时更新
                    // FixedCost 不自动更新,保持手工录入值
                    
                    detail.SubtotalUnitCost = detail.RealTimeCost * detail.UsedQty;
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                parentBom.TotalMaterialCost = parentBom.tb_BOM_SDetails.Sum(c => c.SubtotalUnitCost);
                parentBom.OutProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalOutManuCost + parentBom.OutApportionedCost;
                parentBom.SelfProductionAllCosts = parentBom.TotalMaterialCost + parentBom.TotalSelfManuCost + parentBom.SelfApportionedCost;

                // 保存明细更新(包含RealTimeCost)
                await _unitOfWorkManage.GetDbClient().Updateable<tb_BOM_SDetail>(parentBom.tb_BOM_SDetails)
                      .UpdateColumns(it => new { it.UnitCost, it.RealTimeCost, it.SubtotalUnitCost })
                      .ExecuteCommandHasChangeAsync();

                // 保存主表更新
                await _unitOfWorkManage.GetDbClient().Updateable(parentBom)
                      .UpdateColumns(it => new { it.TotalMaterialCost, it.OutProductionAllCosts, it.SelfProductionAllCosts })
                      .ExecuteCommandHasChangeAsync();

                // 递归处理上一级BOM
                await UpdateParentBOMsAsync(parentBom.ProdDetailID, parentBom.SelfProductionAllCosts, processedProdDetailIds);
            }
        }
    }
    finally
    {
        processedProdDetailIds.Remove(prodDetailId);
    }
}
```

---

## 四、UI界面调整清单

### 4.1 BOM维护界面 (`UCBillOfMaterials.cs`)

- [ ] 在BOM明细网格中增加"固定成本"列(可编辑)
- [ ] 在BOM明细网格中增加"实时成本"列(只读,灰色显示)
- [ ] 添加工具提示: 鼠标悬停时显示成本说明
- [ ] 增加成本对比功能: 显示 FixedCost vs RealTimeCost 的差异百分比

### 4.2 BOM查询界面 (`UCBillOfMaterialsQuery.cs`)

- [ ] 在查询条件中增加成本范围筛选
- [ ] 在汇总行中显示平均固定成本和平均实时成本

### 4.3 BOM追溯界面 (`UCBillOfMaterialsTracker.cs`)

- [ ] 显示每个子件的 FixedCost 和 RealTimeCost
- [ ] 高亮显示成本差异超过阈值的物料

---

## 五、测试用例

### 5.1 功能测试

| 测试场景 | 操作步骤 | 预期结果 |
|---------|---------|---------|
| **TC01: BOM创建时录入固定成本** | 1. 新建BOM<br>2. 添加子件<br>3. 录入FixedCost=10.5 | FixedCost保存成功,RealTimeCost=0 |
| **TC02: 采购入库更新实时成本** | 1. 采购子件产品<br>2. 入库单价=12.0<br>3. 审核入库单 | 相关BOM明细的RealTimeCost自动更新为12.0 |
| **TC03: 制令单使用实时成本** | 1. 创建制令单<br>2. 引用有RealTimeCost的BOM | 制令单明细UnitCost=RealTimeCost |
| **TC04: 缴库单使用实时成本** | 1. 审核缴库单<br>2. BOM有RealTimeCost | 库存成本按RealTimeCost计算 |
| **TC05: 成本异常波动预警** | 1. RealTimeCost相比库存成本波动>20% | 记录警告日志,可选阻断 |
| **TC06: BOM审核递归更新** | 1. 审核下级BOM<br>2. 检查上级BOM | 上级BOM的RealTimeCost和TotalMaterialCost正确更新 |

### 5.2 边界测试

| 测试场景 | 预期行为 |
|---------|---------|
| FixedCost=0, RealTimeCost=0 | 使用UnitCost,记录警告日志 |
| RealTimeCost为负数 | 数据库约束拦截 |
| BOM循环引用 | 现有循环检测机制生效 |
| 首次缴库无历史成本 | 使用BOM的RealTimeCost或FixedCost |

---

## 六、实施计划

### Phase 1: 数据库迁移 (1天)
- [ ] 执行 `BOM_Cost_Enhancement_Migration.sql`
- [ ] 验证字段添加成功
- [ ] 备份数据以防回滚

### Phase 2: 后端逻辑改造 (3天)
- [ ] 修改 `tb_FinishedGoodsInvControllerPartial.cs` (缴库单审核)
- [ ] 修改 `tb_ManufacturingOrderControllerPartial.cs` (制令单创建)
- [ ] 修改 `tb_BOM_SControllerPartial.cs` (BOM审核递归更新)
- [ ] 实现采购入库自动更新RealTimeCost逻辑
- [ ] 单元测试

### Phase 3: UI界面调整 (2天)
- [ ] 修改 `UCBillOfMaterials.cs` (BOM维护界面)
- [ ] 修改 `UCBillOfMaterialsQuery.cs` (查询界面)
- [ ] 修改 `UCBillOfMaterialsTracker.cs` (追溯界面)
- [ ] UI测试

### Phase 4: 集成测试与上线 (2天)
- [ ] 端到端测试(采购→制令→缴库全流程)
- [ ] 性能测试(大批量BOM更新)
- [ ] 用户培训
- [ ] 生产环境部署

**总工期**: 8个工作日

---

## 七、风险评估与应对

| 风险项 | 概率 | 影响 | 应对措施 |
|--------|------|------|---------|
| 历史数据迁移错误 | 低 | 高 | 迁移前完整备份,提供回滚脚本 |
| 性能下降(BOM递归更新) | 中 | 中 | 添加索引,限制递归深度,异步处理 |
| 用户不适应双成本概念 | 中 | 低 | 提供详细培训文档和UI提示 |
| 与其他模块兼容性问题 | 低 | 中 | 保留UnitCost字段,逐步过渡 |

---

## 八、后续优化建议

1. **成本版本历史**: 建立 `tb_BOM_Cost_History` 表,记录每次成本变更
2. **成本预测**: 基于历史采购价格趋势,预测未来成本
3. **成本分析报表**: 开发 FixedCost vs RealTimeCost 差异分析报表
4. **批量成本调整**: 提供批量调整FixedCost的功能(如原材料涨价时)
5. **多币种支持**: 如果涉及跨国采购,考虑多币种成本换算

---

## 九、总结

本次改造通过引入 **FixedCost**(固定成本) 和 **RealTimeCost**(实时成本) 双字段机制,有效解决了:

✅ **时间滞后问题**: 缴库时使用实时成本,避免使用过时的制令单成本  
✅ **冷启动问题**: 优先使用实时成本,备选固定成本,最后才用UnitCost  
✅ **成本追溯**: 明确区分手工录入成本和系统自动更新成本  
✅ **向后兼容**: 保留UnitCost字段,确保现有功能不受影响  

**关键改进点**:
- 缴库单审核逻辑从 `child.UnitCost` 改为 `bomDetail.RealTimeCost`
- 采购入库自动触发BOM实时成本更新
- BOM编辑界面支持手工录入固定成本

此方案符合ERP成本管理最佳实践,能显著提升成本核算的准确性和时效性。
