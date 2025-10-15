/*****************************************************************************************
 * 【过时作废文件】OBSOLETE - DEPRECATED - DO NOT USE
 * 此文件已被废弃，不再维护和使用
 * 原因：ClientCmdService目录下的所有文件都已过时，实际已排除在项目外
 * 替代方案：请使用新的命令处理机制
 * 创建日期：系统自动标识
 *****************************************************************************************/

using Netron.GraphLib;
using SourceGrid.Cells.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.CommandService;
using TransInstruction.Enums;

namespace RUINORERP.UI.ClientCmdService
{
    public class ClientCommandRegistry
    {
        public ClientCommandRegistry()
        {
            _handlers = new Dictionary<Type, IClientCommand>();
        }

        private Dictionary<Type, IClientCommand> _handlers = new Dictionary<Type, IClientCommand>();

        public List<IClientCommand> AutoRegisterCommandHandler()
        {
            var handlers = new List<IClientCommand>();
            Type[] filter = new Type[] { typeof(IClientCommand) };
            //查找实现接口IClientCommand的类。
            //foreach (var type in Assembly.LoadFrom("TransInstruction.dll").GetTypes().Where(t => t.GetInterfaces().Any(i => filter.Contains(i))))
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterfaces().Any(i => filter.Contains(i))))
            {
                //if (Attribute.IsDefined(type, typeof(CommandHandlerAttribute)))
                //{
                if (type != null)
                {
                    var instance = CreateInstance(type);
                    if (instance != null)
                    {
                        handlers.Add(instance);
                    }
                    else
                    {
                        Console.WriteLine("Failed to create instance.");
                    }

                    //var handler = (IClientCommand)Activator.CreateInstance(type);
                    //// Activator.CreateInstance(type) as IClientCommand;
                    //handlers.Add(handler);

                }
                //}
            }

            return handlers;
            //_handlers = handlers.ToDictionary(h => h.GetType(), h => h);

            //foreach (var commandType in supportedCommandTypes)
            //{
            //    _handlers.Add(commandType, handler);
            //}
        }


        public static IClientCommand CreateInstance(Type type)
        {
            // 首先尝试获取无参数的构造函数
            ConstructorInfo defaultConstructor = type.GetConstructor(Type.EmptyTypes);
            if (defaultConstructor != null)
            {
                return Activator.CreateInstance(type) as IClientCommand;
            }
            else
            {
                // 获取所有的构造函数
                ConstructorInfo[] constructors = type.GetConstructors();
                foreach (ConstructorInfo constructor in constructors)
                {
                    ParameterInfo[] parameters = constructor.GetParameters();
                    object[] parameterValues = new object[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        // 这里可以根据参数的类型提供相应的值，以下是简单的示例，你可以根据具体情况修改
                        if (parameters[i].ParameterType == typeof(CmdOperation))
                        {
                            parameterValues[i] = 1; //设置为 1  send  ,暂时要处理到这里。后面的判断 根据不同情况来处理
                        }
                        else if (parameters[i].ParameterType == typeof(string))
                        {
                            parameterValues[i] = "Hello World"; // 假设这里是字符串参数，设置为 "Hello World"
                        }
                        else
                        {
                            // 对于其他类型，你可能需要更复杂的逻辑来创建相应的参数值
                            parameterValues[i] = null;
                        }
                    }
                    try
                    {
                        return constructor.Invoke(parameterValues) as IClientCommand;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to invoke constructor: {ex.Message}");
                    }
                }
            }
            return null;
        }


        public IEnumerable<IClientCommand> GetAllCommandHandlers()
        {
            return _handlers.Values;
        }

        private Dictionary<string, IClientCommand> commands = new Dictionary<string, IClientCommand>();

        public void RegisterCommand(string name, IClientCommand command)
        {
            commands[name] = command;
        }

        //public void ExecuteCommand(string name)
        //{
        //    if (commands.TryGetValue(name, out IClientCommand command))
        //    {
        //        command.ExecuteAsync();
        //    }
        //}
    }
}
