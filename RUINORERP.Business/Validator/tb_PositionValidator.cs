
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
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
    /// 位置信息验证类
    /// </summary>
    /*public partial class tb_PositionValidator:AbstractValidator<tb_Position>*/
    public partial class tb_PositionValidator:BaseValidatorGeneric<tb_Position>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_PositionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Position =>tb_Position.Left).MaximumMixedLength(50).WithMessage("左边距:不能超过最大长度,50.");
 RuleFor(tb_Position =>tb_Position.Left).NotEmpty().WithMessage("左边距:不能为空。");

 RuleFor(tb_Position =>tb_Position.Right).MaximumMixedLength(50).WithMessage("右边距:不能超过最大长度,50.");

 RuleFor(tb_Position =>tb_Position.Bottom).MaximumMixedLength(50).WithMessage("下边距:不能超过最大长度,50.");

 RuleFor(tb_Position =>tb_Position.Top).MaximumMixedLength(50).WithMessage("上边距:不能超过最大长度,50.");

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

