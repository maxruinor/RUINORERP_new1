
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 字段信息表验证类
    /// </summary>
    /*public partial class tb_FieldInfoValidator:AbstractValidator<tb_FieldInfo>*/
    public partial class tb_FieldInfoValidator:BaseValidatorGeneric<tb_FieldInfo>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FieldInfoValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FieldInfo =>tb_FieldInfo.MenuID).Must(CheckForeignKeyValueCanNull).WithMessage("菜单:下拉选择值不正确。");
 RuleFor(tb_FieldInfo =>tb_FieldInfo.MenuID).NotEmpty().When(x => x.MenuID.HasValue);

 RuleFor(tb_FieldInfo =>tb_FieldInfo.EntityName).MaximumLength(25).WithMessage("实体名称:不能超过最大长度,25.");

 RuleFor(tb_FieldInfo =>tb_FieldInfo.FieldName).MaximumLength(25).WithMessage("字段名称:不能超过最大长度,25.");

 RuleFor(tb_FieldInfo =>tb_FieldInfo.FieldText).MaximumLength(25).WithMessage("字段显示:不能超过最大长度,25.");

 RuleFor(tb_FieldInfo =>tb_FieldInfo.ClassPath).MaximumLength(250).WithMessage("类路径:不能超过最大长度,250.");


//有默认值

 RuleFor(tb_FieldInfo =>tb_FieldInfo.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");


 RuleFor(tb_FieldInfo =>tb_FieldInfo.ChildEntityName).MaximumLength(25).WithMessage("子表名称:不能超过最大长度,25.");

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

