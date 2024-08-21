
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 模块权限表（暂时没有使用，逻辑上用菜单的代替了）验证类
    /// </summary>
    public partial class tb_P4ModuleValidator:AbstractValidator<tb_P4Module>
    {
     public tb_P4ModuleValidator() 
     {
      RuleFor(tb_P4Module =>tb_P4Module.ModuleID).Must(CheckForeignKeyValueCanNull).WithMessage("模块:下拉选择值不正确。");
 RuleFor(tb_P4Module =>tb_P4Module.ModuleID).NotEmpty().When(x => x.ModuleID.HasValue);
 RuleFor(tb_P4Module =>tb_P4Module.RoleID).Must(CheckForeignKeyValueCanNull).WithMessage("角色:下拉选择值不正确。");
 RuleFor(tb_P4Module =>tb_P4Module.RoleID).NotEmpty().When(x => x.RoleID.HasValue);
 RuleFor(tb_P4Module =>tb_P4Module.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_P4Module =>tb_P4Module.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
           	
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

