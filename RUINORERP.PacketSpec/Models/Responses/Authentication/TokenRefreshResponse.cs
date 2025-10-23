using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Responses.Authentication
{
    [Serializable]
    public class TokenRefreshResponse : ResponseBase
    {
        public string NewAccessToken { get; set; }

        public string NewRefreshToken { get; set; }

        public DateTime ExpireTime { get; set; }

        public static TokenRefreshResponse Success(string newAccessToken, string newRefreshToken, DateTime expireTime)
        {
            return new TokenRefreshResponse
            {
                IsSuccess = true,
                Message = "令牌刷新成功",
                NewAccessToken = newAccessToken,
                NewRefreshToken = newRefreshToken,
                ExpireTime = expireTime
            };
        }

        public static TokenRefreshResponse Fail(string errorMessage)
        {
            return new TokenRefreshResponse
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Message = "令牌刷新失败"
            };
        }
    }
}
