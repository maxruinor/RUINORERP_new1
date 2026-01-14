
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
    /// 币别资料表验证类
    /// </summary>
    /*public partial class tb_CurrencyValidator:AbstractValidator<tb_Currency>*/
    public partial class tb_CurrencyValidator:BaseValidatorGeneric<tb_Currency>
    {
     

     public tb_CurrencyValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_Currency =>tb_Currency.Country).MaximumMixedLength(50).WithMessage("国家:不能超过最大长度,50.");

 RuleFor(tb_Currency =>tb_Currency.CurrencyCode).MaximumMixedLength(50).WithMessage("币别代码:不能超过最大长度,50.");

 RuleFor(tb_Currency =>tb_Currency.CurrencyName).MaximumMixedLength(20).WithMessage("币别名称:不能超过最大长度,20.");
 RuleFor(tb_Currency =>tb_Currency.CurrencyName).NotEmpty().WithMessage("币别名称:不能为空。");

 RuleFor(tb_Currency =>tb_Currency.CurrencySymbol).MaximumMixedLength(50).WithMessage("币别符号:不能超过最大长度,50.");


//有默认值


 RuleFor(tb_Currency =>tb_Currency.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_Currency =>tb_Currency.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

           	  
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PaymentRecordDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_StatementDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

