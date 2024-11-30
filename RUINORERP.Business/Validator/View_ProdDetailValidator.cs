
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/14/2024 18:30:43
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
    /// 产品详情视图验证类
    /// </summary>
    public partial class View_ProdDetailValidator:AbstractValidator<View_ProdDetail>
    {
     public View_ProdDetailValidator() 
     {
      RuleFor(View_ProdDetail =>View_ProdDetail.SKU).MaximumLength(40).WithMessage(":不能超过最大长度,40.");
//***** 
 RuleFor(View_ProdDetail =>View_ProdDetail.ProdDetailID).NotNull().WithMessage(":不能为空。");
 RuleFor(View_ProdDetail =>View_ProdDetail.CNName).MaximumLength(127).WithMessage(":不能超过最大长度,127.");
 RuleFor(View_ProdDetail =>View_ProdDetail.CNName).NotEmpty().WithMessage(":不能为空。");
 RuleFor(View_ProdDetail =>View_ProdDetail.Specifications).MaximumLength(500).WithMessage(":不能超过最大长度,500.");
 RuleFor(View_ProdDetail =>View_ProdDetail.Quantity).NotEmpty().When(x => x.Quantity.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.prop).MaximumLength(-1).WithMessage(":不能超过最大长度,-1.");
 RuleFor(View_ProdDetail =>View_ProdDetail.ProductNo).MaximumLength(20).WithMessage(":不能超过最大长度,20.");
 RuleFor(View_ProdDetail =>View_ProdDetail.ProductNo).NotEmpty().WithMessage(":不能为空。");
 RuleFor(View_ProdDetail =>View_ProdDetail.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.Model).MaximumLength(25).WithMessage(":不能超过最大长度,25.");
 RuleFor(View_ProdDetail =>View_ProdDetail.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.ENName).MaximumLength(127).WithMessage(":不能超过最大长度,127.");
 RuleFor(View_ProdDetail =>View_ProdDetail.Brand).MaximumLength(25).WithMessage(":不能超过最大长度,25.");
 RuleFor(View_ProdDetail =>View_ProdDetail.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.On_the_way_Qty).NotEmpty().When(x => x.On_the_way_Qty.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.Sale_Qty).NotEmpty().When(x => x.Sale_Qty.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.Alert_Quantity).NotEmpty().When(x => x.Alert_Quantity.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.MakingQty).NotEmpty().When(x => x.MakingQty.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.NotOutQty).NotEmpty().When(x => x.NotOutQty.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.Notes).MaximumLength(127).WithMessage(":不能超过最大长度,127.");
//***** 
 RuleFor(View_ProdDetail =>View_ProdDetail.Type_ID).NotNull().WithMessage(":不能为空。");
 RuleFor(View_ProdDetail =>View_ProdDetail.ShortCode).MaximumLength(25).WithMessage(":不能超过最大长度,25.");
 RuleFor(View_ProdDetail =>View_ProdDetail.SourceType).NotEmpty().When(x => x.SourceType.HasValue);
 RuleFor(View_ProdDetail =>View_ProdDetail.BarCode).MaximumLength(25).WithMessage(":不能超过最大长度,25.");
 RuleFor(x => x.Inv_Cost).PrecisionScale(19,6,true).WithMessage(":小数位不能超过6。");
 RuleFor(x => x.Standard_Price).PrecisionScale(19,6,true).WithMessage(":小数位不能超过6。");
 RuleFor(x => x.Discount_price).PrecisionScale(19,6,true).WithMessage(":小数位不能超过6。");
 RuleFor(x => x.Market_price).PrecisionScale(19,6,true).WithMessage(":小数位不能超过6。");
 RuleFor(x => x.Wholesale_Price).PrecisionScale(19,6,true).WithMessage(":小数位不能超过6。");
 RuleFor(x => x.Transfer_price).PrecisionScale(19,6,true).WithMessage(":小数位不能超过6。");
 RuleFor(x => x.Weight).PrecisionScale(10,3,true).WithMessage(":小数位不能超过3。");
 RuleFor(View_ProdDetail =>View_ProdDetail.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);
       	
           	
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

