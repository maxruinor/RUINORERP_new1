using RUINORERP.Common.CustomAttribute;
using RUINORERP.Global;
using RUINORERP.Model.TransModel;
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
        private ConcurrentDictionary<ServerCmdEnum, ServerLockCommandHandler> _commandHandlers = new ConcurrentDictionary<ServerCmdEnum, ServerLockCommandHandler>();

        // 添加事件处理程序
        public void AddCommandHandler(ServerCmdEnum command, ServerLockCommandHandler handler)
        {
            _commandHandlers.AddOrUpdate(command, handler, (key, existingHandler) => existingHandler + handler);
        }

        // 移除事件处理程序
        public void RemoveCommandHandler(ServerCmdEnum command, ServerLockCommandHandler handler)
        {
            if (_commandHandlers.TryGetValue(command, out ServerLockCommandHandler existingHandler))
            {
                ServerLockCommandHandler newHandler = existingHandler - handler;
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
            if (_commandHandlers.TryGetValue(command, out ServerLockCommandHandler handler))
            {
                handler(this, new ServerCommandEventArgs { Command = command, Data = data });
            }
        }
    }
 */



    /// <summary>
    /// 线程安全的单例模式
    /// </summary>
    [NoWantIOC]
    public class ClientEventManager : IDisposable, IExcludeFromRegistration
    {
       
        private static readonly Lazy<ClientEventManager> _instance = new Lazy<ClientEventManager>(() => new ClientEventManager(), LazyThreadSafetyMode.ExecutionAndPublication);

        private ConcurrentDictionary<ServerCmdEnum, ServerLockCommandHandler> _commandHandlers = new ConcurrentDictionary<ServerCmdEnum, ServerLockCommandHandler>();

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

        public ConcurrentDictionary<ServerCmdEnum, ServerLockCommandHandler> CommandHandlers
        {
            get { return _commandHandlers; }
        }

        //public void AddCommandHandler(ServerCmdEnum command, ServerLockCommandHandler handler)
        //{
        //    if (true)
        //    {

        //    }
        //    _commandHandlers.AddOrUpdate(command, handler, (key, existingHandler) => existingHandler + handler);
        //}

        public void AddCommandHandler(ServerCmdEnum command, ServerLockCommandHandler handler)
        {
            _commandHandlers.AddOrUpdate(
                command,
                handler,
                (key, existingHandler) => Delegate.Combine(existingHandler, handler) as ServerLockCommandHandler
            );
        }


        //public void RemoveCommandHandler(ServerCmdEnum command, ServerLockCommandHandler handler)
        //{
        //    if (_commandHandlers.TryGetValue(command, out ServerLockCommandHandler existingHandler))
        //    {
        //        ServerLockCommandHandler newHandler = existingHandler - handler;
        //        if (newHandler == null)
        //        {
        //            _commandHandlers.TryRemove(command, out _);
        //        }
        //        else
        //        {
        //            _commandHandlers.TryUpdate(command, newHandler, existingHandler);
        //        }
        //    }
        //}


        public void RemoveCommandHandler(ServerCmdEnum command, ServerLockCommandHandler handler)
        {
            if (_commandHandlers.TryGetValue(command, out ServerLockCommandHandler existingHandler))
            {
                // 使用 Delegate.Remove 方法安全移除委托
                ServerLockCommandHandler newHandler = (ServerLockCommandHandler)Delegate.Remove(existingHandler, handler);

                if (newHandler == null)
                {
                    // 如果没有剩余处理程序，完全移除该命令的处理程序
                    _commandHandlers.TryRemove(command, out _);
                }
                else
                {
                    // 如果还有剩余处理程序，更新处理程序
                    _commandHandlers.TryUpdate(command, newHandler, existingHandler);
                }
            }
        }


        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="command"></param>
        /// <param name="data"></param>
        public void RaiseCommandEvent(ServerCmdEnum command, LockCmd lockCmd, bool isSuccess, byte[] data)
        {
            if (_commandHandlers.TryGetValue(command, out ServerLockCommandHandler handler))
            {
                var args = new ServerLockCommandEventArgs { Command = command, Data = data };

                args.lockCmd = lockCmd;
                args.isSuccess = isSuccess;

                handler(null, args);
            }
        }

        public void Dispose()
        {
            // 清理资源
            _commandHandlers.Clear();
        }
    }


    public delegate void ServerLockCommandHandler(object sender, ServerLockCommandEventArgs e);
    public class ServerCommandEventArgs : EventArgs
    {
        public ServerCmdEnum Command { get; set; }
        public byte[] Data { get; set; }
    }

    /// <summary>
    /// 服务器返回的情况
    /// </summary>
    public class ServerLockCommandEventArgs : ServerCommandEventArgs
    {
        public long BillID { get; set; }
        public LockCmd lockCmd { get; set; }
        public bool isSuccess { get; set; }
    }

}
