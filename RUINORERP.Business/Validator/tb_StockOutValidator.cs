
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:35
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
    /// 出库单验证类
    /// </summary>
    /*public partial class tb_StockOutValidator:AbstractValidator<tb_StockOut>*/
    public partial class tb_StockOutValidator:BaseValidatorGeneric<tb_StockOut>
    {
     public tb_StockOutValidator() 
     {
      RuleFor(tb_StockOut =>tb_StockOut.Type_ID).Must(CheckForeignKeyValue).WithMessage("出库类型:下拉选择值不正确。");
 RuleFor(tb_StockOut =>tb_StockOut.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("接收单位:下拉选择值不正确。");
 RuleFor(tb_StockOut =>tb_StockOut.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
 RuleFor(tb_StockOut =>tb_StockOut.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");
 RuleFor(tb_StockOut =>tb_StockOut.BillNo).MaximumLength(25).WithMessage("其他出库单号:不能超过最大长度,25.");
//***** 
 RuleFor(tb_StockOut =>tb_StockOut.TotalQty).NotNull().WithMessage("总数量:不能为空。");
 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");
 RuleFor(tb_StockOut =>tb_StockOut.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_StockOut =>tb_StockOut.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_StockOut =>tb_StockOut.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
//***** 
 RuleFor(tb_StockOut =>tb_StockOut.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(tb_StockOut =>tb_StockOut.ApprovalOpinions).MaximumLength(250).WithMessage("审批意见:不能超过最大长度,250.");
 RuleFor(tb_StockOut =>tb_StockOut.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_StockOut =>tb_StockOut.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
 RuleFor(tb_StockOut =>tb_StockOut.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
 RuleFor(tb_StockOut =>tb_StockOut.RefNO).MaximumLength(25).WithMessage("引用单号:不能超过最大长度,25.");
 RuleFor(tb_StockOut =>tb_StockOut.RefBizType).NotEmpty().When(x => x.RefBizType.HasValue);
       	
           	                //long
                //MainID
                //tb_StockOutDetail
                //RuleFor(x => x.tb_StockOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_StockOutDetails).NotNull();
                //RuleForEach(x => x.tb_StockOutDetails).NotNull();
                //RuleFor(x => x.tb_StockOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
                Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_StockOutDetail> details)
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

