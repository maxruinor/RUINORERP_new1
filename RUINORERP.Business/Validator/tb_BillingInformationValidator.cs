
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:11
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
    /// 开票资料表验证类
    /// </summary>
    /*public partial class tb_BillingInformationValidator:AbstractValidator<tb_BillingInformation>*/
    public partial class tb_BillingInformationValidator:BaseValidatorGeneric<tb_BillingInformation>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_BillingInformationValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_BillingInformation =>tb_BillingInformation.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("往来单位:下拉选择值不正确。");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.Title).MaximumMixedLength(200).WithMessage("抬头:不能超过最大长度,200.");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.TaxNumber).MaximumMixedLength(200).WithMessage("税号:不能超过最大长度,200.");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.Address).MaximumMixedLength(200).WithMessage("地址:不能超过最大长度,200.");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.PITEL).MaximumMixedLength(50).WithMessage("电话:不能超过最大长度,50.");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.BankAccount).MaximumMixedLength(150).WithMessage("银行账号:不能超过最大长度,150.");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.BankName).MaximumMixedLength(50).WithMessage("开户行:不能超过最大长度,50.");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.Email).MaximumMixedLength(150).WithMessage("邮箱:不能超过最大长度,150.");

 RuleFor(tb_BillingInformation =>tb_BillingInformation.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


 RuleFor(tb_BillingInformation =>tb_BillingInformation.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_BillingInformation =>tb_BillingInformation.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


//有默认值

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

