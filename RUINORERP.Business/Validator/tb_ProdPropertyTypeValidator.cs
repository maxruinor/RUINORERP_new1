﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:10
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
    /// 产品属性类型EVA验证类
    /// </summary>
    /*public partial class tb_ProdPropertyTypeValidator:AbstractValidator<tb_ProdPropertyType>*/
    public partial class tb_ProdPropertyTypeValidator:BaseValidatorGeneric<tb_ProdPropertyType>
    {
     public tb_ProdPropertyTypeValidator() 
     {
      RuleFor(tb_ProdPropertyType =>tb_ProdPropertyType.PropertyTypeName).MaximumLength(25).WithMessage("属性类型名称:不能超过最大长度,25.");
 RuleFor(tb_ProdPropertyType =>tb_ProdPropertyType.PropertyTypeName).NotEmpty().WithMessage("属性类型名称:不能为空。");
 RuleFor(tb_ProdPropertyType =>tb_ProdPropertyType.PropertyTypeDesc).MaximumLength(50).WithMessage("属性类型描述:不能超过最大长度,50.");
       	
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

