
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/25/2025 17:33:33
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
    /// 收款付款记录表-记录所有资金流动一批订单可分账户分批付 记录真实资金流动，用户需确保其 与银行流水一致验证类
    /// </summary>
    /*public partial class tb_FM_PaymentRecordValidator:AbstractValidator<tb_FM_PaymentRecord>*/
    public partial class tb_FM_PaymentRecordValidator:BaseValidatorGeneric<tb_FM_PaymentRecord>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentRecordValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PaymentNo).MaximumLength(15).WithMessage("支付单号:不能超过最大长度,15.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.BizType).NotEmpty().When(x => x.BizType.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.SourceBilllID).NotEmpty().When(x => x.SourceBilllID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReceivePaymentType).NotEmpty().When(x => x.ReceivePaymentType.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("公司账户:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("往来单位:下拉选择值不正确。");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PayeeInfoID).Must(CheckForeignKeyValueCanNull).WithMessage("收款信息:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PayeeAccountNo).MaximumLength(50).WithMessage("收款账号:不能超过最大长度,50.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage("币别:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.ForeignPaidAmount).PrecisionScale(19,4,true).WithMessage("支付金额外币:小数位不能超过4。");

 RuleFor(x => x.LocalPaidAmount).PrecisionScale(19,4,true).WithMessage("支付金额本币:小数位不能超过4。");


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款方式:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ReferenceNo).MaximumLength(150).WithMessage("交易参考号:不能超过最大长度,150.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Remark).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PaymentImagePath).MaximumLength(300).WithMessage("付款凭证:不能超过最大长度,300.");

//***** 
 RuleFor(tb_FM_PaymentRecord =>tb_FM_PaymentRecord.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

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

