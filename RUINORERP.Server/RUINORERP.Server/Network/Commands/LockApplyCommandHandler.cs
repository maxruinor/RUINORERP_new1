using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Lock;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少ILockService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 锁申请命令处理器 - 处理客户端的资源锁申请请求
    /// </summary>
    [CommandHandler("LockApplyCommandHandler", priority: 85)]
    public class LockApplyCommandHandler : CommandHandlerBase
    {
        // private readonly ILockService _lockService; // 暂时注释，缺少ILockService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public LockApplyCommandHandler() : base()
        {
            // _lockService = Program.ServiceProvider.GetRequiredService<ILockService>(); // 暂时注释，缺少ILockService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LockApplyCommandHandler(
            // ILockService lockService, // 暂时注释，缺少ILockService接口定义
            ILogger<LockApplyCommandHandler> logger = null) : base(logger)
        {
            // _lockService = lockService; // 暂时注释，缺少ILockService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)LockCommands.LockRequest
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 85;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == LockCommands.LockRequest)
                {
                    return await HandleLockApplyAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理锁申请命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理锁申请命令
        /// </summary>
        private async Task<CommandResult> HandleLockApplyAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理锁申请命令 [会话: {command.SessionID}]");

                // 解析锁申请数据
                var lockData = command.Packet.GetJsonData<LockApplyData>();
                if (lockData == null)
                {
                    return CommandResult.Failure("锁申请数据格式错误", "INVALID_LOCK_DATA");
                }

                // 暂时返回模拟结果，因为缺少ILockService接口定义
                var lockResult = new LockApplyResult
                {
                    IsAcquired = true,
                    LockId = Guid.NewGuid().ToString(),
                    Message = "锁申请成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateLockApplyResponse(lockResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        ResourceId = lockData.ResourceId,
                        LockType = lockData.LockType,
                        IsAcquired = lockResult.IsAcquired,
                        SessionId = command.SessionID
                    },
                    message: lockResult.IsAcquired ? "锁申请成功（模拟）" : "锁申请失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理锁申请命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"锁申请异常: {ex.Message}", "LOCK_APPLY_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析锁申请数据
        /// </summary>
        private LockApplyData ParseLockApplyData(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return null;

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                if (parts.Length >= 3)
                {
                    return new LockApplyData
                    {
                        ResourceId = parts[0],
                        LockType = parts[1],
                        TimeoutMs = int.TryParse(parts[2], out var timeout) ? timeout : 30000
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"解析锁申请数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建锁申请响应
        /// </summary>
        private OriginalData CreateLockApplyResponse(LockApplyResult lockResult)
        {
            var responseData = $"LOCK_RESULT|{lockResult.IsAcquired}|{lockResult.LockId}|{lockResult.Message}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)LockCommands.LockRequest;
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
    /// 锁申请数据
    /// </summary>
    public class LockApplyData
    {
        public string ResourceId { get; set; }
        public string LockType { get; set; }
        public int TimeoutMs { get; set; }
    }

    /// <summary>
    /// 锁申请结果
    /// </summary>
    public class LockApplyResult
    {
        public bool IsAcquired { get; set; }
        public string LockId { get; set; }
        public string Message { get; set; }
    }
}