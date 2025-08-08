
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
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
    /// 自制成品建议验证类
    /// </summary>
    /*public partial class tb_ProduceGoodsRecommendDetailValidator:AbstractValidator<tb_ProduceGoodsRecommendDetail>*/
    public partial class tb_ProduceGoodsRecommendDetailValidator:BaseValidatorGeneric<tb_ProduceGoodsRecommendDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ProduceGoodsRecommendDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.PDID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.PDID).NotEmpty().When(x => x.PDID.HasValue);

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.ID).NotEmpty().When(x => x.ID.HasValue);

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.ParentId).NotEmpty().When(x => x.ParentId.HasValue);

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.Specifications).MaximumMixedLength(1000).WithMessage("规格:不能超过最大长度,1000.");

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.BOM_ID).Must(CheckForeignKeyValueCanNull).WithMessage("标准配方:下拉选择值不正确。");
 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(x => x.UnitCost).PrecisionScale(19,4,true).WithMessage("单位成本:小数位不能超过4。");

//***** 
 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.RequirementQty).NotNull().WithMessage("请制量:不能为空。");

//***** 
 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.RecommendQty).NotNull().WithMessage("建议量:不能为空。");

//***** 
 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.PlanNeedQty).NotNull().WithMessage("计划需求数:不能为空。");



 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.RefBillNO).MaximumMixedLength(100).WithMessage("生成单号:不能超过最大长度,100.");

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.RefBillType).NotEmpty().When(x => x.RefBillType.HasValue);

 RuleFor(tb_ProduceGoodsRecommendDetail =>tb_ProduceGoodsRecommendDetail.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);

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

