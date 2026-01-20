using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Server.Helpers;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 文件存储监控服务
    /// 监控文件存储空间使用情况,当达到阈值时发送预警
    /// </summary>
    public class FileStorageMonitorService : IDisposable
    {
        private readonly FileCleanupService _fileCleanupService;
        private readonly ServerGlobalConfig _serverConfig;
        private readonly ILogger<FileStorageMonitorService> _logger;
        private Timer _monitorTimer;
        private bool _isDisposed = false;

        /// <summary>
        /// 存储空间使用率预警阈值(百分比)
        /// 默认: 80%
        /// </summary>
        public double WarningThreshold { get; set; } = 80.0;

        /// <summary>
        /// 存储空间紧急阈值(百分比)
        /// 默认: 90%
        /// </summary>
        public double CriticalThreshold { get; set; } = 90.0;

        /// <summary>
        /// 监控间隔(分钟)
        /// 默认: 30分钟
        /// </summary>
        public int MonitorIntervalMinutes { get; set; } = 30;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileStorageMonitorService(
            FileCleanupService fileCleanupService,
            ServerGlobalConfig serverConfig,
            ILogger<FileStorageMonitorService> logger)
        {
            _fileCleanupService = fileCleanupService ?? throw new ArgumentNullException(nameof(fileCleanupService));
            _serverConfig = serverConfig ?? throw new ArgumentNullException(nameof(serverConfig));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 启动监控
        /// </summary>
        public void StartMonitoring()
        {
            if (_monitorTimer != null)
            {
                _logger.LogWarning("文件存储监控服务已在运行");
                return;
            }

            _logger.LogInformation("启动文件存储监控服务,间隔: {Minutes}分钟", MonitorIntervalMinutes);

            // 立即执行一次检查
            _ = Task.Run(async () => await CheckStorageSpaceAsync());

            // 启动定时器
            _monitorTimer = new Timer(
                async _ => await CheckStorageSpaceAsync(),
                null,
                TimeSpan.FromMinutes(MonitorIntervalMinutes),
                TimeSpan.FromMinutes(MonitorIntervalMinutes)
            );
        }

        /// <summary>
        /// 停止监控
        /// </summary>
        public void StopMonitoring()
        {
            if (_monitorTimer != null)
            {
                _monitorTimer.Dispose();
                _monitorTimer = null;
                _logger.LogInformation("文件存储监控服务已停止");
            }
        }

        /// <summary>
        /// 检查存储空间
        /// </summary>
        private async Task CheckStorageSpaceAsync()
        {
            try
            {
                _logger.LogDebug("开始检查文件存储空间");

                // 获取文件统计信息
                var stats = await _fileCleanupService.GetCleanupStatisticsAsync();

                // 获取存储路径信息
                string storagePath = FileStorageHelper.GetStoragePath();
                if (string.IsNullOrEmpty(storagePath))
                {
                    _logger.LogWarning("文件存储路径未配置,无法检查存储空间");
                    return;
                }

                // 检查磁盘空间
                var driveInfo = new System.IO.DriveInfo(storagePath);
                long totalDiskSpace = driveInfo.TotalSize;
                long freeDiskSpace = driveInfo.AvailableFreeSpace;
                long usedDiskSpace = totalDiskSpace - freeDiskSpace;

                // 计算使用率
                double diskUsagePercentage = (double)usedDiskSpace / totalDiskSpace * 100;

                _logger.LogInformation(
                    "磁盘空间检查 - 总空间: {TotalGB:F2}GB, 已用: {UsedGB:F2}GB, 可用: {FreeGB:F2}GB, 使用率: {UsagePercent:F2}%",
                    totalDiskSpace / 1024.0 / 1024 / 1024,
                    usedDiskSpace / 1024.0 / 1024 / 1024,
                    freeDiskSpace / 1024.0 / 1024 / 1024,
                    diskUsagePercentage
                );

                // 检查配置的最大存储限制
                if (_serverConfig.MaxStorageSizeGB > 0)
                {
                    long maxStorageBytes = _serverConfig.MaxStorageSizeGB * 1024L * 1024 * 1024;
                    double fileStorageUsagePercentage = (double)stats.TotalStorageSize / maxStorageBytes * 100;

                    _logger.LogInformation(
                        "文件存储检查 - 总文件: {TotalFiles}, 存储大小: {StorageSize:F2}GB, 配置上限: {MaxGB:F2}GB, 使用率: {UsagePercent:F2}%",
                        stats.TotalFiles,
                        stats.TotalStorageSize / 1024.0 / 1024 / 1024,
                        _serverConfig.MaxStorageSizeGB,
                        fileStorageUsagePercentage
                    );

                    // 检查是否超过阈值
                    if (fileStorageUsagePercentage >= CriticalThreshold)
                    {
                        _logger.LogError(
                            "❌ 文件存储空间紧急警告! 当前使用率: {UsagePercent:F2}%, 阈值: {Threshold:F2}%",
                            fileStorageUsagePercentage,
                            CriticalThreshold
                        );

                        // 尝试自动清理
                        await AutoCleanupAsync("紧急");
                    }
                    else if (fileStorageUsagePercentage >= WarningThreshold)
                    {
                        _logger.LogWarning(
                            "⚠️ 文件存储空间警告! 当前使用率: {UsagePercent:F2}%, 阈值: {Threshold:F2}%",
                            fileStorageUsagePercentage,
                            WarningThreshold
                        );
                    }
                }

                // 检查磁盘空间
                if (diskUsagePercentage >= CriticalThreshold)
                {
                    _logger.LogError(
                        "❌ 磁盘空间紧急警告! 当前使用率: {UsagePercent:F2}%, 阈值: {Threshold:F2}%",
                        diskUsagePercentage,
                        CriticalThreshold
                    );

                    // 尝试自动清理
                    await AutoCleanupAsync("紧急");
                }
                else if (diskUsagePercentage >= WarningThreshold)
                {
                    _logger.LogWarning(
                        "⚠️ 磁盘空间警告! 当前使用率: {UsagePercent:F2}%, 阈值: {Threshold:F2}%",
                        diskUsagePercentage,
                        WarningThreshold
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查文件存储空间失败");
            }
        }

        /// <summary>
        /// 自动清理
        /// </summary>
        /// <param name="reason">清理原因</param>
        private async Task AutoCleanupAsync(string reason)
        {
            try
            {
                _logger.LogInformation("因{Reason}触发自动文件清理", reason);

                // 清理过期文件(7天阈值)
                var expiredCount = await _fileCleanupService.CleanupExpiredFilesAsync(
                    daysThreshold: 7,
                    physicalDelete: true
                );

                _logger.LogInformation("自动清理完成,清理过期文件: {Count}个", expiredCount);

                // 清理孤立文件(3天阈值)
                var orphanedCount = await _fileCleanupService.CleanupOrphanedFilesAsync(
                    daysThreshold: 3,
                    physicalDelete: true
                );

                _logger.LogInformation("自动清理完成,清理孤立文件: {Count}个", orphanedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "自动清理失败");
            }
        }

        /// <summary>
        /// 获取存储统计信息(供监控界面使用)
        /// </summary>
        public async Task<FileStorageMonitorInfo> GetMonitorInfoAsync()
        {
            try
            {
                var stats = await _fileCleanupService.GetCleanupStatisticsAsync();
                string storagePath = FileStorageHelper.GetStoragePath();

                double diskUsagePercentage = 0;
                double freeDiskSpaceGB = 0;
                double totalDiskSpaceGB = 0;

                if (!string.IsNullOrEmpty(storagePath))
                {
                    var driveInfo = new System.IO.DriveInfo(storagePath);
                    totalDiskSpaceGB = driveInfo.TotalSize / 1024.0 / 1024 / 1024;
                    freeDiskSpaceGB = driveInfo.AvailableFreeSpace / 1024.0 / 1024 / 1024;
                    diskUsagePercentage = (1.0 - freeDiskSpaceGB / totalDiskSpaceGB) * 100;
                }

                double fileStorageUsagePercentage = 0;
                if (_serverConfig.MaxStorageSizeGB > 0)
                {
                    fileStorageUsagePercentage = (double)stats.TotalStorageSize / (_serverConfig.MaxStorageSizeGB * 1024L * 1024 * 1024) * 100;
                }

                return new FileStorageMonitorInfo
                {
                    TotalFiles = stats.TotalFiles,
                    ActiveFiles = stats.ActiveFiles,
                    ExpiredFiles = stats.ExpiredFiles,
                    OrphanedFiles = stats.OrphanedFiles,
                    TotalStorageSizeGB = stats.TotalStorageSize / 1024.0 / 1024 / 1024,
                    TotalStorageSizeFormatted = stats.TotalStorageSizeFormatted,
                    DiskUsagePercentage = diskUsagePercentage,
                    FreeDiskSpaceGB = freeDiskSpaceGB,
                    TotalDiskSpaceGB = totalDiskSpaceGB,
                    FileStorageUsagePercentage = fileStorageUsagePercentage,
                    MaxStorageSizeGB = _serverConfig.MaxStorageSizeGB,
                    LastCheckTime = DateTime.Now,
                    WarningThreshold = WarningThreshold,
                    CriticalThreshold = CriticalThreshold
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取监控信息失败");
                throw;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            StopMonitoring();

            _logger.LogInformation("文件存储监控服务已释放");
        }
    }

    /// <summary>
    /// 文件存储监控信息
    /// </summary>
    public class FileStorageMonitorInfo
    {
        /// <summary>
        /// 总文件数
        /// </summary>
        public long TotalFiles { get; set; }

        /// <summary>
        /// 正常文件数
        /// </summary>
        public long ActiveFiles { get; set; }

        /// <summary>
        /// 过期文件数
        /// </summary>
        public long ExpiredFiles { get; set; }

        /// <summary>
        /// 孤立文件数
        /// </summary>
        public long OrphanedFiles { get; set; }

        /// <summary>
        /// 总存储大小(GB)
        /// </summary>
        public double TotalStorageSizeGB { get; set; }

        /// <summary>
        /// 总存储大小(格式化)
        /// </summary>
        public string TotalStorageSizeFormatted { get; set; }

        /// <summary>
        /// 磁盘使用率(百分比)
        /// </summary>
        public double DiskUsagePercentage { get; set; }

        /// <summary>
        /// 可用磁盘空间(GB)
        /// </summary>
        public double FreeDiskSpaceGB { get; set; }

        /// <summary>
        /// 总磁盘空间(GB)
        /// </summary>
        public double TotalDiskSpaceGB { get; set; }

        /// <summary>
        /// 文件存储使用率(百分比)
        /// </summary>
        public double FileStorageUsagePercentage { get; set; }

        /// <summary>
        /// 配置的最大存储空间(GB)
        /// </summary>
        public double MaxStorageSizeGB { get; set; }

        /// <summary>
        /// 最后检查时间
        /// </summary>
        public DateTime LastCheckTime { get; set; }

        /// <summary>
        /// 警告阈值
        /// </summary>
        public double WarningThreshold { get; set; }

        /// <summary>
        /// 紧急阈值
        /// </summary>
        public double CriticalThreshold { get; set; }

        /// <summary>
        /// 是否处于警告状态
        /// </summary>
        public bool IsWarning => FileStorageUsagePercentage >= WarningThreshold;

        /// <summary>
        /// 是否处于紧急状态
        /// </summary>
        public bool IsCritical => FileStorageUsagePercentage >= CriticalThreshold;
    }
}
