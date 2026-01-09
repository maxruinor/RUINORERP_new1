# 行级权限规则优化实现文档

## 概述

本文档描述了行级权限规则的完整实现方案，包括：
1. 用户/角色与权限策略的关联
2. 智能规则编辑窗体的优化
3. 在查询中自动应用行级权限规则
4. 系统启动时的策略加载机制

## 架构设计

### 核心服务类

#### 1. IRowAuthPolicyQueryService / RowAuthPolicyQueryService
**文件路径**：`RUINORERP.Business/RowLevelAuthService/IRowAuthPolicyQueryService.cs`

**职责**：
- 根据用户ID获取用户的行级权限策略
- 根据角色ID列表获取角色的行级权限策略
- 根据用户和角色获取所有行级权限策略（并集）
- 根据实体类型获取对应的权限策略
- 检查菜单是否配置了行级权限
- 提供缓存机制提升查询性能

**核心方法**：
```csharp
Task<List<tb_RowAuthPolicy>> GetPoliciesByUserAsync(long userId, long? menuId = null);
Task<List<tb_RowAuthPolicy>> GetPoliciesByRolesAsync(List<long> roleIds, long? menuId = null);
Task<List<tb_RowAuthPolicy>> GetPoliciesByUserAndRolesAsync(long userId, List<long> roleIds, long? menuId = null);
```

#### 2. IRowAuthPolicyLoaderService / RowAuthPolicyLoaderService
**文件路径**：`RUINORERP.Business/RowLevelAuthService/RowAuthPolicyLoaderService.cs`

**职责**：
- 系统启动时预加载所有启用的行级权限策略
- 预加载指定用户的策略
- 预加载指定角色的策略
- 策略变更时刷新缓存

**核心方法**：
```csharp
Task LoadAllPoliciesAsync();
Task LoadPoliciesForUserAsync(long userId);
Task RefreshPolicyCacheAsync();
```

### 扩展方法

#### RowLevelAuthExtensions
**文件路径**：`RUINORERP.Business/RowLevelAuthService/RowLevelAuthExtensions.cs`

**职责**：
- 为ISugarQueryable提供行级权限扩展方法
- 应用联表操作
- 应用过滤条件
- 支持参数化过滤

**核心方法**：
```csharp
ISugarQueryable<T> ApplyRowLevelAuth<T>(
    this ISugarQueryable<T> query,
    List<tb_RowAuthPolicy> policies,
    ISqlSugarClient db,
    ILogger logger = null) where T : class, new();
```

### UI辅助类

#### 1. JoinType
**文件路径**：`RUINORERP.UI/BI/JoinType.cs`

**描述**：SQL连接类型枚举
- Inner: 内连接
- Left: 左连接
- Right: 右连接
- Full: 全连接
- None: 不需要联表

#### 2. TableType / TableInfo
**文件路径**：`RUINORERP.UI/BI/TableType.cs`

**描述**：
- TableType: 表类型枚举（基础数据、业务数据、视图、系统表等）
- TableInfo: 表信息类（包含表名、实体类型、描述、类型等）

#### 3. EntityTypeSelectorHelper
**文件路径**：`RUINORERP.UI/BI/EntityTypeSelectorHelper.cs`

**职责**：
- 从表结构管理器获取所有可用的表
- 按表类型分类管理表信息
- 获取表的字段信息
- 确定表的类型

## BaseListGeneric集成

### 修改内容

**文件**：`RUINORERP.UI/BaseForm/BaseListGeneric.cs`

**新增功能**：
1. 添加了`GetRowAuthPolicies()`方法，获取当前用户的行级权限策略
2. 修改了`QueryAsync()`方法，在查询时自动应用行级权限规则

### 使用方式

在`QueryAsync`方法中：
```csharp
// 获取行级权限策略
var rowAuthPolicies = GetRowAuthPolicies();

// 应用到查询
var query = MainForm.Instance.AppContext.Db.Queryable<T>()
    .WhereIF(expression != null, expression)
    .ApplyRowLevelAuth(rowAuthPolicies, MainForm.Instance.AppContext.Db);
```

## 智能编辑窗体优化计划

### 优化内容

#### 1. 连接类型改为下拉选择框
- 将`txtJoinType`改为`cmbJoinType`下拉框
- 支持选项：内连接、左连接、右连接、全连接、不需要联表
- 使用`JoinType`枚举

#### 2. 扩展选择范围
- 支持BizType定义的单据
- 包含项目中的基础数据表
- 使用`EntityTypeSelectorHelper`从`ITableSchemaManager`获取所有可缓存的表
- 按表类型分类：基础数据、业务数据、视图、系统表

#### 3. 角色过滤功能
- 添加角色多选下拉框
- 支持基于基础数据字段进行过滤（如`tb_Prod.ProductType_ID`、`tb_Location.LocationType_ID`）
- 生成参数化过滤条件
- 支持占位符替换

#### 4. 界面优化
- 分组配置区域：业务信息、联表配置、角色过滤、过滤条件
- 实时SQL预览
- 智能提示和验证

### 实现步骤

1. 在Designer文件中添加新控件：
   - `cmbJoinType`: 连接类型下拉框
   - `cmbEntityType`: 实体类型下拉框（支持分组显示）
   - `cmbRoles`: 角色多选下拉框
   - `cmbFilterField`: 过滤字段下拉框
   - `cmbFilterValue`: 过滤值下拉框

2. 在代码文件中实现事件处理：
   - 实体类型选择后自动加载字段列表
   - 角色选择后生成对应的过滤条件
   - 实时更新SQL预览

