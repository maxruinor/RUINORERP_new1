using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.Business.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using RUINORERP.PacketSpec.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;

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
        /// 处理缓存响应（使用完整的响应对象）
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
                // 统一验证成功状态和表名（Manage操作允许失败状态）
                if (!response.IsSuccess && response.Operation != CacheOperation.Manage)
                {                    
                    _log?.LogWarning("缓存响应未成功，表名={0}, 操作类型={1}", response.TableName, response.Operation);
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

                        ProcessCacheData(response.TableName, response.CacheData.Data);
                        break;
                    
                    case CacheOperation.Remove:
                        // 统一处理删除操作
                        if (!string.IsNullOrEmpty(response.TableName))
                        {                            
                            HandleRemoveOperation(response);
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
                        }
                        else
                        {                            
                            _log?.LogWarning("Clear操作响应无效，表名为空");
                        }
                        break;
                    
                    case CacheOperation.Manage:
                        HandleManageOperation(response);
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
        /// 处理删除操作
        /// </summary>
        private void HandleRemoveOperation(CacheResponse response)
        {
            try
            {                
                // 如果有数据，尝试删除指定实体；否则删除整个表缓存
                if (response.CacheData?.Data != null)
                {                    
                    // 处理单个实体删除
                    try
                    {                
                        var entityId = Convert.ToInt64(response.CacheData.Data);
                        _cacheManager.DeleteEntity(response.TableName, entityId);
                    }
                    catch (FormatException ex)
                    {                
                        _log?.LogWarning(ex, "实体ID格式无效，表名={0}, ID值={1}", response.TableName, response.CacheData.Data);
                        // 尝试删除整个表缓存作为降级方案
                        CleanCacheSafely(response.TableName);
                    }
                }
                else
                {                    
                    // 删除整个表缓存
                    CleanCacheSafely(response.TableName);
                }
            }
            catch (Exception ex)
            {                
                _log?.LogError(ex, "处理缓存删除响应失败，表名={0}, 错误信息={1}", response.TableName, ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 处理管理类操作
        /// </summary>
        private void HandleManageOperation(CacheResponse response)
        {
            // 非成功响应直接记录并返回
            if (!response.IsSuccess)
            {                
                _log?.LogWarning("管理操作响应无效");
                return;
            }
            
            // 获取操作类型
            string operationType = response.GetOperationResult<string>("OperationType")?.ToLower();
            if (string.IsNullOrEmpty(operationType))
                return;
            
            switch (operationType)
            {                
                case "batch":
                    // 批量操作处理
                    if (!string.IsNullOrEmpty(response.TableName) && response.CacheData?.Data is JArray dataArray)
                    {                
                        try
                        {                    
                            ProcessCacheData(response.TableName, dataArray);
                        }
                        catch (Exception ex)
                        {                    
                            _log?.LogError(ex, "处理缓存批量操作响应失败，表名={0}", response.TableName);
                        }
                    }
                    else
                    {                
                        _log?.LogWarning(string.IsNullOrEmpty(response.TableName) ? 
                            "缓存批量操作响应无效，表名为空" : 
                            "缓存批量操作数据格式不正确");
                    }
                    break;
                
                case "warmup":
                    // 预热操作处理
                    if (response.CacheData?.Data is JObject dataObject)
                    {                
                        try
                        {                    
                            foreach (var property in dataObject.Properties())
                            {                    
                                ProcessCacheData(property.Name, property.Value);
                            }
                        }
                        catch (Exception ex)
                        {                    
                            _log?.LogError(ex, "处理缓存预热响应失败");
                        }
                    }
                    else
                    {                
                        _log?.LogWarning("缓存预热响应数据为空或格式不正确");
                    }
                    break;
                
                case "invalidate":
                    // 缓存失效操作
                    if (!string.IsNullOrEmpty(response.TableName))
                    {                        
                        CleanCacheSafely(response.TableName);
                    }
                    break;
            }
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
                    _log?.LogWarning("缓存数据为空，表名={0}", tableName);
                    return;
                }



                // 获取实体类型
                var entityType = _cacheManager.GetEntityType(tableName);
                if (entityType == null)
                {                    
                    _log?.LogWarning("未找到表{0}的实体类型", tableName);
                    return;
                }

                // 转换数据为实体列表并更新缓存
                var entityList = ConvertToEntityList(data, entityType);
                if (entityList != null && entityList.Count > 0)
                {                    
                    _cacheManager.UpdateEntityList(tableName, entityList);
                    _log?.LogDebug("更新缓存成功，表名={0}，记录数={1}", tableName, entityList.Count);
                }
                else
                {                    
                    _log?.LogDebug("缓存数据为空列表或转换失败，表名={0}", tableName);
                }
            }
            catch (Exception ex)
            {                
                _log?.LogError(ex, "处理缓存数据失败，表名={0}", tableName);
                throw;
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