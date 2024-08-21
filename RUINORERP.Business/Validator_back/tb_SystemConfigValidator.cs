
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:36:23
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
    /// 系统配置表验证类
    /// </summary>
    public partial class tb_SystemConfigValidator:AbstractValidator<tb_SystemConfig>
    {
     public tb_SystemConfigValidator() 
     {
     //***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.QtyDataPrecision).NotNull().WithMessage("数量精度:不能为空。");
//***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.TaxRateDataPrecision).NotNull().WithMessage("税率精度:不能为空。");
//***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.MoneyDataPrecision).NotNull().WithMessage("金额精度:不能为空。");
//有默认值
//***** 
 RuleFor(tb_SystemConfig =>tb_SystemConfig.CostCalculationMethod).NotNull().WithMessage("成本方式:不能为空。");
//有默认值
       	
           	
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

