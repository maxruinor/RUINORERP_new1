
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
    /*public partial class tb_ProcessNavigationNodeValidator:AbstractValidator<tb_ProcessNavigationNode>*/
    public partial class tb_ProcessNavigationNodeValidator:BaseValidatorGeneric<tb_ProcessNavigationNode>
    {
     

     public tb_ProcessNavigationNodeValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.ProcessNavID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.NodeCode).MaximumMixedLength(100).WithMessage(":不能超过最大长度,100.");

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.NodeName).MaximumMixedLength(200).WithMessage(":不能超过最大长度,200.");

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.Description).MaximumMixedLength(500).WithMessage(":不能超过最大长度,500.");

//***** 
 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.BusinessType).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.MenuID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.MenuID).NotEmpty().When(x => x.MenuID.HasValue);

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.ModuleID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.ModuleID).NotEmpty().When(x => x.ModuleID.HasValue);

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.ChildNavigationID).NotEmpty().When(x => x.ChildNavigationID.HasValue);

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.FormName).MaximumMixedLength(200).WithMessage(":不能超过最大长度,200.");

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.ClassPath).MaximumMixedLength(300).WithMessage(":不能超过最大长度,300.");

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.NodeColor).MaximumMixedLength(50).WithMessage(":不能超过最大长度,50.");

 

 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.NodeType).MaximumMixedLength(50).WithMessage(":不能超过最大长度,50.");

//***** 
 RuleFor(tb_ProcessNavigationNode =>tb_ProcessNavigationNode.SortOrder).NotNull().WithMessage(":不能为空。");



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

