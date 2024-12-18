﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:36
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
    /// 出库单明细验证类
    /// </summary>
    /*public partial class tb_StockOutDetailValidator:AbstractValidator<tb_StockOutDetail>*/
    public partial class tb_StockOutDetailValidator:BaseValidatorGeneric<tb_StockOutDetail>
    {
     public tb_StockOutDetailValidator() 
     {
     //***** 
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.MainID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
//***** 
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Qty).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.Price).PrecisionScale(19,6,true).WithMessage("售价:小数位不能超过6。");
 RuleFor(x => x.Cost).PrecisionScale(19,6,true).WithMessage("成本:小数位不能超过6。");
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Summary).MaximumLength(250).WithMessage("摘要:不能超过最大长度,250.");
 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,6,true).WithMessage("成本小计:小数位不能超过6。");
 RuleFor(x => x.SubtotalPirceAmount).PrecisionScale(19,6,true).WithMessage("金额小计:小数位不能超过6。");
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
       	
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

