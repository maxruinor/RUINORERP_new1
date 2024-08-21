
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
    /// 客户交互表，CRM系统中使用      验证类
    /// </summary>
    public partial class tb_Customer_interactionValidator:AbstractValidator<tb_Customer_interaction>
    {
     public tb_Customer_interactionValidator() 
     {
      RuleFor(tb_Customer_interaction =>tb_Customer_interaction.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
 RuleFor(tb_Customer_interaction =>tb_Customer_interaction.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("对接人:下拉选择值不正确。");
 RuleFor(tb_Customer_interaction =>tb_Customer_interaction.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_Customer_interaction =>tb_Customer_interaction.interaction_date).MaximumLength(50).WithMessage("交互日期:不能超过最大长度,50.");
 RuleFor(tb_Customer_interaction =>tb_Customer_interaction.interaction_type).MaximumLength(100).WithMessage("交互类型:不能超过最大长度,100.");
       	
           	
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

