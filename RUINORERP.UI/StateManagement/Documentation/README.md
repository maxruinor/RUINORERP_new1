# 状态管理系统文档

本目录包含RUINORERP项目状态管理系统的所有相关文档。

## 文档列表

### 核心文档

1. **状态管理系统统一重构文档.md** - 状态管理系统的完整重构文档，包含架构设计、文件处理方案、命名空间分布和设计思路
2. **状态管理架构在UI层的应用指南.md** - 详细说明如何将状态管理架构应用到UI层，特别是单据窗体和实体类中
3. **V3状态管理系统代码清单.md** - V3状态管理系统的核心文件清单，包含所有相关文件的路径和简要说明

### 技术方案

4. **RUINORERP分布式锁定与状态同步技术方案.md** - 分布式锁定与状态同步的技术方案
5. **锁定冲突处理与状态同步优化策略.md** - 锁定冲突处理与状态同步优化策略

## 文档整合说明

根据状态管理系统统一重构文档，以下文档已被整合到"状态管理系统统一重构文档.md"中：

- 状态管理系统架构V3.md
- 统一状态管理系统架构文档.md
- 状态管理系统重构计划V3.md
- 状态管理系统V3架构审查与修改报告.md
- 状态管理系统架构与实施指南v2.md
- 状态管理迁移指南.md
- StatusTransitionContext详细文档.md
- 数据状态管理架构总结.md

这些文档的内容已整合到统一重构文档中，避免了文档分散和内容重复的问题。

## 文档阅读顺序

如果您是初次接触状态管理系统，建议按以下顺序阅读文档：

1. 首先阅读 **状态管理系统统一重构文档.md**，了解整体架构和重构内容
2. 然后阅读 **状态管理架构在UI层的应用指南.md**，了解如何在UI层应用状态管理
3. 对于分布式环境下的状态管理，请阅读 **RUINORERP分布式锁定与状态同步技术方案.md**
4. 如需了解锁定冲突处理，请阅读 **锁定冲突处理与状态同步优化策略.md**

## 版本信息

- 当前状态管理系统版本：V3
- 文档最后更新时间：2025年11月
- 维护团队：RUINOR ERP开发团队

## 更新记录

- 2025年11月: 添加V3状态管理系统代码清单.md到文档列表
- 2025年11月: 修复IStatusTransitionRecord接口缺少EntityType属性的问题
- 2025年11月: 更正StateManagerFactoryV3中的注释错误，将"使用Model项目中的UnifiedStateManager"更正为"使用UI项目中的UnifiedStateManager"
- 2025年11月: 移除StateManagerOptions类中的冗余属性(EnableLogging/EnableValidation/CustomTransitionRules)，统一使用EnableTransitionLogging/EnableTransitionValidation/TransitionRules
- 2025年11月: 修复所有引用已删除冗余属性的代码，包括UnifiedStateManager.cs、BaseBillEdit.cs、BaseBillEditGeneric.cs和BaseEntity.cs