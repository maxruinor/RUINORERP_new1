
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
    /// 步骤定义验证类
    /// </summary>
    /*public partial class tb_StepBodyValidator:AbstractValidator<tb_StepBody>*/
    public partial class tb_StepBodyValidator:BaseValidatorGeneric<tb_StepBody>
    {
     

     public tb_StepBodyValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_StepBody =>tb_StepBody.Para_Id).Must(CheckForeignKeyValueCanNull).WithMessage("输入参数:下拉选择值不正确。");
 RuleFor(tb_StepBody =>tb_StepBody.Para_Id).NotEmpty().When(x => x.Para_Id.HasValue);

 RuleFor(tb_StepBody =>tb_StepBody.Name).MaximumMixedLength(50).WithMessage("名称:不能超过最大长度,50.");

 RuleFor(tb_StepBody =>tb_StepBody.DisplayName).MaximumMixedLength(50).WithMessage("显示名称:不能超过最大长度,50.");

 RuleFor(tb_StepBody =>tb_StepBody.TypeFullName).MaximumMixedLength(50).WithMessage("类型全名:不能超过最大长度,50.");

 RuleFor(tb_StepBody =>tb_StepBody.AssemblyFullName).MaximumMixedLength(50).WithMessage("标题:不能超过最大长度,50.");

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

