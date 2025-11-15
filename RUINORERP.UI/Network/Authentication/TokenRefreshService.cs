using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Authentication;
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
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        public async Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default)
        {
            var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
            if (currentToken == null || string.IsNullOrEmpty(currentToken.RefreshToken))
            {
                throw new InvalidOperationException("没有可用的刷新令牌");
            }

            var request = TokenRefreshRequest.Create(currentToken.RefreshToken);
            var response = await _communicationService.SendCommandWithResponseAsync<TokenRefreshResponse>(
                AuthenticationCommands.RefreshToken, request, ct);

            if (response?.IsSuccess == true)
            {
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

        public async Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            var tokenInfo = new TokenInfo { AccessToken = token };
            var request = TokenValidationRequest.Create(tokenInfo);
            
            var response = await _communicationService.SendCommandWithResponseAsync<TokenValidationResponse>(
                AuthenticationCommands.ValidateToken, request, ct);
                
            return response?.IsSuccess ?? false;
        }
    }
}