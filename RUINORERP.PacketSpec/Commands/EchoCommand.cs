using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Enums.Core;
using FluentValidation.Results;


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
            CommandIdentifier = new CommandId(CommandCategory.Special, 0x01);
            // 设置命令方向
            Direction = PacketDirection.ServerToClient;
        }
        
 

 
        
        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.ValidateAsync(cancellationToken);
            if (!result.IsValid)
            {
                return result;
            }
            
            // 验证Message参数
            if (string.IsNullOrWhiteSpace(Message))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(Message), "消息不能为空") });
            }
            
            return new ValidationResult();
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
                TimestampUtc = DateTime.UtcNow
            };
            result.WithMetadata("Data", Message);
            return Task.FromResult((ResponseBase)result);
        }
    }
}
