
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:28
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
    /// 领料单明细验证类
    /// </summary>
    /*public partial class tb_MaterialRequisitionDetailValidator:AbstractValidator<tb_MaterialRequisitionDetail>*/
    public partial class tb_MaterialRequisitionDetailValidator:BaseValidatorGeneric<tb_MaterialRequisitionDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_MaterialRequisitionDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
//***** 
 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.MR_ID).NotNull().WithMessage("领料单:不能为空。");

 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");

 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");

//***** 
 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.ShouldSendQty).NotNull().WithMessage("应发数:不能为空。");

//***** 
 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.ActualSentQty).NotNull().WithMessage("实发数:不能为空。");

//***** 
 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.CanQuantity).NotNull().WithMessage("可发数:不能为空。");

 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");

 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.CustomerPartNo).MaximumLength(25).WithMessage("客户型号:不能超过最大长度,25.");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.Price).PrecisionScale(19,4,true).WithMessage("价格:小数位不能超过4。");

 RuleFor(x => x.SubtotalPrice).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");

 RuleFor(x => x.SubtotalCost).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

//***** 
 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.ReturnQty).NotNull().WithMessage("退回数量:不能为空。");

//***** 
 RuleFor(tb_MaterialRequisitionDetail =>tb_MaterialRequisitionDetail.ManufacturingOrderDetailRowID).NotNull().WithMessage(":不能为空。");

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

