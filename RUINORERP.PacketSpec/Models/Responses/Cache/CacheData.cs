using MessagePack;
using Newtonsoft.Json.Linq;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Models.Responses.Cache
{
    /// <summary>
    /// 统一缓存数据模型 - 支持多种缓存操作的数据存储
    /// 与统一缓存请求响应模型配套使用
    /// </summary>
    [MessagePackObject]
    public class CacheData
    {
        /// <summary>
        /// 表名
        /// </summary>
        [Key(0)]
        public string TableName { get; set; } = string.Empty;


        /// <summary>
        /// 获取实体类型
        /// </summary>
        [IgnoreMember]
        public Type EntityType => TypeResolver.GetType(EntityTypeName);

        [Key(18)]
        public string EntityTypeName { get; set; }


        /// <summary>
        /// 二进制数据
        /// </summary>
        [Key(19)]
        public byte[] EntityByte { get; set; }


        /// <summary>
        /// 缓存时间
        /// </summary>
        [Key(1)]
        public DateTime CacheTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 过期时间
        /// </summary>
        [Key(2)]
        public DateTime ExpirationTime { get; set; } = DateTime.Now.AddDays(1);

 

        /// <summary>
        /// 版本
        /// </summary>
        [Key(4)]
        public string Version { get; set; } = "1.0.0";

        /// <summary>
        /// 是否有更多数据
        /// </summary>
        [Key(5)]
        public bool HasMoreData { get; set; } = false;

        /// <summary>
        /// 创建缓存数据
        /// </summary>
        public static CacheData Create<T>(string tableName, T data, TimeSpan? expiration = null) where T : class
        {
            return new CacheData
            {
                TableName = tableName,
                EntityByte = UnifiedSerializationService.SerializeWithMessagePack(data),
                EntityTypeName = typeof(T).AssemblyQualifiedName, // 使用程序集限定名称
                CacheTime = DateTime.Now,
                ExpirationTime = DateTime.Now.Add(expiration ?? TimeSpan.FromDays(1))
            };
        }

        /// <summary>
        /// 创建缓存数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="data">实体数据</param>
        public static CacheData Create(string tableName, object data, TimeSpan? expiration = null)
        {
            return new CacheData
            {
                TableName = tableName,
                EntityByte = UnifiedSerializationService.SafeSerializeWithMessagePack(data),
                EntityTypeName = data.GetType().AssemblyQualifiedName,
                CacheTime = DateTime.Now,
                ExpirationTime = DateTime.Now.Add(expiration ?? TimeSpan.FromDays(1)),
                Version = "1.0.0"
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
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// 动态获取数据（高性能版本）
        /// </summary>
        public object GetData()
        {
            if (EntityByte == null || string.IsNullOrEmpty(EntityTypeName))
                return null;

            var type = TypeResolver.GetType(EntityTypeName);
            if (type == null) return null;

            try
            {
                // 使用反射调用泛型反序列化方法
                var method = typeof(UnifiedSerializationService).GetMethod("DeserializeWithMessagePack");
                var genericMethod = method.MakeGenericMethod(type);
                return genericMethod.Invoke(null, new object[] { EntityByte });
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

            return UnifiedSerializationService.DeserializeWithMessagePack<T>(EntityByte);
        }


    }

    /// <summary>
    /// 统一分页缓存数据模型
    /// </summary>
    [MessagePackObject]
    public class PagedCacheData
    {
        /// <summary>
        /// 缓存数据
        /// </summary>
        [Key(0)]
        public CacheData CacheData { get; set; }

        /// <summary>
        /// 页索引
        /// </summary>
        [Key(1)]
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// 页大小
        /// </summary>
        [Key(2)]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// 总记录数
        /// </summary>
        [Key(3)]
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// 总页数
        /// </summary>
        [Key(4)]
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
        [IgnoreMember]
        public bool IsFirstPage => PageIndex <= 0;

        /// <summary>
        /// 是否最后一页
        /// </summary>
        [IgnoreMember]
        public bool IsLastPage => PageIndex >= TotalPages - 1;

        /// <summary>
        /// 是否有上一页
        /// </summary>
        [IgnoreMember]
        public bool HasPreviousPage => PageIndex > 0;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        [IgnoreMember]
        public bool HasNextPage => PageIndex < TotalPages - 1;
    }
}
