
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 返工退库明细验证类
    /// </summary>
    /*public partial class tb_MRP_ReworkReturnDetailValidator:AbstractValidator<tb_MRP_ReworkReturnDetail>*/
    public partial class tb_MRP_ReworkReturnDetailValidator:BaseValidatorGeneric<tb_MRP_ReworkReturnDetail>
    {
     

     public tb_MRP_ReworkReturnDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.ReworkReturnID).NotEmpty().When(x => x.ReworkReturnID.HasValue);

 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("所在仓位:下拉选择值不正确。");

 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");

 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

//***** 
 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.DeliveredQuantity).NotNull().WithMessage("已交数量:不能为空。");

 RuleFor(x => x.UnitCost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalReworkFee).PrecisionScale(19,4,true).WithMessage("预估费用小计:小数位不能超过4。");

 RuleFor(x => x.ReworkFee).PrecisionScale(19,4,true).WithMessage("预估费用:小数位不能超过4。");

 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("小计:小数位不能超过4。");

 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.CustomertModel).MaximumMixedLength(50).WithMessage("客户型号:不能超过最大长度,50.");

 RuleFor(tb_MRP_ReworkReturnDetail =>tb_MRP_ReworkReturnDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");

           	  
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

