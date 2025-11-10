
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:16
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
    /// 合同明细验证类
    /// </summary>
    /*public partial class tb_PO_ContractDetailValidator:AbstractValidator<tb_PO_ContractDetail>*/
    public partial class tb_PO_ContractDetailValidator:BaseValidatorGeneric<tb_PO_ContractDetail>
    {
     

     public tb_PO_ContractDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.POContractID).NotEmpty().When(x => x.POContractID.HasValue);

 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.ItemName).MaximumMixedLength(100).WithMessage("项目名称:不能超过最大长度,100.");

 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.ItemNumber).MaximumMixedLength(50).WithMessage("项目编号:不能超过最大长度,50.");


 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.Unit).MaximumMixedLength(20).WithMessage("单位:不能超过最大长度,20.");

 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.Quantity).NotEmpty().When(x => x.Quantity.HasValue);

 RuleFor(x => x.UnitPrice).PrecisionScale(19,4,true).WithMessage("单价:小数位不能超过4。");

 RuleFor(x => x.SubtotalAmount).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");


 RuleFor(x => x.TaxRate).PrecisionScale(8,2,true).WithMessage("税率:小数位不能超过2。");

 RuleFor(x => x.TaxAmount).PrecisionScale(19,4,true).WithMessage("税额:小数位不能超过4。");

 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.Remarks).MaximumMixedLength(500).WithMessage("备注:不能超过最大长度,500.");


 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_PO_ContractDetail =>tb_PO_ContractDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

