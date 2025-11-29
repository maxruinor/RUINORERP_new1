using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RUINORERP.PacketSpec.Models.Lock
{
    /// <summary>
    /// 解锁类型枚举
    /// 定义不同的解锁方式
    /// </summary>
    public enum UnlockType
    {
        Normal,         // 正常解锁
        Force,          // 强制解锁
        Expired,        // 过期解锁
        SessionEnd,     // 会话结束解锁
        RequestResponse,// 请求响应解锁
        ByBizName       // 按业务名称解锁
    }

    /// <summary>
    /// 锁定请求类
    /// 封装锁定操作的请求参数
    /// 简化为请求控制和操作参数，引用LockInfo而非复制字段
    /// </summary>
    [DataContract]
    public class LockRequest : RequestBase
    {
        /// <summary>
        /// 锁定信息
        /// 包含所有核心锁定数据
        /// </summary>
        [DataMember]
        public LockInfo LockInfo { get; set; } = new LockInfo();

        /// <summary>
        /// 解锁类型
        /// 定义不同的解锁方式
        /// </summary>
        [DataMember]
        public UnlockType UnlockType { get; set; }

        /// <summary>
        /// 是否为强制解锁
        /// 便捷属性，判断是否为强制解锁
        /// </summary>
        [DataMember]
        public bool IsForceUnlock => UnlockType == UnlockType.Force;

        /// <summary>
        /// 刷新模式
        /// 是否在刷新时更新锁定时间
        /// </summary>
        [DataMember]
        public bool RefreshMode { get; set; }

        /// <summary>
        /// 锁定的文档列表
        /// 用于广播操作
        /// </summary>
        [DataMember]
        public List<LockInfo> LockedDocuments { get; set; }

        /// <summary>
        /// 锁定用户ID
        /// 便捷属性，从LockInfo中获取
        /// </summary>
        public long LockedUserId
        {
            get { return LockInfo?.LockedUserId ?? 0; }
            set { if (LockInfo != null) LockInfo.LockedUserId = value; }
        }

        /// <summary>
        /// 请求者用户ID
        /// 发送解锁请求的用户ID
        /// 当RequestType为RequestUnlock时有效
        /// </summary>
        [DataMember]
        public long RequesterUserId { get; set; }

        /// <summary>
        /// 请求者用户名
        /// 发送解锁请求的用户名
        /// 当RequestType为RequestUnlock时有效
        /// </summary>
        [DataMember]
        public string RequesterUserName { get; set; } = string.Empty;

        /// <summary>
        /// 锁定用户名
        /// 锁定单据的用户名
        /// 便捷属性，从LockInfo中获取或设置
        /// </summary>
        [DataMember]
        public string LockedUserName
        {
            get { return LockInfo?.LockedUserName ?? string.Empty; }
            set { if (LockInfo != null) LockInfo.LockedUserName = value; }
        }

        /// <summary>
        /// 初始化锁定请求
        /// </summary>
        public LockRequest() { }

        /// <summary>
        /// 使用锁定信息初始化请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        public LockRequest(LockInfo lockInfo)
        {
            LockInfo = lockInfo;
        }

        /// <summary>
        /// 创建锁定请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateLockRequest(LockInfo lockInfo)
        {
            return new LockRequest(lockInfo);
        }

        /// <summary>
        /// 创建解锁请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <param name="unlockType">解锁类型</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateUnlockRequest(LockInfo lockInfo, UnlockType unlockType = UnlockType.Normal)
        {
            return new LockRequest(lockInfo)
            {
                UnlockType = unlockType
            };
        }

        /// <summary>
        /// 创建强制解锁请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateForceUnlockRequest(LockInfo lockInfo)
        {
            return CreateUnlockRequest(lockInfo, UnlockType.Force);
        }

        /// <summary>
        /// 创建锁定刷新请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <param name="refreshMode">刷新模式</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateRefreshRequest(LockInfo lockInfo, bool refreshMode = true)
        {
            return new LockRequest(lockInfo)
            {
                RefreshMode = refreshMode
            };
        }

        /// <summary>
        /// 创建状态查询请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateQueryRequest(LockInfo lockInfo)
        {
            return new LockRequest(lockInfo);
        }

        /// <summary>
        /// 创建请求解锁请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateRequestUnlockRequest(LockInfo lockInfo)
        {
            return new LockRequest(lockInfo);
        }

        /// <summary>
        /// 创建拒绝解锁请求
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        /// <returns>锁定请求实例</returns>
        public static LockRequest CreateRefuseUnlockRequest(LockInfo lockInfo)
        {
            return new LockRequest(lockInfo);
        }
    }
}
