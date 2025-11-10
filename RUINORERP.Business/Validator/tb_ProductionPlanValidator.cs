
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:19
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
    /// 生产计划表 应该是分析来的。可能来自于生产需求单，比方系统根据库存情况分析销售情况等也也可以手动。也可以程序分析验证类
    /// </summary>
    /*public partial class tb_ProductionPlanValidator:AbstractValidator<tb_ProductionPlan>*/
    public partial class tb_ProductionPlanValidator:BaseValidatorGeneric<tb_ProductionPlan>
    {
     

     public tb_ProductionPlanValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.SOrder_ID).Must(CheckForeignKeyValueCanNull).WithMessage("销售单号:下拉选择值不正确。");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.SOrder_ID).NotEmpty().When(x => x.SOrder_ID.HasValue);

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.SaleOrderNo).MaximumMixedLength(50).WithMessage("销售单号:不能超过最大长度,50.");

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.PPNo).MaximumMixedLength(100).WithMessage("计划单号:不能超过最大长度,100.");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.PPNo).NotEmpty().WithMessage("计划单号:不能为空。");

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.DepartmentID).Must(CheckForeignKeyValue).WithMessage("需求部门:下拉选择值不正确。");

//***** 
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Priority).NotNull().WithMessage("紧急程度:不能为空。");

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");



//***** 
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.TotalCompletedQuantity).NotNull().WithMessage("完成数:不能为空。");

//***** 
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.TotalQuantity).NotNull().WithMessage("计划数:不能为空。");


//***** 
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.DataStatus).NotNull().WithMessage("单据状态:不能为空。");


 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");




//***** 
 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


 RuleFor(tb_ProductionPlan =>tb_ProductionPlan.CloseCaseOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");

           	                //long
                //PPID
                //tb_ProductionPlanDetail
                //RuleFor(x => x.tb_ProductionPlanDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_ProductionPlanDetails).NotNull();
                //RuleForEach(x => x.tb_ProductionPlanDetails).NotNull();
                //RuleFor(x => x.tb_ProductionPlanDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
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

