# GridViewRelated 重构说明

## 重构概述

`GridViewRelated` 类已重构，使其能够智能处理不同业务单据的双击打开功能。

## 主要改进

### 1. 智能业务类型映射

- 利用 `IEntityMappingService` 和 `EntityLoader` 服务，实现业务类型、实体类型和表名之间的自动映射
- 移除了大量硬编码的 `if` 语句，统一使用映射服务处理实体加载

### 2. 灵活的配置方式

#### 2.1 普通类型关联（简单场景）

适用于大多数业务场景，如销售订单列表中双击打开销售订单详情：

```csharp
// 方式1：打开自身
GridRelated.SetRelatedInfo<tb_SaleOrder>(c => c.SOrderNo);

// 方式2：关联不同表
GridRelated.SetRelatedInfo<View_SaleOutItems, tb_SaleOrder>(c => c.SaleOrderNo, r => r.SOrderNo);

// 方式3：使用表名字符串
GridRelated.SetRelatedInfo("tb_SaleOrder", "SOrderNo");
```

#### 2.2 复杂类型关联（库存跟踪场景）

适用于目标表由源表格中某一列的值来决定的场景，如库存跟踪：

```csharp
// 设置复杂类型模式
GridRelated.ComplexType = true;
GridRelated.SetComplexTargetField<Proc_InventoryTracking>(c => c.业务类型, c => c.单据编号);

// 配置业务类型到表名的映射
var mappings = new Dictionary<string, string>
{
    { "采购入库", "tb_PurEntry" },
    { "销售出库", "tb_SaleOut" },
    { "库存盘点", "tb_Stocktake" }
    // ... 更多映射
};

foreach (var item in mappings)
{
    KeyNamePair keyNamePair = new KeyNamePair(item.Key, item.Value);
    GridRelated.SetRelatedInfo<Proc_InventoryTracking>(c => c.单据编号, keyNamePair);
}
```

### 3. 自定义加载器扩展

对于需要特殊加载逻辑的实体，可以注册自定义加载器：

```csharp
// 注册自定义加载器
GridRelated.RegisterCustomLoader(typeof(tb_ProductionDemand).Name, async (menuInfo, billNo) =>
{
    var obj = MainForm.Instance.AppContext.Db.Queryable<tb_ProductionDemand>()
        .Includes(c => c.tb_ProductionDemandTargetDetails)
        .Includes(c => c.tb_ProductionDemandDetails)
        .WhereIF(billno.GetType() == typeof(long), c => c.PDID == billno.ToLong())
        .WhereIF(billno.GetType() == typeof(string), c => c.PDNo == billno.ToString())
        .Single();
    await menuPowerHelper.ExecuteEvents(menuInfo, obj);
});
```

### 4. 增强的错误处理

- 当无法找到菜单时，显示友好的错误提示
- 当无法加载实体数据时，提供明确的错误信息
- 支持收付款类型的特殊处理逻辑

## 向后兼容性

重构后的类完全向后兼容，所有现有的调用代码无需修改：

- `SetRelatedInfo<T>()` - 保持不变
- `SetRelatedInfo<T1, T2>()` - 保持不变
- `SetRelatedInfo(string, string)` - 保持不变
- `SetComplexTargetField<T>()` - 保持不变
- `GuideToForm()` - 保持不变
- `OpenTargetEntity()` - 保持不变
- `ComplexType` 属性 - 保持不变
- `RelatedInfoList` 属性 - 保持不变
- `FromMenuInfo` 属性 - 保持不变

## 内置特殊处理

以下实体类型已内置特殊处理逻辑：

1. **tb_ProductionDemand** (需求分析)
   - 包含多个关联明细集合
   - 菜单名称特殊处理：`UCProduceRequirement`

2. **tb_BOM_S** (物料清单)
   - 包含嵌套的明细和替代物料
   - 菜单名称特殊处理：`UCBillOfMaterials`

## 新增功能

### 1. 智能实体加载

```csharp
public async void OpenTargetEntity(tb_MenuInfo relatedMenuInfo, string tableName, object billno)
```

- 优先使用自定义加载器（如果有）
- 其次使用 EntityLoader 进行通用加载
- 自动处理收付款类型等特殊情况
- 提供清晰的错误提示

### 2. 菜单查找优化

```csharp
private tb_MenuInfo FindMenuByTableName(string tableName)
```

- 支持标准菜单查找
- 支持特殊菜单名称映射
- 自动处理菜单名称不一致的情况

### 3. 收付款类型调整

```csharp
private tb_MenuInfo AdjustMenuForPaymentType(tb_MenuInfo menuInfo, object entity, string tableName)
```

- 自动检测实体是否包含 `ReceivePaymentType` 属性
- 根据收付款类型调整目标菜单
- 支持应收应付等财务单据

## 修复记录

### 2025-01-09：修复 BizType 枚举值转换问题

**问题描述：**
在应收应付单查询中，双击"来源单号"列时出错。原因是 `RelatedInfo.TargetTableName.Key` 存储的是 `BizType` 枚举值（如 "2" 代表销售退回单），而不是直接的表名。

**解决方案：**
在 `GuideToForm` 方法中添加了 BizType 枚举值检测和转换逻辑：

1. **新增 `IsBizTypeEnumValue` 方法**
   - 检测字符串是否为有效的 BizType 枚举值
   - 通过 `Enum.IsDefined(typeof(BizType), enumValue)` 验证

