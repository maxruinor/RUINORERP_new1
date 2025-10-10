using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.Server.SuperSocketServices;
using RUINORERP.Business;
using RUINORERP.Global;
using System.Linq;

namespace RUINORERP.Server.SuperSocketServices.Commands.Legacy
{
    /// <summary>
    /// ⚠️ [已过时] 服务器登录命令处理类 - 旧架构实现
    /// 已由新架构 Network/Commands/LoginCommandHandler.cs 替代
    /// 
    /// 迁移说明:
    /// - 登录认证已迁移到: RUINORERP.Server.Network.Commands.LoginCommandHandler
    /// - 会话管理已迁移到: RUINORERP.Server.Network.Services.UnifiedSessionManager
    /// - 认证服务已迁移到: LoginCommandHandler.cs 中的认证逻辑
    /// 
    /// 建议使用新架构进行开发，此命令类将在未来版本中移除
    /// </summary>
    [Obsolete("此命令类已过时，请使用 Network/Commands/LoginCommandHandler.cs 替代", false)]
    public class ServerLoginCommand : IServerCommand
    {
        private readonly ILogger<ServerLoginCommand> _logger;
        private readonly ISessionManagerService _sessionManager;
        private readonly IServerSessionEventHandler _eventHandler;

        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientVersion { get; set; }
        public string ClientInfo { get; set; }

        // ICommand 接口属性
        public string CommandId { get; set; } = Guid.NewGuid().ToString();
        public CmdOperation OperationType { get; set; } = CmdOperation.Receive;
        public OriginalData? DataPacket { get; set; }
        public SessionInfo SessionInfo { get; set; }

        // IServerCommand 接口属性
        public int Priority { get; set; } = 1;
        public string Description => "用户登录认证";
        public bool RequiresAuthentication => false;
        public int TimeoutMs { get; set; } = 30000;

        public ServerLoginCommand(
            ILogger<ServerLoginCommand> logger,
            ISessionManagerService sessionManager,
            IServerSessionEventHandler eventHandler)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _eventHandler = eventHandler;
        }

