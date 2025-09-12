using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Enums;
using OriginalData = RUINORERP.PacketSpec.Protocol.OriginalData;

namespace RUINORERP.PacketSpec.Commands.Business
{
    /// <summary>
    /// 登录处理类型
    /// </summary>
    public enum LoginProcessType
    {
        用户登陆 = 0,
        登陆回复 = 1,
        已经在线 = 2,
        超过限制 = 3
    }

    /// <summary>
    /// 用户登录命令
    /// </summary>
    public class LoginCommand : BaseCommand
    {
        /// <summary>
        /// 登录处理类型
        /// </summary>
        public LoginProcessType ProcessType { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 登录成功事件
        /// </summary>
        public event Action<bool, LoginCommand> OnLoginSuccess;

        /// <summary>
        /// 登录失败事件
        /// </summary>
        public event Action<string, LoginCommand> OnLoginFailure;

        /// <summary>
        /// 构造函数
        /// </summary>
        public LoginCommand(CmdOperation operationType = CmdOperation.Receive) : base(operationType)
        {
            ProcessType = LoginProcessType.用户登陆;
            LoginTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 执行登录命令
        /// </summary>
        public override async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            var startTime = DateTime.UtcNow;
            
            try
            {
                LogExecution($"开始执行登录命令: {Username}");

                switch (ProcessType)
                {
                    case LoginProcessType.用户登陆:
                        return await ProcessUserLogin(cancellationToken);
                    
                    case LoginProcessType.登陆回复:
                        return await ProcessLoginReply(cancellationToken);
                    
                    case LoginProcessType.已经在线:
                        return await ProcessUserAlreadyOnline(cancellationToken);
                    
                    case LoginProcessType.超过限制:
                        return await ProcessUserLimitExceeded(cancellationToken);
                    
                    default:
                        return CommandResult.CreateError("未知的登录处理类型: {ProcessType}");
                }
            }
            catch (Exception ex)
            {
                LogExecution($"登录命令执行异常: {ex.Message}", ex);
                OnLoginFailure?.Invoke(ex.Message, this);
                return CommandResult.CreateError($"登录执行异常: {ex.Message}", ex.GetType().Name);
            }
            finally
            {
                var executionTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
                LogExecution($"登录命令执行完成，耗时: {executionTime}ms");
            }
        }

        /// <summary>
        /// 处理用户登录
        /// </summary>
        private async Task<CommandResult> ProcessUserLogin(CancellationToken cancellationToken)
        {
            // 模拟登录验证逻辑
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                var errorMsg = "用户名或密码不能为空";
                OnLoginFailure?.Invoke(errorMsg, this);
                return CommandResult.CreateError(errorMsg);
            }

            // 这里应该调用实际的用户验证服务
            await Task.Delay(100, cancellationToken); // 模拟验证延迟

            // 模拟验证成功
            var success = !string.IsNullOrEmpty(Username) && Password.Length >= 6;
            
            if (success)
            {
                OnLoginSuccess?.Invoke(true, this);
                return CommandResult.CreateSuccess(this, $"用户 {Username} 登录成功");
            }
            else
            {
                var errorMsg = "用户名或密码错误";
                OnLoginFailure?.Invoke(errorMsg, this);
                return CommandResult.CreateError(errorMsg);
            }
        }

        /// <summary>
        /// 处理登录回复
        /// </summary>
        private async Task<CommandResult> ProcessLoginReply(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            BuildDataPacket(this);
            return CommandResult.CreateSuccess(this, "登录回复已发送");
        }

        /// <summary>
        /// 处理用户已在线
        /// </summary>
        private async Task<CommandResult> ProcessUserAlreadyOnline(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            BuildDataPacket(this);
            return CommandResult.CreateSuccess(this, "用户已在线通知已发送");
        }

        /// <summary>
        /// 处理用户数量超限
        /// </summary>
        private async Task<CommandResult> ProcessUserLimitExceeded(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            BuildDataPacket(this);
            return CommandResult.CreateSuccess(this, "用户超限通知已发送");
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        public override bool AnalyzeDataPacket(OriginalData data, SessionInfo sessionInfo)
        {
            try
            {
                if (data.Two == null)
                {
                    return false;
                }

                int index = 0;
                var loginTimeStr = GetStringFromBytes(data.Two, ref index);
                Username = GetStringFromBytes(data.Two, ref index);
                Password = GetStringFromBytes(data.Two, ref index);

                if (DateTime.TryParse(loginTimeStr, out var loginTime))
                {
                    LoginTime = loginTime;
                }

                // 更新会话信息
                if (sessionInfo != null)
                {
                    SessionInfo = sessionInfo;
                    sessionInfo.Username = Username;
                }

                return !string.IsNullOrEmpty(Username);
            }
            catch (Exception ex)
            {
                LogExecution($"解析登录数据包失败: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 构建数据包
        /// </summary>
        public override void BuildDataPacket(object request = null)
        {
            try
            {
                var data = new OriginalData
                {
                    Cmd = (byte)ServerCommand.LoginReply,
                    Two = BuildLoginResponseData()
                };

                DataPacket = data;
            }
            catch (Exception ex)
            {
                LogExecution($"构建登录数据包失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 构建登录响应数据
        /// </summary>
        private byte[] BuildLoginResponseData()
        {
            var response = new
            {
                ProcessType = (int)ProcessType,
                Success = true,
                SessionId = SessionInfo?.SessionId,
                Username = Username,
                LoginTime = LoginTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Message = "登录成功"
            };

            var json = JsonConvert.SerializeObject(response);
            return Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// 从字节数组中获取字符串
        /// </summary>
        private string GetStringFromBytes(byte[] bytes, ref int index)
        {
            if (bytes == null || index >= bytes.Length)
                return string.Empty;

            // 这里应该实现实际的字节解析逻辑
            // 暂时返回空字符串，实际应该根据协议解析
            var length = Math.Min(20, bytes.Length - index);
            var result = Encoding.UTF8.GetString(bytes, index, length);
            index += length;
            return result?.Trim('\0') ?? string.Empty;
        }

        /// <summary>
        /// 验证命令是否可以执行
        /// </summary>
        public override bool CanExecute()
        {
            return base.CanExecute() && 
                   ((OperationType == CmdOperation.Receive && DataPacket.HasValue) ||
                    (OperationType == CmdOperation.Send && !string.IsNullOrEmpty(Username)));
        }
    }

    /// <summary>
    /// 登录命令处理器
    /// </summary>
    [CommandHandler("LoginCommandHandler")]
    public class LoginCommandHandler : BaseCommandHandler
    {
        public override async Task<CommandResult> HandleAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command is LoginCommand loginCommand)
            {
                LogMessage($"开始处理登录命令: {loginCommand.Username}");
                
                var result = await loginCommand.ExecuteAsync(cancellationToken);
                
                LogMessage($"登录命令处理完成: {loginCommand.Username} - {result.Success}");
                
                return result;
            }

            return CommandResult.CreateError("不支持的命令类型");
        }

        public override bool CanHandle(ICommand command, System.Collections.Concurrent.BlockingCollection<ICommand> queue = null)
        {
            return command is LoginCommand;
        }
    }
}