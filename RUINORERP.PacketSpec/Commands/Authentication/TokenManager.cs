using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    public interface ITokenStorage
    {
        Task<TokenInfo> GetTokenAsync();
        Task SetTokenAsync(TokenInfo tokenInfo);
        Task ClearTokenAsync();
        Task<bool> IsTokenValidAsync();
    }

    public class TokenManager
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenStorage _tokenStorage;

        public TokenManager(ITokenService tokenService, ITokenStorage tokenStorage)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        }

        public async Task<TokenInfo> GenerateAndStoreTokenAsync(string userId, string userName, IDictionary<string, object> claims = null)
        {
            // 如果TokenService支持生成令牌对（访问令牌和刷新令牌）
            if (_tokenService is JwtTokenService jwtTokenService)
            {
                var tokens = jwtTokenService.GenerateTokens(userId, userName, claims);
                var tokenInfo = new TokenInfo 
                { 
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                    ExpiresAt = DateTime.Now.AddHours(8)
                };
                
                await _tokenStorage.SetTokenAsync(tokenInfo);
                return tokenInfo;
            }
            else
            {
                // 兼容旧版本，仅生成访问令牌
                var token = _tokenService.GenerateToken(userId, userName, claims);
                var tokenInfo = new TokenInfo 
                { 
                    AccessToken = token,
                    ExpiresAt = DateTime.Now.AddHours(8)
                };
                
                await _tokenStorage.SetTokenAsync(tokenInfo);
                return tokenInfo;
            }
        }

        public async Task<TokenValidationResult> ValidateStoredTokenAsync()
        {
            var tokenInfo = await _tokenStorage.GetTokenAsync();
            if (tokenInfo == null)
                return new TokenValidationResult { IsValid = false, ErrorMessage = "未找到Token" };
            
            return _tokenService.ValidateToken(tokenInfo.AccessToken);
        }

        public ITokenStorage TokenStorage => _tokenStorage;

        public Task ClearTokenAsync() => _tokenStorage.ClearTokenAsync();
    }
}
