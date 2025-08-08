
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:06
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
    /// 售后申请单明细验证类
    /// </summary>
    /*public partial class tb_AS_AfterSaleApplyDetailValidator:AbstractValidator<tb_AS_AfterSaleApplyDetail>*/
    public partial class tb_AS_AfterSaleApplyDetailValidator:BaseValidatorGeneric<tb_AS_AfterSaleApplyDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_AfterSaleApplyDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.ASApplyID).NotNull().WithMessage("售后申请单:不能为空。");

 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");

 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.FaultDescription).MaximumMixedLength(500).WithMessage("问题描述:不能超过最大长度,500.");

//***** 
 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.InitialQuantity).NotNull().WithMessage("客户申报数量:不能为空。");

//***** 
 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.ConfirmedQuantity).NotNull().WithMessage("复核数量:不能为空。");

 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.CustomerPartNo).MaximumMixedLength(100).WithMessage("客户型号:不能超过最大长度,100.");

//***** 
 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.DeliveredQty).NotNull().WithMessage("交付数量:不能为空。");

 RuleFor(tb_AS_AfterSaleApplyDetail =>tb_AS_AfterSaleApplyDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");

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

