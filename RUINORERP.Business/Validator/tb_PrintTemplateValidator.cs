
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:16
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
    /// 打印模板验证类
    /// </summary>
    /*public partial class tb_PrintTemplateValidator:AbstractValidator<tb_PrintTemplate>*/
    public partial class tb_PrintTemplateValidator:BaseValidatorGeneric<tb_PrintTemplate>
    {
     

     public tb_PrintTemplateValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.PrintConfigID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.PrintConfigID).NotEmpty().When(x => x.PrintConfigID.HasValue);

 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Template_Name).MaximumMixedLength(100).WithMessage("模板名称:不能超过最大长度,100.");

 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.BizType).NotEmpty().When(x => x.BizType.HasValue);

 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.BizName).MaximumMixedLength(30).WithMessage("业务名称:不能超过最大长度,30.");





 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_PrintTemplate =>tb_PrintTemplate.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


           	  
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

