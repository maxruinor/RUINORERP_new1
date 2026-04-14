# UCProdBundle 状态驱动按钮控制完善说明

## 问题描述

套装组合(UCProdBundle)的UI按钮状态控制不完善,没有像销售订单(UCSaleOrder)一样根据单据状态动态控制按钮的可用性。

## 对比分析

### 销售订单(UCSaleOrder)的实现特点

1. ✅ **排除不需要的菜单**: `AddExcludeMenuList()` 排除结案/反结案
2. ✅ **状态绑定**: DataStatus 和 ApprovalStatus 绑定到界面标签
3. ✅ **PropertyChanged 监听**: 监听状态字段变化,自动更新按钮
4. ✅ **打印状态控制**: 根据数据状态和审核状态控制打印按钮
5. ✅ **业务逻辑复杂**: 涉及付款、外币、佣金等多个维度

### 套装组合(UCProdBundle)的需求

1. ✅ **简单业务**: 只是主表字段状态的变化
2. ✅ **基础资料**: 不需要结案/反结案功能
3. ✅ **标准流程**: 草稿 → 审核 → 确认
4. ✅ **按钮控制**: 根据状态控制审核、反审核、修改、删除、打印

## 修复方案

### 1. 添加排除菜单列表

**位置**: `UCProdBundle.cs` - 类定义后

```csharp
/// <summary>
/// 添加排除菜单列表 - 套装组合不需要结案功能
/// </summary>
public override void AddExcludeMenuList()
{
    // 套装组合是基础资料，不需要结案/反结案功能
    base.AddExcludeMenuList(MenuItemEnums.结案);
    base.AddExcludeMenuList(MenuItemEnums.反结案);
}
```

**效果**: 
- ✅ 界面上不显示"结案"和"反结案"按钮
- ✅ 与销售订单保持一致的设计模式

---

### 2. 完善 UpdateButtonStates 方法

**位置**: `UCProdBundle.cs` - UpdateButtonStates 方法

**修复前**:
```csharp
private void UpdateButtonStates()
{
    if (EditEntity == null) return;
    
    var dataStatus = (DataStatus)EditEntity.DataStatus;
    var approvalStatus = (ApprovalStatus)(EditEntity.ApprovalStatus ?? 0);
    
    // 打印按钮：非草稿状态且已审核通过才能打印
    bool canPrint = dataStatus != DataStatus.草稿 && 
                   approvalStatus == ApprovalStatus.审核通过;
    toolStripbtnPrint.Enabled = canPrint;
    
    // ❌ 缺少其他按钮的控制
}
```

**修复后**:
```csharp
/// <summary>
/// 根据单据状态更新按钮可用性
/// 参考销售订单的状态驱动按钮控制逻辑
/// </summary>
private void UpdateButtonStates()
{
    if (EditEntity == null) return;
    
    var dataStatus = (DataStatus)EditEntity.DataStatus;
    var approvalStatus = (ApprovalStatus)(EditEntity.ApprovalStatus ?? 0);
    
    // ✅ 打印按钮：非草稿/新建状态且已审核通过才能打印
    bool canPrint = dataStatus != DataStatus.草稿 && 
                   dataStatus != DataStatus.新建 &&
                   approvalStatus == ApprovalStatus.审核通过;
    toolStripbtnPrint.Enabled = canPrint;
    
    // ✅ 审核按钮：只有草稿或新建状态且未审核时才能审核
    bool canApprove = (dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建) &&
                     approvalStatus == ApprovalStatus.未审核;
    if (toolStripbtnApproval != null)
    {
        toolStripbtnApproval.Enabled = canApprove;
    }
    
    // ✅ 反审核按钮：只有审核通过状态才能反审核
    bool canAntiApprove = approvalStatus == ApprovalStatus.审核通过 &&
                         EditEntity.ApprovalResults.HasValue;
    if (toolStripbtnAntiApproval != null)
    {
        toolStripbtnAntiApproval.Enabled = canAntiApprove;
    }
    
    // ✅ 修改按钮：草稿或新建状态可以修改
    bool canEdit = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
    if (toolStripbtnEdit != null)
    {
        toolStripbtnEdit.Enabled = canEdit;
    }
    
    // ✅ 删除按钮：草稿或新建状态可以删除
    bool canDelete = dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
    if (toolStripbtnDelete != null)
    {
        toolStripbtnDelete.Enabled = canDelete;
    }
}
```

