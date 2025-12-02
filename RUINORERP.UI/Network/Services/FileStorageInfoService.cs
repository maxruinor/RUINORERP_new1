using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 文件存储信息服务类
    /// 提供文件存储使用情况查询等核心功能
    /// </summary>
    public sealed class FileStorageInfoService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<FileStorageInfoService> _log;
        private readonly SemaphoreSlim _operationLock = new SemaphoreSlim(1, 1); // 防止并发操作请求
        private bool _isDisposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当必要参数为空时抛出</exception>
        public FileStorageInfoService(
            ClientCommunicationService communicationService,
            ILogger<FileStorageInfoService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _log = logger;
        }

        /// <summary>
        /// 获取文件存储使用信息
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>存储使用信息响应</returns>
        public async Task<StorageUsageInfo> GetStorageUsageInfoAsync(CancellationToken ct = default)
        {
            // 使用信号量确保同一时间只有一个操作请求
            await _operationLock.WaitAsync(ct);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
                {
                    _log?.LogWarning("获取存储使用信息失败：未连接到服务器");
                    return StorageUsageInfo.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                // 只记录关键信息，移除详细的操作日志
                _log?.LogDebug("开始获取存储使用信息请求");

                // 发送获取存储使用信息命令并获取响应
                var response = await _communicationService.SendCommandWithResponseAsync<StorageUsageInfo>(
                    FileCommands.FileStorageInfo, new RequestBase(), ct);

                // 检查响应数据是否为空
                if (response == null)
                {
                    _log?.LogError("获取存储使用信息失败：服务器返回了空的响应数据");
                    return StorageUsageInfo.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    _log?.LogWarning("获取存储使用信息失败: {ErrorMessage}", response.ErrorMessage);
                    return StorageUsageInfo.CreateFailure($"获取存储使用信息失败: {response.ErrorMessage}");
                }

                // 只记录关键信息，简化日志内容
                return response;
            }
            catch (OperationCanceledException ex)
            {
                return StorageUsageInfo.CreateFailure("获取存储使用信息操作已取消");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取存储使用信息过程中发生未预期的异常");
                return StorageUsageInfo.CreateFailure("获取存储使用信息过程中发生错误，请稍后重试");
            }
            finally
            {
                _operationLock.Release();
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
                // 释放资源
                _operationLock.Dispose();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "释放FileStorageInfoService资源时发生异常");
            }
        }
    }
}