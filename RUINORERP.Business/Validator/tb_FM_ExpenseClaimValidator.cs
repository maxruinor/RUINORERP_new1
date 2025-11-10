
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:11
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
    /// 费用报销单验证类
    /// </summary>
    /*public partial class tb_FM_ExpenseClaimValidator:AbstractValidator<tb_FM_ExpenseClaim>*/
    public partial class tb_FM_ExpenseClaimValidator:BaseValidatorGeneric<tb_FM_ExpenseClaim>
    {
     

     public tb_FM_ExpenseClaimValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.ClaimNo).MaximumMixedLength(30).WithMessage("单据编号:不能超过最大长度,30.");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.ClaimNo).NotEmpty().WithMessage("单据编号:不能为空。");

 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Employee_ID).Must(CheckForeignKeyValue).WithMessage("报销人:下拉选择值不正确。");

 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.PayeeInfoID).Must(CheckForeignKeyValueCanNull).WithMessage("收款信息:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);


 RuleFor(x => x.ClaimAmount).PrecisionScale(19,4,true).WithMessage("报销金额:小数位不能超过4。");


 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Notes).MaximumMixedLength(1000).WithMessage("备注:不能超过最大长度,1000.");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.UntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");


 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.ApprovalOpinions).MaximumMixedLength(500).WithMessage("审批意见:不能超过最大长度,500.");

 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




//***** 
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.CloseCaseImagePath).MaximumMixedLength(300).WithMessage("结案凭证:不能超过最大长度,300.");

 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.CloseCaseOpinions).MaximumMixedLength(200).WithMessage("结案意见:不能超过最大长度,200.");

           	                //long?
                //ClaimMainID
                //tb_FM_ExpenseClaimDetail
                //RuleFor(x => x.tb_FM_ExpenseClaimDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_ExpenseClaimDetails).NotNull();
                //RuleForEach(x => x.tb_FM_ExpenseClaimDetails).NotNull();
                //RuleFor(x => x.tb_FM_ExpenseClaimDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                    Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_ExpenseClaimDetail> details)
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

