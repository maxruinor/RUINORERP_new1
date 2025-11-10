
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
    /// 费用报销单明细验证类
    /// </summary>
    /*public partial class tb_FM_ExpenseClaimDetailValidator:AbstractValidator<tb_FM_ExpenseClaimDetail>*/
    public partial class tb_FM_ExpenseClaimDetailValidator:BaseValidatorGeneric<tb_FM_ExpenseClaimDetail>
    {
     

     public tb_FM_ExpenseClaimDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.ClaimMainID).NotEmpty().When(x => x.ClaimMainID.HasValue);

 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.ClaimName).MaximumMixedLength(300).WithMessage("事由:不能超过最大长度,300.");

 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("报销部门:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.ExpenseType_id).Must(CheckForeignKeyValueCanNull).WithMessage("费用类型:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.ExpenseType_id).NotEmpty().When(x => x.ExpenseType_id.HasValue);

 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.Account_id).Must(CheckForeignKeyValueCanNull).WithMessage("支付账号:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.Account_id).NotEmpty().When(x => x.Account_id.HasValue);

 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.Subject_id).Must(CheckForeignKeyValueCanNull).WithMessage("会计科目:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.Subject_id).NotEmpty().When(x => x.Subject_id.HasValue);

 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("所属项目:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);


 RuleFor(x => x.SingleAmount).PrecisionScale(19,4,true).WithMessage("单项总金额:小数位不能超过4。");


 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.Summary).MaximumMixedLength(500).WithMessage("摘要:不能超过最大长度,500.");

 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.UntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");

 RuleFor(tb_FM_ExpenseClaimDetail =>tb_FM_ExpenseClaimDetail.EvidenceImagePath).MaximumMixedLength(300).WithMessage("凭证图:不能超过最大长度,300.");

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

