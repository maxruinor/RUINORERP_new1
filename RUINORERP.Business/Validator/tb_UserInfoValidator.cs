
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:36
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
    /// 用户表验证类
    /// </summary>
    public partial class tb_UserInfoValidator:AbstractValidator<tb_UserInfo>
    {
     public tb_UserInfoValidator() 
     {
      RuleFor(tb_UserInfo =>tb_UserInfo.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_UserInfo =>tb_UserInfo.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_UserInfo =>tb_UserInfo.UserName).MaximumLength(255).WithMessage("用户名:不能超过最大长度,255.");
 RuleFor(tb_UserInfo =>tb_UserInfo.UserName).NotEmpty().WithMessage("用户名:不能为空。");
 RuleFor(tb_UserInfo =>tb_UserInfo.Password).MaximumLength(255).WithMessage("密码:不能超过最大长度,255.");
//有默认值
//有默认值
 RuleFor(tb_UserInfo =>tb_UserInfo.Notes).MaximumLength(100).WithMessage("备注说明:不能超过最大长度,100.");
 RuleFor(tb_UserInfo =>tb_UserInfo.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_UserInfo =>tb_UserInfo.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
           	
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

