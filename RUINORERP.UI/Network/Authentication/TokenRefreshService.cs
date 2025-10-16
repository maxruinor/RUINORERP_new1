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
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
        }

        /// <summary>
        /// 刷新Token - 简化参数，移除Token验证和存储逻辑
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>登录响应</returns>
        public async Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default)
        {
            try
            {
                // 创建刷新Token请求
                SimpleRequest request = SimpleRequest.CreateString("");
                var baseCommand = CommandDataBuilder.BuildCommand<SimpleRequest, LoginResponse>(AuthenticationCommands.RefreshToken, request);
                
                // 发送请求并获取响应
                var response = await _communicationService.SendCommandAsync<SimpleRequest, LoginResponse>(
                    baseCommand, ct);

                // 从响应中提取Token信息
                if (response?.ExecutionContext?.Token != null)
                {
                    return response.ExecutionContext.Token;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token刷新服务调用失败: {ex.Message}", ex);
            }
        }

        public async Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            try
            {
                SimpleRequest request = SimpleRequest.CreateString(token);

                // 使用_communicationService发送验证Token的请求
                var bc = CommandDataBuilder.BuildCommand<SimpleRequest, LoginResponse>(AuthenticationCommands.ValidateToken, request);

                var response = await _communicationService.SendCommandAsync<SimpleRequest, LoginResponse>(bc,
                    ct, 15000);

                // 验证响应是否成功
                return response != null && !string.IsNullOrEmpty(response.ExecutionContext?.Token?.AccessToken);
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"Token验证失败: {ex.Message}");
                return false;
            }
        }
    }
}