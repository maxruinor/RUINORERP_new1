using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Responses.Cache;

namespace RUINORERP.PacketSpec.Models.Requests.Cache
{
    /// <summary>
    /// 统一缓存请求模型 - 用于所有缓存相关操作
    /// 通过 OperationType 区分不同的缓存操作
    /// </summary>
    [Serializable]
    
    public class CacheRequest : RequestBase
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;

        /// <summary>
        /// 缓存操作类型
        /// </summary>
        public CacheOperation Operation { get; set; } = CacheOperation.Get;

        /// <summary>
        /// 订阅操作类型 - 用于CacheSubscription命令
        /// </summary>
        public SubscribeAction SubscribeAction { get; set; } = SubscribeAction.None;


        /// <summary>
        /// 缓存数据
        /// </summary>
        public CacheData CacheData { get; set; }

        /// <summary>
        /// 主键名称
        /// </summary>
        public string PrimaryKeyName { get; set; } = string.Empty;

        /// <summary>
        /// 主键值
        /// </summary>
        public object PrimaryKeyValue { get; set; }

        /// <summary>
        /// 是否强制刷新缓存
        /// </summary>
        public bool ForceRefresh { get; set; } = false;

        /// <summary>
        /// 上次请求时间，用于增量更新
        /// </summary>
        public DateTime LastRequestTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// 创建缓存获取请求
        /// </summary>
        public static CacheRequest CreateGetRequest(string tableName, bool forceRefresh = false)
        {
            return new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Get,
                ForceRefresh = forceRefresh,
            };
        }


        /// <summary>
        /// 创建缓存删除请求
        /// </summary>
        public static CacheRequest CreateDeleteRequest(string tableName, string primaryKeyName = null, object primaryKeyValue = null)
        {
            var request = new CacheRequest
            {
                TableName = tableName,
                Operation = CacheOperation.Remove,
                PrimaryKeyName = primaryKeyName ?? string.Empty,
                PrimaryKeyValue = primaryKeyValue
            };

            return request;
        }

        /// <summary>
        /// 创建缓存清空请求
        /// </summary>
        public static CacheRequest CreateClearRequest(string tableName = null)
        {
            return new CacheRequest
            {
                TableName = tableName ?? string.Empty,
                Operation = CacheOperation.Clear,
            };
        }



        /// <summary>
        /// 创建缓存获取请求（兼容旧版本）
        /// </summary>
        public static CacheRequest Create(string tableName, bool forceRefresh = false)
        {
            return CreateGetRequest(tableName, forceRefresh);
        }

        /// <summary>
        /// 创建订阅请求
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="subscribeAction">订阅操作类型</param>
        /// <returns>缓存请求对象</returns>
        public static CacheRequest CreateSubscriptionRequest(string tableName, SubscribeAction subscribeAction)
        {
            return new CacheRequest
            {
                TableName = tableName,
                SubscribeAction = subscribeAction
            };
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        public T GetParameter<T>(string key, T defaultValue = default)
        {
            if (Parameters.TryGetValue(key, out var value))
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

        /// <summary>
        /// 设置参数值
        /// </summary>
        public void SetParameter(string key, object value)
        {
            Parameters[key] = value;
        }
    }

    /// <summary>
    /// 订阅操作类型枚举
    /// </summary>
    public enum SubscribeAction
    {
        /// <summary>
        /// 无操作
        /// </summary>
        None = 0,
        /// <summary>
        /// 订阅
        /// </summary>
        Subscribe = 1,
        /// <summary>
        /// 取消订阅
        /// </summary>
        Unsubscribe = 2
    }
}
