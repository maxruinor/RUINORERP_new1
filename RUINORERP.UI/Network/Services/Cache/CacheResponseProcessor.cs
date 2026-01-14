using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Models.Cache;
using System.Linq;

namespace RUINORERP.UI.Network.Services.Cache
{
    /// <summary>
    /// 缓存响应处理器 - 负责处理服务器返回的缓存响应并更新本地缓存
    /// 注意：优化版本，简化实现并更好地利用业务层缓存管理器
    /// </summary>
    public class CacheResponseProcessor : CacheValidationBase, IDisposable
    {
        private readonly ILogger<CacheResponseProcessor> _log;
        private readonly IEntityCacheManager _cacheManager;
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheResponseProcessor(ILogger<CacheResponseProcessor> log, IEntityCacheManager cacheManager)
        {
            _log = log;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源（目前没有需要释放的托管资源）
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 处理缓存响应（使用完整的响应对象）- 增强版，包含数据完整性验证
        /// </summary>
        public void ProcessCacheResponse(CacheResponse response)
        {
            // 使用基类进行验证
            var validationResult = base.ValidateCacheResponse(response);
            if (!validationResult.IsValid)
            {
                _log?.LogError($"缓存响应验证失败: {validationResult.GetValidationErrors()}");
                return;
            }

            try
            {
                // 记录同步元数据
                LogSyncMetadata(response);

                // 统一验证成功状态和表名（Manage操作允许失败状态）
                if (!response.IsSuccess && response.Operation != CacheOperation.Manage)
                {
                    _log?.LogWarning("缓存响应未成功，表名={0}, 操作类型={1}", response.TableName, response.Operation);
                }

                // 数据完整性验证
                if (!ValidateDataIntegrity(response))
                {
                    _log?.LogError("缓存响应数据完整性验证失败，表名={0}, 操作={1}",
                        response.TableName, response.Operation);
                    return;
                }

                switch (response.Operation)
                {
                    case CacheOperation.Get:
                    case CacheOperation.Set:
                        // 合并相似操作的处理逻辑，减少重复判断
                        if (string.IsNullOrEmpty(response.TableName) || response.CacheData == null)
                        {
                            _log?.LogWarning("{0}操作响应数据无效，表名={1}", response.Operation, response.TableName);
                            break;
                        }

                        // 对于Set操作，先清理旧缓存
                        if (response.Operation == CacheOperation.Set)
                        {
                            CleanCacheSafely(response.TableName);
                        }

                        ProcessCacheData(response.TableName, response.CacheData.EntityByte);
                        LogSyncSuccess(response, "数据更新成功");
                        break;

                    case CacheOperation.Remove:
                        // 统一处理删除操作
                        if (!string.IsNullOrEmpty(response.TableName))
                        {
                            HandleRemoveOperation(response);
                            LogSyncSuccess(response, "数据删除成功");
                        }
                        else
                        {
                            _log?.LogWarning("删除操作响应无效，表名为空");
                        }
                        break;

                    case CacheOperation.Clear:
                        // 简化Clear操作的错误处理
                        if (!string.IsNullOrEmpty(response.TableName))
                        {
                            CleanCacheSafely(response.TableName);
                            LogSyncSuccess(response, "缓存清空成功");
                        }
                        else
                        {
                            _log?.LogWarning("Clear操作响应无效，表名为空");
                        }
                        break;

                    case CacheOperation.Manage:
                        //  HandleManageOperation(response);
                        break;

                    default:
                        _log?.LogWarning("未知的缓存操作类型: {0}", response.Operation);
                        break;
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存响应失败，表名={0}, 操作类型={1}, 错误信息={2}",
                    response.TableName, response.Operation, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 验证数据完整性
        /// </summary>
        private bool ValidateDataIntegrity(CacheResponse response)
        {
            try
            {
                // 基本验证
                if (string.IsNullOrEmpty(response.TableName))
                {
                    return false;
                }

                // Set/Get操作必须有数据
                if ((response.Operation == CacheOperation.Set || response.Operation == CacheOperation.Get)
                    && response.CacheData == null)
                {
                    _log?.LogWarning("数据完整性验证失败: Set/Get操作缺少缓存数据，表名={0}", response.TableName);
                    return false;
                }

                // 如果有数据，验证数据不为空
                if (response.CacheData != null && response.CacheData.EntityByte == null
                    && response.Operation != CacheOperation.Remove && response.Operation != CacheOperation.Clear)
                {
                    _log?.LogWarning("数据完整性验证失败: 缓存数据为空，表名={0}", response.TableName);
                    return false;
                }

                // 时间戳验证（如果存在）
                if (response.Timestamp != default && response.Timestamp > DateTime.UtcNow.AddHours(1))
                {
                    _log?.LogWarning("数据完整性验证失败: 时间戳无效，表名={0}, 时间={1}",
                        response.TableName, response.Timestamp);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "验证数据完整性时发生异常，表名={0}", response.TableName);
                return false;
            }
        }

        /// <summary>
        /// 记录同步元数据
        /// </summary>
        private void LogSyncMetadata(CacheResponse response)
        {
            try
            {
                if (response.Metadata != null && response.Metadata.Count > 0)
                {
                    if (response.Metadata.TryGetValue("SyncType", out var syncType))
                    {
                        _log?.LogDebug("同步类型={0}, 表名={1}", syncType, response.TableName);
                    }
                    if (response.Metadata.TryGetValue("ServerTimestamp", out var serverTimestamp))
                    {
                        _log?.LogDebug("服务器时间戳={0}, 表名={1}", serverTimestamp, response.TableName);
                    }
                    if (response.Metadata.TryGetValue("SourceSessionId", out var sourceSessionId))
                    {
                        _log?.LogDebug("源会话ID={0}, 表名={1}", sourceSessionId, response.TableName);
                    }
                }
            }
            catch (Exception ex)
            {
                _log?.LogDebug(ex, "记录同步元数据时发生错误");
            }
        }

        /// <summary>
        /// 记录同步成功
        /// </summary>
        private void LogSyncSuccess(CacheResponse response, string message)
        {
            try
            {
                _log?.LogDebug("缓存同步成功: {0}, 表名={1}, 操作={2}, 时间={3}",
                    message, response.TableName, response.Operation, DateTime.Now);
            }
            catch
            {
                // 忽略日志记录错误
            }
        }



        /// <summary>
        /// 处理缓存请求（使用完整的响应对象）
        /// 服务器主动推送过来的缓存请求
        /// </summary>
        public void ProcessCacheRequest(CacheRequest request)
        {
            // 使用基类进行验证
            var validationResult = base.ValidateCacheRequest(request);
            if (!validationResult.IsValid)
            {
                return;
            }

            try
            {
                switch (request.Operation)
                {
                    case CacheOperation.Get:
                        // Get操作直接处理数据
                        ProcessCacheData(request.TableName, request.CacheData?.EntityByte);
                        break;

                    case CacheOperation.Set:
                        // Set操作：更新或添加单个实体到缓存（不清空整个表）
                        ProcessSingleEntityUpdate(request.TableName, request.CacheData?.EntityByte);
                        break;

                    case CacheOperation.Remove:
                        // 统一处理删除操作
                        if (!string.IsNullOrEmpty(request.TableName))
                        {
                            if (request.PrimaryKeyValue != null)
                            {
                                // 删除单个实体
                                RemoveSingleEntity(request.TableName, request.PrimaryKeyValue);
                            }
                            else
                            {
                                // 清空整个表
                                CleanCacheSafely(request.TableName);
                            }
                        }
                        else
                        {
                            _log?.LogWarning("删除操作响应无效，表名为空");
                        }
                        break;

                    case CacheOperation.Clear:
                        // 简化Clear操作的错误处理
                        if (!string.IsNullOrEmpty(request.TableName))
                        {
                            CleanCacheSafely(request.TableName);
                        }
                        else
                        {
                            _log?.LogWarning("Clear操作响应无效，表名为空");
                        }
                        break;

                    case CacheOperation.Manage:
                        //  HandleManageOperation(request);
                        break;

                    default:
                        _log?.LogWarning("未知的缓存操作类型: {0}", request.Operation);
                        break;
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存响应失败，表名={0}, 操作类型={1}, 错误信息={2}",
                    request.TableName, request.Operation, ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 安全地清理缓存，发生异常时记录日志但不中断流程
        /// </summary>
        private void CleanCacheSafely(string tableName)
        {
            try
            {
                _cacheManager.DeleteEntityList(tableName);

            }
            catch (Exception ex)
            {
                _log?.LogWarning(ex, "清理旧缓存失败，但继续处理，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 处理删除操作 - 支持单个实体删除和批量删除
        /// </summary>
        private void HandleRemoveOperation(CacheResponse response)
        {
            try
            {
                // 如果有数据，尝试删除指定实体；否则删除整个表缓存
                if (response.CacheData?.EntityByte != null)
                {
                    // 处理单个实体删除或批量删除
                    ProcessDeleteData(response.TableName, response.CacheData.EntityByte);
                }
                else
                {
                    // 删除整个表缓存
                    CleanCacheSafely(response.TableName);
                    _log?.LogDebug("清理整个表缓存，表名={0}", response.TableName);
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存删除响应失败，表名={0}, 错误信息={1}", response.TableName, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 处理删除数据 - 支持多种ID格式和批量删除
        /// </summary>
        private void ProcessDeleteData(string tableName, object deleteData)
        {
            try
            {
                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    _log?.LogWarning("未找到表{0}的实体类型，执行整表清理", tableName);
                    CleanCacheSafely(tableName);
                    return;
                }

                // 获取主键属性
                var keyProperty = GetPrimaryKeyProperty(entityType);
                if (keyProperty == null)
                {
                    _log?.LogWarning("未找到表{0}的主键属性，执行整表清理", tableName);
                    CleanCacheSafely(tableName);
                    return;
                }

                // 处理不同类型的删除数据
                if (deleteData is JArray jArray)
                {
                    // 批量删除
                    ProcessBatchDelete(tableName, jArray, keyProperty);
                }
                else if (deleteData is JObject jObject)
                {
                    // 单个实体删除
                    ProcessSingleDelete(tableName, jObject, keyProperty);
                }
                else if (deleteData is string jsonString && !string.IsNullOrEmpty(jsonString))
                {
                    // JSON字符串格式
                    ProcessJsonDelete(tableName, jsonString, keyProperty);
                }
                else if (IsNumericType(deleteData.GetType()))
                {
                    // 直接是ID值
                    var entityId = Convert.ToInt64(deleteData);
                    _cacheManager.DeleteEntity(tableName, entityId);
                    _log?.LogDebug("删除单个实体成功，表名={0}，ID={1}", tableName, entityId);
                }
                else
                {
                    _log?.LogWarning("不支持的删除数据格式: {0}，执行整表清理", deleteData.GetType().Name);
                    CleanCacheSafely(tableName);
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理删除数据失败，表名={0}，执行整表清理作为降级方案", tableName);
                CleanCacheSafely(tableName);
            }
        }

        /// <summary>
        /// 处理批量删除
        /// </summary>
        private void ProcessBatchDelete(string tableName, JArray jArray, PropertyInfo keyProperty)
        {
            var successCount = 0;
            var failCount = 0;

            foreach (var item in jArray)
            {
                try
                {
                    var entityId = ExtractEntityId(item, keyProperty);
                    if (entityId.HasValue)
                    {
                        _cacheManager.DeleteEntity(tableName, entityId.Value);
                        successCount++;
                    }
                    else
                    {
                        failCount++;
                    }
                }
                catch (Exception ex)
                {
                    failCount++;
                    _log?.LogWarning(ex, "批量删除中单个实体处理失败，表名={0}", tableName);
                }
            }

            _log?.LogDebug("批量删除完成，表名={0}，成功={1}，失败={2}", tableName, successCount, failCount);
        }

        /// <summary>
        /// 处理单个实体删除
        /// </summary>
        private void ProcessSingleDelete(string tableName, JObject jObject, PropertyInfo keyProperty)
        {
            var entityId = ExtractEntityId(jObject, keyProperty);
            if (entityId.HasValue)
            {
                _cacheManager.DeleteEntity(tableName, entityId.Value);
                _log?.LogDebug("删除单个实体成功，表名={0}，ID={1}", tableName, entityId.Value);
            }
            else
            {
                _log?.LogWarning("无法提取实体ID，表名={0}，执行整表清理", tableName);
                CleanCacheSafely(tableName);
            }
        }

        /// <summary>
        /// 处理JSON字符串删除
        /// </summary>
        private void ProcessJsonDelete(string tableName, string jsonString, PropertyInfo keyProperty)
        {
            try
            {
                // 尝试解析为JArray（批量）
                var jArray = JArray.Parse(jsonString);
                ProcessBatchDelete(tableName, jArray, keyProperty);
            }
            catch
            {
                try
                {
                    // 尝试解析为JObject（单个）
                    var jObject = JObject.Parse(jsonString);
                    ProcessSingleDelete(tableName, jObject, keyProperty);
                }
                catch (Exception ex)
                {
                    _log?.LogWarning(ex, "JSON字符串解析失败，表名={0}，执行整表清理", tableName);
                    CleanCacheSafely(tableName);
                }
            }
        }

        /// <summary>
        /// 提取实体ID - 支持多种ID字段名格式
        /// </summary>
        private long? ExtractEntityId(JToken token, PropertyInfo keyProperty)
        {
            try
            {
                // 优先使用主键属性名
                var idValue = token[keyProperty.Name]?.ToString() ??
                             token["Id"]?.ToString() ??
                             token["ID"]?.ToString() ??
                             token["id"]?.ToString();

                if (!string.IsNullOrEmpty(idValue) && long.TryParse(idValue, out var entityId))
                {
                    return entityId;
                }

                return null;
            }
            catch (Exception ex)
            {
                _log?.LogWarning(ex, "提取实体ID失败");
                return null;
            }
        }

        /// <summary>
        /// 获取主键属性
        /// </summary>
        private PropertyInfo GetPrimaryKeyProperty(Type entityType)
        {
            // 支持多种主键属性名
            var keyNames = new[] { "Id", "ID", "PrimaryKey", "Key" };

            foreach (var keyName in keyNames)
            {
                var property = entityType.GetProperty(keyName);
                if (property != null && IsNumericType(property.PropertyType))
                {
                    return property;
                }
            }

            // 如果没找到，返回第一个数值类型的属性
            return entityType.GetProperties()
                .FirstOrDefault(p => IsNumericType(p.PropertyType));
        }

        /// <summary>
        /// 判断是否为数值类型
        /// </summary>
        private bool IsNumericType(Type type)
        {
            return type == typeof(long) || type == typeof(int) || type == typeof(short) ||
                   type == typeof(byte) || type == typeof(ulong) || type == typeof(uint) ||
                   type == typeof(ushort) || type == typeof(sbyte) || type == typeof(decimal) ||
                   type == typeof(float) || type == typeof(double);
        }


        /// <summary>
        /// 处理缓存数据并更新到缓存管理器
        /// </summary>
        private void ProcessCacheData(string tableName, object data)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    return;
                }

                // 转换数据为实体列表并更新缓存
                var entityList = ConvertToEntityList(data, entityType);
                if (entityList != null && entityList.Count > 0)
                {
                    _cacheManager.UpdateEntityList(tableName, entityList);
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理缓存数据失败，表名={0}", tableName);
                throw;
            }
        }

        /// <summary>
        /// 处理单个实体的更新（不清理整个表，只更新单个实体）
        /// </summary>
        private void ProcessSingleEntityUpdate(string tableName, object data)
        {
            try
            {
                if (data == null)
                {
                    return;
                }

                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {
                    return;
                }

                // 转换数据为实体对象
                var entity = ConvertToSingleEntity(data, entityType);
                if (entity != null)
                {
                    // 使用UpdateEntity方法更新单个实体（会自动处理新增或更新）
                    _cacheManager.UpdateEntity(tableName, entity);
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "处理单个实体更新失败，表名={0}", tableName);
                throw;
            }
        }

        /// <summary>
        /// 删除单个实体
        /// </summary>
        private void RemoveSingleEntity(string tableName, object primaryKeyValue)
        {
            try
            {
                if (primaryKeyValue == null)
                {
                    return;
                }

                // 尝试将主键值转换为long类型
                long entityId;
                if (primaryKeyValue is long longValue)
                {
                    entityId = longValue;
                }
                else if (primaryKeyValue is int intValue)
                {
                    entityId = intValue;
                }
                else if (long.TryParse(primaryKeyValue.ToString(), out entityId))
                {
                    // 成功转换
                }
                else
                {
                    _log?.LogWarning("无法将主键值转换为long类型，表名={0}, 值={1}", tableName, primaryKeyValue);
                    return;
                }

                // 删除单个实体
                _cacheManager.DeleteEntity(tableName, entityId);
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "删除单个实体失败，表名={0}", tableName);
                throw;
            }
        }

        /// <summary>
        /// 将数据转换为单个实体对象
        /// </summary>
        private object ConvertToSingleEntity(object data, Type entityType)
        {
            try
            {
                if (data is JArray jArray && jArray.Count > 0)
                {
                    // 如果是JArray，取第一个元素
                    return JsonConvert.DeserializeObject(jArray.First.ToString(), entityType);
                }
                else if (data is JObject jObject)
                {
                    // 直接是单个对象
                    return JsonConvert.DeserializeObject(jObject.ToString(), entityType);
                }
                else if (data is string jsonString && !string.IsNullOrEmpty(jsonString))
                {
                    try
                    {
                        // 尝试解析为JArray并取第一个
                        var parsedJArray = JArray.Parse(jsonString);
                        if (parsedJArray.Count > 0)
                        {
                            return JsonConvert.DeserializeObject(parsedJArray.First.ToString(), entityType);
                        }
                    }
                    catch
                    {
                        // 尝试直接解析为单个对象
                        try
                        {
                            return JsonConvert.DeserializeObject(jsonString, entityType);
                        }
                        catch (Exception ex)
                        {
                            _log?.LogWarning(ex, "JSON字符串反序列化失败");
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "转换单个实体失败");
                return null;
            }
        }

        /// <summary>
        /// 将数据转换为指定类型的实体列表
        /// </summary>
        private IList ConvertToEntityList(object data, Type entityType)
        {
            try
            {
                if (data is JArray jArray)
                {
                    return ConvertJArrayToList(jArray, entityType);
                }
                else if (data is JObject jObject)
                {
                    // 单个实体对象

                    var entityList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType));
                    var entity = JsonConvert.DeserializeObject(jObject.ToString(), entityType);
                    if (entity != null)
                    {
                        entityList.Add(entity);
                    }
                    return entityList;
                }
                else if (data is string jsonString && !string.IsNullOrEmpty(jsonString))
                {

                    try
                    {
                        // 尝试解析为JArray
                        var parsedJArray = JArray.Parse(jsonString);
                        return ConvertJArrayToList(parsedJArray, entityType);
                    }
                    catch (Exception)
                    {
                        // 尝试解析为JObject

                        try
                        {
                            var parsedJObject = JObject.Parse(jsonString);
                            var entityList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType));
                            var entity = JsonConvert.DeserializeObject(jsonString, entityType);
                            if (entity != null)
                            {
                                entityList.Add(entity);
                            }
                            return entityList;
                        }
                        catch (Exception ex)
                        {
                            _log?.LogWarning(ex, "JSON字符串解析失败");
                            return null;
                        }
                    }
                }

                _log?.LogWarning("不支持的数据类型: {0}", data.GetType().Name);
                return null;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "转换数据为实体列表失败");
                throw;
            }
        }

        /// <summary>
        /// 将JArray转换为实体列表
        /// </summary>
        private IList ConvertJArrayToList(JArray jArray, Type entityType)
        {
            try
            {

                var entityList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(entityType));
                int successCount = 0;
                int failCount = 0;

                foreach (var item in jArray)
                {
                    try
                    {
                        var entity = JsonConvert.DeserializeObject(item.ToString(), entityType);
                        if (entity != null)
                        {
                            entityList.Add(entity);
                            successCount++;
                        }
                        else
                        {
                            failCount++;
                            _log?.LogWarning("反序列化得到null实体");
                        }
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        _log?.LogWarning(ex, "单个实体反序列化失败");
                        // 继续处理下一个实体
                    }
                }


                return entityList;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "转换JArray失败");
                throw;
            }
        }
    }
}