
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:24
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
    /// 单位换算表验证类
    /// </summary>
    public partial class tb_Unit_ConversionValidator:AbstractValidator<tb_Unit_Conversion>
    {
     public tb_Unit_ConversionValidator() 
     {
     //***** 
 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.target_unit_id).NotNull().WithMessage("目标单位:不能为空。");
 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.source_unit_id).NotEmpty().When(x => x.source_unit_id.HasValue);
 RuleFor(x => x.conversion_ratio).PrecisionScale(10,2,true).WithMessage("换算比例:小数位不能超过2。");
 RuleFor(tb_Unit_Conversion =>tb_Unit_Conversion.notes).MaximumLength(200).WithMessage("备注:不能超过最大长度,200.");
       	
           	
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

