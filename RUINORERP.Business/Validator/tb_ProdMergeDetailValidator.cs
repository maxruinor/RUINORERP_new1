
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:18
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
    /// 组合单明细验证类
    /// </summary>
    /*public partial class tb_ProdMergeDetailValidator:AbstractValidator<tb_ProdMergeDetail>*/
    public partial class tb_ProdMergeDetailValidator:BaseValidatorGeneric<tb_ProdMergeDetail>
    {
     

     public tb_ProdMergeDetailValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_ProdMergeDetail =>tb_ProdMergeDetail.MergeID).NotNull().WithMessage("组合单:不能为空。");

 RuleFor(tb_ProdMergeDetail =>tb_ProdMergeDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

 RuleFor(tb_ProdMergeDetail =>tb_ProdMergeDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("子件:下拉选择值不正确。");

 RuleFor(tb_ProdMergeDetail =>tb_ProdMergeDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");

//***** 
 RuleFor(tb_ProdMergeDetail =>tb_ProdMergeDetail.Qty).NotNull().WithMessage("子件数量:不能为空。");

 RuleFor(tb_ProdMergeDetail =>tb_ProdMergeDetail.Summary).MaximumMixedLength(1000).WithMessage("摘要:不能超过最大长度,1000.");

 RuleFor(x => x.UnitCost).PrecisionScale(19,4,true).WithMessage("单位成本:小数位不能超过4。");

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

