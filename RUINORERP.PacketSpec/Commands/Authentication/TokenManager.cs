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
        public ITokenStorage TokenStorage { get; }

        public TokenManager(ITokenService tokenService, ITokenStorage tokenStorage)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            TokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        }

        /// <summary>
        /// 生成Token并存储 - 完整版，支持附加声明和刷新Token
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明，可选</param>
        /// <returns>包含访问令牌和刷新令牌的TokenInfo</returns>
        public async Task<TokenInfo> GenerateTokenAsync(string userId, string userName, Dictionary<string, object> additionalClaims = null)
        {
            try
            {
                // 使用GenerateTokens方法生成令牌对
                var (accessToken, refreshToken) = _tokenService.GenerateTokens(userId, userName, additionalClaims);
                
                var tokenInfo = new TokenInfo
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.Now.AddHours(8), // 访问令牌8小时过期
                    RefreshTokenExpiresAt = DateTime.Now.AddDays(7) // 刷新令牌7天过期
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
        /// 注意：返回bool表示验证是否成功
        /// </summary>
        public async Task<bool> ValidateTokenAsync(string token)
        {
            // 包装同步方法为异步调用，并提取验证结果
            var result = await Task.Run(() => _tokenService.ValidateToken(token));
            return result?.IsValid == true;
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
    }
}
