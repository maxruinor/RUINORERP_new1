using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 配置同步服务类
    /// 负责向服务器主动请求最新配置文件
    /// 处理配置同步相关的业务逻辑
    /// </summary>
    public sealed class ConfigSyncService : IDisposable
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ILogger<ConfigSyncService> _logger;
        private readonly ClientEventManager _eventManager;
        private bool _isDisposed = false;
        private readonly SemaphoreSlim _syncLock = new SemaphoreSlim(3, 3); // 限制并发同步请求数量

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventManager">客户端事件管理器</param>
        /// <param name="communicationService">客户端通信服务</param>
        /// <param name="logger">日志记录器</param>
        public ConfigSyncService(
            ClientEventManager eventManager,
            IClientCommunicationService communicationService,
            ILogger<ConfigSyncService> logger = null)
        {
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger;

            // 订阅连接状态变更事件
            _eventManager.ConnectionStatusChanged += OnConnectionStatusChanged;
        }

        /// <summary>
        /// 向服务器请求最新配置文件
        /// </summary>
        /// <param name="configTypes">需要请求的配置类型列表，如果为空则请求所有配置</param>
        /// <param name="forceRefresh">是否强制刷新配置</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>配置同步响应结果</returns>
        public async Task<bool> RequestLatestConfigAsync(
            List<string> configTypes = null,
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            try
            {
                // 验证连接状态
                if (!_communicationService.IsConnected)
                {
                    _logger?.LogWarning("配置同步失败：未连接到服务器");
                    return false;
                }

                // 使用信号量限制并发请求
                await _syncLock.WaitAsync(TimeSpan.FromSeconds(10), ct);
                
                try
                {
                    // 创建配置同步请求
                    var request = new GeneralRequest
                    {
                        Data = new Dictionary<string, object>
                        {
                            ["Action"] = "RequestConfig",
                            ["ConfigTypes"] = configTypes ?? new List<string>(),
                            ["ForceRefresh"] = forceRefresh
                        }
                    };

                    // 发送配置请求命令
                    _logger?.LogInformation("向服务器请求最新配置文件，配置类型数量: {ConfigTypeCount}, 强制刷新: {ForceRefresh}", 
                        configTypes?.Count ?? 0, forceRefresh);

                    var response = await _communicationService.SendCommandWithResponseAsync<GeneralResponse>(
                        GeneralCommands.ConfigSync, 
                        request, 
                        ct, 
                        30000); // 30秒超时

                    // 检查响应结果
                    if (response == null)
                    {
                        _logger?.LogError("配置同步失败：服务器返回了空的响应数据");
                        return false;
                    }

                    if (!response.IsSuccess)
                    {
                        _logger?.LogError("配置同步失败：{ErrorMessage}", response.ErrorMessage);
                        return false;
                    }

                    _logger?.LogInformation("配置同步请求成功发送，等待服务器响应配置数据");
                    return true;
                }
                finally
                {
                    // 确保释放信号量
                    _syncLock.Release();
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger?.LogWarning(ex, "配置同步请求已被取消（可能是超时）");
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "请求最新配置文件时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 请求特定类型的配置文件
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="forceRefresh">是否强制刷新配置</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>配置同步响应结果</returns>
        public async Task<bool> RequestSpecificConfigAsync(
            string configType,
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(configType))
            {
                throw new ArgumentException("配置类型不能为空", nameof(configType));
            }

            var configTypes = new List<string> { configType };
            return await RequestLatestConfigAsync(configTypes, forceRefresh, ct);
        }

        /// <summary>
        /// 批量请求常用配置文件
        /// </summary>
        /// <param name="forceRefresh">是否强制刷新配置</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>配置同步响应结果</returns>
        public async Task<bool> RequestCommonConfigsAsync(
            bool forceRefresh = false,
            CancellationToken ct = default)
        {
            // 常用配置类型列表
            var commonConfigTypes = new List<string>
            {
                "SystemGlobalConfig",
                "GlobalValidatorConfig",
                "Database"
            };

            return await RequestLatestConfigAsync(commonConfigTypes, forceRefresh, ct);
        }

        /// <summary>
        /// 处理连接状态变更事件
        /// </summary>
        private void OnConnectionStatusChanged(bool connected)
        {
            if (connected)
            {
                _logger?.LogDebug("连接已建立，可以进行配置同步操作");
            }
            else
            {
                _logger?.LogDebug("连接已断开，暂停配置同步操作");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            try
            {
                // 取消订阅事件
                if (_eventManager != null)
                {
                    _eventManager.ConnectionStatusChanged -= OnConnectionStatusChanged;
                }
                _syncLock.Dispose();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "释放ConfigSyncService资源时发生异常");
            }
        }
    }
}