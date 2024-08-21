
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:23
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
    /// 系统参数验证类
    /// </summary>
    public partial class tb_SysParaValidator:AbstractValidator<tb_SysPara>
    {
     public tb_SysParaValidator() 
     {
      RuleFor(tb_SysPara =>tb_SysPara.CompanyCode).MaximumLength(10).WithMessage("公司代号:不能超过最大长度,10.");
 RuleFor(tb_SysPara =>tb_SysPara.CNName).MaximumLength(100).WithMessage("名称:不能超过最大长度,100.");
 RuleFor(tb_SysPara =>tb_SysPara.ENName).MaximumLength(100).WithMessage("英语名称:不能超过最大长度,100.");
 RuleFor(tb_SysPara =>tb_SysPara.ShortName).MaximumLength(50).WithMessage("简称:不能超过最大长度,50.");
 RuleFor(tb_SysPara =>tb_SysPara.LegalPersonName).MaximumLength(50).WithMessage("法人姓名:不能超过最大长度,50.");
 RuleFor(tb_SysPara =>tb_SysPara.UnifiedSocialCreditIdentifier).MaximumLength(50).WithMessage("公司执照代码:不能超过最大长度,50.");
 RuleFor(tb_SysPara =>tb_SysPara.Contact).MaximumLength(100).WithMessage("联系人:不能超过最大长度,100.");
 RuleFor(tb_SysPara =>tb_SysPara.Phone).MaximumLength(100).WithMessage("电话:不能超过最大长度,100.");
 RuleFor(tb_SysPara =>tb_SysPara.Address).MaximumLength(255).WithMessage("地址:不能超过最大长度,255.");
 RuleFor(tb_SysPara =>tb_SysPara.ENAddress).MaximumLength(255).WithMessage("英文地址:不能超过最大长度,255.");
 RuleFor(tb_SysPara =>tb_SysPara.Website).MaximumLength(255).WithMessage("网址:不能超过最大长度,255.");
 RuleFor(tb_SysPara =>tb_SysPara.Email).MaximumLength(100).WithMessage("电子邮件:不能超过最大长度,100.");
 RuleFor(tb_SysPara =>tb_SysPara.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_SysPara =>tb_SysPara.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_SysPara =>tb_SysPara.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
       	
           	
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

