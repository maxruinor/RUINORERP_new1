using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Helpers;
using RUINORERP.Server.Network.Services;
using System.IO;
using System.Collections.Generic;
using RUINORERP.Model;
using System.Linq;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 文件存储信息服务处理器
    /// 处理文件存储使用情况查询等业务逻辑
    /// </summary>
    [CommandHandler("FileStorageInfoCommandHandler", priority: 50)]
    public class FileStorageInfoCommandHandler : BaseCommandHandler
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<FileStorageInfoCommandHandler> _logger;
        private readonly string _fileStoragePath;

        public FileStorageInfoCommandHandler(SessionService sessionService, ILogger<FileStorageInfoCommandHandler> logger = null)
        {
            _sessionService = sessionService;
            _logger = logger;
            
            // 获取文件存储路径
            _fileStoragePath = Startup.GetFromFac<Model.ConfigModel.ServerConfig>()?.FileStoragePath ?? "Files";

            // 设置支持的命令
            SetSupportedCommands(
                FileCommands.FileStorageInfo
            );
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;

                if (commandId == FileCommands.FileStorageInfo)
                {
                    return await HandleStorageInfoAsync(cmd.Packet.ExecutionContext, cancellationToken);
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的文件存储信息命令");
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理文件存储信息命令时出错");
            }
        }

        /// <summary>
        /// 处理存储信息查询
        /// </summary>
        private async Task<IResponse> HandleStorageInfoAsync(CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                // 获取存储使用信息
                var storageInfo = GetStorageUsageInfo();

                // 只记录关键信息，简化日志内容
                _logger?.LogDebug("获取存储使用信息成功");

                return StorageUsageInfo.CreateSuccess(
                    storageInfo.TotalSize,
                    storageInfo.TotalFileCount,
                    storageInfo.CategoryUsage,
                    "获取存储使用信息成功"
                );
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取存储使用信息时出错");
                return StorageUsageInfo.CreateFailure(ex.Message);
            }
        }

        /// <summary>
        /// 获取存储使用信息
        /// </summary>
        private StorageUsageInfoData GetStorageUsageInfo()
        {
            var usageData = new StorageUsageInfoData
            {
                TotalSize = 0,
                TotalFileCount = 0,
                CategoryUsage = new Dictionary<string, CategoryUsage>()
            };

            try
            {
                if (!Directory.Exists(_fileStoragePath))
                {
                    return usageData;
                }

                // 获取所有分类目录
                var categories = new Dictionary<string, string>
                {
                    { "PaymentVoucher", Path.Combine(_fileStoragePath, "PaymentVoucher") },
                    { "ProductImage", Path.Combine(_fileStoragePath, "ProductImage") },
                    { "BOMManual", Path.Combine(_fileStoragePath, "BOMManual") },
                    { "General", _fileStoragePath }
                };

                foreach (var category in categories)
                {
                    if (Directory.Exists(category.Value))
                    {
                        var categoryUsage = CalculateDirectoryUsage(category.Value);
                        usageData.CategoryUsage[category.Key] = categoryUsage;
                        usageData.TotalSize += categoryUsage.TotalSize;
                        usageData.TotalFileCount += categoryUsage.FileCount;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "计算存储使用信息时出错");
            }

            return usageData;
        }

        /// <summary>
        /// 计算目录使用情况
        /// </summary>
        private CategoryUsage CalculateDirectoryUsage(string directoryPath)
        {
            var usage = new CategoryUsage { FileCount = 0, TotalSize = 0 };

            try
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                var files = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

                usage.FileCount = files.Length;
                usage.TotalSize = files.Sum(f => f.Length);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "计算目录使用情况时出错: {DirectoryPath}", directoryPath);
            }

            return usage;
        }
    }
}