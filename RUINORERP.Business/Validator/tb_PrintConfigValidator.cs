﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
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
    /// 报表打印配置表验证类
    /// </summary>
    /*public partial class tb_PrintConfigValidator:AbstractValidator<tb_PrintConfig>*/
    public partial class tb_PrintConfigValidator:BaseValidatorGeneric<tb_PrintConfig>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_PrintConfigValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Config_Name).MaximumMixedLength(100).WithMessage("配置名称:不能超过最大长度,100.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Config_Name).NotEmpty().WithMessage("配置名称:不能为空。");

//***** 
 RuleFor(tb_PrintConfig =>tb_PrintConfig.BizType).NotNull().WithMessage("业务类型:不能为空。");

 RuleFor(tb_PrintConfig =>tb_PrintConfig.BizName).MaximumMixedLength(30).WithMessage("业务名称:不能超过最大长度,30.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.BizName).NotEmpty().WithMessage("业务名称:不能为空。");

 RuleFor(tb_PrintConfig =>tb_PrintConfig.PrinterName).MaximumMixedLength(200).WithMessage("打印机名称:不能超过最大长度,200.");




 RuleFor(tb_PrintConfig =>tb_PrintConfig.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_PrintConfig =>tb_PrintConfig.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


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

