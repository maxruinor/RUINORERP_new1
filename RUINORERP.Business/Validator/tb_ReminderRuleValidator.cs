
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/04/2025 18:55:01
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
    /// 提醒规则验证类
    /// </summary>
    /*public partial class tb_ReminderRuleValidator:AbstractValidator<tb_ReminderRule>*/
    public partial class tb_ReminderRuleValidator:BaseValidatorGeneric<tb_ReminderRule>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ReminderRuleValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ReminderRule =>tb_ReminderRule.RuleName).MaximumLength(50).WithMessage("规则名称:不能超过最大长度,50.");
 RuleFor(tb_ReminderRule =>tb_ReminderRule.RuleName).NotEmpty().WithMessage("规则名称:不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.RuleEngineType).NotNull().WithMessage("引擎类型:不能为空。");

 RuleFor(tb_ReminderRule =>tb_ReminderRule.Description).MaximumLength(250).WithMessage("规则描述:不能超过最大长度,250.");
 RuleFor(tb_ReminderRule =>tb_ReminderRule.Description).NotEmpty().WithMessage("规则描述:不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.ReminderBizType).NotNull().WithMessage("业务类型:不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.CheckIntervalByMinutes).NotNull().WithMessage("检测频率(分钟):不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.ReminderPriority).NotNull().WithMessage("优先级:不能为空。");


//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.NotifyChannel).NotNull().WithMessage("通知渠道:不能为空。");



 RuleFor(tb_ReminderRule =>tb_ReminderRule.Condition).NotEmpty().WithMessage("规则条件:不能为空。");

 RuleFor(tb_ReminderRule =>tb_ReminderRule.NotifyRecipientNames).NotEmpty().WithMessage("通知接收人员:不能为空。");

 RuleFor(tb_ReminderRule =>tb_ReminderRule.NotifyRecipients).NotEmpty().WithMessage("通知接收人员ID:不能为空。");

 RuleFor(tb_ReminderRule =>tb_ReminderRule.NotifyMessage).NotEmpty().WithMessage("通知消息模板:不能为空。");

 RuleFor(tb_ReminderRule =>tb_ReminderRule.JsonConfig).NotEmpty().WithMessage("扩展JSON配置:不能为空。");


//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.Created_by).NotNull().WithMessage("创建人:不能为空。");


//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.Modified_by).NotNull().WithMessage("修改人:不能为空。");

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

