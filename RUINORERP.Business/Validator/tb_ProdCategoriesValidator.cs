
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
    /// 产品类别表 与行业相关的产品分类验证类
    /// </summary>
    /*public partial class tb_ProdCategoriesValidator:AbstractValidator<tb_ProdCategories>*/
    public partial class tb_ProdCategoriesValidator:BaseValidatorGeneric<tb_ProdCategories>
    {
     

     public tb_ProdCategoriesValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProdCategories =>tb_ProdCategories.Category_name).MaximumMixedLength(50).WithMessage("类别名称:不能超过最大长度,50.");

 RuleFor(tb_ProdCategories =>tb_ProdCategories.CategoryCode).MaximumMixedLength(20).WithMessage("类别代码:不能超过最大长度,20.");

//有默认值

 RuleFor(tb_ProdCategories =>tb_ProdCategories.CategoryLevel).NotEmpty().When(x => x.CategoryLevel.HasValue);

 RuleFor(tb_ProdCategories =>tb_ProdCategories.Sort).NotEmpty().When(x => x.Sort.HasValue);

 RuleFor(tb_ProdCategories =>tb_ProdCategories.Parent_id).NotEmpty().When(x => x.Parent_id.HasValue);


 RuleFor(tb_ProdCategories =>tb_ProdCategories.Notes).MaximumMixedLength(200).WithMessage("备注:不能超过最大长度,200.");

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

