
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:12
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
    /// 价格调整单明细验证类
    /// </summary>
    /*public partial class tb_FM_PriceAdjustmentDetailValidator:AbstractValidator<tb_FM_PriceAdjustmentDetail>*/
    public partial class tb_FM_PriceAdjustmentDetailValidator:BaseValidatorGeneric<tb_FM_PriceAdjustmentDetail>
    {
     

     public tb_FM_PriceAdjustmentDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.AdjustId).NotEmpty().When(x => x.AdjustId.HasValue);

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("产品:下拉选择值不正确。");
 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.Specifications).MaximumMixedLength(1000).WithMessage("规格:不能超过最大长度,1000.");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.Unit_ID).Must(CheckForeignKeyValueCanNull).WithMessage("单位:下拉选择值不正确。");
 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);

 RuleFor(x => x.Original_UnitPrice_NoTax).PrecisionScale(18,4,true).WithMessage("原未税单价:小数位不能超过4。");

 RuleFor(x => x.Correct_UnitPrice_NoTax).PrecisionScale(18,4,true).WithMessage("新未税单价:小数位不能超过4。");

 RuleFor(x => x.Original_TaxRate).PrecisionScale(5,2,true).WithMessage("原税率:小数位不能超过2。");

 RuleFor(x => x.Correct_TaxRate).PrecisionScale(5,2,true).WithMessage("新税率:小数位不能超过2。");

 RuleFor(x => x.Original_UnitPrice_WithTax).PrecisionScale(18,4,true).WithMessage("原含税单价:小数位不能超过4。");

 RuleFor(x => x.Correct_UnitPrice_WithTax).PrecisionScale(18,4,true).WithMessage("新含税单价:小数位不能超过4。");

 RuleFor(x => x.UnitPrice_NoTax_Diff).PrecisionScale(18,4,true).WithMessage("未税单价差异:小数位不能超过4。");

 RuleFor(x => x.UnitPrice_WithTax_Diff).PrecisionScale(18,4,true).WithMessage("含税单价差异:小数位不能超过4。");

 RuleFor(x => x.Quantity).PrecisionScale(10,4,true).WithMessage("数量:小数位不能超过4。");

 RuleFor(x => x.Original_TaxAmount).PrecisionScale(18,4,true).WithMessage("原始税额:小数位不能超过4。");

 RuleFor(x => x.Correct_TaxAmount).PrecisionScale(18,4,true).WithMessage("新调税额:小数位不能超过4。");

 RuleFor(x => x.TaxAmount_Diff).PrecisionScale(18,4,true).WithMessage("税额差异:小数位不能超过4。");

 RuleFor(x => x.TotalAmount_Diff_NoTax).PrecisionScale(18,4,true).WithMessage("总未税差异金额:小数位不能超过4。");

 RuleFor(x => x.TotalAmount_Diff_WithTax).PrecisionScale(18,4,true).WithMessage("总含税差异金额价:小数位不能超过4。");

 RuleFor(x => x.TotalAmount_Diff).PrecisionScale(18,4,true).WithMessage("总差异金额:小数位不能超过4。");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.CustomerPartNo).MaximumMixedLength(100).WithMessage("往来单位料号:不能超过最大长度,100.");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.AdjustReason).MaximumMixedLength(500).WithMessage("调整原因:不能超过最大长度,500.");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.Summary).MaximumMixedLength(300).WithMessage("摘要:不能超过最大长度,300.");

           	  
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

