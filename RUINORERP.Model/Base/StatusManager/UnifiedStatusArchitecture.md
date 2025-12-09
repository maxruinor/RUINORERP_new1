/**
 * 文件: UnifiedStatusArchitecture.md
 * 说明: 统一状态管理架构设计方案
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本历史:
 * V1.0 - 初始版本，整合V3和V4架构优点
 */

# 统一状态管理架构设计方案

## 1. 架构概述

基于对现有V3和V4状态管理系统的分析，设计一套统一的状态管理架构，整合两者的优点，解决当前存在的重复代码和UI状态更新分散问题。

### 1.1 设计目标

1. **统一状态管理**：整合V3和V4架构，提供统一的状态管理接口
2. **消除重复代码**：合并重复的状态转换规则和验证逻辑
3. **分离UI与业务逻辑**：将UI状态更新逻辑从业务逻辑中分离
4. **提高可维护性**：简化架构，减少接口数量，提高代码可维护性
5. **保持向后兼容**：确保现有代码可以平滑迁移到新架构

### 1.2 核心原则

1. **单一职责原则**：每个组件只负责一个明确的功能
2. **依赖倒置原则**：高层模块不依赖低层模块，都依赖抽象
3. **开闭原则**：对扩展开放，对修改关闭
4. **接口隔离原则**：客户端不应依赖它不需要的接口
5. **最少知识原则**：减少组件之间的耦合度

## 2. 架构设计

### 2.1 整体架构图

```
┌─────────────────────────────────────────────────────────────┐
│                    UI层 (WinForms)                          │
├─────────────────────────────────────────────────────────────┤
│  BaseBillEditGeneric  │  其他UI组件                          │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   UI状态适配器层                             │
├─────────────────────────────────────────────────────────────┤
│  IUIStateController     │  UIStateController               │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   统一状态管理层                              │
├─────────────────────────────────────────────────────────────┤
│  IUnifiedStateManager  │  UnifiedStateManager                │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   状态规则配置层                              │
├─────────────────────────────────────────────────────────────┤
│  IStatusRuleConfig     │  StatusRuleConfiguration            │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   状态转换引擎层                              │
├─────────────────────────────────────────────────────────────┤
│  IStatusTransitionEngine │  StatusTransitionEngine          │
└─────────────────────────────────────────────────────────────┘
```

### 2.2 核心组件设计

#### 2.2.1 统一状态管理器 (IUnifiedStateManager)

**职责**：
- 提供统一的状态管理接口
- 管理实体的数据状态、业务状态和操作状态
- 协调状态转换引擎和规则配置

**接口设计**：
```csharp
public interface IUnifiedStateManager
{
    // 状态获取
    DataStatus GetDataStatus(BaseEntity entity);
    T GetBusinessStatus<T>(BaseEntity entity) where T : struct, Enum;
    ActionStatus GetActionStatus(BaseEntity entity);
    
    // 状态设置
    Task<bool> SetDataStatusAsync(BaseEntity entity, DataStatus status, string reason = null);
    Task<bool> SetBusinessStatusAsync<T>(BaseEntity entity, T status, string reason = null) where T : struct, Enum;
    Task<bool> SetActionStatusAsync(BaseEntity entity, ActionStatus status, string reason = null);
    
    // 状态转换验证
    Task<StateTransitionResult> ValidateTransitionAsync<T>(BaseEntity entity, T fromStatus, T toStatus) where T : struct, Enum;
    
    // 状态转换执行
    Task<StateTransitionResult> ExecuteTransitionAsync<T>(BaseEntity entity, T fromStatus, T toStatus, string reason = null) where T : struct, Enum;
    
    // 获取可用转换
    IEnumerable<T> GetAvailableTransitions<T>(BaseEntity entity) where T : struct, Enum;
    
    // 状态变更事件
    event EventHandler<StateTransitionEventArgs> StatusChanged;
}
```

#### 2.2.2 UI状态控制器 (IUIStateController)

**职责**：
- 管理UI控件状态（启用/禁用、可见/隐藏）
- 响应状态变更事件，更新UI控件状态
- 提供UI状态规则配置

**接口设计**：
```csharp
public interface IUIStateController
{
    // UI状态更新
    void UpdateUIState(BaseEntity entity, Control container);
    void UpdateControlState(string controlName, bool enabled, bool visible = true);
    
    // UI状态规则注册
    void RegisterUIRule<T>(T status, string controlName, bool enabled, bool visible = true) where T : Enum;
    
    // 批量UI状态更新
    void UpdateUIStateBatch(BaseEntity entity, Dictionary<string, (bool Enabled, bool Visible)> controlStates);
    
    // 状态变更事件处理
    void OnStatusChanged(object sender, StateTransitionEventArgs e);
}
```

