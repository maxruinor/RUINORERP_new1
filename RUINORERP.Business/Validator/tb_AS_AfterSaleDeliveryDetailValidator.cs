
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/19/2025 17:12:37
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
    /// 售后交付明细验证类
    /// </summary>
    /*public partial class tb_AS_AfterSaleDeliveryDetailValidator:AbstractValidator<tb_AS_AfterSaleDeliveryDetail>*/
    public partial class tb_AS_AfterSaleDeliveryDetailValidator:BaseValidatorGeneric<tb_AS_AfterSaleDeliveryDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_AfterSaleDeliveryDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.ASDeliveryID).NotEmpty().When(x => x.ASDeliveryID.HasValue);

 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品详情:下拉选择值不正确。");

 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");

//***** 
 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.SaleFlagCode).MaximumLength(100).WithMessage("标识代码:不能超过最大长度,100.");

 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");

 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.CustomerPartNo).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");

 RuleFor(tb_AS_AfterSaleDeliveryDetail =>tb_AS_AfterSaleDeliveryDetail.ASApplyDetailID).NotEmpty().When(x => x.ASApplyDetailID.HasValue);

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

