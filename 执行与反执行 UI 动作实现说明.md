# 执行与反执行 UI 动作实现说明

## 📋 概述

在 `BaseBillEditGeneric.cs` 中实现了"执行"和"反执行"两个新的 UI 动作，遵循现有的操作处理模式，确保与其他操作（审核、反审、结案等）保持一致的用户体验。

## ✅ 实现位置

**文件**: `RUINORERP.UI\BaseForm\BaseBillEditGeneric.cs`  
**行号**: 3336-3450 (约)

## 🎯 核心实现

### 1. 执行动作（MenuItemEnums.执行）

#### 实现代码
```csharp
case MenuItemEnums.执行:
    // 声明变量在 try 块外部，以便在 catch 和 finally 中访问
    var btnExecute = FindToolStripButtonByName("toolStripBtnExecute");
    try
    {
        // 使用状态管理架构检查执行权限
        var canExecute = StateManager?.CanExecuteActionWithMessage(EditEntity, menuItem);
        if (canExecute == null || !canExecute.Value.CanExecute)
        {
            var message = canExecute?.Message ?? "无法检查执行权限";
            MainForm.Instance.uclog.AddLog($"当前状态下无法执行单据：{message}");
            return;
        }

        var lockStatusExecute = await CheckLockStatusAndUpdateUI(EditEntity.PrimaryKeyID);
        if (!lockStatusExecute.CanPerformCriticalOperations)
        {
            return;
        }

        // 立即禁用执行按钮，防止重复点击
        if (btnExecute != null)
        {
            btnExecute.Enabled = false;
        }

        await LockBill();
        bool rsExecute = await ConfirmExecution();
        if (!rsExecute)
        {
            // 执行失败时恢复执行按钮状态
            if (btnExecute != null && EditEntity != null)
            {
                btnExecute.Enabled = StateManager.GetButtonState(EditEntity, "toolStripBtnExecute");
            }
        }
        else
        {
            // 执行成功后更新所有 UI 状态，让按钮根据新状态重新评估
            UpdateAllUIStates(EditEntity);
        }
    }
    catch (Exception ex)
    {
        MainForm.Instance.uclog.AddLog($"执行单据失败：{ex.Message}");
        // 执行异常时恢复执行按钮状态
        if (btnExecute != null && EditEntity != null)
        {
            btnExecute.Enabled = StateManager.GetButtonState(EditEntity, "toolStripBtnExecute");
        }
    }
    finally
    {

    }
    break;
```

#### 关键特性
- ✅ **权限检查**：使用 StateManager 检查执行权限
- ✅ **锁定检查**：确保单据可执行关键操作
- ✅ **防重复点击**：立即禁用按钮
- ✅ **单据锁定**：执行前锁定单据
- ✅ **状态恢复**：失败时恢复按钮状态
- ✅ **UI 更新**：成功后更新所有 UI 状态

### 2. 反执行动作（MenuItemEnums.反执行）

#### 实现代码
```csharp
case MenuItemEnums.反执行:
    // 声明变量在 try 块外部，以便在 catch 和 finally 中访问
    var btnReverseExecute = FindToolStripButtonByName("toolStripBtnReverseExecute");
    try
    {
        // 使用状态管理架构检查反执行权限
        var canReverseExecute = StateManager?.CanExecuteActionWithMessage(EditEntity, menuItem);
        if (canReverseExecute == null || !canReverseExecute.Value.CanExecute)
        {
            var message = canReverseExecute?.Message ?? "无法检查反执行权限";
            MainForm.Instance.uclog.AddLog($"当前状态下无法反执行单据：{message}");
            return;
        }

        var lockStatusReverseExecute = await CheckLockStatusAndUpdateUI(EditEntity.PrimaryKeyID);
        if (!lockStatusReverseExecute.CanPerformCriticalOperations)
        {
            return;
        }

        // 立即禁用反执行按钮，防止重复点击
        if (btnReverseExecute != null)
        {
            btnReverseExecute.Enabled = false;
        }

        await LockBill();
        bool rsReverseExecute = await AntiConfirmExecution();
        if (!rsReverseExecute)
        {
            // 反执行失败时恢复反执行按钮状态
            if (btnReverseExecute != null && EditEntity != null)
            {
                btnReverseExecute.Enabled = StateManager.GetButtonState(EditEntity, "toolStripBtnReverseExecute");
            }
        }
        else
        {
            // 反执行成功后更新所有 UI 状态，让按钮根据新状态重新评估
            UpdateAllUIStates(EditEntity);
        }
    }
    catch (Exception ex)
    {
        MainForm.Instance.uclog.AddLog($"反执行单据失败：{ex.Message}");
        // 反执行异常时恢复反执行按钮状态
        if (btnReverseExecute != null && EditEntity != null)
        {
            btnReverseExecute.Enabled = StateManager.GetButtonState(EditEntity, "toolStripBtnReverseExecute");
        }
    }
    finally
    {

    }
    break;
```

