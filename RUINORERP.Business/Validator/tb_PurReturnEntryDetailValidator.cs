
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/16/2024 20:05:38
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
    /// 采购退货入库单明细验证类
    /// </summary>
    /*public partial class tb_PurReturnEntryDetailValidator:AbstractValidator<tb_PurReturnEntryDetail>*/
    public partial class tb_PurReturnEntryDetailValidator:BaseValidatorGeneric<tb_PurReturnEntryDetail>
    {
     public tb_PurReturnEntryDetailValidator() 
     {
     //***** 
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.PurReEntry_ID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.CustomertModel).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.UnitPrice).PrecisionScale(19,6,true).WithMessage("单价:小数位不能超过6。");
 RuleFor(x => x.Discount).PrecisionScale(8,2,true).WithMessage("折扣:小数位不能超过2。");
 RuleFor(x => x.TransactionPrice).PrecisionScale(19,6,true).WithMessage("成交单价:小数位不能超过6。");
 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");
 RuleFor(x => x.TaxAmount).PrecisionScale(19,6,true).WithMessage("税额:小数位不能超过6。");
 RuleFor(x => x.SubtotalTrPriceAmount).PrecisionScale(19,6,true).WithMessage("小计:小数位不能超过6。");
 RuleFor(tb_PurReturnEntryDetail =>tb_PurReturnEntryDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
 RuleFor(x => x.DiscountAmount).PrecisionScale(19,6,true).WithMessage("优惠金额:小数位不能超过6。");
       	
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

