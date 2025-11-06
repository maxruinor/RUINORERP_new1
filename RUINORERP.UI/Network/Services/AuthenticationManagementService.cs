using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using static RUINORERP.PacketSpec.Models.Requests.SystemCommandRequest;
using RUINORERP.UI.Network;
using RUINORERP.PacketSpec.Models.Responses.Message;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 认证管理服务 - 提供强制用户下线等认证管理功能
    /// </summary>
    public class AuthenticationManagementService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<AuthenticationManagementService> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuthenticationManagementService(
            ClientCommunicationService communicationService,
            ILogger<AuthenticationManagementService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger;
        }

        /// <summary>
        /// 强制用户下线
        /// </summary>
        /// <param name="targetUserId">目标用户ID</param>
        /// <param name="adminUserId">管理员ID</param>
        /// <param name="reason">强制下线原因</param>
        /// <param name="adminRemark">管理员备注</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>强制下线响应</returns>
        public async Task<SystemCommandResponse> ForceLogoutAsync(
            string targetUserId,
            string adminUserId,
            string reason = "管理员强制下线",
            string adminRemark = "",
            CancellationToken ct = default)
        {
            try
            {
                var request = SystemCommandRequest.CreateForceLogoutRequest(targetUserId, adminUserId, reason, adminRemark);
                var response = await _communicationService.SendCommandWithResponseAsync<SystemCommandResponse>(
                    AuthenticationCommands.ForceLogout, request, ct);

                if (response != null && response.IsSuccess)
                {
                    _logger?.LogDebug("强制用户下线指令发送成功 - 目标用户: {TargetUserId}", targetUserId);
                }
                else
                {
                    _logger?.LogWarning("强制用户下线指令发送失败 - 目标用户: {TargetUserId}, 错误: {ErrorMessage}", 
                        targetUserId, response?.Message ?? "未知错误");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送强制用户下线指令时发生异常 - 目标用户: {TargetUserId}", targetUserId);
                return SystemCommandResponse.CreateForceLogoutFailure($"指令发送失败: {ex.Message}", "FORCE_LOGOUT_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理接收到的强制下线通知
        /// 默认错误返回有问题，先这样写让编译器通过
        /// </summary>
        public async Task<IResponse> HandleForceLogoutNotificationAsync(SystemCommandRequest request)
        {
            try
            {
                _logger?.LogDebug("收到强制下线通知 - 管理员: {AdminUserId}, 原因: {Reason}", 
                    request.AdminUserId, request.Reason);

                // 这里应该执行实际的下线操作，例如：
                // 1. 显示通知给用户
                // 2. 自动执行登出流程
                // 3. 关闭应用程序或跳转到登录界面

                // 模拟异步处理
                await Task.Delay(100);

                var response = new MessageResponse
                {
                    IsSuccess = true,
                    Message = "已收到强制下线通知"
                };

                _logger?.LogDebug("强制下线通知处理完成");
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理强制下线通知时发生异常");
                return ResponseFactory.CreateSpecificErrorResponse<IResponse>($"处理通知失败: {ex.Message}")
                  ;
            }
        }
    }
}