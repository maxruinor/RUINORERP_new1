
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/30/2024 00:18:30
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据验证类
    /// </summary>
    /*public partial class tb_UIMenuPersonalizationValidator:AbstractValidator<tb_UIMenuPersonalization>*/
    public partial class tb_UIMenuPersonalizationValidator:BaseValidatorGeneric<tb_UIMenuPersonalization>
    {
     public tb_UIMenuPersonalizationValidator() 
     {
      RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.MenuID).Must(CheckForeignKeyValue).WithMessage("关联菜单:下拉选择值不正确。");
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.UIQCID).Must(CheckForeignKeyValueCanNull).WithMessage("查询条件:下拉选择值不正确。");
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.UIQCID).NotEmpty().When(x => x.UIQCID.HasValue);
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.UIGID).Must(CheckForeignKeyValueCanNull).WithMessage("表格设置:下拉选择值不正确。");
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.UIGID).NotEmpty().When(x => x.UIGID.HasValue);
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.QueryConditionCols).NotEmpty().When(x => x.QueryConditionCols.HasValue);
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.MenuType).MaximumLength(10).WithMessage("菜单类型:不能超过最大长度,10.");
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.MenuType).NotEmpty().WithMessage("菜单类型:不能为空。");
 RuleFor(tb_UIMenuPersonalization =>tb_UIMenuPersonalization.Sort).NotEmpty().When(x => x.Sort.HasValue);
       	
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

