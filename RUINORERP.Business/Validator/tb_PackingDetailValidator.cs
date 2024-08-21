
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2024 14:54:20
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
    /// 包装清单验证类
    /// </summary>
    public partial class tb_PackingDetailValidator:AbstractValidator<tb_PackingDetail>
    {
     public tb_PackingDetailValidator() 
     {
     //***** 
 RuleFor(tb_PackingDetail =>tb_PackingDetail.Pack_ID).NotNull().WithMessage("包装:不能为空。");
//***** 
 RuleFor(tb_PackingDetail =>tb_PackingDetail.ProdDetailID).NotNull().WithMessage("产品详情:不能为空。");
 RuleFor(tb_PackingDetail =>tb_PackingDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_PackingDetail =>tb_PackingDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
 RuleFor(tb_PackingDetail =>tb_PackingDetail.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
 RuleFor(tb_PackingDetail =>tb_PackingDetail.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_PackingDetail =>tb_PackingDetail.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
       	
           	
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

