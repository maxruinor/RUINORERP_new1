using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// Token服务配置选项
    /// 用于配置JWT Token服务的各项参数
    /// </summary>
    public class TokenServiceOptions
    {
        /// <summary>
        /// JWT签名密钥
        /// </summary>
        public string SecretKey { get; set; } = "you0-default-secret-key";

        /// <summary>
        /// 默认Token过期时间（小时）
        /// </summary>
        public int DefaultExpiryHours { get; set; } = 8;

        /// <summary>
        /// 刷新Token过期时间（小时）
        /// </summary>
        public int RefreshTokenExpiryHours { get; set; } = 24;

        /// <summary>
        /// Token发行者
        /// </summary>
        public string Issuer { get; set; } = "RUINORERP";

        /// <summary>
        /// Token受众
        /// </summary>
        public string Audience { get; set; } = "RUINORERP-Users";

        /// <summary>
        /// 是否验证发行者
        /// </summary>
        public bool ValidateIssuer { get; set; } = true;

        /// <summary>
        /// 是否验证受众
        /// </summary>
        public bool ValidateAudience { get; set; } = true;

        /// <summary>
        /// 是否验证过期时间
        /// </summary>
        public bool ValidateLifetime { get; set; } = true;

        /// <summary>
        /// 时钟偏移量（秒）
        /// 用于处理服务器时间差异
        /// </summary>
        public int ClockSkewSeconds { get; set; } = 300; // 5分钟

        /// <summary>
        /// Token即将过期阈值（分钟）
        /// 用于判断Token是否需要刷新
        /// </summary>
        public int ExpiryThresholdMinutes { get; set; } = 5;

        /// <summary>
        /// 验证配置有效性
        /// </summary>
        /// <exception cref="ArgumentException">配置无效时抛出异常</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SecretKey))
            {
                throw new ArgumentException("SecretKey不能为空", nameof(SecretKey));
            }

            if (SecretKey.Length < 16)
            {
                throw new ArgumentException("SecretKey长度至少为16个字符", nameof(SecretKey));
            }

            if (DefaultExpiryHours <= 0)
            {
                throw new ArgumentException("DefaultExpiryHours必须大于0", nameof(DefaultExpiryHours));
            }

            if (RefreshTokenExpiryHours <= 0)
            {
                throw new ArgumentException("RefreshTokenExpiryHours必须大于0", nameof(RefreshTokenExpiryHours));
            }

            if (RefreshTokenExpiryHours <= DefaultExpiryHours)
            {
                throw new ArgumentException("RefreshTokenExpiryHours必须大于DefaultExpiryHours", nameof(RefreshTokenExpiryHours));
            }

            if (ClockSkewSeconds < 0)
            {
                throw new ArgumentException("ClockSkewSeconds不能为负数", nameof(ClockSkewSeconds));
            }

            if (ExpiryThresholdMinutes <= 0)
            {
                throw new ArgumentException("ExpiryThresholdMinutes必须大于0", nameof(ExpiryThresholdMinutes));
            }

            if (string.IsNullOrWhiteSpace(Issuer))
            {
                throw new ArgumentException("Issuer不能为空", nameof(Issuer));
            }

            if (string.IsNullOrWhiteSpace(Audience))
            {
                throw new ArgumentException("Audience不能为空", nameof(Audience));
            }
        }

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <returns>默认的Token服务配置</returns>
        public static TokenServiceOptions CreateDefault()
        {
            return new TokenServiceOptions
            {
                SecretKey = "RUINORERP-Default-Secret-Key-2024",
                DefaultExpiryHours = 8,
                RefreshTokenExpiryHours = 24,
                Issuer = "RUINORERP",
                Audience = "RUINORERP-Users",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkewSeconds = 300,
                ExpiryThresholdMinutes = 5
            };
        }

        /// <summary>
        /// 从配置节创建选项
        /// </summary>
        /// <param name="configuration">配置字典</param>
        /// <returns>Token服务配置</returns>
        public static TokenServiceOptions FromConfiguration(IConfiguration configuration)
        {
            if (configuration == null)
            {
                return CreateDefault();
            }

            var options = new TokenServiceOptions
            {
                SecretKey = configuration["Jwt:SecretKey"] ?? "RUINORERP-Default-Secret-Key-2024",
                DefaultExpiryHours = int.TryParse(configuration["Jwt:DefaultExpiryHours"], out var expiry) ? expiry : 8,
                RefreshTokenExpiryHours = int.TryParse(configuration["Jwt:RefreshTokenExpiryHours"], out var refreshExpiry) ? refreshExpiry : 24,
                Issuer = configuration["Jwt:Issuer"] ?? "RUINORERP",
                Audience = configuration["Jwt:Audience"] ?? "RUINORERP-Users",
                ValidateIssuer = bool.TryParse(configuration["Jwt:ValidateIssuer"], out var validateIssuer) ? validateIssuer : true,
                ValidateAudience = bool.TryParse(configuration["Jwt:ValidateAudience"], out var validateAudience) ? validateAudience : true,
                ValidateLifetime = bool.TryParse(configuration["Jwt:ValidateLifetime"], out var validateLifetime) ? validateLifetime : true,
                ClockSkewSeconds = int.TryParse(configuration["Jwt:ClockSkewSeconds"], out var clockSkew) ? clockSkew : 300,
                ExpiryThresholdMinutes = int.TryParse(configuration["Jwt:ExpiryThresholdMinutes"], out var threshold) ? threshold : 5
            };

            return options;
        }
    }
}
