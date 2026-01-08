
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:21
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
    /// 采购退货入库单验证类
    /// </summary>
    /*public partial class tb_PurReturnEntryValidator:AbstractValidator<tb_PurReturnEntry>*/
    public partial class tb_PurReturnEntryValidator:BaseValidatorGeneric<tb_PurReturnEntry>
    {
     

     public tb_PurReturnEntryValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.PurReEntryNo).MaximumMixedLength(50).WithMessage("入库单号:不能超过最大长度,50.");

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("供应商:下拉选择值不正确。");

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款类型:下拉选择值不正确。");
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.PurEntryRe_ID).Must(CheckForeignKeyValueCanNull).WithMessage("采购退货单:下拉选择值不正确。");
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.PurEntryRe_ID).NotEmpty().When(x => x.PurEntryRe_ID.HasValue);

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.PurEntryReNo).MaximumMixedLength(50).WithMessage("采购退货单号:不能超过最大长度,50.");

//***** 
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Currency_ID).NotNull().WithMessage("币别:不能为空。");

//***** 
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.TotalQty).NotNull().WithMessage("合计数量:不能为空。");

 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("合计税额:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("合计金额:小数位不能超过4。");


 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.ShippingWay).MaximumMixedLength(50).WithMessage("发货方式:不能超过最大长度,50.");

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.TrackNo).MaximumMixedLength(50).WithMessage("物流单号:不能超过最大长度,50.");




 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");




 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);


 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);


//***** 
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


//***** 
 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");


 RuleFor(tb_PurReturnEntry =>tb_PurReturnEntry.VoucherNO).MaximumMixedLength(50).WithMessage("凭证号码:不能超过最大长度,50.");

           	                //long
                //PurReEntry_ID
                //tb_PurReturnEntryDetail
                //RuleFor(x => x.tb_PurReturnEntryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_PurReturnEntryDetails).NotNull();
                //RuleForEach(x => x.tb_PurReturnEntryDetails).NotNull();
                //RuleFor(x => x.tb_PurReturnEntryDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_PurReturnEntryDetail> details)
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

