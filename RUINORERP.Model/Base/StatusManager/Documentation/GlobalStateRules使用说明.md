# 全局状态规则管理器使用说明

## 概述

`GlobalStateRulesManager` 是一个单例模式的规则管理器，用于统一管理系统中的所有状态转换规则和UI控件规则。它将原来分散在多个文件中的规则集中管理，确保规则在应用启动时只初始化一次，并提供全局访问接口。

## 主要特点

1. **单例模式**：确保全局唯一实例，避免重复初始化
2. **规则集中管理**：将状态转换规则和UI控件规则集中管理
3. **向后兼容**：保留原有接口，现有代码无需修改
4. **线程安全**：使用Lazy<T>实现线程安全的延迟初始化
5. **易于扩展**：提供清晰的接口，方便添加新的规则类型

## 初始化方法

### 方法一：在应用启动时手动初始化

```csharp
// 在应用启动时调用一次
GlobalRulesInitializer.InitializeGlobalRules();
```

### 方法二：使用Microsoft DI容器

```csharp
// 在Startup.cs或Program.cs中
services.AddGlobalStateRules();
// 或者整合状态管理服务
services.AddStateManagerWithGlobalRules();
```

### 方法三：使用Autofac容器

```csharp
// 在模块注册中
builder.AddGlobalStateRules();
// 或者整合状态管理服务
builder.AddStateManagerWithGlobalRules();
```

## 使用方法

### 获取全局规则管理器实例

```csharp
var rulesManager = GlobalStateRulesManager.Instance;
```

### 状态转换规则操作

```csharp
// 获取状态转换规则
var transitionRules = rulesManager.StateTransitionRules;

// 验证状态转换是否合法
bool canTransition = rulesManager.IsTransitionAllowed(DataStatus.草稿, DataStatus.新建);

// 获取可转换的状态列表
var availableTransitions = rulesManager.GetAvailableTransitions(DataStatus.草稿);

// 添加自定义状态转换规则
rulesManager.AddTransitionRule(DataStatus.草稿, DataStatus.新建, DataStatus.作废);
```

### UI按钮规则操作

```csharp
// 获取UI按钮规则
var buttonRules = rulesManager.UIButtonRules;

// 获取特定状态的按钮规则
// 注意：返回值从Dictionary<string, (bool Enabled, bool Visible)>变更为Dictionary<string, bool>
// 状态管理系统现在只控制按钮的可用性(Enabled)，Visible由权限系统管理
var statusButtonRules = rulesManager.GetButtonRules(DataStatus.草稿);

// 添加自定义按钮规则
// 注意：参数从(buttonName, enabled, visible)变更为(buttonName, enabled)
rulesManager.AddButtonRule(DataStatus.草稿, "btnCustom", true);
```

### 操作权限规则操作

```csharp
// 获取操作权限规则
var permissionRules = rulesManager.ActionPermissionRules;

// 获取特定状态的操作权限
var statusPermissions = rulesManager.GetActionPermissionRules(DataStatus.草稿);

// 添加自定义操作权限规则
var customActions = new List<MenuItemEnums> { MenuItemEnums.保存, MenuItemEnums.提交 };
rulesManager.AddActionPermissionRule(DataStatus.草稿, customActions);
```

## 向后兼容性

原有的 `StateTransitionRules` 和 `UIControlRules` 类仍然可以正常使用，它们内部会自动使用 `GlobalStateRulesManager`：

```csharp
// 以下代码仍然有效，但会显示过时警告
var transitionRules = StateTransitionRules.Instance;
var buttonRules = UIControlRules.UIButtonRules;
var permissionRules = UIControlRules.ActionPermissionRules;

// 方法调用仍然有效，但建议使用新的GlobalStateRulesManager
StateTransitionRules.AddTransitionRule(transitionRules, DataStatus.草稿, DataStatus.新建);
UIControlRules.AddButtonRule(DataStatus.草稿, "btnSave", true);
UIControlRules.AddActionPermissionRule(DataStatus.草稿, new List<MenuItemEnums> { MenuItemEnums.保存 });
```

## 最佳实践

1. **应用启动时初始化**：确保在应用启动时调用初始化方法，只调用一次
2. **使用全局实例**：通过 `GlobalStateRulesManager.Instance` 获取全局唯一实例
3. **优先使用新接口**：新代码优先使用 `GlobalStateRulesManager` 的方法，而不是过时的 `StateTransitionRules` 和 `UIControlRules`
4. **规则扩展**：通过 `AddTransitionRule`、`AddButtonRule` 和 `AddActionPermissionRule` 方法动态添加规则
5. **线程安全**：所有公共方法都是线程安全的，可以在多线程环境中使用

## 架构优势

1. **统一管理**：将分散的规则集中管理，便于维护和扩展
2. **单例模式**：确保全局唯一实例，避免重复初始化和内存浪费
3. **规则一致性**：所有规则在同一地方定义，确保一致性
4. **易于测试**：提供重置方法，便于单元测试
5. **性能优化**：规则只初始化一次，提高运行时性能

## 迁移指南

从旧版本迁移到新版本：

1. **无代码更改**：现有代码无需修改，可以直接运行
2. **逐步迁移**：建议新代码使用 `GlobalStateRulesManager`，旧代码可以逐步迁移
3. **初始化更新**：在应用启动代码中添加规则初始化调用
4. **移除过时警告**：将所有对 `StateTransitionRules` 和 `UIControlRules` 的引用替换为 `GlobalStateRulesManager`

## 示例代码

```csharp
// 应用启动时初始化
public void ConfigureServices(IServiceCollection services)
{
    // 添加全局状态规则服务
    services.AddStateManagerWithGlobalRules();
    
    // 其他服务注册...
}

// 在业务代码中使用
public class BusinessService
{
    private readonly GlobalStateRulesManager _rulesManager;
    
    public BusinessService()
    {
        _rulesManager = GlobalStateRulesManager.Instance;
    }
    
    public void ProcessData()
    {
        // 检查状态转换是否允许
        if (_rulesManager.IsTransitionAllowed(DataStatus.草稿, DataStatus.新建))
        {
            // 执行状态转换
            // ...
        }
        
        // 获取按钮状态
        var buttonRules = _rulesManager.GetButtonRules(DataStatus.草稿);
        foreach (var rule in buttonRules)
        {
            // 更新按钮状态
            // ...
        }
    }
}
```