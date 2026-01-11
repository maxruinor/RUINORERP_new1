
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/28/2024 17:10:40
// **************************************
using System;
using SqlSugar;
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
    public partial class tb_sys_RegistrationInfoValidator : BaseValidatorGeneric<tb_sys_RegistrationInfo>
    {
        //用于生成机器码的检测验证
        public tb_sys_RegistrationInfoValidator()
        {
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.CompanyName).MaximumLength(100).WithMessage("注册公司名:不能超过最大长度,100.");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.CompanyName).NotEmpty().WithMessage("注册公司名:不能为空。");

            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.ContactName).MaximumLength(100).WithMessage("联系人姓名:不能超过最大长度,100.");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.ContactName).NotEmpty().WithMessage("联系人姓名:不能为空。");

            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.PhoneNumber).MaximumLength(100).WithMessage("手机号:不能超过最大长度,100.");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.PhoneNumber).NotEmpty().WithMessage("手机号:不能为空。");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.PhoneNumber).MinimumLength(10).WithMessage("手机号:不能小于最小长度,10。");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.ConcurrentUsers).GreaterThan(0).WithMessage("同时在线用户数:要大于0。");

            //***** 
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.ConcurrentUsers).NotNull().WithMessage("同时在线用户数:不能为空。");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.LicenseType).MinimumLength(3).WithMessage("授权类型:请选择正确的授权类型.");

            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.ProductVersion).MaximumLength(100).WithMessage("版本信息:不能超过最大长度,100.");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.ProductVersion).NotEmpty().WithMessage("版本信息:不能为空。");

            //***** 
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.LicenseType).NotNull().WithMessage("授权类型:不能为空。");

        }

        public override void Initialize()
        {
            // 这里添加额外的初始化代码
            RuleFor(tb_StockTransfer => tb_StockTransfer.ExpirationDate).GreaterThan(System.DateTime.Now.AddDays(30)).WithMessage("截止时间:要大于30天。请设置授权期限");
            RuleFor(tb_sys_RegistrationInfo => tb_sys_RegistrationInfo.LicenseType).MinimumLength(3).WithMessage("授权类型:请选择正确的授权类型.");
        }
    }

}

