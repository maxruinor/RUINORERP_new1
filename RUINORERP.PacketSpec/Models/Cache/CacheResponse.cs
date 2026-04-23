using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Responses;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Cache
{
    /// <summary>
    /// 统一缓存响应模型
    /// 
    /// 【核心原则】：谁先发送谁就是请求方，Response 用于对 Request 的回复
    /// 
    /// 【使用场景】
    /// 1. 服务器 -> 客户端（查询响应）：
    ///    - 触发：客户端发送 CacheOperation.Get 请求后，服务器用 CacheResponse 回复
    ///    - 内容：包含完整的 CacheData (EntityByte)
    /// 
    /// 2. 客户端 -> 服务器（操作确认）：
    ///    - 触发：客户端发送 CacheSync 上报变更后，服务器用 CacheResponse 确认
    ///    - 内容：包含操作结果状态
    /// 
    /// 3. 注意：服务器主动推送变更时，应使用 CacheRequest 而非 CacheResponse
    ///    - 原因：服务器是发起方，符合“谁先发送谁请求”的原则
    /// </summary>
    [Serializable]
    public class CacheResponse : ResponseBase
    {
        /// <summary>
        /// 缓存数据载荷
        /// 【注意】在查询响应中包含全量数据；在同步推送中仅包含变更部分
        /// </summary>
        public CacheData CacheData { get; set; }

        /// <summary>
        /// 关联的表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 操作类型（用于标识是 Set, Remove 还是 Get 的结果）
        /// </summary>
        public CacheOperation Operation { get; set; } = CacheOperation.Get;

        /// <summary>
        /// 缓存时间戳
        /// </summary>
        public DateTime CacheTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; } = DateTime.Now.Add(CacheData.DefaultExpiration);

        /// <summary>
        /// 是否还有更多数据（分页查询时使用）
        /// </summary>
        public bool HasMoreData { get; set; } = false;

        /// <summary>
        /// 服务器版本号
        /// </summary>
        public string ServerVersion { get; set; } = string.Empty;

        /// <summary>
        /// 扩展操作结果集
        /// </summary>
        public Dictionary<string, object> OperationResult { get; set; } = new Dictionary<string, object>();

        public static CacheResponse CreateSuccess(CacheData cacheData, CacheOperation operation, string tableName, string message = "成功")
        {
            return new CacheResponse
            {
                IsSuccess = true,
                Message = message,
                CacheData = cacheData,
                Operation = operation,
                TableName = tableName,
                CacheTime = DateTime.Now,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        public static CacheResponse CreateError(string message, int code = 500, CacheOperation operation = CacheOperation.Get)
        {
            return new CacheResponse
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = code,
                Operation = operation,
                CacheTime = DateTime.Now,
                Timestamp = DateTime.Now
            };
        }
 
        public void AddOperationResult(string key, object value)
        {
            OperationResult[key] = value;
        }

        /// <summary>
        /// 获取操作结果信息
        /// </summary>
        public T GetOperationResult<T>(string key, T defaultValue = default)
        {
            if (OperationResult.TryGetValue(key, out var value))
            {
                try
                {
                    return CacheDataConverter.ConvertToType<T>(value);
                }
                catch (OutOfMemoryException)
                {
                    throw; // 重新抛出严重异常
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"转换操作结果失败: Key={key}, 错误: {ex.Message}");
                    return defaultValue;
                }
            }
            return defaultValue;
        }
    }
}



