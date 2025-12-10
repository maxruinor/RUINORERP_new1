using System;
using RUINORERP.PacketSpec.Models.Lock;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 解锁请求信息类
    /// 包含解锁请求及其创建时间，用于实现自动过期机制
    /// </summary>
    public class UnlockRequestInfo
    {
        /// <summary>
        /// 解锁请求
        /// </summary>
        public LockRequest Request { get; set; }

        /// <summary>
        /// 请求创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 请求过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="request">解锁请求</param>
        /// <param name="timeout">超时时间</param>
        public UnlockRequestInfo(LockRequest request, TimeSpan timeout)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            CreatedTime = DateTime.Now;
            ExpireTime = CreatedTime.Add(timeout);
        }

        /// <summary>
        /// 检查请求是否已过期
        /// </summary>
        /// <returns>是否已过期</returns>
        public bool IsExpired => DateTime.Now > ExpireTime;
    }
}