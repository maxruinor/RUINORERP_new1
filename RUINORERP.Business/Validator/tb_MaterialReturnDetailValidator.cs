
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:14
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
    /// 退料单明细验证类
    /// </summary>
    /*public partial class tb_MaterialReturnDetailValidator:AbstractValidator<tb_MaterialReturnDetail>*/
    public partial class tb_MaterialReturnDetailValidator:BaseValidatorGeneric<tb_MaterialReturnDetail>
    {
     

     public tb_MaterialReturnDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_MaterialReturnDetail =>tb_MaterialReturnDetail.MRE_ID).NotNull().WithMessage("主单:不能为空。");

 RuleFor(tb_MaterialReturnDetail =>tb_MaterialReturnDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_MaterialReturnDetail =>tb_MaterialReturnDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_MaterialReturnDetail =>tb_MaterialReturnDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

//***** 
 RuleFor(tb_MaterialReturnDetail =>tb_MaterialReturnDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.Cost).PrecisionScale(10,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.Price).PrecisionScale(10,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(tb_MaterialReturnDetail =>tb_MaterialReturnDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");

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

