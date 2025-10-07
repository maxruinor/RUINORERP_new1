using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token存储接口 - 定义Token的存储和获取抽象方法
    /// 统一异步接口，简化Token生命周期管理
    /// </summary>
    public interface ITokenStorage
    {
        /// <summary>
        /// 异步获取Token信息
        /// </summary>
        /// <returns>TokenInfo对象，如果不存在则返回null</returns>
        Task<TokenInfo> GetTokenAsync();

        /// <summary>
        /// 异步设置Token信息
        /// </summary>
        /// <param name="tokenInfo">完整的Token信息对象</param>
        Task SetTokenAsync(TokenInfo tokenInfo);

        /// <summary>
        /// 异步清除存储的Token信息
        /// </summary>
        Task ClearTokenAsync();

        /// <summary>
        /// 异步检查Token是否有效（未过期且存在）
        /// </summary>
        /// <returns>如果Token有效则返回true，否则返回false</returns>
        Task<bool> IsTokenValidAsync();
    }

    /// <summary>
    /// Token管理器 - 简化的Token管理实现
    /// 提供核心的Token生成、验证和存储功能
    /// </summary>
    public class TokenManager
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenStorage _tokenStorage;

        public TokenManager(ITokenService tokenService, ITokenStorage tokenStorage)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        }

        /// <summary>
        /// 生成并存储Token
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="claims">自定义声明</param>
        /// <returns>生成的Token信息</returns>
        public async Task<TokenInfo> GenerateAndStoreTokenAsync(string userId, string userName, IDictionary<string, object> claims = null)
        {
            var token = _tokenService.GenerateToken(userId, userName, claims);
            var tokenInfo = new TokenInfo { AccessToken = token };
            await _tokenStorage.SetTokenAsync(tokenInfo);
            return tokenInfo;
        }

        /// <summary>
        /// 验证存储的Token
        /// </summary>
        /// <returns>Token验证结果</returns>
        public async Task<TokenValidationResult> ValidateStoredTokenAsync()
        {
            var tokenInfo = await _tokenStorage.GetTokenAsync();
            if (tokenInfo == null)
                return new TokenValidationResult { IsValid = false, ErrorMessage = "No token found" };
            
            return _tokenService.ValidateToken(tokenInfo.AccessToken);
        }

        /// <summary>
        /// 获取Token存储（用于兼容现有代码）
        /// </summary>
        public ITokenStorage TokenStorage => _tokenStorage;

        /// <summary>
        /// 清除Token
        /// </summary>
        public Task ClearTokenAsync() => _tokenStorage.ClearTokenAsync();

        /// <summary>
        /// 刷新Token - 简化版Token刷新实现
        /// 验证当前Token并生成新的Token对
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <param name="currentAccessToken">当前访问Token</param>
        /// <returns>Token刷新结果</returns>
        public async Task<(bool Success, string AccessToken, string ErrorMessage)> RefreshTokenAsync(string refreshToken, string currentAccessToken)
        {
            try
            {
                // 验证当前Token
                var validationResult = _tokenService.ValidateToken(currentAccessToken);
                if (!validationResult.IsValid)
                {
                    return (false, null, "Invalid current token");
                }

                // 验证刷新Token
                var refreshValidation = _tokenService.ValidateToken(refreshToken);
                if (!refreshValidation.IsValid)
                {
                    return (false, null, "Invalid refresh token");
                }

                // 从当前Token获取用户信息
                var userId = validationResult.UserId;
                var userName = validationResult.UserName;

                // 生成新的Token对
                var newToken = _tokenService.RefreshToken(refreshToken, currentAccessToken);

                // 更新存储的Token信息
                var newTokenInfo = new TokenInfo 
                { 
                    AccessToken = newToken
                };
                await _tokenStorage.SetTokenAsync(newTokenInfo);

                return (true, newToken, null);
            }
            catch (Exception ex)
            {
                return (false, null, $"Token refresh failed: {ex.Message}");
            }
        }
    }
}
