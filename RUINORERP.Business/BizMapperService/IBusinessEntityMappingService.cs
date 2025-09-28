using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 业务实体映射服务接口
    /// 提供业务类型(BizType)与实体类型(Type)、数据库表名之间的相互转换功能
    /// </summary>
    public interface IBusinessEntityMappingService
    {
        /// <summary>
        /// 初始化业务实体映射服务
        /// 此方法仅检查初始化状态，实际的实体映射注册通过InitializeMappings方法完成
        /// </summary>
        void Initialize();
        

        
        ERPEntityInfo GetEntityInfo(BizType bizType);
        ERPEntityInfo GetEntityInfo(Type entityType);

        /// <summary>
        /// 根据指定枚举类型的值来判断是哪种业务类型
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="EnumFlag"></param>
        /// <returns></returns>
        ERPEntityInfo GetEntityInfo(Type entityType, int EnumFlag);
        ERPEntityInfo GetEntityInfoByTableName(string tableName);
        IEnumerable<ERPEntityInfo> GetAllEntityInfos();
        Type GetEntityType(BizType bizType);
        BizType GetBizType(Type entityType, object entity = null);
        BizType GetBizTypeByEntity(object entity);
        Type GetEntityTypeByTableName(string tableName);
        //void RegisterEntity<TEntity>(BizType bizType, Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class;
        //void RegisterSharedTable<TEntity, TDiscriminator>(
        //    IDictionary<TDiscriminator, BizType> typeMapping,
        //    Expression<Func<TEntity, TDiscriminator>> discriminatorExpr,
        //    Action<EntityInfoBuilder<TEntity>> configure = null) where TEntity : class;
        bool IsRegistered(BizType bizType);
        bool IsRegistered(Type entityType);
        bool IsRegisteredByTableName(string tableName);
        (long Id, string Name) GetIdAndName(object entity);
    }

    public static class BusinessEntityMappingServiceExtensions
    {

        public static ERPEntityInfo GetEntityInfo<TEntity>(this IBusinessEntityMappingService service, int Flag) where TEntity : class 
        {
            return service.GetEntityInfo(typeof(TEntity), Flag);
        }

        public static ERPEntityInfo GetEntityInfo<TEntity>(this IBusinessEntityMappingService service) where TEntity : class
        {
            return service.GetEntityInfo(typeof(TEntity));
        }

        public static Type GetEntityType<T>(this IBusinessEntityMappingService service) where T : class
        {
            return typeof(T);
        }

        public static BizType GetBizType<T>(this IBusinessEntityMappingService service, T entity) where T : class
        {
            if (entity == null)
                return BizType.无对应数据;

            return service.GetBizType(typeof(T), entity);
        }
    }
}
