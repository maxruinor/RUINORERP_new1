using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using SuperSocket.Command;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Interfaces;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Enums.Exception;
using Newtonsoft.Json;
using SuperSocket.Server.Abstractions.Session;
using System;
using RUINORERP.PacketSpec.Enums.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using ICommand = RUINORERP.PacketSpec.Commands.ICommand;
using CommandAttribute = RUINORERP.PacketSpec.Commands.CommandAttribute;
using MessagePack.Formatters;
using System.Diagnostics;

namespace RUINORERP.Server.Network.Commands.SuperSocket
{
    /// <summary>
    /// SuperSocket命令适配器
    /// 将SuperSocket的命令调用转换为现有的命令处理系统
    /// </summary>
    /// <typeparam name="TAppSession">SuperSocket会话类型</typeparam>
    public class SuperSocketCommandAdapter<TAppSession> : IAsyncCommand<TAppSession, PacketModel>
        where TAppSession : IAppSession
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ILogger _logger;
        private readonly Dictionary<uint, Type> _commandTypeMap;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="logger">日志记录器</param>
        public SuperSocketCommandAdapter(CommandDispatcher commandDispatcher, ILogger logger = null)
        {
            _commandDispatcher = commandDispatcher;
            _logger = logger;
            _commandTypeMap = new Dictionary<uint, Type>();
            InitializeCommandMap();
        }

        /// <summary>
        /// 初始化命令类型映射
        /// 扫描程序集中所有实现ICommand接口的命令类型，并通过CommandAttribute注册
        /// </summary>
        protected virtual void InitializeCommandMap()
        {
            try
            {
                // 动态扫描并注册所有实现ICommand接口的命令类型
                var assembly = Assembly.GetAssembly(typeof(ICommand));
                if (assembly != null)
                {
                    var commandTypes = assembly.GetTypes()
                        .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                        .ToList();

                    foreach (var commandType in commandTypes)
                    {
                        // 尝试获取命令特性
                        var commandAttribute = commandType.GetCustomAttribute<CommandAttribute>();
                        if (commandAttribute != null && commandAttribute.Id > 0)
                        {
                            _commandTypeMap[commandAttribute.Id] = commandType;
                            _logger?.LogDebug("注册命令类型映射: CommandId={CommandId}, Type={TypeName}, Name={CommandName}",
                                commandAttribute.Id, commandType.FullName, commandAttribute.Name);
                        }
                    }
                }

                _logger?.LogInformation("命令类型映射初始化完成，共注册{Count}个命令类型", _commandTypeMap.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化命令类型映射时出错");
                // 即使出错也要继续执行，使用默认的命令映射
            }
        }

        /// <summary>
        /// 执行命令
        /// 将SuperSocket的命令调用转换为现有的命令处理系统
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="package">数据包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果任务</returns>
        public async ValueTask ExecuteAsync(TAppSession session, PacketModel package, CancellationToken cancellationToken)
        {
            if (package == null)
            {
                _logger?.LogWarning("接收到空的数据包");
                return;
            }

            try
            {
                // 确保命令调度器已初始化
                if (!_commandDispatcher.IsInitialized)
                {
                    await _commandDispatcher.InitializeAsync(cancellationToken);
                }

                // 确保命令类型映射已初始化
                if (_commandTypeMap == null || _commandTypeMap.Count == 0)
                {
                    InitializeCommandMap();
                }

                SessionInfo sessionInfo = new SessionInfo();
                //这里先这样，先跑起来  todo by watson

                // 创建命令对象
                var command = CreateCommand(package, sessionInfo);
                if (command == null)
                {
                    _logger?.LogWarning("无法创建命令对象: CommandId={CommandId}", package.Command);
                    await SendErrorResponseAsync(session, package, ErrorCodes.CommandNotFound, cancellationToken);
                    return;
                }

                // 记录命令执行开始的日志
                _logger?.LogDebug("开始执行命令: CommandId={CommandId}, Type={TypeName}",
                    package.Command, command.GetType().FullName);

                // 通过现有的命令调度器处理命令
                var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
                await HandleCommandResultAsync(session, package, result, cancellationToken);

                // 记录命令执行完成的日志
                _logger?.LogDebug("命令执行完成: CommandId={CommandId}, Success={Success}",
                    package.Command, result?.IsSuccess ?? false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理SuperSocket命令时发生异常: CommandId={CommandId}", package.Command);
                // 发送错误响应给客户端
                await SendErrorResponseAsync(session, package, ErrorCodes.UnhandledException, cancellationToken);
            }
        }

        /// <summary>
        /// 创建命令对象
        /// 根据命令ID和数据包内容创建适当类型的命令对象
        /// </summary>
        /// <param name="package">数据包</param>
        /// <param name="sessionContext">会话上下文</param>
        /// <returns>创建的命令对象</returns>
        protected virtual ICommand CreateCommand(PacketModel package, SessionInfo sessionContext)
        {
            try
            {
                // 根据命令ID查找对应的命令类型
                if (_commandTypeMap.TryGetValue(package.Command, out var commandType))
                {
                    // 尝试使用构造函数创建命令实例
                    var constructor = GetSuitableConstructor(commandType);
                    if (constructor != null)
                    {
                        var parameters = PrepareConstructorParameters(constructor, package, sessionContext);
                        var command = Activator.CreateInstance(commandType, parameters) as ICommand;
                        _logger?.LogDebug("根据命令ID创建命令实例: CommandId={CommandId}, Type={TypeName}",
                            package.Command, commandType.FullName);
                        return command;
                    }
                }

                // 如果没有找到对应的命令类型或无法创建实例，使用默认的MessageCommand
                var defaultCommand = new MessageCommand(
                    package.Command,
                    package,
                    package.Body);
                _logger?.LogDebug("使用默认MessageCommand创建命令实例: CommandId={CommandId}", package.Command);
                return defaultCommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令对象时出错: CommandId={CommandId}", package.Command);
                // 如果创建失败，返回一个默认的命令对象
                return new MessageCommand(
                    package.Command,
                    package,
                    package.Body);
            }
        }

        /// <summary>
        /// 获取适合的构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>构造函数信息</returns>
        protected virtual ConstructorInfo GetSuitableConstructor(Type commandType)
        {
            // 查找包含CommandId、SessionInfo和Data参数的构造函数
            var constructors = commandType.GetConstructors();
            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == 3 &&
                    parameters[0].ParameterType == typeof(uint) &&
                    typeof(SessionInfo).IsAssignableFrom(parameters[1].ParameterType) &&
                    parameters[2].ParameterType == typeof(object))
                {
                    return constructor;
                }
            }

            // 如果没有找到理想的构造函数，返回第一个可用的构造函数
            return constructors.FirstOrDefault();
        }

