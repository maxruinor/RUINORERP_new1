using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token服务配置选项
    /// </summary>
    public class TokenServiceOptions
    {
        /// <summary>
        /// JWT签名密钥（必须配置）
        /// 建议：至少32位字符，生产环境使用强随机密钥
        /// </summary>
        public string SecretKey { get; set; }
        
        /// <summary>
        /// AccessToken默认过期时间（小时）
        /// </summary>
        public int DefaultExpiryHours { get; set; } = 12;
        
        /// <summary>
        /// JWT签发者
        /// </summary>
        public string Issuer { get; set; } = "RUINORERP";
        
        /// <summary>
        /// JWT接收者
        /// </summary>
        public string Audience { get; set; } = "RUINORERP-Users";
        
        /// <summary>
        /// 是否验证签发者
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;
        
        /// <summary>
        /// 是否验证接收者
        /// </summary>
        public bool ValidateAudience { get; set; } = true;
        
        /// <summary>
        /// 是否验证令牌有效期
        /// </summary>
        public bool ValidateLifetime { get; set; } = true;
        
        /// <summary>
        /// 时钟偏移容忍秒数
        /// </summary>
        public int ClockSkewSeconds { get; set; } = 300;

        /// <summary>
        /// 验证配置有效性
        /// </summary>
        /// <exception cref="ArgumentException">当配置无效时抛出</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SecretKey))
                throw new ArgumentException("Token服务配置错误：必须配置 SecretKey，建议使用至少32位的随机字符串", nameof(SecretKey));
                
            if (SecretKey.Length < 32)
                throw new ArgumentException("Token服务配置错误：SecretKey 长度至少需要32位字符以确保安全性", nameof(SecretKey));
                
            if (DefaultExpiryHours <= 0)
                throw new ArgumentException("Token服务配置错误：DefaultExpiryHours必须大于0", nameof(DefaultExpiryHours));
                
            if (ClockSkewSeconds < 0)
                throw new ArgumentException("Token服务配置错误：ClockSkewSeconds不能为负数", nameof(ClockSkewSeconds));
        }
    }
}
