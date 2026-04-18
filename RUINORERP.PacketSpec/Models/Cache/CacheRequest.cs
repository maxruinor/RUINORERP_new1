using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Cache
{
    /// <summary>
    /// 统一缓存请求模型
    /// 
    /// 【核心原则】：谁先发送谁就是请求方，没有固定的客户端=请求、服务器=响应的绑定关系
    /// 
    /// 【使用场景】
    /// 1. 客户端 -> 服务器（查询）：CommandId = CacheOperation (0x0201)
    ///    - 用途：客户端向服务器请求指定表的数据
    ///    - 关键字段：TableName, ForceRefresh, LastRequestTime
    /// 
    /// 2. 客户端 -> 服务器（上报变更）：CommandId = CacheSync (0x0202)
    ///    - 用途：客户端向服务器报告本地数据的增删改
    ///    - 关键字段：TableName, Operation (Set/Remove), CacheData
    /// 
    /// 3. 服务器 -> 客户端（推送变更）：CommandId = CacheSync (0x0202)
    ///    - 用途：服务器向订阅的客户端推送数据变更
    ///    - 关键字段：TableName, Operation, CacheData
    ///    - 注意：虽然方向是 Server->Client，但仍使用 CacheRequest 作为载体（因为服务器是发起方）
    /// 
    /// 4. 订阅管理：CommandId = CacheSubscription (0x0203)
    ///    - 用途：管理客户端对特定表的订阅状态
    ///    - 关键字段：TableName, SubscribeAction
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
        /// 【注意】仅在 CacheSync 命令或 CacheOperation.Get 响应中使用
        /// </summary>
        public CacheOperation Operation { get; set; } = CacheOperation.Get;

        /// <summary>
        /// 订阅操作类型 - 用于CacheSubscription命令
        /// </summary>
        public SubscribeAction SubscribeAction { get; set; } = SubscribeAction.None;


        /// <summary>
        /// 缓存数据载荷
        /// 【注意】仅在 CacheSync.Set 操作或查询响应中携带实际数据
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
