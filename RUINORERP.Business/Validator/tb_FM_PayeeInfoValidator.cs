
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/29/2025 11:22:21
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
    /// 收款信息，供应商报销人的收款账号验证类
    /// </summary>
    /*public partial class tb_FM_PayeeInfoValidator:AbstractValidator<tb_FM_PayeeInfo>*/
    public partial class tb_FM_PayeeInfoValidator:BaseValidatorGeneric<tb_FM_PayeeInfo>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PayeeInfoValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("员工:下拉选择值不正确。");
 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("往来单位:下拉选择值不正确。");
 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

//***** 
 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.Account_type).NotNull().WithMessage("账户类型:不能为空。");

 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.Account_name).MaximumLength(25).WithMessage("账户名称:不能超过最大长度,25.");

 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.Account_No).MaximumLength(50).WithMessage("账号:不能超过最大长度,50.");

 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.PaymentCodeImagePath).MaximumLength(150).WithMessage("收款码:不能超过最大长度,150.");

 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.BelongingBank).MaximumLength(25).WithMessage("所属银行:不能超过最大长度,25.");

 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.OpeningBank).MaximumLength(30).WithMessage("开户行:不能超过最大长度,30.");

 RuleFor(tb_FM_PayeeInfo =>tb_FM_PayeeInfo.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");


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

