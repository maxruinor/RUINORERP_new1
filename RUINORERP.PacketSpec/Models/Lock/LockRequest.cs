using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Lock
{
    /// <summary>
    /// 锁定请求类
    /// 统一的锁定请求结构，用于所有锁定操作（锁定、解锁、请求解锁、刷新锁定等）
    /// </summary>
    public class LockRequest : RequestBase
    {
        /// <summary>
        /// 锁定信息
        /// 包含所有锁定相关的核心数据
        /// </summary>
        public LockInfo LockInfo { get; set; } = new LockInfo();

        /// <summary>
        /// 解锁类型
        /// 仅在解锁操作时使用
        /// </summary>
        public UnlockType UnlockType { get; set; } = UnlockType.Normal;

        /// <summary>
        /// 请求者用户ID
        /// 仅在请求解锁操作时使用
        /// </summary>
        public long RequesterUserId { get; set; }

        /// <summary>
        /// 请求者用户名
        /// 仅在请求解锁操作时使用
        /// </summary>
        public string RequesterUserName { get; set; } = string.Empty;

        /// <summary>
        /// 被请求解锁的用户ID
        /// 仅在请求解锁操作时使用
        /// </summary>
        public long LockedUserId { get; set; }

        /// <summary>
        /// 被请求解锁的用户名
        /// 仅在请求解锁操作时使用
        /// </summary>
        public string LockedUserName { get; set; } = string.Empty;

        /// <summary>
        /// 请求解锁的原因
        /// 仅在请求解锁操作时使用
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// 要广播的锁定文档列表
        /// 仅在广播锁定状态时使用
        /// </summary>
        public List<LockInfo> LockedDocuments { get; set; } = new List<LockInfo>();

        /// <summary>
        /// 锁定超时时间（毫秒）
        /// 默认5分钟
        /// </summary>
        public int TimeoutMs { get; set; } = 300000;
        public bool RefreshMode { get; set; }

        /// <summary>
        /// 初始化锁定请求
        /// </summary>
        public LockRequest() { }

        /// <summary>
        /// 创建锁定单据请求
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名称</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="billData">单据数据</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="lockReason">锁定原因</param>
        /// <param name="timeoutMs">超时时间(毫秒)</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateLockRequest(long billId, long userId, string userName, long menuId,
            CommBillData? billData, string sessionId, string lockReason = "", int timeoutMs = 300000)
        {
            var expireTime = DateTime.Now.AddMilliseconds(timeoutMs);
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                    BillID = billId,
                    UserId = userId,
                    UserName = userName,
                    MenuID = menuId,
                    BillData = billData,
                    SessionId = sessionId,
                    ExpireTime = expireTime,
                    Remark = lockReason
                },
                TimeoutMs = timeoutMs
            };
        }

        /// <summary>
        /// 创建解锁单据请求
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="lockId">锁定ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="unlockType">解锁类型</param>
        /// <param name="unlockReason">解锁原因</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateUnlockRequest(long billId, long userId, string lockId, string sessionId,
            UnlockType unlockType = UnlockType.Normal, string unlockReason = "")
        {
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                    BillID = billId,
                    UserId = userId,
                    LockId = lockId,
                    SessionId = sessionId,
                    Remark = unlockReason
                },
                UnlockType = unlockType
            };
        }

        /// <summary>
        /// 创建强制解锁请求
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="operatorUserId">操作员用户ID</param>
        /// <param name="operatorUserName">操作员用户名</param>
        /// <param name="lockId">锁定ID</param>
        /// <param name="reason">强制解锁原因</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateForceUnlockRequest(long billId, long operatorUserId, string operatorUserName,
            string lockId, string reason = "")
        {
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                    BillID = billId,
                    UserId = operatorUserId,
                    UserName = operatorUserName,
                    LockId = lockId,
                    Remark = reason
                },
                UnlockType = UnlockType.Force
            };
        }

        /// <summary>
        /// 创建请求解锁请求
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="requesterUserId">请求者用户ID</param>
        /// <param name="requesterUserName">请求者用户名</param>
        /// <param name="lockedUserId">被请求解锁的用户ID</param>
        /// <param name="lockedUserName">被请求解锁的用户名</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="billData">单据数据</param>
        /// <param name="reason">请求原因</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateRequestUnlockRequest(long billId, long requesterUserId, string requesterUserName,
            long lockedUserId, string lockedUserName, long menuId, CommBillData? billData, string reason = "")
        {
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                    BillID = billId,
                    MenuID = menuId,
                    BillData = billData,
                },
                RequesterUserId = requesterUserId,
                RequesterUserName = requesterUserName,
                LockedUserId = lockedUserId,
                LockedUserName = lockedUserName,
                Reason = reason
            };
        }

        /// <summary>
        /// 创建刷新锁定请求
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="lockId">锁定ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="timeoutMs">超时时间(毫秒)</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateRefreshLockRequest(long billId, long userId, string lockId, string sessionId,
            int timeoutMs = 300000)
        {
            var expireTime = DateTime.Now.AddMilliseconds(timeoutMs);
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                    BillID = billId,
                    UserId = userId,
                    LockId = lockId,
                    SessionId = sessionId,
                    ExpireTime = expireTime
                },
                TimeoutMs = timeoutMs
            };
        }

        /// <summary>
        /// 创建查询锁定状态请求
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateQueryLockStatusRequest(long billId)
        {
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                    BillID = billId,
                }
            };
        }

        /// <summary>
        /// 创建拒绝解锁请求
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="refuseUserId">拒绝用户ID</param>
        /// <param name="requestUserId">请求用户ID</param>
        /// <param name="refuseReason">拒绝原因</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateRefuseUnlockRequest(long billId, long refuseUserId, long requestUserId, string refuseReason = "")
        {
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                    BillID = billId,
                    UserId = refuseUserId,
                    Remark = refuseReason
                },
                RequesterUserId = requestUserId,
                Reason = refuseReason
            };
        }

        /// <summary>
        /// 创建广播锁定请求
        /// </summary>
        /// <param name="lockedDocuments">锁定文档列表</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateBroadcastLockRequest(List<LockInfo> lockedDocuments)
        {
            return new LockRequest
            {
                LockInfo = new LockInfo
                {
                },
                LockedDocuments = lockedDocuments
            };
        }
    }

    /// <summary>
    /// 解锁类型枚举
    /// 定义了系统中所有可能的解锁操作类型
    /// </summary>
    public enum UnlockType
    {
        /// <summary>
        /// 普通解锁
        /// 由锁定者自己执行的正常解锁操作
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 强制解锁
        /// 通常由管理员执行，强制解除其他用户的锁定
        /// </summary>
        Force = 1,

        /// <summary>
        /// 过期自动解锁
        /// 当锁定超过有效期后，系统自动执行的解锁操作
        /// </summary>
        Expired = 2,

        /// <summary>
        /// 会话结束解锁
        /// 当用户会话结束时自动执行的解锁操作
        /// </summary>
        SessionEnd = 3,

        /// <summary>
        /// 响应解锁请求
        /// 锁定者响应其他用户的解锁请求而执行的解锁操作
        /// </summary>
        RequestResponse = 4,

        /// <summary>
        /// 意思是按业务类型将这个类型的批量全部解锁
        /// </summary>
        ByBizName = 5
    }
}
