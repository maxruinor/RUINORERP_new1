
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:25
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
    /// 返厂明细验证类
    /// </summary>
    /*public partial class tb_ReturnDetailValidator:AbstractValidator<tb_ReturnDetail>*/
    public partial class tb_ReturnDetailValidator:BaseValidatorGeneric<tb_ReturnDetail>
    {
     public tb_ReturnDetailValidator() 
     {
      RuleFor(tb_ReturnDetail =>tb_ReturnDetail.MainID).NotEmpty().When(x => x.MainID.HasValue);
 RuleFor(tb_ReturnDetail =>tb_ReturnDetail.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("货品详情:下拉选择值不正确。");
 RuleFor(tb_ReturnDetail =>tb_ReturnDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
//***** 
 RuleFor(tb_ReturnDetail =>tb_ReturnDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.Cost).PrecisionScale(18,0,true).WithMessage("成本:小数位不能超过0。");
 RuleFor(x => x.Price).PrecisionScale(18,0,true).WithMessage("单价:小数位不能超过0。");
 RuleFor(x => x.SubtotalAmount).PrecisionScale(19,6,true).WithMessage("金额小计:小数位不能超过6。");
 RuleFor(x => x.SubtotalCost).PrecisionScale(19,6,true).WithMessage("成本小计:小数位不能超过6。");
 RuleFor(tb_ReturnDetail =>tb_ReturnDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");
 RuleFor(tb_ReturnDetail =>tb_ReturnDetail.CustomerPartNo).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");
       	
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

