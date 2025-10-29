/*****************************************************************************************
 * 【过时作废文件】OBSOLETE - DEPRECATED - DO NOT USE
 * 此文件已被废弃，不再维护和使用
 * 原因：ClientCmdService目录下的所有文件都已过时，实际已排除在项目外
 * 替代方案：请使用新的命令处理机制
 * 创建日期：系统自动标识
 *****************************************************************************************/

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

namespace RUINORERP.UI.ClientCmdService
{
    [Obsolete("此类已过时，不再使用")]
    public class ClientCommandDispatcher
    {
        

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
        public  void DispatchAsync(IClientCommand command, CancellationToken cancellationToken , object parameters = null)
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
                handler.ExecuteAsync(cancellationToken, parameters);
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
