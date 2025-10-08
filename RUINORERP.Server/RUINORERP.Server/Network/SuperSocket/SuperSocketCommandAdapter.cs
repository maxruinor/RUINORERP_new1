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

namespace RUINORERP.Server.Network.SuperSocket
{
    /// <summary>
    /// ͳһ��SuperSocket����������
    /// ������ԭ�е�SimplifiedSuperSocketAdapter��SocketCommand��SuperSocketCommandAdapter�Ĺ���
    /// 
    /// �������̣�
    /// 1. SuperSocket���յ����Կͻ��˵����ݰ�
    /// 2. SuperSocketCommandAdapter.ExecuteAsync����������
    /// 3. ��CommandDispatcher��ȡ��ע�����������ӳ��
    /// 4. �������ݰ��е�����ID������Ӧ������ʵ��
    /// 5. ͨ��CommandDispatcher.DispatchAsync�����ַ��������Ӧ�Ĵ�����
    /// 6. ������ͨ�����緵�ظ��ͻ���
    /// </summary>
    [Command(Key = "SuperSocketCommandAdapter")]
    public class SuperSocketCommandAdapter<TAppSession> : IAsyncCommand<TAppSession, ServerPackageInfo>
        where TAppSession : IAppSession
    {
        private readonly CommandDispatcher _commandDispatcher;
        private readonly ILogger<SuperSocketCommandAdapter> _logger;
        private readonly ICommandFactory _commandFactory;
        private readonly CommandPacketAdapter packetAdapter;
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();
     
  
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="commandDispatcher">���������</param>
        /// <param name="commandFactory">�����</param>
        /// <param name="logger">��־��¼��</param>
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            CommandPacketAdapter _packetAdapter,
            ICommandFactory commandFactory,
            ILogger<SuperSocketCommandAdapter> logger = null)
        {
            packetAdapter = _packetAdapter;
            _commandDispatcher = commandDispatcher;
            _commandFactory = commandFactory;
            _logger = logger;
        }



        /// <summary>
        /// ִ������
        /// ��SuperSocket���������ת��Ϊ���е������ϵͳ
        /// </summary>
        /// <param name="session">SuperSocket�Ự</param>
        /// <param name="package">���ݰ�</param>
        /// <param name="cancellationToken">ȡ������</param>
        /// <returns>ִ�н������</returns>
        public async ValueTask ExecuteAsync(TAppSession session, ServerPackageInfo package, CancellationToken cancellationToken)
        {
            if (package == null)
            {
                _logger?.LogWarning("���յ��յ����ݰ�");
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, cancellationToken);
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(package.Packet.SessionId))
                {
                    package.Packet.SessionId = session.SessionID;
                }

                // ȷ������������ѳ�ʼ��
                if (!_commandDispatcher.IsInitialized)
                {
                    await _commandDispatcher.InitializeAsync(cancellationToken);
                }

