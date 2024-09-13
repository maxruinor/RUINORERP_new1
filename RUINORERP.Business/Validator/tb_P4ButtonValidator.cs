
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:59
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
    /// 按钮权限表验证类
    /// </summary>
    /*public partial class tb_P4ButtonValidator:AbstractValidator<tb_P4Button>*/
    public partial class tb_P4ButtonValidator:BaseValidatorGeneric<tb_P4Button>
    {
     public tb_P4ButtonValidator() 
     {
      RuleFor(tb_P4Button =>tb_P4Button.ButtonInfo_ID).Must(CheckForeignKeyValueCanNull).WithMessage("按钮:下拉选择值不正确。");
 RuleFor(tb_P4Button =>tb_P4Button.ButtonInfo_ID).NotEmpty().When(x => x.ButtonInfo_ID.HasValue);
 RuleFor(tb_P4Button =>tb_P4Button.RoleID).Must(CheckForeignKeyValueCanNull).WithMessage("角色:下拉选择值不正确。");
 RuleFor(tb_P4Button =>tb_P4Button.RoleID).NotEmpty().When(x => x.RoleID.HasValue);
 RuleFor(tb_P4Button =>tb_P4Button.MenuID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_P4Button =>tb_P4Button.MenuID).NotEmpty().When(x => x.MenuID.HasValue);
 RuleFor(tb_P4Button =>tb_P4Button.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_P4Button =>tb_P4Button.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

