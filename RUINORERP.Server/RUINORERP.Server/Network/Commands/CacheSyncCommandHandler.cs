using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少ICacheService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 缓存同步命令处理器 - 处理客户端与服务器之间的缓存同步请求
    /// </summary>
    [CommandHandler("CacheSyncCommandHandler", priority: 80)]
    public class CacheSyncCommandHandler : UnifiedCommandHandlerBase
    {
        // private readonly ICacheService _cacheService; // 暂时注释，缺少ICacheService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public CacheSyncCommandHandler() : base()
        {
            // _cacheService = Program.ServiceProvider.GetRequiredService<ICacheService>(); // 暂时注释，缺少ICacheService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CacheSyncCommandHandler(
            // ICacheService cacheService, // 暂时注释，缺少ICacheService接口定义
            ILogger<CacheSyncCommandHandler> logger = null) : base(logger)
        {
            // _cacheService = cacheService; // 暂时注释，缺少ICacheService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)CacheCommands.CacheSync
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 80;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == CacheCommands.CacheSync)
                {
                    return await HandleCacheSyncAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理缓存同步命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理缓存同步命令
        /// </summary>
        private async Task<CommandResult> HandleCacheSyncAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理缓存同步命令 [会话: {command.SessionID}]");

                // 解析缓存同步数据
                var syncData = ParseCacheSyncData(command.Packet.Body);
                if (syncData == null)
                {
                    return CommandResult.Failure("缓存同步数据格式错误", "INVALID_SYNC_DATA");
                }

                // 暂时返回模拟结果，因为缺少ICacheService接口定义
                var syncResult = new CacheSyncResult
                {
                    SyncedKeys = syncData.CacheKeys,
                    IsSuccess = true,
                    Message = "缓存同步成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateCacheSyncResponse(syncResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        SyncedKeys = syncResult.SyncedKeys?.Count ?? 0,
                        SyncMode = syncData.SyncMode,
                        SessionId = command.SessionID
                    },
                    message: "缓存同步成功（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理缓存同步命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"缓存同步异常: {ex.Message}", "CACHE_SYNC_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析缓存同步数据
        /// </summary>
        private CacheSyncData ParseCacheSyncData(byte[] body)
        {
            try
            {
                if (body == null || body.Length == 0)
                    return null;

                var dataString = System.Text.Encoding.UTF8.GetString(body);
                var parts = dataString.Split('|');

                if (parts.Length >= 2)
                {
                    return new CacheSyncData
                    {
                        CacheKeys = new List<string>(parts[0].Split(',')),
                        SyncMode = parts[1]
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"解析缓存同步数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建缓存同步响应
        /// </summary>
        private OriginalData CreateCacheSyncResponse(CacheSyncResult syncResult)
        {
            var responseData = $"SUCCESS|{string.Join(",", syncResult.SyncedKeys ?? new List<string>())}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)CacheCommands.CacheSync;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }
    }

    /// <summary>
    /// 缓存同步数据
    /// </summary>
    public class CacheSyncData
    {
        public List<string> CacheKeys { get; set; }
        public string SyncMode { get; set; }
    }

    /// <summary>
    /// 缓存同步结果
    /// </summary>
    public class CacheSyncResult
    {
        public List<string> SyncedKeys { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}