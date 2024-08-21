
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/11/2024 00:37:03
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
    public partial class View_ProdPropGoupValidator:AbstractValidator<View_ProdPropGoup>
    {
     public View_ProdPropGoupValidator() 
     {
     //***** 
 RuleFor(View_ProdPropGoup =>View_ProdPropGoup.ProdDetailID).NotNull().WithMessage(":不能为空。");
 RuleFor(View_ProdPropGoup =>View_ProdPropGoup.prop).MaximumLength(-1).WithMessage(":不能超过最大长度,-1.");
//***** 
 RuleFor(View_ProdPropGoup =>View_ProdPropGoup.ProdBaseID).NotNull().WithMessage(":不能为空。");
       	
           	
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

