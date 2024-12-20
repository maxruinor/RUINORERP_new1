
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:28
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
    /// 功能模块定义（仅限部分已经硬码并体现于菜单表中）验证类
    /// </summary>
    /*public partial class tb_ModuleDefinitionValidator:AbstractValidator<tb_ModuleDefinition>*/
    public partial class tb_ModuleDefinitionValidator:BaseValidatorGeneric<tb_ModuleDefinition>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ModuleDefinitionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ModuleDefinition =>tb_ModuleDefinition.ModuleNo).NotEmpty().WithMessage("模块编号:不能为空。");

 RuleFor(tb_ModuleDefinition =>tb_ModuleDefinition.ModuleName).MaximumLength(10).WithMessage("模块名称:不能超过最大长度,10.");
 RuleFor(tb_ModuleDefinition =>tb_ModuleDefinition.ModuleName).NotEmpty().WithMessage("模块名称:不能为空。");



 RuleFor(tb_ModuleDefinition =>tb_ModuleDefinition.IconFile_Path).MaximumLength(50).WithMessage("图标路径:不能超过最大长度,50.");

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