#### 关键特性
- ✅ **权限检查**：使用 StateManager 检查反执行权限
- ✅ **锁定检查**：确保单据可执行关键操作
- ✅ **防重复点击**：立即禁用按钮
- ✅ **单据锁定**：反执行前锁定单据
- ✅ **状态恢复**：失败时恢复按钮状态
- ✅ **UI 更新**：成功后更新所有 UI 状态

## 🔄 操作流程对比

| 步骤 | 执行 | 反执行 |
|------|------|--------|
| 1 | 权限检查 | 权限检查 |
| 2 | 锁定状态检查 | 锁定状态检查 |
| 3 | 禁用按钮 | 禁用按钮 |
| 4 | 锁定单据 | 锁定单据 |
| 5 | 调用 `ConfirmExecution()` | 调用 `AntiConfirmExecution()` |
| 6 | 失败→恢复按钮<br>成功→更新 UI | 失败→恢复按钮<br>成功→更新 UI |

## 📊 与其他操作的对比

### 审核操作模式
```csharp
case MenuItemEnums.审核:
    var btnReview = FindToolStripButtonByName("toolStripbtnReview");
    try
    {
        // 权限检查
        var canReview = StateManager?.CanExecuteActionWithMessage(EditEntity, menuItem);
        
        // 锁定检查
        var lockStatusReview = await CheckLockStatusAndUpdateUI(...);
        
        // 禁用按钮
        btnReview.Enabled = false;
        
        // 锁定单据
        await LockBill();
        
        // 执行业务
        ReviewResult reviewResult = await Review();
        
        // 状态处理
        if (!reviewResult.Succeeded)
            btnReview.Enabled = StateManager.GetButtonState(...);
        else
            UpdateAllUIStates(EditEntity);
    }
    catch (Exception ex)
    {
        // 错误处理
    }
```

### 执行操作模式（完全一致）
```csharp
case MenuItemEnums.执行:
    var btnExecute = FindToolStripButtonByName("toolStripBtnExecute");
    try
    {
        // 权限检查 ✓
        // 锁定检查 ✓
        // 禁用按钮 ✓
        // 锁定单据 ✓
        // 执行业务 ✓
        // 状态处理 ✓
    }
    catch (Exception ex)
    {
        // 错误处理 ✓
    }
```

## 🔧 基类方法定义

### BaseBillEdit.cs 中的虚方法

```csharp
/// <summary>
/// 确认执行 - 执行业务单据的具体业务操作（如库存变动、财务记账等）
/// 该方法作为业务单据执行具体操作的统一入口，子类可重写实现具体业务逻辑
/// </summary>
/// <returns>执行结果，true 表示执行成功，false 表示执行失败</returns>
protected virtual Task<bool> ConfirmExecution()
{
    return Task.FromResult(false);
}

/// <summary>
/// 反执行 - 回滚执行时的业务操作
/// </summary>
/// <returns>反执行结果，true 表示成功，false 表示失败</returns>
protected virtual Task<bool> AntiConfirmExecution()
{
    return Task.FromResult(false);
}
```

## 📝 子类实现示例

### 借出单实现（tb_ProdBorrowingControllerPartial.cs）

