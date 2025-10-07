using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    public interface ITokenService
    {
        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明</param>
        /// <returns>生成的Token</returns>
        string GenerateToken(string userId, string userName, IDictionary<string, object> additionalClaims = null);

        /// <summary>
        /// 验证Token
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <returns>Token验证结果</returns>
        TokenValidationResult ValidateToken(string token);

        /// <summary>
        /// 刷新Token - 增强版支持可选的当前Token验证
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <param name="currentToken">当前访问Token（可选，用于验证用户身份）</param>
        /// <returns>新的访问Token</returns>
        string RefreshToken(string refreshToken, string currentToken = null);

        /// <summary>
        /// 撤销Token
        /// </summary>
        /// <param name="token">要撤销的Token</param>
        void RevokeToken(string token);

        /// <summary>
        /// 异步验证Token - 合并自TokenValidationService
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <returns>验证结果</returns>
        Task<TokenValidationResult> ValidateTokenAsync(string token);

        /// <summary>
        /// 检查Token是否即将过期 - 合并自TokenValidationService
        /// </summary>
        /// <param name="token">要检查的Token</param>
        /// <param name="thresholdMinutes">过期阈值（分钟）</param>
        /// <returns>检查结果</returns>
        Task<(bool isExpiringSoon, int expiresInSeconds)> CheckTokenExpiryAsync(string token, int thresholdMinutes = 5);
    }




}
