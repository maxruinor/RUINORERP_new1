
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:46
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
    /// 生产计划表 应该是分析来的。可能来自于生产需求单，比方系统根据库存情况分析销售情况等也也可以手动。也可以程序分析验证类
    /// </summary>
    public partial class tb_ProductionPlanValidator:AbstractValidator<tb_ProductionPlan>
    {
     public tb_ProductionPlanValidator() 
     {
      RuleFor(tb_ProductionPlan =>tb_ProductionPlan.PDID).Must(CheckForeignKeyValueCanNull).WithMessage("生产需求:下拉选择值不正确。");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.PDID).NotEmpty().When(x => x.PDID.HasValue);
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.PPNo).MaximumLength(100).WithMessage("计划单号:不能超过最大长度,100.");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.PPNo).NotEmpty().WithMessage("计划单号:不能为空。");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.DepartmentID).Must(CheckForeignKeyValue).WithMessage("需求部门:下拉选择值不正确。");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");
//***** 
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.TotalQuantity).NotNull().WithMessage("总数量:不能为空。");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.DataStatus).MaximumLength(10).WithMessage("单据状态:不能超过最大长度,10.");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.DataStatus).NotEmpty().WithMessage("单据状态:不能为空。");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.ApprovalOpinions).MaximumLength(200).WithMessage("审批意见:不能超过最大长度,200.");
       	
           	                //long
                //PPID
                //tb_ProductionPlanDetail
                RuleFor(c => c.tb_ProductionPlanDetails).NotNull();
                RuleForEach(x => x.tb_ProductionPlanDetails).NotNull();
                //RuleFor(x => x.tb_ProductionPlanDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_ProductionPlanDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProductionPlanDetail> details)
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

