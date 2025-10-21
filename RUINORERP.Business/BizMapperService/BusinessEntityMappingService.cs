using RUINORERP.Global;
using RUINORERP.Common.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace RUINORERP.Business.BizMapperService
{
    public class BusinessEntityMappingService : IBusinessEntityMappingService
    {
        private readonly ILogger<BusinessEntityMappingService> _logger;
        private readonly EntityInfoConfig _config;
        private bool _initialized = false;
        private readonly object _lock = new object();

        public BusinessEntityMappingService(EntityInfoConfig config, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BusinessEntityMappingService>();
            _config = config;
        }

        /// <summary>
        /// 确保服务已初始化
        /// 如果尚未初始化，则调用配置的注册方法初始化实体映射
        /// </summary>
        private void EnsureInitialized()
        {
            if (!_initialized)
            {
                lock (_lock)
                {
                    if (!_initialized)
                    {
                        try
                        {
                            _config.RegisterCommonMappings();
                            _initialized = true;
                            _logger.LogDebug("BusinessEntityMappingService 初始化完成");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "BusinessEntityMappingService 初始化失败");
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化业务实体映射服务
        /// 此方法仅检查初始化状态，实际的实体映射注册通过EnsureInitialized方法完成
        /// </summary>
        public void Initialize()
        {
            EnsureInitialized();
        }

        public ERPEntityInfo GetEntityInfo(BizType bizType)
        {
            EnsureInitialized();

            if (_config.BizTypeToEntityInfo.TryGetValue(bizType, out var entityInfo))
            {
                return entityInfo;
            }

            var sharedTableEntityInfo = GetSharedTableEntityInfoForBizType(bizType);
            if (sharedTableEntityInfo != null)
            {
                return sharedTableEntityInfo;
            }

            return null;
        }

        private ERPEntityInfo GetSharedTableEntityInfoForBizType(BizType bizType)
        {
            try
            {
                switch (bizType)
                {
                    case BizType.应收款单:
                    case BizType.应付款单:
                        return _config.SharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_ReceivablePayable");

                    case BizType.收款单:
                    case BizType.付款单:
                        return _config.SharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PaymentRecord");

                    case BizType.预收款单:
                    case BizType.预付款单:
                        return _config.SharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PreReceivedPayment");

                    case BizType.收款核销:
                    case BizType.付款核销:
                        return _config.SharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PaymentSettlement");

                    case BizType.其他费用收入:
                    case BizType.其他费用支出:
                        return _config.SharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_OtherExpense");

                    case BizType.销售价格调整单:
                    case BizType.采购价格调整单:
                        return _config.SharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PriceAdjustment");

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查找共用表业务类型对应的实体信息时发生错误: {ErrorMessage}", ex.Message);
                return null;
            }
        }

        public ERPEntityInfo GetEntityInfo(Type entityType)
        {
            EnsureInitialized();

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            if (_config.EntityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                EnsureTableNameMapping(entityInfo);
                if (entityInfo.BizType == BizType.无对应数据)
                {
                    foreach (var item in _config.BizTypeToEntityInfo.Values)
                    {
                        if (item.FullTypeName == entityInfo.FullTypeName)
                        {
                            entityInfo.BizType = item.BizType;
                            break;
                        }
                    }

                }
                return entityInfo;
            }

            _logger.LogDebug("未找到实体类型 {0} 对应的实体信息", entityType.FullName);
            return null;
        }


        public ERPEntityInfo GetEntityInfo(Type entityType, int EnumFlag)
        {
            EnsureInitialized();

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            if (_config.EntityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                EnsureTableNameMapping(entityInfo);
                if (entityInfo.BizType == BizType.无对应数据)
                {
                    foreach (var item in _config.BizTypeToEntityInfo.Values)
                    {
                        if (item.FullTypeName == entityInfo.FullTypeName)
                        {
                            entityInfo.BizType = item.BizType;
                            break;
                        }
                    }
                }
                if (entityInfo.EnumMaper != null && entityInfo.EnumMaper.Count > 0)
                {
                    var maperlist = _config.BizTypeToEntityInfo.Values.Where(c => c.EnumMaper != null);
                    foreach (var item in maperlist)
                    {
                        if (item.FullTypeName == entityInfo.FullTypeName)
                        {
                            foreach (var key in item.EnumMaper)
                            {
                                if (key.Key == EnumFlag && key.Value == entityInfo.BizType)
                                {
                                    return item;
                                }
                            }
                        }
                    }
                }


                return entityInfo;
            }

            _logger.LogDebug("未找到实体类型 {0} 对应的实体信息", entityType.FullName);
            return null;
        }
        public ERPEntityInfo GetEntityInfoByTableName(string tableName)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (_config.TableNameToEntityInfo.TryGetValue(tableName, out var entityInfo))
            {
                return entityInfo;
            }

            entityInfo = _config.SharedTableConfigs.Values
                .FirstOrDefault(info => string.Equals(info.TableName, tableName, StringComparison.OrdinalIgnoreCase));

            if (entityInfo != null)
            {
                _logger.LogDebug("通过共用表配置找到表名对应的实体信息: TableName={0}, EntityType={1}",
                    tableName, entityInfo.EntityType.Name);
                EnsureTableNameMapping(entityInfo);
                return entityInfo;
            }

            if (_config.TableNameToEntityInfo.Count == 0 && _config.EntityTypeToEntityInfo.Count > 0)
            {
                _logger.LogDebug("尝试通过_entityTypeToEntityInfo查找表名 {0} 对应的实体信息", tableName);
                entityInfo = _config.EntityTypeToEntityInfo.Values
                    .FirstOrDefault(info => string.Equals(info.TableName, tableName, StringComparison.OrdinalIgnoreCase));

                if (entityInfo != null)
                {
                    _logger.LogDebug("通过_entityTypeToEntityInfo找到表名对应的实体信息: TableName={0}, EntityType={1}",
                        tableName, entityInfo.EntityType.Name);
                    EnsureTableNameMapping(entityInfo);
                    return entityInfo;
                }
            }

            _logger.LogDebug("未找到表名 {0} 对应的实体信息", tableName);
            return null;
        }

        public IEnumerable<ERPEntityInfo> GetAllEntityInfos()
        {
            EnsureInitialized();

            var allEntityInfos = _config.BizTypeToEntityInfo.Values
                .Concat(_config.SharedTableConfigs.Values)
                .GroupBy(info => new { Type = info.EntityType?.AssemblyQualifiedName, Table = info.TableName })
                .Select(g => g.First())
                .ToList();

            foreach (var entityInfo in allEntityInfos)
            {
                EnsureTableNameMapping(entityInfo);
            }

            return allEntityInfos;
        }

        public Type GetEntityType(BizType bizType)
        {
            EnsureInitialized();

            var entityInfo = GetEntityInfo(bizType);
            return entityInfo?.EntityType;
        }

        public string GetTableNameByType(Type entityType)
        {
            EnsureInitialized();

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            var entityInfo = GetEntityInfo(entityType);
            return entityInfo?.TableName;
        }

        public BizType GetBizType(Type entityType, object entity = null)
        {
            EnsureInitialized();

            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));

            if (_config.EntityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                return entityInfo.BizType;
            }

            if (_config.SharedTableConfigs.TryGetValue(entityType, out var sharedConfig))
            {
                if (sharedConfig.TypeResolver != null && entity != null)
                {
                    try
                    {
                        var discriminatorValue = entity.GetPropertyValue(sharedConfig.DiscriminatorField);
                        var resolvedBizType = sharedConfig.TypeResolver(discriminatorValue);

                        _logger.LogDebug("通过共用表鉴别器解析业务类型: EntityType={0}, DiscriminatorField={1}, DiscriminatorValue={2}, ResolvedBizType={3}",
                            entityType.Name, sharedConfig.DiscriminatorField, discriminatorValue, resolvedBizType);

                        return resolvedBizType;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "获取共用表业务类型失败: EntityType={0}, DiscriminatorField={1}",
                            entityType.Name, sharedConfig.DiscriminatorField);
                    }
                }

                _logger.LogDebug("使用共用表实体的默认业务类型: EntityType={0}, DefaultBizType={1}",
                    entityType.Name, sharedConfig.BizType);

                return sharedConfig.BizType != BizType.无对应数据 ? sharedConfig.BizType : BizType.无对应数据;
            }

            _logger.LogDebug("未找到实体类型 {0} 对应的实体信息或业务类型", entityType.FullName);
            return BizType.无对应数据;
        }

        public BizType GetBizTypeByEntity(object entity)
        {
            EnsureInitialized();

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityType = entity.GetType();
            return GetBizType(entityType, entity);
        }

        public Type GetEntityTypeByTableName(string tableName)
        {
            EnsureInitialized();

            var entityInfo = GetEntityInfoByTableName(tableName);
            return entityInfo?.EntityType;
        }

        private void EnsureTableNameMapping(ERPEntityInfo entityInfo)
        {
            if (entityInfo == null || string.IsNullOrEmpty(entityInfo.TableName))
                return;

            if (!_config.TableNameToEntityInfo.ContainsKey(entityInfo.TableName))
            {
                _config.TableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
                _logger.LogDebug("添加表名到实体信息的映射: TableName={0}, EntityType={1}",
                    entityInfo.TableName, entityInfo.EntityType.Name);
            }
        }


        public bool IsRegistered(BizType bizType)
        {
            EnsureInitialized();

            return _config.BizTypeToEntityInfo.ContainsKey(bizType);
        }

        public bool IsRegistered(Type entityType)
        {
            EnsureInitialized();

            if (entityType == null)
                return false;

            return _config.EntityTypeToEntityInfo.ContainsKey(entityType) ||
                   _config.SharedTableConfigs.ContainsKey(entityType);
        }

        public bool IsRegisteredByTableName(string tableName)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(tableName))
                return false;

            return _config.TableNameToEntityInfo.ContainsKey(tableName);
        }


        public (long Id, string Name) GetIdAndName(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityType = entity.GetType();
            var entityInfo = GetEntityInfo(entityType);

            if (entityInfo == null)
            {
                if (_config.SharedTableConfigs.TryGetValue(entityType, out entityInfo))
                {
                    var bizType = GetBizType(entityType, entity);
                    entityInfo = GetEntityInfo(bizType);
                }
            }

            if (entityInfo == null)
            {
                _logger.LogDebug("未找到实体类型 {0} 对应的实体信息", entityType.FullName);
                return (0, null);
            }

            try
            {
                object idValue = null;
                if (!string.IsNullOrEmpty(entityInfo.IdField))
                {
                    idValue = entity.GetPropertyValue(entityInfo.IdField);
                }

                object nameValue = null;
                if (!string.IsNullOrEmpty(entityInfo.DescriptionField))
                {
                    nameValue = entity.GetPropertyValue(entityInfo.DescriptionField);
                }
                else if (!string.IsNullOrEmpty(entityInfo.NoField))
                {
                    nameValue = entity.GetPropertyValue(entityInfo.NoField);
                }

                return (idValue.ToLong(), nameValue?.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取实体对象的ID和名称值时发生错误");
                return (0, null);
            }
        }







    }
}