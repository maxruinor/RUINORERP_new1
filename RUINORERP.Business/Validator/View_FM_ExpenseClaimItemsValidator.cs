
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 01:04:32
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
    /// 费用报销统计分析验证类
    /// </summary>
    /*public partial class View_FM_ExpenseClaimItemsValidator:AbstractValidator<View_FM_ExpenseClaimItems>*/
    public partial class View_FM_ExpenseClaimItemsValidator : BaseValidatorGeneric<View_FM_ExpenseClaimItems>
    {
        public View_FM_ExpenseClaimItemsValidator()
        {
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.ClaimNo).MaximumLength(15).WithMessage("单据编号:不能超过最大长度,15.");
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.Notes).MaximumLength(50).WithMessage("备注:不能超过最大长度,50.");
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.ClaimName).MaximumLength(150).WithMessage("事由:不能超过最大长度,150.");
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.ExpenseType_id).NotEmpty().When(x => x.ExpenseType_id.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.Account_id).NotEmpty().When(x => x.Account_id.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.Subject_id).NotEmpty().When(x => x.Subject_id.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.EvidenceImagePath).MaximumLength(300).WithMessage("凭证图:不能超过最大长度,300.");
            RuleFor(View_FM_ExpenseClaimItems => View_FM_ExpenseClaimItems.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");

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

