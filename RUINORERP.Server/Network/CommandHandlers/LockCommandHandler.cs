using Azure.Core;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using RUINORERP.Business.CommService;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 锁定命令处理器 - 处理客户端的锁定请求
    /// 包括单据锁定、解锁、强制解锁等操作
    /// </summary>
    [CommandHandler("LockCommandHandler", priority: 30)]
    public class LockCommandHandler : BaseCommandHandler
    {
        private readonly ILockManagerService _lockManagerService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<LockCommandHandler> _logger;

        /// <summary>
        /// 构造函数 - 通过DI注入依赖项
        /// </summary>
        public LockCommandHandler(
            ILockManagerService lockManagerService,
            ISessionService sessionService,
              ILogger<LockCommandHandler> logger = null)
            : base(logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lockManagerService = lockManagerService ?? throw new ArgumentNullException(nameof(lockManagerService));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));

            // 设置支持的命令
            SetSupportedCommands(
                LockCommands.Lock,
                LockCommands.Unlock,
                LockCommands.CheckLockStatus,
                LockCommands.RequestUnlock,
                LockCommands.RefuseUnlock,
                LockCommands.ForceUnlock,
                LockCommands.BroadcastLockStatus
            );
        }

        /// <summary>
        /// 统一的请求数据验证方法
        /// </summary>
        /// <param name="cmd">队列命令对象</param>
        /// <param name="operationName">操作名称</param>
        /// <returns>验证结果</returns>
        private (LockRequest LockRequest, string ErrorMessage) ValidateLockRequest(QueuedCommand cmd, string operationName)
        {
            var lockRequest = cmd.Packet.Request as LockRequest;
            if (lockRequest == null)
            {
                return (null, $"无效的{operationName}请求数据");
            }
            if (lockRequest.UnlockType != UnlockType.ByBizName)
            {
                if (lockRequest.LockInfo?.BillID <= 0)
                {
                    return (null, "单据ID无效");
                }
            }
            return (lockRequest, null);
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        /// <param name="cmd">队列命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;

                // 使用字典映射替代冗长的if-else链
                var commandHandlers = new Dictionary<CommandId, Func<QueuedCommand, CancellationToken, Task<IResponse>>>
                {
                    { LockCommands.Lock, HandleLockRequestAsync },
                    { LockCommands.Unlock, HandleLockReleaseAsync },
                    { LockCommands.CheckLockStatus, HandleQueryLockStatusAsync },
                    { LockCommands.RequestUnlock, HandleRequestUnlockAsync },
                    { LockCommands.RefuseUnlock, HandleRefuseUnlockAsync },
                    { LockCommands.ForceUnlock, HandleForceUnlockAsync },
                    { LockCommands.BroadcastLockStatus, HandleBroadcastLockStatusAsync }
                };

                if (commandHandlers.TryGetValue(commandId, out var handler))
                {
                    return await handler(cmd, cancellationToken);
                }

                return CreateErrorResponse($"不支持的锁定命令类型: {commandId.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁定命令异常: {ex.Message}");
                return CreateErrorResponse($"处理锁定命令异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 统一的错误响应创建方法
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="context">执行上下文（可选）</param>
        /// <param name="exception">异常信息（可选）</param>
        /// <returns>错误响应</returns>
        private IResponse CreateErrorResponse(string message, CommandContext context = null, Exception exception = null)
        {
            if (exception != null && context != null)
            {
                return ResponseFactory.CreateSpecificErrorResponse<LockResponse>(message);
            }
            return ResponseFactory.CreateSpecificErrorResponse<LockResponse>(message);
        }

        /// <summary>
        /// 处理锁定请求命令
        /// </summary>
        /// <param name="cmd">锁定请求命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleLockRequestAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var validation = ValidateLockRequest(cmd, "锁定");
                if (validation.ErrorMessage != null)
                {
                    return CreateErrorResponse(validation.ErrorMessage);
                }

                // 请求锁定
                return await _lockManagerService.TryLockDocumentAsync(validation.LockRequest.LockInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁定请求异常: {ex.Message}");
                return CreateErrorResponse($"处理锁定请求异常: {ex.Message}", cmd.Packet.ExecutionContext, ex);
            }
        }

        /// <summary>
        /// 处理锁定释放命令
        /// </summary>
        /// <param name="cmd">锁定释放命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleLockReleaseAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var validation = ValidateLockRequest(cmd, "解锁");
                if (validation.ErrorMessage != null)
                {
                    return CreateErrorResponse(validation.ErrorMessage);
                }
                LockRequest lockRequest = new LockRequest();
                if (cmd.Packet.Request is LockRequest request)
                {
                    lockRequest = request;
                }
                if (lockRequest.UnlockType == UnlockType.ByBizName)
                {
                    // 释放锁定
                    return await _lockManagerService.UnlockDocumentsByBizNameAsync(validation.LockRequest.LockInfo.LockedUserId, (int)validation.LockRequest.LockInfo.bizType);
                }
                else
                {
                    // 释放锁定
                    return await _lockManagerService.UnlockDocumentAsync(validation.LockRequest.LockInfo.BillID, validation.LockRequest.LockInfo.LockedUserId);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁定释放异常: {ex.Message}");
                return CreateErrorResponse($"处理锁定释放异常: {ex.Message}", cmd.Packet.ExecutionContext, ex);
            }
        }

        /// <summary>
        /// 处理强制解锁命令
        /// </summary>
        /// <param name="cmd">强制解锁命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleForceUnlockAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var validation = ValidateLockRequest(cmd, "强制解锁");
                if (validation.ErrorMessage != null)
                {
                    return CreateErrorResponse(validation.ErrorMessage);
                }

                // 强制解锁
                return await _lockManagerService.ForceUnlockDocumentAsync(validation.LockRequest.LockInfo.BillID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理强制解锁异常: {ex.Message}");
                return CreateErrorResponse($"处理强制解锁异常: {ex.Message}", cmd.Packet.ExecutionContext, ex);
            }
        }

        /// <summary>
        /// 处理请求解锁命令
        /// </summary>
        /// <param name="cmd">请求解锁命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleRequestUnlockAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var validation = ValidateLockRequest(cmd, "请求解锁");
                if (validation.ErrorMessage != null)
                {
                    return CreateErrorResponse(validation.ErrorMessage);
                }

                // 请求解锁
                return await _lockManagerService.RequestUnlockDocumentAsync(validation.LockRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理请求解锁异常: {ex.Message}");
                return CreateErrorResponse($"处理请求解锁异常: {ex.Message}", cmd.Packet.ExecutionContext, ex);
            }
        }

        /// <summary>
        /// 处理拒绝解锁命令
        /// </summary>
        /// <param name="cmd">拒绝解锁命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleRefuseUnlockAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var validation = ValidateLockRequest(cmd, "拒绝解锁");
                if (validation.ErrorMessage != null)
                {
                    return CreateErrorResponse(validation.ErrorMessage);
                }

                // 拒绝解锁
                return await _lockManagerService.RefuseUnlockRequestAsync(validation.LockRequest);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理拒绝解锁异常: {ex.Message}");
                return CreateErrorResponse($"处理拒绝解锁异常: {ex.Message}", cmd.Packet.ExecutionContext, ex);
            }
        }

        /// <summary>
        /// 处理查询锁状态命令
        /// </summary>
        /// <param name="cmd">查询锁状态命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleQueryLockStatusAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var validation = ValidateLockRequest(cmd, "查询锁状态");
                if (validation.ErrorMessage != null)
                {
                    return CreateErrorResponse(validation.ErrorMessage);
                }

                // 查询锁状态
                var lockInfo = _lockManagerService.GetLockInfo(validation.LockRequest.LockInfo.BillID);
                if (lockInfo == null)
                {
                    lockInfo = new LockInfo();
                    lockInfo.BillID = validation.LockRequest.LockInfo.BillID;
                    lockInfo.IsLocked = false;
                }
                var response = new LockResponse
                {
                    IsSuccess = true,
                    Message = "锁状态查询成功",
                    LockInfo = lockInfo,
                };
                // 模拟异步操作 要Task方法签名一致
                await Task.CompletedTask;
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理查询锁状态异常: {ex.Message}");
                return CreateErrorResponse($"处理查询锁状态异常: {ex.Message}", cmd.Packet.ExecutionContext, ex);
            }
        }

        /// <summary>
        /// 处理广播锁状态命令
        /// </summary>
        /// <param name="cmd">广播锁状态命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        private async Task<IResponse> HandleBroadcastLockStatusAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var validation = ValidateLockRequest(cmd, "广播锁状态");
                if (validation.ErrorMessage != null)
                {
                    return CreateErrorResponse(validation.ErrorMessage);
                }

                // 广播锁状态
                await _lockManagerService.BroadcastLockStatusToAllClientsAsync(new List<LockInfo> { validation.LockRequest.LockInfo });

                var response = ResponseFactory.CreateSpecificSuccessResponse(cmd.Packet.ExecutionContext, "锁状态广播成功");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理广播锁状态异常: {ex.Message}");
                return CreateErrorResponse($"广播锁状态失败: {ex.Message}", cmd.Packet.ExecutionContext, ex);
            }
        }

    }
}