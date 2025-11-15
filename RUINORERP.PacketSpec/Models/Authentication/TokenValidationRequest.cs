using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Models.Authentication
{
    [Serializable]
    public class TokenValidationRequest : RequestBase
    {
        public TokenInfo Token { get; set; }

        public static TokenValidationRequest Create(TokenInfo token)
        {
            return new TokenValidationRequest
            {
                Token = token
            };
        }

        public bool IsValid()
        {
            return Token != null && !string.IsNullOrEmpty(Token.AccessToken) && Token.ExpiresAt > DateTime.Now;
        }
    }
}
