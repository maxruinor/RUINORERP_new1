
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:13
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
    /// 开票资料验证类
    /// </summary>
    /*public partial class tb_InvoiceInfoValidator:AbstractValidator<tb_InvoiceInfo>*/
    public partial class tb_InvoiceInfoValidator:BaseValidatorGeneric<tb_InvoiceInfo>
    {
     

     public tb_InvoiceInfoValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("往来单位:下拉选择值不正确。");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PICompanyName).MaximumMixedLength(200).WithMessage("公司名称:不能超过最大长度,200.");

 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PITaxID).MaximumMixedLength(100).WithMessage("税号:不能超过最大长度,100.");

 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PIAddress).MaximumMixedLength(200).WithMessage("地址:不能超过最大长度,200.");

 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PITEL).MaximumMixedLength(50).WithMessage("电话:不能超过最大长度,50.");

 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PIBankName).MaximumMixedLength(150).WithMessage("开户行:不能超过最大长度,150.");

 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PIBankNo).MaximumMixedLength(50).WithMessage("银行帐号:不能超过最大长度,50.");

 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.Notes).MaximumMixedLength(255).WithMessage(":不能超过最大长度,255.");








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

