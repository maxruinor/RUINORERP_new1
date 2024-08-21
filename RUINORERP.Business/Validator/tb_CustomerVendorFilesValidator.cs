
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:29
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
    /// 客户厂商认证文件表验证类
    /// </summary>
    public partial class tb_CustomerVendorFilesValidator:AbstractValidator<tb_CustomerVendorFiles>
    {
     public tb_CustomerVendorFilesValidator() 
     {
      RuleFor(tb_CustomerVendorFiles =>tb_CustomerVendorFiles.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_CustomerVendorFiles =>tb_CustomerVendorFiles.FileName).MaximumLength(200).WithMessage("文件名:不能超过最大长度,200.");
 RuleFor(tb_CustomerVendorFiles =>tb_CustomerVendorFiles.FileType).MaximumLength(50).WithMessage("文件类型:不能超过最大长度,50.");
       	
           	
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

