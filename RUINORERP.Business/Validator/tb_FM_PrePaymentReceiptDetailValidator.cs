
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:10
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
    /// 预收款明细表验证类
    /// </summary>
    /*public partial class tb_FM_PrePaymentReceiptDetailValidator:AbstractValidator<tb_FM_PrePaymentReceiptDetail>*/
    public partial class tb_FM_PrePaymentReceiptDetailValidator:BaseValidatorGeneric<tb_FM_PrePaymentReceiptDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PrePaymentReceiptDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.PrePaymentReceiptID).NotEmpty().When(x => x.PrePaymentReceiptID.HasValue);

 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.PaymentReason).MaximumLength(25).WithMessage("事由:不能超过最大长度,25.");

 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);

 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);

 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");

 RuleFor(x => x.PrePayAmount).PrecisionScale(19,4,true).WithMessage("预收金额:小数位不能超过4。");

 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.Summary).MaximumLength(150).WithMessage("摘要:不能超过最大长度,150.");


 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PrePaymentReceiptDetail =>tb_FM_PrePaymentReceiptDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

