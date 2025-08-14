
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:34
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
    /// 价格调整单明细验证类
    /// </summary>
    /*public partial class tb_FM_PriceAdjustmentDetailValidator:AbstractValidator<tb_FM_PriceAdjustmentDetail>*/
    public partial class tb_FM_PriceAdjustmentDetailValidator:BaseValidatorGeneric<tb_FM_PriceAdjustmentDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_PriceAdjustmentDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.AdjustId).NotEmpty().When(x => x.AdjustId.HasValue);

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("产品:下拉选择值不正确。");
 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.OriginalUnitPrice).PrecisionScale(19,4,true).WithMessage("原始单价:小数位不能超过4。");

 RuleFor(x => x.AdjustedUnitPrice).PrecisionScale(19,4,true).WithMessage("调整后单价:小数位不能超过4。");

 RuleFor(x => x.DiffUnitPrice).PrecisionScale(19,4,true).WithMessage("差异单价:小数位不能超过4。");

 RuleFor(x => x.Quantity).PrecisionScale(10,4,true).WithMessage("数量:小数位不能超过4。");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.CustomerPartNo).MaximumMixedLength(100).WithMessage("往来单位料号:不能超过最大长度,100.");

 RuleFor(x => x.SubtotalDiffLocalAmount).PrecisionScale(19,4,true).WithMessage("差异金额小计:小数位不能超过4。");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.Description).MaximumMixedLength(300).WithMessage("描述:不能超过最大长度,300.");

 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxDiffLocalAmount).PrecisionScale(19,4,true).WithMessage("税额差异:小数位不能超过4。");

 RuleFor(x => x.TaxSubtotalDiffLocalAmount).PrecisionScale(19,4,true).WithMessage("税额差异小计:小数位不能超过4。");

 RuleFor(tb_FM_PriceAdjustmentDetail =>tb_FM_PriceAdjustmentDetail.Summary).MaximumMixedLength(300).WithMessage("摘要:不能超过最大长度,300.");

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

