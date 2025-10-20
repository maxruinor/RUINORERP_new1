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
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandDispatcher">命令调度器</param>
        /// <param name="commandCreationService">命令创建服务</param>
        /// <param name="logger">日志记录器</param>
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            ILogger<SuperSocketCommandAdapter> logger = null)
        {
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
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, CancellationToken.None);
                return;
            }

            try
            {

                if (package.Packet.CommandId == AuthenticationCommands.Login)
                {
                    // 如果命令ID为0，则表示是心跳包
                    package.Packet.SessionId = session.SessionID;
                    package.Packet.ExecutionContext.SessionId = session.SessionID;
                }

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
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Auth_SessionExpired, CancellationToken.None);
                    return;
                }

                // 更新会话的最后活动时间
                sessionInfo.UpdateActivity(); // 使用专门的UpdateActivity方法更新活动时间
                SessionService.UpdateSession(sessionInfo);
                SessionService.UpdateSession(sessionInfo);
                // 同时调用专门的UpdateSessionActivity方法确保活动时间被正确更新
                SessionService.UpdateSessionActivity(session.SessionID);

                /*
                // 创建命令对象（第一层解析：基础命令创建）
                var command = commandCreationService.CreateCommand(package.Packet);
                if (command == null)
                {
                    _logger?.LogWarning("无法创建命令对象: CommandId={CommandId}", package.Packet.CommandId);
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Command_NotFound, CancellationToken.None);
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
                    if (command is BaseCommand<IRequest, LoginResponse>)
                    {
                        executionContext.SessionId = session.SessionID;
                    }


                }
                */

                // 通过现有的命令调度器处理命令，添加超时保护
                //BaseCommand<IRequest, IResponse> result;


                IResponse result;
                try
                {
                    // 使用链接的取消令牌，考虑命令超时设置
                    // 修复：正确创建链接令牌源，避免修改原始cancellationToken
                    CancellationTokenSource linkedCts;
                    if (cancellationToken != CancellationToken.None)
                    {
                        // 如果传入了有效的取消令牌，则创建链接令牌源
                        linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    }
                    else
                    {
                        // 否则创建新的令牌源
                        linkedCts = new CancellationTokenSource();
                    }

                    // 如果命令有设置超时时间，则使用命令的超时时间，否则使用默认300秒
                    //这里设置大一点按执行时间算。每个处理类自己定义了不同的时间
                    var timeout = TimeSpan.FromSeconds(300);
                    linkedCts.CancelAfter(timeout);
                    result = await _commandDispatcher.DispatchAsync(package.Packet, linkedCts.Token);
                    //  result = await _commandDispatcher.DispatchAsync(package.Packet, command, linkedCts.Token);

                    // 释放令牌源资源
                    linkedCts.Dispose();
                }
                catch (OperationCanceledException ex)
                {
                    _logger?.LogError(ex, "命令执行超时或被取消: CommandId={CommandId}", package.Packet.CommandId);
                    result = ResponseBase.CreateError(UnifiedErrorCodes.System_Timeout.Message, UnifiedErrorCodes.System_Timeout.Code);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "命令执行异常: CommandId={CommandId}", package.Packet.CommandId);
                    result = ResponseBase.CreateError(UnifiedErrorCodes.System_InternalError.Message, UnifiedErrorCodes.System_InternalError.Code);
                }
                if (result == null)
                {
                    result = ResponseBase.CreateError(UnifiedErrorCodes.System_InternalError.Message, UnifiedErrorCodes.System_InternalError.Code);
                }

                // 使用新的 CancellationToken.None 来确保响应能够发送，即使命令处理超时
                await HandleCommandResultAsync(session, package, result, CancellationToken.None);

            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理SuperSocket命令时发生异常: CommandId={CommandId}", package.Packet.CommandId);
                // 发送错误响应给客户端，使用 CancellationToken.None 确保错误响应能够发送
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, CancellationToken.None);
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
            IResponse response,
            CancellationToken cancellationToken)
        {
            if (response == null)
            {
                //_logger?.LogWarning("命令执行结果为空，发送默认错误响应");
                await SendErrorResponseAsync(session, requestPackage, UnifiedErrorCodes.System_InternalError, CancellationToken.None);
                return;
            }

            if (response != null && response.IsSuccess)
            {
                // 命令执行成功，发送成功响应
                var responsePackage = UpdatePacketWithResponse(requestPackage.Packet, response);

                await SendResponseAsync(session, requestPackage.Packet, CancellationToken.None);
            }
            else
            {
                // 命令执行失败，发送增强的错误响应
                // 从结果中提取所有错误信息，包括元数据中的详细信息
                // var errorCode = ExtractErrorCodeFromResponse(response);
                await SendEnhancedErrorResponseAsync(session, requestPackage, response, UnifiedErrorCodes.Biz_DataNotFound, CancellationToken.None);
            }
        }



        /// <summary>
        /// 附加响应数据到数据包发回客户端
        /// </summary>
        /// <param name="package"></param>
        /// <param name="command"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual PacketModel UpdatePacketWithResponse(PacketModel package, IResponse result)
        {
            // 检查响应数据是否为空，避免空引用异常
            if (result == null)
            {
                _logger?.LogWarning("响应数据为空，使用默认错误状态");
                package.Status = PacketStatus.Error;
            }
            else
            {
                package.Status = result.IsSuccess ? PacketStatus.Completed : PacketStatus.Error;
            }
            package.Response = result;
            package.Direction = PacketDirection.Response;

            package.PacketId = IdGenerator.GenerateResponseId(package.PacketId);
            package.Direction = PacketDirection.Response; // 明确设置为响应方向
            package.SessionId = package.SessionId;
            package.Extensions = new Dictionary<string, object>
            {
                ["Data"] = result,
            };



            // 添加元数据
            if (result.Metadata != null && result.Metadata.Count > 0)
            {
                foreach (var metadata in result.Metadata)
                {
                    package.Extensions[metadata.Key] = metadata.Value;
                }
            }

            //if (command is BaseCommand baseCommand && result.Response != null)
            //{
            //    // 序列化响应数据 - 使用具体类型而不是接口类型，确保客户端能正确反序列化
            //    var ResponsePackBytes = MessagePackSerializer.Serialize(package.ExecutionContext.ResponseType, result.Response, UnifiedSerializationService.MessagePackOptions);
            //    baseCommand.SetResponseData(ResponsePackBytes);
            //    package.CommandData = MessagePackSerializer.Serialize(package.ExecutionContext.CommandType, command, UnifiedSerializationService.MessagePackOptions);
            //    //package.SetCommandDataByMessagePack(command);
            //}


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

            //// 优先使用WithJsonData设置业务响应数据
            //if (result != null)
            //{
            //    try
            //    {
            //        response.WithJsonData(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger?.LogWarning(ex, "未能为响应数据包设置JSON数据 {PacketId}", response.PacketId);
            //    }
            //}

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
                package.Direction = PacketDirection.Response;
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
        protected virtual ErrorCode ExtractErrorCodeFromResponse(IResponse result)
        {
            if (result == null)
            {
                return UnifiedErrorCodes.System_InternalError;
            }

            // 检查响应数据是否为空
            if (result == null)
            {
                _logger?.LogWarning("响应数据为空，使用默认错误信息");
                return UnifiedErrorCodes.System_InternalError;
            }

            // 优先使用响应中的元数据提取更详细的错误信息
            string detailedMessage = result.ErrorMessage;

            if (result.Metadata != null)
            {
                // 尝试获取更详细的错误信息
                if (result.Metadata.TryGetValue("Exception", out var exceptionObj))
                {
                    // 使用空值条件运算符避免空引用异常
                    string responseMessage = result?.Message ?? "未知错误";
                    detailedMessage = $"{responseMessage} | Exception: {exceptionObj}";
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
          IResponse result,
            ErrorCode errorCode,
            CancellationToken cancellationToken)
        {
            // 检查响应数据是否为空，避免空引用异常
            string originalErrorMessage = result?.ErrorMessage ?? "响应数据为空";
            string originalMessage = result?.Message ?? "无错误消息";

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
                    ["OriginalErrorMessage"] = originalErrorMessage,
                    ["OriginalMessage"] = originalMessage,
                }
            };



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
            ILogger<SuperSocketCommandAdapter> logger = null)
            : base(commandDispatcher,  logger)
        { }
    }

}