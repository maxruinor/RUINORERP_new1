﻿using System;
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
            //.FirstOrDefault(t => t.GetInterfaces().Contains(typeof(ICommandHandler)) && t.GetCustomAttribute<CommandHandlerAttribute>() != null);
            return (ICommandHandler)Activator.CreateInstance(commandType);
        }
    }
}
