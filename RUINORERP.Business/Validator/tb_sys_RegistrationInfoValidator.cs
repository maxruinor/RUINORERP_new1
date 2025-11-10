
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:23
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
    /// 系统注册信息验证类
    /// </summary>
    /*public partial class tb_sys_RegistrationInfoValidator:AbstractValidator<tb_sys_RegistrationInfo>*/
    public partial class tb_sys_RegistrationInfoValidator:BaseValidatorGeneric<tb_sys_RegistrationInfo>
    {
     

     public tb_sys_RegistrationInfoValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.CompanyName).MaximumMixedLength(200).WithMessage("注册公司名:不能超过最大长度,200.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.CompanyName).NotEmpty().WithMessage("注册公司名:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.FunctionModule).MaximumMixedLength(3000).WithMessage("功能模块:不能超过最大长度,3000.");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ContactName).MaximumMixedLength(200).WithMessage("联系人姓名:不能超过最大长度,200.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ContactName).NotEmpty().WithMessage("联系人姓名:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.PhoneNumber).MaximumMixedLength(200).WithMessage("手机号:不能超过最大长度,200.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.PhoneNumber).NotEmpty().WithMessage("手机号:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.MachineCode).MaximumMixedLength(3000).WithMessage("机器码:不能超过最大长度,3000.");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.RegistrationCode).MaximumMixedLength(3000).WithMessage("注册码:不能超过最大长度,3000.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.RegistrationCode).NotEmpty().WithMessage("注册码:不能为空。");

//***** 
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ConcurrentUsers).NotNull().WithMessage("同时在线用户数:不能为空。");


 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ProductVersion).MaximumMixedLength(200).WithMessage("版本信息:不能超过最大长度,200.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.ProductVersion).NotEmpty().WithMessage("版本信息:不能为空。");

 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.LicenseType).MaximumMixedLength(20).WithMessage("授权类型:不能超过最大长度,20.");
 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.LicenseType).NotEmpty().WithMessage("授权类型:不能为空。");




 RuleFor(tb_sys_RegistrationInfo =>tb_sys_RegistrationInfo.Remarks).MaximumMixedLength(200).WithMessage("备注:不能超过最大长度,200.");


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

