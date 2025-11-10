using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Responses;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Commands.Cache;

namespace RUINORERP.PacketSpec.Models.Responses.Cache
{
    /// <summary>
    /// 缁熶竴缂撳瓨鍝嶅簲妯″瀷 - 鐢ㄤ簬鎵€鏈夌紦瀛樻搷浣滅殑鍝嶅簲
    /// 涓庣粺涓€缂撳瓨璇锋眰妯″瀷閰嶅浣跨敤
    /// </summary>
    [Serializable]
    public class CacheResponse : ResponseBase
    {
        /// <summary>
        /// </summary>
        public CacheData CacheData { get; set; }

        /// <summary>
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        public CacheOperation Operation { get; set; } = CacheOperation.Get;

        /// <summary>
        /// </summary>
        public DateTime CacheTime { get; set; } = DateTime.Now;

        /// <summary>
        /// </summary>
        public DateTime ExpirationTime { get; set; } = DateTime.Now.AddDays(1);

        /// <summary>
        public bool HasMoreData { get; set; } = false;

        /// <summary>
        /// </summary>
        public string ServerVersion { get; set; } = string.Empty;

        /// <summary>
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
        /// 鑾峰彇鎿嶄綔缁撴灉淇℃伅
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



