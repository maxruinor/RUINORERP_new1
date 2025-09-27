using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 实体信息服务接口
    /// </summary>
    public interface IEntityInfoService
    {
        /// <summary>
        /// 初始化实体信息服务
        /// </summary>
        void Initialize();
        
        ERPEntityInfo GetEntityInfo(BizType bizType);
        ERPEntityInfo GetEntityInfo(Type entityType);

        /// <summary>
        /// 根据指定枚举类型的值来判断 是哪种业务类型
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="entityType"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        ERPEntityInfo GetEntityInfo(Type entityType, int EnumFlag);
        ERPEntityInfo GetEntityInfoByTableName(string tableName);
        IEnumerable<ERPEntityInfo> GetAllEntityInfos();
        Type GetEntityType(BizType bizType);
        BizType GetBizType(Type entityType, object entity = null);
        BizType GetBizTypeByEntity(object entity);
        Type GetEntityTypeByTableName(string tableName);
        void RegisterEntity<TEntity>(BizType bizType, Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class;
        void RegisterSharedTable<TEntity, TDiscriminator>(
            IDictionary<TDiscriminator, BizType> typeMapping,
            Expression<Func<TEntity, TDiscriminator>> discriminatorExpr,
            Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class;
        bool IsRegistered(BizType bizType);
        bool IsRegistered(Type entityType);
        bool IsRegisteredByTableName(string tableName);
        (long Id, string Name) GetIdAndName(object entity);
    }

    public static class EntityInfoServiceExtensions
    {

        public static ERPEntityInfo GetEntityInfo<TEntity>(this IEntityInfoService service, int Flag) where TEntity : class 
        {
            return service.GetEntityInfo(typeof(TEntity), Flag);
        }

        public static ERPEntityInfo GetEntityInfo<TEntity>(this IEntityInfoService service) where TEntity : class
        {
            return service.GetEntityInfo(typeof(TEntity));
        }

        public static Type GetEntityType<T>(this IEntityInfoService service) where T : class
        {
            return typeof(T);
        }

        public static BizType GetBizType<T>(this IEntityInfoService service, T entity) where T : class
        {
            if (entity == null)
                return BizType.无对应数据;

            return service.GetBizType(typeof(T), entity);
        }
    }
}
