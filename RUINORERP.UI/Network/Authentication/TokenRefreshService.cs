using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// Token刷新服务 - 简化版
    /// 专注于核心的Token刷新和验证功能
    /// </summary>
    public class TokenRefreshService : ITokenRefreshService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly TokenManager _tokenManager;

        public TokenRefreshService(ClientCommunicationService communicationService, TokenManager tokenManager)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        /// <summary>
        /// 刷新Token
        /// 简化版：直接使用TokenManager获取当前Token，减少重复代码
        /// </summary>
        public async Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default)
        {
            var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
            if (currentToken == null || string.IsNullOrEmpty(currentToken.RefreshToken))
            {
                throw new InvalidOperationException("没有可用的刷新令牌");
            }

            var request = new TokenRefreshRequest { RefreshToken = currentToken.RefreshToken };
            var response = await _communicationService.SendCommandWithResponseAsync<TokenRefreshResponse>(
                AuthenticationCommands.RefreshToken, request, ct);

            if (response?.IsSuccess == true)
            {
                // 创建并存储新的TokenInfo
                var newToken = new TokenInfo
                {
                    AccessToken = response.NewAccessToken,
                    RefreshToken = response.NewRefreshToken,
                    ExpiresAt = response.ExpireTime
                };
                
                await _tokenManager.TokenStorage.SetTokenAsync(newToken);
                return newToken;
            }

            throw new Exception($"Token刷新失败: {response?.ErrorMessage}");
        }

        /// <summary>
        /// 验证Token
        /// 简化版：直接使用客户端通信服务发送验证请求
        /// </summary>
        public async Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            var request = new TokenValidationRequest { Token = new TokenInfo { AccessToken = token } };
            
            try
            {
                var response = await _communicationService.SendCommandWithResponseAsync<TokenValidationResponse>(
                    AuthenticationCommands.ValidateToken, request, ct);
                    
                return response?.IsSuccess ?? false;
            }
            catch (Exception ex)
            {
                // 验证失败时返回false，而不是抛出异常
                System.Diagnostics.Debug.WriteLine($"Token验证异常: {ex.Message}");
                return false;
            }
        }
    }
}