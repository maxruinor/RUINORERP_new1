
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:22
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
    /// 提醒规则验证类
    /// </summary>
    /*public partial class tb_ReminderRuleValidator:AbstractValidator<tb_ReminderRule>*/
    public partial class tb_ReminderRuleValidator:BaseValidatorGeneric<tb_ReminderRule>
    {
     

     public tb_ReminderRuleValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ReminderRule =>tb_ReminderRule.RuleName).MaximumMixedLength(100).WithMessage("规则名称:不能超过最大长度,100.");
 RuleFor(tb_ReminderRule =>tb_ReminderRule.RuleName).NotEmpty().WithMessage("规则名称:不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.RuleEngineType).NotNull().WithMessage("引擎类型:不能为空。");

 RuleFor(tb_ReminderRule =>tb_ReminderRule.Description).MaximumMixedLength(500).WithMessage("规则描述:不能超过最大长度,500.");
 RuleFor(tb_ReminderRule =>tb_ReminderRule.Description).NotEmpty().WithMessage("规则描述:不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.ReminderBizType).NotNull().WithMessage("业务类型:不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.CheckIntervalByMinutes).NotNull().WithMessage("检测频率(分钟):不能为空。");

//***** 
 RuleFor(tb_ReminderRule =>tb_ReminderRule.ReminderPriority).NotNull().WithMessage("优先级:不能为空。");


 RuleFor(tb_ReminderRule =>tb_ReminderRule.NotifyChannels).NotEmpty().WithMessage("通知渠道:不能为空。");



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

