
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 产品主次及属性关系表验证类
    /// </summary>
    /*public partial class tb_Prod_Attr_RelationValidator:AbstractValidator<tb_Prod_Attr_Relation>*/
    public partial class tb_Prod_Attr_RelationValidator:BaseValidatorGeneric<tb_Prod_Attr_Relation>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_Prod_Attr_RelationValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Prod_Attr_Relation =>tb_Prod_Attr_Relation.PropertyValueID).Must(CheckForeignKeyValueCanNull).WithMessage("属性值:下拉选择值不正确。");
 RuleFor(tb_Prod_Attr_Relation =>tb_Prod_Attr_Relation.PropertyValueID).NotEmpty().When(x => x.PropertyValueID.HasValue);

 RuleFor(tb_Prod_Attr_Relation =>tb_Prod_Attr_Relation.Property_ID).Must(CheckForeignKeyValueCanNull).WithMessage("属性:下拉选择值不正确。");
 RuleFor(tb_Prod_Attr_Relation =>tb_Prod_Attr_Relation.Property_ID).NotEmpty().When(x => x.Property_ID.HasValue);

 RuleFor(tb_Prod_Attr_Relation =>tb_Prod_Attr_Relation.ProdDetailID).Must(CheckForeignKeyValueCanNull).WithMessage("货品详情:下拉选择值不正确。");
 RuleFor(tb_Prod_Attr_Relation =>tb_Prod_Attr_Relation.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);

 RuleFor(tb_Prod_Attr_Relation =>tb_Prod_Attr_Relation.ProdBaseID).NotEmpty().When(x => x.ProdBaseID.HasValue);


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

