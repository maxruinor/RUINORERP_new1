
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 20:05:19
// **************************************
using System;
﻿using SqlSugar;
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
    /// 损益费用单验证类
    /// </summary>
    /*public partial class tb_FM_ProfitLossDetailValidator:AbstractValidator<tb_FM_ProfitLossDetail>*/
    public partial class tb_FM_ProfitLossDetailValidator:BaseValidatorGeneric<tb_FM_ProfitLossDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_ProfitLossDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.ProfitLossId).NotEmpty().When(x => x.ProfitLossId.HasValue);

 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("产品:下拉选择值不正确。");
 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.ProfitLossType).NotNull().WithMessage("损溢类型:不能为空。");

 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.IncomeExpenseDirection).NotEmpty().When(x => x.IncomeExpenseDirection.HasValue);

 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.ExpenseDescription).MaximumMixedLength(300).WithMessage("费用说明:不能超过最大长度,300.");

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(x => x.Quantity).PrecisionScale(10,4,true).WithMessage("数量:小数位不能超过4。");

 RuleFor(x => x.SubtotalAmont).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");

 RuleFor(x => x.UntaxedSubtotalAmont).PrecisionScale(19,4,true).WithMessage("未税小计:小数位不能超过4。");

 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxSubtotalAmont).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(tb_FM_ProfitLossDetail =>tb_FM_ProfitLossDetail.Summary).MaximumMixedLength(300).WithMessage("摘要:不能超过最大长度,300.");

           	        Initialize();
     }







    
          private bool CheckForeignKeyValue(long ForeignKeyID)
        {
            bool rs = true;    
            if (ForeignKeyID == 0 || ForeignKeyID == -1)
            {
                return false;
            }
            return rs;
        }
        
        private bool CheckForeignKeyValueCanNull(long? ForeignKeyID)
        {
            bool rs = true;
            if (ForeignKeyID.HasValue)
            {
                if (ForeignKeyID == 0 || ForeignKeyID == -1)
                {
                    return false;
                }
            }
            return rs;
        
    }
}

}

