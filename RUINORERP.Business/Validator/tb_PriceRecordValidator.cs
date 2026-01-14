
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
    /// 价格记录表验证类
    /// </summary>
    /*public partial class tb_PriceRecordValidator:AbstractValidator<tb_PriceRecord>*/
    public partial class tb_PriceRecordValidator:BaseValidatorGeneric<tb_PriceRecord>
    {
     

     public tb_PriceRecordValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PriceRecord =>tb_PriceRecord.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");

 RuleFor(tb_PriceRecord =>tb_PriceRecord.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");



 RuleFor(x => x.PurPrice).PrecisionScale(19,4,true).WithMessage("采购价:小数位不能超过4。");

 RuleFor(x => x.SalePrice).PrecisionScale(19,4,true).WithMessage("销售价:小数位不能超过4。");

           	  
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

