
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:28
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
    /// 销售订单验证类
    /// </summary>
    /*public partial class tb_SaleOrderValidator:AbstractValidator<tb_SaleOrder>*/
    public partial class tb_SaleOrderValidator:BaseValidatorGeneric<tb_SaleOrder>
    {
     public tb_SaleOrderValidator() 
     {
      RuleFor(tb_SaleOrder =>tb_SaleOrder.SOrderNo).MaximumLength(25).WithMessage("订单编号:不能超过最大长度,25.");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.SOrderNo).NotEmpty().WithMessage("订单编号:不能为空。");
//***** 
 RuleFor(tb_SaleOrder =>tb_SaleOrder.PayStatus).NotNull().WithMessage("付款状态:不能为空。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("付款类型:下拉选择值不正确。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);
 RuleFor(tb_SaleOrder =>tb_SaleOrder.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("客户:下拉选择值不正确。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
 RuleFor(tb_SaleOrder =>tb_SaleOrder.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);
 RuleFor(x => x.ShipCost).PrecisionScale(19,4,true).WithMessage("运费:小数位不能超过4。");
//***** 
 RuleFor(tb_SaleOrder =>tb_SaleOrder.TotalQty).NotNull().WithMessage("总数量:不能为空。");
 RuleFor(x => x.TotalCost).PrecisionScale(19,4,true).WithMessage("总成本:小数位不能超过4。");
 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("总金额:小数位不能超过4。");
 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("总税额:小数位不能超过4。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.ShippingAddress).MaximumLength(250).WithMessage("发货地址:不能超过最大长度,250.");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.ShippingWay).MaximumLength(25).WithMessage("发货方式:不能超过最大长度,25.");
 RuleFor(x => x.CollectedMoney).PrecisionScale(19,4,true).WithMessage("实收金额:小数位不能超过4。");
 RuleFor(x => x.PrePayMoney).PrecisionScale(19,4,true).WithMessage("预收款:小数位不能超过4。");
 RuleFor(x => x.Deposit).PrecisionScale(19,4,true).WithMessage("订金:小数位不能超过4。");
 RuleFor(x => x.TaxRate).PrecisionScale(5,3,true).WithMessage("税率:小数位不能超过3。");
 RuleFor(x => x.TotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);
 RuleFor(tb_SaleOrder =>tb_SaleOrder.CloseCaseOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.ApprovalOpinions).MaximumLength(127).WithMessage("审批意见:不能超过最大长度,127.");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);
//***** 
 RuleFor(tb_SaleOrder =>tb_SaleOrder.DataStatus).NotNull().WithMessage("数据状态:不能为空。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);
 RuleFor(tb_SaleOrder =>tb_SaleOrder.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);
//***** 
 RuleFor(tb_SaleOrder =>tb_SaleOrder.OrderPriority).NotNull().WithMessage("紧急程度:不能为空。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.PlatformOrderNo).MaximumLength(50).WithMessage("平台单号:不能超过最大长度,50.");
//***** 
 RuleFor(tb_SaleOrder =>tb_SaleOrder.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
 RuleFor(tb_SaleOrder =>tb_SaleOrder.RefNO).MaximumLength(25).WithMessage("引用单号:不能超过最大长度,25.");
 RuleFor(tb_SaleOrder =>tb_SaleOrder.RefBizType).NotEmpty().When(x => x.RefBizType.HasValue);
       	
           	                //long
                //SOrder_ID
                //tb_SaleOrderDetail
                //RuleFor(x => x.tb_SaleOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_SaleOrderDetails).NotNull();
                //RuleForEach(x => x.tb_SaleOrderDetails).NotNull();
                //RuleFor(x => x.tb_SaleOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
        
                Initialize();
     }




        private bool DetailedRecordsNotEmpty(List<tb_SaleOrderDetail> details)
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

