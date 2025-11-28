using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Lock
{
    /// <summary>
    /// 锁定响应类
    /// 统一的锁定响应结构，用于所有锁定操作的响应（锁定、解锁、请求解锁、刷新锁定、查询状态等）
    /// </summary>
    public class LockResponse : ResponseBase
    {
        /// <summary>
        /// 锁定信息
        /// 包含锁定操作相关的核心数据
        /// </summary>
        public LockInfo LockInfo { get; set; } = new LockInfo();

        /// <summary>
        /// 锁定状态集合
        /// 用于批量查询或广播操作时返回多个锁定状态
        /// </summary>
        public List<LockInfo> LockInfoList { get; set; } = new List<LockInfo>();

        /// <summary>
        /// 锁定状态
        /// 表示单据当前的锁定状态
        /// </summary>
        public LockStatus Status { get; set; } = LockStatus.Unlocked;

        /// <summary>
        /// 锁定剩余时间（毫秒）
        /// 表示当前锁定还有多长时间过期
        /// </summary>
        public long RemainingLockTimeMs { get; set; }

        /// <summary>
        /// 锁定请求的拒绝原因
        /// 当锁定请求被拒绝时提供详细原因
        /// </summary>
        public string DenialReason { get; set; } = string.Empty;

        /// <summary>
        /// 响应时间
        /// 服务器处理响应的时间戳
        /// </summary>
        public DateTime ResponseTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 初始化锁定响应
        /// </summary>
        public LockResponse()
        {
            ResponseTime = DateTime.Now;
        }

        /// <summary>
        /// 初始化锁定响应
        /// </summary>
        /// <param name="success">操作是否成功</param>
        /// <param name="message">响应消息</param>
        public LockResponse(bool success, string message)
        {
            IsSuccess = success;
            Message = message;
            ResponseTime = DateTime.Now;
        }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定响应实例</returns>
        public static LockResponse SuccessResponse(string message = "操作成功", LockInfo lockInfo = null)
        {
            return new LockResponse
            {
                IsSuccess = true,
                Message = message,
                LockInfo = lockInfo ?? new LockInfo(),
                Status = lockInfo?.IsLocked == true ? LockStatus.Locked : LockStatus.Unlocked,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="denialReason">详细拒绝原因</param>
        /// <returns>锁定响应实例</returns>
        public static LockResponse FailedResponse(string message = "操作失败", int errorCode = 1, string denialReason = "")
        {
            return new LockResponse
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                DenialReason = denialReason,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建锁定成功响应
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <param name="message">成功消息</param>
        /// <returns>锁定响应实例</returns>
        public static LockResponse LockSuccessResponse(LockInfo lockInfo, string message = "单据锁定成功")
        {
            long remainingTime = 0;
            if (lockInfo.ExpireTime.HasValue)
            {
                remainingTime = (long)(lockInfo.ExpireTime.Value - DateTime.Now).TotalMilliseconds;
                remainingTime = Math.Max(0, remainingTime);
            }

            return new LockResponse
            {
                IsSuccess = true,
                Message = message,
                LockInfo = lockInfo,
                Status = LockStatus.Locked,
                RemainingLockTimeMs = remainingTime,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建锁定失败响应
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <param name="message">失败消息</param>
        /// <param name="denialReason">详细拒绝原因</param>
        /// <returns>锁定响应实例</returns>
        public static LockResponse LockFailedResponse(LockInfo lockInfo, string message = "单据锁定失败", string denialReason = "")
        {
            return new LockResponse
            {
                IsSuccess = false,
                Message = message,
                LockInfo = lockInfo,
                Status = LockStatus.Unlocked,
                DenialReason = denialReason,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建解锁成功响应
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <param name="message">成功消息</param>
        /// <returns>锁定响应实例</returns>
        public static LockResponse UnlockSuccessResponse(LockInfo lockInfo, string message = "单据解锁成功")
        {
            return new LockResponse
            {
                IsSuccess = true,
                Message = message,
                LockInfo = lockInfo,
                Status = LockStatus.Unlocked,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建状态查询响应
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <param name="message">响应消息</param>
        /// <returns>锁定响应实例</returns>
        public static LockResponse StatusQueryResponse(LockInfo lockInfo, string message = "查询成功")
        {
            long remainingTime = 0;
            if (lockInfo.IsLocked && lockInfo.ExpireTime.HasValue)
            {
                remainingTime = (long)(lockInfo.ExpireTime.Value - DateTime.Now).TotalMilliseconds;
                remainingTime = Math.Max(0, remainingTime);
            }

            return new LockResponse
            {
                IsSuccess = true,
                Message = message,
                LockInfo = lockInfo,
                Status = lockInfo.IsLocked ? LockStatus.Locked : LockStatus.Unlocked,
                RemainingLockTimeMs = remainingTime,
                ResponseTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建批量查询响应
        /// </summary>
        /// <param name="lockInfoList">锁定信息列表</param>
        /// <param name="message">响应消息</param>
        /// <returns>锁定响应实例</returns>
        public static LockResponse BatchQueryResponse(List<LockInfo> lockInfoList, string message = "批量查询成功")
        {
            return new LockResponse
            {
                IsSuccess = true,
                Message = message,
                LockInfoList = lockInfoList,
                ResponseTime = DateTime.Now
            };
        }
    }
 
}
