
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:01
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
    /// 生产需求分析目标对象明细验证类
    /// </summary>
    /*public partial class tb_ProductionDemandTargetDetailValidator:AbstractValidator<tb_ProductionDemandTargetDetail>*/
    public partial class tb_ProductionDemandTargetDetailValidator:BaseValidatorGeneric<tb_ProductionDemandTargetDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ProductionDemandTargetDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.PDID).NotEmpty().When(x => x.PDID.HasValue);

 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");

 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.BOM_ID).Must(CheckForeignKeyValue).WithMessage("配方名称:下拉选择值不正确。");

//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.NeedQuantity).NotNull().WithMessage("需求数量:不能为空。");


//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.BookInventory).NotNull().WithMessage("账面库存:不能为空。");

//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.AvailableStock).NotNull().WithMessage("可用库存:不能为空。");

//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.InTransitInventory).NotNull().WithMessage("在途库存:不能为空。");

//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.MakeProcessInventory).NotNull().WithMessage("在制库存:不能为空。");

//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.SaleQty).NotNull().WithMessage("拟销售量:不能为空。");

//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.NotIssueMaterialQty).NotNull().WithMessage("未发料量:不能为空。");

 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");

//***** 
 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.PPCID).NotNull().WithMessage(":不能为空。");

 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.Specifications).MaximumMixedLength(1000).WithMessage("规格:不能超过最大长度,1000.");

 RuleFor(tb_ProductionDemandTargetDetail =>tb_ProductionDemandTargetDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

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

