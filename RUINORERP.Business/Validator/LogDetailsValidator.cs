
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/14/2025 20:40:52
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
    /// 验证类
    /// </summary>
    /*public partial class LogDetailsValidator:AbstractValidator<LogDetails>*/
    public partial class LogDetailsValidator:BaseValidatorGeneric<LogDetails>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public LogDetailsValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     

 RuleFor(LogDetails =>LogDetails.LogThread).MaximumLength(255).WithMessage(":不能超过最大长度,255.");
 RuleFor(LogDetails =>LogDetails.LogThread).NotEmpty().WithMessage(":不能为空。");

 RuleFor(LogDetails =>LogDetails.LogLevel).MaximumLength(50).WithMessage(":不能超过最大长度,50.");
 RuleFor(LogDetails =>LogDetails.LogLevel).NotEmpty().WithMessage(":不能为空。");

 RuleFor(LogDetails =>LogDetails.LogLogger).MaximumLength(255).WithMessage(":不能超过最大长度,255.");
 RuleFor(LogDetails =>LogDetails.LogLogger).NotEmpty().WithMessage(":不能为空。");

 RuleFor(LogDetails =>LogDetails.LogActionClick).MaximumLength(1000).WithMessage(":不能超过最大长度,1000.");

 RuleFor(LogDetails =>LogDetails.LogMessage).MaximumLength(-1).WithMessage(":不能超过最大长度,-1.");
 RuleFor(LogDetails =>LogDetails.LogMessage).NotEmpty().WithMessage(":不能为空。");

 RuleFor(LogDetails =>LogDetails.UserName).MaximumLength(255).WithMessage(":不能超过最大长度,255.");

 RuleFor(LogDetails =>LogDetails.UserIP).MaximumLength(22).WithMessage(":不能超过最大长度,22.");

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

