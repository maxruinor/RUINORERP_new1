﻿
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
    /// 合同明细验证类
    /// </summary>
    /*public partial class tb_ContractDetailValidator:AbstractValidator<tb_ContractDetail>*/
    public partial class tb_ContractDetailValidator:BaseValidatorGeneric<tb_ContractDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ContractDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ContractDetail =>tb_ContractDetail.ContractID).NotEmpty().When(x => x.ContractID.HasValue);

 RuleFor(tb_ContractDetail =>tb_ContractDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_ContractDetail =>tb_ContractDetail.Qty).NotEmpty().When(x => x.Qty.HasValue);

 RuleFor(x => x.Price).PrecisionScale(19,4,true).WithMessage("售价:小数位不能超过4。");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(tb_ContractDetail =>tb_ContractDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");

//***** 
 RuleFor(tb_ContractDetail =>tb_ContractDetail.SubtotalQty).NotNull().WithMessage("数量小计:不能为空。");

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(x => x.SubtotalPirceAmount).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");

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