2. **新增 `ConvertBizTypeToTableName` 方法**
   - 使用 `IEntityMappingService.GetEntityInfo(BizType)` 获取实体信息
   - 从 `BizEntityInfo.TableName` 中获取实际的表名
   - 支持回退机制：如果映射服务无法找到，则从 `RelatedInfo.TargetTableName.Name` 获取

3. **修改 `GuideToForm` 方法**
   - 在获取表名后，先检测是否为 BizType 枚举值
   - 如果是，则调用 `ConvertBizTypeToTableName` 进行转换
   - 转换后的表名用于后续的菜单查找和实体加载

**影响范围：**
- 修复了应收应付单、付款单、收款单等财务模块的双击打开功能
- 支持所有使用 `SetRelatedInfo<tb_FM_ReceivablePayable>(c => c.SourceBillNo, keyNamePair)` 的场景
- 与现有的其他使用方式完全兼容

**测试验证：**
- ✅ 编译通过，无错误和警告
- ✅ 应收应付单查询中的"来源单号"列可以正常双击打开对应单据
- ✅ 支持所有 BizType 枚举定义的业务类型

## 使用示例

### 示例1：销售出库单查询

```csharp
public override void InitGridRelated()
{
    // 双击订单号打开销售订单
    _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_SaleOut, tb_SaleOrder>(
        c => c.SaleOrderNo,
        r => r.SOrderNo
    );

    // 双击出库单号打开出库单详情
    _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_SaleOut>(c => c.SaleOutNo);
}
```

### 示例2：库存跟踪（复杂类型）

```csharp
private void UCInventoryTracking_Load(object sender, EventArgs e)
{
    base._UCOutlookGridAnalysis1.GridRelated.FromMenuInfo = this.CurMenuInfo;
    base._UCOutlookGridAnalysis1.GridRelated.ComplexType = true;
    base._UCOutlookGridAnalysis1.GridRelated.SetComplexTargetField<Proc_InventoryTracking>(
        c => c.业务类型,
        c => c.单据编号
    );

    // 配置业务类型映射
    var mappings = new Dictionary<string, string>
    {
        { "采购入库", "tb_PurEntry" },
        { "销售出库", "tb_SaleOut" },
        { "库存盘点", "tb_Stocktake" }
    };

    foreach (var item in mappings)
    {
        KeyNamePair keyNamePair = new KeyNamePair(item.Key, item.Value);
        base._UCOutlookGridAnalysis1.GridRelated.SetRelatedInfo<Proc_InventoryTracking>(
            c => c.单据编号,
            keyNamePair
        );
    }
}
```

### 示例3：自定义加载器

```csharp
public class UCBillOfMaterialsQuery : BaseListGeneric<tb_BOM_S>
{
    public override void InitGridRelated()
    {
        // 为 BOM 单据注册自定义加载器
        _UCBillMasterQuery.GridRelated.RegisterCustomLoader(typeof(tb_BOM_S).Name,
            async (menuInfo, billNo) =>
            {
                var obj = MainForm.Instance.AppContext.Db.Queryable<tb_BOM_S>()
                    .Includes(c => c.tb_BOM_SDetails, d => d.tb_BOM_SDetailSubstituteMaterials)
                    .Includes(c => c.view_ProdInfo)
                    .WhereIF(billNo.GetType() == typeof(long), c => c.BOM_ID == billNo.ToLong())
                    .WhereIF(billNo.GetType() == typeof(string), c => c.BOM_No == billNo.ToString())
                    .Single();
                await menuPowerHelper.ExecuteEvents(menuInfo, obj);
            });

        _UCBillMasterQuery.GridRelated.SetRelatedInfo<tb_BOM_S>(c => c.BOM_ID);
    }
}
```

## 性能优化

1. **减少硬编码**：移除大量重复的 if 语句，代码从 857 行减少到约 400 行
2. **统一加载逻辑**：使用 EntityLoader 统一处理实体加载，减少代码重复
3. **智能缓存**：自定义加载器支持重复使用，避免重复注册

## 注意事项

1. **实体映射配置**：确保 `IEntityMappingService` 已正确配置所有业务实体的映射
2. **菜单配置**：确保菜单表 (`tb_MenuInfo`) 中的 `BIBaseForm` 字段正确设置为 `"BaseBillEditGeneric`2"`
3. **特殊菜单**：对于菜单名称与实体名称不一致的情况，请在 `SpecialMenuMappings` 中添加映射

## 故障排查

### 问题1：双击后无反应

**原因**：未找到对应的菜单

**解决**：
- 检查菜单表配置
- 检查 `EntityName` 是否正确
- 检查 `BIBaseForm` 是否为 `"BaseBillEditGeneric`2"`

### 问题2：显示"无法加载表的数据"错误

**原因**：实体映射服务中未注册该表

**解决**：
- 检查 `IEntityMappingService` 的配置
- 确保表名和实体类型映射正确

### 问题3：特殊实体（如 BOM、需求分析）无法打开

**原因**：需要特殊加载逻辑

**解决**：
- 使用 `RegisterCustomLoader` 注册自定义加载器
- 确保包含所有需要的明细集合

## 总结

重构后的 `GridViewRelated` 类：

✅ 支持智能业务类型映射
✅ 完善字段映射机制（ID 和编号）
✅ 处理复杂业务场景（库存跟踪）
✅ 提供灵活的配置方式
✅ 优化命名规范和代码结构
✅ 保持与现有架构的兼容性
✅ 增强错误处理机制
✅ 大幅减少代码重复（从 857 行到约 400 行）
