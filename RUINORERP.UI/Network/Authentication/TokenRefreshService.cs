using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Requests.Authentication;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    public class TokenRefreshService : ITokenRefreshService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly TokenManager _tokenManager;

        public TokenRefreshService(ClientCommunicationService communicationService, TokenManager tokenManager)
        {
            _communicationService = communicationService;
            _tokenManager = tokenManager;
        }

        /// <summary>
        /// 刷新Token - 从TokenManager获取当前Token并刷新
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>新的Token信息</returns>
        public async Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default)
        {
            // 从TokenManager获取当前Token
            var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
            if (currentToken == null || string.IsNullOrEmpty(currentToken.AccessToken))
            {
                throw new InvalidOperationException("没有可用的Token进行刷新");
            }
            
            try
            {
                // 创建刷新Token请求
                SimpleRequest request = SimpleRequest.CreateString(currentToken.AccessToken);

                // 发送请求并获取响应
                var response = await _communicationService.SendCommandAsync(AuthenticationCommands.RefreshToken, request, ct);

                // 从响应中提取Token信息
                if (response?.ExecutionContext?.Token != null)
                {
                    // 自动更新存储的Token
                    await _tokenManager.TokenStorage.SetTokenAsync(response.ExecutionContext.Token);
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
                var response = await _communicationService.SendCommandAsync(AuthenticationCommands.ValidateToken, request, ct);
                return response?.Response.IsSuccess ?? false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token验证服务调用失败: {ex.Message}", ex);
            }
        }
    }
}