using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    public class JwtTokenService : ITokenService
    {
        private readonly TokenServiceOptions _options;
        private readonly SigningCredentials _signingCredentials;

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
        /// 生成Token
        /// </summary>
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

            var expires = DateTime.UtcNow.AddHours(_options.DefaultExpiryHours);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expires,
                signingCredentials: _signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        public TokenValidationResult ValidateToken(string token)
        {
            var result = new TokenValidationResult();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_options.SecretKey);
                var securityKey = new SymmetricSecurityKey(key);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = securityKey
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                
                result.IsValid = true;
                result.UserId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                result.UserName = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
                
                // 直接从JwtSecurityToken获取过期时间，而不是从validatedToken.ValidTo
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
            }
            catch (SecurityTokenExpiredException)
            {
                result.IsValid = false;
                result.ErrorMessage = "Token已过期";
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                result.IsValid = false;
                result.ErrorMessage = "Token签名无效";
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorMessage = $"Token验证失败: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 刷新Token - 增强版刷新逻辑
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <param name="currentToken">当前访问Token（可选，用于验证用户身份）</param>
        /// <returns>新的访问Token</returns>
        public string RefreshToken(string refreshToken, string currentToken = null)
        {
            if (string.IsNullOrEmpty(refreshToken))
                throw new ArgumentException("刷新Token不能为空", nameof(refreshToken));

            // 验证刷新Token的有效性
            var refreshValidation = ValidateToken(refreshToken);
            if (!refreshValidation.IsValid)
                throw new SecurityTokenException($"刷新Token无效: {refreshValidation.ErrorMessage}");

            // 如果提供了当前Token，验证其一致性（可选的安全检查）
            if (!string.IsNullOrEmpty(currentToken))
            {
                var currentValidation = ValidateToken(currentToken);
                if (!currentValidation.IsValid)
                    throw new SecurityTokenException("当前Token无效，无法执行刷新操作");
                
                // 确保两个Token属于同一用户
                if (refreshValidation.UserId != currentValidation.UserId)
                    throw new SecurityTokenException("Token用户不一致，刷新操作被拒绝");
            }

            // 生成新的访问Token，保持用户信息一致
            return GenerateToken(refreshValidation.UserId, refreshValidation.UserName, refreshValidation.Claims);
        }

        /// <summary>
    /// 撤销Token
    /// </summary>
        public void RevokeToken(string token)
        {
            // 简化实现，实际项目中可能需要将Token加入黑名单
            // 这里可以考虑使用Redis等存储被撤销的Token
        }

        /// <summary>
        /// 异步验证Token - 合并自TokenValidationService
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <returns>验证结果</returns>
        public Task<TokenValidationResult> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Token不能为空"
                });
            }
            
            try 
            {
                // 直接调用底层验证逻辑
                var result = ValidateToken(token);
                return Task.FromResult(result);
            } 
            catch (Exception ex) 
            {
                return Task.FromResult(new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Token验证异常: {ex.Message}"
                });
            }
        }

        /// <summary>
        /// 检查Token是否即将过期 - 合并自TokenValidationService
        /// </summary>
        /// <param name="token">要检查的Token</param>
        /// <param name="thresholdMinutes">过期阈值（分钟）</param>
        /// <returns>检查结果</returns>
        public Task<(bool isExpiringSoon, int expiresInSeconds)> CheckTokenExpiryAsync(string token, int thresholdMinutes = 5)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult((false, 0));
            }

            try
            {
                var validationResult = ValidateToken(token);
                if (validationResult.IsValid && validationResult.ExpiryTime.HasValue)
                {
                    var timeUntilExpiry = validationResult.ExpiryTime.Value - DateTime.UtcNow;
                    var isExpiringSoon = timeUntilExpiry.TotalMinutes < thresholdMinutes;
                    var expiresInSeconds = Math.Max(0, (int)timeUntilExpiry.TotalSeconds);
                    
                    return Task.FromResult((isExpiringSoon, expiresInSeconds));
                }
            }
            catch
            {
                // 验证失败时返回安全默认值
            }

            return Task.FromResult((false, 0));
        }
    }
}
