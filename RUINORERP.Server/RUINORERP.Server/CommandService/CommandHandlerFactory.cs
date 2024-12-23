using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.CommandService;

namespace RUINORERP.Server.CommandService
{
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        //实现 ICommandHandlerFactory 接口，使用反射来动态创建和注册命令处理器。
        public ICommandHandler CreateHandler(Type commandType)
        {
            //var handlerType = Assembly.GetExportedTypes()
            //                    .FirstOrDefault(t => t.GetInterfaces().Contains(typeof(ICommandHandler)) && t.GetCustomAttribute<CommandHandlerAttribute>() != null);
            //if (handlerType != null)
            //    return (ICommandHandler)Activator.CreateInstance(handlerType);
            //throw new InvalidOperationException($"No command handler found for type {commandType.FullName}.");

            switch (commandType.Name)
            {
                case nameof(LoginCommandHandler): return new LoginCommandHandler();
                case nameof(AddProductCommandHandler): return new AddProductCommandHandler();
                // ...其他命令处理器
                default: throw new InvalidOperationException("没有为命令类型注册处理程序。");
            }
        }
    }
}
