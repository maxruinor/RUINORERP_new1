
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/21/2026 18:12:15
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
    /// 文件信息元数据表验证类
    /// </summary>
    /*public partial class tb_FS_FileStorageInfoValidator:AbstractValidator<tb_FS_FileStorageInfo>*/
    public partial class tb_FS_FileStorageInfoValidator:BaseValidatorGeneric<tb_FS_FileStorageInfo>
    {
     

     public tb_FS_FileStorageInfoValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.OriginalFileName).MaximumMixedLength(255).WithMessage("原始文件名:不能超过最大长度,255.");
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.OriginalFileName).NotEmpty().WithMessage("原始文件名:不能为空。");

 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.StorageFileName).MaximumMixedLength(255).WithMessage("存储文件名:不能超过最大长度,255.");
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.StorageFileName).NotEmpty().WithMessage("存储文件名:不能为空。");

 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.FileExtension).MaximumMixedLength(50).WithMessage("文件扩展名:不能超过最大长度,50.");
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.FileExtension).NotEmpty().WithMessage("文件扩展名:不能为空。");

 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.BusinessType).NotEmpty().When(x => x.BusinessType.HasValue);

 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.FileType).MaximumMixedLength(200).WithMessage("文件类型:不能超过最大长度,200.");
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.FileType).NotEmpty().WithMessage("文件类型:不能为空。");

//***** 
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.FileSize).NotNull().WithMessage("文件大小（字节）:不能为空。");

 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.HashValue).MaximumMixedLength(64).WithMessage("文件哈希值:不能超过最大长度,64.");

 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.StorageProvider).MaximumMixedLength(50).WithMessage("存储引擎:不能超过最大长度,50.");
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.StorageProvider).NotEmpty().WithMessage("存储引擎:不能为空。");

 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.StoragePath).MaximumMixedLength(300).WithMessage("存储路径:不能超过最大长度,300.");
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.StoragePath).NotEmpty().WithMessage("存储路径:不能为空。");

//***** 
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.CurrentVersion).NotNull().WithMessage("版本号:不能为空。");

//***** 
 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.FileStatus).NotNull().WithMessage("文件状态:不能为空。");


 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.Description).MaximumMixedLength(200).WithMessage("文件描述:不能超过最大长度,200.");


 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FS_FileStorageInfo =>tb_FS_FileStorageInfo.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);



           	      
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

