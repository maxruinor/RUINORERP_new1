// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/12/2025 16:18:33
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
    /// 业务编号规则验证类
    /// </summary>
    /*public partial class tb_sys_BillNoRuleValidator:AbstractValidator<tb_sys_BillNoRule>*/
    public partial class tb_sys_BillNoRuleValidator:BaseValidatorGeneric<tb_sys_BillNoRule>
    {
     

     public tb_sys_BillNoRuleValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
        
     
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RuleName).MaximumMixedLength(200).WithMessage("规则名称:不能超过最大长度,200.");
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RuleName).NotEmpty().WithMessage("规则名称:不能为空。");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RuleType).NotNull().WithMessage("规则类型:不能为空。");


 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Prefix).MaximumMixedLength(200).WithMessage("前缀:不能超过最大长度,200.");
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Prefix).NotEmpty().WithMessage("前缀:不能为空。");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.DateFormat).NotNull().WithMessage("日期格式:不能为空。");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.SequenceLength).NotNull().WithMessage("流水号长度:不能为空。");


 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RedisKeyPattern).MaximumMixedLength(3000).WithMessage("Redis键模式:不能超过最大长度,3000.");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.StorageType).NotNull().WithMessage("存储数据库类型:不能为空。");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Priority).NotNull().WithMessage("优先级:不能为空。");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.ResetMode).NotNull().WithMessage("重置模式:不能为空。");

 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RulePattern).MaximumMixedLength(200).WithMessage("编码规则:不能超过最大长度,200.");
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RulePattern).NotEmpty().WithMessage("编码规则:不能为空。");

//有默认值

 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Description).MaximumMixedLength(200).WithMessage("规则描述:不能超过最大长度,200.");


 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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