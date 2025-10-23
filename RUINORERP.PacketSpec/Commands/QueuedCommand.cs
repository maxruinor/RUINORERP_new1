using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 排队命令类，用于在命令队列中传递和处理命令
    /// </summary>
    public sealed class QueuedCommand
    {
        /// <summary>
        /// 数据包模型，包含命令相关的所有信息
        /// </summary>
        public PacketModel Packet { get; set; }
        
        /// <summary>
        /// 任务完成源，用于异步获取命令处理结果
        /// </summary>
        public TaskCompletionSource<IResponse> Tcs { get; set; }
    }
}
