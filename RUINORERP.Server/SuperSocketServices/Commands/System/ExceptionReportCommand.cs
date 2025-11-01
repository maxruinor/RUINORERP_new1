using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.Network.Interfaces.Services;
 
using System.Linq;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Commands.Business;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests.Message;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Responses.Message;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 异常报告命令 - 处理实时异常上报和转发
    /// 替代BizCommand中的ClientCommand.实时汇报异常处理逻辑
    /// </summary>
    public class ExceptionReportCommand : BaseServerCommand
    {
        private readonly ISessionService _sessionService;
        
        public ExceptionLevel Level { get; set; } = ExceptionLevel.Info;
        public string ExceptionType { get; set; } = string.Empty;
        public string ExceptionMessage { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public string SourceModule { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 来源会话
        /// </summary>
        public SessionforBiz FromSession { get; set; }
        
        /// <summary>
        /// 命令优先级
        /// </summary>
        public int Priority { get; set; } = 5;
        
        // IServerCommand 接口实现
       // public override string Description => $"异常报告命令: {ExceptionType}";
        public bool RequiresAuthentication => true;
        public int TimeoutMs { get; set; } = 30000;
        public string CommandId { get; set; } = Guid.NewGuid().ToString();

        public ExceptionReportCommand() : base(CmdOperation.Receive)
        {
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
        }

        public ExceptionReportCommand(OriginalData data, SessionforBiz fromSession) : base(CmdOperation.Receive)
        {
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            DataPacket = data;
            FromSession = fromSession;
            if (fromSession?.User != null)
            {
                SessionInfo.Username = fromSession.User.用户名;
                sessionInfo.SessionID = fromSession.SessionID;
            }
        }

        public override async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default) // 修复返回类型
        {
            try
            {
                // 解析数据包
                if (DataPacket.HasValue && FromSession != null) // 修复null比较
                {
                    AnalyzeDataPacket(DataPacket.Value, SessionInfo); // 修复参数传递
                }

                // 智能异常分类
                ClassifyException();

                // 记录异常信息到日志
                await LogExceptionAsync();

                // 转发给超级管理员
                await ForwardToAdministratorsAsync();

                // 根据异常级别执行不同的处理策略
                await HandleByExceptionLevel();
                
                return CommandResult.CreateSuccess(); // 添加返回值
            }
            catch (Exception ex)
            {
                // 避免在异常处理中再次抛出异常
                Console.WriteLine($"ExceptionReportCommand执行时出错: {ex.Message}");
                return CommandResult.CreateError(ex.Message); // 添加返回值
            }
        }
        
        public override bool CanExecute()
        {
            return SessionInfo?.IsAuthenticated == true;
        }
    

        /// <summary>
        /// 解析数据包
        /// </summary>
        public override bool AnalyzeDataPacket(OriginalData gd, SessionInfo sessionInfo)
        {
            try
            {
                SessionInfo = sessionInfo;
                int index = 0;
                Timestamp = DateTime.TryParse(ByteDataAnalysis.GetString(gd.Two, ref index), out var time) ? time : DateTime.Now;
                ExceptionType = ByteDataAnalysis.GetString(gd.Two, ref index);
                ExceptionMessage = ByteDataAnalysis.GetString(gd.Two, ref index);
                StackTrace = ByteDataAnalysis.GetString(gd.Two, ref index);
                SourceModule = ByteDataAnalysis.GetString(gd.Two, ref index);
                
                // 尝试解析异常级别
                if (gd.Two.Length > index + 4)
                {
                    int levelValue = ByteDataAnalysis.GetInt(gd.Two, ref index);
                    Level = (ExceptionLevel)levelValue;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析异常报告数据包时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 智能异常分类
        /// </summary>
        private void ClassifyException()
        {
            if (string.IsNullOrEmpty(ExceptionType))
            {
                // 根据异常消息内容智能分类
                var message = ExceptionMessage?.ToLower() ?? "";
                var stack = StackTrace?.ToLower() ?? "";

                if (message.Contains("database") || message.Contains("sql") || message.Contains("数据库"))
                {
                    ExceptionType = "DatabaseException";
                }
                else if (message.Contains("network") || message.Contains("timeout") || message.Contains("网络"))
                {
                    ExceptionType = "NetworkException";
                }
                else if (message.Contains("authorization") || message.Contains("permission") || message.Contains("权限"))
                {
                    ExceptionType = "SecurityException";
                }
                else if (message.Contains("file") || message.Contains("path") || message.Contains("文件"))
                {
                    ExceptionType = "FileException";
                }
                else if (message.Contains("memory") || message.Contains("overflow") || message.Contains("内存"))
                {
                    ExceptionType = "MemoryException";
                }
                else
                {
                    ExceptionType = "GeneralException";
                }
            }

            // 根据异常类型和消息严重程度调整级别
            if (Level == ExceptionLevel.Info)
            {
                var message = ExceptionMessage?.ToLower() ?? "";
                if (message.Contains("critical") || message.Contains("fatal") || message.Contains("严重"))
                {
                    Level = ExceptionLevel.Critical;
                }
                else if (message.Contains("error") || message.Contains("exception") || message.Contains("错误"))
                {
                    Level = ExceptionLevel.Error;
                }
                else if (message.Contains("warning") || message.Contains("warn") || message.Contains("警告"))
                {
                    Level = ExceptionLevel.Warning;
                }
            }
        }

        /// <summary>
        /// 记录异常信息到日志
        /// </summary>
        private async Task LogExceptionAsync()
        {
            try
            {
                var logMessage = $"[{Level}] {ExceptionType}: {ExceptionMessage}";
                if (!string.IsNullOrEmpty(SourceModule))
                {
                    logMessage += $" (模块: {SourceModule})";
                }
                if (!string.IsNullOrEmpty(SessionInfo.Username)) // 修复属性名
                {
                    logMessage += $" (用户: {SessionInfo.Username})";
                }

                // 这里可以集成具体的日志框架
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {logMessage}");
                
                if (Level >= ExceptionLevel.Error && !string.IsNullOrEmpty(StackTrace))
                {
                    Console.WriteLine($"堆栈跟踪: {StackTrace}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"记录异常日志时出错: {ex.Message}");
            }
            
            await Task.CompletedTask;
        }

        /// <summary>
        /// 转发给超级管理员
        /// </summary>
        private async Task ForwardToAdministratorsAsync()
        {
            try
            {
                var adminSessions = _sessionService.GetAllUserSessions()
                    .Where(s => s.IsSuperUser) // 假设SessionInfo有IsSuperUser属性
                    .ToList();

                // 构建转发消息
                var forwardMessage = BuildForwardMessage();

                foreach (var adminSession in adminSessions)
                {
                    try
                    {
                        // 发送异常报告给管理员 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "EXCEPTION_REPORT",
                            Data = Convert.ToBase64String(forwardMessage)
                        };

                        var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                        // 使用异步调用避免阻塞
                        var success = await _sessionService.SendCommandAsync(
                            adminSession.SessionID, 
                            MessageCommands.SendMessageToUser, 
                            request); // 注意：这里使用.Result是为了保持原有的同步行为
                        
                        if (!success)
                        {
                            Console.WriteLine($"转发异常报告给管理员 {adminSession.UserName} 失败");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"转发异常报告给管理员 {adminSession.UserName} 时出错: {ex.Message}");
                    }
                }

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"转发异常报告时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 构建转发消息
        /// </summary>
        private byte[] BuildForwardMessage()
        {
            try
            {
                var tx = new ByteBuff(1000);
                
                tx.PushString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                tx.PushString(SessionInfo.Username ?? "未知用户"); // 修复属性名
                tx.PushString($"[{Level}] {ExceptionType}");
                tx.PushString(ExceptionMessage ?? "");
                tx.PushString(SourceModule ?? "");
                tx.PushString(StackTrace ?? "");
                tx.PushInt((int)Level);

                return tx.toByte();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"构建转发消息时出错: {ex.Message}");
                return new byte[0];
            }
        }

        /// <summary>
        /// 根据异常级别执行不同的处理策略
        /// </summary>
        private async Task HandleByExceptionLevel()
        {
            try
            {
                switch (Level)
                {
                    case ExceptionLevel.Critical:
                        // 严重异常：立即通知所有管理员
                        await NotifyAllAdminsImmediately();
                        break;
                        
                    case ExceptionLevel.Error:
                        // 错误：记录并通知管理员
                        await NotifyAdminsWithDelay(TimeSpan.FromMinutes(1));
                        break;
                        
                    case ExceptionLevel.Warning:
                        // 警告：延迟通知，避免频繁打扰
                        await NotifyAdminsWithDelay(TimeSpan.FromMinutes(5));
                        break;
                        
                    case ExceptionLevel.Info:
                        // 信息：仅记录，不通知
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"根据异常级别处理时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 立即通知所有管理员
        /// </summary>
        private async Task NotifyAllAdminsImmediately()
        {
            var adminSessions = _sessionService.GetAllUserSessions()
                .Where(s => s.IsSuperUser) // 假设SessionInfo有IsSuperUser属性
                .ToList();

            foreach (var session in adminSessions)
            {
                try
                {
                    // 发送紧急通知 - 使用新的发送方法
                    var messageJson = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        MessageType = "SystemMessage",
                        MessageContent = $"严重异常警报：{ExceptionMessage}",
                        PromptType = "确认窗口",
                        Timestamp = DateTime.Now
                    });
                        
                    var messageData = new
                    {
                        Command = "URGENT_NOTIFICATION",
                        Data = messageJson
                    };

                    var request = new MessageRequest(MessageCmdType.Unknown, messageData);
                    var success = await _sessionService.SendCommandAsync(
                        session.SessionID, 
                        MessageCommands.SendMessageToUser, 
                        request); // 使用异步调用避免阻塞
                        
                    if (!success)
                    {
                        Console.WriteLine($"发送紧急通知给管理员 {session.UserName} 失败");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"通知管理员时出错: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 延迟通知管理员
        /// </summary>
        private async Task NotifyAdminsWithDelay(TimeSpan delay)
        {
            await Task.Delay(delay);
            // 这里可以实现批量通知逻辑，避免频繁打扰
        }
        
        // 实现IServerCommand接口的其他方法
        
        public override void BuildDataPacket(object request = null)
        {
            // 简单实现
        }
        
        public override RUINORERP.PacketSpec.Commands.ValidationResult ValidateParameters()
        {
            if (SessionInfo?.IsAuthenticated != true)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户未认证");
            return RUINORERP.PacketSpec.Commands.ValidationResult.Success();
        }
        
        public override string GetCommandId()
        {
            return CommandId;
        }
    }

    /// <summary>
    /// 异常级别枚举
    /// </summary>
    public enum ExceptionLevel
    {
        Info = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }
}