using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Responses.Cache
{
    /// <summary>
    /// 缓存响应模型 - 服务器返回缓存数据给客户端
    /// </summary>
    [Serializable]
    public class CacheResponse : ResponseBase
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        public Dictionary<string, object> CacheData { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 缓存表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 缓存生成时间
        /// </summary>
        public DateTime CacheTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; } = DateTime.Now.AddDays(1);

        /// <summary>
        /// 是否需要继续请求更多缓存数据
        /// </summary>
        public bool HasMoreData { get; set; } = false;

        /// <summary>
        /// 服务器版本号
        /// </summary>
        public string ServerVersion { get; set; } = string.Empty;

        /// <summary>
        /// 创建成功的缓存响应
        /// </summary>
        public static CacheResponse CreateSuccess(string tableName, Dictionary<string, object> cacheData)
        {
            return new CacheResponse
            {
                IsSuccess = true,
                Message = "缓存数据获取成功",
                Code = 200,
                TableName = tableName,
                CacheData = cacheData,
                CacheTime = DateTime.Now,
                ExpirationTime = DateTime.Now.AddDays(1)
            };
        }

        /// <summary>
        /// 创建失败的缓存响应
        /// </summary>
        public static CacheResponse CreateError(string message, int code = 500)
        {
            return new CacheResponse
            {
                IsSuccess = false,
                Message = message,
                Code = code,
                CacheTime = DateTime.Now
            };
        }

        /// <summary>
        /// 创建部分缓存响应（表示还有更多数据）
        /// </summary>
        public static CacheResponse CreatePartial(string tableName, Dictionary<string, object> cacheData)
        {
            return new CacheResponse
            {
                IsSuccess = true,
                Message = "获取部分缓存数据",
                Code = 206,
                TableName = tableName,
                CacheData = cacheData,
                HasMoreData = true,
                CacheTime = DateTime.Now,
                ExpirationTime = DateTime.Now.AddDays(1)
            };
        }
    }
}
