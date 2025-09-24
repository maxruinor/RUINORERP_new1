using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.System
{
    /// <summary>
    /// 心跳命令
    /// 用于客户端向服务器发送心跳包，维持连接活跃状态
    /// </summary>
    [PacketCommandAttribute("Heartbeat", CommandCategory.System, Description = "心跳命令")]
    public class HeartbeatCommand : RUINORERP.PacketSpec.Commands.BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => SystemCommands.Heartbeat;

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatCommand()
        {
            Priority = CommandPriority.Normal;
            TimeoutMs = 10000; // 心跳命令超时时间10秒
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        public HeartbeatCommand(string clientId)
        {
            ClientId = clientId;
            Priority = CommandPriority.Normal;
            TimeoutMs = 10000; // 心跳命令超时时间10秒
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的心跳数据</returns>
        public override object GetSerializableData()
        {
            return new
            {
                ClientId = this.ClientId,
                Timestamp = this.Timestamp
            };
        }

        /// <summary>
        /// 命令执行的具体逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 验证客户端ID
                if (string.IsNullOrEmpty(ClientId))
                {
                    return CommandResult.Failure("客户端ID不能为空", "EMPTY_CLIENT_ID");
                }

                // 构建心跳数据
                var heartbeatData = GetSerializableData();

                // 返回成功结果，实际的网络请求由通信服务处理
                return CommandResult.Success(heartbeatData, "心跳命令构建成功");
            }
            catch (Exception ex)
            {
                return CommandResult.Failure($"心跳命令执行异常: {ex.Message}", "HEARTBEAT_EXCEPTION", ex);
            }
        }

        /// <summary>
        /// 验证命令数据
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            // 调用基类验证
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            // 验证客户端ID
            if (string.IsNullOrEmpty(ClientId))
            {
                return CommandValidationResult.Failure("客户端ID不能为空");
            }

            return CommandValidationResult.Success();
        }
    }
}