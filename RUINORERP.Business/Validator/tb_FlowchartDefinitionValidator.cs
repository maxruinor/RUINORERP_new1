
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:25
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
    /// 流程图定义验证类
    /// </summary>
    /*public partial class tb_FlowchartDefinitionValidator:AbstractValidator<tb_FlowchartDefinition>*/
    public partial class tb_FlowchartDefinitionValidator:BaseValidatorGeneric<tb_FlowchartDefinition>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FlowchartDefinitionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FlowchartDefinition =>tb_FlowchartDefinition.ModuleID).Must(CheckForeignKeyValueCanNull).WithMessage("模块:下拉选择值不正确。");
 RuleFor(tb_FlowchartDefinition =>tb_FlowchartDefinition.ModuleID).NotEmpty().When(x => x.ModuleID.HasValue);

 RuleFor(tb_FlowchartDefinition =>tb_FlowchartDefinition.FlowchartNo).MaximumMixedLength(50).WithMessage("流程图编号:不能超过最大长度,50.");
 RuleFor(tb_FlowchartDefinition =>tb_FlowchartDefinition.FlowchartNo).NotEmpty().WithMessage("流程图编号:不能为空。");

 RuleFor(tb_FlowchartDefinition =>tb_FlowchartDefinition.FlowchartName).MaximumMixedLength(20).WithMessage("流程图名称:不能超过最大长度,20.");
 RuleFor(tb_FlowchartDefinition =>tb_FlowchartDefinition.FlowchartName).NotEmpty().WithMessage("流程图名称:不能为空。");

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

