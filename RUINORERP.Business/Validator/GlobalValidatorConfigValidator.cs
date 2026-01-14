using FluentValidation;
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Business
{
    /// <summary>
    /// GlobalValidatorConfig配置验证器
    /// 用于验证全局验证配置的合法性
    /// </summary>
    public class GlobalValidatorConfigValidator : BaseValidatorGeneric<GlobalValidatorConfig>
    {
        // 配置全局参数
        public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;

        public GlobalValidatorConfigValidator(IOptionsMonitor<GlobalValidatorConfig> config = null)
        {
            ValidatorConfig = config;

            // 生产模块配置验证
            RuleFor(x => x.ReworkTipDays).GreaterThanOrEqualTo(0).WithMessage("返工提醒天数不能小于0");
            RuleFor(x => x.ReworkTipDays).LessThanOrEqualTo(365).WithMessage("返工提醒天数不能超过365天");

            // 客户关系模块配置验证
            RuleFor(x => x.计划提前提示天数).GreaterThanOrEqualTo(0).WithMessage("计划提前提示天数不能小于0");
            RuleFor(x => x.计划提前提示天数).LessThanOrEqualTo(30).WithMessage("计划提前提示天数不能超过30天");

            // 销售模块配置验证
            RuleFor(x => x.MoneyDataPrecision).GreaterThanOrEqualTo(0).WithMessage("销售金额精度不能小于0");
            RuleFor(x => x.MoneyDataPrecision).LessThanOrEqualTo(8).WithMessage("销售金额精度不能超过8位小数");
            // IsFromPlatform是布尔值，无需额外验证
            // NeedInputProjectGroup是布尔值，无需额外验证

            // 通用配置设置验证
            RuleFor(x => x.SomeSetting).MaximumLength(200).WithMessage("通用配置设置长度不能超过200个字符");

          
        }
    }
}