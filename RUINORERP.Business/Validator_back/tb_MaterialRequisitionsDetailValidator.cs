
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:19
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
    /// 领料单明细验证类
    /// </summary>
    public partial class tb_MaterialRequisitionsDetailValidator:AbstractValidator<tb_MaterialRequisitionsDetail>
    {
     public tb_MaterialRequisitionsDetailValidator() 
     {
     //***** 
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.MainID).NotNull().WithMessage("领料单:不能为空。");
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
//***** 
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.ProdDetailID).NotNull().WithMessage("货品详情:不能为空。");
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.RefBIll_ID).NotEmpty().When(x => x.RefBIll_ID.HasValue);
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.RefBIllType).NotEmpty().When(x => x.RefBIllType.HasValue);
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.RefBIllNO).MaximumLength(50).WithMessage("引用单号:不能超过最大长度,50.");
//***** 
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.ActualQuantity).NotNull().WithMessage("实发数量:不能为空。");
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.Summary).MaximumLength(255).WithMessage("摘要:不能超过最大长度,255.");
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.CustomerPartNo).MaximumLength(50).WithMessage("客户型号:不能超过最大长度,50.");
 RuleFor(x => x.Cost).PrecisionScale(19,4,true).WithMessage("成本:小数位不能超过4。");
 RuleFor(x => x.Price).PrecisionScale(19,4,true).WithMessage("价格:小数位不能超过4。");
 RuleFor(x => x.SubtotalPrice).PrecisionScale(19,4,true).WithMessage("金额小计:小数位不能超过4。");
 RuleFor(x => x.SubtotalCost).PrecisionScale(19,4,true).WithMessage("成本小计:小数位不能超过4。");
//***** 
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.TotatQty).NotNull().WithMessage("累计数量:不能为空。");
//***** 
 RuleFor(tb_MaterialRequisitionsDetail =>tb_MaterialRequisitionsDetail.ReturnQty).NotNull().WithMessage("退回数量:不能为空。");
       	
           	
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

