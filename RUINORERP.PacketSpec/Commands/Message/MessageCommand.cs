using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

using System;
using System.Text;
using RUINORERP.PacketSpec.Enums.Core;
using FluentValidation.Results;

namespace RUINORERP.PacketSpec.Commands.Message
{
    /// <summary>
    /// 消息命令基类 - 提供通用消息命令的实现
    /// </summary>
    [PacketCommand("MessageCommand", CommandCategory.Message, Description = "通用消息命令基类")]
    public class MessageCommand : BaseCommand
    {
 

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
            : base(PacketDirection.Response)
        {
            // 注释掉调试用的异常抛出
            // throw new Exception("  这里要调试处理");

            CommandType = commandType;
            CommandIdentifier = new CommandId(GetCommandCategory(commandType), GetCommandCode(commandType));
            Data = data;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 根据命令类型获取命令类别
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>命令类别</returns>
        private CommandCategory GetCommandCategory(uint commandType)
        {
            // 从命令类型中提取类别信息（高16位）
            uint categoryValue = commandType >> 16 & 0xFFFF;

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
        /// 执行核心逻辑
        /// </summary>
        protected override async Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"执行消息命令: CommandType={{CommandType}}, SessionId={{SessionInfo?.SessionId}}");

                // 基本的消息命令执行逻辑
                // 实际的命令处理应该在具体的命令处理器中完成
                await Task.Delay(10, cancellationToken);

                return ResponseBase.CreateSuccess(
                    $"消息命令执行成功: {{CommandType}}"
                );
            }
            catch (Exception ex)
            {
                LogError($"执行消息命令异常: {{ex.Message}}", ex);
                return ResponseBase.CreateError(
                    $"执行消息命令失败: {{ex.Message}}",
                    500
                ).WithMetadata("ErrorCode", "MESSAGE_COMMAND_EXECUTION_FAILED")
                 .WithMetadata("Exception", ex.Message)
                 .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 验证命令
        /// </summary>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            var baseResult = await base.ValidateAsync(cancellationToken);
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            // 额外的命令验证
            if (CommandType == 0)
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(CommandType), "命令类型不能为空") });
            }

            return new ValidationResult();
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
        public override object GetSerializableData()
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
            catch (Exception ex)
            {
                LogError("反序列化自定义数据失败: " + ex.Message, ex);
                return false;
            }
        }
    }
}
