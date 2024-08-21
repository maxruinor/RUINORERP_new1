
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:29
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
    /// 费用报销单验证类
    /// </summary>
    public partial class tb_FM_ExpenseClaimValidator:AbstractValidator<tb_FM_ExpenseClaim>
    {
     public tb_FM_ExpenseClaimValidator() 
     {
      RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.ClaimNo).MaximumLength(30).WithMessage("单据编号:不能超过最大长度,30.");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.ClaimNo).NotEmpty().WithMessage("单据编号:不能为空。");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Employee_ID).Must(CheckForeignKeyValue).WithMessage("制单人:下拉选择值不正确。");
 RuleFor(x => x.ClaimlAmount).PrecisionScale(19,4,true).WithMessage("报销金额:小数位不能超过4。");
 RuleFor(x => x.ApprovedAmount).PrecisionScale(19,4,true).WithMessage("核准金额:小数位不能超过4。");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");
 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");
 RuleFor(x => x.TaxRate).PrecisionScale(8,2,true).WithMessage("税率:小数位不能超过2。");
 RuleFor(x => x.UntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.ApprovalOpinions).MaximumLength(500).WithMessage("审批意见:不能超过最大长度,500.");
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_FM_ExpenseClaim =>tb_FM_ExpenseClaim.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
       	
           	                //long?
                //ClaimMainID
                //tb_FM_ExpenseClaimDetail
                RuleFor(c => c.tb_FM_ExpenseClaimDetails).NotNull();
                RuleForEach(x => x.tb_FM_ExpenseClaimDetails).NotNull();
                //RuleFor(x => x.tb_FM_ExpenseClaimDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
                RuleFor(x => x.tb_FM_ExpenseClaimDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
        
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

