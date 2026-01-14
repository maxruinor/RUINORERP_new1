# 优化RepeatOperationGuardService以兼容现有防重复操作机制

## 优化目标
1. 使RepeatOperationGuardService类适用于现有的单据编辑基类中的防重复操作机制
2. 添加状态消息显示功能，让用户看到提示
3. 保持与现有实现的兼容性
4. 提供统一的防重复操作接口

## 优化方案

### 1. 添加状态消息支持
- 在RepeatOperationGuardService类中添加显示状态消息的机制
- 支持通过MainForm.Instance.ShowStatusText()方法显示提示信息

### 2. 扩展ShouldBlockOperation方法
- 添加`showStatusMessage`参数，控制是否显示状态消息
- 当操作被阻止时，根据参数决定是否显示提示信息

### 3. 兼容现有实现的核心逻辑
- 支持同一实体的相同操作判断
- 支持按钮禁用状态管理
- 使用与现有实现相同的防抖间隔默认值

### 4. 添加按钮状态管理方法
- 添加DisableButton方法，用于禁用按钮并记录状态
- 添加EnableButton方法，用于启用按钮并清除状态
- 添加RecordButtonOperation方法，用于记录按钮操作

### 5. 支持实体级别的防重复检查
- 添加基于实体ID的防重复检查
- 支持不同实体的相同操作可以并行执行

## 代码实现计划

### 1. 修改RepeatOperationGuardService.cs
- 扩展OperationRecord类，添加实体ID字段
- 添加`showStatusMessage`参数到ShouldBlockOperation方法
- 添加DisableButton、EnableButton和RecordButtonOperation方法
- 添加状态消息显示逻辑

### 2. 确保与现有实现兼容
- 使用相同的默认防抖间隔（1000ms）
- 保持相似的操作判断逻辑
- 支持现有代码的调用方式

### 3. 提供统一的防重复操作接口
- 让BaseBillEditGeneric可以方便地使用RepeatOperationGuardService
- 支持现有方法的替换使用

## 预期效果
1. RepeatOperationGuardService将能够完全替代BaseBillEditGeneric中现有的防重复操作机制
2. 支持显示状态消息，提高用户体验
3. 提供统一的防重复操作接口，便于在其他基类（如BaseBillQueryMC.cs和BaseListGeneric.cs）中使用
4. 保持线程安全和高性能
5. 与现有代码兼容，无需大规模修改现有实现