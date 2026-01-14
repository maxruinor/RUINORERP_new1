
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
    /// 对账单明细验证类
    /// </summary>
    /*public partial class tb_FM_StatementDetailValidator:AbstractValidator<tb_FM_StatementDetail>*/
    public partial class tb_FM_StatementDetailValidator:BaseValidatorGeneric<tb_FM_StatementDetail>
    {
     

     public tb_FM_StatementDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.StatementId).NotNull().WithMessage("对账单:不能为空。");

 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.ARAPId).Must(CheckForeignKeyValue).WithMessage("应收付款单:下拉选择值不正确。");

 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.ARAPNo).MaximumMixedLength(30).WithMessage("单据编号:不能超过最大长度,30.");

//***** 
 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.ReceivePaymentType).NotNull().WithMessage("收付类型:不能为空。");



 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.IncludedLocalAmount).PrecisionScale(19,4,true).WithMessage("对账金额本币:小数位不能超过4。");

 RuleFor(x => x.IncludedForeignAmount).PrecisionScale(19,4,true).WithMessage("对账金额外币:小数位不能超过4。");

 RuleFor(x => x.WrittenOffLocalAmount).PrecisionScale(19,4,true).WithMessage("本次已核销本币金额:小数位不能超过4。");

 RuleFor(x => x.WrittenOffForeignAmount).PrecisionScale(19,4,true).WithMessage("本次已核销原币金额:小数位不能超过4。");

 RuleFor(x => x.RemainingLocalAmount).PrecisionScale(19,4,true).WithMessage("剩余未核销本币金额:小数位不能超过4。");

 RuleFor(x => x.RemainingForeignAmount).PrecisionScale(19,4,true).WithMessage("剩余未核销原币金额:小数位不能超过4。");

//***** 
 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.ARAPWriteOffStatus).NotNull().WithMessage("核销状态:不能为空。");

 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.Summary).MaximumMixedLength(300).WithMessage("摘要:不能超过最大长度,300.");

           	  
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

