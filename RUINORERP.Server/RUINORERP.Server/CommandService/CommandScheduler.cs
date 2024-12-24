using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// 计划调试器
    /// </summary>
    public class CommandScheduler
    {
        /// <summary>
        /// 这个队列是不是 比方 要发给一个人。他不在线。就先不发。后面他上线就发。如果他在上线关。还可以取消不发送
        /// </summary>
        //private readonly CommandQueue _commandQueue;
        private readonly CommandDispatcher _commandDispatcher;
        //本来是私有的。
        public BlockingCollection<IServerCommand> queue = new BlockingCollection<IServerCommand>();

        public CommandScheduler( CommandDispatcher commandDispatcher)
        {
          // _commandQueue = commandQueue;
            _commandDispatcher = commandDispatcher;
        }

        public void StartProcessing()
        {
            Task.Run(() => ProcessCommandsAsync());
        }
         
         private async Task ProcessCommandsAsync()
         {
             while (!queue.IsCompleted)
             {
                 IServerCommand command;
                 if (queue.TryTake(out command, Timeout.Infinite, CancellationToken.None))
                 {
                     await _commandDispatcher.DispatchAsync(command, CancellationToken.None);
                 }
             }
         }

    }
}
