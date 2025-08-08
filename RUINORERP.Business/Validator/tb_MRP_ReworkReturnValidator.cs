
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:45
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
    /// 返工退库验证类
    /// </summary>
    /*public partial class tb_MRP_ReworkReturnValidator:AbstractValidator<tb_MRP_ReworkReturn>*/
    public partial class tb_MRP_ReworkReturnValidator:BaseValidatorGeneric<tb_MRP_ReworkReturn>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_MRP_ReworkReturnValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.ReworkReturnNo).MaximumMixedLength(50).WithMessage("退库单号:不能超过最大长度,50.");
 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.ReworkReturnNo).NotEmpty().WithMessage("退库单号:不能为空。");

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("外发工厂:下拉选择值不正确。");
 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);


 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("生产部门:下拉选择值不正确。");
 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.FG_ID).NotEmpty().When(x => x.FG_ID.HasValue);

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.DeliveryBillNo).MaximumMixedLength(100).WithMessage("制令单号:不能超过最大长度,100.");

//***** 
 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.TotalQty).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.TotalReworkFee).PrecisionScale(19,4,true).WithMessage("预估费用:小数位不能超过4。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("成本金额:小数位不能超过4。");



 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.ReasonForRework).MaximumMixedLength(500).WithMessage("返工原因:不能超过最大长度,500.");



 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");



 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.CloseCaseOpinions).MaximumMixedLength(200).WithMessage("结案情况:不能超过最大长度,200.");

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);


//***** 
 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


//***** 
 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");


 RuleFor(tb_MRP_ReworkReturn =>tb_MRP_ReworkReturn.VoucherID).NotEmpty().When(x => x.VoucherID.HasValue);

           	                //long?
                //ReworkReturnID
                //tb_MRP_ReworkReturnDetail
                //RuleFor(x => x.tb_MRP_ReworkReturnDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_MRP_ReworkReturnDetails).NotNull();
                //RuleForEach(x => x.tb_MRP_ReworkReturnDetails).NotNull();
                //RuleFor(x => x.tb_MRP_ReworkReturnDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_MRP_ReworkReturnDetail> details)
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

