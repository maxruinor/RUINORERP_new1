
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/19/2024 12:18:10
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
    /// 采购商品建议验证类
    /// </summary>
    /*public partial class tb_PurGoodsRecommendDetailValidator:AbstractValidator<tb_PurGoodsRecommendDetail>*/
    public partial class tb_PurGoodsRecommendDetailValidator:BaseValidatorGeneric<tb_PurGoodsRecommendDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_PurGoodsRecommendDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.PDID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.PDID).NotEmpty().When(x => x.PDID.HasValue);

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("供应商:下拉选择值不正确。");
 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(x => x.RecommendPurPrice).PrecisionScale(19,4,true).WithMessage("建议采购价:小数位不能超过4。");

//***** 
 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.ActualRequiredQty).NotNull().WithMessage("需求数量:不能为空。");

//***** 
 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.RecommendQty).NotNull().WithMessage("建议量:不能为空。");

//***** 
 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.RequirementQty).NotNull().WithMessage("请购量:不能为空。");


 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.RefBillNO).MaximumLength(50).WithMessage("生成单号:不能超过最大长度,50.");

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.RefBillType).NotEmpty().When(x => x.RefBillType.HasValue);

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);

 RuleFor(tb_PurGoodsRecommendDetail =>tb_PurGoodsRecommendDetail.PDCID_RowID).NotEmpty().When(x => x.PDCID_RowID.HasValue);

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

