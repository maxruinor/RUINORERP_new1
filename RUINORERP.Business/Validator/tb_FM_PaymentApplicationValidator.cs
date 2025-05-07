
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 14:22:25
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
    /// 付款申请单-目前代替纸的申请单将来完善明细则用付款单的主子表来完成系统可以根据客户来自动生成经人确认验证类
    /// </summary>
    /*public partial class tb_FM_PaymentApplicationValidator:AbstractValidator<tb_FM_PaymentApplication>*/
    public partial class tb_FM_PaymentApplicationValidator:BaseValidatorGeneric<tb_FM_PaymentApplication>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentApplicationValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.ApplicationNo).MaximumLength(15).WithMessage("申请单号:不能超过最大长度,15.");
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.ApplicationNo).NotEmpty().WithMessage("申请单号:不能为空。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Employee_ID).Must(CheckForeignKeyValue).WithMessage("制单人:下拉选择值不正确。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("收款单位:下拉选择值不正确。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PayeeInfoID).Must(CheckForeignKeyValue).WithMessage("收款信息:下拉选择值不正确。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PayeeAccountNo).MaximumLength(50).WithMessage("收款账号:不能超过最大长度,50.");
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PayeeAccountNo).NotEmpty().WithMessage("收款账号:不能为空。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("付款账户:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Account_id).NotEmpty().When(x => x.Account_id.HasValue);


 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PrePaymentBill_id).NotEmpty().When(x => x.PrePaymentBill_id.HasValue);

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PayReasonItems).MaximumLength(500).WithMessage("付款项目/原因:不能超过最大长度,500.");
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PayReasonItems).NotEmpty().WithMessage("付款项目/原因:不能为空。");


 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Notes).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("付款金额:小数位不能超过4。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PamountInWords).MaximumLength(50).WithMessage("大写金额:不能超过最大长度,50.");
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PamountInWords).NotEmpty().WithMessage("大写金额:不能为空。");

 RuleFor(x => x.OverpaymentAmount).PrecisionScale(19,4,true).WithMessage("超付金额:小数位不能超过4。");


 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.CloseCaseImagePath).MaximumLength(300).WithMessage("结案凭证:不能超过最大长度,300.");

 RuleFor(tb_FM_PaymentApplication =>tb_FM_PaymentApplication.CloseCaseOpinions).MaximumLength(100).WithMessage("结案意见:不能超过最大长度,100.");

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

