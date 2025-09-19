using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.CommandService
{
    /// <summary>
    /// CommandPublisher 负责将命令发布到消息队列，而 CommandSubscriber 订阅特定类型的命令并处理它们。这种方式允许服务之间通过消息队列进行通信，而不需要直接调用彼此的API
    /// 
    /// 他要安装一个RabbitMQ 服务端和客户端（模拟）使用是引用RabbitMQ.Client
    /// </summary>
    public class CommandPublisher
    {
        private readonly IMessageQueue _messageQueue;

        public CommandPublisher(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public void PublishCommand(ICommand command)
        {
            try
            {
                _messageQueue.Publish(command);
            }
            catch (Exception ex)
            {
                // 处理发布命令时的异常
                Console.WriteLine($"Error publishing command: {ex.Message}");
            }
        }
    }

    public class CommandSubscriber
    {
        private readonly ICommandHandler _commandHandler;

        public CommandSubscriber(ICommandHandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public void Subscribe()
        {
            _messageQueue.Subscribe<ICommand>(_commandHandler.HandleCommand);
        }
    }
}
