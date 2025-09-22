using RUINORERP.PacketSpec.Commands;
using System;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 命令工厂接口
    /// 定义创建和管理命令的标准方法
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// 根据命令类型创建命令实例
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        /// <returns>命令实例</returns>
        TCommand CreateCommand<TCommand>() where TCommand : ICommand, new();

        /// <summary>
        /// 根据命令ID创建命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令实例</returns>
        ICommand CreateCommand(CommandId commandId);

        /// <summary>
        /// 根据命令名称创建命令实例
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <returns>命令实例</returns>
        ICommand CreateCommand(string commandName);

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <typeparam name="TCommand">命令类型</typeparam>
        void RegisterCommand<TCommand>() where TCommand : ICommand, new();

        /// <summary>
        /// 注册命令类型与命令ID的映射
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="commandType">命令类型</param>
        void RegisterCommand(CommandId commandId, Type commandType);
    }
}