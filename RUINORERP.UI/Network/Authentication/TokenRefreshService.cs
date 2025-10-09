using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// 客户端Token请求刷新服务
    /// </summary>
    public class TokenRefreshService : ITokenRefreshService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly TokenManager _tokenManager;
        public TokenRefreshService(ClientCommunicationService communicationService, TokenManager tokenManager)
        {
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _communicationService = communicationService;
        }

        /// <summary>
        /// 获取设备标识
        /// </summary>
        /// <returns>设备标识</returns>
        private string GetDeviceId()
        {
            try
            {
                // 使用机器名和用户名组合生成设备标识
                var machineName = Environment.MachineName;
                var userName = Environment.UserName;
                return $"{machineName}_{userName}";
            }
            catch
            {
                // 如果获取失败，使用GUID
                return Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns>客户端IP地址</returns>
        private string GetClientIp()
        {
            try
            {
                // 获取本地机器的IP地址
                var hostName = System.Net.Dns.GetHostName();
                var hostEntry = System.Net.Dns.GetHostEntry(hostName);
                var ipAddress = hostEntry.AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                return ipAddress?.ToString() ?? "127.0.0.1";
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// 刷新Token - 简化参数，移除Token验证和存储逻辑
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>登录响应</returns>
        public async Task<LoginResponse> RefreshTokenAsync(CancellationToken ct = default)
        {
            try
            {
                SimpleRequest request = SimpleRequest.CreateString("");
                var baseCommand = CommandDataBuilder.BuildCommand<SimpleRequest, LoginResponse>(AuthenticationCommands.RefreshToken, request);
                var response = await _communicationService.SendCommandAsync<SimpleRequest, LoginResponse>(
                    baseCommand, ct);

                LoginResponse loginResponse = null;

                //像登陆一样 取值

                return loginResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token刷新服务调用失败: {ex.Message}", ex);
            }
        }

        public async Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
        {

            SimpleRequest request = SimpleRequest.CreateString(token);

            // 使用_communicationService发送验证Token的请求
            var bc = CommandDataBuilder.BuildCommand<SimpleRequest, LoginResponse>(AuthenticationCommands.ValidateToken, request);

            var response = await _communicationService.SendCommandAsync<SimpleRequest, LoginResponse>(bc,
                ct, 15000);

            // 验证响应是否成功
            return response != null && !string.IsNullOrEmpty(response.ExecutionContext.Token.AccessToken);
        }
    }
}
