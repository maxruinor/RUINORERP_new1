
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:32
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
    /// 系统参数验证类
    /// </summary>
    /*public partial class tb_SysParaValidator:AbstractValidator<tb_SysPara>*/
    public partial class tb_SysParaValidator:BaseValidatorGeneric<tb_SysPara>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_SysParaValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_SysPara =>tb_SysPara.CompanyCode).MaximumLength(5).WithMessage("公司代号:不能超过最大长度,5.");

 RuleFor(tb_SysPara =>tb_SysPara.CNName).MaximumLength(50).WithMessage("名称:不能超过最大长度,50.");

 RuleFor(tb_SysPara =>tb_SysPara.ENName).MaximumLength(50).WithMessage("英语名称:不能超过最大长度,50.");

 RuleFor(tb_SysPara =>tb_SysPara.ShortName).MaximumLength(25).WithMessage("简称:不能超过最大长度,25.");

 RuleFor(tb_SysPara =>tb_SysPara.LegalPersonName).MaximumLength(25).WithMessage("法人姓名:不能超过最大长度,25.");

 RuleFor(tb_SysPara =>tb_SysPara.UnifiedSocialCreditIdentifier).MaximumLength(25).WithMessage("公司执照代码:不能超过最大长度,25.");

 RuleFor(tb_SysPara =>tb_SysPara.Contact).MaximumLength(50).WithMessage("联系人:不能超过最大长度,50.");

 RuleFor(tb_SysPara =>tb_SysPara.Phone).MaximumLength(50).WithMessage("电话:不能超过最大长度,50.");

 RuleFor(tb_SysPara =>tb_SysPara.Address).MaximumLength(127).WithMessage("地址:不能超过最大长度,127.");

 RuleFor(tb_SysPara =>tb_SysPara.ENAddress).MaximumLength(127).WithMessage("英文地址:不能超过最大长度,127.");

 RuleFor(tb_SysPara =>tb_SysPara.Website).MaximumLength(127).WithMessage("网址:不能超过最大长度,127.");

 RuleFor(tb_SysPara =>tb_SysPara.Email).MaximumLength(50).WithMessage("电子邮件:不能超过最大长度,50.");


 RuleFor(tb_SysPara =>tb_SysPara.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_SysPara =>tb_SysPara.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_SysPara =>tb_SysPara.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");

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

