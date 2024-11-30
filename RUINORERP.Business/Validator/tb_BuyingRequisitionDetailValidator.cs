
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:43:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

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
     public tb_BuyingRequisitionDetailValidator() 
     {
      RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.PuRequisition_ID).NotEmpty().When(x => x.PuRequisition_ID.HasValue);
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.ActualRequiredQty).NotNull().WithMessage("需求数量:不能为空。");
//***** 
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.Quantity).NotNull().WithMessage("请购数量:不能为空。");
 RuleFor(x => x.EstimatedPrice).PrecisionScale(19,6,true).WithMessage("预估价格:小数位不能超过6。");
//***** 
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.DeliveredQuantity).NotNull().WithMessage("已交数量:不能为空。");
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.Purpose).MaximumLength(250).WithMessage("用途:不能超过最大长度,250.");
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.Purpose).NotEmpty().WithMessage("用途:不能为空。");
 RuleFor(tb_BuyingRequisitionDetail =>tb_BuyingRequisitionDetail.Notes).MaximumLength(500).WithMessage("备注:不能超过最大长度,500.");
       	
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

