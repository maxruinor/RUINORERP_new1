
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
    /// 跟进记录表验证类
    /// </summary>
    /*public partial class tb_CRM_FollowUpRecordsValidator:AbstractValidator<tb_CRM_FollowUpRecords>*/
    public partial class tb_CRM_FollowUpRecordsValidator:BaseValidatorGeneric<tb_CRM_FollowUpRecords>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CRM_FollowUpRecordsValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Customer_id).Must(CheckForeignKeyValueCanNull).WithMessage("目标客户:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Customer_id).NotEmpty().When(x => x.Customer_id.HasValue);

 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.LeadID).Must(CheckForeignKeyValueCanNull).WithMessage("线索:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.LeadID).NotEmpty().When(x => x.LeadID.HasValue);

 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.PlanID).Must(CheckForeignKeyValueCanNull).WithMessage("跟进计划:下拉选择值不正确。");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.PlanID).NotEmpty().When(x => x.PlanID.HasValue);

 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Employee_ID).Must(CheckForeignKeyValue).WithMessage("跟进人:下拉选择值不正确。");


//***** 
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpMethod).NotNull().WithMessage("跟进方式:不能为空。");

 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpSubject).MaximumMixedLength(200).WithMessage("跟进主题:不能超过最大长度,200.");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpSubject).NotEmpty().WithMessage("跟进主题:不能为空。");

 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpContent).MaximumMixedLength(1000).WithMessage("跟进内容:不能超过最大长度,1000.");
 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpContent).NotEmpty().WithMessage("跟进内容:不能为空。");


 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.FollowUpResult).MaximumMixedLength(500).WithMessage("跟进结果:不能超过最大长度,500.");

 RuleFor(tb_CRM_FollowUpRecords =>tb_CRM_FollowUpRecords.Notes).MaximumMixedLength(1000).WithMessage("备注:不能超过最大长度,1000.");


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

