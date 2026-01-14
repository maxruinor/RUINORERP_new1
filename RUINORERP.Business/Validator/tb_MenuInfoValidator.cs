
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:14
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
    /// 菜单程序集信息表验证类
    /// </summary>
    /*public partial class tb_MenuInfoValidator:AbstractValidator<tb_MenuInfo>*/
    public partial class tb_MenuInfoValidator:BaseValidatorGeneric<tb_MenuInfo>
    {
     

     public tb_MenuInfoValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_MenuInfo =>tb_MenuInfo.ModuleID).Must(CheckForeignKeyValueCanNull).WithMessage("模块:下拉选择值不正确。");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.ModuleID).NotEmpty().When(x => x.ModuleID.HasValue);

 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuName).MaximumMixedLength(255).WithMessage("菜单名称:不能超过最大长度,255.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuType).MaximumMixedLength(20).WithMessage("菜单类型:不能超过最大长度,20.");
 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuType).NotEmpty().WithMessage("菜单类型:不能为空。");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.UIPropertyIdentifier).MaximumMixedLength(150).WithMessage("注入业务基类:不能超过最大长度,150.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.BizInterface).MaximumMixedLength(150).WithMessage("注入业务基类:不能超过最大长度,150.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.BIBizBaseForm).MaximumMixedLength(150).WithMessage("业务接口标识:不能超过最大长度,150.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.BIBaseForm).MaximumMixedLength(100).WithMessage("注入框架基类:不能超过最大长度,100.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.BizType).NotEmpty().When(x => x.BizType.HasValue);

 RuleFor(tb_MenuInfo =>tb_MenuInfo.UIType).NotEmpty().When(x => x.UIType.HasValue);

 RuleFor(tb_MenuInfo =>tb_MenuInfo.CaptionCN).MaximumMixedLength(250).WithMessage("中文显示:不能超过最大长度,250.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.CaptionEN).MaximumMixedLength(250).WithMessage("英文显示:不能超过最大长度,250.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.FormName).MaximumMixedLength(255).WithMessage("窗体名称:不能超过最大长度,255.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.ClassPath).MaximumMixedLength(500).WithMessage("类路径:不能超过最大长度,500.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.EntityName).MaximumMixedLength(100).WithMessage("关联实体名:不能超过最大长度,100.");


//有默认值

 RuleFor(tb_MenuInfo =>tb_MenuInfo.Parent_id).NotEmpty().When(x => x.Parent_id.HasValue);

 RuleFor(tb_MenuInfo =>tb_MenuInfo.Discription).MaximumMixedLength(250).WithMessage("描述:不能超过最大长度,250.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuNo).MaximumMixedLength(250).WithMessage("菜单编码:不能超过最大长度,250.");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.MenuLevel).NotEmpty().When(x => x.MenuLevel.HasValue);


 RuleFor(tb_MenuInfo =>tb_MenuInfo.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_MenuInfo =>tb_MenuInfo.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

//***** 
 RuleFor(tb_MenuInfo =>tb_MenuInfo.Sort).NotNull().WithMessage("排序:不能为空。");

 RuleFor(tb_MenuInfo =>tb_MenuInfo.HotKey).MaximumMixedLength(50).WithMessage("热键:不能超过最大长度,50.");


           	  
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

