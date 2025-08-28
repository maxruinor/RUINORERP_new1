using RUINORERP.Global;
using System;
using System.Linq.Expressions;

namespace RUINORERP.Business.BizMapperService
{
    /// <summary>
    /// 实体-业务映射服务接口
    /// </summary>
    public interface IEntityBizMappingService
    {
        /// <summary>
        /// 注册业务类型映射
        /// </summary>
        void RegisterMapping<TEntity>(
            BizType bizType,
            Expression<Func<TEntity, long>> idSelector,
            Expression<Func<TEntity, string>> noSelector,
            Expression<Func<TEntity, object>> detailsSelector = null)
            where TEntity : class;

        /// <summary>
        /// 注册共用表区分器
        /// </summary>
        void RegisterSharedTableDiscriminator<TEntity, TValue>(
            Expression<Func<TEntity, object>> discriminatorSelector,
            Func<TValue, BizType> typeResolver,
            Expression<Func<TEntity, long>> idSelector,
            Expression<Func<TEntity, string>> noSelector,
            Expression<Func<TEntity, object>> detailsSelector = null)
            where TEntity : class;

        /// <summary>
        /// 获取字段配置
        /// </summary>
        EntityFieldConfig GetFieldConfig(Type entityType);

        /// <summary>
        /// 获取业务类型对应的实体类型
        /// </summary>
        Type GetEntityType(BizType bizType);

        /// <summary>
        /// 获取实体类型对应的业务类型
        /// </summary>
        BizType GetBizType(Type entityType, object entity = null);

        /// <summary>
        /// 获取表名对应的实体类型
        /// </summary>
        Type GetEntityTypeByTableName(string tableName);
    }
}
