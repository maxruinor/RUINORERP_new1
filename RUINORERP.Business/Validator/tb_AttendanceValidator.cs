
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:11
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
    /// 考勤表验证类
    /// </summary>
    /*public partial class tb_AttendanceValidator:AbstractValidator<tb_Attendance>*/
    public partial class tb_AttendanceValidator:BaseValidatorGeneric<tb_Attendance>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AttendanceValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Attendance =>tb_Attendance.badgenumber).MaximumMixedLength(50).WithMessage(":不能超过最大长度,50.");

 RuleFor(tb_Attendance =>tb_Attendance.username).MaximumMixedLength(50).WithMessage("姓名:不能超过最大长度,50.");

 RuleFor(tb_Attendance =>tb_Attendance.deptname).MaximumMixedLength(60).WithMessage("部门:不能超过最大长度,60.");

 RuleFor(tb_Attendance =>tb_Attendance.sDate).MaximumMixedLength(100).WithMessage("开始时间:不能超过最大长度,100.");

 RuleFor(tb_Attendance =>tb_Attendance.stime).MaximumMixedLength(255).WithMessage("时间组:不能超过最大长度,255.");






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

