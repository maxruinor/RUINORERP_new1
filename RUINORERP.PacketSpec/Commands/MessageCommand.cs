using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 消息命令基类 - 提供通用消息命令的实现
    /// </summary>
    [Command(0x0001, "MessageCommand", CommandCategory.Message, Description = "通用消息命令基类")]
    public class MessageCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier { get; }

        /// <summary>
        /// 命令类型
        /// </summary>
        public uint CommandType { get; }

        /// <summary>
        /// 命令数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="packetModel">会话信息</param>
        /// <param name="data">命令数据</param>
        public MessageCommand(uint commandType, PacketModel packetModel, object data)
            : base(CommandDirection.Receive)
        {

            throw new System.Exception("  这里要调试处理");

            CommandType = commandType;
            CommandIdentifier = new CommandId(GetCommandCategory(commandType), GetCommandCode(commandType));
            // packetModel = packetModel;
            Data = data;
            TimeoutMs = 30000; // 默认超时时间30秒

            // 如果提供了数据，设置原始数据
            if (data != null)
            {
                OriginalData = CreateOriginalDataFromObject(data);
            }
        }

        /// <summary>
        /// 根据命令类型获取命令类别
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>命令类别</returns>
        private CommandCategory GetCommandCategory(uint commandType)
        {
            // 从命令类型中提取类别信息（高16位）
            uint categoryValue = (commandType >> 16) & 0xFFFF;

            // 根据CommandCategory枚举的定义，使用switch语句映射
            switch (categoryValue)
            {
                case 0x00:
                    return CommandCategory.System;
                case 0x01:
                    return CommandCategory.Authentication;
                case 0x02:
                    return CommandCategory.Cache;
                case 0x03:
                    return CommandCategory.Message;
                case 0x04:
                    return CommandCategory.Workflow;
                case 0x05:
                    return CommandCategory.Exception;
                case 0x06:
                    return CommandCategory.File;
                case 0x07:
                    return CommandCategory.DataSync;
                case 0x08:
                    return CommandCategory.Lock;
                case 0x09:
                    return CommandCategory.SystemManagement;
                case 0x10:
                    return CommandCategory.Composite;
                case 0x11:
                    return CommandCategory.Connection;
                case 0x90:
                    return CommandCategory.Special;
                default:
                    // 对于未知的命令类别，默认使用系统类别
                    // LogWarning("未知的命令类别: {CategoryValue}", categoryValue);
                    return CommandCategory.System;
            };
        }

        /// <summary>
        /// 根据命令类型获取命令代码
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>命令代码</returns>
        private byte GetCommandCode(uint commandType)
        {
            // 从命令类型中提取代码信息（低8位）
            return (byte)(commandType & 0xFF);
        }

        /// <summary>
        /// 从对象创建原始数据
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns>原始数据</returns>
        private OriginalData CreateOriginalDataFromObject(object data)
        {
            try
            {
                // 根据数据类型创建适当的原始数据
                if (data is byte[] byteData)
                {
                    return new OriginalData((byte)(CommandType & 0xFF), byteData, null);
                }
                else if (data is string stringData)
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(stringData);
                    return new OriginalData((byte)(CommandType & 0xFF), bytes, null);
                }
                else
                {
                    // 对于其他类型的数据，序列化为JSON字符串
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    var bytes = System.Text.Encoding.UTF8.GetBytes(json);
                    return new OriginalData((byte)(CommandType & 0xFF), bytes, null);
                }
            }
            catch (System.Exception ex)
            {
                LogError("创建原始数据失败: " + ex.Message, ex);
                return new OriginalData((byte)(CommandType & 0xFF), null, null);
            }
        }

        /// <summary>
        /// 执行核心逻辑
        /// </summary>
        protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"执行消息命令: CommandType={{CommandType}}, SessionId={{SessionInfo?.SessionId}}");

                // 基本的消息命令执行逻辑
                // 实际的命令处理应该在具体的命令处理器中完成
                await Task.Delay(10, cancellationToken);

                return CommandResult.Success(
                    data: Data,
                    message: $"消息命令执行成功: {{CommandType}}"
                );
            }
            catch (System.Exception ex)
            {
                LogError($"执行消息命令异常: {{ex.Message}}", ex);
                return CommandResult.Failure(
                    message: $"执行消息命令失败: {{ex.Message}}",
                    errorCode: "MESSAGE_COMMAND_EXECUTION_FAILED",
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
            if (CommandType == 0)
            {
                return CommandValidationResult.Failure("命令类型不能为空", "INVALID_COMMAND_TYPE");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 是否需要会话信息
        /// </summary>
        protected override bool RequiresSession()
        {
            // 大多数消息命令都需要会话信息
            return true;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        protected override object GetSerializableData()
        {
            return Data;
        }

        /// <summary>
        /// 反序列化自定义数据
        /// </summary>
        protected override bool DeserializeCustomData(dynamic data)
        {
            try
            {
                Data = data;
                return true;
            }
            catch (System.Exception ex)
            {
                LogError("反序列化自定义数据失败: " + ex.Message, ex);
                return false;
            }
        }
    }
}
