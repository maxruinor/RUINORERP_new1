using RUINORERP.PacketSpec.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Commands
{
    /// <summary>
    /// 心跳命令
    /// 用于保持客户端与服务器的连接活跃
    /// </summary>
    [Command(0x00F0, "Heartbeat", CommandCategory.System, Description = "心跳命令")]
    public class HeartbeatCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// 使用系统命令类别
        /// </summary>
        public override CommandId CommandIdentifier => new CommandId(CommandCategory.System, 0xF0);

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        public HeartbeatCommand(string clientId = null)
        {
            ClientId = clientId;
            Priority = CommandPriority.Low; // 心跳命令优先级低
            TimeoutMs = 5000; // 心跳超时时间短
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的心跳数据</returns>
        protected override object GetSerializableData()
        {
            return new {
                Timestamp = DateTime.Now.Ticks,
                ClientId = ClientId
            };
        }

        /// <summary>
        /// 命令执行的具体逻辑
        /// 客户端心跳命令通常只构建数据，不执行复杂逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        protected override async Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 客户端通常只构建命令，实际的发送由通信服务处理
            // 这里可以添加一些自定义逻辑，例如记录心跳发送时间等
            return CommandResult.Success(GetSerializableData());
        }
    }
}