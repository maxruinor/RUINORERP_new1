using Netron.GraphLib;
using SourceGrid.Cells.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.CommandService;

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
                    var handler = (IClientCommand)Activator.CreateInstance(type);
                    // Activator.CreateInstance(type) as IClientCommand;
                    handlers.Add(handler);
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
