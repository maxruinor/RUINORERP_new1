# 实体业务映射服务注册机制

## 概述

本模块提供了一个统一的服务注册契约机制，用于优化和标准化Autofac容器中的服务注册流程，特别是针对`IEntityBizMappingService`及相关服务的注册。

## 为什么需要优化服务注册

原有的`ConfigureContainerForDll`方法存在以下问题：
- 代码冗长且硬编码严重
- 缺乏统一的服务注册规范
- 难以维护和扩展
- 针对不同类型的服务注册逻辑混杂在一起

## 新的服务注册机制

新的机制基于以下核心组件：

### 1. 服务注册契约接口 (`IServiceRegistrationContract`)

定义了服务注册的标准接口，所有需要注册服务的模块都应该实现这个接口。

```csharp
public interface IServiceRegistrationContract
{
    void Register(ContainerBuilder builder);
}
```

### 2. 实体业务映射服务注册实现 (`EntityBizMappingServiceRegistration`)

实现了`IServiceRegistrationContract`接口，专门负责注册实体业务映射相关的服务。

### 3. 旧体系实体映射注册器 (`LegacyEntityMappingRegistration`)

实现了`IServiceRegistrationContract`接口，负责将旧的`EntityBizMappingService`中的映射规则迁移到新的实体信息服务体系。

### 4. 服务注册管理器 (`ServiceRegistrationManager`)

提供了自动发现和执行所有`IServiceRegistrationContract`实现的功能。

### 5. Autofac模块 (`ServiceRegistrationModule`)

集成了服务注册管理器到Autofac的模块系统中，便于在Startup.cs中使用。

## 旧体系实体映射迁移机制

为了实现新体系和旧体系的完全合并，我们提供了专门的旧体系实体映射迁移机制。

### 为什么需要迁移

原有的`EntityBizMappingService`包含了大量的实体映射规则，但这些规则没有被迁移到新的`IEntityInfoService`体系中。

### 迁移原理

迁移机制基于以下核心组件：

1. **`LegacyEntityMappingRegistration`** - 负责注册相关服务并触发迁移过程
2. **`LegacyEntityMappingMigrationHelper`** - 提供具体的迁移逻辑，将旧的映射规则转换为新的注册方式

### 迁移过程

1. 保留原有的`EntityBizMappingService`以确保向后兼容
2. 在应用启动时，自动将旧的映射规则迁移到新的`IEntityInfoService`体系中
3. 使用特殊的日志记录迁移过程，便于追踪和调试
4. 采用优雅的错误处理，确保即使部分迁移失败，也不会影响整体应用启动

### 迁移的映射类型

迁移机制支持两种类型的实体映射：

1. **普通实体映射** - 如销售订单、采购订单等
2. **共用表实体映射** - 如收款/付款单、预收款/预付款单等

## 使用方法

### 在Startup.cs中使用

新的服务注册机制已经集成到`ConfigureContainerForDll`方法中，它会：

1. 尝试加载并使用`ServiceRegistrationModule`
2. 如果失败，会使用降级方案直接注册必要的服务

### 添加新的服务注册器

要添加新的服务注册器，只需：

1. 创建一个实现`IServiceRegistrationContract`接口的类
2. 在`Register`方法中添加你的服务注册逻辑
3. 该类会被自动发现并执行

## 代码示例

### 旧体系实体映射迁移示例

```csharp
// LegacyEntityMappingRegistration类负责触发迁移过程
public class LegacyEntityMappingRegistration : IServiceRegistrationContract
{
    public void Register(ContainerBuilder builder)
    {
        // 注册实体信息服务
        builder.RegisterType<EntityInfoServiceImpl>()
            .As<IEntityInfoService>()
            .SingleInstance();

        // 注册旧体系的实体业务映射服务（保留向后兼容）
        builder.RegisterType<EntityBizMappingService>()
            .As<IEntityBizMappingService>()
            .SingleInstance();

        // 在应用启动时执行旧体系到新体系的映射迁移
        builder.RegisterBuildCallback(container =>
        {
            MigrateLegacyMappingsToNewSystem(container);
        });
    }
}
```

### 注册普通实体映射示例

```csharp
// 在LegacyEntityMappingMigrationHelper类中
private void MigrateCommonEntityMappings()
{
    // 销售订单
    _entityInfoService.RegisterEntity<tb_SaleOrder>(BizType.销售订单, builder =>
    {
        builder.WithTableName("tb_SaleOrder")
               .WithDescription("销售订单")
               .WithIdField(e => e.SOrder_ID)
               .WithNoField(e => e.SOrderNo)
               .WithDetailProperty(e => e.tb_SaleOrderDetails);
    });
    
    // 其他实体映射...
}
```

### 注册共用表实体映射示例

```csharp
// 在LegacyEntityMappingMigrationHelper类中
private void MigrateSharedTableMappings()
{
    // 收款/付款单
    _entityInfoService.RegisterSharedTable<tb_FM_PaymentRecord, int>(
        typeResolver: value => value == (int)ReceivePaymentType.收款 ? BizType.收款单 : BizType.付款单,
        configure: builder =>
        {
            builder.WithTableName("tb_FM_PaymentRecord")
                   .WithDescription("收付款记录单")
                   .WithIdField(e => e.PaymentId)
                   .WithNoField(e => e.PaymentNo)
                   .WithDetailProperty(e => e.tb_FM_PaymentRecordDetails)
                   .WithDiscriminator(e => e.ReceivePaymentType, type =>
                   {
                       return type == (int)ReceivePaymentType.收款 ? BizType.收款单 : BizType.付款单;
                   });
        });
    
    // 其他共用表映射...
}
```

### 创建新的服务注册器

```csharp
public class MyServiceRegistration : IServiceRegistrationContract
{
    public void Register(ContainerBuilder builder)
    {
        // 注册你的服务
        builder.RegisterType<MyService>().As<IMyService>().SingleInstance();
    }
}
```

### 直接使用服务注册管理器

```csharp
// 注册所有实现了IServiceRegistrationContract的服务注册器
ServiceRegistrationManager.RegisterAllServices(builder);

// 注册特定类型的服务注册器
ServiceRegistrationManager.RegisterService<MyServiceRegistration>(builder);
```

## 优势

1. **统一的注册规范**：所有服务注册都遵循相同的接口
2. **更好的代码组织**：服务注册逻辑被分散到专门的类中
3. **易于扩展**：添加新的服务注册器非常简单
4. **向后兼容**：提供了降级方案确保原有功能不受影响
5. **可维护性**：代码更清晰、更易于理解和维护
6. **完整的迁移机制**：旧体系的所有映射规则都被保留并迁移到新体系
7. **可追踪性**：详细的日志记录使迁移过程可追踪和调试
8. **可靠性**：优雅的错误处理确保即使部分迁移失败也不会影响应用启动

## 注意事项

1. 确保所有实现`IServiceRegistrationContract`的类都是可实例化的（非抽象类）
2. 服务注册器之间的依赖关系需要谨慎处理
3. 在开发环境中，建议查看控制台输出以确认服务注册是否成功