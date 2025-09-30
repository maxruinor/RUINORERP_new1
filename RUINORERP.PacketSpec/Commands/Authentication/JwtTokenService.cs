using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

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
                result.ExpiryTime = validatedToken.ValidTo;
                
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
        /// 刷新Token
        /// </summary>
        public string RefreshToken(string refreshToken)
        {
            // 简化实现，实际项目中可能需要更复杂的刷新逻辑
            var validation = ValidateToken(refreshToken);
            if (!validation.IsValid)
                throw new SecurityTokenException("刷新Token无效");

            return GenerateToken(validation.UserId, validation.UserName, validation.Claims);
        }

        /// <summary>
        /// 撤销Token
        /// </summary>
        public void RevokeToken(string token)
        {
            // 简化实现，实际项目中可能需要将Token加入黑名单
            // 这里可以考虑使用Redis等存储被撤销的Token
        }
    }
}