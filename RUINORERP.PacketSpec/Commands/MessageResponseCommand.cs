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
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 文本消息
        /// </summary>
        Text = 0,

        /// <summary>
        /// 提示消息
        /// </summary>
        Prompt = 1,

        /// <summary>
        /// 即时消息
        /// </summary>
        IM = 2,

        /// <summary>
        /// 业务数据
        /// </summary>
        BusinessData = 3,

        /// <summary>
        /// 事件消息
        /// </summary>
        Event = 4,

        /// <summary>
        /// 系统消息
        /// </summary>
        System = 5
    }

    /// <summary>
    /// 提示类型
    /// </summary>
    public enum PromptType
    {
        /// <summary>
        /// 信息提示
        /// </summary>
        Information = 0,

        /// <summary>
        /// 警告提示
        /// </summary>
        Warning = 1,

        /// <summary>
        /// 错误提示
        /// </summary>
        Error = 2,

        /// <summary>
        /// 确认窗口
        /// </summary>
        Confirmation = 3,

        /// <summary>
        /// 输入窗口
        /// </summary>
        Input = 4
    }

    /// <summary>
    /// 下一步处理
    /// </summary>
    public enum NextProcessStep
    {
        /// <summary>
        /// 无后续处理
        /// </summary>
        None = 0,

        /// <summary>
        /// 转发
        /// </summary>
        Forward = 1,

        /// <summary>
        /// 广播
        /// </summary>
        Broadcast = 2,

        /// <summary>
        /// 存储
        /// </summary>
        Store = 3
    }

    /// <summary>
    /// 消息响应处理命令
    /// </summary>
    public class MessageResponseCommand : BaseCommand
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageType MessageType { get; set; } = MessageType.Text;

        /// <summary>
        /// 提示类型
        /// </summary>
        public PromptType PromptType { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 发送者会话信息
        /// </summary>
        public SessionInfo FromSession { get; set; }

        /// <summary>
        /// 接收者会话信息
        /// </summary>
        public SessionInfo ToSession { get; set; }

        /// <summary>
        /// 下一步处理
        /// </summary>
        public NextProcessStep NextStep { get; set; }

        /// <summary>
        /// 接收者会话ID
        /// </summary>
        public string ReceiverSessionId { get; set; }

        /// <summary>
        /// 消息数据
        /// </summary>
        public object MessageData { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageResponseCommand(CmdOperation operationType = CmdOperation.Receive) : base(operationType)
        {
        }

        /// <summary>
        /// 执行消息处理命令
        /// </summary>
        public override async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                LogExecution($"开始执行消息处理命令: {MessageType}");

                switch (OperationType)
                {
                    case CmdOperation.Receive:
                        return await ProcessReceivedMessage(cancellationToken);
                    
                    case CmdOperation.Send:
                        return await ProcessSendMessage(cancellationToken);
                    
                    case CmdOperation.Forward:
                        return await ProcessForwardMessage(cancellationToken);
                    
                    case CmdOperation.Broadcast:
                        return await ProcessBroadcastMessage(cancellationToken);
                    
                    default:
                        return CommandResult.CreateError($"不支持的操作类型: {OperationType}");
                }
            }
            catch (Exception ex)
            {
                LogExecution($"消息处理命令执行异常: {ex.Message}", ex);
                return CommandResult.CreateError($"消息处理异常: {ex.Message}", ex.GetType().Name);
            }
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        private async Task<CommandResult> ProcessReceivedMessage(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            
            switch (MessageType)
            {
                case MessageType.IM:
                    return await ProcessIMMessage(cancellationToken);
                
                case MessageType.BusinessData:
                    return await ProcessBusinessDataMessage(cancellationToken);
                
                case MessageType.Event:
                    return await ProcessEventMessage(cancellationToken);
                
                default:
                    return CommandResult.CreateSuccess(MessageData, "消息接收成功");
            }
        }

        /// <summary>
        /// 处理发送消息
        /// </summary>
        private async Task<CommandResult> ProcessSendMessage(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            BuildDataPacket(MessageData);
            return CommandResult.CreateSuccess(MessageData, "消息发送成功");
        }

        /// <summary>
        /// 处理转发消息
        /// </summary>
        private async Task<CommandResult> ProcessForwardMessage(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            
            if (ToSession == null && string.IsNullOrEmpty(ReceiverSessionId))
            {
                return CommandResult.CreateError("转发消息缺少目标会话信息");
            }

            BuildDataPacket(MessageData);
            return CommandResult.CreateSuccess(MessageData, "消息转发成功");
        }

        /// <summary>
        /// 处理广播消息
        /// </summary>
        private async Task<CommandResult> ProcessBroadcastMessage(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            BuildDataPacket(MessageData);
            return CommandResult.CreateSuccess(MessageData, "消息广播成功");
        }

        /// <summary>
        /// 处理即时消息
        /// </summary>
        private async Task<CommandResult> ProcessIMMessage(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);

            if (NextStep == NextProcessStep.Forward && !string.IsNullOrEmpty(ReceiverSessionId))
            {
                // 转发给指定接收者
                BuildDataPacket(MessageData);
                return CommandResult.CreateSuccess(MessageData, "即时消息转发成功");
            }

            return CommandResult.CreateSuccess(MessageData, "即时消息处理成功");
        }

        /// <summary>
        /// 处理业务数据消息
        /// </summary>
        private async Task<CommandResult> ProcessBusinessDataMessage(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            
            // 这里可以添加业务数据处理逻辑
            return CommandResult.CreateSuccess(MessageData, "业务数据消息处理成功");
        }

        /// <summary>
        /// 处理事件消息
        /// </summary>
        private async Task<CommandResult> ProcessEventMessage(CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            
            // 这里可以添加事件处理逻辑
            return CommandResult.CreateSuccess(MessageData, "事件消息处理成功");
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
                var timeStr = GetStringFromBytes(data.Two, ref index);
                MessageType = (MessageType)GetIntFromBytes(data.Two, ref index);

                switch (MessageType)
                {
                    case MessageType.IM:
                        MessageContent = GetStringFromBytes(data.Two, ref index);
                        NextStep = (NextProcessStep)GetIntFromBytes(data.Two, ref index);
                        ReceiverSessionId = GetStringFromBytes(data.Two, ref index);
                        break;

                    case MessageType.Prompt:
                        MessageContent = GetStringFromBytes(data.Two, ref index);
                        PromptType = (PromptType)GetIntFromBytes(data.Two, ref index);
                        if (index < data.Two.Length)
                        {
                            ReceiverSessionId = GetStringFromBytes(data.Two, ref index);
                        }
                        break;

                    case MessageType.BusinessData:
                        var jsonData = GetStringFromBytes(data.Two, ref index);
                        if (!string.IsNullOrEmpty(jsonData))
                        {
                            MessageData = JsonConvert.DeserializeObject(jsonData);
                        }
                        break;

                    default:
                        MessageContent = GetStringFromBytes(data.Two, ref index);
                        break;
                }

                FromSession = sessionInfo;
                return true;
            }
            catch (Exception ex)
            {
                LogExecution($"解析消息数据包失败: {ex.Message}", ex);
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
                    Cmd = (byte)ServerCommand.MessageResponse,
                    One = new byte[] { (byte)MessageType },
                    Two = BuildMessageResponseData()
                };

                DataPacket = data;
            }
            catch (Exception ex)
            {
                LogExecution($"构建消息数据包失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 构建消息响应数据
        /// </summary>
        private byte[] BuildMessageResponseData()
        {
            var data = new StringBuilder();
            var sendTime = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            
            // 添加发送时间
            data.Append(sendTime).Append('\0');
            
            // 添加消息类型
            data.Append((int)MessageType).Append('\0');

            switch (MessageType)
            {
                case MessageType.Prompt:
                    data.Append(MessageContent ?? string.Empty).Append('\0');
                    data.Append((int)PromptType).Append('\0');
                    if (ToSession != null)
                    {
                        data.Append(ToSession.SessionId ?? string.Empty).Append('\0');
                    }
                    break;

                case MessageType.IM:
                    data.Append(MessageContent ?? string.Empty).Append('\0');
                    data.Append((int)NextStep).Append('\0');
                    data.Append(FromSession?.SessionId ?? string.Empty).Append('\0');
                    break;

                case MessageType.BusinessData:
                    var json = MessageData != null ? JsonConvert.SerializeObject(MessageData) : string.Empty;
                    data.Append(json).Append('\0');
                    break;

                default:
                    data.Append(MessageContent ?? string.Empty).Append('\0');
                    break;
            }

            return Encoding.UTF8.GetBytes(data.ToString());
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
            var result = Encoding.UTF8.GetString(bytes, index, length);
            index = nullIndex + 1;
            return result;
        }

        /// <summary>
        /// 从字节数组中获取整数
        /// </summary>
        private int GetIntFromBytes(byte[] bytes, ref int index)
        {
            var str = GetStringFromBytes(bytes, ref index);
            return int.TryParse(str, out var result) ? result : 0;
        }

        /// <summary>
        /// 验证命令是否可以执行
        /// </summary>
        public override bool CanExecute()
        {
            return base.CanExecute() && 
                   ((OperationType == CmdOperation.Receive && DataPacket.HasValue) ||
                    (OperationType != CmdOperation.Receive && !string.IsNullOrEmpty(MessageContent)));
        }
    }

    /// <summary>
    /// 消息响应命令处理器
    /// </summary>
    [CommandHandler("MessageResponseCommandHandler")]
    public class MessageResponseCommandHandler : BaseCommandHandler
    {
        public override async Task<CommandResult> HandleAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command is MessageResponseCommand messageCommand)
            {
                LogMessage($"开始处理消息命令: {messageCommand.MessageType}");
                
                var result = await messageCommand.ExecuteAsync(cancellationToken);
                
                LogMessage($"消息命令处理完成: {messageCommand.MessageType} - {result.Success}");
                
                return result;
            }

            return CommandResult.CreateError("不支持的命令类型");
        }

        public override bool CanHandle(ICommand command, System.Collections.Concurrent.BlockingCollection<ICommand> queue = null)
        {
            return command is MessageResponseCommand;
        }
    }
}