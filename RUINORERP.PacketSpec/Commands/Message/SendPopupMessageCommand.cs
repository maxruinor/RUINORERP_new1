using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;

namespace RUINORERP.PacketSpec.Commands.Message
{
    /// <summary>
    /// 发送弹窗消息命令 - 用于向客户端发送弹窗消息
    /// </summary>
    [PacketCommand("SendPopupMessage", CommandCategory.Message)]
    public class SendPopupMessageCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => MessageCommands.SendPopupMessage;

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 目标用户ID列表
        /// </summary>
        public string[] TargetUserIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SendPopupMessageCommand()
        {
            TargetUserIds = new string[0];
            MessageType = "INFO"; // 默认信息类型
            Direction = PacketDirection.ServerToClient;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="title">消息标题</param>
        /// <param name="content">消息内容</param>
        /// <param name="messageType">消息类型</param>
        /// <param name="targetUserIds">目标用户ID列表</param>
        public SendPopupMessageCommand(string title, string content, string messageType = "INFO", string[] targetUserIds = null)
        {
            Title = title;
            Content = content;
            MessageType = messageType;
            TargetUserIds = targetUserIds ?? new string[0];
            Direction = PacketDirection.ServerToClient;
        }

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

            // 验证消息标题
            if (string.IsNullOrWhiteSpace(Title))
            {
                return CommandValidationResult.Failure("消息标题不能为空", "INVALID_MESSAGE_TITLE");
            }

            // 验证消息内容
            if (string.IsNullOrWhiteSpace(Content))
            {
                return CommandValidationResult.Failure("消息内容不能为空", "INVALID_MESSAGE_CONTENT");
            }

            // 验证消息类型
            if (string.IsNullOrWhiteSpace(MessageType))
            {
                return CommandValidationResult.Failure("消息类型不能为空", "INVALID_MESSAGE_TYPE");
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
            // 发送弹窗消息命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            var result = ResponseBase.CreateSuccess("发送弹窗消息命令构建成功")
                .WithMetadata("Data", new { Title = Title, Content = Content, MessageType = MessageType, TargetUserIds = TargetUserIds });
            return Task.FromResult((ResponseBase)result);
        }
    }
}
