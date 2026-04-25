# TreeView节点选择修复说明

## 📅 更新日期
2026-04-25

## ❌ 问题描述

用户反馈两个问题：

1. **选中分类节点提示没有选中**
   - 点击"基础数据"等分类节点时，系统提示"请先选择要清理的数据表"
   
2. **选中单个表不自动加载数据**
   - 勾选某个具体的表（如"类目表"）后，DataGridView没有自动加载数据

## 🔍 问题分析

### 根本原因

在 `UpdateTreeViewWithRecordCounts()` 方法中，我们将节点的Tag从 `Type` 类型更新为 `TreeNodeData` 类型：

```csharp
// 初始时
entityNode.Tag = entity.EntityType; // Type 类型

// 加载记录数后
entityNode.Tag = new TreeNodeData
{
    EntityType = entityType,
    RecordCount = recordCount,
    TableName = metadata.TableName
}; // TreeNodeData 类型
```

但是在 `ProcessSelectionAfterCheck()` 方法中，只处理了 `Type` 类型：

```csharp
// ❌ 旧代码：只能处理 Type 类型
_selectedEntityType = selectedNodes[0].Tag as Type;
```

当Tag是 `TreeNodeData` 类型时，`as Type` 返回 `null`，导致 `_selectedEntityType` 为null，从而：
- 刷新按钮检查失败，提示"请先选择要清理的数据表"
- 无法自动加载数据

## ✅ 解决方案

### 修改 ProcessSelectionAfterCheck 方法

在 [UCBasicDataCleanup.cs](file:///e:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.UI/SysConfig/BasicDataCleanup/UCBasicDataCleanup.cs#L215-L261) 中：

```csharp
/// <summary>
/// 处理选择逻辑（提取为独立方法，避免重复代码）
/// </summary>
private void ProcessSelectionAfterCheck()
{
    // 获取所有选中的子节点（实体节点）
    var selectedNodes = GetSelectedEntityNodes();
    
    if (selectedNodes.Count == 0)
    {
        // 没有选中任何实体
        _selectedEntityType = null;
        dgvDataPreview.Rows.Clear();
        kbtnDeleteSelected.Enabled = false;
        kbtnPreview.Enabled = false;
        return;
    }

    if (selectedNodes.Count == 1)
    {
        // 单选：加载预览数据
        var node = selectedNodes[0];
        
        // ✅ 修复：支持 Type 和 TreeNodeData 两种Tag类型
        if (node.Tag is Type type)
        {
            _selectedEntityType = type;
        }
        else if (node.Tag is TreeNodeData nodeData)
        {
            _selectedEntityType = nodeData.EntityType;
        }
        
        if (_selectedEntityType != null)
        {
            _ = LoadDataAsync();
            kbtnPreview.Enabled = true;
            kbtnDeleteSelected.Enabled = true;
        }
    }
    else
    {
        // 多选：不加载预览，直接启用删除按钮
        _selectedEntityType = null;
        dgvDataPreview.Rows.Clear();
        kbtnPreview.Enabled = false;
        kbtnDeleteSelected.Enabled = true;
        
        // 显示多选提示
        AppendRealTimeLog($"已选择 {selectedNodes.Count} 个数据表，将进行批量删除");
    }
}
```

### 关键改进

1. **支持两种Tag类型**
   ```csharp
   if (node.Tag is Type type)
   {
       _selectedEntityType = type;
   }
   else if (node.Tag is TreeNodeData nodeData)
   {
       _selectedEntityType = nodeData.EntityType;
   }
   ```

2. **兼容性保证**
   - 如果Tag还是 `Type` 类型（未加载记录数前），正常工作
   - 如果Tag已更新为 `TreeNodeData` 类型（加载记录数后），也能正常工作

## 🧪 测试验证

### 测试场景1：启动后立即选择表

**操作步骤**：
1. 打开基础数据清理工具
2. 等待记录数加载完成（TreeView节点显示记录数）
3. 勾选任意一个表（如"类目表"）

**预期结果**：
- ✅ DataGridView自动加载数据
- ✅ "删除选中"按钮启用
- ✅ "预览"按钮启用
- ✅ 状态栏显示记录数

### 测试场景2：刷新数据

**操作步骤**：
1. 选择一个表
2. 点击"刷新"按钮

**预期结果**：
- ✅ 不会提示"请先选择要清理的数据表"
- ✅ 正常刷新数据
- ✅ 日志显示"[缓存更新] 表 xxx 记录数: xxx"

### 测试场景3：分类节点选择

**操作步骤**：
1. 点击"基础数据"分类节点的CheckBox

**预期结果**：
- ✅ 全选/取消全选该分类下的所有子节点
- ✅ 如果只选中一个子节点，自动加载数据
- ✅ 如果选中多个子节点，启用批量删除

### 测试场景4：删除后重新选择

**操作步骤**：
1. 选择一个表并执行删除
2. 删除完成后，再次勾选同一个表

**预期结果**：
- ✅ 正常加载数据（可能为空）
- ✅ TreeView节点显示最新的记录数（可能为0）

## 📊 相关代码位置

### 修改的文件
1. ✅ `RUINORERP.UI/SysConfig/BasicDataCleanup/UCBasicDataCleanup.cs`
   - 修改 `ProcessSelectionAfterCheck()` 方法（第215-261行）

### 涉及的相关方法
1. `UpdateTreeViewWithRecordCounts()` - 更新节点Tag为TreeNodeData
2. `GetSelectedEntityNodes()` - 获取选中的节点（已支持两种Tag类型）
3. `GetSelectedEntityTypes()` - 获取选中的实体类型（已支持两种Tag类型）
4. `UpdateTreeNodeRecordCount()` - 更新单个节点的记录数

## 🎯 技术要点

### 1. Tag类型的演变

```
初始化时：
  entityNode.Tag = Type

加载记录数后：
  entityNode.Tag = TreeNodeData { EntityType, RecordCount, TableName }

ProcessSelectionAfterCheck需要同时支持这两种情况
```

### 2. 模式匹配的使用

```csharp
// C# 7.0+ 的模式匹配语法
if (node.Tag is Type type)
{
    // type 变量自动转换为 Type 类型
}
else if (node.Tag is TreeNodeData nodeData)
{
    // nodeData 变量自动转换为 TreeNodeData 类型
}
```

### 3. 向后兼容

即使某些节点的Tag还没有更新为 `TreeNodeData`（比如加载记录数失败），代码仍然能正常工作。

## ⚠️ 注意事项

### 1. 加载顺序
- 工具启动时先初始化TreeView（Tag为Type）
- 然后异步加载记录数（Tag更新为TreeNodeData）
- 在加载过程中选择表，仍能正常工作

### 2. 异常处理
- 如果加载记录数失败，Tag保持为Type类型
- ProcessSelectionAfterCheck仍然能正确处理

### 3. 性能影响
- 模式匹配的开销极小，可以忽略
- 不影响用户体验

## 📝 总结

本次修复解决了以下问题：

1. ✅ **选中表后不自动加载数据** - 现在能正确识别TreeNodeData类型
2. ✅ **刷新时提示未选择表** - 现在能正确获取_selectedEntityType
3. ✅ **兼容性问题** - 同时支持Type和TreeNodeData两种Tag类型
4. ✅ **健壮性** - 即使部分节点加载失败，仍能正常工作

**版本**: v3.4  
**状态**: ✅ 已完成  
**下一步**: 在实际环境中测试，验证修复效果
