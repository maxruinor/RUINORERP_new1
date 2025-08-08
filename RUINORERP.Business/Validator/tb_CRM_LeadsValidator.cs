
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:19
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
    /// 线索机会-询盘验证类
    /// </summary>
    /*public partial class tb_CRM_LeadsValidator:AbstractValidator<tb_CRM_Leads>*/
    public partial class tb_CRM_LeadsValidator:BaseValidatorGeneric<tb_CRM_Leads>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CRM_LeadsValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Employee_ID).Must(CheckForeignKeyValue).WithMessage("收集人:下拉选择值不正确。");

//***** 
 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.LeadsStatus).NotNull().WithMessage("线索状态:不能为空。");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.wwSocialTools).MaximumMixedLength(200).WithMessage("其他/IM工具:不能超过最大长度,200.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.SocialTools).MaximumMixedLength(200).WithMessage("旺旺/IM工具:不能超过最大长度,200.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.CustomerName).MaximumMixedLength(100).WithMessage("客户名/线索名:不能超过最大长度,100.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.GetCustomerSource).MaximumMixedLength(250).WithMessage("获客来源:不能超过最大长度,250.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.InterestedProducts).MaximumMixedLength(50).WithMessage("兴趣产品:不能超过最大长度,50.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Contact_Name).MaximumMixedLength(50).WithMessage("联系人姓名:不能超过最大长度,50.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Contact_Phone).MaximumMixedLength(50).WithMessage("电话:不能超过最大长度,50.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Contact_Email).MaximumMixedLength(100).WithMessage("邮箱:不能超过最大长度,100.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Position).MaximumMixedLength(50).WithMessage("职位:不能超过最大长度,50.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.SalePlatform).MaximumMixedLength(50).WithMessage("销售平台:不能超过最大长度,50.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Address).MaximumMixedLength(255).WithMessage("地址:不能超过最大长度,255.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Website).MaximumMixedLength(255).WithMessage("网址:不能超过最大长度,255.");

 RuleFor(tb_CRM_Leads =>tb_CRM_Leads.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");


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

