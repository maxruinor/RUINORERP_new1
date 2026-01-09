# 行级权限配置智能编辑界面优化方案

## 目标
基于现有Business目录下的行数据权限实现和窗体查询基类中的通用查询方法，优化配置编辑窗体使其更加智能化。

## 改进要求

### 1. 连接类型下拉选择框优化
- 当前：手动输入连接类型
- 改进：改为下拉选择框，支持固定值选项
- 选项包括：
  - INNER JOIN（内连接）
  - LEFT JOIN（左连接）
  - RIGHT JOIN（右连接）
  - FULL JOIN（全连接）
  - 不需要联表

### 2. 扩展选择范围

#### 2.1 业务类型选择
- **BizType定义的单据**：从BizType枚举中获取所有定义的业务类型
- **基础数据表**：从缓存初始化服务获取所有基础数据表
  - 基础数据包括：`tb_Company`, `tb_Currency`, `tb_Unit`, `tb_Department`, `tb_Employee`等
  - 表类型：`TableType.Base`
- **业务数据表**：从缓存获取所有业务表
  - 表类型：`TableType.Business`
- **视图**：支持选择视图作为目标表
  - 表类型：`TableType.View`

#### 2.2 表数据来源
- 从`ITableSchemaManager`获取表结构信息
- 从`EntityCacheInitializationService`获取缓存状态
- 从`IEntityMappingService`获取实体映射关系

### 3. 基于角色的数据过滤

#### 3.1 角色选择器
- 添加角色选择下拉框
- 从`tb_RoleInfo`表加载所有角色
- 支持多选（一个规则可以应用到多个角色）

#### 3.2 基础数据过滤
- 利用实体表中已有的中文描述字段（如`tb_Prod.CNName`）
- 支持配置如：
  - 限制某角色只能查看特定类别的产品（如按`ProductType`字段）
  - 限制某角色只能查看特定库位（按`LocationType`字段）
  - 限制某角色只能查看特定区域的产品

#### 3.3 过滤条件生成
- 基于选择的角色和基础数据字段生成过滤条件
- 示例：`ProductType_ID = {ProductTypeId}` 或 `LocationType_ID = {LocationTypeId}`
- 支持参数化占位符，动态替换

### 4. 界面优化

#### 4.1 控件组织
```
┌─────────────────────────────────────────────────────────────┐
│ 业务信息配置区                                          │
├─────────────────────────────────────────────────────────────┤
│ 业务类型: [下拉框]          默认规则: [下拉框]    │
│ 目标表: [下拉框]            状态: [启用]☑    │
│ 目标实体: [只读]                                  │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│ 联表配置区                                            │
├─────────────────────────────────────────────────────────────┤
│ ☑需要联表   连接类型: [INNER JOIN▼]                │
│ 关联表: [下拉框]  关联字段: [下拉框]          │
│ 关联条件: [文本框]                                  │
│ 目标表关联字段: [文本框]  关联表关联字段: [文本框] │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│ 角色过滤配置区                                         │
├─────────────────────────────────────────────────────────────┤
│ ☑启用角色过滤  适用角色: [多选下拉框]                 │
│ 基础数据分类: [下拉框]  分类值: [下拉框]    │
│ 分类字段: [下拉框]                                    │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│ 过滤条件配置区                                         │
├─────────────────────────────────────────────────────────────┤
│ 字段: [下拉框]  操作符: [ = ▼]  值: [文本框]  │
│ [生成条件]                                            │
│ 过滤条件: [多行文本框]                              │
│                                                     │
│ 参数化过滤: ☑使用参数化                           │
│ 参数化模板: [多行文本框]                           │
│ 支持占位符: {UserId}, {RoleId}, {分类值}        │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│ SQL预览区                                              │
├─────────────────────────────────────────────────────────────┤
│ SELECT * FROM [目标表]                                 │
│   [连接类型] [关联表] ON [关联条件]                  │
│ WHERE [过滤条件]                                      │
└─────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────┐
│ 描述信息区                                             │
├─────────────────────────────────────────────────────────────┤
│ 规则名称: [文本框]                                 │
│ 规则描述: [多行文本框]                               │
│                                                     │
│                    [确定]  [取消]                          │
└─────────────────────────────────────────────────────────────┘
```

