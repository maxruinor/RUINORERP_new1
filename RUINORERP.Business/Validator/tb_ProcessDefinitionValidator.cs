﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 流程定义 http://www.phpheidong.com/blog/article/68471/a3129f742e5e396e3d1e/验证类
    /// </summary>
    /*public partial class tb_ProcessDefinitionValidator:AbstractValidator<tb_ProcessDefinition>*/
    public partial class tb_ProcessDefinitionValidator:BaseValidatorGeneric<tb_ProcessDefinition>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ProcessDefinitionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Step_Id).Must(CheckForeignKeyValueCanNull).WithMessage("流程定义:下拉选择值不正确。");
 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Step_Id).NotEmpty().When(x => x.Step_Id.HasValue);

 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Version).MaximumLength(25).WithMessage("版本:不能超过最大长度,25.");
 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Version).NotEmpty().WithMessage("版本:不能为空。");

 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Title).MaximumLength(25).WithMessage("标题:不能超过最大长度,25.");

 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Color).MaximumLength(25).WithMessage("颜色:不能超过最大长度,25.");

 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Icon).MaximumLength(125).WithMessage("图标:不能超过最大长度,125.");

 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Description).MaximumLength(127).WithMessage("描述:不能超过最大长度,127.");

 RuleFor(tb_ProcessDefinition =>tb_ProcessDefinition.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");

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

