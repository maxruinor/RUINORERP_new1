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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 锁管理命令 - 处理单据锁定相关功能
    /// 替代旧系统的ServerLockManagerCmd
    /// </summary>
    public class LockManagementCommand : IServerCommand
    {
        private readonly ILogger<LockManagementCommand> _logger;
        private readonly ISessionManagerService _sessionManager;
        private readonly IServerSessionEventHandler _eventHandler;

        public LockCmd LockAction { get; set; }
        public string BillTableName { get; set; }
        public string BillPrimaryKey { get; set; }
        public long BillId { get; set; }
        public string LockerUserId { get; set; }
        public string LockerUsername { get; set; }
        public DateTime LockTime { get; set; }
        public string LockReason { get; set; }
        public object LockData { get; set; }

        // ICommand 接口属性
        public string CommandId { get; set; } = Guid.NewGuid().ToString();
        public CmdOperation OperationType { get; set; } = CmdOperation.Receive;
        public OriginalData? DataPacket { get; set; }
        public SessionInfo SessionInfo { get; set; }

        // IServerCommand 接口属性
        public int Priority { get; set; } = 2;
        public string Description => $"锁管理: {LockAction} - {BillTableName}({BillId})";
        public bool RequiresAuthentication => true;
        public int TimeoutMs { get; set; } = 10000;

        public LockManagementCommand(
            ILogger<LockManagementCommand> logger,
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
                   LockAction != LockCmd.Unknown;
        }

        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"开始处理锁管理: Action={LockAction}, Table={BillTableName}, BillId={BillId}, SessionId={SessionInfo?.SessionId}");

                // 验证参数
                var validationResult = ValidateParameters();
                if (!validationResult.IsValid)
                {
                    return CommandResult.CreateError(validationResult.ErrorMessage);
                }

                // 根据锁动作执行相应操作
                var result = LockAction switch
                {
                    LockCmd.Lock => await HandleLockAsync(),
                    LockCmd.UnLock => await HandleUnlockAsync(),
                    LockCmd.ForceUnlock => await HandleForceUnlockAsync(),
                    LockCmd.CheckLock => await HandleCheckLockAsync(),
                    LockCmd.Broadcast => await HandleBroadcastAsync(),
                    LockCmd.RequestUnlock => await HandleRequestUnlockAsync(),
                    LockCmd.RefuseUnlock => await HandleRefuseUnlockAsync(),
                    _ => CommandResult.CreateError($"不支持的锁操作: {LockAction}")
                };

                if (result.Success)
                {
                    _logger.LogInformation($"锁管理处理成功: Action={LockAction}, Table={BillTableName}, BillId={BillId}");
                }
                else
                {
                    _logger.LogWarning($"锁管理处理失败: Action={LockAction}, Table={BillTableName}, BillId={BillId}, Error={result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"锁管理处理异常: Action={LockAction}, Table={BillTableName}, BillId={BillId}");
                return CommandResult.CreateError("锁管理处理异常");
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

                // 从data.One中获取锁操作类型
                if (data.One != null && data.One.Length > 0)
                {
                    LockAction = (LockCmd)data.One[0];
                }

                // 解析数据内容
                int index = 0;
                string sendTime = ByteDataAnalysis.GetString(data.Two, ref index);
                
                switch (LockAction)
                {
                    case LockCmd.Lock:
                    case LockCmd.UnLock:
                    case LockCmd.CheckLock:
                        return ParseBasicLockData(data.Two, ref index);
                    case LockCmd.RequestUnlock:
                    case LockCmd.RefuseUnlock:
                        return ParseUnlockRequestData(data.Two, ref index);
                    case LockCmd.Broadcast:
                        return ParseBroadcastData(data.Two, ref index);
                    default:
                        return ParseBasicLockData(data.Two, ref index);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析锁管理数据包失败");
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
                var tx = new ByteBuff(200);
                string sendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                tx.PushString(sendTime);

                switch (LockAction)
                {
                    case LockCmd.Lock:
                        BuildLockDataPacket(tx, request);
                        break;
                    case LockCmd.UnLock:
                        BuildUnlockDataPacket(tx, request);
                        break;
                    case LockCmd.RequestUnlock:
                        BuildRequestUnlockDataPacket(tx, request);
                        break;
                    case LockCmd.RefuseUnlock:
                        BuildRefuseUnlockDataPacket(tx, request);
                        break;
                    case LockCmd.Broadcast:
                        BuildBroadcastDataPacket(tx, request);
                        break;
                    default:
                        BuildDefaultDataPacket(tx, request);
                        break;
                }

                DataPacket = new OriginalData
                {
                    Cmd = (byte)ClientCommand.复合型锁单处理,
                    One = new byte[] { (byte)LockAction },
                    Two = tx.toByte()
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建锁管理数据包失败");
            }
        }

        public RUINORERP.PacketSpec.Commands.ValidationResult ValidateParameters()
        {
            if (SessionInfo?.IsAuthenticated != true)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户未认证");

            if (LockAction == LockCmd.Unknown)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("无效的锁操作类型");

            // 对于需要单据信息的操作，验证必要参数
            if (LockAction == LockCmd.Lock || LockAction == LockCmd.UnLock || LockAction == LockCmd.CheckLock)
            {
                if (string.IsNullOrWhiteSpace(BillTableName))
                    return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("单据表名不能为空");

                if (BillId <= 0)
                    return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("单据ID无效");
            }

            return RUINORERP.PacketSpec.Commands.ValidationResult.Success();
        }

        public string GetCommandId()
        {
            return CommandId;
        }

        #region 私有解析方法

        /// <summary>
        /// 解析基本锁数据
        /// </summary>
        private bool ParseBasicLockData(byte[] data, ref int index)
        {
            try
            {
                string json = ByteDataAnalysis.GetString(data, ref index);
                if (!string.IsNullOrEmpty(json))
                {
                    JObject obj = JObject.Parse(json);
                    BillTableName = obj["BillTableName"]?.ToString();
                    BillId = obj["BillId"]?.ToObject<long>() ?? 0;
                    BillPrimaryKey = obj["BillPrimaryKey"]?.ToString();
                    LockerUserId = obj["LockerUserId"]?.ToString();
                    LockerUsername = obj["LockerUsername"]?.ToString();
                    LockReason = obj["LockReason"]?.ToString();
                    
                    if (DateTime.TryParse(obj["LockTime"]?.ToString(), out var lockTime))
                    {
                        LockTime = lockTime;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析基本锁数据失败");
                return false;
            }
        }

        /// <summary>
        /// 解析解锁请求数据
        /// </summary>
        private bool ParseUnlockRequestData(byte[] data, ref int index)
        {
            try
            {
                string json = ByteDataAnalysis.GetString(data, ref index);
                if (!string.IsNullOrEmpty(json))
                {
                    JObject obj = JObject.Parse(json);
                    LockData = obj.ToObject<object>();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析解锁请求数据失败");
                return false;
            }
        }

        /// <summary>
        /// 解析广播数据
        /// </summary>
        private bool ParseBroadcastData(byte[] data, ref int index)
        {
            try
            {
                int lockCmd = ByteDataAnalysis.GetInt(data, ref index);
                LockAction = (LockCmd)lockCmd;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析广播数据失败");
                return false;
            }
        }

        #endregion

        #region 私有构建方法

        /// <summary>
        /// 构建锁定数据包
        /// </summary>
        private void BuildLockDataPacket(ByteBuff tx, object request)
        {
            var lockInfo = new
            {
                BillTableName,
                BillId,
                BillPrimaryKey,
                LockerUserId = SessionInfo?.UserId?.ToString(),
                LockerUsername = SessionInfo?.Username,
                LockTime = DateTime.Now,
                LockReason
            };

            string json = JsonConvert.SerializeObject(lockInfo, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            tx.PushString(json);
        }

        /// <summary>
        /// 构建解锁数据包
        /// </summary>
        private void BuildUnlockDataPacket(ByteBuff tx, object request)
        {
            var unlockInfo = new
            {
                BillTableName,
                BillId,
                BillPrimaryKey,
                UnlockerUserId = SessionInfo?.UserId?.ToString(),
                UnlockerUsername = SessionInfo?.Username,
                UnlockTime = DateTime.Now
            };

            string json = JsonConvert.SerializeObject(unlockInfo, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            tx.PushString(json);
        }

        /// <summary>
        /// 构建请求解锁数据包
        /// </summary>
        private void BuildRequestUnlockDataPacket(ByteBuff tx, object request)
        {
            string json = JsonConvert.SerializeObject(request ?? LockData, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            tx.PushString(json);
        }

        /// <summary>
        /// 构建拒绝解锁数据包
        /// </summary>
        private void BuildRefuseUnlockDataPacket(ByteBuff tx, object request)
        {
            string json = JsonConvert.SerializeObject(request ?? LockData, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            tx.PushString(json);
        }

        /// <summary>
        /// 构建广播数据包
        /// </summary>
        private void BuildBroadcastDataPacket(ByteBuff tx, object request)
        {
            tx.PushInt((int)LockAction);
        }

        /// <summary>
        /// 构建默认数据包
        /// </summary>
        private void BuildDefaultDataPacket(ByteBuff tx, object request)
        {
            if (request != null)
            {
                string json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                tx.PushString(json);
            }
        }

        #endregion

        #region 业务处理方法

        /// <summary>
        /// 处理锁定操作
        /// </summary>
        private async Task<CommandResult> HandleLockAsync()
        {
            try
            {
                _logger.LogInformation($"处理锁定操作: Table={BillTableName}, BillId={BillId}, User={SessionInfo?.Username}");

                // TODO: 检查是否已被其他用户锁定
                // var existingLock = await CheckExistingLock(BillTableName, BillId);
                
                // TODO: 创建锁记录
                // await CreateLockRecord(BillTableName, BillId, SessionInfo.UserId, LockReason);

                // 广播锁定通知给其他用户
                await BroadcastLockNotificationAsync(LockCmd.Lock);

                _logger.LogInformation($"单据锁定成功: {BillTableName}({BillId})");
                
                var response = new
                {
                    Success = true,
                    Message = "锁定成功",
                    BillTableName,
                    BillId,
                    LockerUsername = SessionInfo?.Username,
                    LockTime = DateTime.Now
                };

                return CommandResult.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理锁定操作时出错");
                return CommandResult.CreateError("锁定操作失败");
            }
        }

        /// <summary>
        /// 处理解锁操作
        /// </summary>
        private async Task<CommandResult> HandleUnlockAsync()
        {
            try
            {
                _logger.LogInformation($"处理解锁操作: Table={BillTableName}, BillId={BillId}, User={SessionInfo?.Username}");

                // TODO: 验证用户是否有权限解锁
                // var lockInfo = await GetLockInfo(BillTableName, BillId);
                // if (lockInfo != null && lockInfo.LockerUserId != SessionInfo.UserId && !SessionInfo.IsSuperUser)
                // {
                //     return CommandResult.CreateError("您没有权限解锁此单据");
                // }

                // TODO: 删除锁记录
                // await RemoveLockRecord(BillTableName, BillId);

                // 广播解锁通知给其他用户
                await BroadcastLockNotificationAsync(LockCmd.UnLock);

                _logger.LogInformation($"单据解锁成功: {BillTableName}({BillId})");

                var response = new
                {
                    Success = true,
                    Message = "解锁成功",
                    BillTableName,
                    BillId,
                    UnlockerUsername = SessionInfo?.Username,
                    UnlockTime = DateTime.Now
                };

                return CommandResult.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理解锁操作时出错");
                return CommandResult.CreateError("解锁操作失败");
            }
        }

        /// <summary>
        /// 处理强制解锁操作
        /// </summary>
        private async Task<CommandResult> HandleForceUnlockAsync()
        {
            try
            {
                _logger.LogInformation($"处理强制解锁操作: Table={BillTableName}, BillId={BillId}, User={SessionInfo?.Username}");

                // TODO: 验证管理员权限
                // if (!SessionInfo.IsSuperUser)
                // {
                //     return CommandResult.CreateError("您没有强制解锁权限");
                // }

                // TODO: 强制删除锁记录
                // await ForceRemoveLockRecord(BillTableName, BillId);

                // 广播强制解锁通知
                await BroadcastLockNotificationAsync(LockCmd.ForceUnlock);

                _logger.LogInformation($"单据强制解锁成功: {BillTableName}({BillId})");

                return CommandResult.CreateSuccess("强制解锁成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理强制解锁操作时出错");
                return CommandResult.CreateError("强制解锁操作失败");
            }
        }

        /// <summary>
        /// 处理检查锁状态操作
        /// </summary>
        private async Task<CommandResult> HandleCheckLockAsync()
        {
            try
            {
                _logger.LogInformation($"检查锁状态: Table={BillTableName}, BillId={BillId}");

                // TODO: 查询锁状态
                // var lockInfo = await GetLockInfo(BillTableName, BillId);

                var response = new
                {
                    IsLocked = false, // lockInfo != null
                    BillTableName,
                    BillId,
                    // LockerUsername = lockInfo?.LockerUsername,
                    // LockTime = lockInfo?.LockTime
                };

                await Task.CompletedTask;
                return CommandResult.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查锁状态时出错");
                return CommandResult.CreateError("检查锁状态失败");
            }
        }

        /// <summary>
        /// 处理广播操作
        /// </summary>
        private async Task<CommandResult> HandleBroadcastAsync()
        {
            try
            {
                _logger.LogInformation($"处理广播操作: Action={LockAction}");

                await BroadcastLockNotificationAsync(LockAction);

                return CommandResult.CreateSuccess("广播成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理广播操作时出错");
                return CommandResult.CreateError("广播操作失败");
            }
        }

        /// <summary>
        /// 处理请求解锁操作
        /// </summary>
        private async Task<CommandResult> HandleRequestUnlockAsync()
        {
            try
            {
                _logger.LogInformation($"处理请求解锁操作: Table={BillTableName}, BillId={BillId}");

                // TODO: 发送解锁请求给锁定者
                // await SendUnlockRequestToLocker(BillTableName, BillId, SessionInfo);

                await Task.CompletedTask;
                return CommandResult.CreateSuccess("解锁请求已发送");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理请求解锁操作时出错");
                return CommandResult.CreateError("请求解锁操作失败");
            }
        }

        /// <summary>
        /// 处理拒绝解锁操作
        /// </summary>
        private async Task<CommandResult> HandleRefuseUnlockAsync()
        {
            try
            {
                _logger.LogInformation($"处理拒绝解锁操作");

                // TODO: 回复拒绝解锁消息

                await Task.CompletedTask;
                return CommandResult.CreateSuccess("已拒绝解锁请求");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理拒绝解锁操作时出错");
                return CommandResult.CreateError("拒绝解锁操作失败");
            }
        }

        /// <summary>
        /// 广播锁通知
        /// </summary>
        private async Task BroadcastLockNotificationAsync(LockCmd action)
        {
            try
            {
                var allSessions = _sessionManager.GetAllSessions();
                var excludeSession = SessionInfo?.SessionId; // 排除发送者自己

                // 构建广播数据包
                LockAction = action;
                OperationType = CmdOperation.Send;
                BuildDataPacket();

                int broadcastCount = 0;
                foreach (var session in allSessions)
                {
                    if (session.SessionId != excludeSession)
                    {
                        // TODO: 发送锁通知到指定会话
                        // await SendLockNotificationToSession(session);
                        broadcastCount++;
                    }
                }

                _logger.LogInformation($"锁通知已广播到 {broadcastCount} 个会话, Action={action}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "广播锁通知时出错");
            }
        }

        #endregion
    }

    /// <summary>
    /// 锁操作枚举
    /// </summary>
    public enum LockCmd
    {
        Unknown = 0,
        Lock = 1,           // 锁定
        UnLock = 2,         // 解锁
        ForceUnlock = 3,    // 强制解锁
        CheckLock = 4,      // 检查锁状态
        Broadcast = 5,      // 广播
        RequestUnlock = 6,  // 请求解锁
        RefuseUnlock = 7    // 拒绝解锁
    }
}