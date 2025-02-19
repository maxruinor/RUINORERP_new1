
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:04
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
    /// 收款单明细-对应的应收单据项目验证类
    /// </summary>
    /*public partial class tb_FM_PaymentReceiptDetailValidator:AbstractValidator<tb_FM_PaymentReceiptDetail>*/
    public partial class tb_FM_PaymentReceiptDetailValidator:BaseValidatorGeneric<tb_FM_PaymentReceiptDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentReceiptDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("所属项目:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.PaymentReceiptID).NotEmpty().When(x => x.PaymentReceiptID.HasValue);

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");


 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.PayReasonItems).MaximumLength(100).WithMessage("收款项目/原因:不能超过最大长度,100.");
 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.PayReasonItems).NotEmpty().WithMessage("收款项目/原因:不能为空。");

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(x => x.SubAmount).PrecisionScale(19,4,true).WithMessage("收款金额:小数位不能超过4。");

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.SubPamountInWords).MaximumLength(50).WithMessage("大写金额:不能超过最大长度,50.");
 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.SubPamountInWords).NotEmpty().WithMessage("大写金额:不能为空。");

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.SubEvidenceImagePath).MaximumLength(300).WithMessage("凭证图:不能超过最大长度,300.");

 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.Summary).MaximumLength(150).WithMessage("摘要:不能超过最大长度,150.");


 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentReceiptDetail =>tb_FM_PaymentReceiptDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

