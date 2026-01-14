
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:09
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
    /// 请购单明细表验证类
    /// </summary>
    /*public partial class tb_BuyingRequisitionDetailValidator:AbstractValidator<tb_BuyingRequisitionDetail>*/
    public partial class tb_BuyingRequisitionDetailValidator:BaseValidatorGeneric<tb_BuyingRequisitionDetail>
    {
     

     public tb_BuyingRequisitionDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.PuRequisition_ID).NotEmpty().When(x => x.PuRequisition_ID.HasValue);

 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");


 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.ActualRequiredQty).NotNull().WithMessage("需求数量:不能为空。");

//***** 
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.Quantity).NotNull().WithMessage("请购数量:不能为空。");

 RuleFor(x => x.EstimatedPrice).PrecisionScale(19,4,true).WithMessage("预估价格:小数位不能超过4。");

//***** 
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.DeliveredQuantity).NotNull().WithMessage("已交数量:不能为空。");

 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.Notes).MaximumMixedLength(1000).WithMessage("备注:不能超过最大长度,1000.");


           	  
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

