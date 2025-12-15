# ActionStatus设计规范文档

## 1. 设计意图

ActionStatus枚举在RUINORERP系统中扮演着重要的角色，它的主要设计意图包括：

- **操作状态标识**：明确标识用户当前对实体执行的操作类型
- **UI行为控制**：为界面层提供操作状态信息，用于控制数据绑定、按钮状态、界面显示等
- **操作流跟踪**：在整个业务处理流程中跟踪和记录用户的操作意图
- **状态管理补充**：与DataStatus(数据生命周期状态)和BusinessStatus(业务流程状态)形成多维度的状态管理体系

## 2. ActionStatus枚举定义

```csharp
public enum ActionStatus
{
    无操作,   // 默认状态，表示用户未进行任何操作
    新增,     // 用户正在创建新记录
    修改,     // 用户正在修改现有记录
    删除,     // 用户正在删除记录
    加载,     // 系统正在加载数据
    复制,     // 用户正在复制现有记录创建新记录
}
```

## 3. 使用规范

### 3.1 基本使用原则

- **初始化**：实体创建时应默认设置为`ActionStatus.无操作`
- **操作开始**：开始具体操作时（如点击新增按钮），应立即设置对应的ActionStatus
- **操作结束**：操作完成或取消时，应重置为`ActionStatus.无操作`
- **状态变更**：通过BaseEntity的ActionStatus属性进行设置，确保状态变更通知正常工作
- **重要设计优化**：ActionStatus变更不会触发实体的HasChanged标志，因为它仅表示UI操作意图而非实际数据变更

### 3.2 UI层使用规范

- **数据绑定**：在BindData方法中接收ActionStatus参数，根据不同状态执行相应的数据加载逻辑
- **按钮控制**：根据当前ActionStatus状态控制界面按钮的可用性
- **界面行为**：根据ActionStatus调整界面行为，如新增时隐藏某些字段，修改时启用编辑

### 3.3 状态管理器集成

- **状态转换**：通过UnifiedStateManager的SetActionStatusAsync方法进行状态转换
- **规则验证**：确保状态转换符合预定义的转换规则
- **事件监听**：订阅状态变更事件，实现业务逻辑响应

## 4. 状态转换规则

ActionStatus的推荐转换规则：

- **无操作** → 新增/修改/删除/加载/复制：允许（开始操作）
- **新增/修改/删除/加载/复制** → 无操作：允许（操作完成/取消）
- **其他直接转换**：不推荐，应通过无操作状态过渡

## 5. 最佳实践

### 5.1 标准操作流程

```csharp
// 1. 开始新增操作
entity.ActionStatus = ActionStatus.新增;
// 执行新增相关逻辑
// ...
// 2. 操作完成，重置状态
entity.ActionStatus = ActionStatus.无操作;
```

### 5.2 与状态管理器结合使用

```csharp
// 使用状态管理器设置操作状态
await unifiedStateManager.SetActionStatusAsync(entity, ActionStatus.修改, "用户修改数据");
// 执行修改操作
// ...
// 操作完成，恢复无操作状态
await unifiedStateManager.SetActionStatusAsync(entity, ActionStatus.无操作, "操作完成");
```

### 5.3 事件响应模式

```csharp
// 订阅状态变更事件
entity.StatusChanged += OnEntityStatusChanged;

private void OnEntityStatusChanged(object sender, StatusChangedEventArgs e)
{
    if (e.StatusType == typeof(ActionStatus))
    {
        // 根据ActionStatus变更执行相应逻辑
        ActionStatus newStatus = (ActionStatus)e.NewStatus;
        switch (newStatus)
        {
            case ActionStatus.新增:
                // 处理新增状态逻辑
                break;
            case ActionStatus.修改:
                // 处理修改状态逻辑
                break;
            // 其他状态处理...
        }
    }
}
```

## 6. 性能优化建议

- **减少频繁变更**：避免在短时间内频繁变更ActionStatus值
- **批量操作考虑**：批量操作时可考虑临时禁用状态变更通知
- **事件处理优化**：确保状态变更事件处理器执行高效，避免阻塞操作

## 7. 常见问题与解决方案

- **状态不一致**：确保在所有操作路径上都正确设置和重置ActionStatus
- **事件重复触发**：通过比较新旧值避免不必要的状态变更通知
- **UI响应延迟**：优化事件处理器，考虑使用异步处理长时间运行的操作

## 8. 版本历史

- **V1.0**：初始设计，定义基本枚举值和使用规范
- **V2.0**：完善与状态管理系统的集成，添加详细使用指南
- **V3.0**：增加性能优化建议和最佳实践示例