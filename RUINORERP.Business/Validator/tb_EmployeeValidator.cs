
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:29
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
    /// 员工表验证类
    /// </summary>
    public partial class tb_EmployeeValidator:AbstractValidator<tb_Employee>
    {
     public tb_EmployeeValidator() 
     {
      RuleFor(tb_Employee =>tb_Employee.DepartmentID).Must(CheckForeignKeyValue).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_Employee =>tb_Employee.Employee_NO).MaximumLength(20).WithMessage("员工编号:不能超过最大长度,20.");
 RuleFor(tb_Employee =>tb_Employee.Employee_NO).NotEmpty().WithMessage("员工编号:不能为空。");
 RuleFor(tb_Employee =>tb_Employee.Employee_Name).MaximumLength(100).WithMessage("姓名:不能超过最大长度,100.");
 RuleFor(tb_Employee =>tb_Employee.Employee_Name).NotEmpty().WithMessage("姓名:不能为空。");
 RuleFor(tb_Employee =>tb_Employee.Position).MaximumLength(20).WithMessage("职位:不能超过最大长度,20.");
 RuleFor(tb_Employee =>tb_Employee.JobTitle).MaximumLength(50).WithMessage("职称:不能超过最大长度,50.");
 RuleFor(tb_Employee =>tb_Employee.Address).MaximumLength(255).WithMessage("联络地址:不能超过最大长度,255.");
 RuleFor(tb_Employee =>tb_Employee.Email).MaximumLength(100).WithMessage("邮件:不能超过最大长度,100.");
 RuleFor(tb_Employee =>tb_Employee.Education).MaximumLength(100).WithMessage("教育程度:不能超过最大长度,100.");
 RuleFor(tb_Employee =>tb_Employee.LanguageSkills).MaximumLength(50).WithMessage("外语能力:不能超过最大长度,50.");
 RuleFor(tb_Employee =>tb_Employee.University).MaximumLength(100).WithMessage("毕业院校:不能超过最大长度,100.");
 RuleFor(tb_Employee =>tb_Employee.IDNumber).MaximumLength(30).WithMessage("身份证号:不能超过最大长度,30.");
 RuleFor(x => x.salary).PrecisionScale(19,4,true).WithMessage("工资:小数位不能超过4。");
 RuleFor(tb_Employee =>tb_Employee.Notes).MaximumLength(200).WithMessage("备注说明:不能超过最大长度,200.");
//有默认值
//有默认值
 RuleFor(tb_Employee =>tb_Employee.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Employee =>tb_Employee.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_Employee =>tb_Employee.PhoneNumber).MaximumLength(50).WithMessage("手机号:不能超过最大长度,50.");
 RuleFor(tb_Employee =>tb_Employee.BankAccount_id).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_Employee =>tb_Employee.BankAccount_id).NotEmpty().When(x => x.BankAccount_id.HasValue);
       	
           	
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

