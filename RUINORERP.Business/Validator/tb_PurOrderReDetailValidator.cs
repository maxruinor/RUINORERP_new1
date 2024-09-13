﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:24
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
    /// 采购退回单验证类
    /// </summary>
    /*public partial class tb_PurOrderReDetailValidator:AbstractValidator<tb_PurOrderReDetail>*/
    public partial class tb_PurOrderReDetailValidator:BaseValidatorGeneric<tb_PurOrderReDetail>
    {
     public tb_PurOrderReDetailValidator() 
     {
     //***** 
 RuleFor(tb_PurOrderReDetail =>tb_PurOrderReDetail.PurRetrunID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_PurOrderReDetail =>tb_PurOrderReDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");
 RuleFor(tb_PurOrderReDetail =>tb_PurOrderReDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_PurOrderReDetail =>tb_PurOrderReDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_PurOrderReDetail =>tb_PurOrderReDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.Discount).PrecisionScale(8,3,true).WithMessage("折扣:小数位不能超过3。");
 RuleFor(x => x.TransactionPrice).PrecisionScale(18,0,true).WithMessage("成交单价:小数位不能超过0。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("小计:小数位不能超过4。");
 RuleFor(tb_PurOrderReDetail =>tb_PurOrderReDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");
 RuleFor(x => x.commission).PrecisionScale(19,4,true).WithMessage("抽成金额:小数位不能超过4。");
 RuleFor(tb_PurOrderReDetail =>tb_PurOrderReDetail.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");
       	
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

