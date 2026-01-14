
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/24/2025 17:01:21
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
    /// 验证类
    /// </summary>
    /*public partial class tb_ProcessNavigationValidator:AbstractValidator<tb_ProcessNavigation>*/
    public partial class tb_ProcessNavigationValidator:BaseValidatorGeneric<tb_ProcessNavigation>
    {
     

     public tb_ProcessNavigationValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.ProcessNavName).MaximumMixedLength(200).WithMessage(":不能超过最大长度,200.");

 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.Description).MaximumMixedLength(500).WithMessage(":不能超过最大长度,500.");

//***** 
 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.Version).NotNull().WithMessage(":不能为空。");



 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.ModuleID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.ModuleID).NotEmpty().When(x => x.ModuleID.HasValue);

//***** 
 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.NavigationLevel).NotNull().WithMessage("层级深度（别名：HierarchyLevel）:不能为空。");

 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.ParentNavigationID).NotEmpty().When(x => x.ParentNavigationID.HasValue);

//***** 
 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.HierarchyLevel).NotNull().WithMessage(":不能为空。");

//***** 
 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.SortOrder).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.CreateUserID).NotEmpty().When(x => x.CreateUserID.HasValue);

//有默认值


 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.Category).MaximumMixedLength(100).WithMessage(":不能超过最大长度,100.");

 RuleFor(tb_ProcessNavigation =>tb_ProcessNavigation.Tags).MaximumMixedLength(300).WithMessage(":不能超过最大长度,300.");



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

