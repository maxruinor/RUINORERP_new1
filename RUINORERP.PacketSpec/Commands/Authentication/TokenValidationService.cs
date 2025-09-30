using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token验证服务
    /// </summary>
    public class TokenValidationService
    {
        private readonly ITokenService _tokenService;

        public TokenValidationService(ITokenService tokenService)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <returns>验证结果</returns>
        public async Task<TokenValidationResult> ValidateTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Token不能为空"
                };
            }
            
            try 
            {
                var result = _tokenService.ValidateToken(token);
                
                // 检查token是否即将过期（在5分钟内过期）
                if (result.IsValid && result.ExpiryTime.HasValue)
                {
                    var timeUntilExpiry = result.ExpiryTime.Value - DateTime.UtcNow;
                    if (timeUntilExpiry.TotalMinutes < 5)
                    {
                        result.Claims["NeedsRefresh"] = true;
                        result.Claims["ExpiresIn"] = (int)timeUntilExpiry.TotalSeconds;
                    }
                }
                
                return result;
            } 
            catch (Exception ex) 
            {
                return new TokenValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"Token验证异常: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <param name="currentToken">当前Token</param>
        /// <returns>刷新结果</returns>
        public async Task<TokenRefreshResult> RefreshTokenAsync(string refreshToken, string currentToken)
        {
            await Task.Delay(1); // 模拟异步操作

            if (string.IsNullOrEmpty(refreshToken))
            {
                return new TokenRefreshResult
                {
                    IsSuccess = false,
                    ErrorMessage = "刷新Token不能为空"
                };
            }

            try
            {
                // 验证当前Token
                var validationResult = _tokenService.ValidateToken(currentToken);
                if (!validationResult.IsValid)
                {
                    return new TokenRefreshResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "当前Token无效"
                    };
                }

                // 刷新Token
                var newToken = _tokenService.RefreshToken(refreshToken);

                return new TokenRefreshResult
                {
                    IsSuccess = true,
                    AccessToken = newToken,
                    RefreshToken = Guid.NewGuid().ToString(), // 生成新的刷新Token
                    ExpiresIn = 3600, // 1小时
                    UserId = validationResult.UserId
                };
            }
            catch (Exception ex)
            {
                return new TokenRefreshResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Token刷新失败: {ex.Message}"
                };
            }
        }
    }

    /// <summary>
    /// Token刷新结果
    /// </summary>
    public class TokenRefreshResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新Token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
