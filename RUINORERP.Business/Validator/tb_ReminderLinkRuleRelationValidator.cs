
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2026 23:59:00
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
    /// 链路与规则关联表验证类
    /// </summary>
    /*public partial class tb_ReminderLinkRuleRelationValidator:AbstractValidator<tb_ReminderLinkRuleRelation>*/
    public partial class tb_ReminderLinkRuleRelationValidator:BaseValidatorGeneric<tb_ReminderLinkRuleRelation>
    {
     

     public tb_ReminderLinkRuleRelationValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ReminderLinkRuleRelation =>tb_ReminderLinkRuleRelation.LinkId).Must(CheckForeignKeyValueCanNull).WithMessage("链路ID:下拉选择值不正确。");
 RuleFor(tb_ReminderLinkRuleRelation =>tb_ReminderLinkRuleRelation.LinkId).NotEmpty().When(x => x.LinkId.HasValue);

 RuleFor(tb_ReminderLinkRuleRelation =>tb_ReminderLinkRuleRelation.RuleId).Must(CheckForeignKeyValueCanNull).WithMessage("提醒规则:下拉选择值不正确。");
 RuleFor(tb_ReminderLinkRuleRelation =>tb_ReminderLinkRuleRelation.RuleId).NotEmpty().When(x => x.RuleId.HasValue);


 RuleFor(tb_ReminderLinkRuleRelation =>tb_ReminderLinkRuleRelation.Created_by).NotEmpty().When(x => x.Created_by.HasValue);

           	      
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