**按钮状态规则**:

| 按钮 | 草稿 | 新建 | 确认(已审核) | 完结(已结案) |
|------|------|------|-------------|-------------|
| 审核 | ✅ | ✅ | ❌ | ❌ |
| 反审核 | ❌ | ❌ | ✅ | ❌ |
| 修改 | ✅ | ✅ | ❌ | ❌ |
| 删除 | ✅ | ✅ | ❌ | ❌ |
| 打印 | ❌ | ❌ | ✅ | ✅ |

---

### 3. 增强 PropertyChanged 事件监听

**位置**: `UCProdBundle.cs` - BindData 方法中的 PropertyChanged 事件

**修复前**:
```csharp
entity.PropertyChanged += (sender, s2) =>
{
    // 更新打印状态显示
    UpdatePrintStatusDisplay();
    
    // 如果状态发生变化，刷新按钮状态
    if (s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.DataStatus) ||
        s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.ApprovalStatus))
    {
        UpdateButtonStates();
    }
};
```

**修复后**:
```csharp
entity.PropertyChanged += (sender, s2) =>
{
    if (EditEntity == null) return;
    
    // 更新打印状态显示
    UpdatePrintStatusDisplay();
    
    // ✅ 监听更多状态字段
    if (s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.DataStatus) ||
        s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.ApprovalStatus) ||
        s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.ApprovalResults) ||
        s2.PropertyName == entity.GetPropertyName<tb_ProdBundle>(c => c.PrintStatus))
    {
        UpdateButtonStates();
    }
};
```

**新增监听的字段**:
- `ApprovalResults`: 审核结果(同意/拒绝)
- `PrintStatus`: 打印次数

---

### 4. 在关键操作后刷新按钮状态

#### 4.1 保存成功后

```csharp
if (SaveResult.Succeeded)
{
    MainForm.Instance.PrintInfoLog($"保存成功,{EditEntity.BundleName}。");
    
    // ✅ 保存成功后刷新按钮状态
    UpdateButtonStates();
}
```

#### 4.2 审核成功后

```csharp
if (rs.Succeeded)
{
    EditEntity.AcceptChanges();
    await MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_ProdBundle>("审核", EditEntity, $"审核意见:{frm.OpinionText}");
    Refreshs();
    MainForm.Instance.PrintInfoLog("审核成功！");
    
    // ✅ 审核成功后刷新按钮状态
    UpdateButtonStates();
}
```

#### 4.3 反审核成功后

```csharp
if (rs.Succeeded)
{
    await MainForm.Instance.AuditLogHelper.CreateAuditLog<tb_ProdBundle>("反审核", EditEntity, $"反审核意见:{frm.OpinionText}");
    Refreshs();
    MainForm.Instance.PrintInfoLog("反审核成功！");
    
    // ✅ 反审核成功后刷新按钮状态
    UpdateButtonStates();
}
```

#### 4.4 结案/反结案成功后

虽然套装组合排除了结案功能,但为了代码完整性也添加了刷新:

```csharp
if (rs.Succeeded)
{
    // ... 审计日志等
    
    // ✅ 结案成功后刷新按钮状态
    UpdateButtonStates();
}
```

---

## 状态流转图

```
┌─────────┐     保存      ┌─────────┐     审核      ┌──────────┐
│  新建   │ ──────────> │  草稿   │ ──────────> │  确认    │
│ (New)   │             │ (Draft) │             │(Confirmed)│
└─────────┘             └─────────┘             └──────────┘
    ↑                       │                        │
    │                  反审核 │                        │
    │                       ↓                        │
    │                 ┌─────────┐                    │
    └─────────────────│  新建   │                    │
                      │ (New)   │                    │
                      └─────────┘                    │
                                                   │
                                              打印(可选)
                                                   │
                                                   ↓
                                              ┌──────────┐
                                              │  确认    │
                                              │(Confirmed)│
                                              └──────────┘
```

**注意**: 套装组合作为基础资料,通常不需要"完结"状态,保持在"确认"状态即可。

---

## 测试验证

### 测试场景 1: 新增套装组合

1. 点击"新增"按钮
2. 输入套装名称、描述等信息
3. 添加明细产品
4. 点击"保存"

**预期结果**:
- ✅ 保存前: 审核按钮可用,打印/反审核按钮禁用
- ✅ 保存后: 状态变为"草稿",按钮状态不变
- ✅ 界面显示: lblDataStatus = "草稿", lblReview = "未审核"

