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
        /// 优化版：改进错误处理，区分不同类型的失败
        /// </summary>
        public async Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default)
        {
            var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
            if (currentToken == null || string.IsNullOrEmpty(currentToken.RefreshToken))
            {
                throw new InvalidOperationException("没有可用的刷新令牌");
            }

            // 检查刷新令牌是否已过期
            if (currentToken.IsRefreshTokenExpired())
            {
                throw new InvalidOperationException("刷新令牌已过期，需要重新登录");
            }

            try
            {
                var request = new TokenRefreshRequest { RefreshToken = currentToken.RefreshToken };
                var response = await _communicationService.SendCommandWithResponseAsync<TokenRefreshResponse>(
                    AuthenticationCommands.RefreshToken, request, ct, 10000); // 10秒超时

                if (response?.IsSuccess == true)
                {
                    // 创建并存储新的TokenInfo
                    var newToken = new TokenInfo
                    {
                        AccessToken = response.NewAccessToken,
                        RefreshToken = response.NewRefreshToken,
                        ExpiresAt = response.ExpireTime,
                        RefreshTokenExpiresAt = currentToken.RefreshTokenExpiresAt // 保留原有的刷新令牌过期时间
                    };
                    
                    await _tokenManager.TokenStorage.SetTokenAsync(newToken);
                    return newToken;
                }

                // 根据错误类型抛出不同的异常
                if (response?.ErrorMessage?.Contains("过期") == true || 
                    response?.ErrorMessage?.Contains("expired") == true)
                {
                    throw new InvalidOperationException($"刷新令牌已过期: {response?.ErrorMessage}");
                }
                
                throw new Exception($"Token刷新失败: {response?.ErrorMessage}");
            }
            catch (TimeoutException)
            {
                // 超时异常，可能是网络问题
                throw new InvalidOperationException("Token刷新请求超时，请检查网络连接");
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                // 其他异常，包装后抛出
                throw new InvalidOperationException($"Token刷新过程中发生错误: {ex.Message}", ex);
            }
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