
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:25
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
    /// 银行账号信息表验证类
    /// </summary>
    /*public partial class tb_BankAccountValidator:AbstractValidator<tb_BankAccount>*/
    public partial class tb_BankAccountValidator:BaseValidatorGeneric<tb_BankAccount>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_BankAccountValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_BankAccount =>tb_BankAccount.Account_Name).MaximumLength(50).WithMessage("账户名称:不能超过最大长度,50.");
 RuleFor(tb_BankAccount =>tb_BankAccount.Account_Name).NotEmpty().WithMessage("账户名称:不能为空。");

 RuleFor(tb_BankAccount =>tb_BankAccount.Account_No).MaximumLength(50).WithMessage("账号:不能超过最大长度,50.");
 RuleFor(tb_BankAccount =>tb_BankAccount.Account_No).NotEmpty().WithMessage("账号:不能为空。");

 RuleFor(tb_BankAccount =>tb_BankAccount.OpeningBank).MaximumLength(50).WithMessage("开户行:不能超过最大长度,50.");
 RuleFor(tb_BankAccount =>tb_BankAccount.OpeningBank).NotEmpty().WithMessage("开户行:不能为空。");

//有默认值

 RuleFor(tb_BankAccount =>tb_BankAccount.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");

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

