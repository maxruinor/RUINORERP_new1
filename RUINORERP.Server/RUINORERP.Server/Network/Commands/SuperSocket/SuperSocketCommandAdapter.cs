using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using global::RUINORERP.PacketSpec.Commands;
using global::RUINORERP.PacketSpec.Commands.Message;
using global::RUINORERP.PacketSpec.Models.Responses;
using global::RUINORERP.PacketSpec.Serialization;
using global::RUINORERP.Server.Network.Models;
using global::RUINORERP.Server.Network.Interfaces.Services;
using global::SuperSocket.Server.Abstractions.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.PacketSpec;
using RUINORERP.PacketSpec.Enums.Core;
using ICommand = RUINORERP.PacketSpec.Commands.ICommand;
using SuperSocket.Command;
using Azure;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.Server.Network.Commands.SuperSocket
{
    /// <summary>
    /// 统一的SuperSocket命令适配器
    /// 整合了原有的SimplifiedSuperSocketAdapter、SocketCommand和SuperSocketCommandAdapter的功能
    /// 
    /// 工作流程：
    /// 1. SuperSocket接收到来自客户端的数据包
    /// 2. SuperSocketCommandAdapter.ExecuteAsync方法被调用
    /// 3. 从CommandDispatcher获取已注册的命令类型映射
    /// 4. 根据数据包中的命令ID创建对应的命令实例
    /// 5. 通过CommandDispatcher.DispatchAsync方法分发命令给相应的处理器
    /// 6. 处理结果通过网络返回给客户端
    /// </summary>
    [Command(Key = "SuperSocketCommandAdapter")]
    public class SuperSocketCommandAdapter<TAppSession> : IAsyncCommand<TAppSession, ServerPackageInfo>
        where TAppSession : IAppSession
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ILogger<SuperSocketCommandAdapter> _logger;
        private readonly Dictionary<uint, Type> _commandTypeMap;
        private readonly ICommandFactory _commandFactory;
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();

        #region 错误代码字典化处理
        /// <summary>
        /// 错误代码映射字典
        /// </summary>
        private static readonly Dictionary<string, (int code, string message)> ErrorCodeMap = new Dictionary<string, (int code, string message)>
        {
            { ErrorCodes.CommandNotFound, (404, "命令未找到") },
            { ErrorCodes.UnhandledException, (500, "处理命令时发生未预期的异常") },
            { ErrorCodes.UnknownError, (999, "发生未知错误") },
            { "SessionNotFound", (401, "会话不存在或已过期") } // 添加会话不存在的错误码
        };

        /// <summary>
        /// 根据错误代码获取对应的错误消息和数字代码
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误代码数字值和消息</returns>
        protected virtual (int code, string message) GetErrorInfoByCode(string errorCode)
        {
            if (ErrorCodeMap.TryGetValue(errorCode, out var errorInfo))
            {
                return errorInfo;
            }

            // 默认返回错误代码本身
            return (1, errorCode);
        }

        /// <summary>
        /// 根据错误代码获取对应的错误消息
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误消息</returns>
        protected virtual string GetErrorMessageByCode(string errorCode)
        {
            return GetErrorInfoByCode(errorCode).message;
        }

        /// <summary>
        /// 获取错误代码对应的数字值
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <returns>错误代码数字值</returns>
        protected virtual int GetErrorCodeNumber(string errorCode)
        {
            return GetErrorInfoByCode(errorCode).code;
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="commandFactory">命令工厂</param>
        /// <param name="logger">日志记录器</param>
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            ICommandFactory commandFactory,
            ILogger<SuperSocketCommandAdapter> logger = null)
        {
            _commandDispatcher = commandDispatcher;
            _commandFactory = commandFactory;
            _logger = logger;
            _commandTypeMap = new Dictionary<uint, Type>();
            InitializeCommandMap();
        }

        /// <summary>
        /// 初始化命令类型映射
        /// 使用CommandDispatcher中已注册的命令类型
        /// </summary>
        protected virtual void InitializeCommandMap()
        {
            try
            {
                // 直接使用CommandDispatcher中已注册的命令类型，避免重复扫描
                var commandTypes = _commandDispatcher.GetAllCommandTypes();
                foreach (var kvp in commandTypes)
                {
                    _commandTypeMap[kvp.Key] = kvp.Value;
                    _logger?.LogDebug("注册命令类型映射: CommandId={CommandId}, Type={TypeName}",
                        kvp.Key, kvp.Value.FullName);
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
        public async ValueTask ExecuteAsync(TAppSession session, ServerPackageInfo package, CancellationToken cancellationToken)
        {
            if (package == null)
            {
                _logger?.LogWarning("接收到空的数据包");
                await SendErrorResponseAsync(session, package, ErrorCodes.NullCommand, cancellationToken);
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

                // 获取现有会话信息
                var sessionInfo = SessionService.GetSession(session.SessionID);
                if (sessionInfo == null)
                {
                    // 如果会话不存在，可能是连接已断开或会话已过期
                    await SendErrorResponseAsync(session, package, "SessionNotFound", cancellationToken);
                    return;
                }

                // 更新会话的最后活动时间
                sessionInfo.UpdateActivity(); // 使用专门的UpdateActivity方法更新活动时间
                SessionService.UpdateSession(sessionInfo);
                // 同时调用专门的UpdateSessionActivity方法确保活动时间被正确更新
                SessionService.UpdateSessionActivity(session.SessionID);

                // 创建命令对象
                var command = CreateCommand(package, sessionInfo);
                if (command == null)
                {
                    _logger?.LogWarning("无法创建命令对象: CommandId={CommandId}", package.Packet.Command);
                    await SendErrorResponseAsync(session, package, ErrorCodes.CommandNotFound, cancellationToken);
                    return;
                }

                // 记录命令执行开始的日志
                _logger?.LogDebug("开始执行命令: CommandId={CommandId}, Type={TypeName}",
                    package.Packet.Command, command.GetType().FullName);

                // 通过现有的命令调度器处理命令
                var result = await _commandDispatcher.DispatchAsync(command, cancellationToken);
                await HandleCommandResultAsync(session, package, result, cancellationToken);

                // 记录命令执行完成的日志
                _logger?.LogDebug("命令执行完成: CommandId={CommandId}, Success={Success}",
                    package.Packet.Command, result?.IsSuccess ?? false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理SuperSocket命令时发生异常: CommandId={CommandId}", package.Packet.Command);
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
        protected virtual ICommand CreateCommand(ServerPackageInfo package, SessionInfo sessionContext)
        {
            try
            {
                // 优先使用命令工厂创建命令
                if (_commandFactory != null)
                {
                    var command = _commandFactory.CreateCommand(package.Packet as PacketModel);
                    if (command != null)
                    {
                        // 设置命令的会话ID和数据包模型
                        command.SessionID = sessionContext.SessionID;
                        command.Packet = package.Packet;

                        // 尝试从数据包中获取业务数据 这里晚再再看
                        // businessDataCommand.BusinessData = packetModel.GetJsonData<object>();


                        return command;
                    }
                }

                // 如果命令工厂无法创建命令，尝试根据命令ID查找对应的命令类型
                if (_commandTypeMap.TryGetValue(package.Packet.Command.FullCode, out var commandType))
                {
                    // 尝试使用构造函数创建命令实例
                    var constructor = GetSuitableConstructor(commandType);
                    if (constructor != null)
                    {
                        var parameters = PrepareConstructorParameters(constructor, package, sessionContext);
                        var command = Activator.CreateInstance(commandType, parameters) as ICommand;

                        // 设置命令的会话ID和数据包模型
                            if (command != null)
                            {
                                command.SessionID = sessionContext.SessionID;
                                command.Packet = package.Packet;
                            }

                        _logger?.LogDebug("根据命令ID创建命令实例: CommandId={CommandId}, Type={TypeName}",
                            package.Packet.Command, commandType.FullName);
                        return command;
                    }
                }

                // 如果没有找到对应的命令类型或无法创建实例，使用默认的MessageCommand
                var packetModelForDefault = new PacketModel(package.Packet.Body, package.Packet.Command)
                {
                    SessionId = package.Packet.SessionId,
                    PacketId = package.Packet.PacketId
                };

                var defaultCommand = new MessageCommand(
                    package.Packet.Command,
                    packetModelForDefault,
                    package.Packet.Body);
                defaultCommand.SessionID = sessionContext.SessionID;

                // 尝试从数据包中获取业务数据
                //  defaultBusinessDataCommand.BusinessData = packetModelForDefault.GetJsonData<object>();


                return defaultCommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建命令对象时出错: CommandId={CommandId}", package.Packet.Command);
                // 如果创建失败，返回一个默认的命令对象
                var packetModelForError = new PacketModel(package.Packet.Body, package.Packet.Command)
                {
                    SessionId = package.Packet.SessionId,
                    PacketId = package.Packet.PacketId
                };

                return new MessageCommand(
                    package.Packet.Command,
                    packetModelForError,
                    package.Packet.Body)
                {
                    SessionID = sessionContext.SessionID
                };
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
                if (parameters.Length >= 1 && parameters[0].ParameterType == typeof(PacketModel))
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
        protected virtual object[] PrepareConstructorParameters(ConstructorInfo constructor, ServerPackageInfo package, SessionInfo sessionContext)
        {
            var parameters = constructor.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType == typeof(PacketModel))
                {
                    // 创建PacketModel对象
                    parameterValues[i] = new PacketModel(package.Packet.Body, package.Packet.Command)
                    {
                        SessionId = package.Packet.SessionId,
                        PacketId = package.Packet.PacketId,
                        Extensions = package.Packet.Extensions ?? new Dictionary<string, object>()
                    };
                }
                else if (parameters[i].ParameterType == typeof(byte[]))
                {
                    parameterValues[i] = package.Packet.Body;
                }
                else if (parameters[i].ParameterType == typeof(CommandId))
                {
                    parameterValues[i] = package.Packet.Command;
                }
                else if (parameters[i].ParameterType == typeof(string))
                {
                    parameterValues[i] = package.Packet.SessionId;
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
            ServerPackageInfo requestPackage,
            ResponseBase result,
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
                    result.Code.ToString(),
                    cancellationToken);
            }
        }

        /// <summary>
        /// 创建响应数据包
        /// </summary>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="result">命令执行结果</param>
        /// <returns>响应数据包</returns>
        protected virtual PacketModel CreateResponsePackage(ServerPackageInfo requestPackage, ResponseBase result)
        {
            var response = new PacketModel
            {
                PacketId = GenerateResponseId(requestPackage.Packet.PacketId),
                Command = requestPackage.Packet.Command,
                Direction = requestPackage.Packet.Direction == PacketDirection.Request ? PacketDirection.Response : requestPackage.Packet.Direction,
                SessionId = requestPackage.Packet.SessionId,
                ClientId = requestPackage.Packet.ClientId,
                Status = result.IsSuccess ? PacketStatus.Completed : PacketStatus.Error,
                Extensions = new Dictionary<string, object>
                {
                    ["Data"] = result,
                    ["Message"] = result.Message,
                    ["Success"] = result.IsSuccess,
                    ["Code"] = result.Code,
                    ["Timestamp"] = result.Timestamp
                }
            };
     
            // 如果请求包中包含RequestId，则在响应包中保留它，以便客户端匹配请求和响应
            if (requestPackage.Packet?.Extensions?.TryGetValue("RequestId", out var requestId) == true)
            {
                response.Extensions["RequestId"] = requestId;
            }

            // 设置请求标识
            if (!string.IsNullOrEmpty(result.RequestId))
            {
                response.Extensions["RequestId"] = result.RequestId;
            }

            // 添加元数据
            if (result.Metadata != null && result.Metadata.Count > 0)
            {
                foreach (var metadata in result.Metadata)
                {
                    response.Extensions[metadata.Key] = metadata.Value;
                }
            }

            // 优先使用WithJsonData设置业务响应数据
            if (result != null)
            {
                try
                {
                    response.WithJsonData(result);
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Failed to set JSON data for response packet {PacketId}", response.PacketId);
                }
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
            try
            {
                // 检查会话是否有效
                if (session == null)
                {
                    _logger?.LogWarning("尝试发送响应到空会话: PacketId={PacketId}, CommandId={CommandId}", 
                        package.PacketId, package.Command);
                    return;
                }

                // 使用统一的序列化方法
                var serializedData = SerializePacket(package);

                // 加密数据
                var originalData = new RUINORERP.PacketSpec.Protocol.OriginalData(
                    (byte)package.Command.Category,
                    new byte[] { package.Command.OperationCode },
                    serializedData
                );
                var encryptedData = RUINORERP.PacketSpec.Security.EncryptedProtocol.EncryptionServerPackToClient(originalData);

                // 发送数据并捕获可能的异常
                try
                {
                    await session.SendAsync(encryptedData.ToByteArray(), cancellationToken);
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("Writing is not allowed after writer was completed"))
                {
                    // 处理管道写入器已完成的特定异常
                    _logger?.LogWarning(ex, "管道写入器已完成，无法发送响应: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // 忽略此异常，因为会话可能已经关闭
                }
                catch (Exception ex)
                {
                    // 记录其他发送异常
                    _logger?.LogError(ex, "发送响应时发生异常: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // 可以选择是否向上传播异常
                    // throw;
                }
            }
            catch (Exception ex)
            {
                // 捕获所有其他异常以确保方法不会失败
                _logger?.LogError(ex, "处理响应发送时发生未预期的异常");
            }
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
            ServerPackageInfo requestPackage,
            string errorCode,
            CancellationToken cancellationToken)
        {
            var errorInfo = GetErrorInfoByCode(errorCode);
            var errorResponse = new PacketModel
            {
                PacketId = GenerateResponseId(requestPackage.Packet?.PacketId ?? Guid.NewGuid().ToString()),
                Command = requestPackage.Packet?.Command ?? default(CommandId),
                Direction = PacketDirection.Response,
                SessionId = requestPackage.Packet?.SessionId,
                ClientId = requestPackage.Packet?.ClientId,
                Status = PacketStatus.Error,
                Extensions = new Dictionary<string, object>
                {
                    ["ErrorCode"] = errorInfo.code,
                    ["ErrorMessage"] = errorInfo.message,
                    ["Success"] = false
                }
            };

            // 如果请求包中包含RequestId，则在响应包中保留它，以便客户端匹配请求和响应
            if (requestPackage.Packet?.Extensions?.TryGetValue("RequestId", out var requestId) == true)
            {
                errorResponse.Extensions["RequestId"] = requestId;
            }

            await SendResponseAsync(session, errorResponse, cancellationToken);
        }

        /// <summary>
        /// 序列化数据包
        /// </summary>
        /// <param name="package">数据包</param>
        /// <returns>序列化后的字节数组</returns>
        protected virtual byte[] SerializePacket(PacketModel package)
        {
            // 使用统一的序列化方法
            return UnifiedSerializationService.SerializeWithMessagePack(package);
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

    /// <summary>
    /// 非泛型版本的统一SuperSocket命令适配器，便于在不需要指定会话类型的场景中使用
    /// </summary>
    [Command(Key = "SuperSocketCommandAdapter")]
    public class SuperSocketCommandAdapter : SuperSocketCommandAdapter<IAppSession>
    {
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            ICommandFactory commandFactory,
            ILogger<SuperSocketCommandAdapter> logger = null)
            : base(commandDispatcher, commandFactory, logger)
        { }
    }
}