﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2024 18:45:29
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 审计日志表验证类
    /// </summary>
    public partial class tb_AuditLogsValidator : AbstractValidator<tb_AuditLogs>
    {
        public tb_AuditLogsValidator()
        {
            RuleFor(tb_AuditLogs => tb_AuditLogs.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("员工信息:下拉选择值不正确。");
            RuleFor(tb_AuditLogs => tb_AuditLogs.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
            RuleFor(tb_AuditLogs => tb_AuditLogs.UserName).MaximumLength(127).WithMessage("用户名:不能超过最大长度,127.");
            RuleFor(tb_AuditLogs => tb_AuditLogs.UserName).NotEmpty().WithMessage("用户名:不能为空。");
            RuleFor(tb_AuditLogs => tb_AuditLogs.ActionType).MaximumLength(25).WithMessage("动作:不能超过最大长度,25.");
            RuleFor(tb_AuditLogs => tb_AuditLogs.ObjectType).NotEmpty().When(x => x.ObjectType.HasValue);
            RuleFor(tb_AuditLogs => tb_AuditLogs.ObjectId).NotEmpty().When(x => x.ObjectId.HasValue);
            RuleFor(tb_AuditLogs => tb_AuditLogs.ObjectNo).MaximumLength(25).WithMessage("单据编号:不能超过最大长度,25.");
            RuleFor(tb_AuditLogs => tb_AuditLogs.OldState).MaximumLength(50).WithMessage("操作前状态:不能超过最大长度,50.");
            RuleFor(tb_AuditLogs => tb_AuditLogs.NewState).MaximumLength(50).WithMessage("操作后状态:不能超过最大长度,50.");
            RuleFor(tb_AuditLogs => tb_AuditLogs.Notes).MaximumLength(50).WithMessage("备注说明:不能超过最大长度,50.");


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
