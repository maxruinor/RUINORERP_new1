
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2024 13:38:38
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
    /// 借出单明细验证类
    /// </summary>
    public partial class tb_ProdBorrowingDetailValidator:AbstractValidator<tb_ProdBorrowingDetail>
    {
     public tb_ProdBorrowingDetailValidator() 
     {
     //***** 
 RuleFor(tb_ProdBorrowingDetail =>tb_ProdBorrowingDetail.BorrowID).NotNull().WithMessage(":不能为空。");
 RuleFor(tb_ProdBorrowingDetail =>tb_ProdBorrowingDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_ProdBorrowingDetail =>tb_ProdBorrowingDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");
 RuleFor(tb_ProdBorrowingDetail =>tb_ProdBorrowingDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_ProdBorrowingDetail =>tb_ProdBorrowingDetail.Qty).NotNull().WithMessage("数量:不能为空。");
 RuleFor(x => x.Price).PrecisionScale(19,4,true).WithMessage("售价:小数位不能超过4。");
 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");
 RuleFor(tb_ProdBorrowingDetail =>tb_ProdBorrowingDetail.Summary).MaximumLength(250).WithMessage("摘要:不能超过最大长度,250.");
 RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");
 RuleFor(x => x.SubtotalPirceAmount).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");
       	
           	
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

