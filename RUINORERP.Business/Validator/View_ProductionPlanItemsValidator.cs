
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:42
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
    /// 计划单明细统计验证类
    /// </summary>
    /*public partial class View_ProductionPlanItemsValidator:AbstractValidator<View_ProductionPlanItems>*/
    public partial class View_ProductionPlanItemsValidator:BaseValidatorGeneric<View_ProductionPlanItems>
    {
     public View_ProductionPlanItemsValidator() 
     {
      
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.SaleOrderNo).MaximumLength(25).WithMessage("销售单号:不能超过最大长度,25.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.PPNo).MaximumLength(50).WithMessage("计划单号:不能超过最大长度,50.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Priority).NotEmpty().When(x => x.Priority.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Quantity).NotEmpty().When(x => x.Quantity.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.BarCode).MaximumLength(25).WithMessage("条码:不能超过最大长度,25.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.ShortCode).MaximumLength(25).WithMessage("短码:不能超过最大长度,25.");
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.AnalyzedQuantity).NotEmpty().When(x => x.AnalyzedQuantity.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.CompletedQuantity).NotEmpty().When(x => x.CompletedQuantity.HasValue);
 RuleFor(View_ProductionPlanItems =>View_ProductionPlanItems.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");
       	
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

