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
    /// 功能配置验证类
    /// </summary>
    public partial class FunctionConfigurationValidator : BaseValidatorGeneric<FunctionConfiguration>
    {
        public FunctionConfigurationValidator(ApplicationContext appContext = null) : base(appContext)
        {
            // 目前 FunctionConfiguration 只有一个布尔类型的属性，无需验证
            // 如果未来添加更多属性，可以在这里添加验证规则

            // 示例：如果有字符串属性
            // RuleFor(x => x.PropertyName)
            //     .NotNull().WithMessage("属性名：不能为空。")
            //     .MaximumLength(50).WithMessage("属性名：不能超过50个字符。");
        }
    }
}
