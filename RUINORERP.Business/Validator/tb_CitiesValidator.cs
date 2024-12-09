
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:41
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
    /// 城市表验证类
    /// </summary>
    /*public partial class tb_CitiesValidator:AbstractValidator<tb_Cities>*/
    public partial class tb_CitiesValidator:BaseValidatorGeneric<tb_Cities>
    {
     public tb_CitiesValidator() 
     {
      RuleFor(tb_Cities =>tb_Cities.ProvinceID).Must(CheckForeignKeyValueCanNull).WithMessage("省:下拉选择值不正确。");
 RuleFor(tb_Cities =>tb_Cities.ProvinceID).NotEmpty().When(x => x.ProvinceID.HasValue);
 RuleFor(tb_Cities =>tb_Cities.CityCNName).MaximumLength(40).WithMessage("城市中文名:不能超过最大长度,40.");
 RuleFor(tb_Cities =>tb_Cities.CityENName).MaximumLength(40).WithMessage("城市英文名:不能超过最大长度,40.");
       	
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

