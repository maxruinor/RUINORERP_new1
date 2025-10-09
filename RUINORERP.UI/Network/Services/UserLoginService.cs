using MessagePack;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.Authentication;
using SourceLibrary.Security;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 用户登录服务类
    /// 提供用户认证、Token管理等功能
    /// 使用简化版TokenManager体系，提供核心的Token管理功能
    /// </summary>
    public sealed class UserLoginService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly CommandPacketAdapter commandPacketAdapter;
        private readonly ILogger<UserLoginService> _log;
        private readonly SilentTokenRefresher _silentTokenRefresher;
        private readonly TokenManager _tokenManager;
        /// <summary>
        /// 构造函数 - 使用依赖注入的TokenManager
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="tokenManager">Token管理器（依赖注入）</param>
        /// <param name="logger">日志记录器</param>
        public UserLoginService(
            ClientCommunicationService communicationService,
              CommandPacketAdapter _commandPacketAdapter,
            TokenManager tokenManager,
            ILogger<UserLoginService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            commandPacketAdapter = _commandPacketAdapter;
            _tokenManager = tokenManager ?? throw new ArgumentNullException(nameof(tokenManager));
            _log = logger;

            _silentTokenRefresher = new SilentTokenRefresher(new TokenRefreshService(communicationService, _tokenManager));

            // 订阅静默刷新事件
            _silentTokenRefresher.RefreshSucceeded += OnRefreshSucceeded;
            _silentTokenRefresher.RefreshFailed += OnTokenRefreshFailed;
        }

        /* -------------------- 唯一对外入口 -------------------- */

        /// <summary>
        /// 用户登录 - 使用简化版TokenManager
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>登录响应</returns>
        public async Task<ResponseBase<LoginResponse>> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            try
            {
                var loginRequest = LoginRequest.Create(username, password);

                LoginCommand loginCommand = new LoginCommand(username, password);
                loginCommand.Request = loginRequest;
                loginCommand.Request.RequestId = IdGenerator.GenerateRequestId(loginCommand.CommandIdentifier);


                var packet = await _communicationService.SendCommandAsync<LoginRequest, ResponseBase<LoginResponse>>(loginCommand, ct);

                LoginResponse loginResponse = null;

                // 登录成功后设置token - 使用简化版TokenManager
                if (packet != null)
                {
#warning 登陆成功后 得到token 返回
                    //_tokenManager.TokenStorage.SetTokenAsync(response.AccessToken, response.RefreshToken, response.ExpiresIn);
                    ICommand baseCommand = commandPacketAdapter.CreateCommandFromBytes(packet.CommandData, packet.ExecutionContext.CommandType.Name);
                    if (baseCommand is LoginCommand responseCommand)
                    {
                        if (responseCommand.Response == null)
                        {
                            var lastresponse = MessagePackSerializer.Deserialize(packet.ExecutionContext.ResponseType, responseCommand.JsonResponseData);
                            loginResponse = lastresponse as LoginResponse;
                        }
                    }

                    if (packet.ExecutionContext.Token != null && !string.IsNullOrEmpty(packet.ExecutionContext.Token.AccessToken))
                    {

                    }
                }

                return loginResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"登录失败: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// 用户登出 - 使用简化版TokenManager
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>登出结果</returns>
        public async Task<bool> LogoutAsync(CancellationToken ct = default)
        {
            try
            {
                // 获取当前令牌信息
                var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
                if (tokenInfo == null)
                {
                    _log?.LogWarning("登出失败：未找到有效的令牌信息");
                    return false;
                }

                // 创建登出请求
                var request = SimpleRequest.CreateBool(true);

                var baseCommand = CommandDataBuilder.BuildCommand<SimpleRequest, SimpleResponse>(AuthenticationCommands.Logout, request);
                baseCommand.Request = request;
                var response = await _communicationService.SendCommandAsync<SimpleRequest, SimpleResponse>(
                    baseCommand, ct);

                // 检查响应是否成功
                bool isSuccess = response != null;// && response.IsSuccess;

                if (isSuccess)
                {
                    // 登出成功后清除令牌并停止静默刷新 - 使用简化版TokenManager
                    await _tokenManager.TokenStorage.ClearTokenAsync();
                    _log?.LogInformation("用户登出成功");
                }

                return isSuccess;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "登出过程中发生异常");
                throw new Exception($"登出失败: {ex.Message}", ex);
            }
        }





        /// <summary>
        /// 获取当前访问令牌 - 使用简化版TokenManager
        /// </summary>
        /// <returns>访问令牌</returns>
        public async Task<string> GetCurrentAccessToken()
        {
            var tokenInfo = await _tokenManager.TokenStorage.GetTokenAsync();
            return tokenInfo?.AccessToken;
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
        /// 处理Token刷新成功事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void OnRefreshSucceeded(object sender, SilentTokenRefresher.RefreshSucceededEventArgs e)
        {
            _log?.LogInformation("静默刷新Token成功");
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
            _silentTokenRefresher.RefreshFailed -= OnTokenRefreshFailed;
            _silentTokenRefresher.RefreshSucceeded -= OnRefreshSucceeded;
            _silentTokenRefresher.Dispose();
        }
    }
}