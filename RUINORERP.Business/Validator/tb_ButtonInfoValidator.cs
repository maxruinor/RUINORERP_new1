
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:28
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
    /// 字段信息表验证类
    /// </summary>
    public partial class tb_ButtonInfoValidator:AbstractValidator<tb_ButtonInfo>
    {
     public tb_ButtonInfoValidator() 
     {
      RuleFor(tb_ButtonInfo =>tb_ButtonInfo.MenuID).Must(CheckForeignKeyValueCanNull).WithMessage("所属菜单:下拉选择值不正确。");
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.MenuID).NotEmpty().When(x => x.MenuID.HasValue);
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.BtnName).MaximumLength(255).WithMessage("按钮名称:不能超过最大长度,255.");
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.BtnText).MaximumLength(250).WithMessage("按钮文本:不能超过最大长度,250.");
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.HotKey).MaximumLength(50).WithMessage("热键:不能超过最大长度,50.");
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.FormName).MaximumLength(255).WithMessage("窗体名称:不能超过最大长度,255.");
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.ClassPath).MaximumLength(500).WithMessage("类路径:不能超过最大长度,500.");
//有默认值
 RuleFor(tb_ButtonInfo =>tb_ButtonInfo.Notes).MaximumLength(200).WithMessage("备注:不能超过最大长度,200.");
       	
           	
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

