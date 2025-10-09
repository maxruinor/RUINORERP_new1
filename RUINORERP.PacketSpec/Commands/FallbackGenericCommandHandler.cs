using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令调度器的默认回退处理器
    /// 用于处理没有专门处理器的命令
    /// 替代原有的GenericCommandHandler实现
    /// </summary>
    public sealed class FallbackGenericCommandHandler : BaseCommandHandler
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FallbackGenericCommandHandler()
        {
            // 该处理器不指定具体支持的命令码，而是作为所有未注册命令的回退
        }

        /// <summary>
        /// 获取处理器名称
        /// </summary>
        public override string Name => "FallbackGenericHandler";

        /// <summary>
        /// 此处理器不特定支持任何命令码
        /// 它将作为CommandDispatcher中的默认回退处理器
        /// </summary>
        public new IReadOnlyList<uint> SupportedCommands => Array.Empty<uint>();

        /// <summary>
        /// 处理命令的核心逻辑
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>处理结果</returns>
        protected override Task<ResponseBase> OnHandleAsync(QueuedCommand cmd, CancellationToken ct)
        {
            try
            {
                LogInfo($"处理未注册命令: {cmd.Command.CommandIdentifier} [ID: {cmd.Command.CommandIdentifier.ToString()}]");
                
                // 尝试获取命令的可序列化数据
                var payload = cmd.Command.GetSerializableData();
                
                if (payload != null)
                {
                    // 如果有有效载荷，返回成功响应并包含原数据
                    return Task.FromResult(ResponseBase.CreateError($"未注册命令 [{cmd.Command.CommandIdentifier}] 已通过回退处理器处理")
                        .WithMetadata("Message", $"未注册命令 [{cmd.Command.CommandIdentifier}] 已通过回退处理器处理")
                        .WithMetadata("CommandType", payload.GetType().FullName)
                        .WithMetadata("HandlerType", Name));
                }
                else
                {
                    // 没有有效载荷，返回成功响应
                    return Task.FromResult(ResponseBase.CreateSuccess()
                        .WithMetadata("Message", $"未注册命令 [{cmd.Command.CommandIdentifier}] 已通过回退处理器处理")
                        .WithMetadata("HandlerType", Name));
                }
            }
            catch (Exception ex)
            {
                LogError($"处理未注册命令时发生异常: {cmd.Command.CommandIdentifier}", ex);
                
                // 发生异常时返回错误响应
                return Task.FromResult(ResponseBase.CreateError("处理未注册命令时发生异常")
                    .WithMetadata("ErrorCode", "FALLBACK_HANDLER_ERROR")
                    .WithMetadata("ErrorMessage", ex.Message)
                    .WithMetadata("CommandIdentifier", cmd.Command.CommandIdentifier)
                    .WithMetadata("HandlerType", Name));
            }
        }
    }
}