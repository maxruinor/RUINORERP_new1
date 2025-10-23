using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 令牌服务接口
    /// 定义令牌的生成、验证、刷新和撤销功能
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 生成JWT令牌
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明（可选）</param>
        /// <returns>生成的JWT令牌字符串</returns>
        string GenerateToken(string userId, string userName, IDictionary<string, object> additionalClaims = null);

        /// <summary>
        /// 验证JWT令牌
        /// </summary>
        /// <param name="token">要验证的令牌</param>
        /// <returns>令牌验证结果</returns>
        TokenValidationResult ValidateToken(string token);

        /// <summary>
        /// 刷新JWT令牌
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <param name="currentToken">当前访问令牌（可选，用于额外验证）</param>
        /// <returns>新生成的访问令牌</returns>
        string RefreshToken(string refreshToken, string currentToken = null);

        /// <summary>
        /// 撤销令牌
        /// </summary>
        /// <param name="token">要撤销的令牌</param>
        void RevokeToken(string token);

        /// <summary>
        /// 异步验证JWT令牌
        /// </summary>
        /// <param name="token">要验证的令牌</param>
        /// <returns>令牌验证结果</returns>
        Task<TokenValidationResult> ValidateTokenAsync(string token);

        /// <summary>
        /// 检查令牌是否即将过期
        /// </summary>
        /// <param name="token">要检查的令牌</param>
        /// <param name="thresholdMinutes">过期阈值（分钟），默认5分钟</param>
        /// <returns>包含是否即将过期和剩余有效秒数的元组</returns>
        Task<(bool isExpiringSoon, int expiresInSeconds)> CheckTokenExpiryAsync(string token, int thresholdMinutes = 5);
    }
}
