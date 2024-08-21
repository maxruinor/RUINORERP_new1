
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/13/2023 17:33:44
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
    /// 客户交互表，CRM系统中使用      验证类
    /// </summary>
    public partial class tb_Customer_interactionsValidator:AbstractValidator<tb_Customer_interactions>
    {
     public tb_Customer_interactionsValidator() 
     {
      RuleFor(tb_Customer_interactions =>tb_Customer_interactions.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_Customer_interactions =>tb_Customer_interactions.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
 RuleFor(tb_Customer_interactions =>tb_Customer_interactions.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_Customer_interactions =>tb_Customer_interactions.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_Customer_interactions =>tb_Customer_interactions.interaction_date).MaximumLength(50).WithMessage("交互日期:不能超过最大长度,50.");
 RuleFor(tb_Customer_interactions =>tb_Customer_interactions.interaction_type).MaximumLength(100).WithMessage("交互类型:不能超过最大长度,100.");
       	
           	
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
            if (ForeignKeyID == 0 || ForeignKeyID == -1)
            {
                return false;
            }
            return rs;
        }
        
    }
}

