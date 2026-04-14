# 单据操作按钮状态统一管理与一致性控制功能 Spec

## Why
当前系统中按钮状态管理存在不一致性，特别是保存、提交、审核等关键操作按钮的状态逻辑不够精确，导致用户操作流程不规范，可能引起数据一致性问题。需要完善按钮状态与单据状态的精确匹配，确保按钮可用性严格遵循业务流程逻辑。

## What Changes
- **增强保存按钮状态逻辑**：新建后立即启用，保存成功后禁用，数据变更时自动恢复
- **优化提交按钮状态控制**：仅保存成功且有权限时启用，提交后禁用，撤销后恢复
- **完善审核按钮状态控制**：仅提交成功且有权限时启用，审核后禁用
- **统一按钮状态管理**：基于现有状态管理系统架构进行功能完善
- **结合数据变更检测**：利用BaseEntity的变化属性集合实现数据变更检测
- **结合用户权限系统**：所有状态判断需结合用户权限进行综合评估

## Impact
- Affected specs: 状态管理系统, 单据编辑UI
- Affected code: 
  - `RUINORERP.Model/Base/StatusManager/UnifiedStateManager.cs`
  - `RUINORERP.Model/Base/StatusManager/GlobalStateRulesManager.cs`
  - `RUINORERP.UI/BaseForm/BaseBillEdit.cs`
  - `RUINORERP.UI/BaseForm/BaseBillEditGeneric.cs`
  - `RUINORERP.Model/Base/BaseEntity.cs`

## ADDED Requirements

### Requirement: 保存按钮状态精确控制
系统 SHALL 实现保存按钮状态的精确控制，确保：
- 用户点击新建按钮后，保存按钮立即启用
- 保存操作成功后，保存按钮设置为禁用状态（灰色）
- 当单据处于可修改状态且数据发生变更时，保存按钮自动恢复启用状态
- 保存操作失败时，保持保存按钮可用状态，并在状态栏显示错误信息

#### Scenario: 新建单据保存成功
- **WHEN** 用户点击新建按钮
- **THEN** 保存按钮立即启用
- **WHEN** 用户填写数据并点击保存
- **THEN** 保存操作成功后，保存按钮变为禁用状态

#### Scenario: 修改已有单据
- **WHEN** 用户打开已有单据
- **THEN** 保存按钮初始为禁用状态
- **WHEN** 用户修改数据
- **THEN** 保存按钮自动恢复启用状态

#### Scenario: 保存失败
- **WHEN** 用户点击保存但验证失败
- **THEN** 保存按钮保持启用状态
- **THEN** 状态栏显示错误信息

---

### Requirement: 提交按钮状态控制
系统 SHALL 实现提交按钮状态控制，确保：
- 仅在保存操作成功且用户具有提交权限时，提交按钮才启用
- 提交操作成功后，提交按钮立即设置为禁用状态，防止重复提交
- 仅当执行撤销提交操作后，提交按钮方可重新启用

#### Scenario: 提交按钮启用条件
- **WHEN** 单据保存成功且处于草稿状态
- **AND** 用户具有提交权限
- **THEN** 提交按钮启用

#### Scenario: 提交成功后
- **WHEN** 用户成功提交单据
- **THEN** 提交按钮立即禁用

---

### Requirement: 审核按钮状态控制
系统 SHALL 实现审核按钮状态控制，确保：
- 仅在提交操作成功且用户具有审核权限时，审核按钮才启用
- 审核操作成功后，审核按钮立即设置为禁用状态

#### Scenario: 审核按钮启用条件
- **WHEN** 单据已成功提交
- **AND** 用户具有审核权限
- **THEN** 审核按钮启用

#### Scenario: 审核成功后
- **WHEN** 用户成功审核单据
- **THEN** 审核按钮立即禁用

---

### Requirement: 按钮状态与单据状态精确匹配
系统 SHALL 确保按钮状态与单据状态（DataStatus）精确匹配，遵循业务流程逻辑：
- 草稿状态：启用新增、修改、删除、保存、提交按钮
- 新建（已提交）状态：启用审核、反审按钮
- 审核状态：启用结案按钮
- 结案/作废状态：禁用所有编辑操作按钮

#### Scenario: 按钮状态与单据状态匹配
- **WHEN** 单据处于草稿状态
- **THEN** 新增、修改、删除、保存、提交按钮启用
- **WHEN** 单据处于新建状态
- **THEN** 审核、反审按钮启用
- **WHEN** 单据处于审核状态
- **THEN** 结案按钮启用
- **WHEN** 单据处于结案或作废状态
- **THEN** 所有编辑操作按钮禁用

---

## MODIFIED Requirements

### Requirement: 利用现有状态管理系统架构
修改现有按钮状态管理逻辑，基于IUnifiedStateManager接口和UnifiedStateManager实现类进行状态控制，确保：
- 所有按钮状态查询通过StateManager.GetUIControlStates获取
- 按钮状态规则定义在GlobalStateRulesManager中
- 保持现有防抖、防重复操作逻辑不变

---

### Requirement: 利用实体变化属性集合
修改数据变更检测逻辑，利用BaseEntity中的实体变化属性集合实现数据变更检测，确保：
- 检查实体的ChangedProperties集合
- 当有变更属性时，保存按钮启用
- 保存成功后清空变更属性集合

---

## 技术实现原则

### 1. 基于现有架构完善
- 禁止重构现有防抖、防重复操作逻辑
- 基于IUnifiedStateManager接口和UnifiedStateManager实现类进行状态控制
- 利用BaseEntity中的实体变化属性集合实现数据变更检测

### 2. 简洁高效实现
- 确保实现简洁高效，禁止过度设计和冗余代码
- 所有状态判断需结合用户权限系统进行综合评估

### 3. 质量保障
- 确保修改后不会引入新的功能错误或性能问题
- 保证按钮状态变化的实时性和准确性
- 维持系统原有功能和业务流程的完整性

---

## 特殊单据状态说明

除了通用的DataStatus（草稿、新建、审核、结案、作废）外，财务模块等特殊单据需参考其特定状态实现：
- PaymentStatus（付款状态）
- PrePaymentStatus（预付款状态）
- ARAPStatus（应收应付状态）
- StatementStatus（对账状态）

这些特殊状态的按钮控制规则已在GlobalStateRulesManager中定义，需确保其与通用规则保持一致。
