
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:42
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
    /// 其它费用记录表，账户管理，财务系统中使用,像基础资料一样单表操作简单验证类
    /// </summary>
    /*public partial class tb_FM_OtherExpenseValidator:AbstractValidator<tb_FM_OtherExpense>*/
    public partial class tb_FM_OtherExpenseValidator:BaseValidatorGeneric<tb_FM_OtherExpense>
    {
     public tb_FM_OtherExpenseValidator() 
     {
      RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.ExpenseNo).MaximumLength(15).WithMessage("单据编号:不能超过最大长度,15.");
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.ExpenseNo).NotEmpty().WithMessage("单据编号:不能为空。");
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.Employee_ID).Must(CheckForeignKeyValue).WithMessage("制单人:下拉选择值不正确。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");
//有默认值
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.Notes).MaximumLength(500).WithMessage("备注:不能超过最大长度,500.");
 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");
 RuleFor(x => x.TaxRate).PrecisionScale(8,2,true).WithMessage("税率:小数位不能超过2。");
 RuleFor(x => x.UntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
 RuleFor(x => x.ApprovedAmount).PrecisionScale(19,4,true).WithMessage(":小数位不能超过4。");
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_FM_OtherExpense =>tb_FM_OtherExpense.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);
       	
           	                //long
                //ExpenseMainID
                //tb_FM_OtherExpenseDetail
                //RuleFor(x => x.tb_FM_OtherExpenseDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_FM_OtherExpenseDetails).NotNull();
                //RuleForEach(x => x.tb_FM_OtherExpenseDetails).NotNull();
                //RuleFor(x => x.tb_FM_OtherExpenseDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
                Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_OtherExpenseDetail> details)
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

