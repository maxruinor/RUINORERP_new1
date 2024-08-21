
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2024 00:36:56
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
    /// 验证类
    /// </summary>
    public partial class View_ProdPropertyValidator:AbstractValidator<View_ProdProperty>
    {
     public View_ProdPropertyValidator() 
     {
      RuleFor(View_ProdProperty =>View_ProdProperty.ProdBaseID).NotEmpty().When(x => x.ProdBaseID.HasValue);
 RuleFor(View_ProdProperty =>View_ProdProperty.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
 RuleFor(View_ProdProperty =>View_ProdProperty.Property_ID).NotEmpty().When(x => x.Property_ID.HasValue);
 RuleFor(View_ProdProperty =>View_ProdProperty.PropertyName).MaximumLength(20).WithMessage(":不能超过最大长度,20.");
 RuleFor(View_ProdProperty =>View_ProdProperty.PropertyValueID).NotEmpty().When(x => x.PropertyValueID.HasValue);
 RuleFor(View_ProdProperty =>View_ProdProperty.PropertyValueName).MaximumLength(20).WithMessage(":不能超过最大长度,20.");
       	
           	
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

