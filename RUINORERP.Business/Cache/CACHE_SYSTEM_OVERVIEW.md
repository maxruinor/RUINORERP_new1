# RUINORERP 统一缓存系统概述

本文档描述了 RUINORERP 系统中的统一缓存管理架构，旨在提供高效、一致的数据缓存机制，支持客户端和服务器端的缓存同步。系统现在采用全新的缓存管理体系，分为缓存管理和缓存同步两个核心部分，以实现更好的可维护性和扩展性。

## 1. 核心架构

### 1.1 缓存管理体系

缓存管理负责数据的存储、检索和基本操作，主要组件包括：

- **IEntityCacheManager 接口**：缓存系统的核心接口，定义了实体缓存的基本操作方法
- **EntityCacheManager 类**：IEntityCacheManager 的主要实现，提供完整的缓存管理功能
- **TableSchemaManager 类**：管理实体表结构信息，支持缓存键生成和实体类型查找
- **EventDrivenCacheManager 类**：事件驱动的缓存管理器，负责处理缓存变更事件和同步
- **SqlSugarCacheDataProvider 类**：基于 SqlSugar 的缓存数据提供者实现

### 1.2 缓存同步体系

缓存同步负责维护缓存元数据和保证多端数据一致性，主要组件包括：

- **ICacheSyncMetadata 接口**：定义缓存同步元数据的管理方法
- **CacheSyncMetadataManager 类**：ICacheSyncMetadata 的实现，管理缓存同步元数据
- **CacheSyncInfo 类**：缓存同步信息实体，包含表名、数据量、估计大小和过期时间等信息
- **CacheSyncExtensions 类**：提供缓存同步相关的扩展方法
- **CacheSubscriptionManager 类**：管理缓存订阅关系，支持服务器端和客户端的缓存变更通知

## 2. 关键组件详解

### 2.1 IEntityCacheManager 接口

定义了缓存系统的核心操作方法：

- 缓存查询（GetEntityList, GetEntity, GetDisplayValue）
- 缓存更新（UpdateEntityList, UpdateEntity）
- 缓存删除（DeleteEntity, DeleteEntityList, DeleteEntityList(string tableName), DeleteEntities）
- 缓存初始化（InitializeTableSchema）
- 缓存键生成（GenerateCacheKey）
- 数据序列化/反序列化（SerializeCacheData, DeserializeCacheData）

### 2.2 EntityCacheManager 类

提供了完整的缓存管理功能：

- 使用 ConcurrentDictionary 存储缓存数据，确保线程安全
- 集成了表结构信息管理（通过 TableSchemaManager）
- 提供缓存统计功能（缓存命中率、未命中率等）
- 支持缓存键格式化和标准化
- 实现数据序列化和反序列化
- 支持自动更新缓存同步元数据

### 2.3 ICacheSyncMetadata 接口

定义了缓存同步元数据的管理方法：

- **GetTableSyncInfo**：获取表的缓存同步信息
- **UpdateTableSyncInfo**：更新表的缓存同步信息（接收数据数量和估计大小）
- **SetTableExpiration**：设置表的缓存过期时间
- **IsTableExpired**：检查表的缓存是否过期
- **GetAllTableSyncInfo**：获取所有表的缓存同步元数据
- **RemoveTableSyncInfo**：从同步元数据中移除指定表
- **CleanupExpiredSyncInfo**：清理过期的缓存同步元数据

### 2.4 CacheSyncMetadataManager 类

实现了 ICacheSyncMetadata 接口：

- 使用 ConcurrentDictionary 存储缓存同步元数据
- 提供线程安全的元数据访问和更新方法
- 支持元数据克隆和验证
- 实现详细的日志记录和异常处理
- 自动管理缓存过期时间

### 2.5 CacheSyncInfo 类

缓存同步信息实体，包含：

- **TableName**：表名
- **DataCount**：数据数量
- **EstimatedSize**：估计大小（字节）
- **LastUpdateTime**：最后更新时间
- **ExpirationTime**：过期时间
- **HasExpiration**：是否设置了过期时间
- **SourceInfo**：源信息（用于存储额外的源数据信息）

### 2.6 CacheSyncExtensions 类

提供缓存同步相关的扩展方法：

- **UpdateEntityListWithSync**：更新实体列表并同步元数据（两个重载版本）
- **UpdateEntityListWithSync<T>**：泛型版本，支持直接传递实体列表
- **UpdateEntityListWithSync**：非泛型版本，支持通过表名更新
- 所有扩展方法都正确地更新同步元数据和设置过期时间

### 2.7 CacheSubscriptionManager 类

管理缓存订阅关系：

- 维护表与订阅者的映射关系
- 提供订阅和取消订阅方法
- 支持获取特定表的所有订阅者
- 在客户端和服务器端共享相同的订阅管理逻辑

## 3. 网络通信组件

### 3.1 CacheCommandHandler

服务器端缓存命令处理器：

- 处理缓存操作命令（获取、设置、更新、删除）
- 处理缓存同步命令
- 管理缓存订阅关系
- 广播缓存变更到订阅的客户端
- 直接使用 PrimaryKeyName 和 PrimaryKeyValue 属性进行缓存操作
- 移除了对 Parameters 字典的依赖，简化了代码逻辑

### 3.2 CacheClientService

客户端缓存服务：

- 与服务器通信，发送缓存请求
- 处理服务器返回的缓存响应
- 处理缓存变更通知
- 管理本地缓存订阅
- 使用新的缓存管理器直接更新本地缓存
- 优化了缓存请求和响应处理逻辑
- 实现了基于缓存状态的智能请求机制，避免重复请求
- 通过检查本地缓存有效性，智能决定是否需要从服务器获取数据