                // ��ȡ���лỰ��Ϣ
                var sessionInfo = SessionService.GetSession(session.SessionID);
                if (sessionInfo == null)
                {
                    // ����Ự�����ڣ������������ѶϿ���Ự�ѹ���
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Auth_SessionExpired, cancellationToken);
                    return;
                }

                // ���»Ự�����ʱ��
                sessionInfo.UpdateActivity(); // ʹ��ר�ŵ�UpdateActivity�������»ʱ��
                SessionService.UpdateSession(sessionInfo);
                // ͬʱ����ר�ŵ�UpdateSessionActivity����ȷ���ʱ�䱻��ȷ����
                SessionService.UpdateSessionActivity(session.SessionID);

                // ����������󣨵�һ������������������
                var command = packetAdapter.CreateCommand(package.Packet);
                if (command == null)
                {
                    _logger?.LogWarning("�޷������������: CommandId={CommandId}", package.Packet.CommandId);
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Command_NotFound, cancellationToken);
                    return;
                }

                // �ڶ������������Ԥ��������ȡָ����Ϣ������������ҵ�����ݣ�
                var commandInfo = PreParseCommand(command, package.Packet);
                if (commandInfo == null)
                {
                    _logger?.LogWarning("����Ԥ����ʧ��: CommandId={CommandId}", package.Packet.CommandId);
                    await SendErrorResponseAsync(session, package, UnifiedErrorCodes.Command_InvalidFormat, cancellationToken);
                    return;
                }

                // �����������ȼ�������Ԥ���������ʹ��CommandPriorityö�٣�
                command.Priority = commandInfo.PriorityLevel;

                // ��������������û������ݣ�����������ҵ�����ݣ�
                // ����Ƿ�Ϊ����BaseCommand<,>���ͣ���������Զ������������������
                var commandType = command.GetType();
                if (commandType.IsGenericType &&
                    commandType.GetGenericTypeDefinition() == typeof(BaseCommand<,>))
                {
                    var setRequest = commandType.GetMethod("SetRequestFromBinary");
                    setRequest?.Invoke(command, new object[] { package.Packet.CommandData });
                }

                // �����BaseCommand�Ұ���AuthToken�����Զ���ȡ�����õ�ִ��������
                if (command is BaseCommand baseCommand && !string.IsNullOrEmpty(baseCommand.AuthToken))
                {
                    // ȷ��ExecutionContext�ѳ�ʼ��
                    if (baseCommand.ExecutionContext == null)
                    {
                        baseCommand.ExecutionContext = new CommandExecutionContext();
                    }
                    // ����Token
                    baseCommand.ExecutionContext.Token = baseCommand.AuthToken;
                }



                // ͨ�����е�������������������ӳ�ʱ����
                ResponseBase result;
                try
                {
                    // ʹ�����ӵ�ȡ�����ƣ��������ʱ����
                    var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

                    // ������������ó�ʱʱ�䣬��ʹ������ĳ�ʱʱ�䣬����ʹ��Ĭ��30��
                    var timeout = command.TimeoutMs > 0 ? TimeSpan.FromMilliseconds(command.TimeoutMs) : TimeSpan.FromSeconds(30);
                    linkedCts.CancelAfter(timeout);

                    result = await _commandDispatcher.DispatchAsync(package.Packet, command, linkedCts.Token);
                }
                catch (OperationCanceledException ex)
                {
                    _logger?.LogError(ex, "����ִ�г�ʱ��ȡ��: CommandId={CommandId}", package.Packet.CommandId);
                    result = ResponseBase.CreateError(UnifiedErrorCodes.System_Timeout.Message, UnifiedErrorCodes.System_Timeout.Code);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "����ִ���쳣: CommandId={CommandId}", package.Packet.CommandId);
                    result = ResponseBase.CreateError(UnifiedErrorCodes.System_InternalError.Message, UnifiedErrorCodes.System_InternalError.Code);
                }
                if (result == null)
                {
                    result = ResponseBase.CreateError(UnifiedErrorCodes.System_InternalError.Message, UnifiedErrorCodes.System_InternalError.Code);
                }

                if (!result.IsSuccess)
                {
                    _logger?.LogDebug($"����ִ�����:{result.Message}, Success={result.IsSuccess}");
                }
                await HandleCommandResultAsync(session, package, result, cancellationToken);

                // ��¼����ִ����ɵ���־
                _logger?.LogDebug("����ִ�����: CommandId={CommandId}, Success={Success}",
                    package.Packet.CommandId, result?.IsSuccess ?? false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "����SuperSocket����ʱ�����쳣: CommandId={CommandId}", package.Packet.CommandId);
                // ���ʹ�����Ӧ���ͻ���
                await SendErrorResponseAsync(session, package, UnifiedErrorCodes.System_InternalError, cancellationToken);
            }
        }


        /// <summary>
        /// ����Ԥ������Ϣ
        /// </summary>
        private class CommandPreParseInfo
        {
            public uint CommandId { get; set; }
            public string CommandName { get; set; }
            public bool RequiresAuthentication { get; set; }
            public CommandPriority PriorityLevel { get; set; }
            public Type TargetCommandType { get; set; }
        }

        /// <summary>
        /// Ԥ��������ڶ����������ȡָ����Ϣ������������ҵ�����ݣ�
        /// </summary>
        /// <param name="command">�������</param>
        /// <param name="packet">���ݰ�</param>
        /// <returns>Ԥ������Ϣ</returns>
        private CommandPreParseInfo PreParseCommand(ICommand command, PacketModel packet)
        {
            try
            {
                var commandId = packet.CommandId;

                // ��������IDȷ����������
                var requiresAuth = IsAuthenticationRequired(commandId);
                var priorityLevel = command.Priority;
                var targetType = command?.GetType();

                return new CommandPreParseInfo
                {
                    CommandId = commandId,
                    RequiresAuthentication = requiresAuth,
                    PriorityLevel = priorityLevel,
                    TargetCommandType = targetType
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "����Ԥ����ʧ��: CommandId={CommandId}", packet.CommandId);
                return null;
            }
        }


        /// <summary>
        /// �ж��Ƿ���Ҫ��֤
        /// </summary>
        private bool IsAuthenticationRequired(uint commandId)
        {
            // ��¼��������Ҫ��֤
            var authCommands = new uint[]
            {
                AuthenticationCommands.Login.FullCode,
                AuthenticationCommands.LoginRequest.FullCode,
                AuthenticationCommands.PrepareLogin.FullCode,
                AuthenticationCommands.ValidateToken.FullCode,
                AuthenticationCommands.RefreshToken.FullCode
            };

            return !authCommands.Contains(commandId);
        }





        /*
        /// <summary>
        /// �����������
        /// ��������ID�����ݰ����ݴ����ʵ����͵��������
        /// </summary>
        /// <param name="package">���ݰ�</param>
        /// <param name="sessionContext">�Ự������</param>
        /// <returns>�������������</returns>
        protected virtual ICommand CreateCommand(ServerPackageInfo package, SessionInfo sessionContext)
        {
            try
            {
                // ����ʹ���������������
                if (_commandFactory != null)
                {
                    var command = _commandFactory.CreateCommand(package.Packet as PacketModel);
                    if (command != null)
                    {
                        // ��������ĻỰID�����ݰ�ģ��
                        command.SessionId = sessionContext.SessionID;
                        command.Packet = package.Packet;

                        // ���Դ����ݰ��л�ȡҵ������ ���������ٿ�
                        // businessDataCommand.BusinessData = packetModel.GetJsonData<object>();


                        return command;
                    }
                }

                // ���������޷�����������Ը�������ID���Ҷ�Ӧ����������
                // ֱ��ʹ��CommandDispatcher�еķ�����ȡ��������
                var commandType = _commandDispatcher.GetCommandType(package.Packet.Command.FullCode);
                if (commandType != null)
                {
                    // ����ʹ�ù��캯����������ʵ��
                    var constructor = GetSuitableConstructor(commandType);
                    if (constructor != null)
                    {
                        var parameters = PrepareConstructorParameters(constructor, package, sessionContext);
                        var command = Activator.CreateInstance(commandType, parameters) as ICommand;

                        // ��������ĻỰID�����ݰ�ģ��
                            if (command != null)
                            {
                                command.SessionId = sessionContext.SessionID;
                                command.Packet = package.Packet;
                            }

                        _logger?.LogDebug("��������ID��������ʵ��: CommandId={CommandId}, Type={TypeName}",
                            package.Packet.Command, commandType.FullName);
                        return command;
                    }
                }

                // ���û���ҵ���Ӧ���������ͻ��޷�����ʵ����ʹ��Ĭ�ϵ�MessageCommand
                var packetModelForDefault = PacketBuilder.Create()
                    .WithCommand(package.Packet.Command)
                    .WithBinaryData(package.Packet.Body)
                    .WithSession(package.Packet.SessionId)
                    .WithExtension("PacketId", package.Packet.PacketId)
                    .Build();

                var defaultCommand = new MessageCommand(
                    package.Packet.Command,
                    packetModelForDefault,
                    package.Packet.Body);

                // ���Դ����ݰ��л�ȡҵ������
                //  defaultBusinessDataCommand.BusinessData = packetModelForDefault.GetJsonData<object>();


                return defaultCommand;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "�����������ʱ����: CommandId={CommandId}", package.Packet.Command);
                // �������ʧ�ܣ�����һ��Ĭ�ϵ��������
                var packetModelForError = PacketBuilder.Create()
                    .WithCommand(package.Packet.Command)
                    .WithBinaryData(package.Packet.Body)
                    .WithSession(package.Packet.SessionId)
                    .WithExtension("PacketId", package.Packet.PacketId)
                    .Build();

                return new MessageCommand(
                    package.Packet.Command,
                    packetModelForError,
                    package.Packet.Body)
                {
                    SessionId = sessionContext.SessionID
                };
            }
        }

        /// <summary>
        /// ��ȡ�ʺϵĹ��캯��
        /// </summary>
        /// <param name="commandType">��������</param>
        /// <returns>���캯����Ϣ</returns>
        protected virtual ConstructorInfo GetSuitableConstructor(Type commandType)
        {
            try
            {
                // ���Ұ���CommandId��SessionInfo��Data�����Ĺ��캯��
                var constructors = commandType.GetConstructors();
                foreach (var constructor in constructors)
                {
                    var parameters = constructor.GetParameters();
                    if (parameters.Length >= 1 && parameters[0].ParameterType == typeof(PacketModel))
                    {
                        return constructor;
                    }
                }

                // ���û���ҵ�����Ĺ��캯�������ص�һ�����õĹ��캯��
                return constructors.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "��ȡ�������� {CommandType} �Ĺ��캯��ʱ����", commandType?.FullName ?? "null");
                return null;
            }
        }

        /// <summary>
        /// ׼�����캯������
        /// </summary>
        /// <param name="constructor">���캯����Ϣ</param>
        /// <param name="package">���ݰ�</param>
        /// <param name="sessionContext">�Ự������</param>
        /// <returns>��������</returns>
        protected virtual object[] PrepareConstructorParameters(ConstructorInfo constructor, ServerPackageInfo package, SessionInfo sessionContext)
        {
            try
            {
                var parameters = constructor.GetParameters();
                var parameterValues = new object[parameters.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i].ParameterType == typeof(PacketModel))
                    {
                        // ����PacketModel����
                        var builder = PacketBuilder.Create()
                            .WithCommand(package.Packet.Command)
                            .WithBinaryData(package.Packet.Body)
                            .WithSession(package.Packet.SessionId)
                            .WithExtension("PacketId", package.Packet.PacketId);
                        
                        if (package.Packet.Extensions != null)
                        {
                            foreach (var extension in package.Packet.Extensions)
                            {
                                // ���⸲���Ѿ����õ���չ����
                                if (extension.Key != "PacketId")
                                {
                                    builder.WithExtension(extension.Key, extension.Value);
                                }
                            }
                        }
                        
                        parameterValues[i] = builder.Build();
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
                        // �����������͵Ĳ���������ʹ��Ĭ��ֵ��null
                        parameterValues[i] = parameters[i].HasDefaultValue ? parameters[i].DefaultValue : null;
                    }
                }

                return parameterValues;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "׼�����캯������ʱ����");
                return new object[0];
            }
        }

        */


        /// <summary>
        /// ��������ִ�н��
        /// </summary>
        /// <param name="session">SuperSocket�Ự</param>
        /// <param name="requestPackage">�������ݰ�</param>
        /// <param name="result">����ִ�н��</param>
        /// <param name="cancellationToken">ȡ������</param>
        /// <returns>����������</returns>
        protected virtual async ValueTask HandleCommandResultAsync(
            TAppSession session,
            ServerPackageInfo requestPackage,
            ResponseBase result,
            CancellationToken cancellationToken)
        {
            if (result == null)
            {
                _logger?.LogWarning("����ִ�н��Ϊ�գ�����Ĭ�ϴ�����Ӧ");
                await SendErrorResponseAsync(session, requestPackage, UnifiedErrorCodes.System_InternalError, cancellationToken);
                return;
            }

            if (result.IsSuccess)
            {
                // ����ִ�гɹ������ͳɹ���Ӧ
                var responsePackage = CreateResponsePackage(requestPackage, result);
                await SendResponseAsync(session, responsePackage, cancellationToken);
            }
            else
            {
                // ����ִ��ʧ�ܣ�������ǿ�Ĵ�����Ӧ
                // �ӽ������ȡ���д�����Ϣ������Ԫ�����е���ϸ��Ϣ
                var errorCode = ExtractErrorCodeFromResponse(result);
                await SendEnhancedErrorResponseAsync(session, requestPackage, result, errorCode, cancellationToken);
            }
        }

        /// <summary>
        /// ������Ӧ���ݰ�
        /// </summary>
        /// <param name="requestPackage">�������ݰ�</param>
        /// <param name="result">����ִ�н��</param>
        /// <returns>��Ӧ���ݰ�</returns>
        protected virtual PacketModel CreateResponsePackage(ServerPackageInfo requestPackage, ResponseBase result)
        {
            var response = new PacketModel
            {
                PacketId =IdGenerator. GenerateResponseId(requestPackage.Packet.PacketId),
                Direction = requestPackage.Packet.Direction == PacketDirection.Request ? PacketDirection.Response : requestPackage.Packet.Direction,
                SessionId = requestPackage.Packet.SessionId,
                Status = result.IsSuccess ? PacketStatus.Completed : PacketStatus.Error,
                Extensions = new Dictionary<string, object>
                {
                    ["Data"] = result,
                    ["Message"] = result.Message,
                    ["Code"] = result.Code,
                    ["TimestampUtc"] = result.TimestampUtc
                }
            };

            // ���������а���RequestId��������Ӧ���б��������Ա�ͻ���ƥ���������Ӧ
            if (requestPackage.Packet?.Extensions?.TryGetValue("RequestId", out var requestId) == true)
            {
                response.Extensions["RequestId"] = requestId;
            }

            // ���������ʶ
            if (!string.IsNullOrEmpty(result.RequestId))
            {
                response.Extensions["RequestId"] = result.RequestId;
            }

            // ���Ԫ����
            if (result.Metadata != null && result.Metadata.Count > 0)
            {
                foreach (var metadata in result.Metadata)
                {
                    response.Extensions[metadata.Key] = metadata.Value;
                }
            }

            // ����ʹ��WithJsonData����ҵ����Ӧ����
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
        /// ������Ӧ
        /// </summary>
        /// <param name="session">SuperSocket�Ự</param>
        /// <param name="package">���ݰ�</param>
        /// <param name="cancellationToken">ȡ������</param>
        /// <returns>���ͽ������</returns>
        protected virtual async ValueTask SendResponseAsync(TAppSession session, PacketModel package, CancellationToken cancellationToken)
        {
            try
            {
                // ���Ự�Ƿ���Ч
                if (session == null)
                {
                    _logger?.LogWarning("���Է�����Ӧ���ջỰ: PacketId={PacketId}, CommandId={CommandId}",
                        package.PacketId, package.CommandId);
                    return;
                }
                package.SessionId = session.SessionID;
                // ʹ��ͳһ�����л�����
                var serializedData = SerializePacket(package);

                // ��������
                var originalData = new OriginalData(
                    (byte)package.CommandId.Category,
                    new byte[] { package.CommandId.OperationCode },
                    serializedData
                );
                var encryptedData = PacketSpec.Security.EncryptedProtocol.EncryptionServerPackToClient(originalData);

                // �������ݲ�������ܵ��쳣
                try
                {
                    await session.SendAsync(encryptedData.ToByteArray(), cancellationToken);
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("Writing is not allowed after writer was completed"))
                {
                    // ����ܵ�д��������ɵ��ض��쳣
                    _logger?.LogWarning(ex, "�ܵ�д��������ɣ��޷�������Ӧ: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // ���Դ��쳣����Ϊ�Ự�����Ѿ��ر�
                }
                catch (Exception ex)
                {
                    // ��¼���������쳣
                    _logger?.LogError(ex, "������Ӧʱ�����쳣: SessionId={SessionId}, PacketId={PacketId}",
                        package.SessionId, package.PacketId);
                    // ����ѡ���Ƿ����ϴ����쳣
                    // throw;
                }
            }
            catch (Exception ex)
            {
                // �������������쳣��ȷ����������ʧ��
                _logger?.LogError(ex, "������Ӧ����ʱ����δԤ�ڵ��쳣");
            }
        }

        /// <summary>
        /// ���ʹ�����Ӧ
        /// </summary>
        /// <param name="session">SuperSocket�Ự</param>
        /// <param name="requestPackage">�������ݰ�</param>
        /// <param name="errorCode">�������</param>
        /// <param name="cancellationToken">ȡ������</param>
        /// <returns>���ͽ������</returns>
        protected virtual async ValueTask SendErrorResponseAsync(
            TAppSession session,
            ServerPackageInfo requestPackage,
            ErrorCode errorCode,
            CancellationToken cancellationToken)
        { 
            var errorResponse = new PacketModel
            {
                PacketId =IdGenerator.GenerateResponseId(requestPackage.Packet?.PacketId ?? Guid.NewGuid().ToString()),
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

            // ���������а���RequestId��������Ӧ���б��������Ա�ͻ���ƥ���������Ӧ
            if (requestPackage.Packet?.Extensions?.TryGetValue("RequestId", out var requestId) == true)
            {
                errorResponse.Extensions["RequestId"] = requestId;
            }

            await SendResponseAsync(session, errorResponse, cancellationToken);
        }

        /// <summary>
        /// ���л����ݰ�
        /// </summary>
        /// <param name="package">���ݰ�</param>
        /// <returns>���л�����ֽ�����</returns>
        protected virtual byte[] SerializePacket(PacketModel package)
        {
            // ʹ��ͳһ�����л�����
            return UnifiedSerializationService.SerializeWithMessagePack(package);
        }

        /// <summary>
        /// ����Ӧ�������ȡ���������Ϣ
        /// </summary>
        /// <param name="result">��Ӧ���</param>
        /// <returns>����������</returns>
        protected virtual ErrorCode ExtractErrorCodeFromResponse(ResponseBase result)
        {
            if (result == null)
            {
                return UnifiedErrorCodes.System_InternalError;
            }

            // ����ʹ����Ӧ�е�Ԫ������ȡ����ϸ�Ĵ�����Ϣ
            string detailedMessage = result.Message;

            if (result.Metadata != null)
            {
                // ���Ի�ȡ����ϸ�Ĵ�����Ϣ
                if (result.Metadata.TryGetValue("Exception", out var exceptionObj))
                {
                    detailedMessage = $"{result.Message} | Exception: {exceptionObj}";
                }
            }

            // ֱ��ʹ����Ӧ�еĴ�����봴������������
            return new ErrorCode(result.Code, detailedMessage);
        }

        /// <summary>
        /// ������ǿ�Ĵ�����Ӧ��������������е����д�����Ϣ
        /// </summary>
        /// <param name="session">SuperSocket�Ự</param>
        /// <param name="requestPackage">�������ݰ�</param>
        /// <param name="result">�������</param>
        /// <param name="errorCode">�������</param>
        /// <param name="cancellationToken">ȡ������</param>
        /// <returns>���ͽ������</returns>
        protected virtual async ValueTask SendEnhancedErrorResponseAsync(
            TAppSession session,
            ServerPackageInfo requestPackage,
            ResponseBase result,
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
                    ["OriginalCode"] = result.Code
                }
            };

           

            // ��������ʶ
            if (!string.IsNullOrEmpty(result.RequestId))
            {
                errorResponse.Extensions["RequestId"] = result.RequestId;
            }

            // ���Ԫ�����е����д�����Ϣ
            if (result.Metadata != null && result.Metadata.Count > 0)
            {
                foreach (var metadata in result.Metadata)
                {
                    // �����ظ�����Ѿ����ڵļ�
                    if (!errorResponse.Extensions.ContainsKey(metadata.Key))
                    {
                        errorResponse.Extensions[metadata.Key] = metadata.Value;
                    }
                }
            }

            // ���������а���RequestId��������Ӧ���б��������Ա�ͻ���ƥ���������Ӧ
            if (requestPackage.Packet?.Extensions?.TryGetValue("RequestId", out var requestId) == true)
            {
                errorResponse.Extensions["RequestId"] = requestId;
            }

            // ��¼��ϸ�Ĵ�����Ϣ���ڵ���
            _logger?.LogWarning("������ǿ������Ӧ: ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}, OriginalCode={OriginalCode}, MetadataKeys=[{MetadataKeys}]",
                errorCode.Code, errorCode.Message, result.Code, 
                result.Metadata != null ? string.Join(", ", result.Metadata.Keys) : "none");

            await SendResponseAsync(session, errorResponse, cancellationToken);
        }


    }

    /// <summary>
    /// �Ƿ��Ͱ汾��ͳһSuperSocket�����������������ڲ���Ҫָ���Ự���͵ĳ�����ʹ��
    /// </summary>
    [Command(Key = "SuperSocketCommandAdapter")]
    public class SuperSocketCommandAdapter : SuperSocketCommandAdapter<IAppSession>
    {
        public SuperSocketCommandAdapter(
            CommandDispatcher commandDispatcher,
            CommandPacketAdapter _packetAdapter,
            ICommandFactory commandFactory,
            ILogger<SuperSocketCommandAdapter> logger = null)
            : base(commandDispatcher, _packetAdapter, commandFactory, logger)
        { }
    }
}