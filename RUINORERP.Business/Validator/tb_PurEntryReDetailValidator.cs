
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:20
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 采购入库退回单验证类
    /// </summary>
    /*public partial class tb_PurEntryReDetailValidator:AbstractValidator<tb_PurEntryReDetail>*/
    public partial class tb_PurEntryReDetailValidator:BaseValidatorGeneric<tb_PurEntryReDetail>
    {
     

     public tb_PurEntryReDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.PurEntryRe_ID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.Quantity).NotNull().WithMessage("退货数量:不能为空。");

//***** 
 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.DeliveredQuantity).NotNull().WithMessage("交回数量:不能为空。");

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(x => x.CustomizedCost).PrecisionScale(19,4,true).WithMessage("定制成本:小数位不能超过4。");
 

 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 //RuleFor(x => x.TransactionPrice).PrecisionScale(19,4,true).WithMessage("成交单价:小数位不能超过4。");

 RuleFor(x => x.SubtotalTrPriceAmount).PrecisionScale(19,4,true).WithMessage("小计:小数位不能超过4。");

 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.VendorModelCode).MaximumMixedLength(50).WithMessage("厂商型号:不能超过最大长度,50.");

 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.CustomertModel).MaximumMixedLength(50).WithMessage("客户型号:不能超过最大长度,50.");

 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");
 

 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_PurEntryReDetail =>tb_PurEntryReDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

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

