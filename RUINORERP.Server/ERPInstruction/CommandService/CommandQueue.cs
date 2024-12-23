using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TransInstruction.CommandService
{
    /// <summary>
    /// 为了处理并发命令，可以实现一个命令队列，命令被添加到队列中，并由一个或多个工作者线程异步执行
    /// </summary>
    public class CommandQueue
    {
        //本来是私有的。
        public BlockingCollection<IServerCommand> queue = new BlockingCollection<IServerCommand>();

        public void EnqueueCommand(IServerCommand command)
        {
            queue.Add(command);
        }
        public void ProcessCommands(CancellationToken cancellationToken)
        {
            foreach (var command in queue.GetConsumingEnumerable(cancellationToken))
            {
                Task.Run(() => command.Execute(), cancellationToken);
            }
        }
        public void ProcessCommands()
        {
            foreach (var command in queue.GetConsumingEnumerable())
            {
                command.Execute();
            }
        }
    }
}
