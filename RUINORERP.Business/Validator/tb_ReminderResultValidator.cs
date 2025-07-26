
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/26/2025 12:18:32
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
    /// 用户接收提醒内容验证类
    /// </summary>
    /*public partial class tb_ReminderResultValidator:AbstractValidator<tb_ReminderResult>*/
    public partial class tb_ReminderResultValidator:BaseValidatorGeneric<tb_ReminderResult>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ReminderResultValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ReminderResult =>tb_ReminderResult.RuleId).Must(CheckForeignKeyValueCanNull).WithMessage("提醒规则:下拉选择值不正确。");
 RuleFor(tb_ReminderResult =>tb_ReminderResult.RuleId).NotEmpty().When(x => x.RuleId.HasValue);

//***** 
 RuleFor(tb_ReminderResult =>tb_ReminderResult.ReminderBizType).NotNull().WithMessage("提醒类型:不能为空。");


 RuleFor(tb_ReminderResult =>tb_ReminderResult.Message).MaximumLength(100).WithMessage("提醒内容:不能超过最大长度,100.");




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

