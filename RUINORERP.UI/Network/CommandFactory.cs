using RUINORERP.PacketSpec.Commands;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 命令工厂类
    /// 负责创建和管理命令实例
    /// </summary>
    public class CommandFactory : ICommandFactory
    {
        private readonly Dictionary<CommandId, Type> _commandTypeMap;
        private readonly Dictionary<string, Type> _commandNameMap;

        /// <summary>
        /// 构造函数
        /// 初始化命令工厂并注册命令类型
        /// </summary>
        public CommandFactory()
        {
            _commandTypeMap = new Dictionary<CommandId, Type>();
            _commandNameMap = new Dictionary<string, Type>();

            // 注册内置命令
            RegisterCommandTypes();
        }

        /// <summary>
        /// 创建命令实例
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        /// <param name="args">构造函数参数</param>
        /// <returns>命令实例</returns>
        public T CreateCommand<T>(params object[] args) where T : ICommand
        {
            return (T)CreateCommand(typeof(T), args);
        }

        /// <summary>
        /// 根据命令类型创建命令实例
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="args">构造函数参数</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommand(Type commandType, params object[] args)
        {
            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new ArgumentException("命令类型必须实现ICommand接口", nameof(commandType));
            }

            try
            {
                return (ICommand)Activator.CreateInstance(commandType, args);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"创建命令实例失败: {commandType.Name}", ex);
            }
        }

        /// <summary>
        /// 根据命令ID创建命令实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="args">构造函数参数</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommand(CommandId commandId, params object[] args)
        {
            if (!_commandTypeMap.TryGetValue(commandId, out var commandType))
            {
                throw new ArgumentException($"未注册的命令ID: {commandId}", nameof(commandId));
            }

            return CreateCommand(commandType, args);
        }

        /// <summary>
        /// 根据命令名称创建命令实例
        /// </summary>
        /// <param name="commandName">命令名称</param>
        /// <param name="args">构造函数参数</param>
        /// <returns>命令实例</returns>
        public ICommand CreateCommand(string commandName, params object[] args)
        {
            if (!_commandNameMap.TryGetValue(commandName, out var commandType))
            {
                throw new ArgumentException($"未注册的命令名称: {commandName}", nameof(commandName));
            }

            return CreateCommand(commandType, args);
        }

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <param name="commandType">命令类型</param>
        public void RegisterCommand(Type commandType)
        {
            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new ArgumentException("命令类型必须实现ICommand接口", nameof(commandType));
            }

            try
            {
                // 创建临时实例来获取CommandIdentifier
                var tempInstance = (ICommand)Activator.CreateInstance(commandType, true);
                var commandId = tempInstance.CommandIdentifier;

                // 获取命令名称（从类型名称中提取）
                var commandName = GetCommandNameFromType(commandType);

                // 注册命令类型
                _commandTypeMap[commandId] = commandType;
                _commandNameMap[commandName] = commandType;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"注册命令类型失败: {commandType.Name}", ex);
            }
        }

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <typeparam name="T">命令类型</typeparam>
        public void RegisterCommand<T>() where T : ICommand
        {
            RegisterCommand(typeof(T));
        }

        /// <summary>
        /// 注册多个命令类型
        /// </summary>
        /// <param name="commandTypes">命令类型集合</param>
        public void RegisterCommands(IEnumerable<Type> commandTypes)
        {
            foreach (var commandType in commandTypes)
            {
                RegisterCommand(commandType);
            }
        }

        /// <summary>
        /// 从命令类型名称中提取命令名称
        /// 例如：GetUserDataCommand -> GetUserData
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>命令名称</returns>
        private string GetCommandNameFromType(Type commandType)
        {
            string typeName = commandType.Name;
            const string commandSuffix = "Command";

            if (typeName.EndsWith(commandSuffix))
            {
                return typeName.Substring(0, typeName.Length - commandSuffix.Length);
            }

            return typeName;
        }

        /// <summary>
        /// 注册内置命令
        /// 扫描程序集查找并注册命令类型
        /// </summary>
        private void RegisterCommandTypes()
        {
            // 这里可以实现扫描程序集查找命令类型的逻辑
            // 为了简化，我们可以在应用启动时手动注册命令
            // 例如：RegisterCommand<GetUserDataCommand>();
        }
    }
}