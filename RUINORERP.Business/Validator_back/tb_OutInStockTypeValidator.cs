
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/20/2024 16:35:26
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
    /// 出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？验证类
    /// </summary>
    public partial class tb_OutInStockTypeValidator:AbstractValidator<tb_OutInStockType>
    {
     public tb_OutInStockTypeValidator() 
     {
      RuleFor(tb_OutInStockType =>tb_OutInStockType.TypeName).MaximumLength(50).WithMessage("类型名称:不能超过最大长度,50.");
 RuleFor(tb_OutInStockType =>tb_OutInStockType.TypeName).NotEmpty().WithMessage("类型名称:不能为空。");
 RuleFor(tb_OutInStockType =>tb_OutInStockType.TypeDesc).MaximumLength(100).WithMessage("描述:不能超过最大长度,100.");
       	
           	
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

