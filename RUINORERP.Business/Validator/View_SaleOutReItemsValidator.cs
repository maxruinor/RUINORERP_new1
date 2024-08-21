
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2024 11:43:14
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
    /// 销售退货统计分析验证类
    /// </summary>
    public partial class View_SaleOutReItemsValidator:AbstractValidator<View_SaleOutReItems>
    {
     public View_SaleOutReItemsValidator() 
     {
      RuleFor(View_SaleOutReItems =>View_SaleOutReItems.SKU).MaximumLength(80).WithMessage("SKU码:不能超过最大长度,80.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.ReturnNo).MaximumLength(50).WithMessage("退回单号:不能超过最大长度,50.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.SaleOut_NO).MaximumLength(50).WithMessage("销售出库单号:不能超过最大长度,50.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.property).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Quantity).NotEmpty().When(x => x.Quantity.HasValue);
 
 
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Summary).MaximumLength(255).WithMessage("摘要:不能超过最大长度,255.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.CustomerPartNo).MaximumLength(50).WithMessage("客户型号:不能超过最大长度,50.");
 
 
 
 RuleFor(x => x.TaxRate).PrecisionScale(0,8,true).WithMessage("税率:小数位不能超过8。");
 
 
 
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.CNName).MaximumLength(255).WithMessage("品名:不能超过最大长度,255.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Specifications).MaximumLength(1000).WithMessage("规格:不能超过最大长度,1000.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.ProductNo).MaximumLength(40).WithMessage("品号:不能超过最大长度,40.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Model).MaximumLength(50).WithMessage("型号:不能超过最大长度,50.");
 RuleFor(View_SaleOutReItems =>View_SaleOutReItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
       	
           	
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

