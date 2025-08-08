
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:09
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
    /// 维修领料单明细验证类
    /// </summary>
    /*public partial class tb_AS_RepairMaterialPickupDetailValidator:AbstractValidator<tb_AS_RepairMaterialPickupDetail>*/
    public partial class tb_AS_RepairMaterialPickupDetailValidator:BaseValidatorGeneric<tb_AS_RepairMaterialPickupDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_RepairMaterialPickupDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品详情:下拉选择值不正确。");

 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.RMRID).NotEmpty().When(x => x.RMRID.HasValue);

 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(x => x.ShouldSendQty).PrecisionScale(10,3,true).WithMessage("应发数:小数位不能超过3。");

 RuleFor(x => x.ActualSentQty).PrecisionScale(10,3,true).WithMessage("实发数:小数位不能超过3。");

//***** 
 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.CanQuantity).NotNull().WithMessage("可发数:不能为空。");

 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");

 RuleFor(x => x.Price).PrecisionScale(19,4,true).WithMessage("价格:小数位不能超过4。");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalPrice).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");

 RuleFor(x => x.SubtotalCost).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

//***** 
 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.ReturnQty).NotNull().WithMessage("退回数量:不能为空。");

 RuleFor(tb_AS_RepairMaterialPickupDetail =>tb_AS_RepairMaterialPickupDetail.ManufacturingOrderDetailRowID).NotEmpty().When(x => x.ManufacturingOrderDetailRowID.HasValue);

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

