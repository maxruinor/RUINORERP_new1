using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Core;
using System;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 请求基类 - 提供所有请求的公共属性和方法
    /// </summary>
    [Serializable]
    public partial class RequestBase : IRequest
    {
        /// <summary>
        /// 请求唯一标识
        /// </summary>
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 请求时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 查询参数（用于复杂查询）
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestBase()
        {
            RequestId = IdGenerator.GenerateRequestId();
        }

        /// <summary>
        /// 设置请求标识
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>当前实例</returns>
        public virtual RequestBase WithRequestId(string requestId)
        {
            RequestId = requestId;
            return this;
        }
    }

    /// <summary>
    /// 业务实体请求基类 - 专门用于CRUD操作的请求
    /// </summary>
    /// <typeparam name="TEntity">业务实体类型</typeparam>
    public class RequestBase<TEntity> : RequestBase
        where TEntity : class
    {
        /// <summary>
        /// 业务实体数据
        /// </summary>
        public TEntity Entity { get; set; }

        /// <summary>
        /// 实体ID（用于查询、删除、更新操作）
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public EntityOperationType OperationType { get; set; }

        /// <summary>
        /// 数据版本号（用于乐观锁）
        /// </summary>
        public string DataVersion { get; set; }

        /// <summary>
        /// 是否包含关联数据
        /// </summary>
        public bool IncludeRelatedData { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public RequestBase()
        {
            // 移除重复的QueryParameters定义，使用基类的Parameters
        }

        /// <summary>
        /// 创建实体请求
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="includeRelatedData">是否包含关联数据</param>
        /// <returns>实体请求实例</returns>
        public static RequestBase<TEntity> CreateCreateRequest(TEntity entity, bool includeRelatedData = false)
        {
            return new RequestBase<TEntity>
            {
                Entity = entity,
                OperationType = EntityOperationType.Create,
                IncludeRelatedData = includeRelatedData
            };
        }

        /// <summary>
        /// 创建更新请求
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <param name="entityId">实体ID</param>
        /// <param name="dataVersion">数据版本号</param>
        /// <returns>实体请求实例</returns>
        public static RequestBase<TEntity> CreateUpdateRequest(TEntity entity, string entityId, string dataVersion = null)
        {
            return new RequestBase<TEntity>
            {
                Entity = entity,
                EntityId = entityId,
                OperationType = EntityOperationType.Update,
                DataVersion = dataVersion
            };
        }

        /// <summary>
        /// 创建删除请求
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <param name="dataVersion">数据版本号</param>
        /// <returns>实体请求实例</returns>
        public static RequestBase<TEntity> CreateDeleteRequest(string entityId, string dataVersion = null)
        {
            return new RequestBase<TEntity>
            {
                EntityId = entityId,
                OperationType = EntityOperationType.Delete,
                DataVersion = dataVersion
            };
        }

        /// <summary>
        /// 创建查询请求
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <param name="includeRelatedData">是否包含关联数据</param>
        /// <returns>实体请求实例</returns>
        public static RequestBase<TEntity> CreateGetRequest(string entityId, bool includeRelatedData = true)
        {
            return new RequestBase<TEntity>
            {
                EntityId = entityId,
                OperationType = EntityOperationType.Get,
                IncludeRelatedData = includeRelatedData
            };
        }

        /// <summary>
        /// 创建列表查询请求
        /// </summary>
        /// <param name="queryParameters">查询参数</param>
        /// <param name="includeRelatedData">是否包含关联数据</param>
        /// <returns>实体请求实例</returns>
        public static RequestBase<TEntity> CreateListRequest(Dictionary<string, object> queryParameters = null, bool includeRelatedData = false)
        {
            return new RequestBase<TEntity>
            {
                // 使用基类的Parameters属性而不是QueryParameters
                Parameters = queryParameters ?? new Dictionary<string, object>(),
                OperationType = EntityOperationType.List,
                IncludeRelatedData = includeRelatedData
            };
        }

        /// <summary>
        /// 创建自定义查询请求
        /// </summary>
        /// <param name="queryParameters">查询参数</param>
        /// <param name="includeRelatedData">是否包含关联数据</param>
        /// <returns>实体请求实例</returns>
        public static RequestBase<TEntity> CreateCustomRequest(Dictionary<string, object> queryParameters = null, bool includeRelatedData = false)
        {
            return new RequestBase<TEntity>
            {
                // 使用基类的Parameters属性而不是QueryParameters
                Parameters = queryParameters ?? new Dictionary<string, object>(),
                OperationType = EntityOperationType.Custom,
                IncludeRelatedData = includeRelatedData
            };
        }
    }

    /// <summary>
    /// 实体操作类型枚举
    /// </summary>
    public enum EntityOperationType
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create = 1,

        /// <summary>
        /// 更新
        /// </summary>
        Update = 2,

        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3,

        /// <summary>
        /// 获取单个实体
        /// </summary>
        Get = 4,

        /// <summary>
        /// 获取列表
        /// </summary>
        List = 5,

        /// <summary>
        /// 自定义查询
        /// </summary>
        Custom = 6,

        /// <summary>
        /// 批量创建
        /// </summary>
        BatchCreate = 7,

        /// <summary>
        /// 批量更新
        /// </summary>
        BatchUpdate = 8,

        /// <summary>
        /// 批量删除
        /// </summary>
        BatchDelete = 9
    }

    /// <summary>
    /// 批量实体请求类 - 支持批量CRUD操作
    /// </summary>
    /// <typeparam name="TEntity">业务实体类型</typeparam>
    
    public class BatchEntityRequest<TEntity> : RequestBase
        where TEntity : class
    {
        /// <summary>
        /// 实体列表
        /// </summary>
        public List<TEntity> Entities { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public EntityOperationType OperationType { get; set; }

        /// <summary>
        /// 是否事务处理
        /// </summary>
        public bool UseTransaction { get; set; } = true;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BatchEntityRequest()
        {
            Entities = new List<TEntity>();
        }

        /// <summary>
        /// 创建批量创建请求
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="useTransaction">是否使用事务</param>
        /// <returns>批量请求实例</returns>
        public static BatchEntityRequest<TEntity> CreateBatchCreateRequest(List<TEntity> entities, bool useTransaction = true)
        {
            return new BatchEntityRequest<TEntity>
            {
                Entities = entities,
                OperationType = EntityOperationType.BatchCreate,
                UseTransaction = useTransaction
            };
        }

        /// <summary>
        /// 创建批量更新请求
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <param name="useTransaction">是否使用事务</param>
        /// <returns>批量请求实例</returns>
        public static BatchEntityRequest<TEntity> CreateBatchUpdateRequest(List<TEntity> entities, bool useTransaction = true)
        {
            return new BatchEntityRequest<TEntity>
            {
                Entities = entities,
                OperationType = EntityOperationType.BatchUpdate,
                UseTransaction = useTransaction
            };
        }

        /// <summary>
        /// 创建批量删除请求
        /// </summary>
        /// <param name="entityIds">实体ID列表</param>
        /// <param name="useTransaction">是否使用事务</param>
        /// <returns>批量请求实例</returns>
        public static BatchEntityRequest<TEntity> CreateBatchDeleteRequest(List<string> entityIds, bool useTransaction = true)
        {
            return new BatchEntityRequest<TEntity>
            {
                OperationType = EntityOperationType.BatchDelete,
                UseTransaction = useTransaction
            };
        }
    }
}
