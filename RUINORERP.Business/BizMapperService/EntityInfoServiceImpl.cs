using RUINORERP.Global;
using RUINORERP.Common.Helper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 实体信息服务实现类
    /// </summary>
    public class EntityInfoService : IEntityInfoService
    {
        private readonly ILogger _logger;
        
        // 业务类型到实体信息的映射
        private readonly ConcurrentDictionary<BizType, EntityInfo> _bizTypeToEntityInfo = new ConcurrentDictionary<BizType, EntityInfo>();
        
        // 实体类型到实体信息的映射
        private readonly ConcurrentDictionary<Type, EntityInfo> _entityTypeToEntityInfo = new ConcurrentDictionary<Type, EntityInfo>();
        
        // 表名到实体信息的映射
        private readonly ConcurrentDictionary<string, EntityInfo> _tableNameToEntityInfo = new ConcurrentDictionary<string, EntityInfo>(StringComparer.OrdinalIgnoreCase);
        
        // 共用表配置（实体类型->实体信息）
        private readonly ConcurrentDictionary<Type, EntityInfo> _sharedTableConfigs = new ConcurrentDictionary<Type, EntityInfo>();
        
        private bool _initialized = false;
        private readonly object _lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public EntityInfoService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EntityInfoService>();
        }

        /// <summary>
        /// 确保实体信息已初始化
        /// </summary>
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
                    // 输出当前注册的实体数量，用于调试
                    _logger.LogDebug("当前已注册的业务类型数量: {0}", _bizTypeToEntityInfo.Count);
                    _logger.LogDebug("当前已注册的实体类型数量: {0}", _entityTypeToEntityInfo.Count);
                    _logger.LogDebug("当前已注册的表名数量: {0}", _tableNameToEntityInfo.Count);
                    _logger.LogDebug("当前已注册的共用表数量: {0}", _sharedTableConfigs.Count);
                    
                    // 检查所有映射集合的状态
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
                        // 重点：确保_tableNameToEntityInfo不为空
                        if (_tableNameToEntityInfo.Count == 0)
                        {
                            _logger.LogWarning("_tableNameToEntityInfo集合为空，开始强制重建");
                            
                            // 尝试通过所有可用的数据源重建_tableNameToEntityInfo
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
        
        /// <summary>
        /// 强制重建表名到实体信息的映射
        /// </summary>
        private void ForceRebuildTableNameToEntityInfo()
        {
            try
            {
                // 清空现有映射
                _tableNameToEntityInfo.Clear();
                
                // 从_entityTypeToEntityInfo重建
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
                
                // 从_bizTypeToEntityInfo补充
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
                
                // 从_sharedTableConfigs补充
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
                
                // 如果重建后仍然为空，记录严重警告
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
        
        /// <summary>
        /// 根据业务类型获取实体信息
        /// </summary>
        public EntityInfo GetEntityInfo(BizType bizType)
        {
            EnsureInitialized();

            // 首先尝试直接从映射中获取
            if (_bizTypeToEntityInfo.TryGetValue(bizType, out var entityInfo))
            {
                return entityInfo;
            }
            
            // 特殊处理：查找是否有共用表配置对应此业务类型
            // 注意：这是一个反向查找，因为共用表通常是通过实体类型和鉴别器值来解析业务类型的
            // 这里我们需要手动维护一些常见的业务类型与共用表实体的对应关系
            var sharedTableEntityInfo = GetSharedTableEntityInfoForBizType(bizType);
            if (sharedTableEntityInfo != null)
            {
                return sharedTableEntityInfo;
            }
            
            _logger.Debug("未找到业务类型 {0} 对应的实体信息", bizType);
            return null;
        }
        
        /// <summary>
        /// 为业务类型查找对应的共用表实体信息
        /// </summary>
        private EntityInfo GetSharedTableEntityInfoForBizType(BizType bizType)
        {
            try
            {
                // 对于共用表，我们需要特殊处理常见的业务类型映射
                // 这里维护了应收款/应付款单、收款/付款单等共用表业务类型的映射
                switch (bizType)
                {
                    // 应收款/应付款单 - 共用表 tb_FM_ReceivablePayable
                    case BizType.应收款单:
                    case BizType.应付款单:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_ReceivablePayable");
                    
                    // 收款单/付款单 - 共用表 tb_FM_PaymentRecord
                    case BizType.收款单:
                    case BizType.付款单:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PaymentRecord");
                    
                    // 预收款/预付款单 - 共用表 tb_FM_PreReceivedPayment
                    case BizType.预收款单:
                    case BizType.预付款单:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PreReceivedPayment");
                    
                    // 收款核销/付款核销 - 共用表 tb_FM_PaymentSettlement
                    case BizType.收款核销:
                    case BizType.付款核销:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_PaymentSettlement");
                    
                    // 其他费用收入/支出单 - 共用表 tb_FM_OtherExpense
                    case BizType.其他费用收入:
                    case BizType.其他费用支出:
                        return _sharedTableConfigs.Values
                            .FirstOrDefault(info => info.TableName == "tb_FM_OtherExpense");
                    
                    // 价格调整单 - 共用表 tb_FM_PriceAdjustment
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
        
        /// <summary>
        /// 根据实体类型获取实体信息
        /// </summary>
        public EntityInfo GetEntityInfo(Type entityType)
        {
            EnsureInitialized();
            
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            
            if (_entityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                // 确保表名到实体信息的映射已建立
                EnsureTableNameMapping(entityInfo);
                return entityInfo;
            }
            
            _logger.Debug("未找到实体类型 {0} 对应的实体信息", entityType.FullName);
            return null;
        }
        
        /// <summary>
        /// 根据表名获取实体信息（优化后的统一入口）
        /// </summary>
        public EntityInfo GetEntityInfoByTableName(string tableName)
        {
            EnsureInitialized();
            
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));
            
            // 首先尝试直接从表名映射中获取
            if (_tableNameToEntityInfo.TryGetValue(tableName, out var entityInfo))
            {
                return entityInfo;
            }
            
            // 特殊处理：对于共用表，我们可能需要在_sharedTableConfigs中查找
            // 注意：共用表的表名应该已经通过RegisterSharedTable方法添加到_tableNameToEntityInfo中
            // 这里添加一个额外的检查作为备用方案
            entityInfo = _sharedTableConfigs.Values
                .FirstOrDefault(info => string.Equals(info.TableName, tableName, StringComparison.OrdinalIgnoreCase));
            
            if (entityInfo != null)
            {
                _logger.LogDebug("通过共用表配置找到表名对应的实体信息: TableName={0}, EntityType={1}",
                    tableName, entityInfo.EntityType.Name);
                // 确保表名到实体信息的映射已建立
                EnsureTableNameMapping(entityInfo);
                return entityInfo;
            }
            
            // 如果_tableNameToEntityInfo为空，尝试通过_entityTypeToEntityInfo查找（用于错误恢复）
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
            
            _logger.Debug("未找到表名 {0} 对应的实体信息", tableName);
            return null;
        }
        
        /// <summary>
        /// 获取所有注册的实体信息
        /// </summary>
        public IEnumerable<EntityInfo> GetAllEntityInfos()
        {
            EnsureInitialized();
            
            // 合并所有实体信息，去除重复项
            // 使用自定义比较逻辑确保类型兼容性
            var allEntityInfos = _bizTypeToEntityInfo.Values
                .Concat(_sharedTableConfigs.Values)
                .GroupBy(info => new { Type = info.EntityType?.AssemblyQualifiedName, Table = info.TableName })
                .Select(g => g.First())
                .ToList();
            
            // 确保所有实体信息都已添加到_tableNameToEntityInfo
            foreach (var entityInfo in allEntityInfos)
            {
                EnsureTableNameMapping(entityInfo);
            }
            
            return allEntityInfos;
        }
        
        /// <summary>
        /// 获取业务类型对应的实体类型
        /// </summary>
        public Type GetEntityType(BizType bizType)
        {
            EnsureInitialized();
            
            var entityInfo = GetEntityInfo(bizType);
            return entityInfo?.EntityType;
        }
        
        /// <summary>
        /// 通过实体类型获取表名
        /// </summary>
        public string GetTableNameByType(Type entityType)
        {
            EnsureInitialized();
            
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            
            var entityInfo = GetEntityInfo(entityType);
            return entityInfo?.TableName;
        }
        
        /// <summary>
        /// 获取实体类型对应的业务类型
        /// </summary>
        public BizType GetBizType(Type entityType, object entity = null)
        {
            EnsureInitialized();
            
            if (entityType == null)
                throw new ArgumentNullException(nameof(entityType));
            
            // 首先尝试直接从映射中获取
            if (_entityTypeToEntityInfo.TryGetValue(entityType, out var entityInfo))
            {
                return entityInfo.BizType;
            }
            
            // 检查是否为共用表
            if (_sharedTableConfigs.TryGetValue(entityType, out var sharedConfig))
            {
                // 如果有实体实例和类型解析器，可以通过鉴别器值解析业务类型
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
                
                // 如果没有实体实例或解析失败，返回共用表实体默认的业务类型
                // 注意：共用表实体的BizType通常是不明确的，但我们可以设置一个默认值
                _logger.LogDebug("使用共用表实体的默认业务类型: EntityType={0}, DefaultBizType={1}",
                    entityType.Name, sharedConfig.BizType);
                
                return sharedConfig.BizType != BizType.无对应数据 ? sharedConfig.BizType : BizType.Unknown;
            }
            
            _logger.Debug("未找到实体类型 {0} 对应的实体信息或业务类型", entityType.FullName);
            return BizType.Unknown;
        }
        
        /// <summary>
        /// 通过实体实例获取业务类型
        /// </summary>
        public BizType GetBizTypeByEntity(object entity)
        {
            EnsureInitialized();
            
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            
            var entityType = entity.GetType();
            return GetBizType(entityType, entity);
        }
        
        /// <summary>
        /// 获取表名对应的实体类型
        /// </summary>
        public Type GetEntityTypeByTableName(string tableName)
        {
            EnsureInitialized();
            
            var entityInfo = GetEntityInfoByTableName(tableName);
            return entityInfo?.EntityType;
        }
        
        /// <summary>
        /// 检查并确保表名到实体信息的映射已建立
        /// </summary>
        private void EnsureTableNameMapping(EntityInfo entityInfo)
        {
            if (entityInfo == null || string.IsNullOrEmpty(entityInfo.TableName))
                return;
            
            // 确保表名到实体信息的映射已建立
            if (!_tableNameToEntityInfo.ContainsKey(entityInfo.TableName))
            {
                _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
                _logger.LogDebug("添加表名到实体信息的映射: TableName={0}, EntityType={1}",
                    entityInfo.TableName, entityInfo.EntityType.Name);
            }
        }
        
        /// <summary>
        /// 注册实体信息
        /// </summary>
        public void RegisterEntity<TEntity>(BizType bizType, Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class
        {
            var builder = new EntityInfoBuilder<TEntity>();
            builder.WithBizType(bizType);
            
            // 应用配置
            configure?.Invoke(builder);
            
            var entityInfo = builder.Build();
            
            // 线程安全的添加操作
            _bizTypeToEntityInfo.TryAdd(bizType, entityInfo);
            _entityTypeToEntityInfo.TryAdd(typeof(TEntity), entityInfo);
            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
            
            _logger.LogDebug("已注册实体信息: BizType={0}, EntityType={1}, TableName={2}",
                bizType, typeof(TEntity).Name, entityInfo.TableName);
        }


        
        /// <summary>
        /// 注册共用表实体信息
        /// </summary>
        public void RegisterSharedTable<TEntity, TDiscriminator>(
            Func<TDiscriminator, BizType> typeResolver,
            Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class
        {
            var builder = new EntityInfoBuilder<TEntity>();
            
            // 默认共用表配置
            var entityType = typeof(TEntity);
            builder.WithTableName(entityType.Name);
            
            // 应用自定义配置
            configure?.Invoke(builder);
            
            var entityInfo = builder.Build();

            // 保存到实体类型映射
            _entityTypeToEntityInfo.TryAdd(entityType, entityInfo);
            
            // 保存表名到实体信息的映射
            _tableNameToEntityInfo.TryAdd(entityInfo.TableName, entityInfo);
            
            // 保存共用表配置
            _sharedTableConfigs.TryAdd(entityType, entityInfo);
            
            // 特殊处理：如果是带TypeResolver的共用表，
            // 为了支持通过业务类型直接查找，我们需要特殊处理
            if (entityInfo.TypeResolver != null && typeResolver != null)
            {
                // 注意：在实际应用中，共用表通常只有有限的几种业务类型
                // 这里我们无法预先知道所有可能的鉴别器值，
                // 但我们可以在GetEntityInfo方法中特殊处理共用表的情况
                _logger.LogDebug("共用表实体已配置TypeResolver，将在运行时解析业务类型: EntityType={0}",
                    entityType.Name);
            }
            
            _logger.LogDebug("已注册共用表实体信息: EntityType={0}, TableName={1}",
                entityType.Name, entityInfo.TableName);
        }


        /// <summary>
        /// 判断业务类型是否已注册
        /// </summary>
        public bool IsRegistered(BizType bizType)
        {
            EnsureInitialized();
            
            return _bizTypeToEntityInfo.ContainsKey(bizType);
        }
        
        /// <summary>
        /// 判断实体类型是否已注册
        /// </summary>
        public bool IsRegistered(Type entityType)
        {
            EnsureInitialized();
            
            if (entityType == null)
                return false;
            
            return _entityTypeToEntityInfo.ContainsKey(entityType) || 
                   _sharedTableConfigs.ContainsKey(entityType);
        }
        
        /// <summary>
        /// 判断表名是否已注册
        /// </summary>
        public bool IsRegisteredByTableName(string tableName)
        {
            EnsureInitialized();
            
            if (string.IsNullOrEmpty(tableName))
                return false;
            
            return _tableNameToEntityInfo.ContainsKey(tableName);
        }
        
        /// <summary>
        /// 通过反射获取对象属性值
        /// </summary>
        private object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return null;
            
            var property = obj.GetType().GetProperty(propertyName);
            return property?.GetValue(obj);
        }

        /// <summary>
        /// 获取对象的ID和名称的值
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>包含ID和名称值的元组</returns>
        public (long Id, string Name) GetIdAndName(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityType = entity.GetType();
            var entityInfo = GetEntityInfo(entityType);

            if (entityInfo == null)
            {
                // 如果找不到实体信息，尝试从共用表配置中获取
                if (_sharedTableConfigs.TryGetValue(entityType, out entityInfo))
                {
                    // 获取共用表业务类型
                    var bizType = GetBizType(entityType, entity);
                    entityInfo = GetEntityInfo(bizType);
                }
            }

            if (entityInfo == null)
            {
                _logger.Debug("未找到实体类型 {0} 对应的实体信息", entityType.FullName);
                return (0, null);
            }

            try
            {
                // 获取ID值
                object idValue = null;
                if (!string.IsNullOrEmpty(entityInfo.IdField))
                {
                    idValue = GetPropertyValue(entity, entityInfo.IdField);
                }

                // 获取名称值（优先使用描述字段，如果没有则使用编号字段）
                object nameValue = null;
                if (!string.IsNullOrEmpty(entityInfo.DescriptionField))
                {
                    nameValue = GetPropertyValue(entity, entityInfo.DescriptionField);
                }
                else if (!string.IsNullOrEmpty(entityInfo.NoField))
                {
                    nameValue = GetPropertyValue(entity, entityInfo.NoField);
                }

                return (idValue.ToLong(), nameValue.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("获取实体对象的ID和名称值时发生错误: {0}", ex.Message);
                _logger.Debug("异常详情:", ex);
                return (0, null);
            }
        }
    }
}