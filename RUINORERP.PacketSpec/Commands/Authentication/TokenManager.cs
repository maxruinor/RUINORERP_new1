﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token存储接口
    /// 定义Token的存储和获取抽象方法
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
    /// Token管理器
    /// 负责Token的生成、验证、刷新和存储管理
    /// </summary>
    public class TokenManager
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenStorage _tokenStorage;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tokenService">令牌服务实例</param>
        /// <param name="tokenStorage">令牌存储实例</param>
        /// <exception cref="ArgumentNullException">当服务或存储为空时抛出</exception>
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
        /// <param name="claims">自定义声明（可选）</param>
        /// <returns>生成的Token信息</returns>
        public async Task<TokenInfo> GenerateAndStoreTokenAsync(string userId, string userName, IDictionary<string, object> claims = null)
        {
            var token = _tokenService.GenerateToken(userId, userName, claims);
            var tokenInfo = new TokenInfo 
            { 
                AccessToken = token,
                ExpiresAt = DateTime.Now.AddHours(8) // 默认8小时过期
            };
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
                return new TokenValidationResult { IsValid = false, ErrorMessage = "未找到Token" };
            
            return _tokenService.ValidateToken(tokenInfo.AccessToken);
        }

        /// <summary>
        /// 获取Token存储实例
        /// </summary>
        public ITokenStorage TokenStorage => _tokenStorage;

        /// <summary>
        /// 清除Token
        /// </summary>
        public Task ClearTokenAsync() => _tokenStorage.ClearTokenAsync();

        /// <summary>
        /// 刷新Token
        /// 使用刷新令牌生成新的访问令牌
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <param name="currentAccessToken">当前访问Token</param>
        /// <returns>刷新后的Token信息</returns>
        /// <exception cref="ArgumentException">当参数无效时抛出</exception>
        /// <exception cref="SecurityTokenException">当Token验证失败时抛出</exception>
        public async Task<TokenInfo> RefreshTokenAsync(string refreshToken, string currentAccessToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException("刷新令牌不能为空", nameof(refreshToken));
            
            if (string.IsNullOrEmpty(currentAccessToken))
                throw new ArgumentException("当前访问令牌不能为空", nameof(currentAccessToken));

            try
            {
                // 验证当前Token
                var validationResult = _tokenService.ValidateToken(currentAccessToken);
                if (!validationResult.IsValid)
                {
                    throw new SecurityTokenException("当前令牌无效，无法刷新");
                }

                // 验证刷新Token
                var refreshValidation = _tokenService.ValidateToken(refreshToken);
                if (!refreshValidation.IsValid)
                {
                    throw new SecurityTokenException("刷新令牌无效");
                }

                // 确保两个Token属于同一用户
                if (validationResult.UserId != refreshValidation.UserId)
                {
                    throw new SecurityTokenException("令牌用户不一致");
                }

                // 生成新的Token
                var newToken = _tokenService.RefreshToken(refreshToken, currentAccessToken);

                // 更新存储的Token信息
                var newTokenInfo = new TokenInfo 
                { 
                    AccessToken = newToken,
                    ExpiresAt = DateTime.Now.AddHours(8) // 默认8小时过期
                };
                await _tokenStorage.SetTokenAsync(newTokenInfo);

                return newTokenInfo;
            }
            catch (SecurityTokenException)
            {
                throw; // 直接抛出安全令牌异常
            }
            catch (Exception ex)
            {
                throw new Exception($"Token刷新失败: {ex.Message}", ex);
            }
        }
    }
}
