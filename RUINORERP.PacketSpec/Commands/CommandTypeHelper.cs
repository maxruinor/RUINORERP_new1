using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令类型辅助类 - 提供命令类型的注册、查找和创建功能
    /// </summary>
    public class CommandTypeHelper
    {
        private readonly Dictionary<uint, Type> _commandTypes;
        private readonly object _lock = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandTypeHelper()
        {
            _commandTypes = new Dictionary<uint, Type>();
        }

        /// <summary>
        /// 注册命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <param name="commandType">命令类型</param>
        public void RegisterCommandType(uint commandCode, Type commandType)
        {
            if (commandType == null)
                throw new ArgumentNullException(nameof(commandType));

            if (!typeof(ICommand).IsAssignableFrom(commandType))
                throw new ArgumentException("命令类型必须实现ICommand接口", nameof(commandType));

            lock (_lock)
            {
                _commandTypes[commandCode] = commandType;
            }
        }

        /// <summary>
        /// 获取命令类型
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandType(uint commandCode)
        {
            lock (_lock)
            {
                _commandTypes.TryGetValue(commandCode, out Type commandType);
                return commandType;
            }
        }

        /// <summary>
        /// 创建命令实例
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令实例，如果找不到类型或创建失败则返回null</returns>
        public ICommand CreateCommand(uint commandCode)
        {
            try
            {
                var commandType = GetCommandType(commandCode);
                if (commandType == null)
                    return null;

                return (ICommand)Activator.CreateInstance(commandType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 清理注册的命令类型
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _commandTypes.Clear();
            }
        }

        /// <summary>
        /// 获取所有注册的命令类型
        /// </summary>
        /// <returns>命令代码和类型的映射</returns>
        public Dictionary<uint, Type> GetAllCommandTypes()
        {
            lock (_lock)
            {
                return new Dictionary<uint, Type>(_commandTypes);
            }
        }

        /// <summary>
        /// 获取所有注册的命令类型（只读字典版本）
        /// 为兼容客户端代码而提供
        /// </summary>
        /// <returns>命令代码和类型的只读映射</returns>
        public IReadOnlyDictionary<uint, Type> GetRegisteredCommandTypes()
        {
            lock (_lock)
            {
                return new Dictionary<uint, Type>(_commandTypes);
            }
        }
    }
}