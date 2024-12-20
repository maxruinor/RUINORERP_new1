
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:26
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
    /// 销售分区表-大中华区验证类
    /// </summary>
    /*public partial class tb_CRM_RegionValidator:AbstractValidator<tb_CRM_Region>*/
    public partial class tb_CRM_RegionValidator:BaseValidatorGeneric<tb_CRM_Region>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_CRM_RegionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_CRM_Region =>tb_CRM_Region.Region_Name).MaximumLength(25).WithMessage("地区名称:不能超过最大长度,25.");

 RuleFor(tb_CRM_Region =>tb_CRM_Region.Region_code).MaximumLength(10).WithMessage("地区代码:不能超过最大长度,10.");

 RuleFor(tb_CRM_Region =>tb_CRM_Region.Parent_region_id).NotEmpty().When(x => x.Parent_region_id.HasValue);

 RuleFor(tb_CRM_Region =>tb_CRM_Region.Sort).NotEmpty().When(x => x.Sort.HasValue);

//有默认值

 RuleFor(tb_CRM_Region =>tb_CRM_Region.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");

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

