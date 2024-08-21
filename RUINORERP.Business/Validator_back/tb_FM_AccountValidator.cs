
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:34:58
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
    /// 账户管理，财务系统中使用验证类
    /// </summary>
    public partial class tb_FM_AccountValidator:AbstractValidator<tb_FM_Account>
    {
     public tb_FM_AccountValidator() 
     {
      RuleFor(tb_FM_Account =>tb_FM_Account.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_Account =>tb_FM_Account.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
 RuleFor(tb_FM_Account =>tb_FM_Account.subject_id).Must(CheckForeignKeyValueCanNull).WithMessage("会计科目:下拉选择值不正确。");
 RuleFor(tb_FM_Account =>tb_FM_Account.subject_id).NotEmpty().When(x => x.subject_id.HasValue);
 RuleFor(tb_FM_Account =>tb_FM_Account.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage("币种:下拉选择值不正确。");
 RuleFor(tb_FM_Account =>tb_FM_Account.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);
 RuleFor(tb_FM_Account =>tb_FM_Account.account_name).MaximumLength(50).WithMessage("账户名称:不能超过最大长度,50.");
 RuleFor(tb_FM_Account =>tb_FM_Account.account_No).MaximumLength(100).WithMessage("账号:不能超过最大长度,100.");
 RuleFor(tb_FM_Account =>tb_FM_Account.account_type).NotEmpty().When(x => x.account_type.HasValue);
 RuleFor(tb_FM_Account =>tb_FM_Account.Bank).MaximumLength(30).WithMessage("所属银行:不能超过最大长度,30.");
 RuleFor(x => x.OpeningBalance).PrecisionScale(19,4,true).WithMessage("初始余额:小数位不能超过4。");
 RuleFor(x => x.CurrentBalance).PrecisionScale(19,4,true).WithMessage("当前余额:小数位不能超过4。");
       	
           	
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
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_PrePaymentBillDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