        /// <summary>
        /// 准备构造函数参数
        /// </summary>
        /// <param name="constructor">构造函数信息</param>
        /// <param name="package">数据包</param>
        /// <param name="sessionContext">会话上下文</param>
        /// <returns>参数数组</returns>
        protected virtual object[] PrepareConstructorParameters(ConstructorInfo constructor, PacketModel package, SessionInfo sessionContext)
        {
            var parameters = constructor.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(uint) && i == 0)
                {
                    parameterValues[i] = package.Command;
                }
                else if (typeof(SessionInfo).IsAssignableFrom(parameters[i].ParameterType) && i == 1)
                {
                    parameterValues[i] = sessionContext as SessionInfo;
                }
                else if (parameters[i].ParameterType == typeof(object) && i == 2)
                {
                    parameterValues[i] = package.Body;
                }
                else if (parameters[i].ParameterType == typeof(string))
                {
                    parameterValues[i] = package.SessionId;
                }
                else
                {
                    // 对于其他类型的参数，尝试使用默认值或null
                    parameterValues[i] = parameters[i].HasDefaultValue ? parameters[i].DefaultValue : null;
                }
            }

            return parameterValues;
        }

        /// <summary>
        /// 处理命令执行结果
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="result">命令执行结果</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果任务</returns>
        protected virtual async ValueTask HandleCommandResultAsync(
            TAppSession session,
            PacketModel requestPackage,
            CommandResult result,
            CancellationToken cancellationToken)
        {
            if (result.IsSuccess)
            {
                // 命令执行成功，发送成功响应
                var responsePackage = CreateResponsePackage(requestPackage, result);
                await SendResponseAsync(session, responsePackage, cancellationToken);
            }
            else
            {
                // 命令执行失败，发送错误响应
                await SendErrorResponseAsync(
                    session,
                    requestPackage,
                    result.ErrorCode ?? ErrorCodes.UnknownError,
                    cancellationToken);
            }
        }

        /// <summary>
        /// 创建响应数据包
        /// </summary>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="result">命令执行结果</param>
        /// <returns>响应数据包</returns>
        protected virtual PacketModel CreateResponsePackage(PacketModel requestPackage, CommandResult result)
        {
            var response = new PacketModel
            {
                PacketId = GenerateResponseId(requestPackage.PacketId),
                Command = requestPackage.Command,
                Direction = requestPackage.Direction == PacketDirection.Request ? PacketDirection.Response : requestPackage.Direction,
                SessionId = requestPackage.SessionId,
                ClientId = requestPackage.ClientId,
                Status = PacketStatus.Completed
            };

            // 设置响应数据
            if (result.Data != null)
            {
                response.Extensions["Data"] = result.Data;
            }

            if (!string.IsNullOrEmpty(result.Message))
            {
                response.Extensions["Message"] = result.Message;
            }

            return response;
        }

        /// <summary>
        /// 发送响应
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="package">数据包</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果任务</returns>
        protected virtual async ValueTask SendResponseAsync(TAppSession session, PacketModel package, CancellationToken cancellationToken)
        {
            // 这里需要实现数据包的序列化和发送逻辑
            // 实际实现应根据项目的序列化方式进行调整
            var serializedData = SerializePacket(package);
            await session.SendAsync(serializedData, cancellationToken);
        }

        /// <summary>
        /// 发送错误响应
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果任务</returns>
        protected virtual async ValueTask SendErrorResponseAsync(
            TAppSession session,
            PacketModel requestPackage,
            string errorCode,
            CancellationToken cancellationToken)
        {
            var errorResponse = new PacketModel
            {
                PacketId = GenerateResponseId(requestPackage.PacketId),
                Command = requestPackage.Command,
                Direction = PacketDirection.Response,
                SessionId = requestPackage.SessionId,
                ClientId = requestPackage.ClientId,
                Status = PacketStatus.Error,
                Extensions = new Dictionary<string, object>
                    {
                        { "ErrorCode", GetErrorCodeNumber(errorCode) },
                        { "ErrorMessage", GetErrorMessageByCode(errorCode) }
                    }
            };

            await SendResponseAsync(session, errorResponse, cancellationToken);
        }

        /// <summary>
        /// 根据错误代码获取对应的错误消息
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误消息</returns>
        protected virtual string GetErrorMessageByCode(string errorCode)
        {
            // 根据错误代码返回对应的错误消息
            // 这里可以实现一个更完整的错误消息映射
            switch (errorCode)
            {
                case ErrorCodes.CommandNotFound:
                    return "命令未找到";
                case ErrorCodes.UnhandledException:
                    return "处理命令时发生未预期的异常";
                case ErrorCodes.UnknownError:
                    return "发生未知错误";
                default:
                    return errorCode; // 默认返回错误代码本身
            }
        }
        
        /// <summary>
        /// 获取错误代码对应的数字值
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误代码数字值</returns>
        protected virtual int GetErrorCodeNumber(string errorCode)
        {
            // 将字符串错误代码映射到数字
            // 这里可以实现一个更完整的错误代码数字映射
            switch (errorCode)
            {
                case ErrorCodes.CommandNotFound:
                    return 404;
                case ErrorCodes.UnhandledException:
                    return 500;
                case ErrorCodes.UnknownError:
                    return 999;
                default:
                    return 1; // 默认错误代码
            }
        }

        /// <summary>
        /// 序列化数据包
        /// </summary>
        /// <param name="package">数据包</param>
        /// <returns>序列化后的字节数组</returns>
        protected virtual byte[] SerializePacket(PacketModel package)
        {
            // 实际实现应根据项目的序列化方式进行调整
            // 这里只是一个示例
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(package);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// 生成响应ID
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>响应ID</returns>
        protected virtual string GenerateResponseId(string requestId)
        {
            return $"RESP_{requestId}_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}