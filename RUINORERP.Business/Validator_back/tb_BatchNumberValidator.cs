﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:34:36
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
    /// 批次表 在采购入库时和出库时保存批次ID验证类
    /// </summary>
    public partial class tb_BatchNumberValidator:AbstractValidator<tb_BatchNumber>
    {
     public tb_BatchNumberValidator() 
     {
      RuleFor(tb_BatchNumber =>tb_BatchNumber.BatchNO).MaximumLength(50).WithMessage(":不能超过最大长度,50.");
 RuleFor(tb_BatchNumber =>tb_BatchNumber.采购单号).MaximumLength(20).WithMessage(":不能超过最大长度,20.");
 RuleFor(tb_BatchNumber =>tb_BatchNumber.供应商).NotEmpty().When(x => x.供应商.HasValue);
 RuleFor(x => x.采购单价).PrecisionScale(10,0,true).WithMessage(":小数位不能超过0。");
 RuleFor(x => x.sale_price).PrecisionScale(10,2,true).WithMessage(":小数位不能超过2。");
 RuleFor(tb_BatchNumber =>tb_BatchNumber.quantity).NotEmpty().When(x => x.quantity.HasValue);
       	
           	
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

