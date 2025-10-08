using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
// 添加System.Linq.Expressions命名空间引用
using System.Linq.Expressions;
// 添加System.Collections.Concurrent命名空间引用
using System.Collections.Concurrent;
using System.Xml.Serialization;
using RUINORERP.PacketSpec.Models.Core;
using Microsoft.Extensions.Logging;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令类型辅助类 - 提供命令类型的注册、查找和创建功能
    /// </summary>
    public class CommandTypeHelper
    {
        private readonly Dictionary<uint, Type> _commandTypes;
        private readonly Dictionary<CommandId, Type> _payloadMap;
        private readonly object _lock = new object();
        // 新增命令构造函数缓存
        private static readonly ConcurrentDictionary<uint, Func<ICommand>> _ctorCache = new();

        public CommandTypeHelper()
        {
            _commandTypes = new Dictionary<uint, Type>();
            _payloadMap = new Dictionary<CommandId, Type>();
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
        /// 获取命令构造函数
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>命令构造函数，如果找不到类型则返回null</returns>
        public Func<ICommand> GetCommandCtor(uint commandCode)
        {
            return _ctorCache.GetOrAdd(commandCode, code =>
            {
                var t = GetCommandType(code);
                return Expression.Lambda<Func<ICommand>>(
                           Expression.New(t.GetConstructor(Type.EmptyTypes)))
                       .Compile();
            });
        }

        /// <summary>
        /// 根据类型名称获取命令类型
        /// </summary>
        /// <param name="typeName">类型名称（完整名称或短名称）</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            lock (_lock)
            {
                // 首先尝试完整名称匹配
                foreach (var kvp in _commandTypes)
                {
                    var registeredType = kvp.Value;
                    if (registeredType.FullName == typeName || registeredType.Name == typeName)
                    {
                        return registeredType;
                    }
                }

                // 尝试从当前应用程序域中查找类型
                try
                {
                    var foundType = Type.GetType(typeName);
                    if (foundType != null && typeof(ICommand).IsAssignableFrom(foundType))
                    {
                        return foundType;
                    }
                }
                catch
                {
                    // 忽略类型查找异常
                }

                return null;
            }
        }

        /// <summary>
        /// 获取所有已注册的命令类型
        /// </summary>
        /// <returns>命令类型字典（命令代码 -> 类型）</returns>
        public IReadOnlyDictionary<uint, Type> GetAllCommandTypes()
        {
            lock (_lock)
            {
                return new Dictionary<uint, Type>(_commandTypes);
            }
        }

        /// <summary>
        /// 注册有效载荷类型
        /// </summary>
        /// <typeparam name="TPayload">有效载荷类型</typeparam>
        /// <param name="id">命令ID</param>
        public void RegisterPayloadType<TPayload>(CommandId id)
        {
            lock (_lock)
            {
                _payloadMap[id] = typeof(TPayload);
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
                _payloadMap.Clear();
            }
        }

        /// <summary>
        /// 获取有效载荷类型
        /// </summary>
        /// <param name="id">命令ID</param>
        /// <returns>有效载荷类型，如果找不到则返回null</returns>
        public Type GetPayloadType(CommandId id)
        {
            lock (_lock)
            {
                _payloadMap.TryGetValue(id, out var payloadType);
                return payloadType;
            }
        }

        /// <summary>
        /// 根据请求类型获取命令ID
        /// </summary>
        /// <typeparam name="TReq">请求类型</typeparam>
        /// <returns>对应的命令ID</returns>
        public CommandId GetCommandId<TReq>()
        {
            lock (_lock)
            {
                foreach (var kvp in _payloadMap)
                {
                    if (kvp.Value == typeof(TReq))
                    {
                        return kvp.Key;
                    }
                }

                throw new ArgumentException($"No command ID registered for type {typeof(TReq).FullName}");
            }
        }

 
    }
}
