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
    public class EntityInfoService : IEntityInfoService
    {
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<BizType, ERPEntityInfo> _bizTypeToEntityInfo = new ConcurrentDictionary<BizType, ERPEntityInfo>();
        private readonly ConcurrentDictionary<Type, ERPEntityInfo> _entityTypeToEntityInfo = new ConcurrentDictionary<Type, ERPEntityInfo>();
        private readonly ConcurrentDictionary<string, ERPEntityInfo> _tableNameToEntityInfo = new ConcurrentDictionary<string, ERPEntityInfo>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<Type, ERPEntityInfo> _sharedTableConfigs = new ConcurrentDictionary<Type, ERPEntityInfo>();
        private bool _initialized = false;
        private readonly object _lock = new object();

        public EntityInfoService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EntityInfoService>();
        }

        private void EnsureInitialized()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;

                try
                {
                    _logger.LogDebug("当前已注册的业务类型数量: {0}", _bizTypeToEntityInfo.Count);
                    _logger.LogDebug("当前已注册的实体类型数量: {0}", _entityTypeToEntityInfo.Count);
                    _logger.LogDebug("当前已注册的表名数量: {0}", _tableNameToEntityInfo.Count);
                    _logger.LogDebug("当前已注册的共用表数量: {0}", _sharedTableConfigs.Count);

                    bool allEmpty = _bizTypeToEntityInfo.Count == 0 &&
                                  _entityTypeToEntityInfo.Count == 0 &&
                                  _tableNameToEntityInfo.Count == 0 &&
                                  _sharedTableConfigs.Count == 0;

                    if (allEmpty)
                    {
                        _logger.LogWarning("所有实体映射集合都为空，可能是服务注册或初始化过程中出现严重问题");
                    }
                    else
                    {
                        if (_tableNameToEntityInfo.Count == 0)
                        {
                            _logger.LogWarning("_tableNameToEntityInfo集合为空，开始强制重建");
                            ForceRebuildTableNameToEntityInfo();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "检查实体信息初始化状态时发生错误: {ErrorMessage}", ex.Message);
                }
                finally
                {
                    _initialized = true;
                }
            }
        }

        private void ForceRebuildTableNameToEntityInfo()
        {
            try
            {
                _tableNameToEntityInfo.Clear();

                if (_entityTypeToEntityInfo.Count > 0)
                {
                    _logger.LogInformation("从_entityTypeToEntityInfo重建_tableNameToEntityInfo集合");
                    foreach (var entityInfo in _entityTypeToEntityInfo.Values)
                    {
                        if (!string.IsNullOrEmpty(entityInfo.TableName))
                        {
                            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
                        }
                    }
                }

                if (_bizTypeToEntityInfo.Count > 0)
                {
                    _logger.LogInformation("从_bizTypeToEntityInfo补充_tableNameToEntityInfo集合");
                    foreach (var entityInfo in _bizTypeToEntityInfo.Values)
                    {
                        if (!string.IsNullOrEmpty(entityInfo.TableName) &&
                            !_tableNameToEntityInfo.ContainsKey(entityInfo.TableName))
                        {
                            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
                        }
                    }
                }

                if (_sharedTableConfigs.Count > 0)
                {
                    _logger.LogInformation("从_sharedTableConfigs补充_tableNameToEntityInfo集合");
                    foreach (var entityInfo in _sharedTableConfigs.Values)
                    {
                        if (!string.IsNullOrEmpty(entityInfo.TableName) &&
                            !_tableNameToEntityInfo.ContainsKey(entityInfo.TableName))
                        {
                            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
                        }
                    }
                }

                _logger.LogInformation("强制重建后_tableNameToEntityInfo集合大小: {0}", _tableNameToEntityInfo.Count);

                if (_tableNameToEntityInfo.Count == 0)
                {
                    _logger.LogWarning("强制重建_tableNameToEntityInfo集合失败，集合仍然为空");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "强制重建_tableNameToEntityInfo集合时发生错误: {ErrorMessage}", ex.Message);
            }
        }

        public ERPEntityInfo GetEntityInfo(BizType bizType)
        {
            EnsureInitialized();

            if (_bizTypeToEntityInfo.TryGetValue(bizType, out var entityInfo))
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
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_ReceivablePayable");

                    case BizType.收款单:
                    case BizType.付款单:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PaymentRecord");

                    case BizType.预收款单:
                    case BizType.预付款单:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PreReceivedPayment");

                    case BizType.收款核销:
                    case BizType.付款核销:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PaymentSettlement");

                    case BizType.其他费用收入:
                    case BizType.其他费用支出:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_OtherExpense");

                    case BizType.销售价格调整单:
                    case BizType.采购价格调整单:
                        return _sharedTableConfigs.Values
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

            if (_entityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                EnsureTableNameMapping(entityInfo);
                if (entityInfo.BizType == BizType.无对应数据)
                {
                    foreach (var item in _bizTypeToEntityInfo.Values)
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

            if (_entityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                EnsureTableNameMapping(entityInfo);
                if (entityInfo.BizType == BizType.无对应数据)
                {
                    foreach (var item in _bizTypeToEntityInfo.Values)
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

                    var maperlist = _bizTypeToEntityInfo.Values.Where(c => c.EnumMaper != null);
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

            if (_tableNameToEntityInfo.TryGetValue(tableName, out var entityInfo))
            {
                return entityInfo;
            }

            entityInfo = _sharedTableConfigs.Values
                .FirstOrDefault(info => string.Equals(info.TableName, tableName, StringComparison.OrdinalIgnoreCase));

            if (entityInfo != null)
            {
                _logger.LogDebug("通过共用表配置找到表名对应的实体信息: TableName={0}, EntityType={1}",
                    tableName, entityInfo.EntityType.Name);
                EnsureTableNameMapping(entityInfo);
                return entityInfo;
            }

            if (_tableNameToEntityInfo.Count == 0 && _entityTypeToEntityInfo.Count > 0)
            {
                _logger.LogDebug("尝试通过_entityTypeToEntityInfo查找表名 {0} 对应的实体信息", tableName);
                entityInfo = _entityTypeToEntityInfo.Values
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

            var allEntityInfos = _bizTypeToEntityInfo.Values
                .Concat(_sharedTableConfigs.Values)
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

            if (_entityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                return entityInfo.BizType;
            }

            if (_sharedTableConfigs.TryGetValue(entityType, out var sharedConfig))
            {
                if (sharedConfig.TypeResolver != null && entity != null)
                {
                    try
                    {
                        var discriminatorValue = GetPropertyValue(entity, sharedConfig.DiscriminatorField);
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

            if (!_tableNameToEntityInfo.ContainsKey(entityInfo.TableName))
            {
                _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
                _logger.LogDebug("添加表名到实体信息的映射: TableName={0}, EntityType={1}",
                    entityInfo.TableName, entityInfo.EntityType.Name);
            }
        }

        public void RegisterEntity<TEntity>(BizType bizType, Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class
        {
            var builder = new EntityInfoBuilder<TEntity>();
            builder.WithBizType(bizType);

            configure?.Invoke(builder);

            var entityInfo = builder.Build();

            _bizTypeToEntityInfo.TryAdd(bizType, entityInfo);
            _entityTypeToEntityInfo.TryAdd(typeof(TEntity), entityInfo);
            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);

            _logger.LogDebug("已注册实体信息: BizType={0}, EntityType={1}, TableName={2}",
                bizType, typeof(TEntity).Name, entityInfo.TableName);
        }


        /// <summary>
        /// 这里是注册共享类型的实体信息，根据业务类型如付款方向 。会在_bizTypeToEntityInfo 业务信息集合对应的中添加两条
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDiscriminator"></typeparam>
        /// <param name="typeMapping"></param>
        /// <param name="discriminatorExpr"></param>
        /// <param name="configure"></param>
        public void RegisterSharedTable<TEntity, TDiscriminator>(
            IDictionary<TDiscriminator, BizType> typeMapping,
            Expression<Func<TEntity, TDiscriminator>> discriminatorExpr,
            Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class
        {
            var builder = new EntityInfoBuilder<TEntity>();
            var entityType = typeof(TEntity);


            builder.WithTableName(entityType.Name);

            configure?.Invoke(builder);

            var entityInfo = builder.Build();

            // 设置鉴别器和类型解析器
            var fieldName = GetMemberName(discriminatorExpr);
            entityInfo.DiscriminatorField = fieldName;
            entityInfo.TypeResolver = value =>
            {
                if (value is TDiscriminator discriminatorValue && typeMapping.TryGetValue(discriminatorValue, out var bizType))
                {
                    return bizType;
                }
                return BizType.无对应数据;
            };

            // 预注册所有可能的业务类型 收款，付款，对应一个基本信息
            foreach (var mapping in typeMapping)
            {
                var newEntityInfo = entityInfo.DeepClone();
                newEntityInfo.BizType = mapping.Value;
                _bizTypeToEntityInfo.TryAdd(mapping.Value, newEntityInfo);
            }

            _entityTypeToEntityInfo.TryAdd(entityType, entityInfo);
            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
            _sharedTableConfigs.TryAdd(entityType, entityInfo);

            _logger.LogDebug("已注册共用表实体信息: EntityType={0}, TableName={1}, 预注册业务类型数量={2}",
                entityType.Name, entityInfo.TableName, typeMapping.Count);
        }

        public bool IsRegistered(BizType bizType)
        {
            EnsureInitialized();

            return _bizTypeToEntityInfo.ContainsKey(bizType);
        }

        public bool IsRegistered(Type entityType)
        {
            EnsureInitialized();

            if (entityType == null)
                return false;

            return _entityTypeToEntityInfo.ContainsKey(entityType) ||
                   _sharedTableConfigs.ContainsKey(entityType);
        }

        public bool IsRegisteredByTableName(string tableName)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(tableName))
                return false;

            return _tableNameToEntityInfo.ContainsKey(tableName);
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;

            var property = obj.GetType().GetProperty(propertyName);
            return property?.GetValue(obj);
        }

        private string GetMemberName<TEntity, T>(Expression<Func<TEntity, T>> expression)
        {
            if (expression.Body is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpr && unaryExpr.Operand is MemberExpression memberExpr2)
            {
                return memberExpr2.Member.Name;
            }

            throw new ArgumentException("Expression must be a member access expression.", nameof(expression));
        }

        public (long Id, string Name) GetIdAndName(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityType = entity.GetType();
            var entityInfo = GetEntityInfo(entityType);

            if (entityInfo == null)
            {
                if (_sharedTableConfigs.TryGetValue(entityType, out entityInfo))
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
                    idValue = GetPropertyValue(entity, entityInfo.IdField);
                }

                object nameValue = null;
                if (!string.IsNullOrEmpty(entityInfo.DescriptionField))
                {
                    nameValue = GetPropertyValue(entity, entityInfo.DescriptionField);
                }
                else if (!string.IsNullOrEmpty(entityInfo.NoField))
                {
                    nameValue = GetPropertyValue(entity, entityInfo.NoField);
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