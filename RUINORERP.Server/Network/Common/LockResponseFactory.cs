using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Lock;
using RUINORERP.PacketSpec.Models.Responses;
using System;

namespace RUINORERP.Server.Network.Common
{
    /// <summary>
    /// 锁定相关响应工厂 - 统一错误响应创建机制
    /// 提供标准化的错误响应创建，减少重复代码
    /// </summary>
    public static class LockResponseFactory
    {
        /// <summary>
        /// 创建标准错误响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="billId">单据ID（可选）</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreateError(string message, long billId = 0)
        {
            return new LockResponse
            {
                IsSuccess = false,
                Message = message,
                LockInfo = billId > 0 ? new LockInfo { BillID = billId } : null,
                RemainingLockTimeMs = 0
            };
        }

        /// <summary>
        /// 创建参数验证错误响应
        /// </summary>
        /// <param name="parameterName">参数名称</param>
        /// <param name="billId">单据ID（可选）</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreateParameterError(string parameterName, long billId = 0)
        {
            return CreateError($"{parameterName}无效", billId);
        }

        /// <summary>
        /// 创建单据ID无效错误响应
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreateInvalidBillIdError(long billId)
        {
            return CreateError("单据ID无效", billId);
        }

        /// <summary>
        /// 创建用户ID无效错误响应
        /// </summary>
        /// <param name="billId">单据ID（可选）</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreateInvalidUserIdError(long billId = 0)
        {
            return CreateError("用户ID无效", billId);
        }

        /// <summary>
        /// 创建请求数据无效错误响应
        /// </summary>
        /// <param name="operationName">操作名称</param>
        /// <param name="billId">单据ID（可选）</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreateInvalidRequestError(string operationName, long billId = 0)
        {
            return CreateError($"无效的{operationName}请求数据", billId);
        }

        /// <summary>
        /// 创建权限不足错误响应
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="currentUserId">当前用户ID</param>
        /// <param name="lockUserId">锁持有者ID</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreatePermissionError(long billId, long currentUserId, long lockUserId)
        {
            return CreateError($"无权限操作单据 {billId}，锁持有者: {lockUserId}，当前用户: {currentUserId}", billId);
        }

        /// <summary>
        /// 创建锁不存在错误响应
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreateLockNotExistsError(long billId)
        {
            return CreateError($"单据 {billId} 未被锁定或不存在", billId);
        }

        /// <summary>
        /// 创建锁已被占用错误响应
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="lockedByUser">锁持有者</param>
        /// <returns>错误响应</returns>
        public static LockResponse CreateAlreadyLockedError(long billId, string lockedByUser)
        {
            return CreateError($"单据 {billId} 已被用户 {lockedByUser} 锁定", billId);
        }

        /// <summary>
        /// 创建操作成功响应
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="lockInfo">锁信息（可选）</param>
        /// <returns>成功响应</returns>
        public static LockResponse CreateSuccess(string message, LockInfo lockInfo = null)
        {
            return new LockResponse
            {
                IsSuccess = true,
                Message = message,
                LockInfo = lockInfo,
                Status = lockInfo?.Status ?? LockStatus.Unlocked,
                RemainingLockTimeMs = lockInfo?.RemainingLockTimeMs ?? 0
            };
        }

        /// <summary>
        /// 创建解锁成功响应
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>解锁成功响应</returns>
        public static LockResponse CreateUnlockSuccess(long billId, string operationType = "解锁")
        {
            return CreateSuccess($"{operationType}成功", new LockInfo { BillID = billId });
        }

        /// <summary>
        /// 创建异常错误响应
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="operationName">操作名称</param>
        /// <param name="billId">单据ID（可选）</param>
        /// <returns>异常错误响应</returns>
        public static LockResponse CreateExceptionError(Exception ex, string operationName, long billId = 0)
        {
            return CreateError($"{operationName}异常: {ex.Message}", billId);
        }
    }

    /// <summary>
    /// 锁定操作结果 - 统一操作结果返回
    /// </summary>
    public class LockOperationResult
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 锁信息
        /// </summary>
        public LockInfo LockInfo { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 操作耗时（毫秒）
        /// </summary>
        public long ElapsedMs { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="lockInfo">锁信息</param>
        /// <param name="elapsedMs">耗时</param>
        /// <returns>成功结果</returns>
        public static LockOperationResult Success(LockInfo lockInfo = null, long elapsedMs = 0)
        {
            return new LockOperationResult
            {
                IsSuccess = true,
                LockInfo = lockInfo,
                ElapsedMs = elapsedMs
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="elapsedMs">耗时</param>
        /// <returns>失败结果</returns>
        public static LockOperationResult Failure(string errorMessage, long elapsedMs = 0)
        {
            return new LockOperationResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                ElapsedMs = elapsedMs
            };
        }

        /// <summary>
        /// 转换为LockResponse
        /// </summary>
        /// <returns>LockResponse</returns>
        public LockResponse ToResponse()
        {
            if (IsSuccess)
            {
                return LockResponseFactory.CreateSuccess("操作成功", LockInfo);
            }
            else
            {
                return LockResponseFactory.CreateError(ErrorMessage, LockInfo?.BillID ?? 0);
            }
        }
    }
}