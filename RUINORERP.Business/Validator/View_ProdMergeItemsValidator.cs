
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 15:29:24
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
    /// 组合明细统计-只管明细验证类
    /// </summary>
    /*public partial class View_ProdMergeItemsValidator:AbstractValidator<View_ProdMergeItems>*/
    public partial class View_ProdMergeItemsValidator:BaseValidatorGeneric<View_ProdMergeItems>
    {
     public View_ProdMergeItemsValidator() 
     {
      RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.MergeNo).MaximumLength(25).WithMessage("组合单号:不能超过最大长度,25.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.BOM_No).MaximumLength(25).WithMessage("配方编号:不能超过最大长度,25.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Qty).NotEmpty().When(x => x.Qty.HasValue);
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_ProdMergeItems =>View_ProdMergeItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
       	
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

