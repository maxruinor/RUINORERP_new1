
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/27/2024 19:36:49
// **************************************
using System;
using SqlSugar;
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
    /// 其它费用统计分析验证类
    /// </summary>
    /*public partial class View_FM_OtherExpenseItemsValidator:AbstractValidator<View_FM_OtherExpenseItems>*/
    public partial class View_FM_OtherExpenseItemsValidator : BaseValidatorGeneric<View_FM_OtherExpenseItems>
    {
        public View_FM_OtherExpenseItemsValidator()
        {
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.ExpenseNo).MaximumLength(15).WithMessage("单据编号:不能超过最大长度,15.");

            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.Notes).MaximumLength(50).WithMessage("备注:不能超过最大长度,50.");
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.ExpenseName).MaximumLength(150).WithMessage("事由:不能超过最大长度,150.");
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.ExpenseType_id).NotEmpty().When(x => x.ExpenseType_id.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.Account_id).NotEmpty().When(x => x.Account_id.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.Subject_id).NotEmpty().When(x => x.Subject_id.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.Summary).MaximumLength(50).WithMessage("备注:不能超过最大长度,50.");
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);
            RuleFor(View_FM_OtherExpenseItems => View_FM_OtherExpenseItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);

            Initialize();
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

