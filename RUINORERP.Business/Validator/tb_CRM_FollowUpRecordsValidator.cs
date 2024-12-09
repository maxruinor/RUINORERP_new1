
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 21:24:00
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
    /// 跟进记录表验证类
    /// </summary>
    /*public partial class tb_CRM_FollowUpRecordsValidator:AbstractValidator<tb_CRM_FollowUpRecords>*/
    public partial class tb_CRM_FollowUpRecordsValidator:BaseValidatorGeneric<tb_CRM_FollowUpRecords>
    {
     public tb_CRM_FollowUpRecordsValidator() 
     {
      RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage("机会客户:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.LeadID).Must(CheckForeignKeyValueCanNull).WithMessage("线索:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.LeadID).NotEmpty().When(x => x.LeadID.HasValue);
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.PlanID).Must(CheckForeignKeyValueCanNull).WithMessage("跟进计划:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.PlanID).NotEmpty().When(x => x.PlanID.HasValue);
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.NextPlanID).NotEmpty().When(x => x.NextPlanID.HasValue);
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("执行人:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpMethod).NotEmpty().When(x => x.FollowUpMethod.HasValue);
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpSubject).MaximumLength(100).WithMessage("跟进主题:不能超过最大长度,100.");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpContent).MaximumLength(500).WithMessage("跟进内容:不能超过最大长度,500.");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

