
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:30
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
    /// 业务类型 报销，员工借支还款，运费验证类
    /// </summary>
    public partial class tb_FM_ExpenseTypeValidator:AbstractValidator<tb_FM_ExpenseType>
    {
     public tb_FM_ExpenseTypeValidator() 
     {
      RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.subject_id).Must(CheckForeignKeyValueCanNull).WithMessage("科目:下拉选择值不正确。");
 RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.subject_id).NotEmpty().When(x => x.subject_id.HasValue);
 RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.Expense_name).MaximumLength(50).WithMessage("费用业务名称:不能超过最大长度,50.");
//有默认值
 RuleFor(tb_FM_ExpenseType =>tb_FM_ExpenseType.Notes).MaximumLength(30).WithMessage("备注:不能超过最大长度,30.");
       	
           	
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
        

        private bool DetailedRecordsNotEmpty(List<tb_FM_ExpenseClaimDetail> details)
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