```csharp
public async override Task<ReturnResults<T>> ConfirmExecutionAsync(T ObjectEntity)
{
    // 1. 验证单据合法性
    if (entity == null) return Error("单据实体为空");
    
    // 2. 检查状态（必须是完结状态）
    if (entity.DataStatus != (int)DataStatus.完结)
        return Error("只有已执行的单据才能反执行");
    
    // 3. 预加载库存（死锁优化）
    var invDict = PreloadInventory(entity.tb_ProdBorrowingDetails);
    
    // 4. 开启事务
    _unitOfWorkManage.BeginTran();
    
    // 5. 反向操作库存
    foreach (var detail in entity.tb_ProdBorrowingDetails)
    {
        inv.Quantity += detail.Qty;  // 增加库存
        CreateReverseTransaction(detail, inv);
    }
    
    // 6. 批量更新库存
    await BatchUpdateInventory(invUpdateList);
    
    // 7. 记录库存流水
    await BatchRecordTransactions(transactionList);
    
    // 8. 回退状态到确认
    entity.DataStatus = (int)DataStatus.确认;
    
    // 9. 提交事务
    _unitOfWorkManage.CommitTran();
    
    return Success(entity);
}
```

## 🎨 UI 按钮命名规范

| 操作 | 按钮名称 | 查找代码 |
|------|---------|---------|
| 执行 | toolStripBtnExecute | `FindToolStripButtonByName("toolStripBtnExecute")` |
| 反执行 | toolStripBtnReverseExecute | `FindToolStripButtonByName("toolStripBtnReverseExecute")` |

## 🔐 安全控制

### 1. 权限控制
- 使用 `StateManager.CanExecuteActionWithMessage()` 检查权限
- 支持灵活模式和严格模式的不同权限配置

### 2. 锁定控制
- 使用 `CheckLockStatusAndUpdateUI()` 检查锁定状态
- 只有未被锁定或被当前用户锁定的单据才能操作
- 使用 `LockBill()` 锁定单据防止并发操作

### 3. 防重复点击
- 立即禁用按钮防止用户重复点击
- 在 finally 块或 catch 块中恢复按钮状态

## 📈 错误处理

### 错误日志
```csharp
MainForm.Instance.uclog.AddLog($"执行单据失败：{ex.Message}");
```

### 按钮状态恢复
```csharp
if (btnExecute != null && EditEntity != null)
{
    btnExecute.Enabled = StateManager.GetButtonState(EditEntity, "toolStripBtnExecute");
}
```

### UI 状态更新
```csharp
UpdateAllUIStates(EditEntity);
```

## ✅ 测试要点

### 功能测试
1. ✅ 权限不足时显示提示
2. ✅ 单据被锁定时阻止操作
3. ✅ 执行成功后状态正确更新
4. ✅ 执行失败后按钮状态恢复
5. ✅ 防重复点击有效

### 集成测试
1. ✅ 与状态管理系统整合
2. ✅ 与锁定系统整合
3. ✅ 与业务控制器整合
4. ✅ UI 按钮状态正确响应

## 📚 相关文档

- [执行按钮状态管理优化说明.md](file://e:\CodeRepository\SynologyDrive\RUINORERP\执行按钮状态管理优化说明.md)
- [借出单反审核与反执行拆分实现说明.md](file://e:\CodeRepository\SynologyDrive\RUINORERP\借出单反审核与反执行拆分实现说明.md)
- [反审核与反执行拆分_快速参考.md](file://e:\CodeRepository\SynologyDrive\RUINORERP\反审核与反执行拆分_快速参考.md)
- [反审核与反执行拆分_对状态管理体系的影响分析.md](file://e:\CodeRepository\SynologyDrive\RUINORERP\反审核与反执行拆分_对状态管理体系的影响分析.md)

## 🎯 总结

通过在 `BaseBillEditGeneric.cs` 中实现"执行"和"反执行"动作，我们：

1. ✅ **遵循现有模式**：完全按照审核、反审、结案的操作模式实现
2. ✅ **统一架构**：使用 StateManager 进行权限检查和状态管理
3. ✅ **安全保障**：包含锁定检查、防重复点击、异常处理
4. ✅ **用户体验**：明确的错误提示、自动的 UI 更新
5. ✅ **易于扩展**：子类只需重写 `ConfirmExecution()` 和 `AntiConfirmExecution()` 虚方法

这套实现为所有需要"执行"功能的业务单据提供了统一的 UI 层框架！✨
