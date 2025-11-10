
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:24
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
    /// UI表格设置验证类
    /// </summary>
    /*public partial class tb_UIGridSettingValidator:AbstractValidator<tb_UIGridSetting>*/
    public partial class tb_UIGridSettingValidator:BaseValidatorGeneric<tb_UIGridSetting>
    {
     

     public tb_UIGridSettingValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.UIMenuPID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单设置:下拉选择值不正确。");
 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.UIMenuPID).NotEmpty().When(x => x.UIMenuPID.HasValue);

 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.GridKeyName).MaximumMixedLength(255).WithMessage("表格名称:不能超过最大长度,255.");


 RuleFor(tb_UIGridSetting =>tb_UIGridSetting.GridType).MaximumMixedLength(50).WithMessage("表格类型:不能超过最大长度,50.");

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

