﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:37
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
    /// 盘点明细表验证类
    /// </summary>
    /*public partial class tb_StocktakeDetailValidator:AbstractValidator<tb_StocktakeDetail>*/
    public partial class tb_StocktakeDetailValidator:BaseValidatorGeneric<tb_StocktakeDetail>
    {
     public tb_StocktakeDetailValidator() 
     {
     //***** 
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.MainID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
 RuleFor(x => x.Cost).PrecisionScale(19,6,true).WithMessage("成本:小数位不能超过6。");
//***** 
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.CarryinglQty).NotNull().WithMessage("载账数量:不能为空。");
 RuleFor(x => x.CarryingSubtotalAmount).PrecisionScale(19,6,true).WithMessage("载账小计:小数位不能超过6。");
//***** 
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.DiffQty).NotNull().WithMessage("差异数量:不能为空。");
 RuleFor(x => x.DiffSubtotalAmount).PrecisionScale(19,6,true).WithMessage("差异小计:小数位不能超过6。");
//***** 
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.CheckQty).NotNull().WithMessage("盘点数量:不能为空。");
 RuleFor(x => x.CheckSubtotalAmount).PrecisionScale(19,6,true).WithMessage("盘点小计:小数位不能超过6。");
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(tb_StocktakeDetail =>tb_StocktakeDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
       	
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

