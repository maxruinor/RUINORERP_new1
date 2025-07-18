using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RUINORERP.Model;
using System.Reflection;


namespace RUINORERP.Business.AutoMapper
{
    public static class AutoMapperExtensions
    {

        /// <summary>
        /// 忽略一些字段
        /// </summary>
        /// <param name="cfg"></param>
        public static void IgnoreBaseEntityProperties(this IMapperConfigurationExpression cfg)
        {
            cfg.ForAllMaps((typeMap, mappingExpression) =>
            {
                if (typeof(BaseEntity).IsAssignableFrom(typeMap.DestinationType))
                {
                    var ignoreProps = new[]
                    {
                    nameof(BaseEntity.ChangedProperties),
                    nameof(BaseEntity.HasChanged),
                    nameof(BaseEntity.FieldNameList),
                    nameof(BaseEntity.ActionStatus),
                    nameof(BaseEntity.RowImage)
                };

                    foreach (var propName in ignoreProps)
                    {
                        var propertyInfo = typeMap.DestinationType.GetProperty(propName);
                        if (propertyInfo != null)
                        {
                            mappingExpression.ForMember(propName, opt => opt.Ignore());
                        }
                    }
                }
            });
        }

        public static void ApplySmartConventions(this IMapperConfigurationExpression cfg)
        {
            cfg.ForAllMaps((typeMap, mappingExpression) =>
            {
                // 自动忽略所有 BaseEntity 中的状态属性
                if (typeof(BaseEntity).IsAssignableFrom(typeMap.DestinationType))
                {
                    mappingExpression.AfterMap((src, dest) =>
                    {
                        if (dest is BaseEntity entity)
                        {
                            entity.ResetChangeTracking();
                            entity.ActionStatus = ActionStatus.加载;
                        }
                    });
                }

                // 空值不覆盖目标值
                //foreach (var memberMap in typeMap.MemberMaps)
                //{
                //    if (memberMap.DestinationMember.CanWrite)
                //    {
                //        mappingExpression.ForMember(memberMap.DestinationName, opt =>
                //            opt.Condition((src, dest, srcMember) =>
                //                srcMember != null && !IsDefaultValue(srcMember)));
                //    }
                //}
            });
        }

        private static bool IsDefaultValue(object value)
        {
            if (value == null) return true;
            var type = value.GetType();
            return type.IsValueType && value.Equals(Activator.CreateInstance(type));
        }
    }
}
