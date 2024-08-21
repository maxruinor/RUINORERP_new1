
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:33
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
    /// 采购入库单 供应商接到采购订单后，向企业发货，用户在收到货物时，可以先检验，对合格品进行入库，也可以直接入库，形成采购入库单。为了保证清楚地记录进货情况，对进货的管理就很重要，而在我们的系统中，凭证、收付款是根据进货单自动一环扣一环地切制，故详细输入进货单资料后，存货的数量、成本会随着改变，收付账款也会跟着你的立帐方式变化；凭证亦会随着“您是否立即产生凭证”变化。采购入库单可以由采购订单、借入单、在途物资单转入，也可以手动录入新增单据。验证类
    /// </summary>
    public partial class tb_PurEntryValidator:AbstractValidator<tb_PurEntry>
    {
     public tb_PurEntryValidator() 
     {
      RuleFor(tb_PurEntry =>tb_PurEntry.PurEntryNo).MaximumLength(50).WithMessage("入库单号:不能超过最大长度,50.");
 RuleFor(tb_PurEntry =>tb_PurEntry.PurEntryNo).NotEmpty().WithMessage("入库单号:不能为空。");
 RuleFor(tb_PurEntry =>tb_PurEntry.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("厂商:下拉选择值不正确。");
 RuleFor(tb_PurEntry =>tb_PurEntry.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_PurEntry =>tb_PurEntry.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(tb_PurEntry =>tb_PurEntry.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_PurEntry =>tb_PurEntry.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_PurEntry =>tb_PurEntry.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("交易方式:下拉选择值不正确。");
 RuleFor(tb_PurEntry =>tb_PurEntry.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);
 RuleFor(tb_PurEntry =>tb_PurEntry.PurOrder_ID).Must(CheckForeignKeyValueCanNull).WithMessage("采购订单:下拉选择值不正确。");
 RuleFor(tb_PurEntry =>tb_PurEntry.PurOrder_ID).NotEmpty().When(x => x.PurOrder_ID.HasValue);
 RuleFor(x => x.TotalQty).PrecisionScale(19,4,true).WithMessage("合计数量:小数位不能超过4。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("合计金额:小数位不能超过4。");
 RuleFor(x => x.ActualAmount).PrecisionScale(19,4,true).WithMessage("实付金额:小数位不能超过4。");
 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("合计税额:小数位不能超过4。");
 RuleFor(x => x.DiscountAmount).PrecisionScale(19,4,true).WithMessage("折扣金额总计:小数位不能超过4。");
 RuleFor(tb_PurEntry =>tb_PurEntry.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_PurEntry =>tb_PurEntry.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_PurEntry =>tb_PurEntry.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_PurEntry =>tb_PurEntry.ApprovalOpinions).MaximumLength(200).WithMessage("审批意见:不能超过最大长度,200.");
//***** 
 RuleFor(tb_PurEntry =>tb_PurEntry.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(tb_PurEntry =>tb_PurEntry.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_PurEntry =>tb_PurEntry.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
 RuleFor(tb_PurEntry =>tb_PurEntry.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);
 RuleFor(x => x.Deposit).PrecisionScale(19,4,true).WithMessage("订金:小数位不能超过4。");
 RuleFor(tb_PurEntry =>tb_PurEntry.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);
 RuleFor(tb_PurEntry =>tb_PurEntry.VoucherNO).MaximumLength(50).WithMessage("凭证号码:不能超过最大长度,50.");
 RuleFor(tb_PurEntry =>tb_PurEntry.PurOrder_NO).MaximumLength(50).WithMessage("采购订单号:不能超过最大长度,50.");
 RuleFor(x => x.ShippingCost).PrecisionScale(19,4,true).WithMessage(":小数位不能超过4。");
       	
           	                //long
                //PurEntryID
                //tb_PurEntryDetail
                RuleFor(c => c.tb_PurEntryDetails).NotNull();
                RuleForEach(x => x.tb_PurEntryDetails).NotNull();
                //RuleFor(x => x.tb_PurEntryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_PurEntryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
     }




        private bool DetailedRecordsNotEmpty(List<tb_PurEntryDetail> details)
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

