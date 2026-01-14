
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:15
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
    /// 菜单权限表验证类
    /// </summary>
    /*public partial class tb_P4MenuValidator:AbstractValidator<tb_P4Menu>*/
    public partial class tb_P4MenuValidator:BaseValidatorGeneric<tb_P4Menu>
    {
     

     public tb_P4MenuValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_P4Menu =>tb_P4Menu.MenuID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单:下拉选择值不正确。");
 RuleFor(tb_P4Menu =>tb_P4Menu.MenuID).NotEmpty().When(x => x.MenuID.HasValue);

 RuleFor(tb_P4Menu =>tb_P4Menu.RoleID).Must(CheckForeignKeyValueCanNull).WithMessage("角色:下拉选择值不正确。");
 RuleFor(tb_P4Menu =>tb_P4Menu.RoleID).NotEmpty().When(x => x.RoleID.HasValue);

 RuleFor(tb_P4Menu =>tb_P4Menu.ModuleID).Must(CheckForeignKeyValueCanNull).WithMessage("模块:下拉选择值不正确。");
 RuleFor(tb_P4Menu =>tb_P4Menu.ModuleID).NotEmpty().When(x => x.ModuleID.HasValue);



 RuleFor(tb_P4Menu =>tb_P4Menu.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_P4Menu =>tb_P4Menu.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

           	  
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

