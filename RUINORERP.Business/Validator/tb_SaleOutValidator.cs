
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:22
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
    /// 销售出库单验证类
    /// </summary>
    /*public partial class tb_SaleOutValidator:AbstractValidator<tb_SaleOut>*/
    public partial class tb_SaleOutValidator:BaseValidatorGeneric<tb_SaleOut>
    {
     

     public tb_SaleOutValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_SaleOut =>tb_SaleOut.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_SaleOut =>tb_SaleOut.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_SaleOut =>tb_SaleOut.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("客户:下拉选择值不正确。");

 RuleFor(tb_SaleOut =>tb_SaleOut.SOrder_ID).Must(CheckForeignKeyValueCanNull).WithMessage("销售订单:下拉选择值不正确。");
 RuleFor(tb_SaleOut =>tb_SaleOut.SOrder_ID).NotEmpty().When(x => x.SOrder_ID.HasValue);

 RuleFor(tb_SaleOut =>tb_SaleOut.SaleOrderNo).MaximumMixedLength(50).WithMessage("销售订单编号:不能超过最大长度,50.");

 RuleFor(tb_SaleOut =>tb_SaleOut.SaleOutNo).MaximumMixedLength(50).WithMessage("出库单号:不能超过最大长度,50.");
 RuleFor(tb_SaleOut =>tb_SaleOut.SaleOutNo).NotEmpty().WithMessage("出库单号:不能为空。");

 RuleFor(tb_SaleOut =>tb_SaleOut.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_SaleOut =>tb_SaleOut.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_SaleOut =>tb_SaleOut.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);

//***** 
 RuleFor(tb_SaleOut =>tb_SaleOut.Currency_ID).NotNull().WithMessage("币别:不能为空。");

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(tb_SaleOut =>tb_SaleOut.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款类型:下拉选择值不正确。");
 RuleFor(tb_SaleOut =>tb_SaleOut.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_SaleOut =>tb_SaleOut.RefundStatus).NotEmpty().When(x => x.RefundStatus.HasValue);

 RuleFor(x => x.ForeignFreightIncome).PrecisionScale(19,4,true).WithMessage("运费收入外币:小数位不能超过4。");

 RuleFor(x => x.FreightIncome).PrecisionScale(19,4,true).WithMessage("运费收入:小数位不能超过4。");

//***** 
 RuleFor(tb_SaleOut =>tb_SaleOut.TotalQty).NotNull().WithMessage("总数量:不能为空。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");




 RuleFor(tb_SaleOut =>tb_SaleOut.ShippingAddress).MaximumMixedLength(500).WithMessage("收货地址:不能超过最大长度,500.");

 RuleFor(tb_SaleOut =>tb_SaleOut.ShippingWay).MaximumMixedLength(50).WithMessage("发货方式:不能超过最大长度,50.");

 RuleFor(tb_SaleOut =>tb_SaleOut.PlatformOrderNo).MaximumMixedLength(100).WithMessage("平台单号:不能超过最大长度,100.");



 RuleFor(tb_SaleOut =>tb_SaleOut.TrackNo).MaximumMixedLength(50).WithMessage("物流单号:不能超过最大长度,50.");

 RuleFor(tb_SaleOut =>tb_SaleOut.CustomerPONo).MaximumMixedLength(50).WithMessage("客户订单号:不能超过最大长度,50.");

 RuleFor(x => x.ForeignTotalAmount).PrecisionScale(19,4,true).WithMessage("金额外币:小数位不能超过4。");

 RuleFor(x => x.CollectedMoney).PrecisionScale(19,4,true).WithMessage("实收金额:小数位不能超过4。");



 RuleFor(tb_SaleOut =>tb_SaleOut.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_SaleOut =>tb_SaleOut.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_SaleOut =>tb_SaleOut.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

 RuleFor(tb_SaleOut =>tb_SaleOut.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");

 RuleFor(tb_SaleOut =>tb_SaleOut.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_SaleOut =>tb_SaleOut.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);

 RuleFor(x => x.ForeignDeposit).PrecisionScale(19,4,true).WithMessage("订金外币:小数位不能超过4。");

 RuleFor(x => x.Deposit).PrecisionScale(19,4,true).WithMessage("订金:小数位不能超过4。");

 RuleFor(x => x.FreightCost).PrecisionScale(19,4,true).WithMessage("运费成本:小数位不能超过4。");

 RuleFor(tb_SaleOut =>tb_SaleOut.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);

//***** 
 RuleFor(tb_SaleOut =>tb_SaleOut.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

//***** 
 RuleFor(tb_SaleOut =>tb_SaleOut.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(x => x.TotalCommissionAmount).PrecisionScale(19,4,true).WithMessage("佣金金额:小数位不能超过4。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");

 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");

 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("总税额:小数位不能超过4。");

 RuleFor(x => x.TotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");


 RuleFor(x => x.DiscountAmount).PrecisionScale(19,4,true).WithMessage("优惠金额:小数位不能超过4。");

 RuleFor(x => x.PrePayMoney).PrecisionScale(19,4,true).WithMessage("预收款:小数位不能超过4。");


           	                //long
                //SaleOut_MainID
                //tb_SaleOutDetail
                //RuleFor(x => x.tb_SaleOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_SaleOutDetails).NotNull();
                //RuleForEach(x => x.tb_SaleOutDetails).NotNull();
                //RuleFor(x => x.tb_SaleOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
     }




        private bool DetailedRecordsNotEmpty(List<tb_SaleOutDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

