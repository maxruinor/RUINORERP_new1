
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:09
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
    /// 卡通箱规格表验证类
    /// </summary>
    /*public partial class tb_CartoonBoxValidator:AbstractValidator<tb_CartoonBox>*/
    public partial class tb_CartoonBoxValidator:BaseValidatorGeneric<tb_CartoonBox>
    {
     

     public tb_CartoonBoxValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_CartoonBox =>tb_CartoonBox.CartonName).MaximumMixedLength(100).WithMessage("纸箱名称:不能超过最大长度,100.");

 RuleFor(tb_CartoonBox =>tb_CartoonBox.Color).MaximumMixedLength(100).WithMessage("颜色:不能超过最大长度,100.");

 RuleFor(tb_CartoonBox =>tb_CartoonBox.Material).MaximumMixedLength(100).WithMessage("材质:不能超过最大长度,100.");

 RuleFor(x => x.EmptyBoxWeight).PrecisionScale(10,3,true).WithMessage("空箱重(kg):小数位不能超过3。");

 RuleFor(x => x.MaxLoad).PrecisionScale(10,3,true).WithMessage("最大承重(kg):小数位不能超过3。");

 RuleFor(x => x.Thickness).PrecisionScale(8,2,true).WithMessage("纸板厚度(cm):小数位不能超过2。");

 RuleFor(x => x.Length).PrecisionScale(8,2,true).WithMessage("长度(cm):小数位不能超过2。");

 RuleFor(x => x.Width).PrecisionScale(8,2,true).WithMessage("宽度(cm):小数位不能超过2。");

 RuleFor(x => x.Height).PrecisionScale(8,2,true).WithMessage("高度(cm):小数位不能超过2。");

 RuleFor(x => x.Volume).PrecisionScale(10,3,true).WithMessage("体积Vol(cm³):小数位不能超过3。");

 RuleFor(tb_CartoonBox =>tb_CartoonBox.FluteType).MaximumMixedLength(100).WithMessage("瓦楞类型:不能超过最大长度,100.");

 RuleFor(tb_CartoonBox =>tb_CartoonBox.PrintType).MaximumMixedLength(100).WithMessage("印刷类型:不能超过最大长度,100.");

 RuleFor(tb_CartoonBox =>tb_CartoonBox.CustomPrint).MaximumMixedLength(100).WithMessage("定制印刷:不能超过最大长度,100.");


 RuleFor(tb_CartoonBox =>tb_CartoonBox.Description).MaximumMixedLength(255).WithMessage("备注:不能超过最大长度,255.");



 RuleFor(tb_CartoonBox =>tb_CartoonBox.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_CartoonBox =>tb_CartoonBox.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

           	  
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

