
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:10
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
    /// 往来单位类型,如级别，电商，大客户，亚马逊等验证类
    /// </summary>
    /*public partial class tb_CustomerVendorTypeValidator:AbstractValidator<tb_CustomerVendorType>*/
    public partial class tb_CustomerVendorTypeValidator:BaseValidatorGeneric<tb_CustomerVendorType>
    {
     

     public tb_CustomerVendorTypeValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_CustomerVendorType =>tb_CustomerVendorType.TypeName).MaximumMixedLength(50).WithMessage("类型等级名称:不能超过最大长度,50.");
 RuleFor(tb_CustomerVendorType =>tb_CustomerVendorType.TypeName).NotEmpty().WithMessage("类型等级名称:不能为空。");

 RuleFor(tb_CustomerVendorType =>tb_CustomerVendorType.Desc).MaximumMixedLength(100).WithMessage("描述:不能超过最大长度,100.");


           	  
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

