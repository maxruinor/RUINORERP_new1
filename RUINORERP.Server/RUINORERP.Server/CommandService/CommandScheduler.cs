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
        private readonly CommandDispatcher commandDispatcher;
        //本来是私有的。
        public BlockingCollection<IServerCommand> queue = new BlockingCollection<IServerCommand>();

        public CommandScheduler(CommandDispatcher _commandDispatcher)
        {
            // _commandQueue = commandQueue;
            commandDispatcher = _commandDispatcher;
        }

        public void StartProcessing()
        {
            Task.Run(() => ProcessCommandsAsync());
        }

        //要加try
        private async Task ProcessCommandsAsync()
        {
            while (!queue.IsCompleted)
            {
                IServerCommand command;
               //TryTake
               //尝试在指定的时间段内从 BlockingCollection<T> 中移除一个项。
               //itimeout 为 -1 毫秒以外的负数，表示无限期超时
               //结果：true 如果可以在指定时间内从集合中删除项，则为 ;否则为 false。
               if (queue.TryTake(out command, Timeout.Infinite, CancellationToken.None))
                {
                    await commandDispatcher.DispatchAsync(queue,command, CancellationToken.None);
                }
            }
            /*
            while (!queue.IsCompleted)
            {
                IServerCommand command;
                if (queue.TryTake(out command, Timeout.Infinite, CancellationToken.None))
                {
                    if (commandDispatcher.handlers.TryGetValue(command.GetType(), out var handler) && handler.CanHandle(command))
                    {
                        await commandDispatcher.DispatchAsync(command, CancellationToken.None);
                    }
                    else
                    {
                        // 如果处理器不能处理，重新放回队列或存储起来
                        queue.Add(command);
                        // 或者存储在持久化存储中，直到用户上线
                    }
                }

            }
            */
        }

    }
}
