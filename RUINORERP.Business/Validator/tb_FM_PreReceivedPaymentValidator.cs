
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:15
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
    /// 预收付款单验证类
    /// </summary>
    /*public partial class tb_FM_PreReceivedPaymentValidator:AbstractValidator<tb_FM_PreReceivedPayment>*/
    public partial class tb_FM_PreReceivedPaymentValidator:BaseValidatorGeneric<tb_FM_PreReceivedPayment>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PreReceivedPaymentValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PreRPNO).MaximumLength(15).WithMessage("单据编号:不能超过最大长度,15.");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PreRPNO).NotEmpty().WithMessage("单据编号:不能为空。");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("收付款账户:下拉选择值不正确。");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("往来单位:下拉选择值不正确。");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PayeeInfoID).Must(CheckForeignKeyValueCanNull).WithMessage("收款信息:下拉选择值不正确。");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PayeeAccountNo).MaximumLength(50).WithMessage("收款账号:不能超过最大长度,50.");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款方式:下拉选择值不正确。");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage("币别:下拉选择值不正确。");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");


 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PrePaymentReason).MaximumLength(100).WithMessage("事由:不能超过最大长度,100.");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PrePaymentReason).NotEmpty().WithMessage("事由:不能为空。");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.FMPaymentStatus).NotEmpty().When(x => x.FMPaymentStatus.HasValue);

 RuleFor(x => x.ForeignPrepaidAmount).PrecisionScale(19,4,true).WithMessage("预定金额外币:小数位不能超过4。");

 RuleFor(x => x.LocalPrepaidAmount).PrecisionScale(19,4,true).WithMessage("预定金额本币:小数位不能超过4。");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.LocalPrepaidAmountInWords).MaximumLength(75).WithMessage("大写预定金额本币:不能超过最大长度,75.");
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.LocalPrepaidAmountInWords).NotEmpty().WithMessage("大写预定金额本币:不能为空。");

 RuleFor(x => x.ForeignPaidAmount).PrecisionScale(19,4,true).WithMessage("核销金额外币:小数位不能超过4。");

 RuleFor(x => x.LocalPaidAmount).PrecisionScale(19,4,true).WithMessage("核销金额本币:小数位不能超过4。");

 RuleFor(x => x.ForeignBalanceAmount).PrecisionScale(19,4,true).WithMessage("余额外币:小数位不能超过4。");

 RuleFor(x => x.LocalBalanceAmount).PrecisionScale(19,4,true).WithMessage("余额本币:小数位不能超过4。");

//***** 
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.ReceivePaymentType).NotNull().WithMessage("收付款类型:不能为空。");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PaymentImagePath).MaximumLength(300).WithMessage("付款凭证:不能超过最大长度,300.");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Remark).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");


 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_PreReceivedPayment =>tb_FM_PreReceivedPayment.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

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

