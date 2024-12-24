using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TransInstruction.CommandService;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// 为了处理并发命令，可以实现一个命令队列，命令被添加到队列中，并由一个或多个工作者线程异步执行
    /// 或是 服务器发送给客户，客户不在线。在线后再发。保存在队列中。当然也可以在发前取消。
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
                command.ExecuteAsync(cancellationToken);
            }
        }
        
    }
}
