
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:12:15
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
    /// 线索机会-询盘验证类
    /// </summary>
    /*public partial class tb_CRM_LeadsValidator:AbstractValidator<tb_CRM_Leads>*/
    public partial class tb_CRM_LeadsValidator:BaseValidatorGeneric<tb_CRM_Leads>
    {
     public tb_CRM_LeadsValidator() 
     {
      RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Employee_ID).Must(CheckForeignKeyValue).WithMessage("收集人:下拉选择值不正确。");
//***** 
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.LeadsStatus).NotNull().WithMessage("线索状态:不能为空。");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.SocialTools).MaximumLength(100).WithMessage("社交工具:不能超过最大长度,100.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.CustomerName).MaximumLength(50).WithMessage("客户名/线索名:不能超过最大长度,50.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.GetCustomerSource).MaximumLength(125).WithMessage("获客来源:不能超过最大长度,125.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.InterestedProducts).MaximumLength(25).WithMessage("兴趣产品:不能超过最大长度,25.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Contact_Name).MaximumLength(25).WithMessage("联系人姓名:不能超过最大长度,25.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Contact_Phone).MaximumLength(25).WithMessage("电话:不能超过最大长度,25.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Contact_Email).MaximumLength(50).WithMessage("邮箱:不能超过最大长度,50.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Position).MaximumLength(25).WithMessage("职位:不能超过最大长度,25.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.SalePlatform).MaximumLength(25).WithMessage("销售平台:不能超过最大长度,25.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Address).MaximumLength(127).WithMessage("地址:不能超过最大长度,127.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Website).MaximumLength(127).WithMessage("网址:不能超过最大长度,127.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

