using Azure.Core;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.CommService;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Lock;
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
            :  base(logger)
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

                return ResponseFactory.CreateSpecificErrorResponse<LockResponse>($"不支持的锁定命令类型: {commandId.ToString()}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁定命令异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse<LockResponse>($"处理锁定命令异常: {ex.Message}");
            }
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
                // 反序列化请求数据
                var lockRequest = cmd.Packet.Request as LockRequest;
                if (lockRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("无效的锁定请求数据");
                }

                // 验证请求数据
                if (lockRequest.LockInfo.BillID <= 0)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("单据ID不能为空");
                }

                // 请求锁定
                var lockSuccess = await _lockManagerService.TryLockDocumentAsync(lockRequest.LockInfo);

                if (lockSuccess)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();

                    var response = new LockResponse
                    {
                        IsSuccess = true,
                        Message = "单据锁定成功",
                        LockInfo = lockRequest.LockInfo
                    };
                    return response;
                }
                else
                {
                    var response = new LockResponse
                    {
                        IsSuccess = false,
                        Message = "单据锁定失败",
                        LockInfo = lockRequest.LockInfo
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁定请求异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse<LockResponse>($"处理锁定请求异常: {ex.Message}");
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
                // 反序列化请求数据
                var lockRequest = cmd.Packet.Request as LockRequest;
                if (lockRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("无效的解锁请求数据");
                }

                // 验证请求数据
                if (lockRequest.LockInfo.BillID <= 0)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("单据ID不能为空");
                }



                // 释放锁定
                var unlockSuccess = await _lockManagerService.UnlockDocumentAsync(lockRequest.LockInfo.BillID, lockRequest.LockedUserId);

                if (unlockSuccess)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();
                    var response = new LockResponse
                    {
                        IsSuccess = true,
                        Message = "单据解锁成功",
                        LockInfo = lockRequest.LockInfo
                    };
                    return response;
                }
                else
                {
                    var response = new LockResponse
                    {
                        IsSuccess = false,
                        Message = "单据解锁失败",
                        LockInfo = lockRequest.LockInfo
                    };

                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁定释放异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse<LockResponse>($"处理锁定释放异常: {ex.Message}");
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
                // 反序列化请求数据
                var lockRequest = cmd.Packet.Request as LockRequest;
                if (lockRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("无效的强制解锁请求数据");
                }

                // 验证请求数据
                if (lockRequest.LockInfo.BillID <= 0)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("单据ID不能为空");
                }


                // 强制解锁
                var forceUnlockResult = await _lockManagerService.ForceUnlockDocumentAsync(lockRequest.LockInfo.BillID);
                if (forceUnlockResult)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();

                    var response = ResponseFactory.CreateSpecificSuccessResponse(cmd.Packet.ExecutionContext, "单据强制解锁成功");
                    return response;
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("单据强制解锁失败");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理强制解锁异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理强制解锁异常: {ex.Message}");
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
                // 反序列化请求数据
                var lockRequest = cmd.Packet.Request as LockRequest;
                if (lockRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("无效的请求解锁数据");
                }

                // 验证请求数据
                if (lockRequest.LockInfo.BillID <= 0)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("单据ID不能为空");
                }


                // 请求解锁
                var requestSuccess = await _lockManagerService.RequestUnlockDocumentAsync(lockRequest);
                if (requestSuccess)
                {
                    var response = ResponseFactory.CreateSpecificSuccessResponse(cmd.Packet.ExecutionContext, "请求解锁处理完成");
                    return response;
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("请求解锁失败");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理请求解锁异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理请求解锁异常: {ex.Message}");
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
                // 反序列化请求数据
                var lockRequest = cmd.Packet.Request as LockRequest;
                if (lockRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("无效的拒绝解锁数据");
                }

                // 验证请求数据
                if (lockRequest.LockInfo.BillID <= 0)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("单据ID不能为空");
                }


                // 拒绝解锁
                var refuseSuccess = await _lockManagerService.RefuseUnlockRequestAsync(lockRequest);

                if (refuseSuccess)
                {
                    var response = ResponseFactory.CreateSpecificSuccessResponse(cmd.Packet.ExecutionContext, "拒绝解锁处理完成");
                    return response;
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("拒绝解锁失败");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理拒绝解锁异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理拒绝解锁异常: {ex.Message}");
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
                // 反序列化请求数据
                var lockRequest = cmd.Packet.Request as LockRequest;
                if (lockRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("无效的查询锁状态数据");
                }

                // 验证请求数据
                if (lockRequest.LockInfo.BillID <= 0)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("单据ID不能为空");
                }

                // 查询锁状态
                var lockInfo = _lockManagerService.GetLockInfo(lockRequest.LockInfo.BillID);

                var response = ResponseFactory.CreateSpecificSuccessResponse(cmd.Packet.ExecutionContext, "锁状态查询成功") as LockResponse;
                response.LockInfo = lockInfo;
                response.Status = lockInfo?.Status ?? LockStatus.Unlocked;
                response.RemainingLockTimeMs = lockInfo?.RemainingLockTimeMs ?? 0;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理查询锁状态异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理查询锁状态异常: {ex.Message}");
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
                // 反序列化请求数据
                var lockRequest = cmd.Packet.Request as LockRequest;
                if (lockRequest == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<LockResponse>("无效的广播锁状态数据");
                }

                // 广播锁状态
                await BroadcastLockStatusToAllClientsAsync(new List<LockInfo> { lockRequest.LockInfo });

                var response = ResponseFactory.CreateSpecificSuccessResponse(cmd.Packet.ExecutionContext, "锁状态广播成功");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理广播锁状态异常: {ex.Message}");
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"广播锁状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 广播锁定状态变化到所有客户端
        /// </summary>
        /// <param name="lockedDocuments">锁定的单据信息列表</param>
        private async Task BroadcastLockStatusToAllClientsAsync(IEnumerable<LockInfo> lockedDocuments)
        {
            try
            {
                // 创建广播数据
                var broadcastData = new LockRequest
                {
                    LockedDocuments = lockedDocuments?.ToList() ?? new List<LockInfo>(),
                    Timestamp = DateTime.UtcNow
                };


                // 获取所有用户会话
                var sessions = _sessionService.GetAllUserSessions();

                // 向所有会话发送消息并等待响应
                int successCount = 0;
                foreach (var session in sessions)
                {
                    if (session is SessionInfo sessionInfo)
                    {
                        var responsePacket = await _sessionService.SendCommandAndWaitForResponseAsync(
                            session.SessionID,
                         LockCommands.BroadcastLockStatus,
                            broadcastData
                             );

                        if (responsePacket?.Response is MessageResponse response && response.IsSuccess)
                        {
                            successCount++;
                        }
                    }
                }

                //await _sessionService.SendCommandAsync(LockCommands.BroadcastLockStatus, broadcastRequest);

                _logger.LogInformation($"广播锁定状态变化，当前锁定单据数: {lockedDocuments?.Count() ?? 0}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"广播锁定状态变化到所有客户端时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 广播锁定状态变化
        /// </summary>
        private async Task BroadcastLockStatusAsync()
        {
            try
            {
                // 获取所有锁定的单据信息
                var lockedDocuments = _lockManagerService.GetAllLockedDocuments();

                // 广播到所有客户端
                await BroadcastLockStatusToAllClientsAsync(lockedDocuments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"广播锁定状态变化时发生异常: {ex.Message}");
            }
        }
    }
}