#### 2.2.3 状态规则配置 (IStatusRuleConfig)

**职责**：
- 管理状态转换规则
- 提供规则注册和查询功能
- 支持动态规则配置

**接口设计**：
```csharp
public interface IStatusRuleConfig
{
    // 状态转换规则
    void RegisterTransitionRule<T>(T fromStatus, T toStatus, Func<BaseEntity, bool> validator = null) where T : Enum;
    bool IsTransitionAllowed<T>(T fromStatus, T toStatus, BaseEntity entity = null) where T : Enum;
    IEnumerable<T> GetAvailableTransitions<T>(T currentStatus, BaseEntity entity = null) where T : Enum;
    
    // UI状态规则
    void RegisterUIRule<T>(T status, string controlName, bool enabled, bool visible = true) where T : Enum;
    Dictionary<string, (bool Enabled, bool Visible)> GetUIRules<T>(T status) where T : Enum;
    
    // 业务规则
    void RegisterBusinessRule<T>(string ruleName, Func<T, BaseEntity, bool> validator) where T : Enum;
    bool ValidateBusinessRule<T>(string ruleName, T status, BaseEntity entity) where T : Enum;
}
```

#### 2.2.4 状态转换引擎 (IStatusTransitionEngine)

**职责**：
- 执行状态转换
- 验证状态转换合法性
- 提供转换上下文管理

**接口设计**：
```csharp
public interface IStatusTransitionEngine
{
    // 状态转换执行
    Task<StateTransitionResult> ExecuteTransitionAsync<T>(T fromStatus, T toStatus, StatusTransitionContext context) where T : struct, Enum;
    
    // 状态转换验证
    Task<StateTransitionResult> ValidateTransitionAsync<T>(T fromStatus, T toStatus, StatusTransitionContext context) where T : struct, Enum;
    
    // 转换上下文管理
    StatusTransitionContext CreateContext(BaseEntity entity, Type statusType);
}
```

## 3. 实现策略

### 3.1 迁移计划

#### 阶段1：核心架构实现
1. 实现IUnifiedStateManager接口和UnifiedStateManager类
2. 实现IUIStateController接口和UIStateController类
3. 实现IStatusRuleConfig接口和StatusRuleConfig类
4. 实现IStatusTransitionEngine接口和StatusTransitionEngine类

#### 阶段2：规则整合
1. 将FMPaymentStatusHelper中的状态转换规则迁移到StatusRuleConfig
2. 将StateTransitionRules中的规则迁移到StatusRuleConfig
3. 统一状态转换验证逻辑

#### 阶段3：UI层改造
1. 修改BaseBillEditGeneric，使用IUIStateController管理UI状态
2. 将UI状态更新逻辑从业务逻辑中分离
3. 实现状态变更事件的响应机制

#### 阶段4：旧代码迁移
1. 逐步替换V3和V4架构中的旧代码
2. 更新依赖注入配置
3. 提供迁移指南和工具

### 3.2 兼容性保证

1. **适配器模式**：为旧接口提供适配器，确保现有代码可以继续工作
2. **渐进式迁移**：支持新旧架构并存，逐步迁移
3. **版本标记**：使用Obsolete特性标记过时的接口和方法
4. **迁移工具**：提供自动化迁移工具，减少手动工作量

## 4. 具体实现

### 4.1 统一状态管理器实现

```csharp
public class UnifiedStateManager : IUnifiedStateManager
{
    private readonly IStatusTransitionEngine _transitionEngine;
    private readonly IStatusRuleConfig _ruleConfig;
    private readonly ILogger<UnifiedStateManager> _logger;

    public event EventHandler<StateTransitionEventArgs> StatusChanged;

    public UnifiedStateManager(
        IStatusTransitionEngine transitionEngine,
        IStatusRuleConfig ruleConfig,
        ILogger<UnifiedStateManager> logger)
    {
        _transitionEngine = transitionEngine;
        _ruleConfig = ruleConfig;
        _logger = logger;
    }

    // 实现接口方法...
}
```

### 4.2 UI状态控制器实现

