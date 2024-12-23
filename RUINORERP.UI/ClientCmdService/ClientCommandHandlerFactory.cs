using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.CommandService;

namespace RUINORERP.UI.ClientCmdService
{
    public class ClientCommandHandlerFactory : ICommandHandlerFactory
    {
        //public IClientCommand CreateHandler(Type commandType)
        //{
            
        //}

        ICommandHandler ICommandHandlerFactory.CreateHandler(Type commandType)
        {
            // 使用反射或其他方法创建命令处理器实例
            throw new NotImplementedException();
        }
    }
}
