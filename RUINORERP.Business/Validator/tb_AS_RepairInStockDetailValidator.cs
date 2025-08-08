
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:08
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
    /// 维修入库单明细验证类
    /// </summary>
    /*public partial class tb_AS_RepairInStockDetailValidator:AbstractValidator<tb_AS_RepairInStockDetail>*/
    public partial class tb_AS_RepairInStockDetailValidator:BaseValidatorGeneric<tb_AS_RepairInStockDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_RepairInStockDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.RepairInStockID).NotEmpty().When(x => x.RepairInStockID.HasValue);

 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品详情:下拉选择值不正确。");

 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.CustomerPartNo).MaximumMixedLength(50).WithMessage("客户型号:不能超过最大长度,50.");

 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");

 RuleFor(tb_AS_RepairInStockDetail =>tb_AS_RepairInStockDetail.RepairOrderDetailID).NotEmpty().When(x => x.RepairOrderDetailID.HasValue);

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

