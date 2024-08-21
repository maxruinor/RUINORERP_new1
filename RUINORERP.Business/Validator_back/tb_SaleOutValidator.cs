
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:09
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
    /// 销售出库单验证类
    /// </summary>
    public partial class tb_SaleOutValidator:AbstractValidator<tb_SaleOut>
    {
     public tb_SaleOutValidator() 
     {
      RuleFor(tb_SaleOut =>tb_SaleOut.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_SaleOut =>tb_SaleOut.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_SaleOut =>tb_SaleOut.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("客户:下拉选择值不正确。");
 RuleFor(tb_SaleOut =>tb_SaleOut.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_SaleOut =>tb_SaleOut.SOrder_ID).Must(CheckForeignKeyValueCanNull).WithMessage("引用订单:下拉选择值不正确。");
 RuleFor(tb_SaleOut =>tb_SaleOut.SOrder_ID).NotEmpty().When(x => x.SOrder_ID.HasValue);
 RuleFor(tb_SaleOut =>tb_SaleOut.SaleOutNo).MaximumLength(50).WithMessage("出库单号:不能超过最大长度,50.");
 RuleFor(tb_SaleOut =>tb_SaleOut.SaleOutNo).NotEmpty().WithMessage("出库单号:不能为空。");
 RuleFor(tb_SaleOut =>tb_SaleOut.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);
 RuleFor(tb_SaleOut =>tb_SaleOut.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);
 RuleFor(x => x.ShipCost).PrecisionScale(19,4,true).WithMessage("运费:小数位不能超过4。");
//***** 
 RuleFor(tb_SaleOut =>tb_SaleOut.TotalQty).NotNull().WithMessage("总数量:不能为空。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");
 RuleFor(tb_SaleOut =>tb_SaleOut.ShippingAddress).MaximumLength(255).WithMessage("发货地址:不能超过最大长度,255.");
 RuleFor(tb_SaleOut =>tb_SaleOut.ShippingWay).MaximumLength(50).WithMessage("发货方式:不能超过最大长度,50.");
 RuleFor(tb_SaleOut =>tb_SaleOut.TrackNo).MaximumLength(50).WithMessage("物流单号:不能超过最大长度,50.");
 RuleFor(x => x.CollectedMoney).PrecisionScale(19,4,true).WithMessage("实收金额:小数位不能超过4。");
 RuleFor(tb_SaleOut =>tb_SaleOut.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_SaleOut =>tb_SaleOut.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_SaleOut =>tb_SaleOut.Notes).MaximumLength(500).WithMessage("备注:不能超过最大长度,500.");
 RuleFor(tb_SaleOut =>tb_SaleOut.ApprovalOpinions).MaximumLength(200).WithMessage("审批意见:不能超过最大长度,200.");
 RuleFor(tb_SaleOut =>tb_SaleOut.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
 RuleFor(tb_SaleOut =>tb_SaleOut.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);
 RuleFor(x => x.Deposit).PrecisionScale(19,4,true).WithMessage("订金:小数位不能超过4。");
 RuleFor(tb_SaleOut =>tb_SaleOut.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);
//***** 
 RuleFor(tb_SaleOut =>tb_SaleOut.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
//***** 
 RuleFor(tb_SaleOut =>tb_SaleOut.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");
 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");
 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");
 RuleFor(x => x.UntaxedAmont).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");
 RuleFor(x => x.DiscountAmount).PrecisionScale(19,4,true).WithMessage("优惠金额:小数位不能超过4。");
 RuleFor(x => x.PrePayMoney).PrecisionScale(19,4,true).WithMessage("预收款:小数位不能超过4。");
       	
           	                //long
                //SaleOut_MainID
                //tb_SaleOutDetail
                RuleFor(c => c.tb_SaleOutDetails).NotNull();
                RuleForEach(x => x.tb_SaleOutDetails).NotNull();
                //RuleFor(x => x.tb_SaleOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_SaleOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
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

