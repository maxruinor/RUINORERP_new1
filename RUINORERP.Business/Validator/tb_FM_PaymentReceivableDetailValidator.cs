
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:06
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
    /// 应收明细 如果一个销售订单多次发货时，销售出库单即可对应这里的明细验证类
    /// </summary>
    /*public partial class tb_FM_PaymentReceivableDetailValidator:AbstractValidator<tb_FM_PaymentReceivableDetail>*/
    public partial class tb_FM_PaymentReceivableDetailValidator:BaseValidatorGeneric<tb_FM_PaymentReceivableDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PaymentReceivableDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.PaymentReceivableID).NotEmpty().When(x => x.PaymentReceivableID.HasValue);



 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.Reason).MaximumLength(100).WithMessage("付款用途:不能超过最大长度,100.");

 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");


 RuleFor(x => x.UntaxedAmont).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");

 RuleFor(x => x.PayableAmount).PrecisionScale(19,4,true).WithMessage("应付金额:小数位不能超过4。");

 RuleFor(x => x.PaidAmount).PrecisionScale(19,4,true).WithMessage("已付金额:小数位不能超过4。");

 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);

 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);

 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");

 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.Summary).MaximumLength(150).WithMessage("摘要:不能超过最大长度,150.");


 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_FM_PaymentReceivableDetail =>tb_FM_PaymentReceivableDetail.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);

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

