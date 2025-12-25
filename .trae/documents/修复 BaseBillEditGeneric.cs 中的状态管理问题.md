## 问题分析

在 BaseBillEditGeneric.cs 中，状态管理体系存在应用问题：单据编辑场景下的状态判断逻辑错误。根据状态管理系统设计规范，修改操作仅需验证当前状态是否允许编辑，无需指定目标状态（目标状态仅适用于提交、审核、结案、反审核、反结案等具有明确状态转换的业务按钮）。

## 修复方案

1. **调整 CanExecuteAction 方法**：为修改操作添加特殊处理，直接检查当前状态是否允许编辑
2. **修改操作特殊处理**：修改操作不需要通过状态转换规则验证，只需检查当前状态下是否允许编辑
3. **保持其他按钮逻辑不变**：确保提交、审核等具有明确状态转换的业务按钮逻辑不受影响

## 修复步骤

1. 找到 `BaseBillEditGeneric.cs` 中的 `CanExecuteAction` 方法（第537-590行）
2. 分析当前的状态检查逻辑
3. 为 `MenuItemEnums.修改` 操作添加特殊处理
4. 修改操作直接检查当前状态是否允许编辑，使用 `GlobalStateRulesManager` 中的按钮规则
5. 确保修改后的代码与 `UnifiedStateManager` 中的状态转换机制一致

## 预期结果

- 不同状态下编辑功能的正确性得到验证
- 不影响其他业务按钮的状态转换逻辑
- UI按钮可用性及Label显示内容能根据当前状态正确更新
- 与 `UnifiedStateManager.cs` 中状态转换机制保持一致

## 修复代码位置

- 文件：`e:\CodeRepository\SynologyDrive\RUINORERP\RUINORERP.UI\BaseForm\BaseBillEditGeneric.cs`
- 方法：`CanExecuteAction`（第537-590行）
- 重点：调整修改操作的状态检查逻辑