
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:44
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
    /// 付款单 中有两种情况，1）如果有应收款，可以抵扣而少付款，如果有预付款也可以抵扣。验证类
    /// </summary>
    /*public partial class tb_FM_PaymentBillValidator:AbstractValidator<tb_FM_PaymentBill>*/
    public partial class tb_FM_PaymentBillValidator:BaseValidatorGeneric<tb_FM_PaymentBill>
    {
     public tb_FM_PaymentBillValidator() 
     {
      RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("厂商:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage("币别:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.account_id).Must(CheckForeignKeyValueCanNull).WithMessage("账户:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.account_id).NotEmpty().When(x => x.account_id.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.accountNo).MaximumLength(15).WithMessage("账号:不能超过最大长度,15.");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.PrePaymentBill_id).Must(CheckForeignKeyValueCanNull).WithMessage("预付单:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.PrePaymentBill_id).NotEmpty().When(x => x.PrePaymentBill_id.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Notes).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.VoucherNumber).MaximumLength(25).WithMessage("凭证号码:不能超过最大长度,25.");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Reason).MaximumLength(100).WithMessage("付款原因:不能超过最大长度,100.");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,6,true).WithMessage("付款总金额:小数位不能超过6。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");
 RuleFor(x => x.OverpaymentAmount).PrecisionScale(19,6,true).WithMessage("超付金额:小数位不能超过6。");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
//***** 
 RuleFor(tb_FM_PaymentBill =>tb_FM_PaymentBill.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
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

