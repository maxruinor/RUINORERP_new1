using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;

namespace RUINORERP.PacketSpec.Commands
{
    public sealed class QueuedCommand
    {
        public ICommand Command { get; set; }
        public TaskCompletionSource<ResponseBase> Tcs { get; set; }
    }
    
    public sealed class PriorityCommandQueue
    {
        private readonly Channel<QueuedCommand>[] _channels = new Channel<QueuedCommand>[3]; // High Normal Low
        
        public PriorityCommandQueue()
        {
            for (int i = 0; i < 3; i++)
                _channels[i] = Channel.CreateUnbounded<QueuedCommand>();
        }
        
        public ValueTask EnqueueAsync(ICommand cmd, CancellationToken ct = default)
        {
            var q = new QueuedCommand
            {
                Command = cmd,
                Tcs = new TaskCompletionSource<ResponseBase>(TaskCreationOptions.RunContinuationsAsynchronously)
            };
            
            // 根据命令优先级确定应该使用的Channel队列
            int priorityChannel = GetPriorityChannel(cmd.Priority);
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
        private int GetPriorityChannel(CommandPriority priority)
        {
            return priority switch
            {
                CommandPriority.High => 0,
                CommandPriority.Normal => 1,
                CommandPriority.Low => 2,
                _ => 1 // 默认使用普通优先级
            };
        }
    }
}