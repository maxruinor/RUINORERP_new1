
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:42
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
    /// 产品属性值表验证类
    /// </summary>
    public partial class tb_ProdPropertyValueValidator:AbstractValidator<tb_ProdPropertyValue>
    {
     public tb_ProdPropertyValueValidator() 
     {
     //***** 
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.Property_ID).NotNull().WithMessage("属性:不能为空。");
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.PropertyValueName).MaximumLength(20).WithMessage("属性值名称:不能超过最大长度,20.");
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.PropertyValueName).NotEmpty().WithMessage("属性值名称:不能为空。");
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.PropertyValueDesc).MaximumLength(50).WithMessage("属性值描述:不能超过最大长度,50.");
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.SortOrder).NotEmpty().When(x => x.SortOrder.HasValue);
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_ProdPropertyValue =>tb_ProdPropertyValue.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
       	
           	
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

