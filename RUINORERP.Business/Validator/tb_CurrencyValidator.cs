
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:28
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 币别资料表验证类
    /// </summary>
    public partial class tb_CurrencyValidator:AbstractValidator<tb_Currency>
    {
     public tb_CurrencyValidator() 
     {
      RuleFor(tb_Currency =>tb_Currency.GroupName).MaximumLength(50).WithMessage("组合名称:不能超过最大长度,50.");
 RuleFor(tb_Currency =>tb_Currency.CurrencyCode).MaximumLength(10).WithMessage("外币代码:不能超过最大长度,10.");
 RuleFor(tb_Currency =>tb_Currency.CurrencyName).MaximumLength(20).WithMessage("外币名称:不能超过最大长度,20.");
 RuleFor(tb_Currency =>tb_Currency.CurrencyName).NotEmpty().WithMessage("外币名称:不能为空。");
 RuleFor(x => x.DefaultExchRate).PrecisionScale(10,2,true).WithMessage("预设汇率:小数位不能超过2。");
 RuleFor(x => x.BuyExchRate).PrecisionScale(10,2,true).WithMessage("买入汇率:小数位不能超过2。");
 RuleFor(x => x.SellOutExchRate).PrecisionScale(10,2,true).WithMessage("卖出汇率:小数位不能超过2。");
 RuleFor(x => x.MonthEndExchRate).PrecisionScale(10,2,true).WithMessage("月末汇率:小数位不能超过2。");
//有默认值
//有默认值
 RuleFor(tb_Currency =>tb_Currency.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Currency =>tb_Currency.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
           	
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PrePaymentBillDetail> details)
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

