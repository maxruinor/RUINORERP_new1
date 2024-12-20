﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:26
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
    /// 流程步骤 为转移条件集合，Field为条件左参数，Operator为操作操作符如果值类型为String则表达式只能为==或者!=，Value为表达式值验证类
    /// </summary>
    /*public partial class tb_ConNodeConditionsValidator:AbstractValidator<tb_ConNodeConditions>*/
    public partial class tb_ConNodeConditionsValidator:BaseValidatorGeneric<tb_ConNodeConditions>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ConNodeConditionsValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ConNodeConditions =>tb_ConNodeConditions.Field).MaximumLength(25).WithMessage("表达式:不能超过最大长度,25.");
 RuleFor(tb_ConNodeConditions =>tb_ConNodeConditions.Field).NotEmpty().WithMessage("表达式:不能为空。");

 RuleFor(tb_ConNodeConditions =>tb_ConNodeConditions.Operator).MaximumLength(25).WithMessage("操作符:不能超过最大长度,25.");

 RuleFor(tb_ConNodeConditions =>tb_ConNodeConditions.Value).MaximumLength(25).WithMessage("表达式值:不能超过最大长度,25.");

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

