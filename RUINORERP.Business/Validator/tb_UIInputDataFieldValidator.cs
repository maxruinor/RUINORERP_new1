
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:24
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
    /// UI录入数据预设值表验证类
    /// </summary>
    /*public partial class tb_UIInputDataFieldValidator:AbstractValidator<tb_UIInputDataField>*/
    public partial class tb_UIInputDataFieldValidator:BaseValidatorGeneric<tb_UIInputDataField>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_UIInputDataFieldValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.UIMenuPID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单设置:下拉选择值不正确。");
 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.UIMenuPID).NotEmpty().When(x => x.UIMenuPID.HasValue);

 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.Caption).MaximumMixedLength(100).WithMessage("字段标题:不能超过最大长度,100.");

 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.FieldName).MaximumMixedLength(100).WithMessage("字段名:不能超过最大长度,100.");

 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.BelongingObjectType).MaximumMixedLength(80).WithMessage("所属实体:不能超过最大长度,80.");

 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.ValueType).MaximumMixedLength(50).WithMessage("值类型:不能超过最大长度,50.");

//***** 
 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.ControlWidth).NotNull().WithMessage("控件宽度:不能为空。");

//***** 
 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.Sort).NotNull().WithMessage("排序:不能为空。");


 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.Default1).MaximumMixedLength(255).WithMessage("默认值1:不能超过最大长度,255.");

 

 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.DiffDays1).NotEmpty().When(x => x.DiffDays1.HasValue);

 
 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_UIInputDataField =>tb_UIInputDataField.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

