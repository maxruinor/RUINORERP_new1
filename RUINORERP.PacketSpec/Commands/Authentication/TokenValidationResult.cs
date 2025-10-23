using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    [Serializable]
    public class TokenValidationResult
    {
        public bool IsValid { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime? ExpiryTime { get; set; }

        public string ErrorMessage { get; set; }

        [JsonIgnore]
        public IDictionary<string, object> Claims { get; set; } = new Dictionary<string, object>();

        public TokenInfo Token { get; set; }

        [JsonIgnore]
        public string RefreshToken => Token?.RefreshToken;
    }
}