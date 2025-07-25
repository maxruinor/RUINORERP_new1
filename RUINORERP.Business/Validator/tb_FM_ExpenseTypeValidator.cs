
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/25/2025 17:18:28
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
    /// 业务类型 报销，员工借支还款，运费验证类
    /// </summary>
    /*public partial class tb_FM_ExpenseTypeValidator:AbstractValidator<tb_FM_ExpenseType>*/
    public partial class tb_FM_ExpenseTypeValidator:BaseValidatorGeneric<tb_FM_ExpenseType>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_ExpenseTypeValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.subject_id).NotEmpty().When(x => x.subject_id.HasValue);

 RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.Expense_name).MaximumLength(25).WithMessage("费用业务名称:不能超过最大长度,25.");

//有默认值

//***** 
 RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.ReceivePaymentType).NotNull().WithMessage("收付类型:不能为空。");

 RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.Notes).MaximumLength(15).WithMessage("备注:不能超过最大长度,15.");

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

