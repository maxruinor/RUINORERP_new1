using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.UI.Network.PacketAdapter;
using SourceLibrary.Security;
using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 用户登录服务类
    /// 提供用户认证、Token管理等功能
    /// </summary>
    public sealed class UserLoginService : IDisposable
    {
        private readonly IClientCommunicationService _comm;
        private readonly ILogger<UserLoginService> _log;
        private readonly SilentTokenRefresher _silentTokenRefresher;

        public UserLoginService(
            IClientCommunicationService comm,
            ILogger<UserLoginService> log = null)
        {
            _comm = comm ?? throw new ArgumentNullException(nameof(comm));
            _log = log;
            _silentTokenRefresher = new SilentTokenRefresher(this);
            _silentTokenRefresher.RefreshFailed += OnTokenRefreshFailed;
        }

        /* -------------------- 唯一对外入口 -------------------- */

        /// <summary>
        /// 登录：成功返回 LoginResponse，失败抛 RpcException
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>登录响应</returns>
        public async Task<LoginResponse> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("用户名或密码不能为空");

            var response = await _comm.SendCommandAsync<LoginRequest, LoginResponse>(new LoginCommand(username, password), ct);

            // 登录成功后设置token
            if (response != null && !string.IsNullOrEmpty(response.AccessToken))
            {
                ClientTokenStorage.SetTokens(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                // 启动静默刷新服务
                _silentTokenRefresher.Start();
            }

            return response;
        }

        /// <summary>
        /// 登出：成功返回 true，失败抛 RpcException
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>登出是否成功</returns>
        public async Task<bool> LogoutAsync(string sessionId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new ArgumentException("sessionId 不能为空");

            var result = await _comm.SendAsync<object, bool>(
                AuthenticationCommands.Logout,
                new { SessionId = sessionId, TimestampUtc = DateTime.UtcNow },
                null,   // 用默认 JsonPacketAdapter
                ct);

            if (result)
            {
                // 登出成功后清除令牌并停止静默刷新
                ClientTokenStorage.ClearTokens();
                _silentTokenRefresher.Stop();
            }

            return result;
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
                new { Token = token, TimestampUtc = DateTime.UtcNow },
                null,
                ct);
        }

        /// <summary>
        /// 刷新 Token：成功返回 LoginResponse，失败抛 RpcException
        /// </summary>
        public Task<LoginResponse> RefreshTokenAsync(string refreshToken, string currentToken, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("refreshToken 不能为空");

            if (string.IsNullOrWhiteSpace(currentToken))
                throw new ArgumentException("currentToken 不能为空");

            return _comm.SendAsync<LoginRequest, LoginResponse>(
                AuthenticationCommands.RefreshToken,
                new LoginRequest { RefreshToken = refreshToken, Token = currentToken },
                new LoginPacketAdapter(),
                ct);
        }

        /// <summary>
        /// 准备登录：用于初始化登录环境
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>准备是否成功</returns>
        public Task<bool> PrepareLoginAsync(CancellationToken ct = default)
        {
            return _comm.SendAsync<object, bool>(
                AuthenticationCommands.PrepareLogin,
                new { TimestampUtc = DateTime.UtcNow },
                null,
                ct);
        }

        /// <summary>
        /// 手动触发Token刷新（通常由静默刷新器调用）
        /// </summary>
        /// <returns>刷新是否成功</returns>
        public Task<bool> TrySilentRefreshAsync()
        {
            return _silentTokenRefresher.TriggerRefreshAsync();
        }

        /// <summary>
        /// 处理Token刷新失败事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnTokenRefreshFailed(object sender, SilentTokenRefresher.RefreshFailedEventArgs e)
        {
            _log?.LogWarning("静默刷新Token失败: {Message}", e.Exception.Message);
            // 可以在这里添加通知用户重新登录的逻辑
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            _silentTokenRefresher.Stop();
            _silentTokenRefresher.RefreshFailed -= OnTokenRefreshFailed;
            _silentTokenRefresher.Dispose();
        }
    }
}