
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:03
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

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
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ProvincesValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_Provinces =>tb_Provinces.Region_ID).Must(CheckForeignKeyValueCanNull).WithMessage("地区:下拉选择值不正确。");
 RuleFor(tb_Provinces =>tb_Provinces.Region_ID).NotEmpty().When(x => x.Region_ID.HasValue);

 RuleFor(tb_Provinces =>tb_Provinces.ProvinceCNName).MaximumMixedLength(80).WithMessage("省份中文名:不能超过最大长度,80.");

 RuleFor(tb_Provinces =>tb_Provinces.CountryID).NotEmpty().When(x => x.CountryID.HasValue);

 RuleFor(tb_Provinces =>tb_Provinces.ProvinceENName).MaximumMixedLength(80).WithMessage("省份英文名:不能超过最大长度,80.");

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

