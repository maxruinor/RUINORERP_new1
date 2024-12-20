
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:26
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
    /// 跟进计划表验证类
    /// </summary>
    /*public partial class tb_CRM_FollowUpPlansValidator:AbstractValidator<tb_CRM_FollowUpPlans>*/
    public partial class tb_CRM_FollowUpPlansValidator:BaseValidatorGeneric<tb_CRM_FollowUpPlans>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CRM_FollowUpPlansValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Customer_id).Must(CheckForeignKeyValue).WithMessage("目标客户:下拉选择值不正确。");

 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Employee_ID).Must(CheckForeignKeyValue).WithMessage("执行人:下拉选择值不正确。");



//***** 
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanStatus).NotNull().WithMessage("计划状态:不能为空。");

 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanSubject).MaximumLength(100).WithMessage("计划主题:不能超过最大长度,100.");
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanSubject).NotEmpty().WithMessage("计划主题:不能为空。");

 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanContent).MaximumLength(500).WithMessage("计划内容:不能超过最大长度,500.");
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanContent).NotEmpty().WithMessage("计划内容:不能为空。");

 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");


 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


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

