
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:29
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
    /// 销售订单明细验证类
    /// </summary>
    /*public partial class tb_SaleOrderDetailValidator:AbstractValidator<tb_SaleOrderDetail>*/
    public partial class tb_SaleOrderDetailValidator:BaseValidatorGeneric<tb_SaleOrderDetail>
    {
     public tb_SaleOrderDetailValidator() 
     {
     //***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.SOrder_ID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");
//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.Discount).PrecisionScale(8,4,true).WithMessage("折扣:小数位不能超过4。");
 RuleFor(x => x.TransactionPrice).PrecisionScale(19,4,true).WithMessage("成交价:小数位不能超过4。");
 RuleFor(x => x.SubtotalTransAmount).PrecisionScale(19,4,true).WithMessage("成交小计:小数位不能超过4。");
 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");
 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");
//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.TotalDeliveredQty).NotNull().WithMessage("订单出库数:不能为空。");
 RuleFor(x => x.CommissionAmount).PrecisionScale(19,4,true).WithMessage("抽成金额:小数位不能超过4。");
 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");
 RuleFor(x => x.SubtotalTaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");
 RuleFor(x => x.SubtotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.CustomerPartNo).MaximumLength(50).WithMessage("客户型号:不能超过最大长度,50.");
//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.TotalReturnedQty).NotNull().WithMessage("订单退回数:不能为空。");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
       	
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

