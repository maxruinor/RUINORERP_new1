
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:09
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
    /// 箱规表验证类
    /// </summary>
    /*public partial class tb_BoxRulesValidator:AbstractValidator<tb_BoxRules>*/
    public partial class tb_BoxRulesValidator:BaseValidatorGeneric<tb_BoxRules>
    {
     

     public tb_BoxRulesValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_BoxRules =>tb_BoxRules.Pack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("包装信息:下拉选择值不正确。");
 RuleFor(tb_BoxRules =>tb_BoxRules.Pack_ID).NotEmpty().When(x => x.Pack_ID.HasValue);

 RuleFor(tb_BoxRules =>tb_BoxRules.CartonID).Must(CheckForeignKeyValue).WithMessage("纸箱规格:下拉选择值不正确。");

 RuleFor(tb_BoxRules =>tb_BoxRules.BoxRuleName).MaximumMixedLength(255).WithMessage("箱规名称:不能超过最大长度,255.");

//***** 
 RuleFor(tb_BoxRules =>tb_BoxRules.QuantityPerBox).NotNull().WithMessage(" 每箱数量:不能为空。");

 RuleFor(tb_BoxRules =>tb_BoxRules.PackingMethod).MaximumMixedLength(100).WithMessage("装箱方式:不能超过最大长度,100.");

 RuleFor(x => x.Length).PrecisionScale(8,2,true).WithMessage("长度(cm):小数位不能超过2。");

 RuleFor(x => x.Width).PrecisionScale(8,2,true).WithMessage("宽度(cm):小数位不能超过2。");

 RuleFor(x => x.Height).PrecisionScale(8,2,true).WithMessage("高度(cm):小数位不能超过2。");

 RuleFor(x => x.Volume).PrecisionScale(10,3,true).WithMessage("体积Vol(cm³):小数位不能超过3。");

 RuleFor(x => x.NetWeight).PrecisionScale(10,3,true).WithMessage("净重N.Wt.(kg):小数位不能超过3。");

 RuleFor(x => x.GrossWeight).PrecisionScale(10,3,true).WithMessage("毛重G.Wt.(kg):小数位不能超过3。");

 RuleFor(tb_BoxRules =>tb_BoxRules.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");

//有默认值


 RuleFor(tb_BoxRules =>tb_BoxRules.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_BoxRules =>tb_BoxRules.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

