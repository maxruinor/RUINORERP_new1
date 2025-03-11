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
using TransInstruction.DataModel;


namespace RUINORERP.UI.ClientCmdService
{


    /// <summary>
    /// 线程安全的单例模式
    /// </summary>
    [NoWantIOC]
    public class ClientEventManager : IDisposable, IExcludeFromRegistration
    {

        private static readonly Lazy<ClientEventManager> _instance = new Lazy<ClientEventManager>(() => new ClientEventManager(), LazyThreadSafetyMode.ExecutionAndPublication);

        private ConcurrentDictionary<Guid, ServerLockCommandHandler> _commandHandlers = new ConcurrentDictionary<Guid, ServerLockCommandHandler>();

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

        public ConcurrentDictionary<Guid, ServerLockCommandHandler> CommandHandlers
        {
            get { return _commandHandlers; }
        }

        //public void AddCommandHandler(Guid command, ServerLockCommandHandler handler)
        //{
        //    if (true)
        //    {

        //    }
        //    _commandHandlers.AddOrUpdate(command, handler, (key, existingHandler) => existingHandler + handler);
        //}

        public void AddCommandHandler(Guid PacketId, ServerLockCommandHandler handler)
        {
           _commandHandlers.AddOrUpdate(
               PacketId,
               handler,
               (key, existingHandler) => Delegate.Combine(existingHandler, handler) as ServerLockCommandHandler
           );
           // _commandHandlers.AddOrUpdate(command, handler, (key, existingHandler) => existingHandler + handler);
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


        public void RemoveCommandHandler(Guid PacketId, ServerLockCommandHandler handler)
        {
            if (_commandHandlers.TryGetValue(PacketId, out ServerLockCommandHandler existingHandler))
            {
                // 使用 Delegate.Remove 方法安全移除委托
                ServerLockCommandHandler newHandler = (ServerLockCommandHandler)Delegate.Remove(existingHandler, handler);

                if (newHandler == null)
                {
                    // 如果没有剩余处理程序，完全移除该命令的处理程序
                    _commandHandlers.TryRemove(PacketId, out _);
                }
                else
                {
                    // 如果还有剩余处理程序，更新处理程序
                    _commandHandlers.TryUpdate(PacketId, newHandler, existingHandler);
                }
            }
        }


        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="command"></param>
        /// <param name="data"></param>
        //public void RaiseCommandEvent(ServerCmdEnum command, ServerLockCommandEventArgs args)
        //{
        //    if (_commandHandlers.TryGetValue(command, out ServerLockCommandHandler handler))
        //    {
        //        handler(null, args);
        //    }
        //}


        public void RaiseCommandEvent(Guid PacketId, ServerLockCommandEventArgs args)
        {
            if (_commandHandlers.TryGetValue(PacketId, out ServerLockCommandHandler handler))
            {
                // 使用 ClientEventManager 实例作为 sender
                handler(this, args);  // this 指代当前的 ClientEventManager 实例
            }
        }
        public void Dispose()
        {
            // 清理资源
            _commandHandlers.Clear();
        }
    }


}
