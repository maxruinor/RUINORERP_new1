
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:03
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
    /// 角色表验证类
    /// </summary>
    public partial class tb_RoleInfoValidator:AbstractValidator<tb_RoleInfo>
    {
     public tb_RoleInfoValidator() 
     {
      RuleFor(tb_RoleInfo =>tb_RoleInfo.RoleName).MaximumLength(50).WithMessage("角色名称:不能超过最大长度,50.");
 RuleFor(tb_RoleInfo =>tb_RoleInfo.RoleName).NotEmpty().WithMessage("角色名称:不能为空。");
 RuleFor(tb_RoleInfo =>tb_RoleInfo.Desc).MaximumLength(250).WithMessage("描述:不能超过最大长度,250.");
       	
           	
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

