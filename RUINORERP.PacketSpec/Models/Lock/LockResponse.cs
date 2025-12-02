using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Lock
{
    
    /// <summary>
    /// 锁定响应类
    /// 封装锁定操作的响应结果
    /// </summary>
    public class LockResponse : ResponseBase
    {
        /// <summary>
        /// 锁定信息
        /// 包含锁定的详细数据
        /// </summary>
        public LockInfo LockInfo { get; set; } = new LockInfo();
        
        /// <summary>
        /// 锁定信息列表
        /// 用于批量操作返回
        /// </summary>
        public List<LockInfo> LockInfoList { get; set; } = new List<LockInfo>();
        
        
        /// <summary>
        /// 响应时间
        /// </summary>
        public DateTime ResponseTime { get; set; } = DateTime.Now;
        
        
        /// <summary>
        /// 初始化锁定响应
        /// </summary>
        public LockResponse() { }
        
        /// <summary>
        /// 使用锁定信息初始化响应
        /// </summary>
        /// <param name="lockInfo">锁定信息</param>
        public LockResponse(LockInfo lockInfo)
        {
            LockInfo = lockInfo;
        }
       
    }
}
