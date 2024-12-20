
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:32
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
    /// 销售出库退回明细验证类
    /// </summary>
    /*public partial class tb_SaleOutReDetailValidator:AbstractValidator<tb_SaleOutReDetail>*/
    public partial class tb_SaleOutReDetailValidator:BaseValidatorGeneric<tb_SaleOutReDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_SaleOutReDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

//***** 
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.SaleOutRe_ID).NotNull().WithMessage("销售退回单:不能为空。");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");

//***** 
 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Quantity).NotNull().WithMessage("退回数量:不能为空。");

 RuleFor(x => x.TransactionPrice).PrecisionScale(19,4,true).WithMessage("成交单价:小数位不能超过4。");

 RuleFor(x => x.SubtotalTransAmount).PrecisionScale(19,4,true).WithMessage("小计:小数位不能超过4。");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");

 RuleFor(tb_SaleOutReDetail =>tb_SaleOutReDetail.CustomerPartNo).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(x => x.SubtotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");


 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.SubtotalTaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.CommissionAmount).PrecisionScale(19,4,true).WithMessage("抽成返还:小数位不能超过4。");

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

