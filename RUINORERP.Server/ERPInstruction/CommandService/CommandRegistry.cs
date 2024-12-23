using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TransInstruction.CommandService
{

    /// <summary>
    /// 如果命令很多，可以使用一个注册表来管理命令，这样可以通过命令名称或其他标识符来查找和执行命令
    /// </summary>
    public class CommandRegistry
    {

        public CommandRegistry()
        {
            _handlers = new Dictionary<Type, ICommandHandler>();
        }

        private Dictionary<Type, ICommandHandler> _handlers = new Dictionary<Type, ICommandHandler>();

        public void RegisterCommandHandler(ICommandHandler handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            var supportedCommandTypes = handler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();
            var registeredType = _handlers.Keys.FirstOrDefault(x => supportedCommandTypes.Contains(x));
            if (registeredType != null)
            {
                var commands = String.Join(", ", supportedCommandTypes.Select(x => x.FullName));
                var registeredHandler = _handlers[registeredType];
                var message = $"The command(s) ('{commands}') handled by the received handler ('{handler}') already has a registered handler ('{registeredHandler}').";
                throw new ArgumentException(message);
            }
            foreach (var commandType in supportedCommandTypes)
            {
                _handlers.Add(commandType, handler);
            }
        }

        public IEnumerable<ICommandHandler> GetAllCommandHandlers()
        {
            return _handlers.Values;
        }

        private Dictionary<string, IServerCommand> commands = new Dictionary<string, IServerCommand>();

        public void RegisterCommand(string name, IServerCommand command)
        {
            commands[name] = command;
        }

        public void ExecuteCommand(string name)
        {
            if (commands.TryGetValue(name, out IServerCommand command))
            {
                command.Execute();
            }
        }
    }
}
