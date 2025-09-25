using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少IExceptionReportService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 异常报告命令处理器 - 处理客户端上报的异常信息
    /// </summary>
    [CommandHandler("ExceptionReportCommandHandler", priority: 50)]
    public class ExceptionReportCommandHandler : CommandHandlerBase
    {
        // private readonly IExceptionReportService _exceptionReportService; // 暂时注释，缺少IExceptionReportService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public ExceptionReportCommandHandler() : base()
        {
            // _exceptionReportService = Program.ServiceProvider.GetRequiredService<IExceptionReportService>(); // 暂时注释，缺少IExceptionReportService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExceptionReportCommandHandler(
            // IExceptionReportService exceptionReportService, // 暂时注释，缺少IExceptionReportService接口定义
            ILogger<ExceptionReportCommandHandler> logger = null) : base(logger)
        {
            // _exceptionReportService = exceptionReportService; // 暂时注释，缺少IExceptionReportService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)SystemCommands.ExceptionReport
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 50;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == SystemCommands.ExceptionReport)
                {
                    return await HandleExceptionReportAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理异常报告命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理异常报告命令
        /// </summary>
        private async Task<CommandResult> HandleExceptionReportAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理异常报告命令 [会话: {command.SessionID}]");

                // 解析异常报告数据
                var exceptionData = command.Packet.GetJsonData<ExceptionReportData>();
                if (exceptionData == null)
                {
                    return CommandResult.Failure("异常报告数据格式错误", "INVALID_EXCEPTION_DATA");
                }

                // 暂时返回模拟结果，因为缺少IExceptionReportService接口定义
                // 创建响应数据
                var responseData = CreateExceptionReportResponse(exceptionData);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        ExceptionType = exceptionData.ExceptionType,
                        Level = exceptionData.Level,
                        Timestamp = exceptionData.Timestamp,
                        SessionId = command.SessionID
                    },
                    message: "异常报告处理成功（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理异常报告命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"异常报告处理异常: {ex.Message}", "EXCEPTION_REPORT_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析异常报告数据
        /// </summary>
        private ExceptionReportData ParseExceptionReportData(OriginalData originalData)
        {
            try
            {
                if (originalData.Two == null || originalData.Two.Length == 0)
                    return null;

                var data = new ExceptionReportData();
                int index = 0;

                // 解析时间戳
                var timeStr = GetStringFromBytes(originalData.Two, ref index);
                if (DateTime.TryParse(timeStr, out var timestamp))
                {
                    data.Timestamp = timestamp;
                }

                // 解析异常类型
                data.ExceptionType = GetStringFromBytes(originalData.Two, ref index);

                // 解析异常消息
                data.ExceptionMessage = GetStringFromBytes(originalData.Two, ref index);

                // 解析堆栈跟踪
                data.StackTrace = GetStringFromBytes(originalData.Two, ref index);

                // 解析源模块
                data.SourceModule = GetStringFromBytes(originalData.Two, ref index);

                // 解析异常级别
                if (originalData.Two.Length > index + 4)
                {
                    int levelValue = GetIntFromBytes(originalData.Two, ref index);
                    data.Level = (ExceptionLevel)levelValue;
                }

                return data;
            }
            catch (Exception ex)
            {
                LogError($"解析异常报告数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建异常报告响应
        /// </summary>
        private OriginalData CreateExceptionReportResponse(ExceptionReportData exceptionData)
        {
            var responseData = $"EXCEPTION_REPORTED|{exceptionData.ExceptionType}|{exceptionData.Level}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)SystemCommands.ExceptionReport;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }

        /// <summary>
        /// 从字节数组中获取字符串
        /// </summary>
        private string GetStringFromBytes(byte[] bytes, ref int index)
        {
            if (bytes == null || index >= bytes.Length)
                return string.Empty;

            var nullIndex = Array.IndexOf(bytes, (byte)0, index);
            if (nullIndex == -1)
                nullIndex = bytes.Length;

            var length = nullIndex - index;
            if (length <= 0)
                return string.Empty;

            var result = System.Text.Encoding.UTF8.GetString(bytes, index, length);
            index = nullIndex + 1;
            return result;
        }

        /// <summary>
        /// 从字节数组中获取整数
        /// </summary>
        private int GetIntFromBytes(byte[] bytes, ref int index)
        {
            if (bytes == null || index + 4 > bytes.Length)
                return 0;

            var result = BitConverter.ToInt32(bytes, index);
            index += 4;
            return result;
        }
    }

    /// <summary>
    /// 异常报告数据
    /// </summary>
    public class ExceptionReportData
    {
        public ExceptionLevel Level { get; set; } = ExceptionLevel.Info;
        public string ExceptionType { get; set; } = string.Empty;
        public string ExceptionMessage { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
        public string SourceModule { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
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