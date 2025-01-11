
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2025 12:08:37
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
    /// 系统注册信息验证类
    /// </summary>
    /*public partial class tb_sys_RegistrationInfoValidator:AbstractValidator<tb_sys_RegistrationInfo>*/
    public partial class tb_sys_RegistrationInfoValidator:BaseValidatorGeneric<tb_sys_RegistrationInfo>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_sys_RegistrationInfoValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.CompanyName).MaximumLength(100).WithMessage("注册公司名:不能超过最大长度,100.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.CompanyName).NotEmpty().WithMessage("注册公司名:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.FunctionModule).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ContactName).MaximumLength(100).WithMessage("联系人姓名:不能超过最大长度,100.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ContactName).NotEmpty().WithMessage("联系人姓名:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.PhoneNumber).MaximumLength(100).WithMessage("手机号:不能超过最大长度,100.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.PhoneNumber).NotEmpty().WithMessage("手机号:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.MachineCode).MaximumLength(1500).WithMessage("机器码:不能超过最大长度,1500.");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.RegistrationCode).MaximumLength(1500).WithMessage("注册码:不能超过最大长度,1500.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.RegistrationCode).NotEmpty().WithMessage("注册码:不能为空。");

//***** 
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ConcurrentUsers).NotNull().WithMessage("同时在线用户数:不能为空。");


 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ProductVersion).MaximumLength(100).WithMessage("版本信息:不能超过最大长度,100.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ProductVersion).NotEmpty().WithMessage("版本信息:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.LicenseType).MaximumLength(10).WithMessage("授权类型:不能超过最大长度,10.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.LicenseType).NotEmpty().WithMessage("授权类型:不能为空。");




 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.Remarks).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");


 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