#### 4.2 交互优化
- 选择默认规则时自动填充所有相关配置
- 选择目标表时自动加载该表的字段列表
- 选择关联字段时自动生成关联条件
- 实时SQL预览，延迟500ms更新
- 智能提示：字段类型对应合适的操作符

#### 4.3 验证逻辑
- 必填项验证：业务类型、目标表、规则名称
- 条件验证：使用`SmartRuleConfigHelper.ValidateFilterClause`
- 参数占位符验证：确保所有占位符都有对应的值源

## 技术实现要点

### 1. 数据获取服务
```csharp
// 获取表结构管理器
private readonly ITableSchemaManager _tableSchemaManager;

// 获取所有可缓存的表名
var allCacheableTables = _tableSchemaManager.GetCacheableTableNamesList();
var baseTables = _tableSchemaManager.GetBaseBusinessTableNames();
var businessTables = _tableSchemaManager.GetCacheableTableNamesByType(TableType.Business);
```

### 2. 连接类型枚举
```csharp
public enum JoinType
{
    [Description("不需要联表")]
    None,
    [Description("内连接(INNER JOIN)")]
    Inner,
    [Description("左连接(LEFT JOIN)")]
    Left,
    [Description("右连接(RIGHT JOIN)")]
    Right,
    [Description("全连接(FULL JOIN)")]
    Full
}
```

### 3. 角色过滤配置类
```csharp
public class RoleFilterConfig
{
    public bool IsEnabled { get; set; }
    public List<long> RoleIds { get; set; }
    public string FilterField { get; set; }
    public string FilterValue { get; set; }
}
```

### 4. 扩展的RuleConfiguration类
```csharp
public class ExtendedRuleConfiguration : RuleConfiguration
{
    // 新增属性
    public JoinType JoinTypeEnum { get; set; }
    public RoleFilterConfig RoleFilter { get; set; }
}
```

## 与现有架构的集成

### 1. 利用EntityCacheInitializationService
- 使用`GetCacheableTableNamesList()`获取所有可配置的表
- 使用`GetBaseBusinessTableNames()`获取基础数据表
- 使用`GetCacheableTableNamesByType(TableType.Business)`获取业务表

### 2. 利用DefaultRowAuthRuleProvider
- 调用`GetDefaultRuleOptions(BizType)`获取默认规则
- 调用`CreatePolicyFromDefaultOption()`创建策略对象
- 支持扩展规则配置

### 3. 利用SmartRuleConfigHelper
- 使用`GetEntityFields()`获取表字段
- 使用`ValidateFilterClause()`验证条件
- 使用`GetSuggestedPolicyName()`生成建议名称

## 实施步骤

### 阶段1：准备阶段
1. 创建`JoinType`枚举
2. 创建`RoleFilterConfig`类
3. 扩展`RuleConfiguration`类
4. 修改Designer文件添加新控件

### 阶段2：核心功能实现
1. 实现连接类型下拉框逻辑
2. 实现扩展表选择逻辑（BizType + Base + Business）
3. 实现角色过滤配置逻辑
4. 实现基础数据分类过滤

### 阶段3：预览和验证
1. 实现实时SQL预览
2. 实现增强的验证逻辑
3. 实现参数占位符验证

### 阶段4：测试和优化
1. 测试各种配置场景
2. 验证SQL生成的正确性
3. 优化用户体验

## 预期收益

1. **配置效率提升**：通过下拉选择和智能填充，减少50%以上配置时间
2. **准确性提升**：通过结构化表信息，避免拼写错误
3. **扩展性提升**：支持角色过滤和基础数据分类，满足复杂业务需求
4. **用户体验提升**：实时预览和智能提示，降低学习成本
