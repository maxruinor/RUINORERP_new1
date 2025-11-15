using RUINORERP.Global;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 任务状态监控器
    /// 负责监控数据库中任务状态的变化并触发更新通知
    /// </summary>
    public class TaskStatusMonitor : IExcludeFromRegistration
    {
        #region 单例模式
        private static readonly Lazy<TaskStatusMonitor> _instance = 
            new Lazy<TaskStatusMonitor>(() => new TaskStatusMonitor());

        /// <summary>
        /// 获取TaskStatusMonitor的单例实例
        /// </summary>
        public static TaskStatusMonitor Instance => _instance.Value;

        private TaskStatusMonitor()
        {
            _statusHistory = new ConcurrentDictionary<string, string>();
            _monitoredBusinessTypes = new List<BizType>();
        }
        #endregion

        #region 字段
        private readonly ConcurrentDictionary<string, string> _statusHistory; // 记录任务状态历史，键为"TaskId_BizType"
        private readonly List<BizType> _monitoredBusinessTypes; // 监控的业务类型列表
        private bool _isMonitoring = false;
        private readonly object _monitorLock = new object();
        #endregion

        #region 公共方法

        /// <summary>
        /// 开始监控指定的业务类型
        /// </summary>
        /// <param name="businessTypes">要监控的业务类型列表</param>
        public void StartMonitoring(List<BizType> businessTypes)
        {
            if (businessTypes == null || !businessTypes.Any())
                return;

            lock (_monitorLock)
            {
                // 添加新的监控业务类型
                foreach (var bizType in businessTypes)
                {
                    if (!_monitoredBusinessTypes.Contains(bizType))
                    {
                        _monitoredBusinessTypes.Add(bizType);
                    }
                }

                // 如果还未开始监控，则启动监控
                if (!_isMonitoring)
                {
                    _isMonitoring = true;
                    InitializeStatusHistory();
                    StartMonitoringTask();
                }
            }
        }

        /// <summary>
        /// 停止监控指定的业务类型
        /// </summary>
        /// <param name="businessTypes">要停止监控的业务类型列表</param>
        public void StopMonitoring(List<BizType> businessTypes)
        {
            if (businessTypes == null || !businessTypes.Any())
                return;

            lock (_monitorLock)
            {
                // 移除指定的业务类型
                foreach (var bizType in businessTypes)
                {
                    _monitoredBusinessTypes.Remove(bizType);
                }

                // 如果没有监控的业务类型了，停止监控
                if (!_monitoredBusinessTypes.Any())
                {
                    _isMonitoring = false;
                }
            }
        }

        /// <summary>
        /// 手动触发任务状态变更通知
        /// 当其他模块修改了任务状态时，可以调用此方法通知监控器
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        public void NotifyTaskStatusChanged(string taskId, BizType bizType, string oldStatus, string newStatus)
        {
            if (string.IsNullOrEmpty(taskId) || bizType == BizType.无对应数据 || oldStatus == newStatus)
                return;

            var taskKey = $"{taskId}_{bizType}";
            _statusHistory[taskKey] = newStatus;

            // 发布状态更新通知
            var update = new TaskStatusUpdate
            {
                UpdateType = TaskStatusUpdateType.StatusChanged,
                BusinessType = bizType,
                TaskId = taskId,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                Timestamp = DateTime.Now
            };

            TaskStatusSyncManager.Instance.PublishUpdate(update);
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化状态历史
        /// 首次启动监控时，从数据库加载当前任务状态
        /// </summary>
        private void InitializeStatusHistory()
        {
            try
            {
                foreach (var bizType in _monitoredBusinessTypes)
                {
                    LoadCurrentTaskStatus(bizType);
                }
            }
            catch (Exception ex)
            {
                // 记录错误日志
                System.Diagnostics.Debug.WriteLine($"初始化任务状态历史失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载指定业务类型的当前任务状态
        /// </summary>
        /// <param name="bizType">业务类型</param>
        private void LoadCurrentTaskStatus(BizType bizType)
        {
            try
            {
                // 获取业务类型对应的表和状态字段
                var (tableType, statusField) = GetTableAndStatusField(bizType);
                if (tableType == null || string.IsNullOrEmpty(statusField))
                    return;

                // 查询当前任务状态
                var data = MainForm.Instance.AppContext.Db.CopyNew()
                    .Queryable(tableType.Name, "t")
                    .Select($"t.ID AS TaskId, t.{statusField} AS Status")
                    .Where($"t.isdeleted = 0")
                    .ToDataTable();

                // 更新状态历史
                foreach (DataRow row in data.Rows)
                {
                    var taskId = row["TaskId"].ToString();
                    var status = row["Status"].ToString();
                    var taskKey = $"{taskId}_{bizType}";

                    _statusHistory[taskKey] = status;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载业务类型 {bizType} 的任务状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 启动监控任务
        /// 定期检查任务状态变化
        /// </summary>
        private void StartMonitoringTask()
        {
            Task.Run(async () =>
            {
                while (_isMonitoring)
                {
                    try
                    {
                        await CheckTaskStatusChanges();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"检查任务状态变化失败: {ex.Message}");
                    }

                    // 每秒检查一次状态变化
                    await Task.Delay(1000);
                }
            });
        }

        /// <summary>
        /// 检查任务状态变化
        /// </summary>
        private async Task CheckTaskStatusChanges()
        {
            foreach (var bizType in _monitoredBusinessTypes)
            {
                await CheckStatusChangesForBusinessType(bizType);
            }
        }

        /// <summary>
        /// 检查指定业务类型的任务状态变化
        /// </summary>
        /// <param name="bizType">业务类型</param>
        private async Task CheckStatusChangesForBusinessType(BizType bizType)
        {
            try
            {
                // 获取业务类型对应的表和状态字段
                var (tableType, statusField) = GetTableAndStatusField(bizType);
                if (tableType == null || string.IsNullOrEmpty(statusField))
                    return;

                // 查询当前任务状态
                var data = await MainForm.Instance.AppContext.Db.CopyNew()
                    .Queryable(tableType.Name, "t")
                    .Select($"t.ID AS TaskId, t.{statusField} AS Status")
                    .Where($"t.isdeleted = 0")
                    .ToDataTableAsync();

                // 检查新增任务
                var currentTaskKeys = new HashSet<string>();
                foreach (DataRow row in data.Rows)
                {
                    var taskId = row["TaskId"].ToString();
                    var currentStatus = row["Status"].ToString();
                    var taskKey = $"{taskId}_{bizType}";
                    currentTaskKeys.Add(taskKey);

                    if (_statusHistory.TryGetValue(taskKey, out var oldStatus))
                    {
                        // 检查状态变化
                        if (oldStatus != currentStatus)
                        {
                            _statusHistory[taskKey] = currentStatus;

                            // 发布状态更新通知
                            var update = new TaskStatusUpdate
                            {
                                UpdateType = TaskStatusUpdateType.StatusChanged,
                                BusinessType = bizType,
                                TaskId = taskId,
                                OldStatus = oldStatus,
                                NewStatus = currentStatus,
                                Timestamp = DateTime.Now
                            };

                            TaskStatusSyncManager.Instance.PublishUpdate(update);
                        }
                    }
                    else
                    {
                        // 新任务
                        _statusHistory[taskKey] = currentStatus;

                        // 发布新增通知
                        var update = new TaskStatusUpdate
                        {
                            UpdateType = TaskStatusUpdateType.Added,
                            BusinessType = bizType,
                            TaskId = taskId,
                            NewStatus = currentStatus,
                            Timestamp = DateTime.Now
                        };

                        TaskStatusSyncManager.Instance.PublishUpdate(update);
                    }
                }

                // 检查删除任务
                var deletedTaskKeys = _statusHistory.Keys
                    .Where(key => key.EndsWith($"_{bizType}") && !currentTaskKeys.Contains(key))
                    .ToList();

                foreach (var taskKey in deletedTaskKeys)
                {
                    if (_statusHistory.TryRemove(taskKey, out var lastStatus))
                    {
                        var taskId = taskKey.Substring(0, taskKey.Length - $"_{bizType}".Length);

                        // 发布删除通知
                        var update = new TaskStatusUpdate
                        {
                            UpdateType = TaskStatusUpdateType.Deleted,
                            BusinessType = bizType,
                            TaskId = taskId,
                            OldStatus = lastStatus,
                            Timestamp = DateTime.Now
                        };

                        TaskStatusSyncManager.Instance.PublishUpdate(update);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"检查业务类型 {bizType} 的任务状态变化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取业务类型对应的表类型和状态字段名
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <returns>表类型和状态字段名</returns>
        private (Type, string) GetTableAndStatusField(BizType bizType)
        {
            // 根据业务类型获取对应的表名和状态字段
            // 这里使用简化的实现，实际项目中可能需要更复杂的映射关系
            switch (bizType)
            {
                //case BizType.采购订单:
                //    return (typeof(采购订单), "ApprovalStatus");
                //case BizType.销售订单:
                //    return (typeof(销售订单), "ApprovalStatus");
                //case BizType.制令单:
                //    return (typeof(制令单), "DataStatus");
                //case BizType.应收款单:
                //case BizType.应付款单:
                //    return (typeof(应收款单), "ARAPStatus");
                // 更多业务类型映射...
                default:
                    return (null, null);
            }
        }
        #endregion
    }
}