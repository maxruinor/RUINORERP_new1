
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/08/2025 09:57:47
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
    /// 系统配置表验证类
    /// </summary>
    /*public partial class tb_SystemConfigValidator:AbstractValidator<tb_SystemConfig>*/
    public partial class tb_SystemConfigValidator:BaseValidatorGeneric<tb_SystemConfig>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_SystemConfigValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.QtyDataPrecision).NotNull().WithMessage("数量精度:不能为空。");

//***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.TaxRateDataPrecision).NotNull().WithMessage("税率精度:不能为空。");

//***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.MoneyDataPrecision).NotNull().WithMessage("金额精度:不能为空。");

//有默认值

//***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.CostCalculationMethod).NotNull().WithMessage("成本方式:不能为空。");


//有默认值




//有默认值



 RuleFor(x => x.AutoApprovedSaleOrderAmount).PrecisionScale(19,4,true).WithMessage("自动审核销售订单金额:小数位不能超过4。");

 RuleFor(x => x.AutoApprovedPurOrderAmount).PrecisionScale(19,4,true).WithMessage("自动审核采购订单金额:小数位不能超过4。");




//有默认值

//有默认值


//有默认值

//有默认值

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

