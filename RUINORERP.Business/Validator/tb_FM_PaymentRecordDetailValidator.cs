
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:12
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 收付款记录明细表验证类
    /// </summary>
    /*public partial class tb_FM_PaymentRecordDetailValidator:AbstractValidator<tb_FM_PaymentRecordDetail>*/
    public partial class tb_FM_PaymentRecordDetailValidator:BaseValidatorGeneric<tb_FM_PaymentRecordDetail>
    {
     

     public tb_FM_PaymentRecordDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
      RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.PaymentId).NotEmpty().When(x => x.PaymentId.HasValue);


//***** 
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBizType).NotNull().WithMessage("来源业务:不能为空。");

//***** 
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBilllId).NotNull().WithMessage("来源单据:不能为空。");

 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBillNo).MaximumMixedLength(30).WithMessage("来源单号:不能超过最大长度,30.");
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.SourceBillNo).NotEmpty().WithMessage("来源单号:不能为空。");


 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.ForeignAmount).PrecisionScale(19,4,true).WithMessage("支付金额外币:小数位不能超过4。");

 RuleFor(x => x.LocalAmount).PrecisionScale(19,4,true).WithMessage("支付金额本币:小数位不能超过4。");

 RuleFor(x => x.ForeignPayableAmount).PrecisionScale(19,4,true).WithMessage("应付金额外币:小数位不能超过4。");

 RuleFor(x => x.LocalPayableAmount).PrecisionScale(19,4,true).WithMessage("应付金额本币:小数位不能超过4。");

 RuleFor(tb_FM_PaymentRecordDetail =>tb_FM_PaymentRecordDetail.Summary).MaximumMixedLength(300).WithMessage("摘要:不能超过最大长度,300.");

           	  
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

