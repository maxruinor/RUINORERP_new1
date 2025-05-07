
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 15:37:51
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
    /// 对账单明细（关联应收单） 验证类
    /// </summary>
    /*public partial class tb_FM_StatementDetailValidator:AbstractValidator<tb_FM_StatementDetail>*/
    public partial class tb_FM_StatementDetailValidator:BaseValidatorGeneric<tb_FM_StatementDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_StatementDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     //***** 
 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.StatementId).NotNull().WithMessage("对账单:不能为空。");

 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.ARAPId).Must(CheckForeignKeyValueCanNull).WithMessage("应收付款单:下拉选择值不正确。");
 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.ARAPId).NotEmpty().When(x => x.ARAPId.HasValue);


 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.IncludedLocalAmount).PrecisionScale(19,4,true).WithMessage("对账金额本币:小数位不能超过4。");

 RuleFor(x => x.IncludedForeignAmount).PrecisionScale(19,4,true).WithMessage("对账金额外币:小数位不能超过4。");

 RuleFor(tb_FM_StatementDetail =>tb_FM_StatementDetail.Summary).MaximumLength(150).WithMessage("摘要:不能超过最大长度,150.");

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

