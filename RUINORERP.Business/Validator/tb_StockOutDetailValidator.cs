
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:20
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
    /// 出库单明细验证类
    /// </summary>
    /*public partial class tb_StockOutDetailValidator:AbstractValidator<tb_StockOutDetail>*/
    public partial class tb_StockOutDetailValidator:BaseValidatorGeneric<tb_StockOutDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_StockOutDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.MainID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

//***** 
 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Qty).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.Price).PrecisionScale(19,4,true).WithMessage("售价:小数位不能超过4。");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.Summary).MaximumMixedLength(500).WithMessage("摘要:不能超过最大长度,500.");

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(x => x.SubtotalPirceAmount).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");

 RuleFor(tb_StockOutDetail =>tb_StockOutDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

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

