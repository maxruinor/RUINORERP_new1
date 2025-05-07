
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 14:22:22
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
    /// 发票验证类
    /// </summary>
    /*public partial class tb_FM_InvoiceValidator:AbstractValidator<tb_FM_Invoice>*/
    public partial class tb_FM_InvoiceValidator:BaseValidatorGeneric<tb_FM_Invoice>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_InvoiceValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.InvoiceNo).MaximumLength(30).WithMessage("发票号码:不能超过最大长度,30.");
 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.InvoiceNo).NotEmpty().WithMessage("发票号码:不能为空。");

 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("付款单位:下拉选择值不正确。");

 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.BillingInfo_ID).Must(CheckForeignKeyValueCanNull).WithMessage("开票资料:下拉选择值不正确。");
 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.BillingInfo_ID).NotEmpty().When(x => x.BillingInfo_ID.HasValue);


 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.TotalWithTax).PrecisionScale(19,4,true).WithMessage("价税合计:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("发票总金额（不含税）:小数位不能超过4。");


 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.Notes).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");

//***** 
 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.ReceivePaymentType).NotNull().WithMessage("付款状态:不能为空。");


 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

//***** 
 RuleFor(tb_FM_Invoice =>tb_FM_Invoice.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long?
                //InvoiceId
                //tb_FM_InvoiceDetail
                //RuleFor(x => x.tb_FM_InvoiceDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_InvoiceDetails).NotNull();
                //RuleForEach(x => x.tb_FM_InvoiceDetails).NotNull();
                //RuleFor(x => x.tb_FM_InvoiceDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_InvoiceDetail> details)
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

