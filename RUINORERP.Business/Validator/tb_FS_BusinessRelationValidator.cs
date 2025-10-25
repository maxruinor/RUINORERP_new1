﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:19
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
    /// 文件业务关联表验证类
    /// </summary>
    /*public partial class tb_FS_BusinessRelationValidator:AbstractValidator<tb_FS_BusinessRelation>*/
    public partial class tb_FS_BusinessRelationValidator:BaseValidatorGeneric<tb_FS_BusinessRelation>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FS_BusinessRelationValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FS_BusinessRelation =>tb_FS_BusinessRelation.FileId).Must(CheckForeignKeyValueCanNull).WithMessage("文件ID:下拉选择值不正确。");
 RuleFor(tb_FS_BusinessRelation =>tb_FS_BusinessRelation.FileId).NotEmpty().When(x => x.FileId.HasValue);

 RuleFor(tb_FS_BusinessRelation =>tb_FS_BusinessRelation.BusinessType).NotEmpty().When(x => x.BusinessType.HasValue);

 RuleFor(tb_FS_BusinessRelation =>tb_FS_BusinessRelation.BusinessNo).MaximumMixedLength(50).WithMessage("业务编号:不能超过最大长度,50.");


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

