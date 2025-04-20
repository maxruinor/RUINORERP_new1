
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:10
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
    /// UI表格设置验证类
    /// </summary>
    /*public partial class tb_UIGridSettingValidator:AbstractValidator<tb_UIGridSetting>*/
    public partial class tb_UIGridSettingValidator:BaseValidatorGeneric<tb_UIGridSetting>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_UIGridSettingValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.UIMenuPID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单设置:下拉选择值不正确。");
 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.UIMenuPID).NotEmpty().When(x => x.UIMenuPID.HasValue);

 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.GridKeyName).MaximumLength(127).WithMessage("表格名称:不能超过最大长度,127.");


 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.GridType).MaximumLength(25).WithMessage("表格类型:不能超过最大长度,25.");

 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.ColumnsMode).NotEmpty().When(x => x.ColumnsMode.HasValue);


 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

