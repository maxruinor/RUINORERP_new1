
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:20
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
    /// 客户关系配置表验证类
    /// </summary>
    /*public partial class tb_CRMConfigValidator:AbstractValidator<tb_CRMConfig>*/
    public partial class tb_CRMConfigValidator:BaseValidatorGeneric<tb_CRMConfig>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CRMConfigValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.CS_NewCustToLeadsCustDays).NotNull().WithMessage("新客转潜客天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.CS_SleepingCustomerDays).NotNull().WithMessage("定义休眠客户天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.CS_LostCustomersDays).NotNull().WithMessage("定义流失客户天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.CS_ActiveCustomers).NotNull().WithMessage("定义活跃客户天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.LS_ConvCustHasFollowUpDays).NotNull().WithMessage("转换为客户后有跟进天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.LS_ConvCustNoTransDays).NotNull().WithMessage("转换为客户后无成交天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.LS_ConvCustLostDays).NotNull().WithMessage("转换为客户后已丢失天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.NoFollToPublicPoolDays).NotNull().WithMessage("无跟进转换到公海的天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.CustomerNoOrderDays).NotNull().WithMessage("客户无返单间隔提醒天数:不能为空。");

//***** 
 RuleFor(tb_CRMConfig =>tb_CRMConfig.CustomerNoFollowUpDays).NotNull().WithMessage("客户无回访间隔提醒天数:不能为空。");


 RuleFor(tb_CRMConfig =>tb_CRMConfig.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_CRMConfig =>tb_CRMConfig.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

