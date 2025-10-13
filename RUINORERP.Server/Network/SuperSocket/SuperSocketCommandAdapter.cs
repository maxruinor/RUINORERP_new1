using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Message;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Interfaces.Services;
using SuperSocket.Server.Abstractions.Session;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.PacketSpec;
using RUINORERP.PacketSpec.Enums.Core;
using ICommand = RUINORERP.PacketSpec.Commands.ICommand;
using SuperSocket.Command;
using Azure;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MessagePack;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
using RUINORERP.Server.Network.Services;

namespace RUINORERP.Server.Network.SuperSocket
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
        private readonly CommandPacketAdapter packetAdapter;
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="commandFactory">命令工厂</param>
        /// <param name="logger">日志记录器</param>
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            CommandPacketAdapter _packetAdapter,
            ILogger<SuperSocketCommandAdapter> logger = null)
        {
            packetAdapter = _packetAdapter;
            _commandDispatcher = commandDispatcher;
            _logger = logger;
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
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, cancellationToken);
                return;
            }

            try
            {

                // 确保命令调度器已初始化
                if (!_commandDispatcher.IsInitialized)
                {
                    await _commandDispatcher.InitializeAsync(cancellationToken);
                }

                // 获取现有会话信息
                var sessionInfo = SessionService.GetSession(session.SessionID);
                if (sessionInfo == null)
                {
                    // 如果会话不存在，可能是连接已断开或会话已过期
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Auth_SessionExpired, cancellationToken);
                    return;
                }

                // 更新会话的最后活动时间
                sessionInfo.UpdateActivity(); // 使用专门的UpdateActivity方法更新活动时间
                SessionService.UpdateSession(sessionInfo);
                SessionService.UpdateSession(sessionInfo);
                // 同时调用专门的UpdateSessionActivity方法确保活动时间被正确更新
                SessionService.UpdateSessionActivity(session.SessionID);

                // 创建命令对象（第一层解析：基础命令创建）
                var command = packetAdapter.CreateCommand(package.Packet);
                if (command == null)
                {
                    _logger?.LogWarning("无法创建命令对象: CommandId={CommandId}", package.Packet.CommandId);
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Command_NotFound, cancellationToken);
                    return;
                }

                // 优化：如果命令已经通过ExecutionContext中的类型信息成功创建，则跳过冗余处理
                var executionContext = package.Packet.ExecutionContext;
                var isFromExecutionContext = executionContext?.CommandType != null && command.GetType() == executionContext.CommandType;
                if (!isFromExecutionContext)
                {
                    // 只有当初始化不是来自ExecutionContext时，才进行这些处理
                    if (command is BaseCommand baseCmd && package.Packet.CommandData != null && package.Packet.CommandData.Length > 0)
                    {
                        baseCmd.SetJsonBizData(package.Packet.CommandData);
                    }

                    // 第三层解析：设置基础数据（不解析具体业务内容）
                    // 检查是否为泛型BaseCommand<,>类型，如果是则自动设置请求二进制数据
                    var commandType = command.GetType();
                    if (commandType.IsGenericType &&
                        commandType.GetGenericTypeDefinition() == typeof(BaseCommand<,>))
                    {
                        var setRequest = commandType.GetMethod("SetRequestFromBinary");
                        setRequest?.Invoke(command, new object[] { package.Packet.CommandData });
                    }
                }
                else
                {
                    //统一验证基本的命令信息
                    if (command is LoginCommand)
                    {
                        executionContext.SessionId = session.SessionID;
                    }
                    // 验证会话有效性,登陆不需要验证
                    //if (!SessionService.IsValidSession(executionContext.SessionId))
                    //{
                    //    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Auth_SessionExpired, cancellationToken);
                    //    return;
                    //}

                }


                // 通过现有的命令调度器处理命令，添加超时保护
                BaseCommand<IResponse> result;
                try
                {
                    // 使用链接的取消令牌，考虑命令超时设置
                    var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                    // 如果命令有设置超时时间，则使用命令的超时时间，否则使用默认30秒
                    var timeout = command.TimeoutMs > 0 ? TimeSpan.FromMilliseconds(command.TimeoutMs) : TimeSpan.FromSeconds(30);
                    linkedCts.CancelAfter(timeout);

                    result = await _commandDispatcher.DispatchAsync(package.Packet, command, linkedCts.Token);
                }
                catch (OperationCanceledException ex)
                {
                    _logger?.LogError(ex, "命令执行超时或被取消: CommandId={CommandId}", package.Packet.CommandId);
                    result = BaseCommand<IResponse>.CreateError(UnifiedErrorCodes.System_Timeout.Message, UnifiedErrorCodes.System_Timeout.Code);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "命令执行异常: CommandId={CommandId}", package.Packet.CommandId);
                    result = BaseCommand<IResponse>.CreateError(UnifiedErrorCodes.System_InternalError.Message, UnifiedErrorCodes.System_InternalError.Code);
                }
                if (result == null)
                {
                    result = BaseCommand<IResponse>.CreateError(UnifiedErrorCodes.System_InternalError.Message, UnifiedErrorCodes.System_InternalError.Code);
                }
                if (!result.IsSuccess)
                {
                    _logger?.LogDebug($"命令执行完成:{result.Message}, Success={result.IsSuccess}");
                }
                await HandleCommandResultAsync(session, package, command, result, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理SuperSocket命令时发生异常: CommandId={CommandId}", package.Packet.CommandId);
                // 发送错误响应给客户端
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, cancellationToken);
            }
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
            ICommand command,
            BaseCommand<IResponse> result,
            CancellationToken cancellationToken)
        {
            if (result == null)
            {
                _logger?.LogWarning("命令执行结果为空，发送默认错误响应");
                await SendErrorResponseAsync(session, requestPackage, UnifiedErrorCodes.System_InternalError, cancellationToken);
                return;
            }

            if (result.IsSuccess)
            {
                // 命令执行成功，发送成功响应
                var responsePackage = UpdatePacketWithResponse(requestPackage.Packet, command, result);
                await SendResponseAsync(session, responsePackage, cancellationToken);
            }
            else
            {
                // 命令执行失败，发送增强的错误响应
                // 从结果中提取所有错误信息，包括元数据中的详细信息
                var errorCode = ExtractErrorCodeFromResponse(result);
                await SendEnhancedErrorResponseAsync(session, requestPackage, result, errorCode, cancellationToken);
            }
        }

        /// <summary>
        /// 附加响应数据到数据包发回客户端
        /// </summary>
        /// <param name="package"></param>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual PacketModel UpdatePacketWithResponse(PacketModel package, ICommand command, BaseCommand<IResponse> result)
        {
            package.ExecutionContext.ResponseType = result.ResponseData?.GetType() ?? typeof(IResponse);
            package.PacketId = IdGenerator.GenerateResponseId(package.PacketId);
            package.Direction = package.Direction == PacketDirection.Request ? PacketDirection.Response : package.Direction;
            package.SessionId = package.SessionId;
            package.Status = result.IsSuccess ? PacketStatus.Completed : PacketStatus.Error;
            package.Extensions = new Dictionary<string, object>
            {
                ["Data"] = result,
                ["Message"] = result.Message,
                ["TimestampUtc"] = result.TimestampUtc
            };



            // 添加元数据
            if (result.Metadata != null && result.Metadata.Count > 0)
            {
                foreach (var metadata in result.Metadata)
                {
                    package.Extensions[metadata.Key] = metadata.Value;
                }
            }


            if (command is BaseCommand baseCommand)
            {
                // 序列化响应数据 - 使用具体类型而不是接口类型，确保客户端能正确反序列化
                var ResponsePackBytes = MessagePackSerializer.Serialize(package.ExecutionContext.ResponseType, result.ResponseData, UnifiedSerializationService.MessagePackOptions);
                baseCommand.SetResponseData(ResponsePackBytes);
                package.CommandData = MessagePackSerializer.Serialize(package.ExecutionContext.CommandType, command, UnifiedSerializationService.MessagePackOptions);
                //package.SetCommandDataByMessagePack(command);
            }


            return package;
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
                PacketId = IdGenerator.GenerateResponseId(requestPackage.Packet.PacketId),
                Direction = requestPackage.Packet.Direction == PacketDirection.Request ? PacketDirection.Response : requestPackage.Packet.Direction,
                SessionId = requestPackage.Packet.SessionId,
                Status = result.IsSuccess ? PacketStatus.Completed : PacketStatus.Error,
                Extensions = new Dictionary<string, object>
                {
                    ["Data"] = result,
                    ["Message"] = result.Message,
                    ["TimestampUtc"] = result.TimestampUtc
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
                    _logger?.LogWarning(ex, "未能为响应数据包设置JSON数据 {PacketId}", response.PacketId);
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
                        package.PacketId, package.CommandId);
                    return;
                }
                package.SessionId = session.SessionID;
                var serializedData = UnifiedSerializationService.SerializeWithMessagePack<PacketModel>(package);

                // 加密数据
                var originalData = new OriginalData((byte)package.CommandId.Category, new byte[] { package.CommandId.OperationCode },
                    serializedData
                );
                var encryptedData = PacketSpec.Security.EncryptedProtocol.EncryptionServerPackToClient(originalData);

                // 发送数据并捕获可能的异常
                try
                {
                    await session.SendAsync(encryptedData.ToByteArray(), cancellationToken);
                }
                catch (TaskCanceledException ex) when (ex.InnerException is OperationCanceledException && cancellationToken.IsCancellationRequested)
                {
                    // 处理任务被取消的特定异常
                    _logger?.LogWarning(ex, "发送响应被取消: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // 忽略此异常，因为可能是正常的超时或取消操作
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



        protected virtual async ValueTask SendResponseToClientAsync(TAppSession session, PacketModel packetModel)
        {
            packetModel.SessionId = session.SessionID;
            // 序列化和加密数据包
            var payload = UnifiedSerializationService.SerializeWithMessagePack(packetModel);

            // 加密数据
            var originalData = new OriginalData(
                (byte)CommandCategory.Authentication,
                new byte[] { AuthenticationCommands.Connected.OperationCode },
                payload
            );
            var encryptedData = PacketSpec.Security.EncryptedProtocol.EncryptionServerPackToClient(originalData);

            // 发送数据并捕获可能的异常
            try
            {
                await session.SendAsync(encryptedData.ToByteArray());
            }
            catch (TaskCanceledException ex)
            {
                // 处理任务被取消的特定异常
                _logger?.LogWarning(ex, "发送响应到客户端被取消: SessionId={SessionId}, PacketId={PacketId}",
                    packetModel.SessionId, packetModel.PacketId);
                // 忽略此异常，因为可能是正常的超时或取消操作
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Writing is not allowed after writer was completed"))
            {
                // 处理管道写入器已完成的特定异常
                _logger?.LogWarning(ex, "管道写入器已完成，无法发送响应到客户端: SessionId={SessionId}, PacketId={PacketId}",
                    packetModel.SessionId, packetModel.PacketId);
                // 忽略此异常，因为会话可能已经关闭
            }
            catch (Exception ex)
            {
                // 记录其他发送异常
                _logger?.LogError(ex, "发送响应到客户端时发生异常: SessionId={SessionId}, PacketId={PacketId}",
                    packetModel.SessionId, packetModel.PacketId);
                // 可以选择是否向上传播异常
                // throw;
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
            ErrorCode errorCode,
            CancellationToken cancellationToken)
        {
            var errorResponse = new PacketModel
            {
                PacketId = IdGenerator.GenerateResponseId(requestPackage.Packet?.PacketId ?? Guid.NewGuid().ToString()),
                Direction = PacketDirection.Response,
                SessionId = requestPackage.Packet?.SessionId,
                Status = PacketStatus.Error,
                Extensions = new Dictionary<string, object>
                {
                    ["ErrorCode"] = errorCode.Code,
                    ["ErrorMessage"] = errorCode.Message,
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
        /// 从响应结果中提取错误代码信息
        /// </summary>
        /// <param name="result">响应结果</param>
        /// <returns>错误代码对象</returns>
        protected virtual ErrorCode ExtractErrorCodeFromResponse(BaseCommand<IResponse> result)
        {
            if (result == null)
            {
                return UnifiedErrorCodes.System_InternalError;
            }

            // 优先使用响应中的元数据提取更详细的错误信息
            string detailedMessage = result.Message;

            if (result.Metadata != null)
            {
                // 尝试获取更详细的错误信息
                if (result.Metadata.TryGetValue("Exception", out var exceptionObj))
                {
                    detailedMessage = $"{result.Message} | Exception: {exceptionObj}";
                }
            }

            // 直接使用响应中的错误代码创建错误代码对象
            return new ErrorCode(0, detailedMessage);
        }

        /// <summary>
        /// 发送增强的错误响应，包含命令处理结果中的所有错误信息
        /// </summary>
        /// <param name="session">SuperSocket会话</param>
        /// <param name="requestPackage">请求数据包</param>
        /// <param name="result">命令处理结果</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>发送结果任务</returns>
        protected virtual async ValueTask SendEnhancedErrorResponseAsync(
            TAppSession session,
            ServerPackageInfo requestPackage,
            BaseCommand<IResponse> result,
            ErrorCode errorCode,
            CancellationToken cancellationToken)
        {
            var errorResponse = new PacketModel
            {
                PacketId = IdGenerator.GenerateResponseId(requestPackage.Packet?.PacketId ?? Guid.NewGuid().ToString()),
                Direction = PacketDirection.Response,
                SessionId = requestPackage.Packet?.SessionId,
                Status = PacketStatus.Error,
                Extensions = new Dictionary<string, object>
                {
                    ["ErrorCode"] = errorCode.Code,
                    ["ErrorMessage"] = errorCode.Message,
                    ["Success"] = false,
                    ["TimestampUtc"] = result.TimestampUtc,
                    ["OriginalMessage"] = result.Message,
                }
            };

            // 添加请求标识
            if (!string.IsNullOrEmpty(result.RequestId))
            {
                errorResponse.Extensions["RequestId"] = result.RequestId;
            }

            // 添加元数据中的所有错误信息
            if (result.Metadata != null && result.Metadata.Count > 0)
            {
                foreach (var metadata in result.Metadata)
                {
                    // 避免重复添加已经存在的键
                    if (!errorResponse.Extensions.ContainsKey(metadata.Key))
                    {
                        errorResponse.Extensions[metadata.Key] = metadata.Value;
                    }
                }
            }

            // 如果请求包中包含RequestId，则在响应包中保留它，以便客户端匹配请求和响应
            if (requestPackage.Packet?.Extensions?.TryGetValue("RequestId", out var requestId) == true)
            {
                errorResponse.Extensions["RequestId"] = requestId;
            }

            // 记录详细的错误信息用于调试
            _logger?.LogWarning("发送增强错误响应: ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}, MetadataKeys=[{MetadataKeys}]",
                errorCode.Code, errorCode.Message,
                result.Metadata != null ? string.Join(", ", result.Metadata.Keys) : "none");

            await SendResponseAsync(session, errorResponse, cancellationToken);
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
            CommandPacketAdapter _packetAdapter,
            ILogger<SuperSocketCommandAdapter> logger = null)
            : base(commandDispatcher, _packetAdapter, logger)
        { }
    }

}