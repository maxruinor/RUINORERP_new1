
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/20/2025 22:58:12
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据验证类
    /// </summary>
    /*public partial class tb_UIMenuPersonalizationValidator:AbstractValidator<tb_UIMenuPersonalization>*/
    public partial class tb_UIMenuPersonalizationValidator:BaseValidatorGeneric<tb_UIMenuPersonalization>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_UIMenuPersonalizationValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.MenuID).Must(CheckForeignKeyValue).WithMessage("关联菜单:下拉选择值不正确。");

 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.UserPersonalizedID).Must(CheckForeignKeyValueCanNull).WithMessage("用户角色设置:下拉选择值不正确。");
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.UserPersonalizedID).NotEmpty().When(x => x.UserPersonalizedID.HasValue);

//***** 
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.QueryConditionCols).NotNull().WithMessage("条件显示列数量:不能为空。");




 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.BaseWidth).NotEmpty().When(x => x.BaseWidth.HasValue);

//***** 
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.Sort).NotNull().WithMessage("基准宽度:不能为空。");



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

