﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:02
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
    /// 付款单明细验证类
    /// </summary>
    /*public partial class tb_FM_PaymentBillDetailValidator:AbstractValidator<tb_FM_PaymentBillDetail>*/
    public partial class tb_FM_PaymentBillDetailValidator:BaseValidatorGeneric<tb_FM_PaymentBillDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentBillDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     

 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.PrePaymentBill_id).NotEmpty().When(x => x.PrePaymentBill_id.HasValue);



 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.Notes).MaximumLength(15).WithMessage("备注:不能超过最大长度,15.");

 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.VoucherNumber).MaximumLength(25).WithMessage("凭证号码:不能超过最大长度,25.");


 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.Reason).MaximumLength(100).WithMessage("付款原因:不能超过最大长度,100.");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("付款总金额:小数位不能超过4。");

 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);

 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);

 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");

 RuleFor(x => x.OverpaymentAmount).PrecisionScale(19,4,true).WithMessage("超付金额:小数位不能超过4。");


 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.Notes2).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");


 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");

 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




 RuleFor(tb_FM_PaymentBillDetail =>tb_FM_PaymentBillDetail.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

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

