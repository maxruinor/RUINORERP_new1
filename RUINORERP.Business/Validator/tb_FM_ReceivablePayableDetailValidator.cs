
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/04/2025 11:58:55
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
    /// 应收应付明细验证类
    /// </summary>
    /*public partial class tb_FM_ReceivablePayableDetailValidator:AbstractValidator<tb_FM_ReceivablePayableDetail>*/
    public partial class tb_FM_ReceivablePayableDetailValidator:BaseValidatorGeneric<tb_FM_ReceivablePayableDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_ReceivablePayableDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.ARAPId).NotEmpty().When(x => x.ARAPId.HasValue);

 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("产品:下拉选择值不正确。");
 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");


 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.ExpenseType_id).Must(CheckForeignKeyValueCanNull).WithMessage("费用类型:下拉选择值不正确。");
 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.ExpenseType_id).NotEmpty().When(x => x.ExpenseType_id.HasValue);

 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.ExpenseDescription).MaximumLength(150).WithMessage("费用说明:不能超过最大长度,150.");
 


 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(x => x.Quantity).PrecisionScale(10,4,true).WithMessage("数量:小数位不能超过4。");

 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.CustomerPartNo).MaximumLength(50).WithMessage("往来单位料号:不能超过最大长度,50.");


 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxLocalAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.LocalPayableAmount).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");

 RuleFor(tb_FM_ReceivablePayableDetail =>tb_FM_ReceivablePayableDetail.Summary).MaximumLength(150).WithMessage("摘要:不能超过最大长度,150.");

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

