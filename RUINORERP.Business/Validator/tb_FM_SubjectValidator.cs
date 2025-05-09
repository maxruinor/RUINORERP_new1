﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：02/19/2025 22:58:11
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
    /// 会计科目表，财务系统中使用验证类
    /// </summary>
    /*public partial class tb_FM_SubjectValidator:AbstractValidator<tb_FM_Subject>*/
    public partial class tb_FM_SubjectValidator:BaseValidatorGeneric<tb_FM_Subject>
    {
     
     //配置全局参数
     public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;
    
     public tb_FM_SubjectValidator(IOptionsMonitor<GlobalValidatorConfig> config)
     {
     
        ValidatorConfig = config;
        
 
        
     
 RuleFor(tb_FM_Subject =>tb_FM_Subject.Parent_subject_id).NotEmpty().When(x => x.Parent_subject_id.HasValue);

 RuleFor(tb_FM_Subject =>tb_FM_Subject.Subject_code).MaximumLength(25).WithMessage("科目代码:不能超过最大长度,25.");
 RuleFor(tb_FM_Subject =>tb_FM_Subject.Subject_code).NotEmpty().WithMessage("科目代码:不能为空。");

 RuleFor(tb_FM_Subject =>tb_FM_Subject.Subject_name).MaximumLength(50).WithMessage("科目名称:不能超过最大长度,50.");
 RuleFor(tb_FM_Subject =>tb_FM_Subject.Subject_name).NotEmpty().WithMessage("科目名称:不能为空。");

 RuleFor(tb_FM_Subject =>tb_FM_Subject.Subject_en_name).MaximumLength(50).WithMessage("英文名称:不能超过最大长度,50.");

//***** 
 RuleFor(tb_FM_Subject =>tb_FM_Subject.Subject_Type).NotNull().WithMessage("科目类型:不能为空。");

//有默认值

//有默认值

 RuleFor(tb_FM_Subject =>tb_FM_Subject.Sort).NotEmpty().When(x => x.Sort.HasValue);


 RuleFor(tb_FM_Subject =>tb_FM_Subject.Notes).MaximumLength(100).WithMessage("备注:不能超过最大长度,100.");


 RuleFor(tb_FM_Subject =>tb_FM_Subject.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_FM_Subject =>tb_FM_Subject.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);


           	        Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_FM_ExpenseClaimDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
        }
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_OtherExpenseDetail> details)
        {
            bool rs = true;
            if (details == null || details.Count == 0)
            {
                return false;
            }
            return rs;
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

