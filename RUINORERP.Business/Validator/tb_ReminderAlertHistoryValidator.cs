
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/12/2025 00:31:25
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
    /// 提醒信息是通过什么规则通知了什么内容给谁在什么时间。通知记录  暂时不处理验证类
    /// </summary>
    /*public partial class tb_ReminderAlertHistoryValidator:AbstractValidator<tb_ReminderAlertHistory>*/
    public partial class tb_ReminderAlertHistoryValidator:BaseValidatorGeneric<tb_ReminderAlertHistory>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ReminderAlertHistoryValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_ReminderAlertHistory =>tb_ReminderAlertHistory.AlertId).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ReminderAlertHistory =>tb_ReminderAlertHistory.User_ID).Must(CheckForeignKeyValue).WithMessage(":下拉选择值不正确。");


 RuleFor(tb_ReminderAlertHistory =>tb_ReminderAlertHistory.Message).NotEmpty().WithMessage(":不能为空。");


//***** 
 RuleFor(tb_ReminderAlertHistory =>tb_ReminderAlertHistory.ReminderBizType).NotNull().WithMessage(":不能为空。");

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

