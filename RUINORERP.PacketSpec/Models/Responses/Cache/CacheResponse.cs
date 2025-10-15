using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Responses;
using MessagePack;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Commands.Cache;

namespace RUINORERP.PacketSpec.Models.Responses.Cache
{
    /// <summary>
    /// 统一缓存响应模型 - 用于所有缓存操作的响应
    /// 与统一缓存请求模型配套使用
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class CacheResponse : ResponseBase
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        [Key(10)]
        public CacheData CacheData { get; set; }

        /// <summary>
        /// 缓存表名
        /// </summary>
        [Key(11)]
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 缓存操作类型（对应请求的操作类型）
        /// </summary>
        [Key(12)]
        public CacheOperation Operation { get; set; } = CacheOperation.Get;

        /// <summary>
        /// 缓存生成时间
        /// </summary>
        [Key(13)]
        public DateTime CacheTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 缓存过期时间
        /// </summary>
        [Key(14)]
        public DateTime ExpirationTime { get; set; } = DateTime.Now.AddDays(1);

        /// <summary>
        /// 是否需要继续请求更多缓存数据
        /// </summary>
        [Key(15)]
        public bool HasMoreData { get; set; } = false;

        /// <summary>
        /// 服务器版本号
        /// </summary>
        [Key(16)]
        public string ServerVersion { get; set; } = string.Empty;

        /// <summary>
        /// 操作结果信息（用于返回操作详情）
        /// </summary>
        [Key(17)]
        public Dictionary<string, object> OperationResult { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 创建成功的缓存响应
        /// </summary>
        public static CacheResponse CreateSuccess(CacheData cacheData, CacheOperation operation, string tableName, string message = "缓存操作成功")
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
        /// 创建失败的缓存响应
        /// </summary>
#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
        public static CacheResponse CreateError(string message, int code = 500, CacheOperation operation = CacheOperation.Get)
#pragma warning restore CS0108 // 成员隐藏继承的成员；缺少关键字 new
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

        /// <summary>
        /// 添加操作结果信息
        /// </summary>
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
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
    }
}
