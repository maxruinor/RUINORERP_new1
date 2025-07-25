
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:43
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
    /// 维修工单明细验证类
    /// </summary>
    /*public partial class tb_AS_RepairOrderDetailValidator:AbstractValidator<tb_AS_RepairOrderDetail>*/
    public partial class tb_AS_RepairOrderDetailValidator:BaseValidatorGeneric<tb_AS_RepairOrderDetail>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_AS_RepairOrderDetailValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.RepairOrderID).NotEmpty().When(x => x.RepairOrderID.HasValue);

 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品详情:下拉选择值不正确。");

 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");

//***** 
 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

//***** 
 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.DeliveredQty).NotNull().WithMessage("交付数量:不能为空。");

 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.RepairContent).MaximumLength(250).WithMessage("维修内容:不能超过最大长度,250.");

 RuleFor(tb_AS_RepairOrderDetail =>tb_AS_RepairOrderDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");

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

