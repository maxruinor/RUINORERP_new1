﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:22
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
    /// 业务编号规则验证类
    /// </summary>
    /*public partial class tb_sys_BillNoRuleValidator:AbstractValidator<tb_sys_BillNoRule>*/
    public partial class tb_sys_BillNoRuleValidator:BaseValidatorGeneric<tb_sys_BillNoRule>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_sys_BillNoRuleValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RuleName).MaximumMixedLength(200).WithMessage("规则名称:不能超过最大长度,200.");
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RuleName).NotEmpty().WithMessage("规则名称:不能为空。");


 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Prefix).MaximumMixedLength(200).WithMessage("前缀:不能超过最大长度,200.");
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.Prefix).NotEmpty().WithMessage("前缀:不能为空。");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.DateFormat).NotNull().WithMessage("日期格式:不能为空。");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.SequenceLength).NotNull().WithMessage("流水号长度:不能为空。");


 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.RedisKeyPattern).MaximumMixedLength(3000).WithMessage("Redis键模式:不能超过最大长度,3000.");

//***** 
 RuleFor(tb_sys_BillNoRule =>tb_sys_BillNoRule.ResetMode).NotNull().WithMessage("重置模式:不能为空。");

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

