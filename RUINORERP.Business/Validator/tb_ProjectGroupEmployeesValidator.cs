
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:20
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
    /// 项目及成员关系表验证类
    /// </summary>
    /*public partial class tb_ProjectGroupEmployeesValidator:AbstractValidator<tb_ProjectGroupEmployees>*/
    public partial class tb_ProjectGroupEmployeesValidator:BaseValidatorGeneric<tb_ProjectGroupEmployees>
    {
     

     public tb_ProjectGroupEmployeesValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_ProjectGroupEmployees =>tb_ProjectGroupEmployees.ProjectGroup_ID).NotNull().WithMessage("项目组:不能为空。");

 RuleFor(tb_ProjectGroupEmployees =>tb_ProjectGroupEmployees.Employee_ID).Must(CheckForeignKeyValue).WithMessage("员工:下拉选择值不正确。");



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

