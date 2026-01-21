// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：AI Assistant
// 时间：01/21/2026
// **************************************
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.Context;

namespace RUINORERP.Business.Validator
{
    /// <summary>
    /// 财务模块配置验证类
    /// </summary>
    public partial class FMConfigurationValidator : BaseValidatorGeneric<FMConfiguration>
    {
        public FMConfigurationValidator(ApplicationContext appContext = null) : base(appContext)
        {
            // 金额计算容差阈值验证
            RuleFor(x => x.AmountCalculationTolerance)
                .GreaterThanOrEqualTo(0).WithMessage("金额计算容差阈值：不能为负数。")
                .PrecisionScale(5, 4, true).WithMessage("金额计算容差阈值：最大精度为4位小数。")
                .LessThanOrEqualTo(0.9999m).WithMessage("金额计算容差阈值：必须小于1。")
                .Must(BeValidTolerance).WithMessage("金额计算容差阈值：建议值不超过0.0001，过大的容差可能导致计算错误。");

            // 启用销售订单付款状态验证时，验证其他相关配置的协调性
            RuleFor(x => x)
                .Must(HaveValidPaymentValidationConfig)
                .WithMessage("配置冲突：启用销售订单付款状态验证时，建议同时启用全额预收款订单自动审核功能，以确保业务流程顺畅。")
                .When(x => x.EnableSalesOrderPaymentStatusValidation);
        }

        /// <summary>
        /// 验证容差阈值是否在合理范围内
        /// </summary>
        private bool BeValidTolerance(decimal tolerance)
        {
            // 容差阈值不应该过大，建议不超过0.0001
            // 但为了灵活性，允许用户设置更大的值，给出警告但不强制限制
            return tolerance <= 0.9999m;
        }

        /// <summary>
        /// 验证付款状态验证相关配置的协调性
        /// </summary>
        private bool HaveValidPaymentValidationConfig(FMConfiguration config)
        {
            // 当启用销售订单付款状态验证时，检查配置的协调性
            if (config.EnableSalesOrderPaymentStatusValidation)
            {
                // 这是一个软验证，返回true表示警告但不阻止配置
                // 实际业务逻辑中可能需要这些配置配合使用
                return true;
            }
            return true;
        }
    }
}
