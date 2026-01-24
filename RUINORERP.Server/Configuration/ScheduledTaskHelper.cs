using System;

namespace RUINORERP.Server.Configuration
{
    /// <summary>
    /// 定时任务辅助类
    /// 提供统一的任务ID常量和配置访问方法
    /// </summary>
    public static class ScheduledTaskHelper
    {
        #region 任务ID常量

        /// <summary>
        /// 库存快照任务ID
        /// </summary>
        public const string InventorySnapshotTask = "InventorySnapshot";

        /// <summary>
        /// 文件清理任务ID
        /// </summary>
        public const string FileCleanupTask = "FileCleanup";

        /// <summary>
        /// 临时图片清理任务ID
        /// </summary>
        public const string TempImageCleanupTask = "TempImageCleanup";

        /// <summary>
        /// 安全库存计算任务ID
        /// </summary>
        public const string SafetyStockCalculationTask = "SafetyStockCalculation";

        /// <summary>
        /// 提醒检查任务ID
        /// </summary>
        public const string ReminderCheckTask = "ReminderCheck";

        #endregion

        /// <summary>
        /// 获取任务执行时间
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>执行时间TimeSpan</returns>
        public static TimeSpan GetTaskExecutionTime(string taskId)
        {
            return ScheduledTaskConfiguration.GetInstance().GetExecutionTimeSpan(taskId);
        }

        /// <summary>
        /// 计算下次执行时间
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>下次执行时间</returns>
        public static DateTime CalculateNextExecutionTime(string taskId)
        {
            var config = ScheduledTaskConfiguration.GetInstance();
            var task = config.Tasks.Find(t => t.Id == taskId);

            if (task == null)
            {
                return DateTime.MaxValue;
            }

            var now = DateTime.Now;
            var executionTime = TimeSpan.Parse(task.ExecutionTime);

            if (task.IntervalType == IntervalType.Daily)
            {
                // 每日执行
                var nextRunTime = new DateTime(now.Year, now.Month, now.Day,
                    executionTime.Hours, executionTime.Minutes, executionTime.Seconds);

                if (nextRunTime <= now)
                {
                    nextRunTime = nextRunTime.AddDays(1);
                }

                return nextRunTime;
            }
            else if (task.IntervalType == IntervalType.Recurring)
            {
                // 循环执行
                return now.Add(executionTime);
            }

            return DateTime.MaxValue;
        }

        /// <summary>
        /// 检查任务是否启用
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>是否启用</returns>
        public static bool IsTaskEnabled(string taskId)
        {
            var config = ScheduledTaskConfiguration.GetInstance();
            var task = config.Tasks.Find(t => t.Id == taskId);
            return task?.Enabled ?? false;
        }

        /// <summary>
        /// 获取任务信息
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <returns>任务信息</returns>
        public static ScheduledTask GetTaskInfo(string taskId)
        {
            var config = ScheduledTaskConfiguration.GetInstance();
            return config.Tasks.Find(t => t.Id == taskId);
        }
    }
}
