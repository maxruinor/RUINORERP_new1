using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.PacketAdapter;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    public sealed class UserLoginService
    {
        private readonly IClientCommunicationService _comm;
        private readonly ILogger<UserLoginService> _log;

        public UserLoginService(IClientCommunicationService comm, ILogger<UserLoginService> log = null)
        {
            _comm = comm ?? throw new ArgumentNullException(nameof(comm));
            _log = log;
        }

        /* -------------------- 唯一对外入口 -------------------- */

        /// <summary>
        /// 登录：成功返回 LoginResponse，失败抛 RpcException
        /// </summary>
        public Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("用户名或密码不能为空");

            _log?.LogInformation("登录开始，用户={}", username);

            return _comm.SendAsync<LoginRequest, LoginResponse>(
                AuthenticationCommands.Login,
                new LoginRequest { Username = username, Password = password },
                new LoginPacketAdapter(),
                ct);
        }

        /// <summary>
        /// 登出：成功返回 true，失败抛 RpcException
        /// </summary>
        public Task<bool> LogoutAsync(string sessionId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("sessionId 不能为空");

            return _comm.SendAsync<object, bool>(
                AuthenticationCommands.Logout,
                new { SessionId = sessionId, Timestamp = DateTime.UtcNow },
                null,   // 用默认 JsonPacketAdapter
                ct);
        }

        /// <summary>
        /// 验证 Token：成功返回 true，失败抛 RpcException
        /// </summary>
        public Task<bool> ValidateTokenAsync(string token, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("token 不能为空");

            return _comm.SendAsync<object, bool>(
                AuthenticationCommands.ValidateToken,
                new { Token = token, Timestamp = DateTime.UtcNow },
                null,
                ct);
        }

        /// <summary>
        /// 刷新 Token：成功返回 TokenInfo，失败抛 RpcException
        /// </summary>
        public Task<TokenInfo> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("refreshToken 不能为空");

            return _comm.SendAsync<object, TokenInfo>(
                AuthenticationCommands.RefreshToken,
                new { RefreshToken = refreshToken, Timestamp = DateTime.UtcNow },
                null,
                ct);
        }
    }
}