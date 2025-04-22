
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:07
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
    /// 付款申请单-即为付款单验证类
    /// </summary>
    /*public partial class tb_FM_PaymentValidator:AbstractValidator<tb_FM_Payment>*/
    public partial class tb_FM_PaymentValidator:BaseValidatorGeneric<tb_FM_Payment>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_Payment =>tb_FM_Payment.PaymentNo).MaximumLength(15).WithMessage("申请单号:不能超过最大长度,15.");
 RuleFor(tb_FM_Payment =>tb_FM_Payment.PaymentNo).NotEmpty().WithMessage("申请单号:不能为空。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.Employee_ID).Must(CheckForeignKeyValue).WithMessage("制单人:下拉选择值不正确。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("收款单位:下拉选择值不正确。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.PayeeInfoID).Must(CheckForeignKeyValue).WithMessage("收款信息:下拉选择值不正确。");

//***** 
 RuleFor(tb_FM_Payment =>tb_FM_Payment.PayeeAccountType).NotNull().WithMessage("收款账号类型:不能为空。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.PayeeAccountNo).MaximumLength(50).WithMessage("收款账号:不能超过最大长度,50.");
 RuleFor(tb_FM_Payment =>tb_FM_Payment.PayeeAccountNo).NotEmpty().WithMessage("收款账号:不能为空。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("付款账户:下拉选择值不正确。");
 RuleFor(tb_FM_Payment =>tb_FM_Payment.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_Payment =>tb_FM_Payment.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");



 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("付款金额:小数位不能超过4。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.PamountInWords).MaximumLength(50).WithMessage("大写金额:不能超过最大长度,50.");
 RuleFor(tb_FM_Payment =>tb_FM_Payment.PamountInWords).NotEmpty().WithMessage("大写金额:不能为空。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.Notes).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");

//***** 
 RuleFor(tb_FM_Payment =>tb_FM_Payment.PayStatus).NotNull().WithMessage("付款状态:不能为空。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.EvidenceImagePath).MaximumLength(300).WithMessage("凭证图:不能超过最大长度,300.");


 RuleFor(tb_FM_Payment =>tb_FM_Payment.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_Payment =>tb_FM_Payment.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_Payment =>tb_FM_Payment.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_Payment =>tb_FM_Payment.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_FM_Payment =>tb_FM_Payment.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.CloseCaseImagePath).MaximumLength(300).WithMessage("结案凭证:不能超过最大长度,300.");

 RuleFor(tb_FM_Payment =>tb_FM_Payment.CloseCaseOpinions).MaximumLength(100).WithMessage("结案意见:不能超过最大长度,100.");

           	                //long?
                //PaymentID
                //tb_FM_PaymentDetail
                //RuleFor(x => x.tb_FM_PaymentDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_PaymentDetails).NotNull();
                //RuleForEach(x => x.tb_FM_PaymentDetails).NotNull();
                //RuleFor(x => x.tb_FM_PaymentDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_PaymentDetail> details)
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

