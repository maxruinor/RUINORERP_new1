using Microsoft.Extensions.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.BizService;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WorkflowCore.Interface;

namespace RUINORERP.Server.Workflow
{
    /// <summary>
    /// 提醒工作流调度器
    /// 负责定期检查提醒数据并自动启动相应的工作流
    /// </summary>
    public class ReminderWorkflowScheduler
    {
        private readonly ILogger<ReminderWorkflowScheduler> _logger;
        private readonly IWorkflowHost _workflowHost;
        private readonly DataServiceChannel _dataService;
        private Timer _checkTimer;
        private bool _isRunning = false;

        /// <summary>
        /// 检查间隔（分钟）
        /// </summary>
        private const int CHECK_INTERVAL_MINUTES = 1;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workflowHost">工作流主机</param>
        /// <param name="dataService">数据服务</param>
        /// <param name="logger">日志记录器</param>
        public ReminderWorkflowScheduler(IWorkflowHost workflowHost, DataServiceChannel dataService, ILogger<ReminderWorkflowScheduler> logger = null)
        {
            _workflowHost = workflowHost ?? throw new ArgumentNullException(nameof(workflowHost));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _logger = logger ?? Startup.GetFromFac<ILogger<ReminderWorkflowScheduler>>();
        }

        /// <summary>
        /// 启动调度器
        /// </summary>
        public void Start()
        {
            if (_isRunning)
            {
                _logger.LogWarning("提醒工作流调度器已经在运行中");
                return;
            }

            try
            {
                _isRunning = true;
                
                // 立即执行一次检查
                Task.Run(() => CheckAndStartReminderWorkflowsAsync());
                
                // 启动定时器，每分钟检查一次
                _checkTimer = new Timer(
                    async _ => await CheckAndStartReminderWorkflowsAsync(),
                    null,
                    TimeSpan.Zero,
                    TimeSpan.FromMinutes(CHECK_INTERVAL_MINUTES)
                );

                _logger.LogInformation("提醒工作流调度器已启动，检查间隔: {Interval}分钟", CHECK_INTERVAL_MINUTES);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动提醒工作流调度器时发生异常");
                _isRunning = false;
            }
        }

        /// <summary>
        /// 停止调度器
        /// </summary>
        public void Stop()
        {
            if (!_isRunning)
            {
                return;
            }

            try
            {
                _isRunning = false;
                _checkTimer?.Dispose();
                _checkTimer = null;
                
                _logger.LogInformation("提醒工作流调度器已停止");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "停止提醒工作流调度器时发生异常");
            }
        }

