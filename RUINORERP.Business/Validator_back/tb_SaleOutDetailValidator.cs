
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:10
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
    /// 销售出库明细验证类
    /// </summary>
    public partial class tb_SaleOutDetailValidator:AbstractValidator<tb_SaleOutDetail>
    {
     public tb_SaleOutDetailValidator() 
     {
      RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
//***** 
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.SaleOut_MainID).NotNull().WithMessage(":不能为空。");
//***** 
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.TransactionPrice).PrecisionScale(19,4,true).WithMessage("成交单价:小数位不能超过4。");
 RuleFor(x => x.SubtotalAmount).PrecisionScale(19,4,true).WithMessage("成交小计:小数位不能超过4。");
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.Summary).MaximumLength(255).WithMessage("摘要:不能超过最大长度,255.");
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.CustomerPartNo).MaximumLength(50).WithMessage("客户型号:不能超过最大长度,50.");
 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");
 RuleFor(x => x.TotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");
 RuleFor(x => x.TaxRate).PrecisionScale(8,3,true).WithMessage("税率:小数位不能超过3。");
//***** 
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.OrderTotalOutQty).NotNull().WithMessage("订单累计数:不能为空。");
//***** 
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.OrderReturnTotalQty).NotNull().WithMessage("订单退回数:不能为空。");
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.property).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");
 RuleFor(x => x.Discount).PrecisionScale(5,3,true).WithMessage("折扣:小数位不能超过3。");
 RuleFor(x => x.UntaxedAmont).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");
 RuleFor(x => x.CommissionAmont).PrecisionScale(19,4,true).WithMessage("抽成金额:小数位不能超过4。");
//***** 
 RuleFor(tb_SaleOutDetail =>tb_SaleOutDetail.DeliveredQty).NotNull().WithMessage("抽成金已出库数量额:不能为空。");
 RuleFor(x => x.TaxSubtotalAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");
       	
           	
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

