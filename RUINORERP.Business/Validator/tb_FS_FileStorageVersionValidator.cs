
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:20
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

//https://github.com/FluentValidation/FluentValidation 使用实例
//https://blog.csdn.net/WuLex/article/details/127985756 中文教程
//https://www.nhooo.com/note/qa3k5f.html  智能化验证
//http://cn.voidcc.com/question/p-qunoezdb-bkh.html
namespace RUINORERP.Business
{
    /// <summary>
    /// 文件版本表验证类
    /// </summary>
    /*public partial class tb_FS_FileStorageVersionValidator:AbstractValidator<tb_FS_FileStorageVersion>*/
    public partial class tb_FS_FileStorageVersionValidator:BaseValidatorGeneric<tb_FS_FileStorageVersion>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FS_FileStorageVersionValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.FileId).Must(CheckForeignKeyValueCanNull).WithMessage("文件ID:下拉选择值不正确。");
 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.FileId).NotEmpty().When(x => x.FileId.HasValue);

//***** 
 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.VersionNo).NotNull().WithMessage("版本号:不能为空。");

 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.StorageFileName).MaximumMixedLength(255).WithMessage("存储文件名:不能超过最大长度,255.");
 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.StorageFileName).NotEmpty().WithMessage("存储文件名:不能为空。");

 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.UpdateReason).MaximumMixedLength(300).WithMessage("存储路径:不能超过最大长度,300.");
 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.UpdateReason).NotEmpty().WithMessage("存储路径:不能为空。");

 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.HashValue).MaximumMixedLength(64).WithMessage("文件哈希值:不能超过最大长度,64.");


 RuleFor(tb_FS_FileStorageVersion =>tb_FS_FileStorageVersion.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

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

