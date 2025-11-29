using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Lock
{
    /// <summary>
    /// 锁定响应工厂类
    /// 统一所有锁定相关响应的创建逻辑
    /// 与LockResponse解耦，负责所有响应构建
    /// </summary>
    public static class LockResponseFactory
    {

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="lockInfo">锁定信息（可选）</param>
        /// <returns>LockResponse实例</returns>
        public static LockResponse CreateSuccessResponse(string message = "操作成功", LockInfo lockInfo = null)
        {
            return new LockResponse
            {
                IsSuccess = true,
                Message = message,
                LockInfo = lockInfo
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="lockInfo">锁定信息（可选）</param>
        /// <returns>LockResponse实例</returns>
        public static LockResponse CreateFailedResponse(string message = "操作失败", LockInfo lockInfo = null)
        {
            return new LockResponse
            {
                IsSuccess = false,
                Message = message,
                LockInfo = lockInfo,
            };
        }

 
  
 
    }
}
