
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 订单装箱单，针对特别订单详细处理 也用主子表来做。暂时不搞验证类
    /// </summary>
    /*public partial class tb_OrderPackingValidator:AbstractValidator<tb_OrderPacking>*/
    public partial class tb_OrderPackingValidator:BaseValidatorGeneric<tb_OrderPacking>
    {
     

     public tb_OrderPackingValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_OrderPacking =>tb_OrderPacking.SOrder_ID).Must(CheckForeignKeyValue).WithMessage("订单:下拉选择值不正确。");

 RuleFor(tb_OrderPacking =>tb_OrderPacking.BoxNo).MaximumMixedLength(50).WithMessage("箱号:不能超过最大长度,50.");
 RuleFor(tb_OrderPacking =>tb_OrderPacking.BoxNo).NotEmpty().WithMessage("箱号:不能为空。");

 RuleFor(tb_OrderPacking =>tb_OrderPacking.BoxMark).MaximumMixedLength(100).WithMessage("箱唛:不能超过最大长度,100.");

 RuleFor(tb_OrderPacking =>tb_OrderPacking.Remarks).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");

//***** 
 RuleFor(tb_OrderPacking =>tb_OrderPacking.QuantityPerBox).NotNull().WithMessage("数量:不能为空。");

 RuleFor(x => x.Length).PrecisionScale(8,2,true).WithMessage("长度(CM):小数位不能超过2。");

 RuleFor(x => x.Width).PrecisionScale(8,2,true).WithMessage("宽度(CM):小数位不能超过2。");

 RuleFor(x => x.Height).PrecisionScale(8,2,true).WithMessage("高度(CM):小数位不能超过2。");

 RuleFor(tb_OrderPacking =>tb_OrderPacking.Created_by).NotEmpty().When(x => x.Created_by.HasValue);

 RuleFor(tb_OrderPacking =>tb_OrderPacking.BoxMaterial).MaximumMixedLength(200).WithMessage("箱子材质:不能超过最大长度,200.");

 RuleFor(x => x.Volume).PrecisionScale(8,2,true).WithMessage("体积(CM):小数位不能超过2。");



 RuleFor(tb_OrderPacking =>tb_OrderPacking.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(x => x.GrossWeight).PrecisionScale(10,3,true).WithMessage("毛重(KG):小数位不能超过3。");

 RuleFor(x => x.NetWeight).PrecisionScale(10,3,true).WithMessage("净重(KG):小数位不能超过3。");

 RuleFor(tb_OrderPacking =>tb_OrderPacking.PackingMethod).MaximumMixedLength(100).WithMessage("打包方式:不能超过最大长度,100.");

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

