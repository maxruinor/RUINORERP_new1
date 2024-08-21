
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:28
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
    /// 系统使用者公司验证类
    /// </summary>
    public partial class tb_CompanyValidator:AbstractValidator<tb_Company>
    {
     public tb_CompanyValidator() 
     {
      RuleFor(tb_Company =>tb_Company.CompanyCode).MaximumLength(10).WithMessage("公司代号:不能超过最大长度,10.");
 RuleFor(tb_Company =>tb_Company.CNName).MaximumLength(100).WithMessage("名称:不能超过最大长度,100.");
 RuleFor(tb_Company =>tb_Company.ENName).MaximumLength(100).WithMessage("英语名称:不能超过最大长度,100.");
 RuleFor(tb_Company =>tb_Company.ShortName).MaximumLength(50).WithMessage("简称:不能超过最大长度,50.");
 RuleFor(tb_Company =>tb_Company.LegalPersonName).MaximumLength(50).WithMessage("法人姓名:不能超过最大长度,50.");
 RuleFor(tb_Company =>tb_Company.UnifiedSocialCreditIdentifier).MaximumLength(50).WithMessage("公司执照代码:不能超过最大长度,50.");
 RuleFor(tb_Company =>tb_Company.Contact).MaximumLength(100).WithMessage("联系人:不能超过最大长度,100.");
 RuleFor(tb_Company =>tb_Company.Phone).MaximumLength(100).WithMessage("电话:不能超过最大长度,100.");
 RuleFor(tb_Company =>tb_Company.Address).MaximumLength(255).WithMessage("地址:不能超过最大长度,255.");
 RuleFor(tb_Company =>tb_Company.ENAddress).MaximumLength(255).WithMessage("英文地址:不能超过最大长度,255.");
 RuleFor(tb_Company =>tb_Company.Website).MaximumLength(255).WithMessage("网址:不能超过最大长度,255.");
 RuleFor(tb_Company =>tb_Company.Email).MaximumLength(100).WithMessage("电子邮件:不能超过最大长度,100.");
 RuleFor(tb_Company =>tb_Company.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Company =>tb_Company.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_Company =>tb_Company.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
       	
           	
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

