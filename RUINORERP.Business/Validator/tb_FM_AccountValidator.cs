
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:05
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
    /// 付款账号管理验证类
    /// </summary>
    /*public partial class tb_FM_AccountValidator:AbstractValidator<tb_FM_Account>*/
    public partial class tb_FM_AccountValidator:BaseValidatorGeneric<tb_FM_Account>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_AccountValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_Account =>tb_FM_Account.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("部门:下拉选择值不正确。");
 RuleFor(tb_FM_Account =>tb_FM_Account.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_FM_Account =>tb_FM_Account.Subject_id).Must(CheckForeignKeyValueCanNull).WithMessage("会计科目:下拉选择值不正确。");
 RuleFor(tb_FM_Account =>tb_FM_Account.Subject_id).NotEmpty().When(x => x.Subject_id.HasValue);

 RuleFor(tb_FM_Account =>tb_FM_Account.Currency_ID).Must(CheckForeignKeyValueCanNull).WithMessage("币种:下拉选择值不正确。");
 RuleFor(tb_FM_Account =>tb_FM_Account.Currency_ID).NotEmpty().When(x => x.Currency_ID.HasValue);

 RuleFor(tb_FM_Account =>tb_FM_Account.Account_name).MaximumLength(25).WithMessage("账户名称:不能超过最大长度,25.");

 RuleFor(tb_FM_Account =>tb_FM_Account.Account_No).MaximumLength(50).WithMessage("账号:不能超过最大长度,50.");

 RuleFor(tb_FM_Account =>tb_FM_Account.Account_type).NotEmpty().When(x => x.Account_type.HasValue);

 RuleFor(tb_FM_Account =>tb_FM_Account.Bank).MaximumLength(15).WithMessage("所属银行:不能超过最大长度,15.");

 RuleFor(x => x.OpeningBalance).PrecisionScale(19,4,true).WithMessage("初始余额:小数位不能超过4。");

 RuleFor(x => x.CurrentBalance).PrecisionScale(19,4,true).WithMessage("当前余额:小数位不能超过4。");

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

