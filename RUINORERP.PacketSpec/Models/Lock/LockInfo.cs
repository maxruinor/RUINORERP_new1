using RUINORERP.Global;
using RUINORERP.Model.CommonModel;
using System;
using System.Runtime.Serialization;

namespace RUINORERP.PacketSpec.Models.Lock
{
    /// <summary>
    /// 统一锁定信息类
    /// 整合了ServerLockInfo、ClientLockInfo、HeldLockInfo和原有LockInfo的所有功能
    /// </summary>
    [DataContract]
    public class LockInfo
    {
        /// <summary>
        /// 锁定键，用于唯一标识锁定（缓存键）
        /// </summary>
        [DataMember]
        public string LockKey { get; set; } = string.Empty;

        /// <summary>
        /// 单据ID
        /// 要锁定的单据唯一标识
        /// </summary>
        [DataMember]
        public long BillID { get; set; }

        /// <summary>
        /// 用户ID
        /// 执行锁定操作的用户ID
        /// </summary>
        [DataMember]
        public long LockedUserId { get; set; }

        /// <summary>
        /// 用户名称
        /// 执行锁定操作的用户名
        /// </summary>
        [DataMember]
        public string LockedUserName { get; set; } = string.Empty;

        /// <summary>
        /// 锁定时间
        /// 锁定操作的执行时间
        /// </summary>
        [DataMember]
        public DateTime LockTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 过期时间
        /// 锁定的过期时间点
        /// </summary>
        [DataMember]
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 锁定备注/原因
        /// 说明锁定的目的或原因
        /// </summary>
        [DataMember]
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 菜单ID
        /// 相关的业务模块菜单ID
        /// </summary>
        [DataMember]
        public long MenuID { get; set; }
        
        /// <summary>
        /// 业务名称
        /// 锁定的业务模块名称
        /// </summary>
        [DataMember]
        public string BizName { get; set; } = string.Empty;
        
        /// <summary>
        /// 菜单名称
        /// 锁定的菜单名称
        /// </summary>
        [DataMember]
        public string MenuName { get; set; } = string.Empty;
        
        /// <summary>
        /// 单据信息
        /// 关联的单据数据
        /// </summary>
        [DataMember]
        public BizType bizType { get; set; } = new BizType();

        /// <summary>
        /// 会话ID
        /// 用户会话的唯一标识
        /// </summary>
        [DataMember]
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// 是否已锁定
        /// </summary>
        [DataMember]
        public bool IsLocked { get; set; }

