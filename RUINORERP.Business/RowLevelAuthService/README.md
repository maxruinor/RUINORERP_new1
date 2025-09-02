# 行级数据权限控制模块

## 概述

本模块提供了一个完整的行级数据权限控制解决方案，允许系统根据用户角色和配置的权限策略，精确控制用户可以访问和操作的数据行。

## 优化内容

本模块已经过全面优化，主要改进包括：

1. **统一命名空间**
   - 所有相关类和接口都使用`RUINORERP.Business.RowLevelAuthService`命名空间
   - 移除了不一致的命名空间引用

2. **改进代码规范和排版**
   - 严格按照C#代码规范格式化所有文件
   - 添加了详细的XML文档注释
   - 优化了代码结构和缩进

3. **增强错误处理机制**
   - 添加了参数验证和异常处理
   - 改进了日志记录功能
   - 实现了优雅的错误降级

4. **完善缓存管理**
   - 添加了内存缓存支持，提高性能
   - 实现了完整的缓存清理机制
   - 优化了缓存键的生成逻辑

5. **依赖注入优化**
   - 添加了构造函数参数验证
   - 确保所有依赖都正确初始化

## 核心组件

### 1. 接口定义

#### IDefaultRowAuthRuleProvider
提供默认规则选项和基于默认规则创建权限策略的功能。

```csharp
public interface IDefaultRowAuthRuleProvider
{
    List<DefaultRuleOption> GetDefaultRuleOptions(BizType bizType);
    tb_RowAuthPolicy CreatePolicyFromDefaultOption(BizType bizType, DefaultRuleOption option, long roleId);
}
```

#### IRowAuthService
提供行级数据权限控制的核心功能。

```csharp
public interface IRowAuthService
{
    string GetUserRowAuthFilterClause(Type entityType, string menuId = "");
    RowAuthConfigDto GetRowAuthConfig(long roleId, BizType bizType);
    bool SaveRowAuthConfig(RowAuthConfigDto config);
    List<tb_RowAuthPolicy> GetAllPolicies();
    bool AssignPolicyToRole(long policyId, long roleId);
    bool RemovePolicyFromRole(long policyId, long roleId);
}
```

### 2. 数据模型

#### DefaultRuleOption
表示系统预定义的行级权限规则选项。

```csharp
public class DefaultRuleOption
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

#### RowAuthConfigDto
用于传输行级权限配置信息。

```csharp
public class RowAuthConfigDto
{
    public long RoleId { get; set; }
    public BizType BizType { get; set; }
    public List<DefaultRuleOption> AvailableDefaultOptions { get; set; }
    public List<tb_RowAuthPolicy> AssignedPolicies { get; set; }
    public tb_RowAuthPolicy NewCustomPolicy { get; set; }
}
```

### 3. 实现类

#### DefaultRowAuthRuleProvider
提供基于业务类型的默认规则选项和创建权限策略的功能。

#### RowAuthService
实现行级数据权限控制的核心功能，包括权限策略管理和过滤条件生成。

## 使用方法

### 获取行级权限配置

```csharp
// 注入IRowAuthService
private readonly IRowAuthService _rowAuthService;

// 获取指定角色和业务类型的权限配置
var config = _rowAuthService.GetRowAuthConfig(roleId, BizType.销售订单);
```

### 保存行级权限配置

```csharp
// 创建或修改权限配置
var config = new RowAuthConfigDto
{
    RoleId = roleId,
    BizType = BizType.销售订单,
    // 设置其他属性...
};

// 保存配置
bool success = _rowAuthService.SaveRowAuthConfig(config);
```

### 应用行级权限过滤

```csharp
// 获取当前用户对特定实体的过滤条件（不指定菜单ID）
string filterClause = _rowAuthService.GetUserRowAuthFilterClause(typeof(tb_SalesOrder));

// 或者指定菜单ID以获取特定功能的数据权限
string filterClause = _rowAuthService.GetUserRowAuthFilterClause(typeof(tb_SalesOrder), "SalesOrderList");

// 将过滤条件应用到查询中
var query = _db.Queryable<tb_SalesOrder>();
if (!string.IsNullOrEmpty(filterClause))
{
    query = query.Where(filterClause);
}

var orders = query.ToList();
```

### 管理权限策略

```csharp
// 获取所有权限策略
var policies = _rowAuthService.GetAllPolicies();

// 为角色分配策略
bool assigned = _rowAuthService.AssignPolicyToRole(policyId, roleId);

// 从角色移除策略
bool removed = _rowAuthService.RemovePolicyFromRole(policyId, roleId);
```

## 技术特点

1. **基于角色的权限控制**：通过角色分配权限策略，实现灵活的权限管理
2. **缓存优化**：使用内存缓存提高频繁查询的性能
3. **SQL过滤条件生成**：根据权限策略动态生成SQL过滤条件
4. **默认规则模板**：提供常用的权限规则模板，简化配置过程
5. **多对多关系支持**：支持一个角色拥有多个策略，一个策略分配给多个角色

## 注意事项

1. 确保所有实现了相关接口的服务都已正确注册到依赖注入容器
2. 权限策略变更后，相关缓存会自动清理，但可能有最多30分钟的延迟
3. 对于复杂的权限需求，可以创建自定义的权限策略
4. 在高并发环境下，建议适当调整缓存过期时间
5. 请确保数据库表结构中包含必要的关联字段，如RoleId、UserId等

## 常见问题

### 如何添加新的默认规则选项？
修改`DefaultRowAuthRuleProvider`类中的`GetDefaultRuleOptions`方法，为特定业务类型添加新的规则选项。

### 如何自定义权限过滤条件？
可以通过`SaveRowAuthConfig`方法，设置`NewCustomPolicy`属性来创建自定义的权限策略。

### 缓存清理机制是如何工作的？
当权限配置变更时，系统会自动清理相关的用户缓存。缓存过期时间默认为30分钟。