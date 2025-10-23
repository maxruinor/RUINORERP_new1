using System;
using System.Collections.Generic;

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
        /// <returns>新生成的访问令牌</returns>
        string RefreshToken(string refreshToken);
        
        /// <summary>
        /// 撤销令牌
        /// </summary>
        /// <param name="token">要撤销的令牌</param>
        void RevokeToken(string token);
        
        // 移除异步方法，简化设计
        // 移除CheckTokenExpiryAsync方法，由调用方根据需要实现
    }
}