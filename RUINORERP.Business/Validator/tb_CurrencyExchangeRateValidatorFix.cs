
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2025 12:02:52
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 币别换算表验证类
    /// </summary>
    /*public partial class tb_CurrencyExchangeRateValidator:AbstractValidator<tb_CurrencyExchangeRate>*/
    public partial class tb_CurrencyExchangeRateValidator : BaseValidatorGeneric<tb_CurrencyExchangeRate>
    {

        public override void Initialize()
        {
            RuleFor(x => x.DefaultExchRate).GreaterThan(0).WithMessage("预设汇率:必须大于0。");
            RuleFor(x => x.ExecuteExchRate).GreaterThan(0).WithMessage("执行汇率:必须大于0。");

            //来源和目标不能相同
            RuleFor(x => x.BaseCurrencyID)
             .Custom((value, context) =>
             {
                 var entity = context.InstanceToValidate as tb_CurrencyExchangeRate;
                 // 确保实体不为null  并且是新增时才判断
                 if (entity != null && entity.ExchangeRateID == 0)
                 {
                     //string propertyName = context.PropertyName;
                     if (entity.BaseCurrencyID == entity.TargetCurrencyID)
                     {
                         context.AddFailure("基本币别和目标币别不能相同。");
                     }
                 }
             });
        }
    }

}

