
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:28
// **************************************
using System;
﻿using SqlSugar;
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
    /// 考勤表验证类
    /// </summary>
    public partial class tb_AttendanceValidator:AbstractValidator<tb_Attendance>
    {
     public tb_AttendanceValidator() 
     {
      RuleFor(tb_Attendance =>tb_Attendance.badgenumber).MaximumLength(30).WithMessage(":不能超过最大长度,30.");
 RuleFor(tb_Attendance =>tb_Attendance.username).MaximumLength(50).WithMessage("姓名:不能超过最大长度,50.");
 RuleFor(tb_Attendance =>tb_Attendance.deptname).MaximumLength(60).WithMessage("部门:不能超过最大长度,60.");
 RuleFor(tb_Attendance =>tb_Attendance.sDate).MaximumLength(100).WithMessage("开始时间:不能超过最大长度,100.");
 RuleFor(tb_Attendance =>tb_Attendance.stime).MaximumLength(255).WithMessage("时间组:不能超过最大长度,255.");
       	
           	
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

