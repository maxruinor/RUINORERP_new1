
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:13
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 销售订单明细验证类
    /// </summary>
    /*public partial class tb_SaleOrderDetailValidator:AbstractValidator<tb_SaleOrderDetail>*/
    public partial class tb_SaleOrderDetailValidator:BaseValidatorGeneric<tb_SaleOrderDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_SaleOrderDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.SOrder_ID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.Discount).PrecisionScale(8,4,true).WithMessage("折扣:小数位不能超过4。");

 RuleFor(x => x.TransactionPrice).PrecisionScale(19,4,true).WithMessage("成交价:小数位不能超过4。");

 RuleFor(x => x.SubtotalTransAmount).PrecisionScale(19,4,true).WithMessage("成交小计:小数位不能超过4。");

 RuleFor(x => x.CustomizedCost).PrecisionScale(19,4,true).WithMessage("定制成本:小数位不能超过4。");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.TotalDeliveredQty).NotNull().WithMessage("订单出库数:不能为空。");

 RuleFor(x => x.UnitCommissionAmount).PrecisionScale(19,4,true).WithMessage("单品佣金:小数位不能超过4。");

 RuleFor(x => x.CommissionAmount).PrecisionScale(19,4,true).WithMessage("佣金小计:小数位不能超过4。");

 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");

 RuleFor(x => x.SubtotalTaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");


 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");

 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.CustomerPartNo).MaximumMixedLength(100).WithMessage("客户型号:不能超过最大长度,100.");


//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.TotalReturnedQty).NotNull().WithMessage("订单退回数:不能为空。");

 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.SaleFlagCode).MaximumMixedLength(200).WithMessage("标识代码:不能超过最大长度,200.");

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

