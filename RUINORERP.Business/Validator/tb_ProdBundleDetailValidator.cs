
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:07
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
    /// 产品套装明细验证类
    /// </summary>
    /*public partial class tb_ProdBundleDetailValidator:AbstractValidator<tb_ProdBundleDetail>*/
    public partial class tb_ProdBundleDetailValidator:BaseValidatorGeneric<tb_ProdBundleDetail>
    {
     public tb_ProdBundleDetailValidator() 
     {
     //***** 
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.BundleID).NotNull().WithMessage("套装组合:不能为空。");
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");
//***** 
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.SKU).MaximumLength(40).WithMessage("SKU:不能超过最大长度,40.");
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.property).MaximumLength(127).WithMessage("子件属性:不能超过最大长度,127.");
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
//有默认值
//有默认值
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ProdBundleDetail =>tb_ProdBundleDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
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

