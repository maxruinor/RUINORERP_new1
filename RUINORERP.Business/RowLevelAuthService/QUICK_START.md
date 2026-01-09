# 行级权限规则 - 快速开始指南

## 概述

本指南将帮助您快速上手使用行级权限规则功能。行级权限规则允许您控制用户可以访问哪些数据行，而不仅仅是控制他们可以访问哪些菜单或功能。

## 核心概念

### 1. 行级权限策略 (tb_RowAuthPolicy)
定义对特定实体类型的数据过滤规则。

### 2. 策略与角色关联 (tb_P4RowAuthPolicyByRole)
将权限策略分配给特定角色，适用于该角色的所有用户。

### 3. 策略与用户关联 (tb_P4RowAuthPolicyByUser)
将权限策略分配给特定用户，用户策略优先级高于角色策略。

## 快速开始

### 步骤1：配置依赖注入

在`Startup.cs`或相关的依赖注入配置中注册服务：

```csharp
// 注册行级权限策略查询服务
services.AddSingleton<IRowAuthPolicyQueryService, RowAuthPolicyQueryService>();

// 注册行级权限策略加载服务
services.AddSingleton<IRowAuthPolicyLoaderService, RowAuthPolicyLoaderService>();
```

### 步骤2：系统启动时加载策略

在系统初始化代码中添加：

```csharp
// 加载行级权限策略
var policyLoaderService = appContext.GetFromFac<IRowAuthPolicyLoaderService>();
if (policyLoaderService != null)
{
    await policyLoaderService.LoadAllPoliciesAsync();
}
```

### 步骤3：创建权限策略

使用智能编辑窗体创建权限策略：

1. 打开"数据权限规则"菜单
2. 点击"添加"按钮
3. 填写以下信息：
   - **策略名称**：例如"限制只能查看A类产品"
   - **实体类型**：选择或输入实体类型，如"RUINORERP.Model.tb_Prod"
   - **是否启用**：勾选
   - **过滤条件**：输入过滤SQL，如"ProductType_ID = 1"
4. 保存策略

### 步骤4：为角色分配策略

1. 在角色管理界面，找到需要分配策略的角色
2. 配置角色与策略的关联
3. 指定适用菜单（可选）
4. 保存配置

### 步骤5：验证权限配置

1. 使用配置了权限策略的用户登录系统
2. 进入相应的菜单（如产品管理）
3. 查看数据列表，应该只能看到符合过滤条件的数据

## 常见使用场景

### 场景1：按产品类型限制访问

**需求**：销售员只能查看自己负责的产品类型。

**实现**：
```csharp
// 创建策略
var policy = new tb_RowAuthPolicy
{
    PolicyName = "销售员产品类型限制",
    EntityType = "RUINORERP.Model.tb_Prod",
    FilterClause = "ProductType_ID = @ProductType",
    IsParameterized = true,
    ParameterizedFilterClause = "ProductType_ID = 1" // 示例值
};

// 为销售员角色分配策略
var policyByRole = new tb_P4RowAuthPolicyByRole
{
    PolicyId = policy.PolicyId,
    RoleID = 2, // 销售员角色ID
    MenuID = 100 // 产品管理菜单ID
};
```

### 场景2：按地区限制访问

**需求**：不同地区的用户只能查看本地区的数据。

**实现**：
```csharp
// 创建策略
var policy = new tb_RowAuthPolicy
{
    PolicyName = "地区数据限制",
    EntityType = "RUINORERP.Model.tb_Location",
    FilterClause = "Region_ID = @RegionID",
    IsParameterized = true,
    ParameterizedFilterClause = "Region_ID = @CurrentUser.RegionID"
};

// 为特定地区的用户分配策略
var policyByUser = new tb_P4RowAuthPolicyByUser
{
    PolicyId = policy.PolicyId,
    User_ID = 100, // 用户ID
    MenuID = 200 // 地区管理菜单ID
};
```

### 场景3：联表过滤

**需求**：通过联表实现复杂的数据过滤。

