
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：06/26/2024 19:53:43
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
    /// 验证类
    /// </summary>
    public partial class Proc_SaleOutStatisticsByEmployeeValidator:AbstractValidator<Proc_SaleOutStatisticsByEmployee>
    {
     public Proc_SaleOutStatisticsByEmployeeValidator() 
     {
      RuleFor(Proc_SaleOutStatisticsByEmployee =>Proc_SaleOutStatisticsByEmployee.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(Proc_SaleOutStatisticsByEmployee =>Proc_SaleOutStatisticsByEmployee.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);
 RuleFor(Proc_SaleOutStatisticsByEmployee =>Proc_SaleOutStatisticsByEmployee.总销售出库数量).NotEmpty().When(x => x.总销售出库数量.HasValue);
 RuleFor(x => x.出库成交金额).PrecisionScale(19,255,true).WithMessage(":小数位不能超过255。");
 RuleFor(Proc_SaleOutStatisticsByEmployee =>Proc_SaleOutStatisticsByEmployee.退货数量).NotEmpty().When(x => x.退货数量.HasValue);
 RuleFor(x => x.退货金额).PrecisionScale(19,255,true).WithMessage(":小数位不能超过255。");
 RuleFor(x => x.销售税额).PrecisionScale(19,255,true).WithMessage(":小数位不能超过255。");
 RuleFor(x => x.退货税额).PrecisionScale(19,255,true).WithMessage(":小数位不能超过255。");
 RuleFor(x => x.佣金返点).PrecisionScale(19,255,true).WithMessage(":小数位不能超过255。");
 RuleFor(x => x.佣金返还).PrecisionScale(19,255,true).WithMessage(":小数位不能超过255。");
 RuleFor(Proc_SaleOutStatisticsByEmployee =>Proc_SaleOutStatisticsByEmployee.实际成交数量).NotEmpty().When(x => x.实际成交数量.HasValue);
 RuleFor(x => x.实际成交金额).PrecisionScale(19,255,true).WithMessage(":小数位不能超过255。");
       	
           	
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