        /// <summary>
        /// 最后心跳时间
        /// </summary>
        [DataMember]
        public DateTime LastHeartbeat { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后更新时间
        /// 锁信息最后更新的时间，用于跟踪锁的状态变化
        /// </summary>
        [DataMember]
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 心跳次数
        /// </summary>
        [DataMember]
        public int HeartbeatCount { get; set; }

        /// <summary>
        /// 锁定类型
        /// </summary>
        [DataMember]
        public LockType Type { get; set; } = LockType.Exclusive;

        /// <summary>
        /// 锁定持续时间（毫秒）
        /// </summary>
        [DataMember]
        public long Duration { get; set; }

        /// <summary>
        /// 过期时间戳（Unix毫秒）
        /// 用于分布式系统中的时间一致性判断
        /// </summary>
        [DataMember]
        public long? ExpireTimestamp => ExpireTime.HasValue ? new DateTimeOffset(ExpireTime.Value).ToUnixTimeMilliseconds() : null;

        /// <summary>
        /// 锁定剩余时间（毫秒）
        /// 获取锁定剩余的有效期
        /// </summary>
        [DataMember]
        public long? RemainingLockTimeMs
        {
            get
            {
                if (!ExpireTime.HasValue)
                    return null;

                long remaining = (long)(ExpireTime.Value - DateTime.Now).TotalMilliseconds;
                return Math.Max(0, remaining);
            }
        }

        /// <summary>
        /// 检查锁定是否已过期
        /// </summary>
        public bool IsExpired
        {
            get { return !ExpireTime.HasValue || DateTime.Now > ExpireTime.Value; }
        }

     
        /// <summary>
        /// 检查锁是否为孤儿锁（2分钟无心跳）
        /// </summary>
        [DataMember]
        public bool IsOrphaned => DateTime.Now > LastHeartbeat.AddMinutes(2);

        /// <summary>
        /// 是否即将过期（剩余时间小于总时间的20%）
        /// </summary>
        public bool IsAboutToExpire
        {
            get
            {
                if (!ExpireTime.HasValue || !IsLocked)
                    return false;

                // 计算锁定总时长
                var totalDuration = (ExpireTime.Value - LockTime).TotalMilliseconds;
                if (totalDuration <= 0)
                    return false;

                // 计算剩余时间占比
                return RemainingLockTimeMs.Value / totalDuration < 0.2; // 剩余时间小于20%则认为即将过期
            }
        }

        /// <summary>
        /// 设置锁定键
        /// </summary>
        /// <param name="prefix">键前缀</param>
        /// <returns>锁定实体（支持链式调用）</returns>
        public LockInfo SetLockKey(string prefix = "lock:document")
        {
            LockKey = $"{prefix}:{BillID}";
            return this;
        }

        /// <summary>
        /// 设置锁定过期时间
        /// </summary>
        /// <param name="expireTime">过期时间</param>
        /// <returns>锁定实体（支持链式调用）</returns>
        public LockInfo SetExpireTime(DateTime expireTime)
        {
            ExpireTime = expireTime;
            return this;
        }

        /// <summary>
        /// 设置锁定过期时间（相对时间）
        /// </summary>
        /// <param name="milliseconds">毫秒数</param>
        /// <returns>锁定实体（支持链式调用）</returns>
        public LockInfo SetExpireTimeFromNow(int milliseconds)
        {
            ExpireTime = DateTime.Now.AddMilliseconds(milliseconds);
            Duration = milliseconds;
            return this;
        }

        /// <summary>
        /// 刷新锁定有效期
        /// </summary>
        /// <param name="milliseconds">新的有效期（毫秒）</param>
        /// <returns>锁定实体（支持链式调用）</returns>
        public LockInfo RefreshExpireTime(int milliseconds)
        {
            ExpireTime = DateTime.Now.AddMilliseconds(milliseconds);
            LastUpdateTime = DateTime.Now; // 刷新最后更新时间
            Duration = milliseconds;
            return this;
        }

        /// <summary>
        /// 更新心跳
        /// </summary>
        public void UpdateHeartbeat()
        {
            LastHeartbeat = DateTime.Now;
            LastUpdateTime = DateTime.Now; // 心跳更新时同时更新最后更新时间
            HeartbeatCount++;
        }

        /// <summary>
        /// 检查锁是否属于当前用户
        /// </summary>
        /// <param name="currentUserId">当前用户ID</param>
        /// <param name="currentSessionId">当前会话ID（可选）</param>
        /// <returns>如果锁属于当前用户则返回true</returns>
        public bool IsOwnedByCurrentUser(long currentUserId, string currentSessionId = null)
        {
            // 首先检查用户ID
            if (LockedUserId != currentUserId)
                return false;

            // 如果提供了会话ID，同时检查会话匹配
            if (!string.IsNullOrEmpty(currentSessionId) && !string.IsNullOrEmpty(SessionId))
                return SessionId == currentSessionId;

            // 如果没有提供会话ID，仅根据用户ID判断
            return true;
        }

        /// <summary>
        /// 创建新的锁定信息实例
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="menuId">菜单ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns>锁定信息实例</returns>
        public static LockInfo Create(long billId, long userId, string userName, long menuId,
            string sessionId, DateTime? expireTime = null)
        {
            var now = DateTime.Now;
            var duration = expireTime.HasValue ? (long)(expireTime.Value - now).TotalMilliseconds : 300000; // 默认5分钟
            return new LockInfo
            {
                BillID = billId,
                LockedUserId = userId,
                LockedUserName = userName,
                MenuID = menuId,
                SessionId = sessionId,
                ExpireTime = expireTime ?? now.AddMinutes(5),
                LockTime = now,
                LastHeartbeat = now,
                LastUpdateTime = now,
                Duration = duration,
                Type = LockType.Exclusive,
                IsLocked = true
            };
        }

        /// <summary>
        /// 创建用于查询的锁定信息实例
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁定信息实例</returns>
        public static LockInfo CreateForQuery(long billId)
        {
            return new LockInfo
            {
                BillID = billId,
            };
        }

        /// <summary>
        /// 克隆锁定信息
        /// </summary>
        /// <returns>锁定信息的副本</returns>
        public LockInfo Clone()
        {
            return new LockInfo
            {
                LockKey = this.LockKey,
                BillID = this.BillID,
                LockedUserId = this.LockedUserId,
                LockedUserName = this.LockedUserName,
                LockTime = this.LockTime,
                ExpireTime = this.ExpireTime,
                Remark = this.Remark,
                MenuID = this.MenuID,
                bizType = this.bizType,
                SessionId = this.SessionId,
                IsLocked = this.IsLocked,
                LastHeartbeat = this.LastHeartbeat,
                LastUpdateTime = this.LastUpdateTime,
                HeartbeatCount = this.HeartbeatCount,
                Type = this.Type,
                Duration = this.Duration,
            };
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>锁定信息的字符串表示</returns>
        public override string ToString()
        {
            return $"LockInfo[BillID={BillID}, UserName={LockedUserName}, IsLocked={IsLocked}, " +
                   $"LockTime={LockTime}, ExpireTime={ExpireTime}, Type={Type}, HeartbeatCount={HeartbeatCount}]";
        }
    }

    /// <summary>
    /// 锁定类型枚举
    /// </summary>
    [DataContract]
    public enum LockType
    {
        /// <summary>
        /// 排他锁
        /// </summary>
        [EnumMember]
        Exclusive = 0,
        /// <summary>
        /// 共享锁
        /// </summary>
        [EnumMember]
        Shared = 1,
        /// <summary>
        /// 意向排他锁
        /// </summary>
        [EnumMember]
        IntentExclusive = 2,
        /// <summary>
        /// 意向共享锁
        /// </summary>
        [EnumMember]
        IntentShared = 3
    }
}

