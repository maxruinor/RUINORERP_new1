
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:17
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
    /// 产品套装明细验证类
    /// </summary>
    /*public partial class tb_ProdBundleDetailValidator:AbstractValidator<tb_ProdBundleDetail>*/
    public partial class tb_ProdBundleDetailValidator:BaseValidatorGeneric<tb_ProdBundleDetail>
    {
     

     public tb_ProdBundleDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.BundleID).NotNull().WithMessage("套装组合:不能为空。");

 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");

//***** 
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Quantity).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.SaleUnitPrice).PrecisionScale(19,4,true).WithMessage("销售单价:小数位不能超过4。");

 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.SKU).MaximumMixedLength(80).WithMessage("SKU:不能超过最大长度,80.");

 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.property).MaximumMixedLength(255).WithMessage("子件属性:不能超过最大长度,255.");

 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Notes).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");

//有默认值

//有默认值


 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

