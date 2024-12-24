using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.CommandService
{
    public class CommandProcessor
    {
        private readonly CommandQueue _commandQueue;
      

      //  private readonly BlockingCollection<ICommand> _commandQueue;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public CommandProcessor()
        {
            _commandQueue = new CommandQueue();
            //    _commandQueue = new BlockingCollection<ICommand>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void StartProcessing()
        {
            Task.Run(() => ProcessCommandsAsync(_cancellationTokenSource.Token));
        }
        private async Task ProcessCommandsAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Run(() => _commandQueue.ProcessCommands(cancellationToken), cancellationToken);
            }
            catch (OperationCanceledException)
            {
                // Handle the cancellation
            }
        }
        //private async Task ProcessCommandsAsync(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        foreach (var command in _commandQueue.GetConsumingEnumerable(cancellationToken))
        //        {
        //            await ExecuteCommandAsync(command);
        //        }
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        // Handle the cancellation
        //    }
        //}

        private async Task ExecuteCommandAsync(IServerCommand command)
        {
            // Simulate some work
            await Task.Delay(100);
            command.Execute();
        }

        //public void EnqueueCommand(ICommand command)
        //{
        //    _commandQueue.Add(command);
        //}
        public void EnqueueCommand(IServerCommand command)
        {
            _commandQueue.EnqueueCommand(command);
        }

  
        public void StopProcessing()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}

    // Usage
    //class Program
    //{
    //    static async Task Main(string[] args)
    //    {
    //        var processor = new CommandProcessor();
    //        processor.StartProcessing();

    //        // Enqueue some commands
    //        processor.EnqueueCommand(new LoginCommand());
    //        processor.EnqueueCommand(new AddProductCommand());

    //        // Stop processing after some time or condition
    //        await Task.Delay(5000);
    //        processor.StopProcessing();
    //    }
    //}


//    在这个示例中：

//CommandProcessor 类负责启动命令处理循环，它使用 BlockingCollection<ICommand> 来存储命令。
//StartProcessing 方法启动了一个后台任务，该任务会不断地从队列中取出命令并执行它们。
//EnqueueCommand 方法允许外部代码将命令添加到队列中。
//StopProcessing 方法允许外部代码在适当的时候停止命令处理循环。
//这种设计允许你将命令的执行与命令的创建和分发解耦，并且可以轻松地扩展以处理更多的命令。此外，使用 CancellationToken 可以优雅地停止命令处理循环。