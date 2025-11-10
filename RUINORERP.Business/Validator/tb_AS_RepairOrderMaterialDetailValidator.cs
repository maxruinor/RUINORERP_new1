
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:08
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
    /// 维修物料明细表验证类
    /// </summary>
    /*public partial class tb_AS_RepairOrderMaterialDetailValidator:AbstractValidator<tb_AS_RepairOrderMaterialDetail>*/
    public partial class tb_AS_RepairOrderMaterialDetailValidator:BaseValidatorGeneric<tb_AS_RepairOrderMaterialDetail>
    {
     

     public tb_AS_RepairOrderMaterialDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_AS_RepairOrderMaterialDetail =>tb_AS_RepairOrderMaterialDetail.RepairOrderID).NotEmpty().When(x => x.RepairOrderID.HasValue);

 RuleFor(tb_AS_RepairOrderMaterialDetail =>tb_AS_RepairOrderMaterialDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_AS_RepairOrderMaterialDetail =>tb_AS_RepairOrderMaterialDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

 RuleFor(tb_AS_RepairOrderMaterialDetail =>tb_AS_RepairOrderMaterialDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("维修物料:下拉选择值不正确。");

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(x => x.ShouldSendQty).PrecisionScale(10,3,true).WithMessage("需求数量:小数位不能超过3。");

 RuleFor(x => x.ActualSentQty).PrecisionScale(10,3,true).WithMessage("实发数量:小数位不能超过3。");

 RuleFor(x => x.SubtotalTransAmount).PrecisionScale(19,4,true).WithMessage("小计:小数位不能超过4。");

 RuleFor(x => x.TaxRate).PrecisionScale(5,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.SubtotalTaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(x => x.SubtotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");

 RuleFor(tb_AS_RepairOrderMaterialDetail =>tb_AS_RepairOrderMaterialDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");

 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalCost).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");



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

