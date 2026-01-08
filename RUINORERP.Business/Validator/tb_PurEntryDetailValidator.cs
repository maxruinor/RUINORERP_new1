
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
    /// 采购入库单验证类
    /// </summary>
    /*public partial class tb_PurEntryDetailValidator:AbstractValidator<tb_PurEntryDetail>*/
    public partial class tb_PurEntryDetailValidator:BaseValidatorGeneric<tb_PurEntryDetail>
    {
     

     public tb_PurEntryDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");

//***** 
 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.PurEntryID).NotNull().WithMessage("采购入库单:不能为空。");

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(x => x.CustomizedCost).PrecisionScale(19,4,true).WithMessage("定制成本:小数位不能超过4。");

 RuleFor(x => x.UntaxedCustomizedCost).PrecisionScale(19,4,true).WithMessage("未税定制成本:小数位不能超过4。");



 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.UntaxedUnitPrice).PrecisionScale(19,4,true).WithMessage("未税单价:小数位不能超过4。");


 RuleFor(x => x.SubtotalAmount).PrecisionScale(19,4,true).WithMessage("成交小计:小数位不能超过4。");

 RuleFor(x => x.SubtotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税金额小计:小数位不能超过4。");

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.VendorModelCode).MaximumMixedLength(50).WithMessage("厂商型号:不能超过最大长度,50.");

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.CustomertModel).MaximumMixedLength(50).WithMessage("客户型号:不能超过最大长度,50.");

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");


//***** 
 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.ReturnedQty).NotNull().WithMessage("退回数:不能为空。");

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.PurOrder_ChildID).NotEmpty().When(x => x.PurOrder_ChildID.HasValue);

 RuleFor(x => x.AllocatedFreightCost).PrecisionScale(19,4,true).WithMessage("运费成本分摊:小数位不能超过4。");

 RuleFor(tb_PurEntryDetail =>tb_PurEntryDetail.FreightAllocationRules).NotEmpty().When(x => x.FreightAllocationRules.HasValue);

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

