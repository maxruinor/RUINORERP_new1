using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 令牌服务接口
    /// 定义令牌的生成、验证、刷新及撤销功能
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 生成令牌对（访问令牌和刷新令牌）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="additionalClaims">附加声明（可选）</param>
        /// <returns>包含访问令牌和刷新令牌的元组</returns>
        (string AccessToken, string RefreshToken) GenerateTokens(string userId, string userName, IDictionary<string, object> additionalClaims = null);
        
        /// <summary>
        /// 验证JWT令牌
        /// </summary>
        /// <param name="token">要验证的令牌</param>
        /// <returns>令牌验证结果</returns>
        TokenValidationResult ValidateToken(string token);
        
        /// <summary>
        /// 刷新JWT令牌对
        /// </summary>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns>包含新访问令牌和新刷新令牌的元组</returns>
        (string AccessToken, string RefreshToken) RefreshTokens(string refreshToken);
        
        /// <summary>
        /// 撤销令牌（加入黑名单）
        /// </summary>
        /// <param name="token">要撤销的令牌</param>
        /// <returns>异步任务</returns>
        Task RevokeTokenAsync(string token);
        
        /// <summary>
        /// 检查令牌是否已被撤销
        /// </summary>
        /// <param name="token">要检查的令牌</param>
        /// <returns>是否已被撤销</returns>
        bool IsTokenRevoked(string token);
    }
}