        public virtual bool CanExecute()
        {
            return !string.IsNullOrEmpty(Username) && 
                   !string.IsNullOrEmpty(Password) &&
                   SessionInfo != null;
        }

        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"开始处理登录请求: Username={Username}, SessionId={SessionInfo?.SessionId}");

                // 验证参数
                var validationResult = ValidateParameters();
                if (!validationResult.IsValid)
                {
                    await _eventHandler.OnAuthenticationFailedAsync(SessionInfo, validationResult.ErrorMessage);
                    return CommandResult.CreateError(validationResult.ErrorMessage);
                }

                // 执行登录验证
                var loginResult = await PerformLoginAsync();
                if (!loginResult.Success)
                {
                    await _eventHandler.OnAuthenticationFailedAsync(SessionInfo, loginResult.Message);
                    return loginResult;
                }

                // 模拟设置用户ID
                SessionInfo.UserId = 1001L; // 这里应该从实际认证服务获取
                SessionInfo.LoginTime = DateTime.Now;

                // 更新会话管理器中的用户映射
                if (_sessionManager is SessionManagerService sessionMgr)
                {
                    sessionMgr.MapUserToSession(SessionInfo.UserId.ToString(), sessionInfo.SessionID);
                }

                // 触发认证成功事件
                await _eventHandler.OnUserAuthenticatedAsync(SessionInfo);

                _logger.LogInformation($"用户登录成功: Username={Username}, SessionId={sessionInfo.SessionID}");

                // 返回登录成功响应
                var responseData = new LoginResponse
                {
                    Success = true,
                    UserId = SessionInfo.UserId ?? 0L, // 使用long类型
                    SessionId = sessionInfo.SessionID,
                    ServerTime = DateTime.Now,
                    Message = "登录成功"
                };

                return CommandResult.CreateSuccess("登录成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"登录处理异常: Username={Username}");
                await _eventHandler.OnSessionErrorAsync(SessionInfo, ex);
                return CommandResult.CreateError("登录处理异常");
            }
        }

        public RUINORERP.PacketSpec.Commands.ValidationResult ValidateParameters()
        {
            if (string.IsNullOrWhiteSpace(Username))
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户名不能为空");

            if (string.IsNullOrWhiteSpace(Password))
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("密码不能为空");

            if (Username.Length > 50)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户名长度不能超过50个字符");

            if (Password.Length > 100)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("密码长度不能超过100个字符");

            return RUINORERP.PacketSpec.Commands.ValidationResult.Success();
        }

        public string GetCommandId()
        {
            return CommandId;
        }
        
        /// <summary>
        /// 解析数据包
        /// </summary>
        public bool AnalyzeDataPacket(OriginalData data, SessionInfo sessionInfo)
        {
            try
            {
                // 这里应该根据实际协议解析登录数据
                // 为了简化，这里使用示例数据
                Username = "demo_user";
                Password = "demo_password";
                SessionInfo = sessionInfo;
                DataPacket = data;
                
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析登录数据包失败");
                return false;
            }
        }
        
        /// <summary>
        /// 构建数据包
        /// </summary>
        public void BuildDataPacket(object request = null)
        {
            try
            {
                // 这里应该根据登录响应构建数据包
                // 为了简化，这里使用示例数据
                if (request is LoginResponse response)
                {
                    var responseBytes = System.Text.Encoding.UTF8.GetBytes(
                        Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    DataPacket = new OriginalData(0xAA, responseBytes, null);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建登录响应数据包失败");
            }
        }

        /// <summary>
        /// 执行登录验证
        /// </summary>
        private async Task<CommandResult> PerformLoginAsync()
        {
            try
            {
                // 这里应该调用实际的用户认证服务
                // 现在先实现一个简单的验证逻辑
                
                // 检查用户是否存在
                var userExists = await CheckUserExistsAsync(Username);
                if (!userExists)
                {
                    return CommandResult.CreateError("用户不存在");
                }

                // 验证密码
                var passwordValid = await ValidatePasswordAsync(Username, Password);
                if (!passwordValid)
                {
                    return CommandResult.CreateError("用户名或密码错误");
                }

                // 检查用户状态
                var userActive = await CheckUserActiveAsync(Username);
                if (!userActive)
                {
                    return CommandResult.CreateError("用户账户已被禁用");
                }

                // 检查是否允许重复登录
                var duplicateLoginAllowed = await CheckDuplicateLoginAsync(Username);
                if (!duplicateLoginAllowed)
                {
                    return CommandResult.CreateError("用户已在其他地方登录");
                }

                return CommandResult.CreateSuccess("认证成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"执行登录验证时发生异常: {Username}");
                return CommandResult.CreateError("认证服务异常");
            }
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        private async Task<bool> CheckUserExistsAsync(string username)
        {
            try
            {
                // 这里应该调用实际的用户服务
                // 现在返回一个简单的检查结果
                return !string.IsNullOrEmpty(username) && username != "invalid";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查用户存在性时发生异常: {username}");
                return false;
            }
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        private async Task<bool> ValidatePasswordAsync(string username, string password)
        {
            try
            {
                // 这里应该调用实际的密码验证服务
                // 现在返回一个简单的验证结果
                return password != "wrong";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"验证密码时发生异常: {username}");
                return false;
            }
        }

        /// <summary>
        /// 检查用户是否激活
        /// </summary>
        private async Task<bool> CheckUserActiveAsync(string username)
        {
            try
            {
                // 这里应该调用实际的用户状态检查服务
                return username != "disabled";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查用户状态时发生异常: {username}");
                return false;
            }
        }

        /// <summary>
        /// 检查是否允许重复登录
        /// </summary>
        private async Task<bool> CheckDuplicateLoginAsync(string username)
        {
            try
            {
                // 检查用户是否已经在其他地方登录
                var userSessions = _sessionManager.GetUserSessions(SessionInfo.UserId?.ToString() ?? "");
                var activeSessionCount = userSessions?.Count() ?? 0;

                // 这里可以根据业务规则决定是否允许重复登录
                // 比如最多允许3个同时登录
                return activeSessionCount < 3;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"检查重复登录时发生异常: {username}");
                return true; // 异常情况下允许登录
            }
        }
    }

    /// <summary>
    /// 登录响应数据
    /// </summary>
    public class LoginResponse
    {
        public bool Success { get; set; }
        public long UserId { get; set; }
        public string SessionId { get; set; }
        public DateTime ServerTime { get; set; }
        public string Message { get; set; }
        public object UserInfo { get; set; }
        public object ServerConfig { get; set; }
    }


}