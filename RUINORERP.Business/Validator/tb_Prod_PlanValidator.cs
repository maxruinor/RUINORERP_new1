
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2023 19:21:45
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;

//https://github.com/FluentValidation/FluentValidation 使用实例
namespace RUINORERP.Business
{
    /// <summary>
    /// 生产计划表验证类
    /// </summary>
    public partial class tb_Prod_PlanValidator:AbstractValidator<tb_Prod_Plan>
    {
     public tb_Prod_PlanValidator() 
     {
      RuleFor(tb_Prod_Plan =>tb_Prod_Plan.id).NotEmpty().When(x => x.id.HasValue);
       	

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
            if (ForeignKeyID == 0 || ForeignKeyID == -1)
            {
                return false;
            }
            return rs;
        }
        
    }
}

