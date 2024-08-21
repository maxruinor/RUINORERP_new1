
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/20/2024 20:30:05
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
    /// 拆分单明细验证类
    /// </summary>
    public partial class tb_ProdSplitDetailValidator:AbstractValidator<tb_ProdSplitDetail>
    {
     public tb_ProdSplitDetailValidator() 
     {
     //***** 
 RuleFor(tb_ProdSplitDetail =>tb_ProdSplitDetail.SplitID).NotNull().WithMessage("拆分单:不能为空。");
 RuleFor(tb_ProdSplitDetail =>tb_ProdSplitDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
 RuleFor(tb_ProdSplitDetail =>tb_ProdSplitDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("子件:下拉选择值不正确。");
 RuleFor(tb_ProdSplitDetail =>tb_ProdSplitDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
//***** 
 RuleFor(tb_ProdSplitDetail =>tb_ProdSplitDetail.Qty).NotNull().WithMessage("子件数量:不能为空。");
 RuleFor(tb_ProdSplitDetail =>tb_ProdSplitDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
       	
           	
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

