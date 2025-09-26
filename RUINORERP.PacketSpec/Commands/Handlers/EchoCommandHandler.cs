using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands.Handlers
{
    /// <summary>
    /// 回显命令处理器
    /// 处理EchoCommand命令的处理器实现
    /// </summary>
    public class EchoCommandHandler : BaseCommandHandler
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public override string Name => "回显命令处理器";

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 10;

        /// <summary>
        /// 构造函数
        /// </summary>
        private readonly IReadOnlyList<uint> _supportedCommands;

        /// <summary>
        /// 支持的命令类型列表
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => _supportedCommands;

        protected ILogger<EchoCommandHandler> logger { get; set; }
        
        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public EchoCommandHandler() : base()
        {
            logger = new LoggerFactory().CreateLogger<EchoCommandHandler>();
            // 注册支持的命令类型
            // 使用Special类别和0x01操作码，与EchoCommand中定义的一致
            _supportedCommands = new List<uint>
            {
                new CommandId(CommandCategory.Special, 0x01).FullCode
            };
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EchoCommandHandler(ILogger<EchoCommandHandler> _Logger):base (_Logger)
        {
            logger = _Logger;
            // 注册支持的命令类型
            // 使用Special类别和0x01操作码，与EchoCommand中定义的一致
            _supportedCommands = new List<uint>
            {
                new CommandId(CommandCategory.Special, 0x01).FullCode
            };
        }

        /// <summary>
        /// 判断处理器是否可以处理指定命令
        /// </summary>
        /// <param name="command">要判断的命令</param>
        /// <returns>是否可以处理</returns>
        public override bool CanHandle(ICommand command)
        {
            return command is EchoCommand;
        }

        /// <summary>
        /// 处理命令的核心逻辑
        /// </summary>
        /// <param name="command">要处理的命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<ResponseBase> OnHandleAsync(ICommand command, CancellationToken cancellationToken)
        {
            var echoCommand = command as EchoCommand;
            if (echoCommand == null)
            {
                return ConvertToApiResponse(ResponseBase.CreateError("无效的命令类型", 400).WithMetadata("ErrorCode", "INVALID_COMMAND_TYPE"));
            }

            try
            {
                // 记录处理信息
                LogDebug($"处理回显命令: {echoCommand.Message}");

                // 创建成功结果，返回处理后的消息
                var responseData = new {
                    OriginalMessage = echoCommand.Message,
                    Timestamp = DateTime.UtcNow,
                    HandlerId = this.HandlerId
                };
                
                var result = new ResponseBase
                {
                    IsSuccess = true,
                    Message = "回显命令处理成功",
                    Code = 200,
                    Timestamp = DateTime.UtcNow
                };
                
                result.WithMetadata("Data", responseData);
                
                // 模拟一些处理延迟
                return result;
            }
            catch (Exception ex)
            {
                LogError("处理回显命令时发生异常", ex);
                return ConvertToApiResponse(ResponseBase.CreateError($"处理回显命令时发生异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "PROCESSING_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        /// <summary>
        /// 将ResponseBase转换为ApiResponse
        /// </summary>
        /// <param name="baseResponse">基础响应对象</param>
        /// <returns>ApiResponse对象</returns>
        private ResponseBase ConvertToApiResponse(ResponseBase baseResponse)
        {
            var response = new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                Code = baseResponse.Code,
                Timestamp = baseResponse.Timestamp,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata,
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }
}
