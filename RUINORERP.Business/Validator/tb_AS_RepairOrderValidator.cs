
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:42
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
    /// 维修工单  工时费 材料费验证类
    /// </summary>
    /*public partial class tb_AS_RepairOrderValidator:AbstractValidator<tb_AS_RepairOrder>*/
    public partial class tb_AS_RepairOrderValidator:BaseValidatorGeneric<tb_AS_RepairOrder>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_RepairOrderValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.RepairOrderNo).MaximumLength(25).WithMessage("维修工单号:不能超过最大长度,25.");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ASApplyID).Must(CheckForeignKeyValueCanNull).WithMessage("售后申请单:下拉选择值不正确。");
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ASApplyID).NotEmpty().When(x => x.ASApplyID.HasValue);

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ASApplyNo).MaximumLength(25).WithMessage("售后申请编号:不能超过最大长度,25.");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人员:下拉选择值不正确。");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目小组:下拉选择值不正确。");
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("所属客户:下拉选择值不正确。");
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.RepairStatus).NotEmpty().When(x => x.RepairStatus.HasValue);

//***** 
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.PayStatus).NotNull().WithMessage("付款状态:不能为空。");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.Paytype_ID).Must(CheckForeignKeyValue).WithMessage("付款方式:下拉选择值不正确。");

//***** 
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.TotalQty).NotNull().WithMessage("总数量:不能为空。");

//***** 
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.TotalDeliveredQty).NotNull().WithMessage("交付数量:不能为空。");

 RuleFor(x => x.LaborCost).PrecisionScale(19,4,true).WithMessage("总人工成本:小数位不能超过4。");

 RuleFor(x => x.TotalMaterialAmount).PrecisionScale(19,4,true).WithMessage("总材料费用:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总费用:小数位不能超过4。");

 RuleFor(x => x.CustomerPaidAmount).PrecisionScale(19,4,true).WithMessage("客户支付金额:小数位不能超过4。");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ExpenseAllocationMode).NotEmpty().When(x => x.ExpenseAllocationMode.HasValue);

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ExpenseBearerType).NotEmpty().When(x => x.ExpenseBearerType.HasValue);





 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

//***** 
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(tb_AS_RepairOrder =>tb_AS_RepairOrder.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");

 RuleFor(x => x.TotalMaterialCost).PrecisionScale(19,4,true).WithMessage("总材料成本:小数位不能超过4。");

           	                //long?
                //RepairOrderID
                //tb_AS_RepairOrderMaterialDetail
                //RuleFor(x => x.tb_AS_RepairOrderMaterialDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_AS_RepairOrderMaterialDetails).NotNull();
                //RuleForEach(x => x.tb_AS_RepairOrderMaterialDetails).NotNull();
                //RuleFor(x => x.tb_AS_RepairOrderMaterialDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                            //long?
                //RepairOrderID
                //tb_AS_RepairOrderDetail
                //RuleFor(x => x.tb_AS_RepairOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_AS_RepairOrderDetails).NotNull();
                //RuleForEach(x => x.tb_AS_RepairOrderDetails).NotNull();
                //RuleFor(x => x.tb_AS_RepairOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_AS_RepairOrderMaterialDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_AS_RepairOrderDetail> details)
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

