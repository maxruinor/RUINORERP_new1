# BaseBillQueryMC.cs 文件CS1503错误修复方案

## 错误描述
文件中存在5处CS1503错误，错误原因是：无法从"System.Collections.Generic.List<M>"转换为"RUINORERP.PacketSpec.Models.Common.TodoUpdate"

## 解决方案

已创建辅助类 `RUINORERP.UI.Common.TodoHelper`，提供了以下扩展方法：

### 方法1: ProcessEntityUpdates 扩展方法
```csharp
// 使用方式示例 - 替换原有错误代码
if (TodoListManager != null && selectlist != null && selectlist.Count > 0)
{
    // 直接处理实体列表，自动转换为TodoUpdate并逐个处理
    TodoListManager.ProcessEntityUpdates(selectlist, TodoUpdateType.StatusChanged);
}
```

### 方法2: ConvertToTodoUpdates 转换方法
```csharp
// 使用方式示例 - 手动转换后处理
if (TodoListManager != null && selectlist != null && selectlist.Count > 0)
{
    // 转换为TodoUpdate列表
    var updates = TodoHelper.ConvertToTodoUpdates(selectlist, TodoUpdateType.StatusChanged);
    
    // 可以自定义处理每个更新，或批量处理
    foreach (var update in updates)
    {
        TodoListManager.ProcessUpdate(update);
    }
}
```

## 各种菜单项对应的更新类型
- 提交: TodoUpdateType.StatusChanged
- 审核: TodoUpdateType.Approved
- 反审: TodoUpdateType.StatusChanged
- 结案: TodoUpdateType.StatusChanged
- 删除: TodoUpdateType.Deleted

## 实现说明

TodoHelper类自动处理了：
1. 实体类型到业务类型的转换
2. 实体主键值的提取（支持ID或id属性名）
3. 类型安全的转换和异常处理
4. 日志记录错误信息

这个辅助类提供了类型安全、异常安全的方式来处理实体列表到TodoUpdate的转换和处理。
