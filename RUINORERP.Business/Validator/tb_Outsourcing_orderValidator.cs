﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:28
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
    /// 外发加工订单表验证类
    /// </summary>
    /*public partial class tb_Outsourcing_orderValidator:AbstractValidator<tb_Outsourcing_order>*/
    public partial class tb_Outsourcing_orderValidator:BaseValidatorGeneric<tb_Outsourcing_order>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_Outsourcing_orderValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_Outsourcing_order =>tb_Outsourcing_order.Quantity).NotNull().WithMessage(":不能为空。");

 RuleFor(x => x.Unit_price).PrecisionScale(10,2,true).WithMessage(":小数位不能超过2。");

 RuleFor(x => x.Total_amount).PrecisionScale(10,2,true).WithMessage(":小数位不能超过2。");

 RuleFor(tb_Outsourcing_order =>tb_Outsourcing_order.Status).NotEmpty().When(x => x.Status.HasValue);



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

