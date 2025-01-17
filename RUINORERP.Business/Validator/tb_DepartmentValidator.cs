
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/16/2025 11:47:57
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
    /// 部门表是否分层验证类
    /// </summary>
    /*public partial class tb_DepartmentValidator:AbstractValidator<tb_Department>*/
    public partial class tb_DepartmentValidator:BaseValidatorGeneric<tb_Department>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_DepartmentValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Department =>tb_Department.ID).Must(CheckForeignKeyValue).WithMessage("公司:下拉选择值不正确。");

 RuleFor(tb_Department =>tb_Department.DepartmentCode).NotEmpty().WithMessage("部门代号:不能为空。");

 RuleFor(tb_Department =>tb_Department.DepartmentName).MaximumLength(127).WithMessage("部门名称:不能超过最大长度,127.");
 RuleFor(tb_Department =>tb_Department.DepartmentName).NotEmpty().WithMessage("部门名称:不能为空。");

 RuleFor(tb_Department =>tb_Department.TEL).MaximumLength(10).WithMessage("电话:不能超过最大长度,10.");

 RuleFor(tb_Department =>tb_Department.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");

 RuleFor(tb_Department =>tb_Department.Director).MaximumLength(10).WithMessage("责任人:不能超过最大长度,10.");

           	        Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_OtherExpenseDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_ExpenseClaimDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

