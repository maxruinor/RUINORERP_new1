# Tasks

## 任务概述
基于现有状态管理系统架构，完善单据操作按钮状态的统一管理与一致性控制功能。

---

## 任务清单

### 任务1：分析现有按钮状态管理逻辑
- [x] 详细阅读`状态管理系统全面检查报告_V6.md`
- [x] 分析`UnifiedStateManager.GetUIControlStates`方法的当前实现
- [x] 分析`GlobalStateRulesManager`中的UI按钮规则定义
- [x] 分析`BaseBillEditGeneric.cs`中的`UpdateAllButtonStates`方法
- [x] 分析`BaseBillEdit.cs`中的按钮事件处理逻辑

### 任务2：完善保存按钮状态逻辑
- [x] 检查新建按钮点击后的保存按钮启用逻辑
- [x] 检查保存成功后的保存按钮禁用逻辑
- [x] 实现数据变更检测（利用BaseEntity.ChangedProperties）
- [x] 实现数据变更时保存按钮自动恢复启用
- [x] 实现保存失败时保持保存按钮可用

### 任务3：完善提交按钮状态控制
- [x] 检查提交按钮的启用条件（保存成功 + 提交权限）
- [x] 检查提交成功后的提交按钮禁用逻辑
- [x] 检查撤销提交操作后的提交按钮恢复逻辑
- [x] **修复**：DataStatus.新建状态下submitEnabled从true改为false，确保提交成功后禁用提交按钮

### 任务4：完善审核按钮状态控制
- [x] 检查审核按钮的启用条件（提交成功 + 审核权限）
- [x] 检查审核成功后的审核按钮禁用逻辑

### 任务5：完善按钮状态与单据状态精确匹配
- [x] 验证草稿状态的按钮可用性
- [x] 验证新建（已提交）状态的按钮可用性
- [x] 验证审核状态的按钮可用性
- [x] 验证结案/作废状态的按钮可用性
- [x] 验证特殊单据状态（财务模块）的按钮控制

### 任务6：优化GlobalStateRulesManager中的UI按钮规则
- [x] 检查并完善DataStatus的UI按钮规则
- [x] 检查并完善PaymentStatus的UI按钮规则
- [x] 检查并完善PrePaymentStatus的UI按钮规则
- [x] 检查并完善ARAPStatus的UI按钮规则
- [x] 检查并完善StatementStatus的UI按钮规则

### 任务7：优化UnifiedStateManager.GetUIControlStates方法
- [x] 增强数据变更检测逻辑（结合BaseEntity.ChangedProperties）
- [x] 确保保存按钮状态正确响应数据变更
- [x] 确保所有按钮状态与单据状态精确匹配

### 任务8：优化UI层按钮状态更新逻辑
- [x] 优化BaseBillEditGeneric.UpdateAllButtonStates方法
- [x] 确保按钮状态更新的实时性和准确性
- [x] 确保与用户权限系统的集成

### 任务9：验证和测试
- [ ] 测试新增单据流程的按钮状态
- [ ] 测试修改单据流程的按钮状态
- [ ] 测试保存失败场景的按钮状态
- [ ] 测试提交流程的按钮状态
- [ ] 测试审核流程的按钮状态
- [ ] 测试结案/作废状态的按钮状态

---

## Task Dependencies
- 任务2-4 依赖于 任务1 的分析结果
- 任务6-8 依赖于 任务1 的分析结果
- 任务9 依赖于 任务2-8 的完成

---

## 已完成的主要工作

### 1. 优化UnifiedStateManager.GetUIControlStates方法
- 增强了数据变更检测逻辑，结合BaseEntity.HasChanged属性
- 保存按钮状态现在根据数据变更情况动态调整：
  - 终态：保存按钮始终禁用
  - 非终态且有数据变更：保存按钮启用
  - 非终态但无数据变更：保存按钮禁用

### 2. 优化BaseBillEditGeneric.UpdateSaveButtonStateBasedOnChanges方法
- 重构为使用StateManager.GetUIControlStates获取统一的按钮状态
- 确保与状态管理器保持一致性
- 同时更新其他按钮状态，确保整体一致性

### 3. 修复提交按钮状态控制
- **重要修复**：DataStatus.新建状态下submitEnabled从true改为false
- 确保提交成功后提交按钮立即禁用（灰色）
- 仅在撤销提交后（返回草稿状态）提交按钮方可重新启用

### 4. 验证现有UI按钮规则
- 确认GlobalStateRulesManager中的DataStatus、PaymentStatus、PrePaymentStatus、ARAPStatus等状态的按钮规则已正确定义
- **完整业务流程验证**：
  - 草稿状态：启用新增、修改、删除、保存、提交按钮
  - 新建状态：启用审核、反审、撤销提交按钮（提交按钮禁用）
  - 确认状态：启用反审核、结案按钮
  - 作废/结案状态：禁用所有编辑操作按钮

### 5. 确认现有PropertyChanged机制
- 系统已有完善的PropertyChanged事件处理机制
- Entity_PropertyChanged事件处理器已实现
- UpdateSaveButtonStateBasedOnChanges方法已集成到属性变更流程中

---

## 各按钮状态控制总结

### 保存按钮
- **草稿状态**：根据HasChanged决定启用/禁用
- **新建状态**：根据HasChanged决定启用/禁用
- **确认/作废/结案**：始终禁用

### 提交按钮
- **草稿状态**：启用
- **新建状态**：禁用（提交成功后）
- **确认/作废/结案**：禁用
- **撤销提交后（返回草稿）**：重新启用

### 审核按钮
- **草稿状态**：禁用
- **新建状态**：启用
- **确认/作废/结案**：禁用

### 反审核按钮
- **草稿/新建状态**：禁用
- **确认状态**：启用
- **作废/结案**：禁用

### 结案按钮
- **草稿/新建状态**：禁用
- **确认状态**：启用
- **作废/结案**：禁用

### 反结案按钮
- **草稿/新建/确认/作废**：禁用
- **结案状态**：启用
