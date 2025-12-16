# 统一状态管理器使用指南

## 概述

本文档介绍了RUINOR ERP系统中统一状态管理器的使用方法和最佳实践。统一状态管理器是系统中用于管理业务状态、状态转换规则和UI状态的核心组件。

## 架构设计

### 核心组件

1. **IUnifiedStateManager** - 统一状态管理器接口
2. **UnifiedStateManager** - 统一状态管理器实现类
3. **GlobalStateRulesManager** - 全局状态规则管理器

### 状态判断方法

统一状态管理器提供了以下状态判断方法：

- `IsFinalStatus<TStatus>(TStatus status)` - 判断指定状态是否为终态
- `CanSubmit<TStatus>(TStatus status)` - 判断指定状态是否可以提交
- `CanApprove<TStatus>(TStatus status)` - 判断指定状态是否可以审核
- `CanAntiApprove<TStatus>(TStatus status)` - 判断指定状态是否可以反审

### 实体级别的状态判断

统一状态管理器还提供了实体级别的状态判断方法：

- `IsFinalStatus<TEntity>(TEntity entity)` - 判断指定实体的业务状态是否为终态
- `CanSubmitEntity<TEntity>(TEntity entity)` - 判断指定实体是否可以提交
- `CanApproveEntity<TEntity>(TEntity entity)` - 判断指定实体是否可以审核
- `CanAntiApproveEntity<TEntity>(TEntity entity)` - 判断指定实体是否可以反审
- `CanModify<TEntity>(TEntity entity)` - 判断指定实体是否可以修改

## 使用示例

### 业务层使用

```csharp
// 在业务层使用统一状态管理器
public async override Task<ReturnResults<T>> ApprovalAsync(T ObjectEntity)
{
    // 获取统一状态管理器
    _stateManager = _appContext.GetRequiredService<IUnifiedStateManager>();
    
    // 检查是否可以审核
    if (!_stateManager.CanApproveEntity(entity))
    {
        rmrs.ErrorMsg = "当前状态不允许审核操作";
        rmrs.Succeeded = false;
        return rmrs;
    }
    
    // 使用统一状态管理器设置状态
    var statusResult = await _stateManager.SetBusinessStatusAsync(entity, DataStatus.确认, "审核通过");
    if (!statusResult.IsSuccess)
    {
        rmrs.ErrorMsg = statusResult.ErrorMessage;
        rmrs.Succeeded = false;
        return rmrs;
    }
    
    // 继续其他业务逻辑...
}
```

### UI层使用

```csharp
// 在UI层使用统一状态管理器
public bool CanApproveCurrentEntity()
{
    if (EditEntity == null) return false;
    
    // 获取统一状态管理器
    var stateManager = Startup.GetFromFac<IUnifiedStateManager>();
    
    // 检查是否可以审核
    return stateManager.CanApproveEntity(EditEntity);
}
```

### 状态判断

```csharp
// 判断状态是否为终态
bool isFinal = stateManager.IsFinalStatus<DataStatus>(DataStatus.完结);

// 判断状态是否可以提交
bool canSubmit = stateManager.CanSubmit<DataStatus>(DataStatus.草稿);

// 判断状态是否可以审核
bool canApprove = stateManager.CanApprove<DataStatus>(DataStatus.新建);

// 判断状态是否可以反审
bool canAntiApprove = stateManager.CanAntiApprove<DataStatus>(DataStatus.确认);
```

## 状态转换规则

状态转换规则在GlobalStateRulesManager中定义，包括：

1. **DataStatus转换规则** - 数据状态转换规则
2. **PaymentStatus转换规则** - 支付状态转换规则
3. **RefundStatus转换规则** - 退款状态转换规则
4. **PrePaymentStatus转换规则** - 预付款状态转换规则
5. **ARAPStatus转换规则** - 应收应付状态转换规则
6. **StatementStatus转换规则** - 对账单状态转换规则

## UI控件状态管理

统一状态管理器还管理UI控件状态，根据实体状态自动启用或禁用相应的按钮：

```csharp
// 获取UI控件状态
var uiStates = stateManager.GetUIControlStates(entity);

// 获取特定按钮状态
bool submitButtonEnabled = stateManager.GetButtonState(entity, "提交");
bool approveButtonEnabled = stateManager.GetButtonState(entity, "审核");
```

## 最佳实践

1. **使用统一状态管理器** - 所有状态判断和状态设置操作都应通过统一状态管理器进行
2. **避免直接修改状态** - 不要直接修改实体的状态属性，应使用SetBusinessStatusAsync方法
3. **使用实体级别的方法** - 优先使用实体级别的状态判断方法，如CanApproveEntity而不是CanApprove
4. **处理状态转换结果** - 检查状态转换操作的返回结果，处理可能的错误
5. **使用依赖注入** - 通过依赖注入获取统一状态管理器实例

## 迁移指南

如果你正在使用旧的状态管理方法，请按照以下步骤迁移：

1. 将状态判断逻辑迁移到统一状态管理器
2. 使用SetBusinessStatusAsync方法替代直接设置状态
3. 使用实体级别的状态判断方法
4. 更新依赖注入配置

## 总结

统一状态管理器提供了集中、一致的状态管理机制，使状态判断和状态转换更加可靠和易于维护。通过使用统一状态管理器，可以确保整个系统中的状态管理逻辑保持一致，减少重复代码，提高系统的可维护性。