
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:33
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
    /// 流程步骤验证类
    /// </summary>
    public partial class tb_ProcessStepValidator:AbstractValidator<tb_ProcessStep>
    {
     public tb_ProcessStepValidator() 
     {
      RuleFor(tb_ProcessStep =>tb_ProcessStep.StepBodyld).Must(CheckForeignKeyValueCanNull).WithMessage("流程定义:下拉选择值不正确。");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.StepBodyld).NotEmpty().When(x => x.StepBodyld.HasValue);
 RuleFor(tb_ProcessStep =>tb_ProcessStep.Position_Id).Must(CheckForeignKeyValueCanNull).WithMessage("位置信息:下拉选择值不正确。");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.Position_Id).NotEmpty().When(x => x.Position_Id.HasValue);
 RuleFor(tb_ProcessStep =>tb_ProcessStep.NextNode_ID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.NextNode_ID).NotEmpty().When(x => x.NextNode_ID.HasValue);
 RuleFor(tb_ProcessStep =>tb_ProcessStep.Version).MaximumLength(50).WithMessage("版本:不能超过最大长度,50.");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.Version).NotEmpty().WithMessage("版本:不能为空。");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.Name).MaximumLength(50).WithMessage("标题:不能超过最大长度,50.");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.DisplayName).MaximumLength(50).WithMessage("显示名称:不能超过最大长度,50.");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.StepNodeType).MaximumLength(50).WithMessage("节点类型:不能超过最大长度,50.");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.Description).MaximumLength(255).WithMessage("描述:不能超过最大长度,255.");
 RuleFor(tb_ProcessStep =>tb_ProcessStep.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
       	
           	
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

