﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/04/2025 19:45:29
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
    /// 返工入库单明细验证类
    /// </summary>
    /*public partial class tb_MRP_ReworkEntryDetailValidator:AbstractValidator<tb_MRP_ReworkEntryDetail>*/
    public partial class tb_MRP_ReworkEntryDetailValidator:BaseValidatorGeneric<tb_MRP_ReworkEntryDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_MRP_ReworkEntryDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.ReworkEntryID).NotEmpty().When(x => x.ReworkEntryID.HasValue);

 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");

 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.CustomertModel).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");

 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");

//***** 
 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.ReworkFee).PrecisionScale(19,4,true).WithMessage("返工费用:小数位不能超过4。");

 RuleFor(x => x.SubtotalReworkFee).PrecisionScale(19,4,true).WithMessage("返工费用小计:小数位不能超过4。");

 RuleFor(x => x.UnitCost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(tb_MRP_ReworkEntryDetail =>tb_MRP_ReworkEntryDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");

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
