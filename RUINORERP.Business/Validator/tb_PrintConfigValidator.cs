
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:02
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
    /// 报表打印配置表验证类
    /// </summary>
    /*public partial class tb_PrintConfigValidator:AbstractValidator<tb_PrintConfig>*/
    public partial class tb_PrintConfigValidator:BaseValidatorGeneric<tb_PrintConfig>
    {
     public tb_PrintConfigValidator() 
     {
      RuleFor(tb_PrintConfig =>tb_PrintConfig.Config_Name).MaximumLength(50).WithMessage("配置名称:不能超过最大长度,50.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.Config_Name).NotEmpty().WithMessage("配置名称:不能为空。");
//***** 
 RuleFor(tb_PrintConfig =>tb_PrintConfig.BizType).NotNull().WithMessage("业务类型:不能为空。");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.BizName).MaximumLength(15).WithMessage("业务名称:不能超过最大长度,15.");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.BizName).NotEmpty().WithMessage("业务名称:不能为空。");
 RuleFor(tb_PrintConfig =>tb_PrintConfig.PrinterName).MaximumLength(100).WithMessage("打印机名称:不能超过最大长度,100.");
       	
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

