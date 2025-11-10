
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:11
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
    /// 收付款记录表验证类
    /// </summary>
    /*public partial class tb_FM_PaymentRecordValidator:AbstractValidator<tb_FM_PaymentRecord>*/
    public partial class tb_FM_PaymentRecordValidator:BaseValidatorGeneric<tb_FM_PaymentRecord>
    {
     

     public tb_FM_PaymentRecordValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PaymentNo).MaximumMixedLength(30).WithMessage("支付单号:不能超过最大长度,30.");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PaymentNo).NotEmpty().WithMessage("支付单号:不能为空。");

//***** 
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReceivePaymentType).NotNull().WithMessage("收付类型:不能为空。");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("公司账户:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Reimburser).NotEmpty().When(x => x.Reimburser.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("往来单位:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PayeeInfoID).Must(CheckForeignKeyValueCanNull).WithMessage("收款信息:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PayeeAccountNo).MaximumMixedLength(100).WithMessage("收款账号:不能超过最大长度,100.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.SourceBillNos).MaximumMixedLength(1000).WithMessage("来源单号:不能超过最大长度,1000.");



 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(x => x.TotalForeignAmount).PrecisionScale(19,4,true).WithMessage("实付金额外币:小数位不能超过4。");

 RuleFor(x => x.TotalLocalAmount).PrecisionScale(19,4,true).WithMessage("实付金额本币:小数位不能超过4。");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.LocalPamountInWords).MaximumMixedLength(100).WithMessage("实付金额大写:不能超过最大长度,100.");

 RuleFor(x => x.TotalForeignPayableAmount).PrecisionScale(19,4,true).WithMessage("应付金额外币:小数位不能超过4。");

 RuleFor(x => x.TotalLocalPayableAmount).PrecisionScale(19,4,true).WithMessage("应付金额本币:小数位不能超过4。");


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款方式:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

//***** 
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PaymentStatus).NotNull().WithMessage("支付状态:不能为空。");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PaymentImagePath).MaximumMixedLength(300).WithMessage("付款凭证:不能超过最大长度,300.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReferenceNo).MaximumMixedLength(300).WithMessage("交易参考号:不能超过最大长度,300.");


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReversedOriginalId).NotEmpty().When(x => x.ReversedOriginalId.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReversedOriginalNo).MaximumMixedLength(30).WithMessage("冲销单号:不能超过最大长度,30.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReversedByPaymentId).NotEmpty().When(x => x.ReversedByPaymentId.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReversedByPaymentNo).MaximumMixedLength(30).WithMessage("被冲销单号:不能超过最大长度,30.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Remark).MaximumMixedLength(300).WithMessage("备注:不能超过最大长度,300.");


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ApprovalOpinions).MaximumMixedLength(255).WithMessage("审批意见:不能超过最大长度,255.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //PaymentId
                //tb_FM_PaymentRecordDetail
                //RuleFor(x => x.tb_FM_PaymentRecordDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_PaymentRecordDetails).NotNull();
                //RuleForEach(x => x.tb_FM_PaymentRecordDetails).NotNull();
                //RuleFor(x => x.tb_FM_PaymentRecordDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PaymentRecordDetail> details)
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

