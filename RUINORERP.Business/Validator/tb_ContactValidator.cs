
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:30
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
    /// 联系人表，CRM系统中使用验证类
    /// </summary>
    /*public partial class tb_ContactValidator:AbstractValidator<tb_Contact>*/
    public partial class tb_ContactValidator:BaseValidatorGeneric<tb_Contact>
    {
     public tb_ContactValidator() 
     {
      RuleFor(tb_Contact =>tb_Contact.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage("意向客户:下拉选择值不正确。");
 RuleFor(tb_Contact =>tb_Contact.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
 RuleFor(tb_Contact =>tb_Contact.Contact_Name).MaximumLength(25).WithMessage("名称:不能超过最大长度,25.");
 RuleFor(tb_Contact =>tb_Contact.Contact_Email).MaximumLength(50).WithMessage("描述:不能超过最大长度,50.");
 RuleFor(tb_Contact =>tb_Contact.Contact_Phone).MaximumLength(15).WithMessage("电话:不能超过最大长度,15.");
 RuleFor(tb_Contact =>tb_Contact.Preferences).MaximumLength(50).WithMessage("爱好:不能超过最大长度,50.");
       	
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

