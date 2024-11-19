
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:41
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
    /// 退料统计验证类
    /// </summary>
    /*public partial class View_MaterialReturnItemsValidator:AbstractValidator<View_MaterialReturnItems>*/
    public partial class View_MaterialReturnItemsValidator:BaseValidatorGeneric<View_MaterialReturnItems>
    {
     public View_MaterialReturnItemsValidator() 
     {
      RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.BillNo).MaximumLength(25).WithMessage("退料单号:不能超过最大长度,25.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.BillType).NotEmpty().When(x => x.BillType.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.MR_ID).NotEmpty().When(x => x.MR_ID.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.MaterialRequisitionNO).MaximumLength(25).WithMessage("领料单号:不能超过最大长度,25.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Quantity).NotEmpty().When(x => x.Quantity.HasValue);
 
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_MaterialReturnItems =>View_MaterialReturnItems.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
       	
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

