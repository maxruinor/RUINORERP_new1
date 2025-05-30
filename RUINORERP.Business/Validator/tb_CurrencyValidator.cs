﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:56:54
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
    /// 币别资料表-备份第一行数据后删除重建 如果不行则直接修改字段删除字段验证类
    /// </summary>
    /*public partial class tb_CurrencyValidator:AbstractValidator<tb_Currency>*/
    public partial class tb_CurrencyValidator:BaseValidatorGeneric<tb_Currency>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CurrencyValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Currency =>tb_Currency.Country).MaximumLength(25).WithMessage("国家:不能超过最大长度,25.");


 RuleFor(tb_Currency =>tb_Currency.CurrencyName).MaximumLength(10).WithMessage("币别名称:不能超过最大长度,10.");
 RuleFor(tb_Currency =>tb_Currency.CurrencyName).NotEmpty().WithMessage("币别名称:不能为空。");

 RuleFor(tb_Currency =>tb_Currency.CurrencySymbol).MaximumLength(5).WithMessage("币别符号:不能超过最大长度,5.");


//有默认值

//有默认值


 RuleFor(tb_Currency =>tb_Currency.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_Currency =>tb_Currency.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

