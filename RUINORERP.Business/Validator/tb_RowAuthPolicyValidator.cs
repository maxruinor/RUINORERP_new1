
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:22
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
    /// 行级权限规则验证类
    /// </summary>
    /*public partial class tb_RowAuthPolicyValidator:AbstractValidator<tb_RowAuthPolicy>*/
    public partial class tb_RowAuthPolicyValidator:BaseValidatorGeneric<tb_RowAuthPolicy>
    {
     

     public tb_RowAuthPolicyValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.PolicyName).MaximumMixedLength(100).WithMessage("规则名称:不能超过最大长度,100.");
 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.PolicyName).NotEmpty().WithMessage("规则名称:不能为空。");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.TargetTable).MaximumMixedLength(100).WithMessage("查询主表:不能超过最大长度,100.");
 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.TargetTable).NotEmpty().WithMessage("查询主表:不能为空。");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.TargetEntity).MaximumMixedLength(100).WithMessage("查询实体:不能超过最大长度,100.");
 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.TargetEntity).NotEmpty().WithMessage("查询实体:不能为空。");


 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.TargetTableJoinField).MaximumMixedLength(50).WithMessage("目标表关联字段:不能超过最大长度,50.");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.JoinTableJoinField).MaximumMixedLength(50).WithMessage("关联表关联字段:不能超过最大长度,50.");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.JoinTable).MaximumMixedLength(100).WithMessage("需要关联的表名:不能超过最大长度,100.");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.JoinType).MaximumMixedLength(10).WithMessage("关联类型:不能超过最大长度,10.");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.JoinOnClause).MaximumMixedLength(500).WithMessage("关联条件:不能超过最大长度,500.");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.FilterClause).MaximumMixedLength(1000).WithMessage("过滤条件:不能超过最大长度,1000.");

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.EntityType).MaximumMixedLength(200).WithMessage("实体的全限定类名:不能超过最大长度,200.");

//有默认值

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.PolicyDescription).MaximumMixedLength(500).WithMessage("规则描述:不能超过最大长度,500.");


 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_RowAuthPolicy =>tb_RowAuthPolicy.DefaultRuleEnum).NotEmpty().When(x => x.DefaultRuleEnum.HasValue);

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

