
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/28/2025 16:58:35
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
    /// 货物类型  成品  半成品  包装材料 下脚料这种内容验证类
    /// </summary>
    /*public partial class tb_ProductTypeValidator:AbstractValidator<tb_ProductType>*/
    public partial class tb_ProductTypeValidator:BaseValidatorGeneric<tb_ProductType>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_ProductTypeValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_ProductType =>tb_ProductType.TypeName).MaximumLength(25).WithMessage("类型名称:不能超过最大长度,25.");
 RuleFor(tb_ProductType =>tb_ProductType.TypeName).NotEmpty().WithMessage("类型名称:不能为空。");

 RuleFor(tb_ProductType =>tb_ProductType.TypeDesc).MaximumLength(50).WithMessage("描述:不能超过最大长度,50.");


           	        Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_ProdConversionDetail> details)
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

