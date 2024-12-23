using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TransInstruction.CommandService
{
    public class CommandScheduler
    {
        private readonly CommandQueue _commandQueue;
        private readonly CommandDispatcher _commandDispatcher;

        public CommandScheduler(CommandQueue commandQueue, CommandDispatcher commandDispatcher)
        {
            _commandQueue = commandQueue;
            _commandDispatcher = commandDispatcher;
        }

        public void StartProcessing()
        {
            Task.Run(() => ProcessCommandsAsync());
        }

        private async Task ProcessCommandsAsync()
        {
            while (!_commandQueue.queue.IsCompleted)
            {
                IServerCommand command;
                if (_commandQueue.queue.TryTake(out command, Timeout.Infinite, CancellationToken.None))
                {
                    await _commandDispatcher.DispatchAsync(command, CancellationToken.None);
                }
            }
        }
    }
}
