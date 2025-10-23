using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// JWT令牌服务实现
    /// 负责生成、验证和刷新JWT令牌
    /// </summary>
    public class JwtTokenService : ITokenService
    {
        private readonly TokenServiceOptions _options;
        private readonly SigningCredentials _signingCredentials;

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
        /// 生成JWT令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明（可选）</param>
        /// <returns>生成的JWT令牌字符串</returns>
        public string GenerateToken(string userId, string userName, IDictionary<string, object> additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // 添加附加声明
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
                
                result.IsValid = true;
                result.UserId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                result.UserName = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
                
                // 直接从JwtSecurityToken获取过期时间
                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    result.ExpiryTime = jwtToken.ValidTo;
                }
                
                // 提取附加声明
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
        /// 刷新JWT令牌
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>新生成的访问令牌</returns>
        /// <exception cref="ArgumentException">当刷新令牌为空时抛出</exception>
        /// <exception cref="SecurityTokenException">当令牌验证失败时抛出</exception>
        public string RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException("刷新Token不能为空", nameof(refreshToken));

            // 验证刷新Token的有效性
            var refreshValidation = ValidateToken(refreshToken);
            if (!refreshValidation.IsValid)
                throw new SecurityTokenException($"刷新Token无效: {refreshValidation.ErrorMessage}");

            // 生成新的访问Token，保持用户信息一致
            return GenerateToken(refreshValidation.UserId, refreshValidation.UserName);
        }

        /// <summary>
        /// 撤销令牌
        /// 注意：在生产环境中，应将Token加入黑名单存储
        /// </summary>
        /// <param name="token">要撤销的令牌</param>
        public void RevokeToken(string token)
        {
            // 简化实现，实际项目中应使用Redis等存储被撤销的Token
            // 此处仅作为接口实现占位
        }
    }
}