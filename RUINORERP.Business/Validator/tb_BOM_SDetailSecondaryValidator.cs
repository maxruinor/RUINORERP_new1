
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
    /// 标准物料表次级产出明细验证类
    /// </summary>
    /*public partial class tb_BOM_SDetailSecondaryValidator:AbstractValidator<tb_BOM_SDetailSecondary>*/
    public partial class tb_BOM_SDetailSecondaryValidator:BaseValidatorGeneric<tb_BOM_SDetailSecondary>
    {
     

     public tb_BOM_SDetailSecondaryValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_BOM_SDetailSecondary =>tb_BOM_SDetailSecondary.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("产品详情:下拉选择值不正确。");
 RuleFor(tb_BOM_SDetailSecondary =>tb_BOM_SDetailSecondary.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_BOM_SDetailSecondary =>tb_BOM_SDetailSecondary.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);

 RuleFor(x => x.Qty).PrecisionScale(8,4,true).WithMessage("数量:小数位不能超过4。");

 RuleFor(x => x.Scale).PrecisionScale(8,4,true).WithMessage("比例:小数位不能超过4。");

 RuleFor(tb_BOM_SDetailSecondary =>tb_BOM_SDetailSecondary.property).MaximumMixedLength(255).WithMessage("副产品属性:不能超过最大长度,255.");

 RuleFor(x => x.UnitCost).PrecisionScale(8,4,true).WithMessage("单位成本:小数位不能超过4。");

 RuleFor(x => x.SubtotalCost).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");

 RuleFor(tb_BOM_SDetailSecondary =>tb_BOM_SDetailSecondary.Remarks).MaximumMixedLength(200).WithMessage("备注说明:不能超过最大长度,200.");

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

