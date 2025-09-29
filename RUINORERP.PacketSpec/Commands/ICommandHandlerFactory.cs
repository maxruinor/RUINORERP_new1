using System;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器工厂接口
    /// </summary>
    public interface ICommandHandlerFactory
    {
        /// <summary>
        /// 创建命令处理器
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>处理器实例</returns>
        ICommandHandler CreateHandler(Type handlerType);

        /// <summary>
        /// 创建命令处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        /// <returns>处理器实例</returns>
        T CreateHandler<T>() where T : class, ICommandHandler;

        /// <summary>
        /// 注册处理器类型
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        void RegisterHandler(Type handlerType);

 
    }

}
