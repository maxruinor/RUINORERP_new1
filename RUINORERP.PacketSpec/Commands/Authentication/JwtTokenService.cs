﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// JWT令牌服务实现
    /// 负责生成、验证和刷新JWT令牌，支持令牌撤销
    /// </summary>
    public class JwtTokenService : ITokenService
    {
        private readonly TokenServiceOptions _options;
        private readonly SigningCredentials _signingCredentials;
        
        /// <summary>
        /// 令牌黑名单最大容量（防止内存溢出）
        /// </summary>
        private const int MaxRevokedTokensCapacity = 10000;
        
        /// <summary>
        /// 令牌黑名单（内存实现，生产环境建议使用Redis）
        /// Key: JWT ID (jti), Value: 过期时间
        /// </summary>
        private static readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new ConcurrentDictionary<string, DateTime>();
        
        /// <summary>
        /// 日志记录器（可选）
        /// </summary>
        private static readonly Action<string> _logger = msg => System.Diagnostics.Debug.WriteLine($"[JwtTokenService] {msg}");

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options">令牌服务配置选项</param>
        /// <exception cref="ArgumentNullException">当选项为空时抛出</exception>
        /// <exception cref="ArgumentException">当密钥为空时抛出</exception>
        public JwtTokenService(TokenServiceOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            
            if (string.IsNullOrEmpty(_options.SecretKey))
                throw new ArgumentException("SecretKey不能为空", nameof(options.SecretKey));

            var key = Encoding.UTF8.GetBytes(_options.SecretKey);
            var securityKey = new SymmetricSecurityKey(key);
            _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// 生成令牌对（访问令牌和刷新令牌）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明（可选）</param>
        /// <returns>包含访问令牌和刷新令牌的元组</returns>
        public (string AccessToken, string RefreshToken) GenerateTokens(string userId, string userName, IDictionary<string, object> additionalClaims = null)
        {
            var accessToken = GenerateAccessToken(userId, userName, additionalClaims);
            var refreshToken = GenerateRefreshToken(userId, userName);
            return (AccessToken: accessToken, RefreshToken: refreshToken);
        }

        /// <summary>
        /// 生成JWT访问令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明（可选）</param>
        /// <returns>生成的JWT访问令牌字符串</returns>
        private string GenerateAccessToken(string userId, string userName, IDictionary<string, object> additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            if (additionalClaims != null)
            {
                foreach (var claim in additionalClaims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value?.ToString() ?? ""));
                }
            }

            var expires = DateTime.Now.AddHours(_options.DefaultExpiryHours);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 生成刷新令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <returns>生成的刷新令牌</returns>
        private string GenerateRefreshToken(string userId, string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("token_type", "refresh")
            };

            var expires = DateTime.Now.AddDays(_options.RefreshTokenExpiryDays);  // ✅ 使用配置

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 验证JWT令牌
        /// </summary>
        /// <param name="token">要验证的令牌</param>
        /// <returns>验证结果</returns>
        public TokenValidationResult ValidateToken(string token)
        {
            var result = new TokenValidationResult();

            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Token不能为空";
                    return result;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_options.SecretKey);
                var securityKey = new SymmetricSecurityKey(key);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = _options.ValidateIssuer,
                    ValidateAudience = _options.ValidateAudience,
                    ValidateLifetime = _options.ValidateLifetime,
                    ClockSkew = TimeSpan.FromSeconds(_options.ClockSkewSeconds),
                    IssuerSigningKey = securityKey,
                    ValidIssuer = _options.Issuer,
                    ValidAudience = _options.Audience
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                
                var jti = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                
                if (!string.IsNullOrEmpty(jti) && IsTokenRevoked(jti))
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Token已被撤销";
                    return result;
                }
                
                result.IsValid = true;
                result.UserId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                result.UserName = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
                
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    result.ExpiryTime = jwtToken.ValidTo;
                }
                
                foreach (var claim in principal.Claims)
                {
                    if (claim.Type != JwtRegisteredClaimNames.Sub && 
                        claim.Type != JwtRegisteredClaimNames.UniqueName &&
                        claim.Type != JwtRegisteredClaimNames.Jti &&
                        claim.Type != JwtRegisteredClaimNames.Iat)
                    {
                        result.Claims[claim.Type] = claim.Value;
                    }
                }
                
                result.Token = new TokenInfo
                {
                    AccessToken = token,
                    ExpiresAt = result.ExpiryTime ?? DateTime.MinValue,
                    TokenType = "Bearer"
                };
                
                var isRefreshToken = principal.FindFirst("token_type")?.Value == "refresh";
                result.IsRefreshToken = isRefreshToken;

                return result;
            }
            catch (SecurityTokenExpiredException)
            {
                result.IsValid = false;
                result.ErrorMessage = "Token已过期";
                return result;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                result.IsValid = false;
                result.ErrorMessage = "Token签名无效";
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Token验证失败: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// 刷新JWT令牌对
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>包含新访问令牌和新刷新令牌的元组</returns>
        /// <exception cref="ArgumentException">当刷新令牌为空时抛出</exception>
        /// <exception cref="SecurityTokenException">当令牌验证失败时抛出</exception>
        public (string AccessToken, string RefreshToken) RefreshTokens(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException("刷新Token不能为空", nameof(refreshToken));

            var refreshValidation = ValidateToken(refreshToken);
            if (!refreshValidation.IsValid)
                throw new SecurityTokenException($"刷新Token无效: {refreshValidation.ErrorMessage}");

            return GenerateTokens(refreshValidation.UserId, refreshValidation.UserName);
        }
        
        /// <summary>
        /// 撤销令牌（加入黑名单）
        /// </summary>
        /// <param name="token">要撤销的令牌</param>
        /// <returns>异步任务</returns>
        public async Task RevokeTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                if (tokenHandler.CanReadToken(token))
                {
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var jti = jwtToken.Id;
                    var expires = jwtToken.ValidTo;

                    if (!string.IsNullOrEmpty(jti))
                    {
                        // 容量检查：如果超过最大容量，清理已过期的条目
                        if (_revokedTokens.Count >= MaxRevokedTokensCapacity)
                        {
                            CleanupExpiredTokens();
                        }
                        
                        var expiryTime = expires > DateTime.Now ? expires : DateTime.Now.AddHours(1);
                        _revokedTokens.TryAdd(jti, expiryTime);
                        _logger($"Token已撤销: Jti={jti}, 过期时间={expiryTime}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger($"Token撤销失败: {ex.Message}");
            }
            
            await Task.CompletedTask;
        }
        
        /// <summary>
        /// 清理已过期的黑名单条目
        /// </summary>
        private void CleanupExpiredTokens()
        {
            var now = DateTime.Now;
            foreach (var kvp in _revokedTokens)
            {
                if (kvp.Value < now)
                {
                    _revokedTokens.TryRemove(kvp.Key, out _);
                }
            }
            _logger($"黑名单清理完成，当前容量: {_revokedTokens.Count}");
        }
        
        /// <summary>
        /// 检查令牌是否已被撤销
        /// </summary>
        /// <param name="jti">JWT ID</param>
        /// <returns>是否已被撤销</returns>
        public bool IsTokenRevoked(string jti)
        {
            if (string.IsNullOrEmpty(jti))
                return false;

            if (_revokedTokens.TryGetValue(jti, out var expiryTime))
            {
                if (expiryTime < DateTime.Now)
                {
                    _revokedTokens.TryRemove(jti, out _);
                    return false;
                }
                return true;
            }

            return false;
        }
    }
}