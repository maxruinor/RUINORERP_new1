
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2026 23:59:01
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
    /// 提醒对象链路验证类
    /// </summary>
    /*public partial class tb_ReminderObjectLinkValidator:AbstractValidator<tb_ReminderObjectLink>*/
    public partial class tb_ReminderObjectLinkValidator:BaseValidatorGeneric<tb_ReminderObjectLink>
    {
     

     public tb_ReminderObjectLinkValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.LinkName).MaximumMixedLength(100).WithMessage("链路名称:不能超过最大长度,100.");

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.Description).MaximumMixedLength(500).WithMessage("链路描述:不能超过最大长度,500.");

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.SourceType).NotEmpty().When(x => x.SourceType.HasValue);

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.SourceValue).NotEmpty().When(x => x.SourceValue.HasValue);

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.TargetValue).NotEmpty().When(x => x.TargetValue.HasValue);

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.ActionType).NotEmpty().When(x => x.ActionType.HasValue);

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.TargetType).NotEmpty().When(x => x.TargetType.HasValue);

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.BizType).NotEmpty().When(x => x.BizType.HasValue);

 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.BillStatus).NotEmpty().When(x => x.BillStatus.HasValue);



 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ReminderObjectLink =>tb_ReminderObjectLink.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

           	      
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

