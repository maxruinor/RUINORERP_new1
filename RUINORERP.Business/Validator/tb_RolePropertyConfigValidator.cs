
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:22
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
    /// 角色属性配置不同角色权限功能等不一样验证类
    /// </summary>
    /*public partial class tb_RolePropertyConfigValidator:AbstractValidator<tb_RolePropertyConfig>*/
    public partial class tb_RolePropertyConfigValidator:BaseValidatorGeneric<tb_RolePropertyConfig>
    {
     

     public tb_RolePropertyConfigValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_RolePropertyConfig =>tb_RolePropertyConfig.RolePropertyName).MaximumMixedLength(255).WithMessage("角色名称:不能超过最大长度,255.");

//***** 
 RuleFor(tb_RolePropertyConfig =>tb_RolePropertyConfig.QtyDataPrecision).NotNull().WithMessage("数量精度:不能为空。");

//***** 
 RuleFor(tb_RolePropertyConfig =>tb_RolePropertyConfig.TaxRateDataPrecision).NotNull().WithMessage("税率精度:不能为空。");

//***** 
 RuleFor(tb_RolePropertyConfig =>tb_RolePropertyConfig.MoneyDataPrecision).NotNull().WithMessage("金额精度:不能为空。");

//有默认值

//***** 
 RuleFor(tb_RolePropertyConfig =>tb_RolePropertyConfig.CostCalculationMethod).NotNull().WithMessage("成本方式:不能为空。");










 RuleFor(tb_RolePropertyConfig =>tb_RolePropertyConfig.DataBoardUnits).MaximumMixedLength(500).WithMessage(":不能超过最大长度,500.");

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

