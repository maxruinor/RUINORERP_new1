using Krypton.Toolkit;
using RUINORERP.Common.Extensions;
using SourceGrid.Cells.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransInstruction.CommandService;

namespace RUINORERP.UI.ClientCmdService
{
    public class ClientCommandDispatcher
    {
        //public void RegisterCommandHandler(IClientCommand handler)
        //{
        //    if (handler == null) throw new ArgumentNullException(nameof(handler));
        //    var supportedCommandTypes = handler.GetType()
        //        .GetInterfaces()
        //        .Where(iface => iface.BaseType == typeof(IClientCommand))
        //        .Select(iface => iface.GetGenericArguments()[0])
        //        .ToList();
        //    var registeredType = _handlers.Keys.FirstOrDefault(x => supportedCommandTypes.Contains(x));
        //    if (registeredType != null)
        //    {
        //        var commands = String.Join(", ", supportedCommandTypes.Select(x => x.FullName));
        //        var registeredHandler = _handlers[registeredType];
        //        var message = $"The command(s) ('{commands}') handled by the received handler ('{handler}') already has a registered handler ('{registeredHandler}').";
        //        throw new ArgumentException(message);
        //    }
        //    foreach (var commandType in supportedCommandTypes)
        //    {
        //        _handlers.Add(commandType, handler);
        //    }
        //}


        private readonly Dictionary<Type, IClientCommand> _handlers;

        public ClientCommandDispatcher(IEnumerable<IClientCommand> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.GetType(), h => h);
        }

        /// <summary>
        /// 派遣处理器来执行
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public async void DispatchAsync(IClientCommand command, CancellationToken cancellationToken , object parameters = null)
        {
            if (_handlers.TryGetValue(command.GetType(), out var handler))
            {
                #region 
                // 属性复制
                if (command != null)
                {
                    foreach (var prop in command.GetType().GetProperties())
                    {
                        var info = command.GetType().GetProperty(prop.Name);
                        if (info != null && info.CanWrite)
                        {
                            var value = command.GetPropertyValue(prop.Name);
                            handler.SetPropertyValue(prop.Name, value);
                        }
                    }
                }

                #endregion
                // handler.ExecuteAsync(cancellationToken, parameters).GetAwaiter().GetResult();
                await handler.ExecuteAsync(cancellationToken, parameters);
            }
            else
            {
                throw new InvalidOperationException("找不到该命令的处理程序,请联系管理员。");
            }
        }

        //public void Dispatch<T>(IClientCommand command, object parameters) where T : class, T : IClientCommand
        //{
        //    if (_handlers.TryGetValue(typeof(T), out var handler))
        //    {
        //        var typedHandler = handler as T;
        //        typedHandler.ExecuteAsync(CancellationToken.None, parameters).GetAwaiter().GetResult();
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("找不到该命令的处理程序,请联系管理员。");
        //    }
        //}
    }
}
