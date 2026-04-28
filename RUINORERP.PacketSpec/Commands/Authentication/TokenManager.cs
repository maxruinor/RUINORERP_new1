using System;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    public interface ITokenStorage
    {
        Task<TokenInfo> GetTokenAsync();
        Task SetTokenAsync(TokenInfo tokenInfo);
        Task ClearTokenAsync();
        Task<bool> IsTokenValidAsync();
    }

    /// <summary>
    /// Token管理器 - 简化版
    /// 专注于核心的Token管理功能
    /// </summary>
    public class TokenManager
    {
        private readonly ITokenService _tokenService;
        private readonly TokenServiceOptions _options;  // 新增：配置选项
        public ITokenStorage TokenStorage { get; }

        public TokenManager(ITokenService tokenService, ITokenStorage tokenStorage, TokenServiceOptions options)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            TokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
            _options = options ?? throw new ArgumentNullException(nameof(options));  // 新增
        }

        /// <summary>
        /// 生成Token并存储（单令牌机制）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明，可选</param>
        /// <returns>包含访问令牌的TokenInfo</returns>
        public async Task<TokenInfo> GenerateTokenAsync(string userId, string userName, Dictionary<string, object> additionalClaims = null)
        {
            try
            {
                // 使用GenerateAccessToken方法生成单一访问令牌
                var accessToken = _tokenService.GenerateAccessToken(userId, userName, additionalClaims);
                
                var tokenInfo = new TokenInfo
                {
                    AccessToken = accessToken,
                    ExpiresAt = DateTime.Now.AddHours(_options.DefaultExpiryHours)  // ✅ 使用配置
                };

                // 存储Token信息
                await TokenStorage.SetTokenAsync(tokenInfo);
                return tokenInfo;
            }
            catch (Exception ex)
            {
                // 记录错误但不抛出异常，返回null表示生成失败
                System.Diagnostics.Debug.WriteLine($"Token生成失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 验证Token有效性
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <returns>Token是否有效</returns>
        public Task<bool> ValidateTokenAsync(string token)
        {
            var result = _tokenService.ValidateToken(token);
            return Task.FromResult(result?.IsValid == true);
        }

        /// <summary>
        /// 清除存储的Token
        /// </summary>
        public async Task ClearTokenAsync()
        {
            await TokenStorage.ClearTokenAsync();
        }
        
        /// <summary>
        /// 检查Token是否存在且有效
        /// </summary>
        public async Task<bool> HasValidTokenAsync()
        {
            return await TokenStorage.IsTokenValidAsync();
        }

        /// <summary>
        /// 刷新Token（延长一个周期）
        /// 简单实现：获取当前Token，延长过期时间
        /// </summary>
        public async Task<TokenInfo> RefreshTokenAsync()
        {
            try
            {
                var currentToken = await TokenStorage.GetTokenAsync();
                if (currentToken == null || string.IsNullOrEmpty(currentToken.AccessToken))
                {
                    return null;
                }

                var refreshedToken = new TokenInfo
                {
                    AccessToken = currentToken.AccessToken,
                    TokenType = currentToken.TokenType,
                    ExpiresAt = DateTime.Now.AddHours(_options.DefaultExpiryHours)
                };

                await TokenStorage.SetTokenAsync(refreshedToken);
                return refreshedToken;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Token刷新失败: {ex.Message}");
                return null;
            }
        }
    }
}
