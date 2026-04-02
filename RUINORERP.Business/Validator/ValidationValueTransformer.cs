
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：2026/04/02
// **************************************
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace RUINORERP.Business.Validator
{
    /// <summary>
    /// 验证层值转换器
    /// 用于在FluentValidation验证过程中动态修改实体属性值
    /// 主要解决UI层传递的默认值(如-1)与业务层允许null值之间的转换问题
    /// </summary>
    public static class ValidationValueTransformer
    {
        /// <summary>
        /// 需要转换为null的默认值集合
        /// </summary>
        public static readonly HashSet<long> DefaultValuesToNull = new HashSet<long> { -1, 0 };

        /// <summary>
        /// 为实体添加属性值转换拦截器
        /// 在验证前自动将指定的默认值转换为null
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="validator">验证器</param>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="transformConfig">转换配置</param>
        public static void ApplyPropertyTransformation<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, long?>> propertyExpression,
            ValueTransformConfig transformConfig = null) where T : class
        {
            if (transformConfig == null)
            {
                transformConfig = ValueTransformConfig.Default;
            }

            if (!transformConfig.EnableTransformation)
            {
                return;
            }

            // 获取属性信息
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException("表达式必须是成员访问表达式", nameof(propertyExpression));
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("表达式必须访问属性", nameof(propertyExpression));
            }

            // 使用自定义验证规则来实现值转换
            validator.RuleFor(propertyExpression)
                .Custom((value, context) =>
                {
                    var instance = context.InstanceToValidate;
                    if (instance == null) return;

                    // 检查是否需要转换
                    if (value.HasValue && transformConfig.ValuesToTransform.Contains(value.Value))
                    {
                        // 将属性值设置为null
                        propertyInfo.SetValue(instance, null);
                    }
                });
        }
    }

    /// <summary>
    /// 值转换配置类
    /// </summary>
    public class ValueTransformConfig
    {
        /// <summary>
        /// 是否启用值转换
        /// </summary>
        public bool EnableTransformation { get; set; } = true;

        /// <summary>
        /// 需要转换为null的值集合
        /// </summary>
        public HashSet<long> ValuesToTransform { get; set; } = new HashSet<long> { -1, 0 };

        /// <summary>
        /// 默认配置实例
        /// </summary>
        public static ValueTransformConfig Default => new ValueTransformConfig
        {
            EnableTransformation = true,
            ValuesToTransform = new HashSet<long> { -1, 0 }
        };

        /// <summary>
        /// 禁用转换的配置
        /// </summary>
        public static ValueTransformConfig Disabled => new ValueTransformConfig
        {
            EnableTransformation = false,
            ValuesToTransform = new HashSet<long>()
        };
    }
}
