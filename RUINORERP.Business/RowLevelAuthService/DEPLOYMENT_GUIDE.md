# 行级权限系统部署指南

## 概述

本文档描述了如何在RUINORERP系统中部署和配置行级权限功能。

## 已完成的工作

### 1. 核心服务创建

#### 1.1 行级权限策略查询服务
- **接口**: `IRowAuthPolicyQueryService`
- **实现**: `RowAuthPolicyQueryService`
- **功能**: 根据用户ID、角色ID和菜单ID查询对应的行级权限策略
- **生命周期**: 单例（Singleton）

#### 1.2 行级权限策略加载服务
- **接口**: `IRowAuthPolicyLoaderService`
- **实现**: `RowAuthPolicyLoaderService`
- **功能**: 系统启动时预加载所有启用的行级权限策略到缓存
- **生命周期**: 单例（Singleton）

#### 1.3 查询扩展方法
- **文件**: `RowLevelAuthExtensions.cs`
- **功能**: 为SqlSugar查询提供`ApplyRowLevelAuth`扩展方法，自动应用行级权限过滤

### 2. UI辅助类

#### 2.1 JoinType枚举
- **文件**: `RUINORERP.UI/BI/JoinType.cs`
- **功能**: 定义SQL连接类型（INNER、LEFT、RIGHT、FULL JOIN）

#### 2.2 TableType和TableInfo类
- **文件**: `RUINORERP.UI/BI/TableType.cs`
- **功能**: 定义表类型（主表、关联表、视图）和表信息结构

#### 2.3 EntityTypeSelectorHelper
- **文件**: `RUINORERP.UI/BI/EntityTypeSelectorHelper.cs`
- **功能**: 辅助类，用于扫描和获取实体类型信息

### 3. BaseListGeneric集成

#### 3.1 QueryAsync方法增强
- 在`QueryAsync`方法中集成行级权限规则应用
- 支持用户策略和角色策略的自动合并
- 错误处理：如果行级权限获取失败，不影响正常查询

#### 3.2 GetRowAuthPolicies方法
- 新增方法用于获取当前用户的行级权限策略
- 支持按用户、角色、菜单维度查询
- 异常处理：记录错误但不中断查询

### 4. 依赖注入配置

#### 4.1 BusinessDIConfig.cs
在`RegisterRowLevelAuthServices`方法中添加了以下服务注册：

```csharp
// 注册行级权限策略查询服务（单例，提供缓存机制）
builder.RegisterType<RowAuthPolicyQueryService>()
    .As<IRowAuthPolicyQueryService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();

// 注册行级权限策略加载服务（单例，系统启动时加载）
builder.RegisterType<RowAuthPolicyLoaderService>()
    .As<IRowAuthPolicyLoaderService>()
    .AsSelf()
    .SingleInstance()
    .PropertiesAutowired();
```

#### 4.2 Program.cs
在`Main`方法中添加了系统启动时加载行级权限策略的代码：

```csharp
// 加载行级权限策略
try
{
    var rowAuthPolicyLoaderService = Startup.GetFromFac<IRowAuthPolicyLoaderService>();
    if (rowAuthPolicyLoaderService != null)
    {
        await rowAuthPolicyLoaderService.LoadAllPoliciesAsync();
        System.Diagnostics.Debug.WriteLine("[行级权限] 策略加载成功");
    }
    else
    {
        System.Diagnostics.Debug.WriteLine("[警告] 未找到IRowAuthPolicyLoaderService服务，行级权限功能将不可用");
    }
}
catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"[错误] 加载行级权限策略失败: {ex.Message}");
}
```

### 5. 文档

- **IMPLEMENTATION_GUIDE.md**: 完整的实现文档，包含架构设计、API说明、使用示例
- **QUICK_START.md**: 快速开始指南，包含配置步骤和使用示例

## 数据库表

行级权限系统需要以下数据库表：

### 1. tb_RowAuthPolicy（行级权限策略表）
定义行级权限策略，包括：
- PolicyID: 策略ID
- PolicyName: 策略名称
- MenuID: 菜单ID（可选，为null表示适用所有菜单）
- JoinTable: 连接表名
- JoinType: 连接类型（INNER/LEFT/RIGHT/FULL）
- JoinCondition: 连接条件
- FilterCondition: 过滤条件
- IsEnabled: 是否启用
- Priority: 优先级
- Description: 描述

