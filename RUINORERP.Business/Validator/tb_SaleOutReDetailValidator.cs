
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:23
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
    /// 销售出库退回明细验证类
    /// </summary>
    /*public partial class tb_SaleOutReDetailValidator:AbstractValidator<tb_SaleOutReDetail>*/
    public partial class tb_SaleOutReDetailValidator:BaseValidatorGeneric<tb_SaleOutReDetail>
    {
     

     public tb_SaleOutReDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

//***** 
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.SaleOutRe_ID).NotNull().WithMessage("销售退回单:不能为空。");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Quantity).NotNull().WithMessage("退回数量:不能为空。");

 RuleFor(x => x.TransactionPrice).PrecisionScale(19,4,true).WithMessage("实际退款单价:小数位不能超过4。");

 RuleFor(x => x.SubtotalTransAmount).PrecisionScale(19,4,true).WithMessage("小计:小数位不能超过4。");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.CustomerPartNo).MaximumMixedLength(50).WithMessage("客户型号:不能超过最大长度,50.");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.CustomizedCost).PrecisionScale(19,4,true).WithMessage("定制成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(x => x.SubtotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");


 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.SubtotalTaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.UnitCommissionAmount).PrecisionScale(19,4,true).WithMessage("单品返还佣金:小数位不能超过4。");

 RuleFor(x => x.CommissionAmount).PrecisionScale(19,4,true).WithMessage("返还佣金小计:小数位不能超过4。");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.SaleFlagCode).MaximumMixedLength(200).WithMessage("标识代码:不能超过最大长度,200.");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.SaleOutDetail_ID).NotEmpty().When(x => x.SaleOutDetail_ID.HasValue);

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