## 4. 设计原则

### 4.1 依赖注入

缓存系统充分利用依赖注入模式，所有主要组件都设计为可注入服务：

- IEntityCacheManager 作为核心服务注入到需要缓存功能的组件中
- ICacheSyncMetadata 作为同步元数据服务注入
- 日志记录器 (ILogger) 注入以支持详细的操作日志
- 其他辅助服务按需注入，提高系统的可测试性和可维护性

### 4.2 职责分离

系统采用明确的职责分离原则：

- 缓存管理器负责数据的存储、检索和基本操作
- 同步元数据管理器负责缓存同步元数据的管理
- 网络组件负责多端通信和数据同步
- 订阅管理器专门处理缓存订阅关系

### 4.3 线程安全

所有缓存操作都设计为线程安全的：

- 使用 ConcurrentDictionary 作为主要缓存容器
- 关键操作使用适当的锁机制保护
- 异步操作设计考虑并发访问场景

## 5. 缓存同步流程

### 5.1 服务器端同步流程

1. 服务器更新缓存数据时，同时更新对应的缓存同步元数据
2. 检查是否有客户端订阅了该表的缓存变更
3. 如有订阅，向所有订阅的客户端广播缓存变更通知
4. 使用新的缓存管理器直接操作缓存，确保数据一致性

### 5.2 客户端同步流程

1. 客户端向服务器请求缓存数据并订阅该表的变更通知
2. 服务器返回缓存数据，客户端更新本地缓存
3. 当有其他客户端修改数据时，服务器向所有订阅的客户端广播变更通知
4. 客户端收到通知后，使用新的缓存管理器更新本地缓存以保持一致

## 6. 使用指南

### 6.1 基本缓存操作

```csharp
// 获取实体列表缓存
var entityList = _cacheManager.GetEntityList<MyEntity>();

// 更新实体列表缓存
_cacheManager.UpdateEntityList(entityList);

// 删除实体缓存
_cacheManager.DeleteEntity<MyEntity>(idValue);

// 删除整个表的缓存
_cacheManager.DeleteEntityList<MyEntity>();
```

### 6.2 缓存同步操作

```csharp
// 更新实体列表并同步元数据
_cacheManager.UpdateEntityListWithSync(entityList);

// 或者使用表名版本
_cacheManager.UpdateEntityListWithSync("MyEntity", entityList);

// 检查缓存是否需要同步
bool isExpired = _cacheSyncMetadata.IsTableExpired("MyEntity");

// 获取同步元数据
var syncInfo = _cacheSyncMetadata.GetTableSyncInfo("MyEntity");
```

### 6.3 缓存订阅操作

```csharp
// 订阅表的缓存变更
_subscriptionManager.Subscribe(sessionId, "MyEntity");

// 取消订阅
_subscriptionManager.Unsubscribe(sessionId, "MyEntity");

// 获取订阅了特定表的所有会话
var subscribers = _subscriptionManager.GetSubscribers("MyEntity");
```

## 7. 缓存系统初始化

在应用启动时，需要初始化缓存系统的核心服务：

```csharp
// 注册缓存服务
services.AddSingleton<IEntityCacheManager, EntityCacheManager>();
services.AddSingleton<ICacheSyncMetadata, CacheSyncMetadataManager>();
services.AddSingleton<CacheSubscriptionManager>();
services.AddSingleton<EventDrivenCacheManager>();

// 初始化表结构信息
var cacheManager = services.GetRequiredService<IEntityCacheManager>();
cacheManager.InitializeTableSchema<MyEntity>(
    e => e.Id,            // 主键表达式
    e => e.DisplayName,   // 显示字段表达式
    isView: false,        // 是否为视图
    isCacheable: true,    // 是否可缓存
    description: "我的实体" // 描述
);
```

## 8. 最佳实践

### 8.1 缓存键管理

- 使用 GenerateCacheKey 方法生成统一格式的缓存键
- 避免手动构造缓存键，以确保一致性

### 8.2 缓存更新策略

- 对于频繁变更的数据，考虑使用较短的缓存过期时间
- 对于相对稳定的数据，可以使用较长的缓存过期时间
- 关键业务数据更新后应立即同步到缓存和元数据
- 使用 UpdateEntityListWithSync 方法确保缓存和元数据一致性

### 8.3 性能优化

- 避免在高频调用路径上进行大量的缓存操作
- 合理设置缓存粒度，避免过度缓存或缓存不足
- 使用批量操作减少缓存访问次数
- 利用缓存同步元数据避免不必要的数据传输
- 仅订阅真正需要实时更新的表
- 利用CacheClientService的智能请求机制避免重复请求
- 在请求缓存前先检查本地缓存有效性，减少网络请求

## 9. 注意事项

- 缓存系统设计为内存缓存，应用重启后缓存会丢失
- 确保缓存操作的线程安全性，特别是在并发场景下
- 缓存同步依赖于网络连接，网络不稳定可能导致缓存不一致
- 对于大量数据的缓存，注意内存使用情况，避免内存溢出
- 当前系统新旧缓存体系同时存在，测试稳定后将删除旧体系
- 所有缓存操作现在都使用新的缓存管理器和同步元数据管理器
- 缓存更新时确保同时更新同步元数据以保持一致性
- 使用DeleteEntityList(string tableName)非泛型重载方法时，需确保表名的正确性和一致性