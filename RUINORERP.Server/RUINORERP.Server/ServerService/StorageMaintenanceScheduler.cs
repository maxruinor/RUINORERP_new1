using Microsoft.Extensions.Logging;
using RUINORERP.Server.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.ServerService
{
    // 服务器端定时任务调度器
    public class StorageMaintenanceScheduler
    {
        private readonly FileStorageManager _storageManager;
        private readonly ILogger<StorageMaintenanceScheduler> _logger;
        private Timer _cleanupTimer;
        private Timer _consistencyTimer;

        public StorageMaintenanceScheduler(
            FileStorageManager storageManager,
            ILogger<StorageMaintenanceScheduler> logger)
        {
            _storageManager = storageManager;
            _logger = logger;
        }

        public void Start()
        {
            // 每天凌晨2点清理临时文件
            _cleanupTimer = new Timer(async _ =>
            {
                try
                {
                    await _storageManager.CleanTempFilesAsync(TimeSpan.FromDays(7));
                    _logger.LogInformation("定时清理任务执行完成");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "定时清理任务执行失败");
                }
            }, null, GetNextTime(2, 0), TimeSpan.FromDays(1));

            // 每周日凌晨3点检查一致性
            _consistencyTimer = new Timer(async _ =>
            {
                try
                {
                    await _storageManager.CheckConsistencyAsync();
                    _logger.LogInformation("一致性检查任务执行完成");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "一致性检查任务执行失败");
                }
            }, null, GetNextTime(3, 0, DayOfWeek.Sunday), TimeSpan.FromDays(7));
        }

        private TimeSpan GetNextTime(int hour, int minute, DayOfWeek? dayOfWeek = null)
        {
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);

            if (dayOfWeek.HasValue)
            {
                while (nextRun.DayOfWeek != dayOfWeek.Value)
                {
                    nextRun = nextRun.AddDays(1);
                }
            }

            if (nextRun < now)
            {
                nextRun = nextRun.AddDays(dayOfWeek.HasValue ? 7 : 1);
            }

            return nextRun - now;
        }

        public void Stop()
        {
            _cleanupTimer?.Dispose();
            _consistencyTimer?.Dispose();
        }
    }
}
