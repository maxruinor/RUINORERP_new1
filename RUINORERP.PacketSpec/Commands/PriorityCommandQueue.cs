using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;

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

    /// <summary>
    /// 优先级命令队列，根据命令优先级进行排队和处理
    /// </summary>
    public sealed class PriorityCommandQueue
    {
        private readonly Channel<QueuedCommand>[] _channels = new Channel<QueuedCommand>[3]; // High Normal Low

        /// <summary>
        /// 初始化优先级命令队列
        /// </summary>
        public PriorityCommandQueue()
        {
            for (int i = 0; i < 3; i++)
                _channels[i] = Channel.CreateUnbounded<QueuedCommand>();
        }

        /// <summary>
        /// 将命令包异步加入队列
        /// </summary>
        /// <param name="packet">包含命令数据的数据包</param>
        /// <param name="priority">命令优先级（可选，默认从数据包获取）</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>加入队列的异步任务</returns>
        public ValueTask EnqueueAsync(PacketModel packet, PacketPriority? priority = null, CancellationToken ct = default)
        {
            var q = new QueuedCommand
            {
                Packet = packet,
                Tcs = new TaskCompletionSource<IResponse>(TaskCreationOptions.RunContinuationsAsynchronously)
            };

            // 使用指定的优先级或从数据包获取优先级
            var actualPriority = priority ?? packet.PacketPriority;
            
            // 根据命令优先级确定应该使用的Channel队列
            int priorityChannel = GetPriorityChannel(actualPriority);
            return _channels[priorityChannel].Writer.WriteAsync(q, ct);
        }

        public async Task<QueuedCommand> DequeueAsync(CancellationToken ct = default)
        {
            // 优先级轮询：High->Normal->Low
            for (int i = 0; i < 3; i++)
                if (_channels[i].Reader.TryRead(out var item))
                    return item;

            // 若都无，异步等高优先级
            return await _channels[0].Reader.ReadAsync(ct);
        }

        /// <summary>
        /// 根据命令优先级确定应该使用的Channel队列
        /// </summary>
        /// <param name="priority">命令优先级</param>
        /// <returns>Channel队列索引 (0=高优先级, 1=普通优先级, 2=低优先级)</returns>
        private int GetPriorityChannel(PacketPriority priority)
        {
            return priority switch
            {
                PacketPriority.High => (int)PacketPriority.High,
                PacketPriority.Normal => (int)PacketPriority.Normal,
                PacketPriority.Low => (int)PacketPriority.Low,
                _ => (int)PacketPriority.Normal // 默认使用普通优先级
            };
        }
    }
}
