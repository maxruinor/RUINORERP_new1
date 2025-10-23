using System;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Models.Requests
{
    [Serializable]
    public class TokenRefreshRequest : RequestBase
    {
     
        public string RefreshToken { get; set; }

        public static TokenRefreshRequest Create(string refreshToken)
        {
            return new TokenRefreshRequest
            {
                RefreshToken = refreshToken
            };
        }
    }
}