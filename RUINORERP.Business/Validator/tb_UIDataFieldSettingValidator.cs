
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/30/2025 15:54:06
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
    /// UI表单输入数据字段设置验证类
    /// </summary>
    /*public partial class tb_UIDataFieldSettingValidator:AbstractValidator<tb_UIDataFieldSetting>*/
    public partial class tb_UIDataFieldSettingValidator:BaseValidatorGeneric<tb_UIDataFieldSetting>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_UIDataFieldSettingValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.UIMenuPID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单设置:下拉选择值不正确。");
 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.UIMenuPID).NotEmpty().When(x => x.UIMenuPID.HasValue);

 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.FieldCaption).MaximumLength(50).WithMessage("字段标题:不能超过最大长度,50.");

 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.FieldName).MaximumLength(50).WithMessage("字段名称:不能超过最大长度,50.");

 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.ValueType).MaximumLength(25).WithMessage("值类型:不能超过最大长度,25.");

//***** 
 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.ControlWidth).NotNull().WithMessage("控件宽度:不能为空。");

//***** 
 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.Sort).NotNull().WithMessage("排序:不能为空。");


 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.Default1).MaximumLength(127).WithMessage("默认值1:不能超过最大长度,127.");

 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.Default2).MaximumLength(127).WithMessage("默认值2:不能超过最大长度,127.");



//有默认值



 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.DiffDays1).NotEmpty().When(x => x.DiffDays1.HasValue);

 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.DiffDays2).NotEmpty().When(x => x.DiffDays2.HasValue);


 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_UIDataFieldSetting =>tb_UIDataFieldSetting.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

