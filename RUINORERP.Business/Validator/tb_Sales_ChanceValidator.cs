
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:33
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
    /// 销售机会验证类
    /// </summary>
    /*public partial class tb_Sales_ChanceValidator:AbstractValidator<tb_Sales_Chance>*/
    public partial class tb_Sales_ChanceValidator:BaseValidatorGeneric<tb_Sales_Chance>
    {
     public tb_Sales_ChanceValidator() 
     {
      RuleFor(tb_Sales_Chance =>tb_Sales_Chance.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage("意向客户:下拉选择值不正确。");
 RuleFor(tb_Sales_Chance =>tb_Sales_Chance.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
 RuleFor(tb_Sales_Chance =>tb_Sales_Chance.opportunity_name).MaximumLength(25).WithMessage("机会名称:不能超过最大长度,25.");
 RuleFor(tb_Sales_Chance =>tb_Sales_Chance.opportunity_amount).MaximumLength(50).WithMessage("机会金额:不能超过最大长度,50.");
 RuleFor(tb_Sales_Chance =>tb_Sales_Chance.opportunity_stage).MaximumLength(100).WithMessage("机会阶段:不能超过最大长度,100.");
       	
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

