
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:14
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
    /// 返工入库验证类
    /// </summary>
    /*public partial class tb_MRP_ReworkEntryValidator:AbstractValidator<tb_MRP_ReworkEntry>*/
    public partial class tb_MRP_ReworkEntryValidator:BaseValidatorGeneric<tb_MRP_ReworkEntry>
    {
     

     public tb_MRP_ReworkEntryValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.ReworkEntryNo).MaximumMixedLength(50).WithMessage("返工入库单号:不能超过最大长度,50.");
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.ReworkEntryNo).NotEmpty().WithMessage("返工入库单号:不能为空。");

 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("外发工厂:下拉选择值不正确。");
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);


 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("生产部门:下拉选择值不正确。");
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.ReworkReturnID).Must(CheckForeignKeyValue).WithMessage("退库单:下拉选择值不正确。");

 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.ReworkReturnNo).MaximumMixedLength(50).WithMessage("退库单号:不能超过最大长度,50.");
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.ReworkReturnNo).NotEmpty().WithMessage("退库单号:不能为空。");

//***** 
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.TotalQty).NotNull().WithMessage("合计数量:不能为空。");

 RuleFor(x => x.TotalReworkFee).PrecisionScale(19,4,true).WithMessage("预估费用:小数位不能超过4。");

 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("合计成本:小数位不能超过4。");




 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");



 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);


//***** 
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


//***** 
 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");


 RuleFor(tb_MRP_ReworkEntry =>tb_MRP_ReworkEntry.VoucherNO).MaximumMixedLength(50).WithMessage("凭证号码:不能超过最大长度,50.");

           	                //long?
                //ReworkEntryID
                //tb_MRP_ReworkEntryDetail
                //RuleFor(x => x.tb_MRP_ReworkEntryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_MRP_ReworkEntryDetails).NotNull();
                //RuleForEach(x => x.tb_MRP_ReworkEntryDetails).NotNull();
                //RuleFor(x => x.tb_MRP_ReworkEntryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
     }




        private bool DetailedRecordsNotEmpty(List<tb_MRP_ReworkEntryDetail> details)
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

