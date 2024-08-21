
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/17/2024 19:26:51
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
    /// 库存视图验证类
    /// </summary>
    public partial class View_InventoryValidator:AbstractValidator<View_Inventory>
    {
     public View_InventoryValidator() 
     {
      RuleFor(View_Inventory =>View_Inventory.ProductNo).MaximumLength(40).WithMessage("品号:不能超过最大长度,40.");
 RuleFor(View_Inventory =>View_Inventory.CNName).MaximumLength(255).WithMessage("品名:不能超过最大长度,255.");
 RuleFor(View_Inventory =>View_Inventory.prop).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
 RuleFor(View_Inventory =>View_Inventory.Quantity).NotEmpty().When(x => x.Quantity.HasValue);
 RuleFor(View_Inventory =>View_Inventory.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
 RuleFor(View_Inventory =>View_Inventory.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
 RuleFor(View_Inventory =>View_Inventory.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
 RuleFor(View_Inventory =>View_Inventory.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(View_Inventory =>View_Inventory.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(View_Inventory =>View_Inventory.SourceType).NotEmpty().When(x => x.SourceType.HasValue);
 RuleFor(View_Inventory =>View_Inventory.Brand).MaximumLength(50).WithMessage("品牌:不能超过最大长度,50.");
 RuleFor(View_Inventory =>View_Inventory.SKU).MaximumLength(80).WithMessage("SKU码:不能超过最大长度,80.");
 RuleFor(View_Inventory =>View_Inventory.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(View_Inventory =>View_Inventory.Alert_Quantity).NotEmpty().When(x => x.Alert_Quantity.HasValue);
 RuleFor(x => x.Inv_Cost).PrecisionScale(,,true).WithMessage("货品成本:小数位不能超过。");
 RuleFor(View_Inventory =>View_Inventory.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
       	
           	
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

