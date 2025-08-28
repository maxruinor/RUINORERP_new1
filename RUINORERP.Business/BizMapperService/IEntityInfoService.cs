using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 实体信息服务接口
    /// </summary>
    public interface IEntityInfoService
    {
        /// <summary>
        /// 根据业务类型获取实体信息
        /// </summary>
        EntityInfo GetEntityInfo(BizType bizType);
        
        /// <summary>
        /// 根据实体类型获取实体信息
        /// </summary>
        EntityInfo GetEntityInfo(Type entityType);
        
        /// <summary>
        /// 根据表名获取实体信息
        /// </summary>
        EntityInfo GetEntityInfoByTableName(string tableName);
        
        /// <summary>
        /// 获取所有注册的实体信息
        /// </summary>
        IEnumerable<EntityInfo> GetAllEntityInfos();
        
        /// <summary>
        /// 获取业务类型对应的实体类型
        /// </summary>
        Type GetEntityType(BizType bizType);
        
        /// <summary>
        /// 获取实体类型对应的业务类型
        /// </summary>
        BizType GetBizType(Type entityType, object entity = null);
        
        /// <summary>
        /// 通过实体实例获取业务类型
        /// </summary>
        BizType GetBizTypeByEntity(object entity);
        
        /// <summary>
        /// 获取表名对应的实体类型
        /// </summary>
        Type GetEntityTypeByTableName(string tableName);
        
        /// <summary>
        /// 注册实体信息
        /// </summary>
        void RegisterEntity<TEntity>(BizType bizType, Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class;
        
        /// <summary>
        /// 注册共用表实体信息
        /// </summary>
        void RegisterSharedTable<TEntity, TDiscriminator>(
            Func<TDiscriminator, BizType> typeResolver,
            Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class;
        
        /// <summary>
        /// 判断业务类型是否已注册
        /// </summary>
        bool IsRegistered(BizType bizType);
        
        /// <summary>
        /// 判断实体类型是否已注册
        /// </summary>
        bool IsRegistered(Type entityType);
        
        /// <summary>
        /// 判断表名是否已注册
        /// </summary>
        bool IsRegisteredByTableName(string tableName);

        /// <summary>
        /// 获取对象的ID和名称的值
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>包含ID和名称值的元组</returns>
        (long Id, string Name) GetIdAndName(object entity);
    }
    
    /// <summary>
    /// 实体信息服务扩展方法
    /// </summary>
    public static class EntityInfoServiceExtensions
    {
        /// <summary>
        /// 获取实体信息的泛型版本
        /// </summary>
        public static EntityInfo GetEntityInfo<TEntity>(this IEntityInfoService service) where TEntity : class
        {
            return service.GetEntityInfo(typeof(TEntity));
        }
        
        /// <summary>
        /// 获取业务类型对应的实体类型的泛型版本
        /// </summary>
        public static Type GetEntityType<T>(this IEntityInfoService service) where T : class
        {
            return typeof(T).GetType();
            //return service.GetEntityType(typeof(T));
        }
        
        /// <summary>
        /// 通过实体实例获取业务类型的扩展方法
        /// </summary>
        public static BizType GetBizType<T>(this IEntityInfoService service, T entity) where T : class
        {
            if (entity == null)
                return BizType.Unknown;
            
            return service.GetBizType(typeof(T), entity);
        }
    }
}