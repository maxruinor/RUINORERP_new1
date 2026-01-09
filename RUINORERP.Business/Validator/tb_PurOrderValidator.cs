
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:21
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
    /// 采购订单，可能来自销售订单也可能来自生产需求也可以直接录数据验证类
    /// </summary>
    /*public partial class tb_PurOrderValidator:AbstractValidator<tb_PurOrder>*/
    public partial class tb_PurOrderValidator:BaseValidatorGeneric<tb_PurOrder>
    {
     

     public tb_PurOrderValidator(ApplicationContext appContext = null) : base(appContext)
     {
        
 
        
     
 RuleFor(tb_PurOrder =>tb_PurOrder.PurOrderNo).MaximumMixedLength(100).WithMessage("采购单号:不能超过最大长度,100.");
 RuleFor(tb_PurOrder =>tb_PurOrder.PurOrderNo).NotEmpty().WithMessage("采购单号:不能为空。");

 RuleFor(tb_PurOrder =>tb_PurOrder.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("厂商:下拉选择值不正确。");

 RuleFor(tb_PurOrder =>tb_PurOrder.Employee_ID).Must(CheckForeignKeyValue).WithMessage("经办人:下拉选择值不正确。");

 RuleFor(tb_PurOrder =>tb_PurOrder.DepartmentID).Must(CheckForeignKeyValueCanNull).WithMessage("使用部门:下拉选择值不正确。");
 RuleFor(tb_PurOrder =>tb_PurOrder.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
 RuleFor(tb_PurOrder =>tb_PurOrder.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.Paytype_ID).Must(CheckForeignKeyValueCanNull).WithMessage("交易方式:下拉选择值不正确。");
 RuleFor(tb_PurOrder =>tb_PurOrder.Paytype_ID).NotEmpty().When(x => x.Paytype_ID.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.PayeeInfoID).Must(CheckForeignKeyValueCanNull).WithMessage("收款信息:下拉选择值不正确。");
 RuleFor(tb_PurOrder =>tb_PurOrder.PayeeInfoID).NotEmpty().When(x => x.PayeeInfoID.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.SOrder_ID).Must(CheckForeignKeyValueCanNull).WithMessage("销售订单:下拉选择值不正确。");
 RuleFor(tb_PurOrder =>tb_PurOrder.SOrder_ID).NotEmpty().When(x => x.SOrder_ID.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.SOrderNo).MaximumMixedLength(50).WithMessage("订单编号:不能超过最大长度,50.");

 RuleFor(tb_PurOrder =>tb_PurOrder.PDID).Must(CheckForeignKeyValueCanNull).WithMessage("生产需求:下拉选择值不正确。");
 RuleFor(tb_PurOrder =>tb_PurOrder.PDID).NotEmpty().When(x => x.PDID.HasValue);



//***** 
 RuleFor(tb_PurOrder =>tb_PurOrder.TotalQty).NotNull().WithMessage("总数量:不能为空。");

//***** 
 RuleFor(tb_PurOrder =>tb_PurOrder.TotalUndeliveredQty).NotNull().WithMessage("未交数量:不能为空。");

 RuleFor(x => x.ForeignShipCost).PrecisionScale(19,4,true).WithMessage("运费外币:小数位不能超过4。");

 RuleFor(x => x.ShipCost).PrecisionScale(19,4,true).WithMessage("运费本币:小数位不能超过4。");

 RuleFor(x => x.TotalTaxAmount).PrecisionScale(19,4,true).WithMessage("总税额:小数位不能超过4。");

 RuleFor(x => x.TotalAmount).PrecisionScale(19,4,true).WithMessage("货款金额:小数位不能超过4。");

 RuleFor(x => x.TotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税总金额:小数位不能超过4。");



 RuleFor(tb_PurOrder =>tb_PurOrder.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);



 RuleFor(tb_PurOrder =>tb_PurOrder.Currency_ID).Must(CheckForeignKeyValue).WithMessage("币别:下拉选择值不正确。");

 RuleFor(x => x.ExchangeRate).PrecisionScale(10,4,true).WithMessage("汇率:小数位不能超过4。");

 RuleFor(x => x.ForeignTotalAmount).PrecisionScale(19,4,true).WithMessage("金额外币:小数位不能超过4。");

 RuleFor(x => x.ForeignDeposit).PrecisionScale(19,4,true).WithMessage("订金外币:小数位不能超过4。");

 RuleFor(x => x.Deposit).PrecisionScale(19,4,true).WithMessage("订金:小数位不能超过4。");

 RuleFor(tb_PurOrder =>tb_PurOrder.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);

//***** 
 RuleFor(tb_PurOrder =>tb_PurOrder.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

 RuleFor(tb_PurOrder =>tb_PurOrder.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");

 
 RuleFor(tb_PurOrder =>tb_PurOrder.CloseCaseOpinions).MaximumMixedLength(200).WithMessage("结案意见:不能超过最大长度,200.");

 RuleFor(tb_PurOrder =>tb_PurOrder.ApprovalOpinions).MaximumMixedLength(200).WithMessage("审批意见:不能超过最大长度,200.");



 RuleFor(tb_PurOrder =>tb_PurOrder.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


//***** 
 RuleFor(tb_PurOrder =>tb_PurOrder.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");



 RuleFor(tb_PurOrder =>tb_PurOrder.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


 RuleFor(tb_PurOrder =>tb_PurOrder.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);

 RuleFor(tb_PurOrder =>tb_PurOrder.RefNO).MaximumMixedLength(50).WithMessage("引用单据:不能超过最大长度,50.");

 RuleFor(tb_PurOrder =>tb_PurOrder.RefBizType).NotEmpty().When(x => x.RefBizType.HasValue);

           	                //long
                //PurOrder_ID
                //tb_PurOrderDetail
                //RuleFor(x => x.tb_PurOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
               //视图不需要验证，目前认为无编辑新增操作
                //RuleFor(c => c.tb_PurOrderDetails).NotNull();
                //RuleForEach(x => x.tb_PurOrderDetails).NotNull();
                //RuleFor(x => x.tb_PurOrderDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
     }




        private bool DetailedRecordsNotEmpty(List<tb_PurOrderDetail> details)
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

