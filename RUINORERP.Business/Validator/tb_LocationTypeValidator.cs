
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:13
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
    /// 库位类别验证类
    /// </summary>
    /*public partial class tb_LocationTypeValidator:AbstractValidator<tb_LocationType>*/
    public partial class tb_LocationTypeValidator:BaseValidatorGeneric<tb_LocationType>
    {
     

     public tb_LocationTypeValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_LocationType =>tb_LocationType.TypeName).MaximumMixedLength(50).WithMessage("类型名称:不能超过最大长度,50.");
 RuleFor(tb_LocationType =>tb_LocationType.TypeName).NotEmpty().WithMessage("类型名称:不能为空。");

 RuleFor(tb_LocationType =>tb_LocationType.Desc).MaximumMixedLength(100).WithMessage("描述:不能超过最大长度,100.");

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

