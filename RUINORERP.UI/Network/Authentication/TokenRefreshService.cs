using NSoup.Helper;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Requests.Authentication;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Responses.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Authentication
{
    /// <summary>
    /// Token刷新服务实现
    /// 负责与服务器通信以刷新和验证Token
    /// </summary>
    public class TokenRefreshService : ITokenRefreshService
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">客户端通信服务</param>
        /// <param name="tokenManager">Token管理器</param>
        public TokenRefreshService(ClientCommunicationService communicationService, TokenManager tokenManager)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
        }

        /// <summary>
        /// 刷新Token - 从TokenManager获取当前Token并刷新
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>新的Token信息对象</returns>
        /// <exception cref="InvalidOperationException">当没有可用Token时抛出</exception>
        /// <exception cref="Exception">当Token刷新失败时抛出</exception>
        public async Task<TokenInfo> RefreshTokenAsync(CancellationToken ct = default)
        {
            // 从TokenManager获取当前Token
            var currentToken = await _tokenManager.TokenStorage.GetTokenAsync();
            if (currentToken == null || string.IsNullOrEmpty(currentToken.AccessToken))
            {
                throw new InvalidOperationException("没有可用的Token进行刷新");
            }

            try
            {
                // 创建刷新Token请求
                SimpleRequest request = SimpleRequest.CreateString(currentToken.AccessToken);

                // 发送请求并获取响应
                var packet = await _communicationService.SendCommandAsync(AuthenticationCommands.RefreshToken, request, ct);

                // 从响应中提取Token信息
                if (packet != null && packet.Response != null)
                {
                    if (packet.Response is TokenRefreshResponse refreshResponse)
                    {
                        // 自动更新存储的Token
                        await _tokenManager.TokenStorage.SetTokenAsync(refreshResponse.RefreshToken);
                        return refreshResponse.RefreshToken;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token刷新服务调用失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 验证Token是否有效
        /// </summary>
        /// <param name="token">要验证的Token</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>Token是否有效</returns>
        /// <exception cref="Exception">当验证过程失败时抛出</exception>
        public async Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            try
            {
                SimpleRequest request = SimpleRequest.CreateString(token);
                var response = await _communicationService.SendCommandAsync(AuthenticationCommands.ValidateToken, request, ct);
                return response?.Response.IsSuccess ?? false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Token验证服务调用失败: {ex.Message}", ex);
            }
        }
    }
}