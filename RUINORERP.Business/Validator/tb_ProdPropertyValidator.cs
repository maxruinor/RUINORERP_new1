
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:57
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
    /// 产品属性表验证类
    /// </summary>
    /*public partial class tb_ProdPropertyValidator:AbstractValidator<tb_ProdProperty>*/
    public partial class tb_ProdPropertyValidator:BaseValidatorGeneric<tb_ProdProperty>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ProdPropertyValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ProdProperty =>tb_ProdProperty.PropertyName).MaximumMixedLength(20).WithMessage("属性名称:不能超过最大长度,20.");
 RuleFor(tb_ProdProperty =>tb_ProdProperty.PropertyName).NotEmpty().WithMessage("属性名称:不能为空。");

 RuleFor(tb_ProdProperty =>tb_ProdProperty.PropertyDesc).MaximumMixedLength(50).WithMessage("属性描述:不能超过最大长度,50.");

 RuleFor(tb_ProdProperty =>tb_ProdProperty.SortOrder).NotEmpty().When(x => x.SortOrder.HasValue);

 RuleFor(tb_ProdProperty =>tb_ProdProperty.InputType).MaximumMixedLength(50).WithMessage("输入类型:不能超过最大长度,50.");


 RuleFor(tb_ProdProperty =>tb_ProdProperty.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ProdProperty =>tb_ProdProperty.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


 RuleFor(tb_ProdProperty =>tb_ProdProperty.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);

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

