using System;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Core;
namespace RUINORERP.PacketSpec.Models.Authentication
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
