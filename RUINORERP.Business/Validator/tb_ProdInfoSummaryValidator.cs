
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:09
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
    /// 商品信息汇总验证类
    /// </summary>
    /*public partial class tb_ProdInfoSummaryValidator:AbstractValidator<tb_ProdInfoSummary>*/
    public partial class tb_ProdInfoSummaryValidator:BaseValidatorGeneric<tb_ProdInfoSummary>
    {
     public tb_ProdInfoSummaryValidator() 
     {
      RuleFor(x => x.平均价格).PrecisionScale(10,2,true).WithMessage("平均价格:小数位不能超过2。");
 RuleFor(tb_ProdInfoSummary =>tb_ProdInfoSummary.总销售量).NotEmpty().When(x => x.总销售量.HasValue);
 RuleFor(tb_ProdInfoSummary =>tb_ProdInfoSummary.库存总量).NotEmpty().When(x => x.库存总量.HasValue);
       	
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

