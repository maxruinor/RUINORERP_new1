using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 系统管理服务 - 提供电脑状态查询、远程关闭等系统管理功能
    /// </summary>
    public class SystemManagementService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<SystemManagementService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemManagementService(
            ClientCommunicationService communicationService,
            ILogger<SystemManagementService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger;
        }

        /// <summary>
        /// 查询电脑状态
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>电脑状态响应</returns>
        public async Task<SystemCommandResponse> QueryComputerStatusAsync(
            string targetUserId,
            CancellationToken ct = default)
        {
            try
            {
                var request = SystemCommandRequest.CreateComputerStatusRequest(targetUserId, "Status");
                var response = await _communicationService.SendCommandWithResponseAsync<SystemCommandResponse>(
                    SystemCommands.ComputerStatus, request, ct);

                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("电脑状态查询成功 - 目标用户: {TargetUserId}", targetUserId);
                }
                else
                {
                    _logger?.LogWarning("电脑状态查询失败 - 目标用户: {TargetUserId}, 错误: {ErrorMessage}",
                        targetUserId, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "查询电脑状态时发生异常 - 目标用户: {TargetUserId}", targetUserId);
                return SystemCommandResponse.CreateComputerStatusFailure($"查询失败: {ex.Message}", "STATUS_QUERY_EXCEPTION");
            }
        }



        /// <summary>
        /// 处理接收到的电脑状态查询请求
        /// </summary>
        public async Task<SystemCommandResponse> HandleComputerStatusRequestAsync(SystemCommandRequest request)
        {
            try
            {
                // 获取本地电脑状态信息
                var computerStatus = await GetLocalComputerStatusAsync();

                var response = SystemCommandResponse.CreateComputerStatusSuccess(
                    MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID.ToString(),
                    MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName,
                    Environment.MachineName,
                    HLH.Lib.Net.IpAddressHelper.GetLocIP(),
                    computerStatus.CpuUsage,
                    computerStatus.MemoryUsage,
                    computerStatus.DiskUsage,
                    computerStatus.SystemUptime,
                    System.Windows.Forms.Application.ProductVersion,
                    _communicationService.IsConnected ? "Connected" : "Disconnected"
                );

                _logger?.LogDebug("本地电脑状态信息已生成");
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取本地电脑状态信息时发生异常");
                return SystemCommandResponse.CreateComputerStatusFailure($"获取状态信息失败: {ex.Message}", "LOCAL_STATUS_ERROR");
            }
        }

        /// <summary>
        /// 处理接收到的关闭电脑请求
        /// </summary>
        public async Task<SystemCommandResponse> HandleShutdownRequestAsync(SystemCommandRequest request)
        {
            try
            {
                // 执行关闭操作
                await ExecuteShutdownAsync(request);

                var response = SystemCommandResponse.CreateShutdownSuccess(
                    MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID.ToString(),
                    MainForm.Instance.AppContext.CurUserInfo.UserInfo.UserName,
                    Environment.MachineName
                );

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行关闭电脑指令时发生异常");
                return SystemCommandResponse.CreateShutdownFailure($"执行关闭指令失败: {ex.Message}", "EXECUTE_SHUTDOWN_ERROR");
            }
        }

        /// <summary>
        /// 获取本地电脑状态信息
        /// </summary>
        private async Task<LocalComputerStatus> GetLocalComputerStatusAsync()
        {
            // 这里应该实现获取真实电脑状态的逻辑
            // 为了示例，我们返回模拟数据
            await Task.Delay(100); // 模拟异步操作

            return new LocalComputerStatus
            {
                CpuUsage = 25.5f,
                MemoryUsage = 45.2f,
                DiskUsage = new System.Collections.Generic.Dictionary<string, DiskInfo>
                {
                    { "C:", new DiskInfo { Name = "C:", TotalSize = 500, AvailableSpace = 200, UsagePercentage = 60 } }
                },
                SystemUptime = (long)(DateTime.Now - System.Diagnostics.Process.GetCurrentProcess().StartTime).TotalSeconds
            };
        }

        /// <summary>
        /// 执行关闭操作
        /// </summary>
        private async Task ExecuteShutdownAsync(SystemCommandRequest request)
        {
            // 这里应该实现真实的关闭逻辑
            // 为了示例，我们只记录日志
            await Task.Delay(100); // 模拟异步操作


            // 实际实现中，这里会执行真正的关闭操作
            // 例如调用Windows API或其他系统命令
        }
    }
}