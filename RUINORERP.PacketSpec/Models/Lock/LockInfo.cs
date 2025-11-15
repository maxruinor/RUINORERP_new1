using RUINORERP.Model.CommonModel;
using System;

namespace RUINORERP.PacketSpec.Models.Lock
{
    /// <summary>
    /// 锁定信息实体类
    /// 作为锁定功能的核心数据结构，包含所有锁定相关的状态和元数据
    /// </summary>
    public class LockInfo
    {
        /// <summary>
        /// 锁定键，用于唯一标识锁定（缓存键）
        /// </summary>
        public string LockKey { get; set; } = string.Empty;

        /// <summary>
        /// 锁定ID，唯一标识本次锁定（UUID格式）
        /// </summary>
        public string LockId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 单据ID
        /// 要锁定的单据唯一标识
        /// </summary>
        public long BillID { get; set; }

        /// <summary>
        /// 用户ID
        /// 执行锁定操作的用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// 执行锁定操作的用户名
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 锁定时间
        /// 锁定操作的执行时间
        /// </summary>
        public DateTime LockTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 过期时间
        /// 锁定的过期时间点
        /// </summary>
        public DateTime? ExpireTime { get; set; }

        /// <summary>
        /// 锁定备注/原因
        /// 说明锁定的目的或原因
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 菜单ID
        /// 相关的业务模块菜单ID
        /// </summary>
        public long MenuID { get; set; }

        /// <summary>
        /// 单据信息
        /// 关联的单据数据
        /// </summary>
        public CommBillData? BillData { get; set; }

     
        /// <summary>
        /// 会话ID
        /// 用户会话的唯一标识
        /// </summary>
        public string SessionId { get; set; } = string.Empty;

        /// <summary>
        /// 操作ID，标识锁定所属的操作
        /// </summary>
        public long OperationId { get; set; }

        /// <summary>
        /// 过期时间戳（Unix毫秒）
        /// 用于分布式系统中的时间一致性判断
        /// </summary>
        public long? ExpireTimestamp => ExpireTime.HasValue ? new DateTimeOffset(ExpireTime.Value).ToUnixTimeMilliseconds() : null;

        /// <summary>
        /// 锁定剩余时间（毫秒）
        /// 获取锁定剩余的有效期
        /// </summary>
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
        /// 是否已锁定
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// 锁定状态
        /// 基于IsLocked和时间状态计算得出
        /// </summary>
        public LockStatus Status
        {
            get
            {
                if (!IsLocked)
                    return LockStatus.Unlocked;

                if (IsExpired())
                    return LockStatus.Unlocked;

                if (IsAboutToExpire())
                    return LockStatus.AboutToExpire;

                return LockStatus.Locked;
            }
            set
            {
                // 只允许设置特定的业务状态
                if (value == LockStatus.RequestingUnlock)
                {
                    IsLocked = true; // 请求解锁时保持锁定状态
                }
                else if (value == LockStatus.Unlocked)
                {
                    IsLocked = false;
                }
                // Locked和AboutToExpire状态由IsLocked和时间状态自动计算
            }
        }

        /// <summary>
        /// 是否即将过期（剩余时间小于总时间的20%）
        /// </summary>
        /// <returns>如果即将过期则返回true</returns>
        public bool IsAboutToExpire()
        {
            if (!ExpireTime.HasValue || !IsLocked)
                return false;

            // 计算锁定总时长
            var totalDuration = (ExpireTime.Value - LockTime).TotalMilliseconds;
            if (totalDuration <= 0)
                return false;

            // 计算剩余时间占比
            var remainingRatio = RemainingLockTimeMs.Value / totalDuration;
            return remainingRatio < 0.2; // 剩余时间小于20%则认为即将过期
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
            return this;
        }

        /// <summary>
        /// 检查锁定是否已过期
        /// </summary>
        /// <returns>如果已过期则返回true</returns>
        public bool IsExpired()
        {
            return !ExpireTime.HasValue || DateTime.Now > ExpireTime.Value;
        }

        /// <summary>
        /// 刷新锁定有效期
        /// </summary>
        /// <param name="milliseconds">新的有效期（毫秒）</param>
        /// <returns>锁定实体（支持链式调用）</returns>
        public LockInfo RefreshExpireTime(int milliseconds)
        {
            ExpireTime = DateTime.Now.AddMilliseconds(milliseconds);
            return this;
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
            return new LockInfo
            {
                BillID = billId,
                UserId = userId,
                UserName = userName,
                MenuID = menuId,
                SessionId = sessionId,
                ExpireTime = expireTime ?? DateTime.Now.AddMinutes(5),
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
                LockId = this.LockId,
                BillID = this.BillID,
                UserId = this.UserId,
                UserName = this.UserName,
                LockTime = this.LockTime,
                ExpireTime = this.ExpireTime,
                Remark = this.Remark,
                MenuID = this.MenuID,
                BillData = this.BillData,
                SessionId = this.SessionId,
                OperationId = this.OperationId,
                IsLocked = this.IsLocked
            };
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>锁定信息的字符串表示</returns>
        public override string ToString()
        {
            return $"LockInfo[BillID={BillID}, UserName={UserName}, IsLocked={IsLocked}, " +
                   $"LockTime={LockTime}, ExpireTime={ExpireTime}]";
        }
    }
}