        /// <summary>
        /// 检查并启动提醒工作流
        /// </summary>
        private async Task CheckAndStartReminderWorkflowsAsync()
        {
            if (!_isRunning)
            {
                return;
            }

            try
            {
                _logger.LogDebug("开始检查需要提醒的业务数据");

                // 获取主窗体实例中的提醒数据列表
                var mainForm = Startup.GetFromFac<frmMainNew>();
                if (mainForm?.ReminderBizDataList == null)
                {
                    _logger.LogWarning("无法获取提醒数据列表，主窗体或数据列表为空");
                    return;
                }

                // 每10分钟刷新一次数据（避免频繁查询数据库）
                if (DateTime.Now.Minute % 20 == 0) // 每10分钟刷新一次
                {
                    await RefreshCRMFollowUpPlansDataAsync(mainForm.ReminderBizDataList);
                }

                var reminderDataList = mainForm.ReminderBizDataList;
                
                // 检查每个提醒数据是否需要启动工作流
                foreach (var reminderData in reminderDataList.Values)
                {
                    await ProcessReminderDataAsync(reminderData);
                }

                _logger.LogDebug("提醒数据检查完成，共检查 {Count} 条数据", reminderDataList.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查提醒数据时发生异常");
            }
        }

        /// <summary>
        /// 刷新CRM跟进计划数据
        /// </summary>
        /// <param name="reminderDataList">提醒数据列表</param>
        private async Task RefreshCRMFollowUpPlansDataAsync(ConcurrentDictionary<long, ReminderData> reminderDataList)
        {
            try
            {
                _logger.LogDebug("开始刷新CRM跟进计划数据");
                
                // 重新加载数据
                _dataService.LoadCRMFollowUpPlansData(reminderDataList);
                
                // 清理已过期或已完成的提醒数据
                var expiredKeys = reminderDataList.Where(kv => 
                    kv.Value.EndTime < DateTime.Now || 
                    kv.Value.IsRead).Select(kv => kv.Key).ToList();
                
                foreach (var key in expiredKeys)
                {
                    reminderDataList.TryRemove(key, out _);
                }
                
                _logger.LogDebug("CRM跟进计划数据刷新完成，清理了 {Count} 条过期数据", expiredKeys.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刷新CRM跟进计划数据时发生异常");
            }
        }

        /// <summary>
        /// 处理单个提醒数据
        /// </summary>
        /// <param name="reminderData">提醒数据</param>
        private async Task ProcessReminderDataAsync(ReminderData reminderData)
        {
            try
            {
                // 检查数据是否有效
                if (reminderData == null)
                {
                    return;
                }

                // 检查是否已经启动过工作流（通过检查工作流ID）
                if (!string.IsNullOrEmpty(reminderData.WorkflowId))
                {
                    // 工作流已启动，跳过处理
                    return;
                }

                // 检查提醒时间是否已到
                if (reminderData.StartTime > DateTime.Now)
                {
                    // 提醒时间未到，跳过处理
                    return;
                }

                // 检查提醒是否已过期
                if (reminderData.EndTime < DateTime.Now)
                {
                    // 提醒已过期，标记为已处理
                    reminderData.IsRead = true;
                    _logger.LogDebug("提醒数据已过期，标记为已处理 - 业务ID: {BizKeyID}", reminderData.BizKeyID);
                    return;
                }

                // 检查是否已读
                if (reminderData.IsRead)
                {
                    // 提醒已处理，跳过
                    return;
                }

                // 启动提醒工作流
                await StartReminderWorkflowAsync(reminderData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "处理提醒数据时发生异常 - 业务ID: {BizKeyID}", reminderData?.BizKeyID);
            }
        }

        /// <summary>
        /// 启动提醒工作流
        /// </summary>
        /// <param name="reminderData">提醒数据</param>
        private async Task StartReminderWorkflowAsync(ReminderData reminderData)
        {
            try
            {
                // 设置工作流数据
                var workflowData = new ReminderData
                {
                    BizPrimaryKey = reminderData.BizPrimaryKey,
                    BizKeyID = reminderData.BizKeyID,
                    BizData = reminderData.BizData,
                    RemindSubject = reminderData.RemindSubject ?? "CRM客户跟进提醒",
                    ReminderContent = reminderData.ReminderContent ?? $"您有客户跟进计划需要处理，业务ID: {reminderData.BizKeyID}",
                    StartTime = reminderData.StartTime,
                    EndTime = reminderData.EndTime,
                    RemindInterval = reminderData.RemindInterval,
                    ReceiverUserIDs = reminderData.ReceiverUserIDs,
                    EntityType = reminderData.EntityType,
                    SendTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                // 启动工作流
                var workflowId = await _workflowHost.StartWorkflow("ReminderWorkflow", workflowData);
                
                if (!string.IsNullOrEmpty(workflowId))
                {
                    // 保存工作流ID到提醒数据中
                    reminderData.WorkflowId = workflowId;
                    
                    _logger.LogInformation("提醒工作流启动成功 - 业务ID: {BizKeyID}, 工作流ID: {WorkflowId}", 
                        reminderData.BizKeyID, workflowId);
                }
                else
                {
                    _logger.LogWarning("提醒工作流启动失败 - 业务ID: {BizKeyID}", reminderData.BizKeyID);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "启动提醒工作流时发生异常 - 业务ID: {BizKeyID}", reminderData.BizKeyID);
            }
        }

        /// <summary>
        /// 手动触发提醒检查（用于测试）
        /// </summary>
        public async Task TriggerManualCheckAsync()
        {
            await CheckAndStartReminderWorkflowsAsync();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Stop();
            _checkTimer?.Dispose();
        }
    }
}