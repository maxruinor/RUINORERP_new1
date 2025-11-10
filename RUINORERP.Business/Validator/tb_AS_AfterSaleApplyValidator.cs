
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:07
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
    /// 售后申请单 -登记，评估，清单，确认。目标是维修翻新验证类
    /// </summary>
    /*public partial class tb_AS_AfterSaleApplyValidator:AbstractValidator<tb_AS_AfterSaleApply>*/
    public partial class tb_AS_AfterSaleApplyValidator:BaseValidatorGeneric<tb_AS_AfterSaleApply>
    {
     

     public tb_AS_AfterSaleApplyValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ASApplyNo).MaximumMixedLength(50).WithMessage("申请编号:不能超过最大长度,50.");
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ASApplyNo).NotEmpty().WithMessage("申请编号:不能为空。");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("申请客户:下拉选择值不正确。");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.CustomerSourceNo).MaximumMixedLength(50).WithMessage("来源单号:不能超过最大长度,50.");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.Location_ID).Must(CheckForeignKeyValue).WithMessage("售后暂存仓:下拉选择值不正确。");

//***** 
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.Priority).NotNull().WithMessage("紧急程度:不能为空。");

//***** 
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ASProcessStatus).NotNull().WithMessage("处理状态:不能为空。");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.Employee_ID).Must(CheckForeignKeyValue).WithMessage("业务员:下拉选择值不正确。");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目小组:下拉选择值不正确。");
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

//***** 
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.TotalInitialQuantity).NotNull().WithMessage("登记数量:不能为空。");

//***** 
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.TotalConfirmedQuantity).NotNull().WithMessage("复核数量:不能为空。");



 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ShippingAddress).MaximumMixedLength(500).WithMessage("收货地址:不能超过最大长度,500.");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ShippingWay).MaximumMixedLength(50).WithMessage("发货方式:不能超过最大长度,50.");



 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.RepairEvaluationOpinion).MaximumMixedLength(500).WithMessage("维修评估意见:不能超过最大长度,500.");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ExpenseAllocationMode).NotEmpty().When(x => x.ExpenseAllocationMode.HasValue);

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ExpenseBearerType).NotEmpty().When(x => x.ExpenseBearerType.HasValue);

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

//***** 
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.TotalDeliveredQty).NotNull().WithMessage("交付数量:不能为空。");



 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.ApprovalOpinions).MaximumMixedLength(255).WithMessage("审批意见:不能超过最大长度,255.");

 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

//***** 
 RuleFor(tb_AS_AfterSaleApply =>tb_AS_AfterSaleApply.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

           	                //long
                //ASApplyID
                //tb_AS_AfterSaleApplyDetail
                //RuleFor(x => x.tb_AS_AfterSaleApplyDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_AS_AfterSaleApplyDetails).NotNull();
                //RuleForEach(x => x.tb_AS_AfterSaleApplyDetails).NotNull();
                //RuleFor(x => x.tb_AS_AfterSaleApplyDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_AS_AfterSaleApplyDetail> details)
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