### 2. tb_P4RowAuthPolicyByRole（角色策略关联表）
将策略分配给角色：
- ID: 主键
- RoleID: 角色ID
- PolicyID: 策略ID
- IsEnabled: 是否启用

### 3. tb_P4RowAuthPolicyByUser（用户策略关联表）
将策略分配给用户：
- ID: 主键
- UserID: 用户ID
- PolicyID: 策略ID
- IsEnabled: 是否启用

## 使用示例

### 1. 自动应用行级权限（已集成到BaseListGeneric）

```csharp
// 在继承自BaseListGeneric<T>的窗体中，查询会自动应用行级权限
public class MyForm : BaseListGeneric<MyEntity>
{
    // QueryAsync方法会自动调用GetRowAuthPolicies()获取策略
    // 并通过ApplyRowLevelAuth扩展方法应用到查询中
}
```

### 2. 手动应用行级权限

```csharp
// 获取行级权限策略
var rowAuthPolicies = GetRowAuthPolicies();

// 应用到查询
var list = await db.Queryable<T>()
    .WhereIF(expression != null, expression)
    .ApplyRowLevelAuth(rowAuthPolicies, db)
    .ToListAsync();
```

### 3. 查询用户的权限策略

```csharp
var policyQueryService = appContext.GetFromFac<IRowAuthPolicyQueryService>();

// 查询用户的策略
var userPolicies = await policyQueryService.GetPoliciesByUserAsync(userId, menuId);

// 查询角色的策略
var rolePolicies = await policyQueryService.GetPoliciesByRoleAsync(roleId, menuId);

// 查询用户和角色的策略（合并）
var allPolicies = await policyQueryService.GetPoliciesByUserAndRolesAsync(userId, roleIds, menuId);
```

## 策略配置示例

### 示例1: 按部门过滤订单
```csharp
// 策略名称: 按部门过滤订单
// 菜单: 订单列表
// 连接表: tb_Department
// 连接类型: INNER JOIN
// 连接条件: tb_Order.DepartmentID = tb_Department.DepartmentID
// 过滤条件: tb_Department.DepartmentID = @DepartmentID
// 参数: @DepartmentID = 当前用户所属部门ID
```

### 示例2: 按创建人过滤任务
```csharp
// 策略名称: 按创建人过滤任务
// 菜单: 任务列表
// 连接表: (无需连接表)
// 连接类型: 无
// 连接条件: (空)
// 过滤条件: tb_Task.CreatedBy = @UserID
// 参数: @UserID = 当前用户ID
```

## 注意事项

### 1. 依赖注入
- 确保`IRowAuthPolicyQueryService`和`IRowAuthPolicyLoaderService`已在DI容器中注册
- 已在`BusinessDIConfig.cs`中完成注册

### 2. 系统启动
- 系统启动时会自动调用`LoadAllPoliciesAsync()`预加载策略
- 策略加载失败不会影响系统启动，会记录警告日志

### 3. 策略优先级
- 用户策略优先级高于角色策略
- 相同优先级的策略按`Priority`字段排序

### 4. 错误处理
- 行级权限获取失败时，不会中断查询，会记录错误日志
- 策略加载失败时，不会影响系统启动，会记录警告日志

### 5. 性能优化
- 策略查询服务使用内存缓存，避免频繁查询数据库
- 策略加载服务在系统启动时预加载所有策略
- 缓存会在策略变更时自动刷新

### 6. 扩展性
- 支持自定义参数解析器（实现`IRuleParameterResolver`接口）
- 支持自定义过滤条款提供器（实现`IExpressionFilterClauseProvider`接口）
- 支持多种连接类型（INNER/LEFT/RIGHT/FULL JOIN）

## 下一步工作

1. **智能编辑窗体优化**
   - 连接类型改为下拉选择框
   - 扩展选择范围（基础数据、业务数据、视图等）
   - 角色过滤功能
   - 实时SQL预览

2. **参数解析器增强**
   - 实现更多内置参数（如`@CurrentUser`、`@CurrentDate`等）
   - 支持自定义参数

3. **过滤条款提供器增强**
   - 支持更复杂的过滤条件
   - 支持逻辑运算符（AND、OR）

4. **测试和验证**
   - 单元测试
   - 集成测试
   - 性能测试

## 相关文档

- [IMPLEMENTATION_GUIDE.md](./IMPLEMENTATION_GUIDE.md) - 完整的实现文档
- [QUICK_START.md](./QUICK_START.md) - 快速开始指南

## 联系支持

如有问题或建议，请联系开发团队。
