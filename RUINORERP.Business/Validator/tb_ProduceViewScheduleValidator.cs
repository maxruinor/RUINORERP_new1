
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:19
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
    /// 可视化排程验证类
    /// </summary>
    /*public partial class tb_ProduceViewScheduleValidator:AbstractValidator<tb_ProduceViewSchedule>*/
    public partial class tb_ProduceViewScheduleValidator:BaseValidatorGeneric<tb_ProduceViewSchedule>
    {
     

     public tb_ProduceViewScheduleValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_ProduceViewSchedule =>tb_ProduceViewSchedule.product_id).NotEmpty().When(x => x.product_id.HasValue);

 RuleFor(tb_ProduceViewSchedule =>tb_ProduceViewSchedule.quantity).NotEmpty().When(x => x.quantity.HasValue);



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

