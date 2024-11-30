
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:23
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
    /// 采购订单明细表验证类
    /// </summary>
    /*public partial class tb_PurOrderDetailValidator:AbstractValidator<tb_PurOrderDetail>*/
    public partial class tb_PurOrderDetailValidator:BaseValidatorGeneric<tb_PurOrderDetail>
    {
     public tb_PurOrderDetailValidator() 
     {
      RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");
//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.PurOrder_ID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.UnitPrice).PrecisionScale(19,6,true).WithMessage("单价:小数位不能超过6。");
 RuleFor(x => x.Discount).PrecisionScale(5,3,true).WithMessage("折扣:小数位不能超过3。");
 RuleFor(x => x.TransactionPrice).PrecisionScale(19,6,true).WithMessage("成交单价:小数位不能超过6。");
 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");
 RuleFor(x => x.TaxAmount).PrecisionScale(19,6,true).WithMessage("税额:小数位不能超过6。");
 RuleFor(x => x.SubtotalAmount).PrecisionScale(19,6,true).WithMessage("成交金额:小数位不能超过6。");
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.CustomertModel).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");
//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.DeliveredQuantity).NotNull().WithMessage("已交数:不能为空。");
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.Notes).MaximumLength(500).WithMessage("备注:不能超过最大长度,500.");
//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.TotalReturnedQty).NotNull().WithMessage("退回数:不能为空。");
       	
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

