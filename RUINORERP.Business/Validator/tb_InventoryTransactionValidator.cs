
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:13
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
    /// 库存流水表验证类
    /// </summary>
    /*public partial class tb_InventoryTransactionValidator:AbstractValidator<tb_InventoryTransaction>*/
    public partial class tb_InventoryTransactionValidator:BaseValidatorGeneric<tb_InventoryTransaction>
    {
     

     public tb_InventoryTransactionValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
//***** 
 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.ProdDetailID).NotNull().WithMessage("产品详情:不能为空。");

//***** 
 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.Location_ID).NotNull().WithMessage("库位:不能为空。");

//***** 
 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.BizType).NotNull().WithMessage("业务类型:不能为空。");

//***** 
 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.ReferenceId).NotNull().WithMessage("业务单据:不能为空。");

//***** 
 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.QuantityChange).NotNull().WithMessage("变动数量:不能为空。");

//***** 
 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.AfterQuantity).NotNull().WithMessage("变后数量:不能为空。");

//***** 
 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.BatchNumber).NotNull().WithMessage("批号:不能为空。");

 RuleFor(x => x.UnitCost).PrecisionScale(19,4,true).WithMessage("单位成本:小数位不能超过4。");


 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.OperatorId).NotEmpty().When(x => x.OperatorId.HasValue);

 RuleFor(tb_InventoryTransaction =>tb_InventoryTransaction.Notes).MaximumMixedLength(250).WithMessage("备注说明:不能超过最大长度,250.");

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

