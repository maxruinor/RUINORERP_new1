using System;
using System.Threading.Tasks;
using SuperSocket.Server.Abstractions.Session;
using RUINORERP.PacketSpec.Models;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;

namespace RUINORERP.Server.SuperSocketServices
{

    public class SuperSocketServerSession
    {
        private readonly ILogger<SuperSocketServerSession> _logger;
        private readonly ISessionService _sessionManager;
        private readonly IServerSessionEventHandler _eventHandler;
        private SessionInfo _sessionInfo;
        private readonly string _sessionId;
        private readonly string _remoteEndPoint;

        public int HeartbeatCounter { get; internal set; }
        public string SessionID { get; internal set; }

        public SuperSocketServerSession(
            ILogger<SuperSocketServerSession> logger,
            ISessionService sessionManager,
            IServerSessionEventHandler eventHandler)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _eventHandler = eventHandler;
            _sessionId = Guid.NewGuid().ToString();
            _remoteEndPoint = "127.0.0.1:0000"; // 示例值
        }

        /// <summary>
        /// 会话连接时触发
        /// </summary>
        public async Task OnSessionConnectedAsync()
        {
            try
            {
                _logger.LogInformation($"客户端连接: {_remoteEndPoint}");

                // 创建会话信息
                _sessionInfo = new SessionInfo
                {
                    ClientIp = _remoteEndPoint,
                    ConnectedTime = DateTime.Now,
                    IsAuthenticated = false,
                    Status = SessionStatus.Connected
                };
                _sessionInfo.UpdateActivity();

                // 注册会话到会话管理器
                await _sessionManager.RemoveSessionAsync(this.SessionID);

                // 触发连接事件
                await _eventHandler.OnSessionConnectedAsync(_sessionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理客户端连接时发生错误: {_remoteEndPoint}");
            }
        }

        /// <summary>
        /// 会话断开时触发
        /// </summary>
        public async Task OnSessionClosedAsync(string reason)
        {
            try
            {
                _logger.LogInformation($"客户端断开连接: {_remoteEndPoint}, 原因: {reason}");

                if (_sessionInfo != null)
                {
                    // 更新会话状态
                    _sessionInfo.Status = SessionStatus.Disconnected;
                    _sessionInfo.DisconnectTime = DateTime.Now;

                    // 从会话管理器注销会话
                    await _sessionManager.RemoveSessionAsync(this.SessionID);

                    // 触发断开事件
                    await _eventHandler.OnSessionDisconnectedAsync(_sessionInfo, reason);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理客户端断开连接时发生错误: {_remoteEndPoint}");
            }
        }

        /// <summary>
        /// 接收到数据包时触发
        /// </summary>
        public async Task OnPackageReceivedAsync(ServerPackageInfo package)
        {
            try
            {
              
                // 更新最后活动时间
                if (_sessionInfo != null)
                {
                    _sessionInfo.UpdateActivity();
                    _sessionInfo.ReceivedPacketsCount++;
                }

                // 处理数据包
                await ProcessPackageAsync(package);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理数据包时发生错误: SessionId={_sessionId}");

                // 发送错误响应
                await SendErrorResponseAsync("处理请求时发生内部错误");
            }
        }

        /// <summary>
        /// 处理数据包
        /// </summary>
        private async Task ProcessPackageAsync(ServerPackageInfo package)
        {
            try
            {
                // 检查会话是否需要认证
                //if (!_sessionInfo.IsAuthenticated && !IsAuthenticationCommand(package.Command))
                //{
                //    await SendErrorResponseAsync("未认证的会话");
                //    return;
                //}

                //// 创建命令执行上下文
                //var command = CreateCommand(package);
                //if (command != null)
                //{
                //    // 设置会话上下文
                //    // command.SessionInfo = _sessionInfo;

                //    // 执行命令
                //    var result = await command.ExecuteAsync();

                //    // 发送响应
                //    if (result.Success)
                //    {
                //        if (result.Data != null)
                //        {
                //            await SendResponseAsync(result.Data);
                //        }
                //    }
                //    else
                //    {
                //        await SendErrorResponseAsync(result.Message);
                //    }
                //}
                //else
                //{
                //    _logger.LogWarning($"未识别的命令: {package.Command}");
                //    await SendErrorResponseAsync("未识别的命令");
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理数据包时发生异常");
                await SendErrorResponseAsync("处理请求时发生错误");
            }
        }

        /// <summary>
        /// 检查是否为认证命令
        /// </summary>
        private bool IsAuthenticationCommand(byte command)
        {
            return command == 0x01 || // Login
                   command == 0x02;   // Register
        }

  

        /// <summary>
        /// 发送响应数据
        /// </summary>
        private async Task SendResponseAsync(object data)
        {
            try
            {
                // 这里需要将响应数据序列化为BizPackageInfo
                // 然后发送给客户端
                if (_sessionInfo != null)
                {
                    _sessionInfo.SentPacketsCount++;
                }

                _logger.LogDebug($"发送响应: SessionId={_sessionId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送响应时发生错误");
            }
        }

        /// <summary>
        /// 发送错误响应
        /// </summary>
        private async Task SendErrorResponseAsync(string errorMessage)
        {
            try
            {
                _logger.LogWarning($"发送错误响应: SessionId={_sessionId}, Error={errorMessage}");

                // 创建错误响应包
                // 这里需要根据协议格式创建错误响应
                if (_sessionInfo != null)
                {
                    _sessionInfo.SentPacketsCount++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送错误响应时发生错误");
            }
        }

        /// <summary>
        /// 获取会话信息
        /// </summary>
        public SessionInfo GetSessionInfo()
        {
            return _sessionInfo;
        }

        /// <summary>
        /// 更新认证状态
        /// </summary>
        public void SetAuthenticated(bool isAuthenticated, long userId)
        {
            if (_sessionInfo != null)
            {
                _sessionInfo.IsAuthenticated = isAuthenticated;
                _sessionInfo.UserId = userId;
                _sessionInfo.Status = isAuthenticated ? SessionStatus.Authenticated : SessionStatus.Connected;
            }
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public async Task DisconnectAsync(string reason)
        {
            await OnSessionClosedAsync(reason);
        }
    }


}