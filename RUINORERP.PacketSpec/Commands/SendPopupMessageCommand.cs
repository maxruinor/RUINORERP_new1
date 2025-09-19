using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 发送弹窗消息命令
    /// </summary>
    [Command(1001, "SendPopupMessage", 
        CommandCategory.Message, 
        Description = "发送弹窗消息命令")]
    public class SendPopupMessageCommand : BaseCommand
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 目标用户ID
        /// </summary>
        public string TargetUserId { get; set; }

        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => new CommandId(CommandCategory.Message, 0x01);

        /// <summary>
        /// 构造函数
        /// </summary>
        public SendPopupMessageCommand() : base()
        {
        }

        /// <summary>
        /// 执行核心逻辑
        /// </summary>
        protected override async System.Threading.Tasks.Task<CommandResult> OnExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            // 模拟发送弹窗消息
            await System.Threading.Tasks.Task.Delay(100, cancellationToken);
            
            return CommandResult.Success(
                data: new { Message = "弹窗消息发送成功", Content, TargetUserId },
                message: "弹窗消息发送成功"
            );
        }
    }
}