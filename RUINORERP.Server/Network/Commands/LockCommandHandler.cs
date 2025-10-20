using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Lock;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Protocol; // 添加这个using语句以使用OriginalData

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 锁命令处理器 - 处理锁相关的命令
    /// </summary>
    [CommandHandler("LockCommandHandler", priority: CommandPriority.High)]
    public class LockCommandHandler : CommandHandlerBase
    {
        private readonly ILogger<LockCommandHandler> _logger;

        public LockCommandHandler(ILogger<LockCommandHandler> logger)
        {
            _logger = logger;
            
            // 使用安全方法设置支持的命令
            SetSupportedCommands(
                LockCommands.LockRequest.FullCode,
                LockCommands.LockRelease.FullCode,
                LockCommands.LockStatus.FullCode,
                LockCommands.ForceUnlock.FullCode,
                LockCommands.RequestUnlock.FullCode,
                LockCommands.RefuseUnlock.FullCode,
                LockCommands.RequestLock.FullCode,
                LockCommands.ReleaseLock.FullCode,
                LockCommands.ForceReleaseLock.FullCode,
                LockCommands.QueryLockStatus.FullCode,
                LockCommands.BroadcastLockStatus.FullCode
            );
        }

        /// <summary>
        /// 锁管理服务
        /// </summary>
        private ILockManagerService LockManagerService => Program.ServiceProvider.GetRequiredService<ILockManagerService>();

        /// <summary>
        /// 会话管理服务
        /// </summary>
        private ISessionService SessionService => Program.ServiceProvider.GetRequiredService<ISessionService>();

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands { get; protected set; }

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override CommandPriority Priority => CommandPriority.High;

        /// <summary>
        /// 判断是否可以处理指定命令
        /// </summary>
        public override bool CanHandle(PacketModel packet)
        {
            return SupportedCommands.Contains(packet.CommandId.FullCode);
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        /// <param name="packet">数据包对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令处理结果</returns>
        protected override async Task<ResponseBase> ProcessCommandAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = packet.CommandId;
                
                switch (commandId.FullCode)
                {
                    case var id when id == LockCommands.LockRequest.FullCode:
                        return await HandleLockRequestAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.LockRelease.FullCode:
                        return await HandleLockReleaseAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.ForceUnlock.FullCode:
                        return await HandleForceUnlockAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.RequestUnlock.FullCode:
                        return await HandleRequestUnlockAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.RefuseUnlock.FullCode:
                        return await HandleRefuseUnlockAsync(packet, cancellationToken);
                        
                    // 注意：这里需要修改所有剩余的方法调用参数类型，从ICommand改为PacketModel
                        
                    case var id when id == LockCommands.RequestLock.FullCode:
                        return await HandleDocumentLockRequestAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.ReleaseLock.FullCode:
                        return await HandleDocumentUnlockRequestAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.ForceReleaseLock.FullCode:
                        return await HandleForceUnlockDocumentAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.QueryLockStatus.FullCode:
                        return await HandleQueryLockStatusAsync(packet, cancellationToken);
                        
                    case var id when id == LockCommands.BroadcastLockStatus.FullCode:
                        return await HandleBroadcastLockStatusAsync(packet, cancellationToken);
                        
                    default:
                        return BaseCommand<IResponse>.CreateError($"不支持的锁命令类型: {command.CommandIdentifier}", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                            .WithMetadata("ErrorCode", "UNSUPPORTED_LOCK_COMMAND");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"处理锁命令异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "LOCK_HANDLER_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理单据解锁请求命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleDocumentUnlockRequestAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取锁申请数据
                var lockApplyCommand = command as LockApplyCommand;
                if (lockApplyCommand == null)
                {
                    return BaseCommand<IResponse>.CreateError("无效的锁申请命令", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_LOCK_APPLY_COMMAND");
                }

                // 验证命令数据
                var validationResult = await lockApplyCommand.ValidateAsync(cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BaseCommand<IResponse>.CreateError(validationResult.Errors[0].ErrorMessage, UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_LOCK_REQUEST");
                }

                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 创建锁定信息
                var lockInfo = new LockedInfo
                {
                    PacketId = Guid.NewGuid(),
                    BillID = long.Parse(lockApplyCommand.ResourceId), // 假设ResourceId是单据ID
                    LockedUserID = sessionInfo.UserId ?? 0,
                    LockedUserName = sessionInfo.Username
                };

                // 尝试锁定单据
                var lockResult = await LockManagerService.TryLockDocumentAsync(lockInfo);
                
                if (lockResult)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();
                    
                    var response = new ResponseBase
                    {
                        Message = "单据锁定成功",
                        IsSuccess = true
                    };
                    response.WithMetadata("BillID", lockInfo.BillID);
                    response.WithMetadata("IsLocked", true);
                    return BaseCommand<IResponse>.CreateSuccess(response);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("单据锁定失败，可能已被其他用户锁定", UnifiedErrorCodes.Biz_Conflict.Code)
                        .WithMetadata("ErrorCode", "DOCUMENT_ALREADY_LOCKED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁申请命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"锁申请异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "LOCK_APPLY_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理锁释放命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleLockReleaseAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 从命令中获取资源ID（假设通过Packet传递）
                var resourceId = command.Packet != null ? 
                    System.Text.Encoding.UTF8.GetString(command.Packet.Body ?? new byte[0]) : 
                    string.Empty;

                if (string.IsNullOrEmpty(resourceId))
                {
                    return BaseCommand<IResponse>.CreateError("资源ID不能为空", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "EMPTY_RESOURCE_ID");
                }

                if (!long.TryParse(resourceId, out var billId))
                {
                    return BaseCommand<IResponse>.CreateError("无效的资源ID格式", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_RESOURCE_ID_FORMAT");
                }

                // 解锁单据
                var unlockResult = await LockManagerService.UnlockDocumentAsync(billId, sessionInfo.UserId ?? 0);
                
                if (unlockResult)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();
                    
                    return BaseCommand<IResponse>.CreateSuccess(new { BillID = billId, IsLocked = false })
                        .WithMetadata("Message", "单据解锁成功");
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("单据解锁失败", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "UNLOCK_FAILED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理锁释放命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"锁释放异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "LOCK_RELEASE_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理强制解锁命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleForceUnlockAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 检查是否是管理员用户（这里简化处理，实际应该检查权限）
                // var isAdmin = sessionInfo.IsAdmin; // 假设有这个属性

                // 从命令中获取资源ID
                var resourceId = command.Packet != null ? 
                    System.Text.Encoding.UTF8.GetString(command.Packet.Body ?? new byte[0]) : 
                    string.Empty;

                if (string.IsNullOrEmpty(resourceId))
                {
                    return BaseCommand<IResponse>.CreateError("资源ID不能为空", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "EMPTY_RESOURCE_ID");
                }

                if (!long.TryParse(resourceId, out var billId))
                {
                    return BaseCommand<IResponse>.CreateError("无效的资源ID格式", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_RESOURCE_ID_FORMAT");
                }

                // 强制解锁单据
                var forceUnlockResult = await LockManagerService.ForceUnlockDocumentAsync(billId);
                
                if (forceUnlockResult)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();
                    
                    return BaseCommand<IResponse>.CreateSuccess(new { BillID = billId, IsLocked = false })
                        .WithMetadata("Message", "单据强制解锁成功");
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("单据强制解锁失败", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "FORCE_UNLOCK_FAILED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理强制解锁命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"强制解锁异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "FORCE_UNLOCK_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理请求解锁命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleRequestUnlockAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 获取请求解锁信息
                var requestInfo = command.Packet?.GetJsonData<RequestUnLockInfo>();
                if (requestInfo == null)
                {
                    return BaseCommand<IResponse>.CreateError("请求解锁信息不能为空", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "EMPTY_UNLOCK_REQUEST");
                }

                // 设置请求用户信息
                requestInfo.RequestUserID = sessionInfo.UserId ?? 0;
                requestInfo.RequestUserName = sessionInfo.Username;

                // 请求解锁单据
                var requestResult = await LockManagerService.RequestUnlockDocumentAsync(requestInfo);
                
                if (requestResult)
                {
                    var response = new ResponseBase
                    {
                        Message = "解锁请求已发送",
                        IsSuccess = true
                    };
                    response.WithMetadata("BillID", requestInfo.BillID);
                    response.WithMetadata("RequestUserID", requestInfo.RequestUserID);
                    return BaseCommand<IResponse>.CreateSuccess(response);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("解锁请求发送失败", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "UNLOCK_REQUEST_FAILED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理请求解锁命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"请求解锁异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "UNLOCK_REQUEST_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理拒绝解锁命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleRefuseUnlockAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 获取拒绝解锁信息
                var refuseInfo = command.Packet?.GetJsonData<RefuseUnLockInfo>();
                if (refuseInfo == null)
                {
                    return BaseCommand<IResponse>.CreateError("拒绝解锁信息不能为空", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "EMPTY_REFUSE_UNLOCK_REQUEST");
                }

                // 设置拒绝用户信息
                refuseInfo.RefuseUserID = sessionInfo.UserId ?? 0;
                refuseInfo.RefuseUserName = sessionInfo.Username;

                // 拒绝解锁请求
                var refuseResult = await LockManagerService.RefuseUnlockRequestAsync(refuseInfo);
                
                if (refuseResult)
                {
                    var response = new ResponseBase
                    {
                        Message = "已拒绝解锁请求",
                        IsSuccess = true
                    };
                    response.WithMetadata("BillID", refuseInfo.BillID);
                    response.WithMetadata("RefuseUserID", refuseInfo.RefuseUserID);
                    return BaseCommand<IResponse>.CreateSuccess(response);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("拒绝解锁请求失败", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "REFUSE_UNLOCK_FAILED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理拒绝解锁命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"拒绝解锁异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "REFUSE_UNLOCK_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理单据锁定申请命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleDocumentLockRequestAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取单据锁定申请数据
                var documentLockApplyCommand = command as DocumentLockApplyCommand;
                if (documentLockApplyCommand == null)
                {
                    return BaseCommand<IResponse>.CreateError("无效的单据锁定申请命令", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_DOCUMENT_LOCK_APPLY_COMMAND");
                }

                // 验证命令数据
                var validationResult = await documentLockApplyCommand.ValidateAsync(cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BaseCommand<IResponse>.CreateError(validationResult.Errors[0].ErrorMessage, UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_DOCUMENT_LOCK_REQUEST");
                }

                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 创建锁定信息
                var lockInfo = new LockedInfo
                {
                    PacketId = Guid.NewGuid(),
                    BillID = documentLockApplyCommand.BillId,
                    BillData = documentLockApplyCommand.BillData,
                    MenuID = documentLockApplyCommand.MenuId,
                    LockedUserID = sessionInfo.UserId ?? 0,
                    LockedUserName = sessionInfo.Username
                };

                // 尝试锁定单据
                var lockResult = await LockManagerService.TryLockDocumentAsync(lockInfo);
                
                if (lockResult)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();
                    
                    return BaseCommand<IResponse>.CreateSuccess(new { BillID = lockInfo.BillID, IsLocked = true })
                        .WithMetadata("Message", "单据锁定成功");
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("单据锁定失败，可能已被其他用户锁定", UnifiedErrorCodes.Biz_Conflict.Code)
                        .WithMetadata("ErrorCode", "DOCUMENT_ALREADY_LOCKED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理单据锁定申请命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"单据锁定申请异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "DOCUMENT_LOCK_APPLY_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理锁申请命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleLockRequestAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取单据解锁申请数据
                var documentUnlockCommand = command as DocumentUnlockCommand;
                if (documentUnlockCommand == null)
                {
                    return BaseCommand<IResponse>.CreateError("无效的单据解锁申请命令", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_DOCUMENT_UNLOCK_COMMAND");
                }

                // 验证命令数据
                var validationResult = await documentUnlockCommand.ValidateAsync(cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BaseCommand<IResponse>.CreateError(validationResult.Errors[0].ErrorMessage, UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_DOCUMENT_UNLOCK_REQUEST");
                }

                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 解锁单据
                var unlockResult = await LockManagerService.UnlockDocumentAsync(
                    documentUnlockCommand.BillId, 
                    sessionInfo.UserId ?? 0);
                
                if (unlockResult)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();
                    
                    var response = new ResponseBase
                    {
                        Message = "单据解锁成功",
                        IsSuccess = true
                    };
                    response.WithMetadata("BillID", documentUnlockCommand.BillId);
                    response.WithMetadata("IsLocked", false);
                    return BaseCommand<IResponse>.CreateSuccess(response);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("单据解锁失败，可能不是锁定该单据的用户", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "DOCUMENT_UNLOCK_FAILED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理单据解锁申请命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"单据解锁申请异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "DOCUMENT_UNLOCK_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理强制解锁单据命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleForceUnlockDocumentAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取强制解锁申请数据
                var forceUnlockCommand = command as ForceUnlockCommand;
                if (forceUnlockCommand == null)
                {
                    return BaseCommand<IResponse>.CreateError("无效的强制解锁申请命令", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_FORCE_UNLOCK_COMMAND");
                }

                // 验证命令数据
                var validationResult = await forceUnlockCommand.ValidateAsync(cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BaseCommand<IResponse>.CreateError(validationResult.Errors[0].ErrorMessage, UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_FORCE_UNLOCK_REQUEST");
                }

                // 获取会话信息
                var sessionInfo = SessionService.GetSession(command.SessionId);
                if (sessionInfo == null || !sessionInfo.IsAuthenticated)
                {
                    return BaseCommand<IResponse>.CreateError("会话无效或未认证", UnifiedErrorCodes.Auth_SessionExpired.Code)
                        .WithMetadata("ErrorCode", "INVALID_SESSION");
                }

                // 检查是否是管理员用户（这里简化处理，实际应该检查权限）
                // var isAdmin = sessionInfo.IsAdmin; // 假设有这个属性

                // 强制解锁单据
                var forceUnlockResult = await LockManagerService.ForceUnlockDocumentAsync(forceUnlockCommand.BillId);
                
                if (forceUnlockResult)
                {
                    // 广播锁定状态变化
                    await BroadcastLockStatusAsync();
                    
                    var response = new ResponseBase
                    {
                        Message = "单据强制解锁成功",
                        IsSuccess = true
                    };
                    response.WithMetadata("BillID", forceUnlockCommand.BillId);
                    response.WithMetadata("IsLocked", false);
                    return BaseCommand<IResponse>.CreateSuccess(response);
                }
                else
                {
                    return BaseCommand<IResponse>.CreateError("单据强制解锁失败", UnifiedErrorCodes.Biz_OperationFailed.Code)
                        .WithMetadata("ErrorCode", "DOCUMENT_FORCE_UNLOCK_FAILED");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理强制解锁单据命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"强制解锁单据异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "DOCUMENT_FORCE_UNLOCK_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理查询锁状态命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleQueryLockStatusAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取查询锁状态申请数据
                var queryLockStatusCommand = command as QueryLockStatusCommand;
                if (queryLockStatusCommand == null)
                {
                    return BaseCommand<IResponse>.CreateError("无效的查询锁状态申请命令", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_QUERY_LOCK_STATUS_COMMAND");
                }

                // 验证命令数据
                var validationResult = await queryLockStatusCommand.ValidateAsync(cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BaseCommand<IResponse>.CreateError(validationResult.Errors[0].ErrorMessage, UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_QUERY_LOCK_STATUS_REQUEST");
                }

                // 获取单据锁定信息
                var lockInfo = LockManagerService.GetLockInfo(queryLockStatusCommand.BillId);
                
                var response = new ResponseBase
                {
                    Message = "锁状态查询成功",
                    IsSuccess = true
                };
                response.WithMetadata("BillID", queryLockStatusCommand.BillId);
                response.WithMetadata("IsLocked", lockInfo != null);
                response.WithMetadata("LockInfo", lockInfo);
                return BaseCommand<IResponse>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理查询锁状态命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"查询锁状态异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "QUERY_LOCK_STATUS_EXCEPTION");
            }
        }

        /// <summary>
        /// 处理广播锁状态命令
        /// </summary>
        private async Task<BaseCommand<IResponse>> HandleBroadcastLockStatusAsync(PacketModel packet, CancellationToken cancellationToken)
        {
            try
            {
                // 获取广播锁状态申请数据
                var broadcastLockStatusCommand = command as BroadcastLockStatusCommand;
                if (broadcastLockStatusCommand == null)
                {
                    return BaseCommand<IResponse>.CreateError("无效的广播锁状态命令", UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_BROADCAST_LOCK_STATUS_COMMAND");
                }

                // 验证命令数据
                var validationResult = await broadcastLockStatusCommand.ValidateAsync(cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BaseCommand<IResponse>.CreateError(validationResult.Errors[0].ErrorMessage, UnifiedErrorCodes.Biz_ValidationFailed.Code)
                        .WithMetadata("ErrorCode", "INVALID_BROADCAST_LOCK_STATUS_REQUEST");
                }

                // 获取会话信息（广播命令通常不需要会话信息）

                // 广播锁定状态变化到所有客户端
                await BroadcastLockStatusToAllClientsAsync(broadcastLockStatusCommand.LockedDocuments);
                
                var response = new ResponseBase
                {
                    Message = "锁状态广播成功",
                    IsSuccess = true
                };
                response.WithMetadata("LockedDocumentsCount", broadcastLockStatusCommand.LockedDocuments?.Count ?? 0);
                return BaseCommand<IResponse>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"处理广播锁状态命令异常: {ex.Message}");
                return BaseCommand<IResponse>.CreateError($"广播锁状态异常: {ex.Message}", UnifiedErrorCodes.System_InternalError.Code)
                    .WithMetadata("ErrorCode", "BROADCAST_LOCK_STATUS_EXCEPTION");
            }
        }

        /// <summary>
        /// 广播锁定状态变化到所有客户端
        /// </summary>
        /// <param name="lockedDocuments">锁定的单据信息列表</param>
        private async Task BroadcastLockStatusToAllClientsAsync(List<LockedInfo> lockedDocuments)
        {
            try
            {
                // 创建广播命令
                var broadcastCommand = new BroadcastLockStatusCommand(lockedDocuments);
                
                // 序列化数据
                var serializedData = RUINORERP.PacketSpec.Serialization.UnifiedSerializationService.SerializeToBinary(broadcastCommand);
                
                // 创建原始数据包
                var originalData = new OriginalData(
                    (byte)CommandCategory.Lock,
                    new byte[] { LockCommands.BroadcastLockStatus.OperationCode },
                    serializedData
                );
                
                // 向所有在线客户端广播锁定状态更新
                // 修复：使用正确的广播方法
                await SessionService.BroadcastMessageAsync(originalData.ToByteArray());
                
                _logger.LogInformation($"广播锁定状态变化，当前锁定单据数: {lockedDocuments?.Count ?? 0}");
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
                var lockedDocuments = LockManagerService.GetAllLockedDocuments();
                
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