
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:16
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
    /// 收付款方式与账号映射配置表-销售订单收款时付款方式即可指定到收到哪个账号下面验证类
    /// </summary>
    /*public partial class tb_PayMethodAccountMapperValidator:AbstractValidator<tb_PayMethodAccountMapper>*/
    public partial class tb_PayMethodAccountMapperValidator:BaseValidatorGeneric<tb_PayMethodAccountMapper>
    {
     

     public tb_PayMethodAccountMapperValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PayMethodAccountMapper =>tb_PayMethodAccountMapper.Paytype_ID).Must(CheckForeignKeyValue).WithMessage("付款方式:下拉选择值不正确。");

 RuleFor(tb_PayMethodAccountMapper =>tb_PayMethodAccountMapper.Account_id).Must(CheckForeignKeyValue).WithMessage("公司账户:下拉选择值不正确。");

 RuleFor(tb_PayMethodAccountMapper =>tb_PayMethodAccountMapper.Description).MaximumMixedLength(50).WithMessage("描述:不能超过最大长度,50.");



           	  
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

