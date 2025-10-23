using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands.Authentication;

namespace RUINORERP.PacketSpec.Models.Requests
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
