
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:32
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
    /// 意向客户，公海客户 CRM系统中使用，给成交客户作外键引用验证类
    /// </summary>
    /*public partial class tb_CustomerValidator:AbstractValidator<tb_Customer>*/
    public partial class tb_CustomerValidator:BaseValidatorGeneric<tb_Customer>
    {
     public tb_CustomerValidator() 
     {
      RuleFor(tb_Customer =>tb_Customer.CompanyName).MaximumLength(25).WithMessage("公司名称:不能超过最大长度,25.");
 RuleFor(tb_Customer =>tb_Customer.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("对接人:下拉选择值不正确。");
 RuleFor(tb_Customer =>tb_Customer.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_Customer =>tb_Customer.CustomerName).MaximumLength(25).WithMessage("客户名称:不能超过最大长度,25.");
 RuleFor(tb_Customer =>tb_Customer.CustomerAddress).MaximumLength(150).WithMessage("客户地址:不能超过最大长度,150.");
 RuleFor(tb_Customer =>tb_Customer.CustomerDesc).MaximumLength(500).WithMessage("客户描述:不能超过最大长度,500.");
       	
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

