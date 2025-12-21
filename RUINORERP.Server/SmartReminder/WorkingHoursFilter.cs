using Microsoft.Extensions.Logging;
using RUINORERP.Server.SmartReminder.InvReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using StackExchange.Redis;

namespace RUINORERP.Server.SmartReminder
{
    /// <summary>
    /// 下班时间暂停检查
    /// </summary>
    public class WorkingHoursFilter
    {
        private readonly ILogger<WorkingHoursFilter> _logger;

        public WorkingHoursFilter(
            ILogger<WorkingHoursFilter> logger)
        {
            _logger = logger;
        }
        public bool ShouldCheck()
        {
            var now = DateTime.Now;
            return now.Hour >= 9 && now.Hour < 18; // 9AM-6PM

            //下面代码可能更完善。更细。将来再实现
            //var now = DateTime.Now;
            //// 检查是否在工作日
            //if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
            //    return false;

            //// 检查是否在工作时间
            //if (now.Hour < _config.WorkingHoursStart || now.Hour >= _config.WorkingHoursEnd)
            //    return false;

            return true;

        }
        public async Task CheckInventoryAsync(ISmartReminderMonitor monitor)
        {
            if (!ShouldCheck())
            {
                return;
            }

            try
            {
                await monitor.CheckRemindersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "库存检查出错");
                // 根据错误类型决定是否进入降级模式
                if (ex is SqlException || ex is RedisConnectionException)
                {
                    EnterDegradedMode(monitor);
                }
            }
        }
        private void EnterDegradedMode(ISmartReminderMonitor monitor)
        {
            _logger.LogWarning("进入库存检查降级模式");
            // 调整检查频率
            //monitor.AdjustMonitoringInterval(_config.EmergencyCheckInterval);

            // 触发报警
            _logger.LogWarning("触发系统管理员提醒");
        }

        // 修改检查方法 Todo  要如果调用修复？
        //private async void CheckInventoryCallback(object state)
        //{
        //    if (!_workingHoursFilter.ShouldCheck()) return;

        //    await CheckInventoryAsync();
        //}
        // Todo 要如果调用修复？
        //public void AdjustMonitoringInterval(TimeSpan newInterval)
        //{
        //    _timer?.Change(
        //        dueTime: newInterval,
        //        period: newInterval);
        //}

        // Todo 要如果调用修复？
        // 根据系统负载自动调整
        //public void AutoAdjustInterval()
        //{
        //    var cpuUsage = GetCpuUsage();
        //    var newInterval = cpuUsage > 80 ?
        //        TimeSpan.FromMinutes(10) :
        //        TimeSpan.FromMinutes(5);

        //    AdjustMonitoringInterval(newInterval);
        //}
    }
}