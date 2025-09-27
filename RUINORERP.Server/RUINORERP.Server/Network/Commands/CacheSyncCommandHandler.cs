using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Core.Cache;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 缓存同步命令处理器
    /// 处理客户端与服务器之间的缓存同步命令
    /// </summary>
    [CommandHandler("CacheSyncCommandHandler", priority: 85)]
    public class CacheSyncCommandHandler : CommandHandlerBase
    {
        private readonly ILogger<CacheSyncCommandHandler> _logger;
        private readonly UnifiedCacheManager _cacheManager;
        private readonly CacheSyncService _cacheSyncService;
        private readonly ISessionService _sessionService;

        public CacheSyncCommandHandler(
            ILogger<CacheSyncCommandHandler> logger,
            UnifiedCacheManager cacheManager,
            CacheSyncService cacheSyncService,
            ISessionService sessionService)
        {
            _logger = logger;
            _cacheManager = cacheManager;
            _cacheSyncService = cacheSyncService;
            _sessionService = sessionService;
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            CacheSyncProtocol.CacheCommands.CacheUpdate.FullCode,
            CacheSyncProtocol.CacheCommands.CacheSync.FullCode,
            CacheSyncProtocol.CacheCommands.CacheSubscribe.FullCode,
            CacheSyncProtocol.CacheCommands.CacheUnsubscribe.FullCode,
            CacheSyncProtocol.CacheCommands.CacheRequest.FullCode,
            CacheSyncProtocol.CacheCommands.CacheResponse.FullCode
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 85;

        /// <summary>
        /// 判断是否可以处理指定命令
        /// </summary>
        public override bool CanHandle(ICommand command)
        {
            return SupportedCommands.Contains(command.CommandIdentifier.FullCode);
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        protected override async Task<ResponseBase> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;
                
                switch (commandId.FullCode)
                {
                    case var id when id == CacheSyncProtocol.CacheCommands.CacheUpdate.FullCode:
                        return await HandleCacheUpdateAsync(command, cancellationToken);
                        
                    case var id when id == CacheSyncProtocol.CacheCommands.CacheSync.FullCode:
                        return await HandleCacheSyncAsync(command, cancellationToken);
                        
                    case var id when id == CacheSyncProtocol.CacheCommands.CacheSubscribe.FullCode:
                        return await HandleCacheSubscribeAsync(command, cancellationToken);
                        
                    case var id when id == CacheSyncProtocol.CacheCommands.CacheUnsubscribe.FullCode:
                        return await HandleCacheUnsubscribeAsync(command, cancellationToken);
                        
                    case var id when id == CacheSyncProtocol.CacheCommands.CacheRequest.FullCode:
                        return await HandleCacheRequestAsync(command, cancellationToken);
                        
                    case var id when id == CacheSyncProtocol.CacheCommands.CacheResponse.FullCode:
                        return await HandleCacheResponseAsync(command, cancellationToken);
                        
                    default:
                        var errorResponse = ResponseBase.CreateError($"不支持的缓存同步命令类型: {command.CommandIdentifier}", 400)
                            .WithMetadata("ErrorCode", "UNSUPPORTED_CACHE_SYNC_COMMAND");
                        return ConvertToApiResponse(errorResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理缓存同步命令异常: {ex.Message}");
                var errorResponse = ResponseBase.CreateError($"处理缓存同步命令异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_SYNC_HANDLER_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
                return ConvertToApiResponse(errorResponse);
            }
        }

        /// <summary>
        /// 处理缓存更新命令
        /// </summary>
        private async Task<ResponseBase> HandleCacheUpdateAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存更新命令
                var cacheUpdateCommand = command as CacheSyncProtocol.CacheUpdateCommand;
                if (cacheUpdateCommand == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("无效的缓存更新命令", 400)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_UPDATE_COMMAND"));
                }

                // 验证会话
                var sessionInfo = _sessionService.GetSession(command.SessionID);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话无效或未认证", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                // 根据操作类型处理缓存更新
                switch (cacheUpdateCommand.Operation)
                {
                    case CacheOperation.Set:
                        _cacheManager.Set(cacheUpdateCommand.Key, cacheUpdateCommand.Value, cacheUpdateCommand.Expiration);
                        break;

                    case CacheOperation.Remove:
                        _cacheManager.Remove(cacheUpdateCommand.Key);
                        break;

                    case CacheOperation.BatchSet:
                        if (cacheUpdateCommand.BatchValues != null)
                        {
                            _cacheManager.SetBatch(cacheUpdateCommand.BatchValues, cacheUpdateCommand.Expiration);
                        }
                        break;

                    case CacheOperation.BatchRemove:
                        if (cacheUpdateCommand.BatchKeys != null)
                        {
                            _cacheManager.RemoveBatch(cacheUpdateCommand.BatchKeys);
                        }
                        break;

                    case CacheOperation.Clear:
                        _cacheManager.Clear();
                        break;
                }

                _logger.LogInformation($"会话 {command.SessionID} 更新缓存: {cacheUpdateCommand.Operation} {cacheUpdateCommand.Key}");
                
                return ConvertToApiResponse(ResponseBase.CreateSuccess("缓存更新成功")
                    .WithMetadata("Data", new { 
                        Key = cacheUpdateCommand.Key, 
                        Operation = cacheUpdateCommand.Operation 
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理缓存更新命令异常: {ex.Message}");
                return ConvertToApiResponse(ResponseBase.CreateError($"缓存更新异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_UPDATE_EXCEPTION"));
            }
        }

        /// <summary>
        /// 处理缓存同步命令
        /// </summary>
        private async Task<ResponseBase> HandleCacheSyncAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存同步命令
                var cacheSyncCommand = command as CacheSyncProtocol.CacheSyncCommand;
                if (cacheSyncCommand == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("无效的缓存同步命令", 400)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_SYNC_COMMAND"));
                }

                // 验证会话
                var sessionInfo = _sessionService.GetSession(command.SessionID);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话无效或未认证", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                // 创建缓存事件并发布
                var cacheEvent = new CacheEvent
                {
                    CacheKey = cacheSyncCommand.Key,
                    Operation = cacheSyncCommand.Operation,
                    Value = cacheSyncCommand.Value,
                    ValueType = cacheSyncCommand.ValueType != null ? Type.GetType(cacheSyncCommand.ValueType) : null,
                    Timestamp = cacheSyncCommand.Timestamp,
                    SessionId = command.SessionID
                };

                // 这里可以添加事件发布逻辑
                // await _cacheEventPublisher.PublishAsync(cacheEvent);

                _logger.LogInformation($"处理缓存同步命令: {cacheSyncCommand.Key} - {cacheSyncCommand.Operation}");
                
                return ConvertToApiResponse(ResponseBase.CreateSuccess("缓存同步处理成功")
                    .WithMetadata("Data", new { 
                        Key = cacheSyncCommand.Key, 
                        Operation = cacheSyncCommand.Operation 
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理缓存同步命令异常: {ex.Message}");
                return ConvertToApiResponse(ResponseBase.CreateError($"缓存同步异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_SYNC_EXCEPTION"));
            }
        }

        /// <summary>
        /// 处理缓存订阅命令
        /// </summary>
        private async Task<ResponseBase> HandleCacheSubscribeAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存订阅命令
                var cacheSubscribeCommand = command as CacheSyncProtocol.CacheSubscribeCommand;
                if (cacheSubscribeCommand == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("无效的缓存订阅命令", 400)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_SUBSCRIBE_COMMAND"));
                }

                // 验证会话
                var sessionInfo = _sessionService.GetSession(command.SessionID);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话无效或未认证", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                // 订阅缓存变更
                if (cacheSubscribeCommand.CacheKeys != null)
                {
                    _cacheSyncService.Subscribe(command.SessionID, cacheSubscribeCommand.CacheKeys);
                }

                _logger.LogInformation($"会话 {command.SessionID} 订阅了缓存: {string.Join(",", cacheSubscribeCommand.CacheKeys ?? new string[0])}");
                
                return ConvertToApiResponse(ResponseBase.CreateSuccess("缓存订阅成功")
                    .WithMetadata("Data", new { 
                        SessionId = command.SessionID,
                        CacheKeys = cacheSubscribeCommand.CacheKeys?.ToArray()
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理缓存订阅命令异常: {ex.Message}");
                return ConvertToApiResponse(ResponseBase.CreateError($"缓存订阅异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_SUBSCRIBE_EXCEPTION"));
            }
        }

        /// <summary>
        /// 处理缓存取消订阅命令
        /// </summary>
        private async Task<ResponseBase> HandleCacheUnsubscribeAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存取消订阅命令
                var cacheUnsubscribeCommand = command as CacheSyncProtocol.CacheUnsubscribeCommand;
                if (cacheUnsubscribeCommand == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("无效的缓存取消订阅命令", 400)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_UNSUBSCRIBE_COMMAND"));
                }

                // 验证会话
                var sessionInfo = _sessionService.GetSession(command.SessionID);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话无效或未认证", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                // 取消订阅缓存变更
                _cacheSyncService.Unsubscribe(command.SessionID);

                _logger.LogInformation($"会话 {command.SessionID} 取消订阅了缓存");
                
                return ConvertToApiResponse(ResponseBase.CreateSuccess("缓存取消订阅成功")
                    .WithMetadata("Data", new { 
                        SessionId = command.SessionID
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理缓存取消订阅命令异常: {ex.Message}");
                return ConvertToApiResponse(ResponseBase.CreateError($"缓存取消订阅异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_UNSUBSCRIBE_EXCEPTION"));
            }
        }

        /// <summary>
        /// 处理缓存请求命令
        /// </summary>
        private async Task<ResponseBase> HandleCacheRequestAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存请求命令
                var cacheRequestCommand = command as CacheSyncProtocol.CacheRequestCommand;
                if (cacheRequestCommand == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("无效的缓存请求命令", 400)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_REQUEST_COMMAND"));
                }

                // 验证会话
                var sessionInfo = _sessionService.GetSession(command.SessionID);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话无效或未认证", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                // 处理缓存请求
                object value = null;
                if (!string.IsNullOrEmpty(cacheRequestCommand.Key))
                {
                    value = _cacheManager.Get<object>(cacheRequestCommand.Key);
                }

                // 创建响应命令
                var responseCommand = new CacheSyncProtocol.CacheResponseCommand
                {
                    Key = cacheRequestCommand.Key,
                    Value = value,
                    Success = true
                };

                _logger.LogInformation($"处理缓存请求命令: {cacheRequestCommand.Key}");
                
                return ConvertToApiResponse(ResponseBase.CreateSuccess("缓存请求处理成功")
                    .WithMetadata("Data", new { 
                        Key = cacheRequestCommand.Key,
                        Value = value
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理缓存请求命令异常: {ex.Message}");
                return ConvertToApiResponse(ResponseBase.CreateError($"缓存请求异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_REQUEST_EXCEPTION"));
            }
        }

        /// <summary>
        /// 处理缓存响应命令
        /// </summary>
        private async Task<ResponseBase> HandleCacheResponseAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                // 获取缓存响应命令
                var cacheResponseCommand = command as CacheSyncProtocol.CacheResponseCommand;
                if (cacheResponseCommand == null)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("无效的缓存响应命令", 400)
                        .WithMetadata("ErrorCode", "INVALID_CACHE_RESPONSE_COMMAND"));
                }

                // 验证会话
                var sessionInfo = _sessionService.GetSession(command.SessionID);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("会话无效或未认证", 401)
                        .WithMetadata("ErrorCode", "INVALID_SESSION"));
                }

                // 处理缓存响应
                if (!cacheResponseCommand.Success)
                {
                    _logger.LogWarning($"缓存响应失败: {cacheResponseCommand.ErrorMessage}");
                }

                _logger.LogInformation($"处理缓存响应命令: {cacheResponseCommand.Key}");
                
                return ConvertToApiResponse(ResponseBase.CreateSuccess("缓存响应处理成功")
                    .WithMetadata("Data", new { 
                        Key = cacheResponseCommand.Key,
                        Success = cacheResponseCommand.Success
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理缓存响应命令异常: {ex.Message}");
                return ConvertToApiResponse(ResponseBase.CreateError($"缓存响应异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "CACHE_RESPONSE_EXCEPTION"));
            }
        }

        /// <summary>
        /// 将ResponseBase转换为ApiResponse
        /// </summary>
        private ResponseBase ConvertToApiResponse(ResponseBase baseResponse)
        {
            return new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                Code = baseResponse.Code,
                Timestamp = baseResponse.Timestamp,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata,
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
        }
    }
}