
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/13/2025 18:30:18
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
    /// UI查询条件设置验证类
    /// </summary>
    /*public partial class tb_UIQueryConditionValidator:AbstractValidator<tb_UIQueryCondition>*/
    public partial class tb_UIQueryConditionValidator:BaseValidatorGeneric<tb_UIQueryCondition>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_UIQueryConditionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.UIMenuPID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单设置:下拉选择值不正确。");
 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.UIMenuPID).NotEmpty().When(x => x.UIMenuPID.HasValue);

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Caption).MaximumLength(50).WithMessage("查询条件名:不能超过最大长度,50.");

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.FieldName).MaximumLength(50).WithMessage("查询字段名:不能超过最大长度,50.");

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.ValueType).MaximumLength(25).WithMessage("值类型:不能超过最大长度,25.");

//***** 
 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Sort).NotNull().WithMessage("排序:不能为空。");


 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Default1).MaximumLength(127).WithMessage("默认值1:不能超过最大长度,127.");

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Default2).MaximumLength(127).WithMessage("默认值2:不能超过最大长度,127.");




 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.DiffDays1).NotEmpty().When(x => x.DiffDays1.HasValue);

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.DiffDays2).NotEmpty().When(x => x.DiffDays2.HasValue);

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