```csharp
public class UIStateController : IUIStateController
{
    private readonly IStatusRuleConfig _ruleConfig;
    private readonly ILogger<UIStateController> _logger;
    private readonly Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> _uiRules;

    public UIStateController(IStatusRuleConfig ruleConfig, ILogger<UIStateController> logger)
    {
        _ruleConfig = ruleConfig;
        _logger = logger;
        _uiRules = new Dictionary<Type, Dictionary<object, Dictionary<string, (bool, bool)>>();
    }

    // 实现接口方法...
}
```

### 4.3 状态规则配置实现

```csharp
public class StatusRuleConfig : IStatusRuleConfig
{
    private readonly Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;
    private readonly Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> _uiRules;
    private readonly Dictionary<string, Delegate> _businessRules;
    private readonly ILogger<StatusRuleConfig> _logger;

    public StatusRuleConfig(ILogger<StatusRuleConfig> logger)
    {
        _transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
        _uiRules = new Dictionary<Type, Dictionary<object, Dictionary<string, (bool, bool)>>();
        _businessRules = new Dictionary<string, Delegate>();
        _logger = logger;
        
        InitializeDefaultRules();
    }

    // 实现接口方法...
}
```

## 5. 依赖注入配置

```csharp
// 在Startup.cs或Program.cs中配置依赖注入
services.AddSingleton<IStatusRuleConfig, StatusRuleConfig>();
services.AddSingleton<IStatusTransitionEngine, StatusTransitionEngine>();
services.AddSingleton<IUnifiedStateManager, UnifiedStateManager>();
services.AddSingleton<IUIStateController, UIStateController>();
```

## 6. 使用示例

### 6.1 状态转换示例

```csharp
// 获取统一状态管理器
var statusManager = serviceProvider.GetService<IUnifiedStateManager>();

// 验证状态转换
var validationResult = await statusManager.ValidateTransitionAsync(entity, DataStatus.草稿, DataStatus.确认);
if (validationResult.IsSuccess)
{
    // 执行状态转换
    var result = await statusManager.ExecuteTransitionAsync(entity, DataStatus.草稿, DataStatus.确认, "用户提交审核");
    if (result.IsSuccess)
    {
        // 状态转换成功
    }
}
```

### 6.2 UI状态更新示例

```csharp
// 获取UI状态控制器
var uiController = serviceProvider.GetService<IUIStateController>();

// 注册UI规则
uiController.RegisterUIRule(DataStatus.草稿, "btnSubmit", true);
uiController.RegisterUIRule(DataStatus.草稿, "btnEdit", true);
uiController.RegisterUIRule(DataStatus.确认, "btnSubmit", false);
uiController.RegisterUIRule(DataStatus.确认, "btnEdit", false);

// 更新UI状态
uiController.UpdateUIState(entity, this);
```

## 7. 迁移指南

### 7.1 从V3架构迁移

1. 替换IStatusTransitionEngine为IUnifiedStateManager
2. 将状态转换逻辑迁移到UnifiedStateManager
3. 使用IUIStateController管理UI状态

### 7.2 从V4架构迁移

1. 替换IStateRuleConfiguration为IStatusRuleConfig
2. 将规则配置迁移到StatusRuleConfig
3. 使用统一的状态管理接口

### 7.3 从FMPaymentStatusHelper迁移

1. 将状态转换规则迁移到StatusRuleConfig
2. 使用UnifiedStateManager进行状态管理
3. 删除FMPaymentStatusHelper中的重复逻辑

## 8. 测试策略

### 8.1 单元测试

1. 测试状态转换规则的正确性
2. 测试UI状态更新的准确性
3. 测试状态管理器的各种场景

### 8.2 集成测试

1. 测试整个状态管理流程
2. 测试UI与业务逻辑的交互
3. 测试新旧架构的兼容性

### 8.3 性能测试

1. 测试状态转换的性能
2. 测试UI状态更新的性能
3. 测试大量实体状态管理的性能

## 9. 总结

本设计方案通过整合V3和V4架构的优点，提供了一套统一的状态管理架构，解决了当前存在的重复代码和UI状态更新分散问题。新架构具有以下优势：

1. **统一性**：提供统一的状态管理接口，减少学习成本
2. **可维护性**：简化架构，减少接口数量，提高代码可维护性
3. **可扩展性**：支持动态规则配置，易于扩展新功能
4. **分离性**：将UI状态更新从业务逻辑中分离，提高代码质量
5. **兼容性**：提供适配器和迁移工具，确保平滑过渡

通过逐步实施本方案，可以显著提高ERP系统的状态管理能力，降低维护成本，提高开发效率。