
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:07
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
    public partial class tb_SaleOrderDetailValidator:AbstractValidator<tb_SaleOrderDetail>
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
 RuleFor(x => x.Discount).PrecisionScale(5,3,true).WithMessage("折扣:小数位不能超过3。");
 RuleFor(x => x.TransactionPrice).PrecisionScale(19,4,true).WithMessage("成交价:小数位不能超过4。");
 RuleFor(x => x.SubtotalTransactionAmount).PrecisionScale(19,4,true).WithMessage("成交小计:小数位不能超过4。");
 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");
 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");
//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.DeliveredQty).NotNull().WithMessage("已交数量:不能为空。");
 RuleFor(x => x.CommissionAmont).PrecisionScale(19,4,true).WithMessage("抽成金额:小数位不能超过4。");
 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");
 RuleFor(x => x.TaxSubtotalAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");
 RuleFor(x => x.UntaxedAmont).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.Summary).MaximumLength(1000).WithMessage("摘要:不能超过最大长度,1000.");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.CustomerPartNo).MaximumLength(100).WithMessage("客户料号:不能超过最大长度,100.");
//***** 
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.ReturnedQty).NotNull().WithMessage("已退数量:不能为空。");
 RuleFor(tb_SaleOrderDetail =>tb_SaleOrderDetail.property).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
       	
           	
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