## 系统启动配置

### 在DI容器中注册服务

在`Startup.cs`或相应的依赖注入配置中添加：

```csharp
// 注册行级权限策略查询服务
services.AddSingleton<IRowAuthPolicyQueryService, RowAuthPolicyQueryService>();

// 注册行级权限策略加载服务
services.AddSingleton<IRowAuthPolicyLoaderService, RowAuthPolicyLoaderService>();
```

### 系统启动时加载策略

在系统初始化代码中：

```csharp
// 加载行级权限策略
var policyLoaderService = appContext.GetFromFac<IRowAuthPolicyLoaderService>();
await policyLoaderService.LoadAllPoliciesAsync();
```

## 数据库表结构

### tb_P4RowAuthPolicyByRole
**描述**：行级权限规则-角色关联表

**关键字段**：
- `Policy_Role_RID`: 主键
- `PolicyId`: 行级权限规则ID
- `RoleID`: 角色ID
- `MenuID`: 菜单ID
- `IsEnabled`: 是否启用

### tb_P4RowAuthPolicyByUser
**描述**：行级权限规则-用户关联表

**关键字段**：
- `Policy_User_RID`: 主键
- `PolicyId`: 行级权限规则ID
- `User_ID`: 用户ID
- `MenuID`: 菜单ID
- `IsEnabled`: 是否启用

## 使用示例

### 示例1：为角色配置行级权限

```csharp
// 1. 创建行级权限策略
var policy = new tb_RowAuthPolicy
{
    PolicyName = "产品类型限制",
    EntityType = "RUINORERP.Model.tb_Prod",
    FilterClause = "ProductType_ID = 1",
    IsEnabled = true
};

// 2. 保存策略
var policyService = Startup.GetFromFac<Itb_RowAuthPolicyServices>();
await policyService.AddAsync(policy);

// 3. 为角色分配策略
var policyByRole = new tb_P4RowAuthPolicyByRole
{
    PolicyId = policy.PolicyId,
    RoleID = 1, // 销售员角色
    MenuID = 100, // 产品管理菜单
    IsEnabled = true
};

// 4. 保存关联
var policyByRoleService = Startup.GetFromFac<Itb_P4RowAuthPolicyByRoleServices>();
await policyByRoleService.AddAsync(policyByRole);
```

### 示例2：为用户配置行级权限

```csharp
// 1. 为用户分配策略（覆盖角色权限）
var policyByUser = new tb_P4RowAuthPolicyByUser
{
    PolicyId = policy.PolicyId,
    User_ID = 100, // 用户ID
    MenuID = 100, // 产品管理菜单
    IsEnabled = true
};

// 2. 保存关联
var policyByUserService = Startup.GetFromFac<Itb_P4RowAuthPolicyByUserServices>();
await policyByUserService.AddAsync(policyByUser);
```

### 示例3：查询时自动应用行级权限

```csharp
// 在BaseListGeneric<T>的查询中自动应用
public async override void QueryAsync(bool UseAutoNavQuery = false)
{
    // 获取行级权限策略
    var rowAuthPolicies = GetRowAuthPolicies();
    
    // 应用到查询
    var list = await MainForm.Instance.AppContext.Db.Queryable<T>()
        .ApplyRowLevelAuth(rowAuthPolicies, MainForm.Instance.AppContext.Db)
        .ToListAsync();
        
    // 绑定数据...
}
```

## 性能优化

### 缓存策略

1. **策略查询缓存**：
   - 使用`EventDrivenCacheManager`缓存查询结果
   - 缓存Key格式：`RowAuthPolicy_User_{userId}_Menu_{menuId}`
   - 缓存过期时间：可配置（建议1小时）

2. **表信息缓存**：
   - `EntityTypeSelectorHelper`内部缓存表信息
   - 减少对`ITableSchemaManager`的重复查询

### 预加载

- 系统启动时预加载所有用户的策略
- 用户登录时预加载该用户的策略
- 角色变更时刷新相关用户的策略缓存

## 扩展性设计

### 1. 支持新的过滤条件类型

可以通过扩展`RowLevelAuthExtensions`来支持新的过滤条件类型。

### 2. 支持动态参数

`ParameterizedFilterClauseProvider`已经支持动态参数，可以在运行时替换占位符。

### 3. 支持更复杂的联表逻辑

可以通过扩展`ApplyJoin`方法来支持更复杂的联表逻辑。

## 注意事项

1. **优先级**：用户策略优先级高于角色策略
2. **去重**：通过`PolicyId`去重，避免重复应用
3. **错误处理**：获取策略失败时不中断查询，只记录日志
4. **日志记录**：关键操作都记录日志，便于排查问题
5. **事务一致性**：策略变更时需要刷新缓存

## 后续优化建议

1. **批量操作优化**：支持批量应用多个策略
2. **策略验证**：添加策略语法验证功能
3. **性能监控**：添加策略应用性能监控
4. **可视化配置**：提供可视化的规则配置界面
5. **策略模板**：提供常用策略模板

## 总结

本实现方案提供了完整的行级权限功能：
- ✅ 用户/角色与权限策略的关联
- ✅ 智能规则编辑窗体的优化设计
- ✅ 在查询中自动应用行级权限规则
- ✅ 系统启动时的策略加载机制
- ✅ 缓存优化和性能提升
- ✅ 良好的扩展性和维护性

该方案与现有架构完全兼容，不影响现有功能，可无缝集成到系统中。
