using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 回显命令 - 简单的示例命令，用于测试命令系统
    /// 发送消息并返回相同的消息
    /// </summary>
    public class EchoCommand : BaseCommand
    {
        /// <summary>
        /// 要回显的消息
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public EchoCommand()
        {
            // 设置命令标识符
            _commandIdentifier = new CommandId(CommandCategory.Special, 0x01);
            // 设置命令方向
            Direction = CommandDirection.Send;
        }
        
        private readonly CommandId _commandIdentifier;

        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => _commandIdentifier;
        
        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }
            
            // 验证Message参数
            if (string.IsNullOrWhiteSpace(Message))
            {
                return CommandValidationResult.Failure("消息不能为空", "INVALID_MESSAGE");
            }
            
            return CommandValidationResult.Success();
        }
        
        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 创建成功结果，返回原始消息
            var result = new ResponseBase
            {
                IsSuccess = true,
                Message = "Echo command executed successfully",
                Code = 200,
                Timestamp = DateTime.UtcNow
            };
            result.WithMetadata("Data", Message);
            return Task.FromResult((ResponseBase)result);
        }
    }
}
