
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:11
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
    /// 发票明细验证类
    /// </summary>
    /*public partial class tb_FM_InvoiceDetailValidator:AbstractValidator<tb_FM_InvoiceDetail>*/
    public partial class tb_FM_InvoiceDetailValidator:BaseValidatorGeneric<tb_FM_InvoiceDetail>
    {
     

     public tb_FM_InvoiceDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.InvoiceId).NotEmpty().When(x => x.InvoiceId.HasValue);

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.SourceBizType).NotEmpty().When(x => x.SourceBizType.HasValue);

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.SourceBillId).NotEmpty().When(x => x.SourceBillId.HasValue);

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.SourceBillNo).MaximumMixedLength(30).WithMessage("来源单号:不能超过最大长度,30.");

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.ItemType).NotEmpty().When(x => x.ItemType.HasValue);

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.Specifications).MaximumMixedLength(1000).WithMessage("规格:不能超过最大长度,1000.");

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);


 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(x => x.Quantity).PrecisionScale(10,4,true).WithMessage("数量:小数位不能超过4。");

 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxSubtotalAmount).PrecisionScale(19,4,true).WithMessage("明细税额:小数位不能超过4。");

 RuleFor(x => x.SubtotalAmount).PrecisionScale(19,4,true).WithMessage("明细金额:小数位不能超过4。");

 RuleFor(tb_FM_InvoiceDetail =>tb_FM_InvoiceDetail.Summary).MaximumMixedLength(300).WithMessage("摘要:不能超过最大长度,300.");

           	  
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

