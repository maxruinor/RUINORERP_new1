
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:54
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
    /// 菜单程序集信息表验证类
    /// </summary>
    /*public partial class tb_MenuInfoValidator:AbstractValidator<tb_MenuInfo>*/
    public partial class tb_MenuInfoValidator:BaseValidatorGeneric<tb_MenuInfo>
    {
     public tb_MenuInfoValidator() 
     {
      RuleFor(tb_MenuInfo =>tb_MenuInfo.ModuleID).Must(CheckForeignKeyValueCanNull).WithMessage("模块:下拉选择值不正确。");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.ModuleID).NotEmpty().When(x => x.ModuleID.HasValue);
 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuName).MaximumLength(127).WithMessage("菜单名称:不能超过最大长度,127.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuType).MaximumLength(10).WithMessage("菜单类型:不能超过最大长度,10.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuType).NotEmpty().WithMessage("菜单类型:不能为空。");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.BIBaseForm).MaximumLength(50).WithMessage("注入基类:不能超过最大长度,50.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.BizType).NotEmpty().When(x => x.BizType.HasValue);
 RuleFor(tb_MenuInfo =>tb_MenuInfo.UIType).NotEmpty().When(x => x.UIType.HasValue);
 RuleFor(tb_MenuInfo =>tb_MenuInfo.CaptionCN).MaximumLength(125).WithMessage("中文显示:不能超过最大长度,125.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.CaptionEN).MaximumLength(125).WithMessage("英文显示:不能超过最大长度,125.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.FormName).MaximumLength(127).WithMessage("窗体名称:不能超过最大长度,127.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.ClassPath).MaximumLength(250).WithMessage("类路径:不能超过最大长度,250.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.EntityName).MaximumLength(50).WithMessage("关联实体名:不能超过最大长度,50.");
//有默认值
 RuleFor(tb_MenuInfo =>tb_MenuInfo.Parent_id).NotEmpty().When(x => x.Parent_id.HasValue);
 RuleFor(tb_MenuInfo =>tb_MenuInfo.Discription).MaximumLength(125).WithMessage("描述:不能超过最大长度,125.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuNo).MaximumLength(125).WithMessage("菜单编码:不能超过最大长度,125.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuLevel).NotEmpty().When(x => x.MenuLevel.HasValue);
 RuleFor(tb_MenuInfo =>tb_MenuInfo.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_MenuInfo =>tb_MenuInfo.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
//***** 
 RuleFor(tb_MenuInfo =>tb_MenuInfo.Sort).NotNull().WithMessage("排序:不能为空。");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.HotKey).MaximumLength(25).WithMessage("热键:不能超过最大长度,25.");
       	
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

