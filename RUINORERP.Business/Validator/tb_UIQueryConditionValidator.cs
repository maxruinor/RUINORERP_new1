
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:24
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
    /// UI查询条件设置验证类
    /// </summary>
    /*public partial class tb_UIQueryConditionValidator:AbstractValidator<tb_UIQueryCondition>*/
    public partial class tb_UIQueryConditionValidator:BaseValidatorGeneric<tb_UIQueryCondition>
    {
     

     public tb_UIQueryConditionValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.UIMenuPID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单设置:下拉选择值不正确。");
 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.UIMenuPID).NotEmpty().When(x => x.UIMenuPID.HasValue);

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Caption).MaximumMixedLength(100).WithMessage("查询条件名:不能超过最大长度,100.");

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.FieldName).MaximumMixedLength(100).WithMessage("查询字段名:不能超过最大长度,100.");

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.ValueType).MaximumMixedLength(50).WithMessage("值类型:不能超过最大长度,50.");

//***** 
 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.ControlWidth).NotNull().WithMessage("控件宽度:不能为空。");

//***** 
 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Sort).NotNull().WithMessage("排序:不能为空。");


 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Default1).MaximumMixedLength(255).WithMessage("默认值1:不能超过最大长度,255.");

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Default2).MaximumMixedLength(255).WithMessage("默认值2:不能超过最大长度,255.");



//有默认值



 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.DiffDays1).NotEmpty().When(x => x.DiffDays1.HasValue);

 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.DiffDays2).NotEmpty().When(x => x.DiffDays2.HasValue);


 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_UIQueryCondition =>tb_UIQueryCondition.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

