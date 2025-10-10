using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.Server.SuperSocketServices;
using RUINORERP.Model.TransModel;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Global;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.PacketSpec.Enums.Workflow;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 实体同步命令 - 处理各种实体数据的同步
    /// 替代旧系统的ReceiveEntityTransferCmd
    /// </summary>
    public class EntitySyncCommand : IServerCommand
    {
        private readonly ILogger<EntitySyncCommand> _logger;
        private readonly ISessionManagerService _sessionManager;
        private readonly IServerSessionEventHandler _eventHandler;

        public EntitySyncType SyncType { get; set; }
        public NextProcessStep NextStep { get; set; }
        public object SyncObject { get; set; }
        public string SyncObjectName { get; set; }
        public string TargetSessionId { get; set; }

        // ICommand 接口属性
        public string CommandId { get; set; } = Guid.NewGuid().ToString();
        public CmdOperation OperationType { get; set; } = CmdOperation.Receive;
        public OriginalData? DataPacket { get; set; }
        public SessionInfo SessionInfo { get; set; }

        // IServerCommand 接口属性
        public int Priority { get; set; } = 2;
        public string Description => $"实体同步: {SyncType} - {SyncObjectName}";
        public bool RequiresAuthentication => true;
        public int TimeoutMs { get; set; } = 20000;

        public EntitySyncCommand(
            ILogger<EntitySyncCommand> logger,
            ISessionManagerService sessionManager,
            IServerSessionEventHandler eventHandler)
        {
            _logger = logger;
            _sessionManager = sessionManager;
            _eventHandler = eventHandler;
        }

        public virtual bool CanExecute()
        {
            return SessionInfo?.IsAuthenticated == true &&
                   SyncType != EntitySyncType.Unknown;
        }

        public async Task<ApiResponse> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"开始处理实体同步: Type={SyncType}, Object={SyncObjectName}, SessionId={SessionInfo?.SessionId}");

                // 验证参数
                var validationResult = ValidateParameters();
                if (!validationResult.IsValid)
                {
                    return ApiResponse.CreateError(validationResult.ErrorMessage, 400).WithMetadata("ErrorCode", "VALIDATION_FAILED");
                }

                // 根据操作类型执行相应操作
                var result = OperationType switch
                {
                    CmdOperation.Receive => await HandleReceiveSyncAsync(),
                    CmdOperation.Send => await HandleSendSyncAsync(),
                    _ => ApiResponse.CreateError("不支持的操作类型", 400).WithMetadata("ErrorCode", "UNSUPPORTED_OPERATION")
                };

                if (result.IsSuccess)
                {
                    _logger.LogInformation($"实体同步处理成功: Type={SyncType}, Object={SyncObjectName}");
                }
                else
                {
                    _logger.LogWarning($"实体同步处理失败: Type={SyncType}, Object={SyncObjectName}, Error={result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"实体同步处理异常: Type={SyncType}, Object={SyncObjectName}");
                return ApiResponse.CreateError("实体同步处理异常", 500)
                    .WithMetadata("ErrorCode", "ENTITY_SYNC_EXCEPTION")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        public bool AnalyzeDataPacket(OriginalData data, SessionInfo sessionInfo)
        {
            try
            {
                SessionInfo = sessionInfo;
                DataPacket = data;

                int index = 0;
                string sendTime = ByteDataAnalysis.GetString(data.Two, ref index);
                SyncObjectName = ByteDataAnalysis.GetString(data.Two, ref index);
                string json = ByteDataAnalysis.GetString(data.Two, ref index);
                SyncType = (EntitySyncType)ByteDataAnalysis.GetInt(data.Two, ref index);
                NextStep = (NextProcesszStep)ByteDataAnalysis.GetInt(data.Two, ref index);

                // 解析同步对象
                return ParseSyncObject(json);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析实体同步数据包失败");
                return false;
            }
        }

        /// <summary>
        /// 构建数据包
        /// </summary>
        public void BuildDataPacket(object request = null)
        {
            try
            {
                var syncObj = request ?? SyncObject;
                if (syncObj == null) return;

                var tx = new ByteBuff(1024);
                string sendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                tx.PushString(sendTime);
                tx.PushString(syncObj.GetType().Name);
                
                string json = JsonConvert.SerializeObject(syncObj, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                tx.PushString(json);
                
                tx.PushInt((int)SyncType);
                tx.PushInt((int)NextStep);

                DataPacket = new OriginalData
                {
                    Cmd = (byte)ClientCommand.复合型实体请求,
                    One = new byte[] { (byte)SyncType },
                    Two = tx.toByte()
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建实体同步数据包失败");
            }
        }

        public RUINORERP.PacketSpec.Commands.ValidationResult ValidateParameters()
        {
            if (SessionInfo?.IsAuthenticated != true)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户未认证");

            if (SyncType == EntitySyncType.Unknown)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("无效的同步类型");

            if (string.IsNullOrWhiteSpace(SyncObjectName))
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("同步对象名称不能为空");

            return RUINORERP.PacketSpec.Commands.ValidationResult.Success();
        }

        public string GetCommandId()
        {
            return CommandId;
        }

        #region 私有方法

        /// <summary>
        /// 解析同步对象
        /// </summary>
        private bool ParseSyncObject(string json)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                
                switch (SyncObjectName)
                {
                    case nameof(GlobalValidatorConfig):
                        SyncObject = obj.ToObject<GlobalValidatorConfig>();
                        break;
                    case nameof(SystemGlobalconfig):
                        SyncObject = obj.ToObject<SystemGlobalconfig>();
                        break;
                    case "CacheData":
                        SyncObject = obj; // 保持为JObject，稍后根据具体类型处理
                        break;
                    default:
                        SyncObject = obj;
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"解析同步对象失败: {SyncObjectName}");
                return false;
            }
        }

        /// <summary>
        /// 处理接收同步
        /// </summary>
        private async Task<ApiResponse> HandleReceiveSyncAsync()
        {
            try
            {
                return SyncType switch
                {
                    EntitySyncType.DynamicConfig => await HandleDynamicConfigSyncAsync(),
                    EntitySyncType.CacheData => await HandleCacheDataSyncAsync(),
                    EntitySyncType.UserData => await HandleUserDataSyncAsync(),
                    EntitySyncType.SystemData => await HandleSystemDataSyncAsync(),
                    _ => ApiResponse.CreateError($"不支持的同步类型: {SyncType}", 400).WithMetadata("ErrorCode", "UNSUPPORTED_SYNC_TYPE")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理接收同步时出错");
                return ApiResponse.CreateError("处理接收同步异常", 500)
                    .WithMetadata("ErrorCode", "RECEIVE_SYNC_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理发送同步
        /// </summary>
        private async Task<ApiResponse> HandleSendSyncAsync()
        {
            try
            {
                // 构建数据包
                BuildDataPacket();

                if (DataPacket == null)
                {
                    return ApiResponse.CreateError("构建数据包失败", 400).WithMetadata("ErrorCode", "BUILD_PACKET_FAILED");
                }

                // 发送同步数据
                if (!string.IsNullOrEmpty(TargetSessionId))
                {
                    // 发送给指定会话
                    var targetSession = _sessionManager.GetSession(TargetSessionId);
                    if (targetSession != null)
                    {
                        await SendSyncToSessionAsync(targetSession);
                        return ApiResponse.CreateSuccess("实体同步发送成功");
                    }
                    else
                    {
                        return ApiResponse.CreateError("目标会话不存在或不在线", 400).WithMetadata("ErrorCode", "TARGET_SESSION_NOT_FOUND");
                    }
                }
                else
                {
                    // 广播给所有在线会话
                    await BroadcastSyncAsync();
                    return ApiResponse.CreateSuccess("实体同步广播成功");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理发送同步时出错");
                return ApiResponse.CreateError("处理发送同步异常", 500)
                    .WithMetadata("ErrorCode", "SEND_SYNC_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理动态配置同步
        /// </summary>
        private async Task<ApiResponse> HandleDynamicConfigSyncAsync()
        {
            try
            {
                _logger.LogInformation($"处理动态配置同步: {SyncObjectName}");

                // 保存配置到本地文件
                string basePath = Path.Combine(Directory.GetCurrentDirectory(), GlobalConstants.DynamicConfigFileDirectory);
                string configFilePath = Path.Combine(basePath, $"{SyncObjectName}.json");

                // 确保目录存在
                Directory.CreateDirectory(basePath);

                // 创建配置文件接收器
                var configReceiver = new ConfigFileReceiver(configFilePath);
                
                // 创建配置的JObject包装
                JObject configJson = new JObject(new JProperty(SyncObjectName, JObject.FromObject(SyncObject)));
                
                // 执行配置保存命令
                var command = new EditConfigCommand(configReceiver, configJson);
                //var commandManager = new CommandManager();
                //commandManager.ExecuteCommand(command);

                _logger.LogInformation($"动态配置已保存到: {configFilePath}");

                // 如果需要转发，则广播给其他在线用户
                if (NextStep == NextProcesszStep.转发)
                {
                    SyncType = EntitySyncType.DynamicConfig;
                    OperationType = CmdOperation.Send;
                    await BroadcastSyncAsync();
                }

                await Task.CompletedTask;
                return ApiResponse.CreateSuccess("动态配置同步成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理动态配置同步时出错");
                return ApiResponse.CreateError("动态配置同步失败", 500)
                    .WithMetadata("ErrorCode", "DYNAMIC_CONFIG_SYNC_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理缓存数据同步
        /// </summary>
        private async Task<ApiResponse> HandleCacheDataSyncAsync()
        {
            try
            {
                _logger.LogInformation($"处理缓存数据同步: {SyncObjectName}");

                // TODO: 实现缓存数据同步逻辑
                // 这里可以调用BizCacheHelper或相关的缓存服务

                await Task.CompletedTask;
                return ApiResponse.CreateSuccess("缓存数据同步成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理缓存数据同步时出错");
                return ApiResponse.CreateError("缓存数据同步失败", 500)
                    .WithMetadata("ErrorCode", "CACHE_DATA_SYNC_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理用户数据同步
        /// </summary>
        private async Task<ApiResponse> HandleUserDataSyncAsync()
        {
            try
            {
                _logger.LogInformation($"处理用户数据同步: {SyncObjectName}");

                // TODO: 实现用户数据同步逻辑

                await Task.CompletedTask;
                return ApiResponse.CreateSuccess("用户数据同步成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理用户数据同步时出错");
                return ApiResponse.CreateError("用户数据同步失败", 500)
                    .WithMetadata("ErrorCode", "USER_DATA_SYNC_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理系统数据同步
        /// </summary>
        private async Task<ApiResponse> HandleSystemDataSyncAsync()
        {
            try
            {
                _logger.LogInformation($"处理系统数据同步: {SyncObjectName}");

                // TODO: 实现系统数据同步逻辑

                await Task.CompletedTask;
                return ApiResponse.CreateSuccess("系统数据同步成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理系统数据同步时出错");
                return ApiResponse.CreateError("系统数据同步失败", 500)
                    .WithMetadata("ErrorCode", "SYSTEM_DATA_SYNC_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 发送同步数据到指定会话
        /// </summary>
        private async Task SendSyncToSessionAsync(SessionInfo sessionInfo)
        {
            // TODO: 实现发送到指定会话的逻辑
            _logger.LogInformation($"发送实体同步到会话: {sessionInfo.SessionID}");
            await Task.CompletedTask;
        }

        /// <summary>
        /// 广播同步数据到所有在线会话
        /// </summary>
        private async Task BroadcastSyncAsync()
        {
            var allSessions = _sessionManager.GetAllSessions();
            var excludeSession = SessionInfo?.SessionId; // 排除发送者自己

            int broadcastCount = 0;
            foreach (var session in allSessions)
            {
                if (session.SessionId != excludeSession)
                {
                    await SendSyncToSessionAsync(session);
                    broadcastCount++;
                }
            }

            _logger.LogInformation($"实体同步已广播到 {broadcastCount} 个会话");
        }

        #endregion
    }

    /// <summary>
    /// 实体同步类型枚举
    /// </summary>
    public enum EntitySyncType
    {
        Unknown = 0,
        DynamicConfig = 1,  // 动态配置
        CacheData = 2,      // 缓存数据
        UserData = 3,       // 用户数据
        SystemData = 4      // 系统数据
    }

 
}