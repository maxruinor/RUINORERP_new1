using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;

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
            return _channels[(int)cmd.Priority].Writer.WriteAsync(q, ct);
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
    }
}
