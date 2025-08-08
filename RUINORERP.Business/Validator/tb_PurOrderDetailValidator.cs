
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:08
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
    /// 采购订单明细表验证类
    /// </summary>
    /*public partial class tb_PurOrderDetailValidator:AbstractValidator<tb_PurOrderDetail>*/
    public partial class tb_PurOrderDetailValidator:BaseValidatorGeneric<tb_PurOrderDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_PurOrderDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");

//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.PurOrder_ID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");



 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.CustomizedCost).PrecisionScale(19,4,true).WithMessage("定制成本:小数位不能超过4。");

 RuleFor(x => x.UntaxedCustomizedCost).PrecisionScale(19,4,true).WithMessage("未税定制成本:小数位不能超过4。");

 RuleFor(x => x.UntaxedUnitPrice).PrecisionScale(19,4,true).WithMessage("未税单价:小数位不能超过4。");

 RuleFor(x => x.SubtotalAmount).PrecisionScale(19,4,true).WithMessage("成交金额:小数位不能超过4。");

 RuleFor(x => x.SubtotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税金额小计:小数位不能超过4。");



 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.VendorModelCode).MaximumMixedLength(50).WithMessage("厂商型号:不能超过最大长度,50.");

 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.CustomertModel).MaximumMixedLength(50).WithMessage("客户型号:不能超过最大长度,50.");

//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.DeliveredQuantity).NotNull().WithMessage("已交数:不能为空。");


 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.Notes).MaximumMixedLength(1000).WithMessage("备注:不能超过最大长度,1000.");

//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.TotalReturnedQty).NotNull().WithMessage("退回数:不能为空。");

//***** 
 RuleFor(tb_PurOrderDetail =>tb_PurOrderDetail.UndeliveredQty).NotNull().WithMessage("未交数量:不能为空。");

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

