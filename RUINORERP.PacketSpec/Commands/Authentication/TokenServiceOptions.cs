using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    public class TokenServiceOptions
    {
        public string SecretKey { get; set; } = "your-default-secret-key";
        public int DefaultExpiryHours { get; set; } = 8;
        public string Issuer { get; set; } = "RUINORERP";
        public string Audience { get; set; } = "RUINORERP-Users";
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
        public int ClockSkewSeconds { get; set; } = 300;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(SecretKey))
                throw new ArgumentException("SecretKey不能为空", nameof(SecretKey));
                
            if (DefaultExpiryHours <= 0)
                throw new ArgumentException("DefaultExpiryHours必须大于0", nameof(DefaultExpiryHours));
                
            if (ClockSkewSeconds < 0)
                throw new ArgumentException("ClockSkewSeconds不能为负数", nameof(ClockSkewSeconds));
        }

        public static TokenServiceOptions CreateDefault()
        {
            return new TokenServiceOptions
            {
                SecretKey = "RUINORERP-Default-Secret-Key",
                DefaultExpiryHours = 8,
                Issuer = "RUINORERP",
                Audience = "RUINORERP-Users",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkewSeconds = 300
            };
        }
    }
}
