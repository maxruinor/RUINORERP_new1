
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 15:37:43
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
    /// 收款单明细表：记录收款分配到应收单的明细验证类
    /// </summary>
    /*public partial class tb_FM_PaymentRecordDetailValidator:AbstractValidator<tb_FM_PaymentRecordDetail>*/
    public partial class tb_FM_PaymentRecordDetailValidator:BaseValidatorGeneric<tb_FM_PaymentRecordDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentRecordDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
      RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.PaymentId).NotEmpty().When(x => x.PaymentId.HasValue);


//***** 
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBizType).NotNull().WithMessage("来源业务:不能为空。");

//***** 
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBilllId).NotNull().WithMessage("来源单据:不能为空。");

 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBillNo).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBillNo).NotEmpty().WithMessage("来源单号:不能为空。");

//***** 
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.Currency_ID).NotNull().WithMessage("币别:不能为空。");

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.ForeignAmount).PrecisionScale(19,4,true).WithMessage("支付金额外币:小数位不能超过4。");

 RuleFor(x => x.LocalAmount).PrecisionScale(19,4,true).WithMessage("支付金额本币:小数位不能超过4。");

 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.Summary).MaximumLength(150).WithMessage("摘要:不能超过最大长度,150.");

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

