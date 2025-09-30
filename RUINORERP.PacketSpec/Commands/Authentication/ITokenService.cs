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
        /// 刷新Token
        /// </summary>
        /// <param name="refreshToken">刷新Token</param>
        /// <returns>新的访问Token</returns>
        string RefreshToken(string refreshToken);

        /// <summary>
        /// 撤销Token
        /// </summary>
        /// <param name="token">要撤销的Token</param>
        void RevokeToken(string token);
    }



    /// <summary>
    /// Token服务选项
    /// </summary>
    [Serializable()]
    public class TokenServiceOptions
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 默认过期小时数
        /// </summary>
        public int DefaultExpiryHours { get; set; } = 8;

        /// <summary>
        /// 刷新Token过期小时数
        /// </summary>
        public int RefreshTokenExpiryHours { get; set; } = 24;
    }
}
