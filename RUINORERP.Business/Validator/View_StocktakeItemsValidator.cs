
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:27
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
    /// 盘点明细统计验证类
    /// </summary>
    /*public partial class View_StocktakeItemsValidator:AbstractValidator<View_StocktakeItems>*/
    public partial class View_StocktakeItemsValidator:BaseValidatorGeneric<View_StocktakeItems>
    {
     public View_StocktakeItemsValidator() 
     {
      RuleFor(View_StocktakeItems =>View_StocktakeItems.CheckNo).MaximumLength(25).WithMessage("盘点单号:不能超过最大长度,25.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.CheckMode).NotEmpty().When(x => x.CheckMode.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Adjust_Type).NotEmpty().When(x => x.Adjust_Type.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.CheckResult).NotEmpty().When(x => x.CheckResult.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_StocktakeItems =>View_StocktakeItems.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);

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

