
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:11
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
    /// 财务审计日志验证类
    /// </summary>
    /*public partial class tb_FM_AuditLogsValidator:AbstractValidator<tb_FM_AuditLogs>*/
    public partial class tb_FM_AuditLogsValidator:BaseValidatorGeneric<tb_FM_AuditLogs>
    {
     

     public tb_FM_AuditLogsValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.UserName).MaximumMixedLength(255).WithMessage("用户名:不能超过最大长度,255.");
 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.UserName).NotEmpty().WithMessage("用户名:不能为空。");


 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.ActionType).MaximumMixedLength(50).WithMessage("动作:不能超过最大长度,50.");

 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.ObjectType).NotEmpty().When(x => x.ObjectType.HasValue);

 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.ObjectId).NotEmpty().When(x => x.ObjectId.HasValue);

 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.ObjectNo).MaximumMixedLength(50).WithMessage("单据编号:不能超过最大长度,50.");

 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.OldState).MaximumMixedLength(100).WithMessage("操作前状态:不能超过最大长度,100.");

 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.NewState).MaximumMixedLength(100).WithMessage("操作后状态:不能超过最大长度,100.");


 RuleFor(tb_FM_AuditLogs =>tb_FM_AuditLogs.Notes).MaximumMixedLength(8000).WithMessage("备注说明:不能超过最大长度,8000.");

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

