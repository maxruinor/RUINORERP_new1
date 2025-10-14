# 动态表结构注册说明

## 概述

本文档说明如何使用动态方式注册表结构信息，以减少硬编码并提高系统的灵活性。

## 1. 静态注册方式（当前实现）

当前系统使用静态方式在[CacheInitializationService.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs)的[InitializeAllTableSchemas](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs#L139-L226)方法中注册所有需要缓存的表结构信息。

```csharp
// 示例：静态注册表结构
RegistInformation<tb_Company>(baseCacheDataList, k => k.ID, v => v.CNName);
RegistInformation<tb_Currency>(baseCacheDataList, k => k.Currency_ID, v => v.CurrencyName);
```

## 2. 动态注册方式（建议实现）

可以通过以下方式实现动态注册：

### 2.1 通过配置文件注册

创建一个JSON配置文件来定义需要缓存的表结构：

```json
{
  "CacheableTables": [
    {
      "EntityTypeName": "tb_Company",
      "PrimaryKeyField": "ID",
      "DisplayField": "CNName",
      "IsView": false,
      "IsCacheable": true
    },
    {
      "EntityTypeName": "tb_Currency",
      "PrimaryKeyField": "Currency_ID",
      "DisplayField": "CurrencyName",
      "IsView": false,
      "IsCacheable": true
    }
  ]
}
```

### 2.2 通过数据库注册

创建一个专门的数据库表来存储表结构信息：

```sql
CREATE TABLE CacheTableConfig (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EntityTypeName NVARCHAR(100) NOT NULL,
    PrimaryKeyField NVARCHAR(50) NOT NULL,
    DisplayField NVARCHAR(50) NOT NULL,
    IsView BIT NOT NULL DEFAULT 0,
    IsCacheable BIT NOT NULL DEFAULT 1,
    Description NVARCHAR(200)
);
```

### 2.3 实现动态注册方法

在[CacheInitializationService.cs](file:///E:/CodeRepository/SynologyDrive/RUINORERP/RUINORERP.Business/CommService/CacheInitializationService.cs)中添加动态注册方法：

```csharp
/// <summary>
/// 从配置文件动态注册表结构信息
/// </summary>
/// <param name="configFilePath">配置文件路径</param>
private static void RegisterTableSchemasFromConfig(string configFilePath)
{
    // 实现从配置文件读取并注册表结构的逻辑
}

/// <summary>
/// 从数据库动态注册表结构信息
/// </summary>
/// <param name="db">数据库客户端</param>
private static void RegisterTableSchemasFromDatabase(ISqlSugarClient db)
{
    // 实现从数据库读取并注册表结构的逻辑
}
```

## 3. 使用建议

1. **混合方式**：可以保留部分核心表的静态注册，同时使用动态方式注册可变的表结构
2. **优先级**：动态注册的表结构可以覆盖静态注册的同名表结构
3. **验证机制**：在注册表结构时添加验证机制，确保表结构信息的完整性
4. **缓存机制**：对动态注册的表结构信息进行缓存，避免重复读取配置或数据库

## 4. 注意事项

1. 动态注册需要确保实体类型能够正确加载
2. 需要考虑性能影响，避免在每次缓存操作时都重新读取配置
3. 异常处理：在动态注册过程中需要完善的异常处理机制
4. 日志记录：记录动态注册的过程和结果，便于调试和监控