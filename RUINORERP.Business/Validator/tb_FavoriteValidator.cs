﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:29
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
    /// 收藏表 收藏订单 产品 库存报警等验证类
    /// </summary>
    public partial class tb_FavoriteValidator:AbstractValidator<tb_Favorite>
    {
     public tb_FavoriteValidator() 
     {
      RuleFor(tb_Favorite =>tb_Favorite.ReferenceID).NotEmpty().When(x => x.ReferenceID.HasValue);
 RuleFor(tb_Favorite =>tb_Favorite.Ref_Table_Name).MaximumLength(100).WithMessage("引用表名:不能超过最大长度,100.");
 RuleFor(tb_Favorite =>tb_Favorite.ModuleName).MaximumLength(255).WithMessage("模块名:不能超过最大长度,255.");
 RuleFor(tb_Favorite =>tb_Favorite.BusinessType).MaximumLength(255).WithMessage("业务类型:不能超过最大长度,255.");
 RuleFor(tb_Favorite =>tb_Favorite.Notes).MaximumLength(500).WithMessage("备注说明:不能超过最大长度,500.");
 RuleFor(tb_Favorite =>tb_Favorite.Owner_by).NotEmpty().When(x => x.Owner_by.HasValue);
 RuleFor(tb_Favorite =>tb_Favorite.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_Favorite =>tb_Favorite.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
           	
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