---

### 测试场景 2: 审核套装组合

1. 打开一个草稿状态的套装组合
2. 点击"审核"按钮
3. 输入审核意见
4. 点击"确定"

**预期结果**:
- ✅ 审核前: 审核按钮可用,反审核/打印按钮禁用
- ✅ 审核后: 状态变为"确认",审核按钮禁用,反审核/打印按钮可用
- ✅ 界面显示: lblDataStatus = "确认", lblReview = "审核通过"

---

### 测试场景 3: 反审核套装组合

1. 打开一个已审核的套装组合
2. 点击"反审核"按钮
3. 输入反审核意见
4. 点击"确定"

**预期结果**:
- ✅ 反审核前: 反审核/打印按钮可用,审核按钮禁用
- ✅ 反审核后: 状态变为"新建",审核按钮可用,反审核/打印按钮禁用
- ✅ 界面显示: lblDataStatus = "新建", lblReview = "未审核"

---

### 测试场景 4: 打印套装组合

1. 打开一个已审核的套装组合
2. 点击"打印"按钮

**预期结果**:
- ✅ 打印按钮可用
- ✅ 打印后 PrintStatus + 1
- ✅ lblPrintStatus 显示 "打印X次"

---

### 测试场景 5: 修改已审核的套装组合

1. 打开一个已审核的套装组合
2. 尝试点击"修改"按钮

**预期结果**:
- ✅ 修改按钮禁用(因为已审核)
- ✅ 必须先反审核才能修改

---

## 与销售订单的对比

| 特性 | 销售订单 | 套装组合 |
|------|---------|---------|
| 业务复杂度 | ⭐⭐⭐⭐⭐ 高 | ⭐ 低 |
| 状态流转 | 草稿→确认→完结 | 新建→草稿→确认 |
| 结案功能 | ✅ 需要 | ❌ 不需要 |
| 付款管理 | ✅ 复杂 | ❌ 无 |
| 外币处理 | ✅ 支持 | ❌ 无 |
| 按钮控制 | ✅ 完整 | ✅ 完整(本次修复) |
| 状态驱动 | ✅ PropertyChanged | ✅ PropertyChanged |

**核心差异**:
- 销售订单: 复杂的业务流程,涉及财务、库存等多个模块
- 套装组合: 简单的基础资料管理,只关注状态变化

**共同点**:
- ✅ 都采用状态驱动的按钮控制
- ✅ 都监听 PropertyChanged 事件
- ✅ 都在关键操作后刷新按钮状态

---

## 相关文件

### UI 层
- ✅ `RUINORERP.UI\ProductEAV\UCProdBundle.cs` - 本次修复的主要文件
- 📖 `RUINORERP.UI\PSI\SAL\UCSaleOrder.cs` - 参考实现

### 业务层
- 📖 `RUINORERP.Business\tb_ProdBundleControllerPartial.cs` - 审核/反审核业务逻辑
- 📖 `RUINORERP.Business\tb_SaleOrderControllerPartial.cs` - 销售订单业务逻辑(参考)

### 基类
- 📖 `RUINORERP.UI\BaseForm\BaseBillEditGeneric.cs` - 通用表单基类

---

## 总结

### 修复内容

1. ✅ 添加 `AddExcludeMenuList()` 排除结案功能
2. ✅ 完善 `UpdateButtonStates()` 控制所有按钮状态
3. ✅ 增强 `PropertyChanged` 监听更多状态字段
4. ✅ 在保存、审核、反审核后刷新按钮状态

### 设计原则

1. **状态驱动**: 按钮可用性完全由数据状态决定
2. **自动更新**: 状态变化时自动刷新,无需手动干预
3. **防御性编程**: 所有按钮访问前检查 null
4. **参考最佳实践**: 借鉴销售订单的成熟实现

### 用户体验提升

- ✅ 按钮状态清晰,用户知道当前能做什么操作
- ✅ 防止误操作,已审核的单据不能随意修改
- ✅ 界面反馈及时,操作后立即看到按钮状态变化
- ✅ 与销售订单保持一致的交互体验

---

## 后续优化建议

1. **添加操作提示**: 当按钮禁用时,鼠标悬停显示原因
2. **权限控制**: 结合用户权限进一步控制按钮可用性
3. **快捷键支持**: 为常用操作添加键盘快捷键
4. **批量操作**: 支持列表页批量审核/反审核

