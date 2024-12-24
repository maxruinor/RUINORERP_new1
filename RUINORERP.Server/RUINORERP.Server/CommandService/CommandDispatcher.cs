using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TransInstruction.DataModel;
using System.Reflection;
using System.Runtime.InteropServices;
using TransInstruction.CommandService;

namespace RUINORERP.Server.CommandService
{
    /// <summary>
    /// 指令调度器，用于根据接收到的指令调用相应的处理器
    /// </summary>
    public class CommandDispatcher
    {
        //handlers 是一个实现了 ICommandHandler 接口的实例集合，这个集合作为 CommandDispatcher 构造函数的参数传入。
        private readonly Dictionary<Type, ICommandHandler> _handlers;

        //public CommandDispatcher()
        //{
        //    _handlers = new Dictionary<Type, ICommandHandler>();
        //}
        private readonly ICommandHandlerFactory _factory;
        public CommandDispatcher(ICommandHandlerFactory factory)
        {
            _factory = factory;
            var handlers = new List<ICommandHandler>();
            Type[] filter = new Type[] { typeof(ICommandHandler) };
            //查找实现接口ICommandHandler的类。
            //foreach (var type in Assembly.LoadFrom("TransInstruction.dll").GetTypes().Where(t => t.GetInterfaces().Any(i => filter.Contains(i))))
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Any(i => filter.Contains(i))))
            {
                if (Attribute.IsDefined(type, typeof(CommandHandlerAttribute)))
                {
                    var handler = factory.CreateHandler(type);
                    handlers.Add(handler);
                }
            }
            _handlers = handlers.ToDictionary(h => h.GetType(), h => h);
            // 注册命令处理器
            //foreach (var handler in factory.GetAllHandlers())
            //{
            //    _handlers[handler.GetType()] = handler;
            //}
        }



        //// 依赖注入容器将自动填充_handlers 集合
        //public CommandDispatcher(IEnumerable<ICommandHandler> handlers)
        //{
        //    _handlers = handlers;
        //}

        //public CommandDispatcher(IEnumerable<IEventHandler<LoginEvent>> loginEventHandlers)
        //{
        //    _loginEventHandlers = loginEventHandlers;
        //}
        //public CommandDispatcher(IEnumerable<ICommand> commands, IEnumerable<IEventHandler<LoginEvent>> loginEventHandlers)
        //{
        //    _commands = commands;
        //    _loginEventHandlers = loginEventHandlers;
        //}

        //public void RegisterHandler<TCommand>(ICommandHandler handler) where TCommand : ICommand
        //{
        //    _handlers.Add(typeof(TCommand), handler);
        //}

        //public CommandDispatcher(IEnumerable<ICommandHandler> handlers)
        //{
        //    //这行代码的作用是将 handlers 集合转换为一个 Dictionary<Type, ICommandHandler> 字典
        //    // h.GetType() 获取处理器的类型，这通常是一个接口类型，如 ICommandHandler<LoginCommand>
        //    //而 h => h 作为值的生成器，它直接使用处理器实例作为字典的值。
        //    _handlers = handlers.ToDictionary(h => h.GetType(), h => h);
        //}
        //public void Dispatch(ICommand command)
        //{
        //    // 根据命令类型分发到具体的处理器
        //    foreach (var handler in _handlers)
        //    {
        //        if (handler.CanHandle(command))
        //        {
        //            handler.Handle(command);
        //            return;
        //        }
        //    }
        //    throw new InvalidOperationException("No handler found for the command.");
        //}

        public void Dispatch(IServerCommand command)
        {
            // 根据命令类型分发到具体的处理器
            if (_handlers.TryGetValue(command.GetType(), out var handler))
            {
                handler.HandleCommandAsync(command, CancellationToken.None).GetAwaiter().GetResult();
            }
            else
            {
                throw new InvalidOperationException("No handler found for the command.");
            }
        }



        //private readonly CommandProcessor _processor;

        //public CommandDispatcher(CommandProcessor processor)
        //{
        //    _processor = processor;
        //}

        //public void Dispatch(ICommand command)
        //{
        //    commandQueue.Add(command);
        //}
        public async Task DispatchAsync(IServerCommand command, CancellationToken cancellationToken)
        {
            foreach (var handler in _handlers)
            {
                if (handler.Value.CanHandle(command))
                {
                    //handler.Handle(command);
                    await handler.Value.HandleCommandAsync(command, cancellationToken);
                    return;
                }
            }

            //if (_handlers.TryGetValue(command.GetType(), out var handler))
            //{
            //    await handler.HandleCommandAsync(command, cancellationToken);
            //}
            //else
            //{
            //    throw new InvalidOperationException("No handler found for command.");
            //}
        }




        private readonly IEnumerable<IServerCommand> _commands;


        //在你的命令调度器中，你可以维护一个 _loginEventHandlers 集合，用于存储所有登录事件的处理器。当命令执行时，你可以触发这些事件
        private readonly IEnumerable<IEventHandler<LoginEvent>> _loginEventHandlers;

        public void Dispatch<TEvent>(TEvent eventData) where TEvent : class
        {
            // 这里可以添加逻辑来决定是否触发登录事件
            if (eventData is LoginEvent)
            {
                foreach (var handler in _loginEventHandlers)
                {
                    handler.Handle(eventData as LoginEvent);
                }
            }
        }
        //public void Dispatch<TEvent>(TEvent eventData) where TEvent : class
        //{
        //    foreach (var command in _commands.OfType<TEvent>())
        //    {
        //        command.Execute();
        //    }

        //    foreach (var handler in _loginEventHandlers)
        //    {
        //        handler.Handle(eventData);
        //    }
        //}

        //使用
        //var commandDispatcher = serviceProvider.GetRequiredService<CommandDispatcher>();
        //var loginEvent = new LoginEvent(); // Assume LoginEvent is a class representing a login event

        //commandDispatcher.Dispatch(loginEvent);


    }


}
/*
 public void SendCommand(Command command)
{
    // 假设你有一个方法将对象转换为字节数组
    byte[] commandBytes = ObjectToByteArray(command);
    AddSendData((byte)command.GetType().GetHashCode(), commandBytes);
}

public static byte[] ObjectToByteArray(object obj)
{
    using (var ms = new MemoryStream())
    {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, obj);
        return ms.ToArray();
    }
}
 

var dispatcher = new CommandDispatcher(new ICommandHandler[]
{
    new LoginCommandHandler(),
    new AddProductCommandHandler()
});

// 登录
var loginCommand = new LoginCommand(session);
await dispatcher.DispatchAsync(loginCommand, CancellationToken.None);

// 添加产品
var addProductCommand = new AddProductCommand("New Product", 9.99m);
await dispatcher.DispatchAsync(addProductCommand, CancellationToken.None);




在客户端代码中，使用 CommandDispatcher 来发送命令。

csharp
var dispatcher = new CommandDispatcher(new ICommandHandler[]
{
    new LoginCommandHandler(),
    new AddProductCommandHandler()
});

// 登录
var loginCommand = new LoginCommand(session);
await dispatcher.DispatchAsync(loginCommand, CancellationToken.None);

// 添加产品
var addProductCommand = new AddProductCommand("New Product", 9.99m);
await dispatcher.DispatchAsync(addProductCommand, CancellationToken.None);

在服务器端，接收数据并反序列化为指令对象，然后调度：

csharp
public void ProcessReceivedData(byte[] data)
{
    using (var ms = new MemoryStream(data))
    {
        var formatter = new BinaryFormatter();
        var command = (Command)formatter.Deserialize(ms);

        var dispatcher = new CommandDispatcher(new LoginCommandHandler(), new AddProductCommandHandler());
        dispatcher.DispatchCommand(command);
    }
}
7. 主程序


 */