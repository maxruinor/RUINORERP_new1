
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:23
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
    /// 步骤变量验证类
    /// </summary>
    /*public partial class tb_StepBodyParaValidator:AbstractValidator<tb_StepBodyPara>*/
    public partial class tb_StepBodyParaValidator:BaseValidatorGeneric<tb_StepBodyPara>
    {
     

     public tb_StepBodyParaValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_StepBodyPara =>tb_StepBodyPara.Key).MaximumMixedLength(50).WithMessage("参数key:不能超过最大长度,50.");
 RuleFor(tb_StepBodyPara =>tb_StepBodyPara.Key).NotEmpty().WithMessage("参数key:不能为空。");

 RuleFor(tb_StepBodyPara =>tb_StepBodyPara.Name).MaximumMixedLength(50).WithMessage("参数名:不能超过最大长度,50.");

 RuleFor(tb_StepBodyPara =>tb_StepBodyPara.DisplayName).MaximumMixedLength(50).WithMessage("显示名称:不能超过最大长度,50.");

 RuleFor(tb_StepBodyPara =>tb_StepBodyPara.Value).MaximumMixedLength(50).WithMessage("参数值:不能超过最大长度,50.");

 RuleFor(tb_StepBodyPara =>tb_StepBodyPara.StepBodyParaType).MaximumMixedLength(50).WithMessage("参数类型:不能超过最大长度,50.");

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

