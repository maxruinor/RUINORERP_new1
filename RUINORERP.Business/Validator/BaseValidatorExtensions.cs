
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：2026/04/02
// **************************************
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using System;
using System.Linq.Expressions;

namespace RUINORERP.Business.Validator
{
    /// <summary>
    /// 基础验证器扩展类
    /// 提供验证层值转换的扩展方法
    /// </summary>
    public static class BaseValidatorExtensions
    {
        /// <summary>
        /// 为可空外键字段添加智能验证规则
        /// 包含值转换和验证逻辑
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="validator">验证器</param>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="fieldName">字段显示名称</param>
        /// <param name="isRequired">是否必填</param>
        /// <param name="config">全局验证配置</param>
        public static void RuleForNullableForeignKey<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, long?>> propertyExpression,
            string fieldName,
            bool isRequired = false,
            GlobalValidatorConfig config = null) where T : class
        {
            // 首先应用值转换 - 将-1或0转换为null
            validator.ApplyPropertyTransformation(propertyExpression, ValueTransformConfig.Default);

            // 添加外键值验证
            validator.RuleFor(propertyExpression)
                .Must((instance, value) =>
                {
                    // 如果值为null，根据是否必填决定验证结果
                    if (!value.HasValue)
                    {
                        return !isRequired;
                    }
                    // 检查是否为有效的非零非负一值
                    return value.Value != 0 && value.Value != -1;
                })
                .WithMessage($"{fieldName}:下拉选择值不正确。");

            // 如果必填，添加非空验证
            if (isRequired)
            {
                validator.RuleFor(propertyExpression)
                    .NotNull()
                    .WithMessage($"{fieldName}:不能为空。");
            }
        }

        /// <summary>
        /// 根据全局配置决定是否允许预收付款单的账户字段为空
        /// 默认值为false，表示预收付款单的账户和付款方式可以为空
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="validator">验证器</param>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="fieldName">字段显示名称</param>
        /// <param name="config">全局验证配置</param>
        public static void RuleForPreReceivedPaymentAccountField<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, long?>> propertyExpression,
            string fieldName,
            GlobalValidatorConfig config = null) where T : class
        {
            // 确定是否必填（根据全局配置【预收付款单账户必填】）
            // 默认值为false，表示预收付款单的账户和付款方式可以为空
            bool isRequired = config?.预收付款单账户必填 ?? false;

            // 应用智能验证规则
            validator.RuleForNullableForeignKey(
                propertyExpression,
                fieldName,
                isRequired,
                config);
        }

        /// <summary>
        /// 根据全局配置决定是否允许收付款单的账户字段为空
        /// 默认值为true，表示收付款单的账户和付款方式必须填写
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="validator">验证器</param>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="fieldName">字段显示名称</param>
        /// <param name="config">全局验证配置</param>
        public static void RuleForPaymentRecordAccountField<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, long?>> propertyExpression,
            string fieldName,
            GlobalValidatorConfig config = null) where T : class
        {
            // 确定是否必填（根据全局配置【收付款单账户必填】）
            // 默认值为true，表示收付款单的账户和付款方式必须填写
            bool isRequired = config?.收付款单账户必填 ?? true;

            // 应用智能验证规则
            validator.RuleForNullableForeignKey(
                propertyExpression,
                fieldName,
                isRequired,
                config);
        }

        /// <summary>
        /// 【已弃用】请使用 RuleForPreReceivedPaymentAccountField 或 RuleForPaymentRecordAccountField 替代
        /// 根据全局配置决定是否允许账户字段为空
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="validator">验证器</param>
        /// <param name="propertyExpression">属性表达式</param>
        /// <param name="fieldName">字段显示名称</param>
        /// <param name="config">全局验证配置</param>
        [Obsolete("请使用 RuleForPreReceivedPaymentAccountField 或 RuleForPaymentRecordAccountField 替代")]
        public static void RuleForAccountField<T>(
            this AbstractValidator<T> validator,
            Expression<Func<T, long?>> propertyExpression,
            string fieldName,
            GlobalValidatorConfig config = null) where T : class
        {
            // 为了向后兼容，使用旧的配置项
            // 如果新的配置项未设置，则回退到旧的配置项
            bool isRequired = config?.收付款账户必填 ?? true;

            // 应用智能验证规则
            validator.RuleForNullableForeignKey(
                propertyExpression,
                fieldName,
                isRequired,
                config);
        }
    }
}
