
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:40
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
    /// 领料统计验证类
    /// </summary>
    /*public partial class View_MaterialRequisitionItemsValidator:AbstractValidator<View_MaterialRequisitionItems>*/
    public partial class View_MaterialRequisitionItemsValidator:BaseValidatorGeneric<View_MaterialRequisitionItems>
    {
     public View_MaterialRequisitionItemsValidator() 
     {
      
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.MaterialRequisitionNO).MaximumLength(25).WithMessage("领料单号:不能超过最大长度,25.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.MONO).MaximumLength(50).WithMessage("制令单号:不能超过最大长度,50.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ShouldSendQty).NotEmpty().When(x => x.ShouldSendQty.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ActualSentQty).NotEmpty().When(x => x.ActualSentQty.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.CanQuantity).NotEmpty().When(x => x.CanQuantity.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.CustomerPartNo).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");
  RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ReturnQty).NotEmpty().When(x => x.ReturnQty.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ProdBaseID).NotEmpty().When(x => x.ProdBaseID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.ShortCode).MaximumLength(25).WithMessage("短码:不能超过最大长度,25.");
 RuleFor(View_MaterialRequisitionItems =>View_MaterialRequisitionItems.BarCode).MaximumLength(25).WithMessage("条码:不能超过最大长度,25.");
       	
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