**实现**：
```csharp
// 创建需要联表的策略
var policy = new tb_RowAuthPolicy
{
    PolicyName = "通过部门联表过滤",
    EntityType = "RUINORERP.Model.tb_UserInfo",
    IsJoinRequired = true,
    JoinTable = "tb_Department",
    JoinType = "LEFT",
    JoinField = "Dept_ID",
    TargetField = "Dept_ID",
    JoinOnClause = "jt.DepartmentType = '销售部'",
    FilterClause = "jt.DepartmentType = '销售部'",
    IsEnabled = true
};
```

## 策略优先级

当用户同时具有角色策略和用户策略时：

1. **用户策略优先级最高**
2. **角色策略次之**
3. **相同实体类型的策略会合并**

## 参数化过滤

使用参数化过滤可以提高灵活性和安全性：

```csharp
// 策略定义
var policy = new tb_RowAuthPolicy
{
    PolicyName = "参数化过滤示例",
    EntityType = "RUINORERP.Model.tb_Prod",
    IsParameterized = true,
    ParameterizedFilterClause = "ProductType_ID = @ProductType AND Status = @Status"
};

// 运行时替换参数
var parameterResolver = new DefaultRuleParameterResolver();
var resolvedClause = await parameterResolver.ResolveParametersAsync(
    policy.ParameterizedFilterClause,
    new Dictionary<string, object>
    {
        { "ProductType", 1 },
        { "Status", "有效" }
    }
);
```

## 调试技巧

### 1. 查看策略应用情况

在日志中查找以下关键字：
- "已为实体 X 应用 N 条行级权限规则"
- "获取行级权限策略失败"

### 2. 验证策略是否生效

```csharp
// 在查询前记录策略数量
var policies = GetRowAuthPolicies();
Console.WriteLine($"当前应用了 {policies.Count} 条行级权限策略");

// 查看实际执行的SQL
// 在QueryAsync中添加日志记录
```

### 3. 测试不同用户的权限

1. 使用管理员账号查看全部数据
2. 使用普通账号查看受限数据
3. 对比两个账号看到的数据差异

## 常见问题

### Q1: 为什么我的策略没有生效？

**A**: 检查以下几点：
1. 策略是否设置为启用状态
2. 策略是否正确分配给用户或角色
3. 实体类型名称是否正确
4. 过滤条件SQL语法是否正确
5. 用户是否在正确的菜单中

### Q2: 如何调试策略的SQL？

**A**: 使用SQL预览功能：
1. 在智能编辑窗体中点击"生成过滤条件"
2. 查看生成的SQL
3. 可以复制SQL到数据库客户端测试

### Q3: 用户策略和角色策略如何优先级？

**A**: 用户策略优先级高于角色策略。如果用户同时具有用户策略和角色策略，会优先使用用户策略，然后合并角色策略（去重）。

### Q4: 如何清除策略缓存？

**A**:
```csharp
var policyQueryService = appContext.GetFromFac<IRowAuthPolicyQueryService>();
policyQueryService.ClearPolicyCache();
```

### Q5: 支持哪些连接类型？

**A**: 支持以下连接类型：
- INNER JOIN: 内连接
- LEFT JOIN: 左连接
- RIGHT JOIN: 右连接
- FULL JOIN: 全连接
- None: 不需要联表

## 性能优化建议

### 1. 合理使用缓存
- 系统启动时预加载策略
- 策略变更时刷新缓存
- 避免频繁查询数据库

### 2. 简化过滤条件
- 避免复杂的子查询
- 使用索引字段进行过滤
- 优先使用参数化过滤

### 3. 合理设计策略
- 一个实体类型不要配置过多策略
- 合并相似的策略
- 避免策略冲突

## 下一步

1. 阅读[IMPLEMENTATION_GUIDE.md](IMPLEMENTATION_GUIDE.md)了解详细实现
2. 查看[README.md](README.md)了解整体架构
3. 查看示例代码学习更多使用场景

## 技术支持

如有问题，请联系开发团队或查看项目文档。
