using RUINORERP.Common.Extensions;
using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.BizMapperService
{

    /// <summary>
    /// 增强型业务类型映射器
    /// </summary>
    public class EnhancedBizTypeMapper
    {
        private readonly EntityBizMappingService _mappingService;

        public EnhancedBizTypeMapper(EntityBizMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public BizType GetBizType(Type entityType, object entity = null)
            => _mappingService.GetBizType(entityType, entity);

        public Type GetEntityType(BizType bizType)
            => _mappingService.GetEntityType(bizType);

        public (string IdField, string NoField) GetEntityFields(Type entityType)
        {
            var config = _mappingService.GetFieldConfig(entityType);
            return (config?.IdField, config?.NoField);
        }

        /// <summary>
        /// 获取实体对象中主键字段和编号字段的值
        /// </summary>
        /// <typeparam name="TKey">主键字段的类型</typeparam>
        /// <param name="entityType">实体类型（仅用于配置检索，可传 null 让方法内部推断）</param>
        /// <param name="entity">实体实例</param>
        /// <returns>(主键值, 编号值)</returns>
        public (TKey Id, string No) GetEntityFieldValue<TKey>(Type entityType, object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            // 如果 entityType 为空，就用 entity.GetType() 兜底
            var type = entityType ?? entity.GetType();

            var (idField, noField) = GetEntityFields(type);

            if (string.IsNullOrEmpty(idField))
                throw new InvalidOperationException($"未在配置中找到 {type.Name} 的主键字段。");

            var idValue = (TKey)entity.GetPropertyValue(idField);

            // 编号字段允许为空
            var noValue = string.IsNullOrEmpty(noField)
                ? null
                : entity.GetPropertyValue(noField) as string;

            return (idValue, noValue);
        }

        /// <summary>
        /// 语义更清晰的别名
        /// </summary>
        public (TKey Id, string No) GetKeyAndNoValues<TKey>(object entity) =>
            GetEntityFieldValue<TKey>(null, entity);


        public string GetDetailProperty(Type entityType)
        {
            var config = _mappingService.GetFieldConfig(entityType);
            return config?.DetailProperty;
        }
    }
}
