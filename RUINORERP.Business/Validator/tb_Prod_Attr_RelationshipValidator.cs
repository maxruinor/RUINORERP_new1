
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/13/2023 17:34:45
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
namespace RUINORERP.Business
{
    /// <summary>
    /// 验证类
    /// </summary>
    public partial class tb_Prod_Attr_RelationshipValidator:AbstractValidator<tb_Prod_Attr_Relationship>
    {
     public tb_Prod_Attr_RelationshipValidator() 
     {
      RuleFor(tb_Prod_Attr_Relationship =>tb_Prod_Attr_Relationship.Property_ID).NotEmpty().WithMessage(":不能为空。");
 RuleFor(tb_Prod_Attr_Relationship =>tb_Prod_Attr_Relationship.Property_ID).NotEmpty().When(x => x.Property_ID.HasValue);
 RuleFor(tb_Prod_Attr_Relationship =>tb_Prod_Attr_Relationship.PropertyValueID).NotEmpty().WithMessage("属性值ID:不能为空。");
 RuleFor(tb_Prod_Attr_Relationship =>tb_Prod_Attr_Relationship.PropertyValueID).NotEmpty().When(x => x.PropertyValueID.HasValue);
 RuleFor(tb_Prod_Attr_Relationship =>tb_Prod_Attr_Relationship.Prod_Base_ID).NotEmpty().WithMessage(":不能为空。");
 RuleFor(tb_Prod_Attr_Relationship =>tb_Prod_Attr_Relationship.Prod_Base_ID).NotEmpty().When(x => x.Prod_Base_ID.HasValue);
       	

     }
    }
}


