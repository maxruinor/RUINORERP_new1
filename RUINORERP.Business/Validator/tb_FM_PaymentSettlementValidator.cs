
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/26/2025 22:26:21
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
    /// 记录收款 与应收的匹配，核销表验证类
    /// </summary>
    /*public partial class tb_FM_PaymentSettlementValidator:AbstractValidator<tb_FM_PaymentSettlement>*/
    public partial class tb_FM_PaymentSettlementValidator:BaseValidatorGeneric<tb_FM_PaymentSettlement>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentSettlementValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.SettlementNo).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.BizType).NotEmpty().When(x => x.BizType.HasValue);

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.SourceBillID).NotEmpty().When(x => x.SourceBillID.HasValue);

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.SourceBillNO).MaximumLength(15).WithMessage("来源单据编号:不能超过最大长度,15.");

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.SourceBizType).NotEmpty().When(x => x.SourceBizType.HasValue);

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.SourceCurrencyID).NotEmpty().When(x => x.SourceCurrencyID.HasValue);

 RuleFor(x => x.SourceExchangeRate).PrecisionScale(18,6,true).WithMessage("来源汇率:小数位不能超过6。");

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.TargetBizType).NotEmpty().When(x => x.TargetBizType.HasValue);

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.TargetBillID).NotEmpty().When(x => x.TargetBillID.HasValue);

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.TargetBillNO).MaximumLength(15).WithMessage("目标单据编号:不能超过最大长度,15.");

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.TargetCurrencyID).NotEmpty().When(x => x.TargetCurrencyID.HasValue);

 RuleFor(x => x.TargetExchangeRate).PrecisionScale(18,6,true).WithMessage("目标汇率:小数位不能超过6。");



 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

//***** 
 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.CustomerVendor_ID).NotNull().WithMessage("往来单位:不能为空。");

 RuleFor(x => x.SettledForeignAmount).PrecisionScale(19,4,true).WithMessage("核销金额外币:小数位不能超过4。");

 RuleFor(x => x.SettledLocalAmount).PrecisionScale(19,4,true).WithMessage("核销金额本币:小数位不能超过4。");



 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.ReversedSettlementID).NotEmpty().When(x => x.ReversedSettlementID.HasValue);


 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.Notes).MaximumLength(150).WithMessage("备注:不能超过最大长度,150.");

//***** 
 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.SettlementType).NotNull().WithMessage("核销状态:不能为空。");

 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.EvidenceImagePath).MaximumLength(300).WithMessage("凭证图:不能超过最大长度,300.");


 RuleFor(tb_FM_PaymentSettlement =>tb_FM_PaymentSettlement.Created_by).NotEmpty().When(x => x.Created_by.HasValue);

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

