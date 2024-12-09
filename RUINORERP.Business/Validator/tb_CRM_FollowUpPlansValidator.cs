
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:44
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
    /// 跟进计划表验证类
    /// </summary>
    /*public partial class tb_CRM_FollowUpPlansValidator:AbstractValidator<tb_CRM_FollowUpPlans>*/
    public partial class tb_CRM_FollowUpPlansValidator:BaseValidatorGeneric<tb_CRM_FollowUpPlans>
    {
     public tb_CRM_FollowUpPlansValidator() 
     {
      RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage("客户:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("执行人:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanStartDate).MaximumLength(25).WithMessage("开始日期:不能超过最大长度,25.");
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanEndDate).MaximumLength(25).WithMessage("结束日期:不能超过最大长度,25.");
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanStatus).NotEmpty().When(x => x.PlanStatus.HasValue);
 RuleFor(tb_CRM_FollowUpPlans =>tb_CRM_FollowUpPlans.PlanContent).MaximumLength(500).WithMessage("计划内容:不能超过最大长度,500.");
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

