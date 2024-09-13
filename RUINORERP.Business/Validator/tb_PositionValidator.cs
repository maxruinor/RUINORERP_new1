
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
    /// 位置信息验证类
    /// </summary>
    /*public partial class tb_PositionValidator:AbstractValidator<tb_Position>*/
    public partial class tb_PositionValidator:BaseValidatorGeneric<tb_Position>
    {
     public tb_PositionValidator() 
     {
      RuleFor(tb_Position =>tb_Position.Left).MaximumLength(25).WithMessage("左边距:不能超过最大长度,25.");
 RuleFor(tb_Position =>tb_Position.Left).NotEmpty().WithMessage("左边距:不能为空。");
 RuleFor(tb_Position =>tb_Position.Right).MaximumLength(25).WithMessage("右边距:不能超过最大长度,25.");
 RuleFor(tb_Position =>tb_Position.Bottom).MaximumLength(25).WithMessage("下边距:不能超过最大长度,25.");
 RuleFor(tb_Position =>tb_Position.Top).MaximumLength(25).WithMessage("上边距:不能超过最大长度,25.");
       	
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

