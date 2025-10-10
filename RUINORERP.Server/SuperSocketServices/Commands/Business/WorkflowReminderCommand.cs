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
using RUINORERP.Global;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 工作流提醒命令 - 处理工作流相关的提醒功能
    /// 替代旧系统的ReceiveReminderCmd和工作流相关命令
    /// </summary>
    public class WorkflowReminderCommand : IServerCommand
    {
        private readonly ILogger<WorkflowReminderCommand> _logger;
        private readonly ISessionManagerService _sessionManager;
        private readonly IServerSessionEventHandler _eventHandler;

        public WorkflowAction Action { get; set; }
        public ReminderData ReminderData { get; set; }
        public ClientResponseData ResponseData { get; set; }
        public WorkflowBizType WorkflowType { get; set; }

        // ICommand 接口属性
        public string CommandId { get; set; } = Guid.NewGuid().ToString();
        public CmdOperation OperationType { get; set; } = CmdOperation.Receive;
        public OriginalData? DataPacket { get; set; }
        public SessionInfo SessionInfo { get; set; }

        // IServerCommand 接口属性
        public int Priority { get; set; } = 3;
        public string Description => $"工作流提醒: {Action} - {ReminderData?.BizPrimaryKey}";
        public bool RequiresAuthentication => true;
        public int TimeoutMs { get; set; } = 15000;

        public WorkflowReminderCommand(
            ILogger<WorkflowReminderCommand> logger,
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
                   Action != WorkflowAction.Unknown;
        }

        public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"开始处理工作流提醒: Action={Action}, SessionId={SessionInfo?.SessionId}");

                // 验证参数
                var validationResult = ValidateParameters();
                if (!validationResult.IsValid)
                {
                    return CommandResult.CreateError(validationResult.ErrorMessage);
                }

                // 根据动作类型执行相应操作
                var result = Action switch
                {
                    WorkflowAction.AddReminder => await HandleAddReminderAsync(),
                    WorkflowAction.RespondReminder => await HandleRespondReminderAsync(),
                    WorkflowAction.StartWorkflow => await HandleStartWorkflowAsync(),
                    WorkflowAction.ApproveWorkflow => await HandleApproveWorkflowAsync(),
                    WorkflowAction.WorkflowCommand => await HandleWorkflowCommandAsync(),
                    _ => CommandResult.CreateError($"不支持的工作流动作: {Action}")
                };

                if (result.Success)
                {
                    _logger.LogInformation($"工作流提醒处理成功: Action={Action}");
                }
                else
                {
                    _logger.LogWarning($"工作流提醒处理失败: Action={Action}, Error={result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"工作流提醒处理异常: Action={Action}");
                return CommandResult.CreateError("工作流提醒处理异常");
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

                // 根据命令类型确定动作
                Action = (ClientCommand)data.Cmd switch
                {
                    ClientCommand.工作流提醒请求 => WorkflowAction.AddReminder,
                    ClientCommand.工作流提醒回复 => WorkflowAction.RespondReminder,
                    ClientCommand.工作流启动 => WorkflowAction.StartWorkflow,
                    ClientCommand.工作流审批 => WorkflowAction.ApproveWorkflow,
                    ClientCommand.工作流指令 => WorkflowAction.WorkflowCommand,
                    ClientCommand.复合型工作流请求 => DetermineActionFromData(data),
                    _ => WorkflowAction.Unknown
                };

                return ParseDataByAction(data);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析工作流提醒数据包失败");
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

                switch (Action)
                {
                    case WorkflowAction.AddReminder:
                        BuildAddReminderPacket(tx, request);
                        break;
                    case WorkflowAction.RespondReminder:
                        BuildRespondReminderPacket(tx, request);
                        break;
                    case WorkflowAction.StartWorkflow:
                        BuildStartWorkflowPacket(tx, request);
                        break;
                    default:
                        BuildDefaultPacket(tx, request);
                        break;
                }

                DataPacket = new OriginalData
                {
                    Cmd = GetCommandByteForAction(),
                    One = GetCommandOneByteForAction(),
                    Two = tx.toByte()
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建工作流提醒数据包失败");
            }
        }

        public ValidationResult ValidateParameters()
        {
            if (SessionInfo?.IsAuthenticated != true)
                return ValidationResult.Failure("用户未认证");

            switch (Action)
            {
                case WorkflowAction.AddReminder:
                    if (ReminderData == null)
                        return ValidationResult.Failure("提醒数据不能为空");
                    if (ReminderData.ReceiverEmployeeIDs == null || !ReminderData.ReceiverEmployeeIDs.Any())
                        return ValidationResult.Failure("接收者不能为空");
                    break;

                case WorkflowAction.RespondReminder:
                    if (ResponseData == null)
                        return ValidationResult.Failure("响应数据不能为空");
                    break;
            }

            return ValidationResult.Success();
        }

        public string GetCommandId()
        {
            return CommandId;
        }

        #region 私有方法

        /// <summary>
        /// 从数据中确定动作类型
        /// </summary>
        private WorkflowAction DetermineActionFromData(OriginalData data)
        {
            try
            {
                // 根据data.One的内容或其他标识来确定具体动作
                if (data.One != null && data.One.Length > 0)
                {
                    var actionType = (WorkflowAction)data.One[0];
                    return actionType;
                }
            }
            catch
            {
                // 忽略解析错误，返回默认值
            }
            return WorkflowAction.AddReminder; // 默认为添加提醒
        }

        /// <summary>
        /// 根据动作解析数据
        /// </summary>
        private bool ParseDataByAction(OriginalData data)
        {
            int index = 0;
            string sendTime = ByteDataAnalysis.GetString(data.Two, ref index);

            switch (Action)
            {
                case WorkflowAction.AddReminder:
                case WorkflowAction.RespondReminder:
                    return ParseReminderData(data.Two, ref index);

                case WorkflowAction.StartWorkflow:
                    return ParseStartWorkflowData(data.Two, ref index);

                case WorkflowAction.ApproveWorkflow:
                    return ParseApprovalData(data.Two, ref index);

                default:
                    return true;
            }
        }

        /// <summary>
        /// 解析提醒数据
        /// </summary>
        private bool ParseReminderData(byte[] data, ref int index)
        {
            try
            {
                string json = ByteDataAnalysis.GetString(data, ref index);
                JObject obj = JObject.Parse(json);

                if (Action == WorkflowAction.AddReminder)
                {
                    ReminderData = obj.ToObject<ReminderData>();
                }
                else if (Action == WorkflowAction.RespondReminder)
                {
                    ResponseData = obj.ToObject<ClientResponseData>();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析提醒数据失败");
                return false;
            }
        }

        /// <summary>
        /// 解析启动工作流数据
        /// </summary>
        private bool ParseStartWorkflowData(byte[] data, ref int index)
        {
            try
            {
                int workflowTypeInt = ByteDataAnalysis.GetInt(data, ref index);
                WorkflowType = (WorkflowBizType)workflowTypeInt;

                if (WorkflowType == WorkflowBizType.基础数据信息推送)
                {
                    string tableName = ByteDataAnalysis.GetString(data, ref index);
                    // 可以将tableName存储到某个属性中
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析启动工作流数据失败");
                return false;
            }
        }

        /// <summary>
        /// 解析审批数据
        /// </summary>
        private bool ParseApprovalData(byte[] data, ref int index)
        {
            try
            {
                long billID = ByteDataAnalysis.GetInt64(data, ref index);
                int bizType = ByteDataAnalysis.GetInt(data, ref index);
                bool approvalResults = ByteDataAnalysis.Getbool(data, ref index);
                string opinion = ByteDataAnalysis.GetString(data, ref index);

                // 创建审批相关的数据对象
                // TODO: 根据实际需要创建相应的数据结构

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "解析审批数据失败");
                return false;
            }
        }

        /// <summary>
        /// 构建添加提醒数据包
        /// </summary>
        private void BuildAddReminderPacket(ByteBuff tx, object request)
        {
            var reminderData = request as ReminderData ?? ReminderData;
            if (reminderData != null)
            {
                string json = JsonConvert.SerializeObject(reminderData, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                tx.PushString(json);
            }
        }

        /// <summary>
        /// 构建响应提醒数据包
        /// </summary>
        private void BuildRespondReminderPacket(ByteBuff tx, object request)
        {
            var responseData = request as ClientResponseData ?? ResponseData;
            if (responseData != null)
            {
                string json = JsonConvert.SerializeObject(responseData, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                tx.PushString(json);
            }
        }

        /// <summary>
        /// 构建启动工作流数据包
        /// </summary>
        private void BuildStartWorkflowPacket(ByteBuff tx, object request)
        {
            tx.PushInt((int)WorkflowType);
            if (request is string tableName)
            {
                tx.PushString(tableName);
            }
        }

        /// <summary>
        /// 构建默认数据包
        /// </summary>
        private void BuildDefaultPacket(ByteBuff tx, object request)
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

        /// <summary>
        /// 根据动作获取命令字节
        /// </summary>
        private byte GetCommandByteForAction()
        {
            return Action switch
            {
                WorkflowAction.AddReminder => (byte)ClientCommand.工作流提醒请求,
                WorkflowAction.RespondReminder => (byte)ClientCommand.工作流提醒回复,
                WorkflowAction.StartWorkflow => (byte)ClientCommand.工作流启动,
                WorkflowAction.ApproveWorkflow => (byte)ClientCommand.工作流审批,
                WorkflowAction.WorkflowCommand => (byte)ClientCommand.工作流指令,
                _ => (byte)ClientCommand.复合型工作流请求
            };
        }

        /// <summary>
        /// 根据动作获取命令One字节
        /// </summary>
        private byte[] GetCommandOneByteForAction()
        {
            return Action switch
            {
                WorkflowAction.AddReminder => new byte[] { (byte)BizType.CRM跟进计划 },
                WorkflowAction.RespondReminder => new byte[] { (byte)BizType.CRM跟进计划 },
                _ => new byte[] { (byte)Action }
            };
        }

        #endregion

        #region 业务处理方法

        /// <summary>
        /// 处理添加提醒
        /// </summary>
        private async Task<CommandResult> HandleAddReminderAsync()
        {
            try
            {
                _logger.LogInformation($"处理添加提醒: BizKey={ReminderData.BizPrimaryKey}");

                // 验证提醒时间（必须是未来时间且至少3分钟后）
                if (ReminderData.StartTime <= DateTime.Now.AddMinutes(3) || 
                    ReminderData.EndTime < DateTime.Now)
                {
                    return CommandResult.CreateError("提醒时间必须是未来时间且至少3分钟后");
                }

                // 验证接收者
                if (ReminderData.ReceiverEmployeeIDs?.Count == 0)
                {
                    return CommandResult.CreateError("接收者列表不能为空");
                }

                // TODO: 将提醒数据保存到服务器缓存或数据库
                // frmMain.Instance.ReminderBizDataList.AddOrUpdate(ReminderData.BizPrimaryKey, ReminderData, (key, value) => value);

                _logger.LogInformation($"工作流提醒添加成功: {ReminderData.BizPrimaryKey}");
                
                await Task.CompletedTask;
                return CommandResult.CreateSuccess("工作流提醒添加成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理添加提醒时出错");
                return CommandResult.CreateError("添加提醒失败");
            }
        }

        /// <summary>
        /// 处理响应提醒
        /// </summary>
        private async Task<CommandResult> HandleRespondReminderAsync()
        {
            try
            {
                _logger.LogInformation($"处理响应提醒: BizKey={ResponseData.BizPrimaryKey}, Status={ResponseData.Status}");

                // TODO: 更新服务器缓存中的提醒状态
                // 这里需要根据ResponseData更新对应的ReminderData

                await Task.CompletedTask;
                return CommandResult.CreateSuccess("工作流提醒响应处理成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理响应提醒时出错");
                return CommandResult.CreateError("响应提醒失败");
            }
        }

        /// <summary>
        /// 处理启动工作流
        /// </summary>
        private async Task<CommandResult> HandleStartWorkflowAsync()
        {
            try
            {
                _logger.LogInformation($"处理启动工作流: Type={WorkflowType}");

                switch (WorkflowType)
                {
                    case WorkflowBizType.基础数据信息推送:
                        // TODO: 启动基础数据推送工作流
                        break;
                    default:
                        return CommandResult.CreateError($"不支持的工作流类型: {WorkflowType}");
                }

                await Task.CompletedTask;
                return CommandResult.CreateSuccess("工作流启动成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理启动工作流时出错");
                return CommandResult.CreateError("启动工作流失败");
            }
        }

        /// <summary>
        /// 处理审批工作流
        /// </summary>
        private async Task<CommandResult> HandleApproveWorkflowAsync()
        {
            try
            {
                _logger.LogInformation($"处理审批工作流");

                // TODO: 实现工作流审批逻辑
                // 调用WorkflowCore相关API

                await Task.CompletedTask;
                return CommandResult.CreateSuccess("工作流审批处理成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理审批工作流时出错");
                return CommandResult.CreateError("审批工作流失败");
            }
        }

        /// <summary>
        /// 处理工作流指令
        /// </summary>
        private async Task<CommandResult> HandleWorkflowCommandAsync()
        {
            try
            {
                _logger.LogInformation($"处理工作流指令");

                // TODO: 实现工作流指令处理逻辑

                await Task.CompletedTask;
                return CommandResult.CreateSuccess("工作流指令处理成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理工作流指令时出错");
                return CommandResult.CreateError("工作流指令处理失败");
            }
        }

        #endregion
    }

    /// <summary>
    /// 工作流动作枚举
    /// </summary>
    public enum WorkflowAction
    {
        Unknown = 0,
        AddReminder = 1,      // 添加提醒
        RespondReminder = 2,  // 响应提醒
        StartWorkflow = 3,    // 启动工作流
        ApproveWorkflow = 4,  // 审批工作流
        WorkflowCommand = 5   // 工作流指令
    }

    /// <summary>
    /// 工作流业务类型枚举
    /// </summary>
    public enum WorkflowBizType
    {
        基础数据信息推送 = 0,
        单据审批流程 = 1,
        提醒通知流程 = 2
    }
}