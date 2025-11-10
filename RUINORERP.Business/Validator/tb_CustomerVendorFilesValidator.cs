
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
    /// 客户厂商认证文件表验证类
    /// </summary>
    /*public partial class tb_CustomerVendorFilesValidator:AbstractValidator<tb_CustomerVendorFiles>*/
    public partial class tb_CustomerVendorFilesValidator:BaseValidatorGeneric<tb_CustomerVendorFiles>
    {
     

     public tb_CustomerVendorFilesValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_CustomerVendorFiles =>tb_CustomerVendorFiles.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);

 RuleFor(tb_CustomerVendorFiles =>tb_CustomerVendorFiles.FileName).MaximumMixedLength(200).WithMessage("文件名:不能超过最大长度,200.");

 RuleFor(tb_CustomerVendorFiles =>tb_CustomerVendorFiles.FileType).MaximumMixedLength(50).WithMessage("文件类型:不能超过最大长度,50.");

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

