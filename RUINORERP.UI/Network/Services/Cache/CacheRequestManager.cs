using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using RUINORERP.PacketSpec.Models;
using RUINORERP.UI.Network;
using RUINORERP.PacketSpec.Validation;
using FluentValidation.Results;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;

namespace RUINORERP.UI.Network.Services.Cache
{
    /// <summary>
    /// 缓存请求管理器 - 负责处理与服务器的缓存数据请求逻辑
    /// 注意：此类是对业务层缓存功能的UI层封装，提供更友好的请求接口
    /// </summary>
    public class CacheRequestManager : CacheValidationBase, IDisposable
    {
        private readonly ILogger<CacheRequestManager> _log;
        private readonly ClientCommunicationService _communicationService;
        private readonly IEntityCacheManager _cacheManager;
        private readonly ConcurrentDictionary<string, DateTime> _lastRequestTimes = new ConcurrentDictionary<string, DateTime>();
        private const int RequestIntervalSeconds = 5; // 请求间隔限制
        private const int MaxCacheAgeMinutes = 30; // 缓存最大有效时间（分钟）

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheRequestManager(ILogger<CacheRequestManager> log, ClientCommunicationService communicationService, IEntityCacheManager cacheManager)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        /// <summary>
        /// 向服务器请求指定表的缓存数据
        /// </summary>
        public async Task RequestCacheAsync(string tableName)
        {
            var validationResult = base.ValidateTableName(tableName);
            if (!validationResult.IsValid)
            {
                _log.LogError(validationResult.GetValidationErrors());
                return;
            }

            // 检查限流和缓存有效性，任一条件不满足则跳过请求
            if (!CheckRequestFrequency(tableName, CacheOperation.Get))
            {
                _log.LogDebug("请求频率过高，跳过请求，表名={0}", tableName);
                return;
            }

            if (IsLocalCacheValid(tableName))
            {
                _log.LogDebug("本地缓存有效，跳过请求，表名={0}", tableName);
                return;
            }

            // 创建Get操作的请求并执行，优化为单步处理
            var response = await ProcessCacheRequestAsync(new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Get
            });

            // 利用业务层缓存管理器更新缓存（如果响应成功）
            if (response?.IsSuccess == true && response.CacheData != null)
            {
                _cacheManager.UpdateEntityList(tableName, response.CacheData.Data);
            }
        }

        /// <summary>
        /// 向服务器发送缓存管理请求
        /// </summary>
        public async Task<CacheResponse> SendCacheManageRequestAsync(string tableName, string operationType, Dictionary<string, object> parameters = null)
        {
            var validationResult = base.ValidateTableName(tableName);
            if (!validationResult.IsValid)
            {
                _log.LogError(validationResult.GetValidationErrors());
                return null;
            }

            // 创建请求并设置参数，优化为一步完成
            return await ProcessCacheRequestAsync(new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Manage,
                Parameters = new Dictionary<string, object>(parameters ?? new Dictionary<string, object>())
                {
                    { "OperationType", operationType }
                }
            });
        }

        /// <summary>
        /// 统一处理缓存请求
        /// </summary>
        internal async Task<CacheResponse> ProcessCacheRequestAsync(CacheRequest request)
        {
            // 参数验证
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // 检查请求频率
            if (!CheckRequestFrequency(request.TableName, request.Operation))
            {
                _log.LogDebug("请求频率过高，跳过请求，表名={0}, 操作类型={1}", request.TableName, request.Operation);
                return null;
            }

            // 记录请求时间
            UpdateLastRequestTime(request.TableName, request.Operation);

            try
            {
                // 使用通信服务发送请求到服务器
                return await _communicationService.SendCommandWithResponseAsync<CacheResponse>(
                    CacheCommands.CacheOperation, request, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "请求缓存失败，表名={0}, 操作类型={1}", request.TableName, request.Operation);
                return null;
            }
        }

        /// <summary>
        /// 检查请求频率(带操作类型)
        /// </summary>
        private bool CheckRequestFrequency(string tableName, CacheOperation operation)
        {
            string key = $"{tableName}_{operation}";
            if (_lastRequestTimes.TryGetValue(key, out DateTime lastRequestTime))
            {
                var timeSpan = DateTime.Now - lastRequestTime;
                if (timeSpan.TotalSeconds < RequestIntervalSeconds)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查本地缓存是否有效
        /// </summary>
        private bool IsLocalCacheValid(string tableName)
        {
            try
            {
                // 检查缓存是否存在且有数据
                var rslist = _cacheManager.GetEntityList<object>(tableName);
                if (rslist == null || rslist.Count == 0)
                    return false;

                // 检查缓存是否过期
                string cacheKey = $"{tableName}_lastUpdated";
                if (_lastRequestTimes.TryGetValue(cacheKey, out DateTime lastUpdated))
                {
                    if ((DateTime.Now - lastUpdated).TotalMinutes > MaxCacheAgeMinutes)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "检查本地缓存有效性失败，表名={0}", tableName);
                return false;
            }
        }

        /// <summary>
        /// 更新表的最后请求时间(带操作类型)
        /// </summary>
        internal void UpdateLastRequestTime(string tableName, CacheOperation operation)
        {
            string key = $"{tableName}_{operation}";
            _lastRequestTimes[key] = DateTime.Now;

            // 同时更新表的最后更新时间（用于缓存过期检查）
            string updateKey = $"{tableName}_lastUpdated";
            _lastRequestTimes[updateKey] = DateTime.Now;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _lastRequestTimes.Clear();
        }
    }
}