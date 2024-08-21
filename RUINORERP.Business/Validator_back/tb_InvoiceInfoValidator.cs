﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:13
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
    /// 开票资料验证类
    /// </summary>
    public partial class tb_InvoiceInfoValidator:AbstractValidator<tb_InvoiceInfo>
    {
     public tb_InvoiceInfoValidator() 
     {
      RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("往来单位:下拉选择值不正确。");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PICompanyName).MaximumLength(200).WithMessage("公司名称:不能超过最大长度,200.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PITaxID).MaximumLength(100).WithMessage("税号:不能超过最大长度,100.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PIAddress).MaximumLength(200).WithMessage("地址:不能超过最大长度,200.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PITEL).MaximumLength(50).WithMessage("电话:不能超过最大长度,50.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PIBankName).MaximumLength(150).WithMessage("开户行:不能超过最大长度,150.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.PIBankNo).MaximumLength(50).WithMessage("银行帐号:不能超过最大长度,50.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.Notes).MaximumLength(255).WithMessage(":不能超过最大长度,255.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.信用天数).MaximumLength(10).WithMessage(":不能超过最大长度,10.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.往来余额).MaximumLength(10).WithMessage(":不能超过最大长度,10.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.应收款).MaximumLength(10).WithMessage(":不能超过最大长度,10.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.预收款).MaximumLength(10).WithMessage(":不能超过最大长度,10.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.纳税号).MaximumLength(10).WithMessage(":不能超过最大长度,10.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.开户行).MaximumLength(10).WithMessage(":不能超过最大长度,10.");
 RuleFor(tb_InvoiceInfo =>tb_InvoiceInfo.银行帐号).MaximumLength(10).WithMessage(":不能超过最大长度,10.");
       	
           	
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

