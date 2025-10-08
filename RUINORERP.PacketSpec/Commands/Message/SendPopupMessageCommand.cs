using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using FluentValidation.Results;
using MessagePack;
namespace RUINORERP.PacketSpec.Commands.Message
{
    /// <summary>
    /// 发送弹窗消息命令 - 用于向客户端发送弹窗消息
    /// </summary>
    [PacketCommand("SendPopupMessage", CommandCategory.Message)]
    [MessagePackObject]
    public class SendPopupMessageCommand : BaseCommand
    {
 

        /// <summary>
        /// 消息标题
        /// </summary>
        [Key(0)]
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Key(1)]
        public string Content { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [Key(2)]
        public string MessageType { get; set; }

        /// <summary>
        /// 目标用户ID列表
        /// </summary>
        [Key(3)]
        public string[] TargetUserIds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SendPopupMessageCommand()
        {
            TargetUserIds = new string[0];
            MessageType = "INFO"; // 默认信息类型
            Direction = PacketDirection.ServerToClient;
            CommandIdentifier = MessageCommands.SendPopupMessage;
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
            CommandIdentifier = MessageCommands.SendPopupMessage;
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

            // 验证消息标题
            if (string.IsNullOrWhiteSpace(Title))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(Title), "消息标题不能为空") });
            }

            // 验证消息内容
            if (string.IsNullOrWhiteSpace(Content))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(Content), "消息内容不能为空") });
            }

            // 验证消息类型
            if (string.IsNullOrWhiteSpace(MessageType))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(MessageType), "消息类型不能为空") });
            }

            return new ValidationResult();
        }

       
    }
}
