
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:10
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

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
    public partial class tb_CurrencyExchangeRateValidator:BaseValidatorGeneric<tb_CurrencyExchangeRate>
    {
     

     public tb_CurrencyExchangeRateValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_CurrencyExchangeRate =>tb_CurrencyExchangeRate.ConversionName).MaximumMixedLength(50).WithMessage("换算名称:不能超过最大长度,50.");
 RuleFor(tb_CurrencyExchangeRate =>tb_CurrencyExchangeRate.ConversionName).NotEmpty().WithMessage("换算名称:不能为空。");

//***** 
 RuleFor(tb_CurrencyExchangeRate =>tb_CurrencyExchangeRate.BaseCurrencyID).NotNull().WithMessage("基本币别:不能为空。");

//***** 
 RuleFor(tb_CurrencyExchangeRate =>tb_CurrencyExchangeRate.TargetCurrencyID).NotNull().WithMessage("目标币别:不能为空。");



 RuleFor(x => x.DefaultExchRate).PrecisionScale(18,6,true).WithMessage("预设汇率:小数位不能超过6。");

 RuleFor(x => x.ExecuteExchRate).PrecisionScale(18,6,true).WithMessage("执行汇率:小数位不能超过6。");

//有默认值

 RuleFor(tb_CurrencyExchangeRate =>tb_CurrencyExchangeRate.Notes).MaximumMixedLength(100).WithMessage(":不能超过最大长度,100.");


 RuleFor(tb_CurrencyExchangeRate =>tb_CurrencyExchangeRate.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_CurrencyExchangeRate =>tb_CurrencyExchangeRate.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

           	  
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

