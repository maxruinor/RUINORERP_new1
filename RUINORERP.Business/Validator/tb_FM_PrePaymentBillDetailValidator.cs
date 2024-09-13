
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:45
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 预付款表明细验证类
    /// </summary>
    /*public partial class tb_FM_PrePaymentBillDetailValidator:AbstractValidator<tb_FM_PrePaymentBillDetail>*/
    public partial class tb_FM_PrePaymentBillDetailValidator:BaseValidatorGeneric<tb_FM_PrePaymentBillDetail>
    {
     public tb_FM_PrePaymentBillDetailValidator() 
     {
      RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.PrePaymentBill_id).NotEmpty().When(x => x.PrePaymentBill_id.HasValue);
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("厂商:下拉选择值不正确。");
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage("币别:下拉选择值不正确。");
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.account_id).Must(CheckForeignKeyValueCanNull).WithMessage("账户:下拉选择值不正确。");
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.account_id).NotEmpty().When(x => x.account_id.HasValue);
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.PaymentReason).MaximumLength(25).WithMessage("事由:不能超过最大长度,25.");
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.SourceBill_BizType).NotEmpty().When(x => x.SourceBill_BizType.HasValue);
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.SourceBill_ID).NotEmpty().When(x => x.SourceBill_ID.HasValue);
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.SourceBillNO).MaximumLength(15).WithMessage("来源单号:不能超过最大长度,15.");
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.Reason).MaximumLength(25).WithMessage("原因:不能超过最大长度,25.");
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.OffsetMethod).MaximumLength(25).WithMessage("冲销方式:不能超过最大长度,25.");
 RuleFor(x => x.Amount).PrecisionScale(19,4,true).WithMessage("金额:小数位不能超过4。");
 RuleFor(x => x.PrepaidAmount).PrecisionScale(19,4,true).WithMessage("已转金额:小数位不能超过4。");
 RuleFor(tb_FM_PrePaymentBillDetail =>tb_FM_PrePaymentBillDetail.Notes).MaximumLength(15).WithMessage("备注:不能超过最大长度,15.");
       	
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

