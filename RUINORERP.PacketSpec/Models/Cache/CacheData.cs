using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Cache
{
    /// <summary>
    /// 统一缓存数据模型 - 支持多种缓存操作的数据存储
    /// 与统一缓存请求响应模型配套使用
    /// </summary>
    public class CacheData
    {
        /// <summary>
        /// 默认缓存过期时间（4小时，基础数据缓存使用较长的过期时间）
        /// 与 EntityCacheManager.GetCacheExpirationTime() 保持一致
        /// </summary>
        public static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(4);

        /// <summary>
        /// 泛型反序列化方法委托缓存 - 避免重复反射调用提升性能
        /// Key: EntityTypeName, Value: Func<byte[], bool, object>
        /// </summary>
        private static readonly ConcurrentDictionary<string, Func<byte[], bool, object>> _deserializeDelegateCache = 
            new ConcurrentDictionary<string, Func<byte[], bool, object>>();

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; } = string.Empty;


        /// <summary>
        /// 获取实体类型
        /// </summary>
        [JsonIgnore]
        public Type EntityType => TypeResolver.GetType(EntityTypeName);

        public string EntityTypeName { get; set; }


        /// <summary>
        /// 二进制数据
        /// </summary>
        public byte[] EntityByte { get; set; }


        /// <summary>
        /// 缓存时间
        /// </summary>
        public DateTime CacheTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime { get; set; } = DateTime.Now.Add(DefaultExpiration);

 

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// 数据版本戳（用于冲突检测）
        /// 对应 CacheSyncInfo.VersionStamp
        /// </summary>
        [JsonProperty]
        public long VersionStamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        /// <summary>
        /// 是否有更多数据
        /// </summary>
        public bool HasMoreData { get; set; } = false;

        /// <summary>
        /// 创建缓存数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="tableName">表名</param>
        /// <param name="data">实体数据</param>
        /// <param name="expiration">过期时间（可选）</param>
        /// <param name="versionStamp">版本戳（可选，默认自动生成）</param>
        public static CacheData Create<T>(string tableName, T data, TimeSpan? expiration = null, long versionStamp = 0) where T : class
        {
            return new CacheData
            {
                TableName = tableName,
                EntityByte = JsonCompressionSerializationService.Serialize(data, true),
                EntityTypeName = typeof(T).AssemblyQualifiedName, // 使用程序集限定名称
                CacheTime = DateTime.Now,
                ExpirationTime = DateTime.Now.Add(expiration ?? DefaultExpiration),
                VersionStamp = versionStamp > 0 ? versionStamp : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
        }

        /// <summary>
        /// 创建缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="data">实体数据</param>
        /// <param name="versionStamp">版本戳（可选，默认自动生成）</param>
        public static CacheData Create(string tableName, object data, TimeSpan? expiration = null, long versionStamp = 0)
        {
            return new CacheData
            {
                TableName = tableName,
                EntityByte = JsonCompressionSerializationService.Serialize(data, true),
                EntityTypeName = data.GetType().AssemblyQualifiedName,
                CacheTime = DateTime.Now,
                ExpirationTime = DateTime.Now.Add(expiration ?? DefaultExpiration),
                Version = "1.0.0",
                VersionStamp = versionStamp > 0 ? versionStamp : DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
        }

        /// <summary>
        /// 检查缓存是否过期
        /// </summary>
        public bool IsExpired()
        {
            return DateTime.Now > ExpirationTime;
        }

        /// <summary>
        /// 获取数据（类型安全）
        /// </summary>
        public T GetData<T>()
        {
            if (EntityByte == null)
                return default;

            try
            {
                return CacheDataConverter.ConvertToType<T>(EntityByte);
            }
            catch (OutOfMemoryException)
            {
                throw; // 重新抛出严重异常
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"反序列化失败: {typeof(T).Name}, 错误: {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// 动态获取数据（高性能版本 - 使用委托缓存避免反射）
        /// </summary>
        public object GetData()
        {
            if (EntityByte == null || string.IsNullOrEmpty(EntityTypeName))
                return null;

            try
            {
                // 使用委托缓存获取或创建反序列化方法
                var deserializeFunc = _deserializeDelegateCache.GetOrAdd(EntityTypeName, typeName =>
                {
                    var type = TypeResolver.GetType(typeName);
                    if (type == null) return null;

                    // 获取泛型方法 Deserialize<T>
                    var method = typeof(JsonCompressionSerializationService).GetMethod("Deserialize");
                    if (method == null) return null;

                    // 构造泛型方法
                    var genericMethod = method.MakeGenericMethod(type);

                    // 创建委托：Func<byte[], bool, object>
                    return (Func<byte[], bool, object>)Delegate.CreateDelegate(
                        typeof(Func<byte[], bool, object>), 
                        genericMethod);
                });

                if (deserializeFunc == null)
                {
                    System.Diagnostics.Debug.WriteLine($"无法为类型 {EntityTypeName} 创建反序列化委托");
                    return null;
                }

                // 直接调用委托，避免每次反射
                return deserializeFunc(EntityByte, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"动态反序列化失败: {EntityTypeName}, 错误: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// 安全获取数据，带类型验证
        /// </summary>
        public T GetDataSafe<T>() where T : class
        {
            var entityType = EntityType;
            if (entityType == null || !typeof(T).IsAssignableFrom(entityType))
            {
                System.Diagnostics.Debug.WriteLine($"类型不匹配: 期望 {typeof(T).Name}, 实际 {entityType?.Name}");
                return default(T);
            }

            return JsonCompressionSerializationService.Deserialize<T>(EntityByte, true);
        }


    }

    /// <summary>
    /// 统一分页缓存数据模型
    /// </summary>
    public class PagedCacheData
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        public CacheData CacheData { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; } = 0;

        /// <summary>
        /// 创建分页缓存数据
        /// </summary>
        public static PagedCacheData Create(CacheData cacheData, int pageIndex, int pageSize, int totalCount)
        {
            return new PagedCacheData
            {
                CacheData = cacheData,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }

        /// <summary>
        /// 是否第一页
        /// </summary>
        [JsonIgnore]
        public bool IsFirstPage => PageIndex <= 0;

        /// <summary>
        /// 是否最后一页
        /// </summary>
        [JsonIgnore]
        public bool IsLastPage => PageIndex >= TotalPages - 1;

        /// <summary>
        /// 是否有上一页
        /// </summary>
        [JsonIgnore]
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        [JsonIgnore]
        public bool HasNextPage => PageIndex < TotalPages - 1;
    }
}
