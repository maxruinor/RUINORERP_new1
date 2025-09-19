using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Commands.Message
{
    /// <summary>
    /// 发送弹窗消息命令
    /// 用于向客户端发送弹窗消息通知
    /// 实际这些具体业务的处理是不是放到实际业务层。当前项目是中间层
    /// </summary>
    [Command(0x0300, "SendPopupMessage", CommandCategory.Message, Description = "发送弹窗消息命令")]
    public class SendPopupMessageCommand : MessageCommand
    {
        /// <summary>
        /// 弹窗消息内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 弹窗消息标题
        /// </summary>
        public string MessageTitle { get; set; }

        /// <summary>
        /// 弹窗消息类型
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="packetModel">会话信息中的业实际数据</param>
        /// <param name="data">命令数据</param>
        public SendPopupMessageCommand(uint commandType, PacketModel packetModel, object data)
            : base(commandType, packetModel, data)
        {
            // 从数据对象中提取消息相关属性
            ParseMessageData(data);
        }

        /// <summary>
        /// 解析消息数据
        /// </summary>
        /// <param name="data">数据对象</param>
        private void ParseMessageData(object data)
        {
            if (data == null)
            {
                return;
            }

            try
            {
                // 如果数据是字典类型，直接从中提取信息
                if (data is IDictionary<string, object> dictData)
                {
                    if (dictData.ContainsKey("Content"))
                        MessageContent = dictData["Content"]?.ToString();

                    if (dictData.ContainsKey("Title"))
                        MessageTitle = dictData["Title"]?.ToString();

                    if (dictData.ContainsKey("Type"))
                    {
                        if (Enum.TryParse<MessageType>(dictData["Type"]?.ToString(), true, out var messageType))
                            MessageType = messageType;
                    }
                }
                // 如果数据是字符串类型，直接作为消息内容
                else if (data is string stringData)
                {
                    MessageContent = stringData;
                    MessageTitle = "系统通知";
                    MessageType = MessageType.Info;
                }
            }
            catch (System.Exception ex)
            {
                LogError("解析消息数据失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 执行核心逻辑
        /// 发送弹窗消息到客户端
        /// </summary>
        protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"准备发送弹窗消息: Title={{MessageTitle}}, Type={{MessageType}}, SessionId={{SessionInfo?.SessionId}}");

                // 确保必要的消息属性不为空
                if (string.IsNullOrEmpty(MessageContent))
                {
                    return CommandResult.Failure(
                        message: "消息内容不能为空",
                        errorCode: "EMPTY_MESSAGE_CONTENT"
                    );
                }

                // 设置默认值
                if (string.IsNullOrEmpty(MessageTitle))
                    MessageTitle = "系统通知";

                // 构建消息对象
                var messageData = new
                {
                    Title = MessageTitle,
                    Content = MessageContent,
                    Type = MessageType.ToString(),
                    Timestamp = DateTime.UtcNow
                };

                // 模拟发送消息的异步操作
                await Task.Delay(50, cancellationToken);

                LogInfo($"弹窗消息发送成功: Title={{MessageTitle}}, SessionId={{SessionInfo?.SessionId}}");

                return CommandResult.Success(
                    data: messageData,
                    message: $"弹窗消息发送成功"
                );
            }
            catch (System.Exception ex)
            {
                LogError($"发送弹窗消息失败: {{ex.Message}}", ex);
                return CommandResult.Failure(
                    message: $"发送弹窗消息失败: {{ex.Message}}",
                    errorCode: "SEND_POPUP_MESSAGE_FAILED",
                    exception: ex
                );
            }
        }

        /// <summary>
        /// 验证命令
        /// </summary>
        public override CommandValidationResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            // 额外的命令验证
            if (string.IsNullOrEmpty(MessageContent))
            {
                return CommandValidationResult.Failure("消息内容不能为空", "EMPTY_MESSAGE_CONTENT");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        protected override object GetSerializableData()
        {
            return new
            {
                Title = MessageTitle,
                Content = MessageContent,
                Type = MessageType
            };
        }
    }

    /// <summary>
    /// 消息类型枚举
    /// 定义不同类型的弹窗消息
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 信息提示
        /// </summary>
        Info,

        /// <summary>
        /// 成功提示
        /// </summary>
        Success,

        /// <summary>
        /// 警告提示
        /// </summary>
        Warning,

        /// <summary>
        /// 错误提示
        /// </summary>
        Error,

        /// <summary>
        /// 确认对话框
        /// </summary>
        Confirm
    }
}
