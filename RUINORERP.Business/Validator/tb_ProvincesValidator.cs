
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 12:02:46
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
    /// 省份表验证类
    /// </summary>
    /*public partial class tb_ProvincesValidator:AbstractValidator<tb_Provinces>*/
    public partial class tb_ProvincesValidator:BaseValidatorGeneric<tb_Provinces>
    {
     public tb_ProvincesValidator() 
     {
      RuleFor(tb_Provinces =>tb_Provinces.ProvinceCNName).MaximumLength(40).WithMessage("省份中文名:不能超过最大长度,40.");
 RuleFor(tb_Provinces =>tb_Provinces.CountryID).NotEmpty().When(x => x.CountryID.HasValue);
 RuleFor(tb_Provinces =>tb_Provinces.ProvinceENName).MaximumLength(40).WithMessage("省份英文名:不能超过最大长度,40.");
       	
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

