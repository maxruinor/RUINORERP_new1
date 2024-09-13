
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:33
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
    /// 步骤定义验证类
    /// </summary>
    /*public partial class tb_StepBodyValidator:AbstractValidator<tb_StepBody>*/
    public partial class tb_StepBodyValidator:BaseValidatorGeneric<tb_StepBody>
    {
     public tb_StepBodyValidator() 
     {
      RuleFor(tb_StepBody =>tb_StepBody.Para_Id).Must(CheckForeignKeyValueCanNull).WithMessage("输入参数:下拉选择值不正确。");
 RuleFor(tb_StepBody =>tb_StepBody.Para_Id).NotEmpty().When(x => x.Para_Id.HasValue);
 RuleFor(tb_StepBody =>tb_StepBody.Name).MaximumLength(25).WithMessage("名称:不能超过最大长度,25.");
 RuleFor(tb_StepBody =>tb_StepBody.DisplayName).MaximumLength(25).WithMessage("显示名称:不能超过最大长度,25.");
 RuleFor(tb_StepBody =>tb_StepBody.TypeFullName).MaximumLength(25).WithMessage("类型全名:不能超过最大长度,25.");
 RuleFor(tb_StepBody =>tb_StepBody.AssemblyFullName).MaximumLength(25).WithMessage("标题:不能超过最大长度,25.");
       	
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

