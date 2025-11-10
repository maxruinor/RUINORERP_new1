
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:24
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
    /// 单位换算表验证类
    /// </summary>
    /*public partial class tb_Unit_ConversionValidator:AbstractValidator<tb_Unit_Conversion>*/
    public partial class tb_Unit_ConversionValidator:BaseValidatorGeneric<tb_Unit_Conversion>
    {
     

     public tb_Unit_ConversionValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.UnitConversion_Name).MaximumMixedLength(100).WithMessage("备注:不能超过最大长度,100.");
 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.UnitConversion_Name).NotEmpty().WithMessage("备注:不能为空。");

//***** 
 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.Source_unit_id).NotNull().WithMessage("来源单位:不能为空。");

//***** 
 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.Target_unit_id).NotNull().WithMessage("目标单位:不能为空。");

 RuleFor(x => x.Conversion_ratio).PrecisionScale(10,5,true).WithMessage("换算比例:小数位不能超过5。");

 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.Notes).MaximumMixedLength(200).WithMessage("备注:不能超过最大长度,200.");

           	        Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_BOM_SDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_BOM_SDetailSubstituteMaterial> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

