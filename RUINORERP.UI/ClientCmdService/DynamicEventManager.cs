using RUINORERP.Common.CustomAttribute;
using RUINORERP.Global;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Forms;
using TransInstruction;



namespace RUINORERP.UI.ClientCmdService
{

    /*
                 eventManager = new DynamicEventManager();

            // 动态添加事件处理程序
            eventManager.AddEventHandler((sender, args) =>
            {
                Console.WriteLine("Event 1 triggered.");
            });

            eventManager.AddEventHandler((sender, args) =>
            {
                Console.WriteLine("Event 2 triggered.");
            });

            // 触发事件
            eventManager.RaiseCommandEvent();
     */

    /// <summary>
    /// 为了在能在接收到服务器信息时根据不同指令或内容 触发不同的事件
    /// 设计一个这种事件集合来统一处理
    /// 
    //DynamicEventManager eventManager;
    //eventManager = new DynamicEventManager();
     // eventManager.AddCommandHandler(ServerCmdEnum.复合型锁单处理, cmd.HandleLockDocument);
    /// ConcurrentDictionary<ServerCmdEnum, ServerCommandHandler> 的生命周期应该由一个全局或长期存在的对象来管理，这样可以确保在不同的类中都能访问到同一个实例。通常，这种集合可以保存在一个静态类或单例模式的类中，以便在应用程序的任何地方都能方便地访问和使用
    /// </summary>
    public class DynamicEventManager
    {

        // 定义一个事件集合，用于存储不同指令的事件处理程序
        private ConcurrentDictionary<ServerCmdEnum, ServerCommandHandler> _commandHandlers = new ConcurrentDictionary<ServerCmdEnum, ServerCommandHandler>();

        // 添加事件处理程序
        public void AddCommandHandler(ServerCmdEnum command, ServerCommandHandler handler)
        {
            _commandHandlers.AddOrUpdate(command, handler, (key, existingHandler) => existingHandler + handler);
        }

        // 移除事件处理程序
        public void RemoveCommandHandler(ServerCmdEnum command, ServerCommandHandler handler)
        {
            if (_commandHandlers.TryGetValue(command, out ServerCommandHandler existingHandler))
            {
                ServerCommandHandler newHandler = existingHandler - handler;
                if (newHandler == null)
                {
                    _commandHandlers.TryRemove(command, out _);
                }
                else
                {
                    _commandHandlers.TryUpdate(command, newHandler, existingHandler);
                }
            }
        }

        // 触发事件
        public void RaiseCommandEvent(ServerCmdEnum command, byte[] data)
        {
            if (_commandHandlers.TryGetValue(command, out ServerCommandHandler handler))
            {
                handler(this, new ServerCommandEventArgs { CommandName = command.ToString(), Data = data });
            }
        }
    }

    /// <summary>
    /// 线程安全的单例模式
    /// </summary>
    [NoWantIOC]
    public class ClientEventManager : IDisposable, IExcludeFromRegistration
    {

        /*
            EventManager.Instance.AddCommandHandler(ServerCmdEnum.Login, OnLoginCommand);
        EventManager.Instance.AddCommandHandler(ServerCmdEnum.SendData, OnSendDataCommand);

        // 模拟事件触发
        EventManager.Instance.RaiseCommandEvent(ServerCmdEnum.Login, new byte[] { 0x01 });
        EventManager.Instance.RaiseCommandEvent(ServerCmdEnum.SendData, new byte[] { 0x02 });
    }

    private static void OnLoginCommand(object sender, ServerCommandEventArgs e)
    {
        Console.WriteLine($"Login command received: {e.CommandName}");
        // 处理登录逻辑
    }

    private static void OnSendDataCommand(object sender, ServerCommandEventArgs e)
    {
        Console.WriteLine($"SendData command received: {e.CommandName}");
        // 处理发送数据逻辑
    }
         */
        private static readonly Lazy<ClientEventManager> _instance = new Lazy<ClientEventManager>(() => new ClientEventManager(), LazyThreadSafetyMode.ExecutionAndPublication);

        private ConcurrentDictionary<ServerCmdEnum, ServerCommandHandler> _commandHandlers = new ConcurrentDictionary<ServerCmdEnum, ServerCommandHandler>();

        private ClientEventManager()
        {

        }

        public static ClientEventManager Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public ConcurrentDictionary<ServerCmdEnum, ServerCommandHandler> CommandHandlers
        {
            get { return _commandHandlers; }
        }

        public void AddCommandHandler(ServerCmdEnum command, ServerCommandHandler handler)
        {
            _commandHandlers.AddOrUpdate(command, handler, (key, existingHandler) => existingHandler + handler);
        }

        public void RemoveCommandHandler(ServerCmdEnum command, ServerCommandHandler handler)
        {
            if (_commandHandlers.TryGetValue(command, out ServerCommandHandler existingHandler))
            {
                ServerCommandHandler newHandler = existingHandler - handler;
                if (newHandler == null)
                {
                    _commandHandlers.TryRemove(command, out _);
                }
                else
                {
                    _commandHandlers.TryUpdate(command, newHandler, existingHandler);
                }
            }
        }

        public void RaiseCommandEvent(ServerCmdEnum command, byte[] data)
        {
            if (_commandHandlers.TryGetValue(command, out ServerCommandHandler handler))
            {
                var args = new ServerCommandEventArgs { CommandName = command.ToString(), Data = data };
                handler(null, args);
            }
        }

        public void Dispose()
        {
            // 清理资源
            _commandHandlers.Clear();
        }
    }



    public delegate void ServerCommandHandler(object sender, ServerCommandEventArgs e);
    public class ServerCommandEventArgs : EventArgs
    {
        public string CommandName { get; set; }
        public byte[] Data { get; set; }
    }
}
