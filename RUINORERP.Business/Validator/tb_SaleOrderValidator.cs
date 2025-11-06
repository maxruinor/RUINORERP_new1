
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:13
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using RUINORERP.Model;
using FluentValidation;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Options;

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
    public partial class tb_SaleOrderValidator : BaseValidatorGeneric<tb_SaleOrder>
    {

        //配置全局参数
        public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;

        public tb_SaleOrderValidator(IOptionsMonitor<GlobalValidatorConfig> config)
        {

            ValidatorConfig = config;




            RuleFor(tb_SaleOrder => tb_SaleOrder.SOrderNo).MaximumMixedLength(50).WithMessage("订单编号:不能超过最大长度,50.");
            RuleFor(tb_SaleOrder => tb_SaleOrder.SOrderNo).NotEmpty().WithMessage("订单编号:不能为空。");

            //***** 
            RuleFor(tb_SaleOrder => tb_SaleOrder.PayStatus).NotNull().WithMessage("付款状态:不能为空。");

            //***** 
            RuleFor(tb_SaleOrder => tb_SaleOrder.CustomerVendor_ID).NotNull().WithMessage("客户:不能为空。");

            RuleFor(tb_SaleOrder => tb_SaleOrder.Account_id).NotEmpty().When(x => x.Account_id.HasValue);


            RuleFor(x => x.ExchangeRate).PrecisionScale(10, 4, true).WithMessage("汇率:小数位不能超过4。");

            //***** 
            RuleFor(tb_SaleOrder => tb_SaleOrder.Employee_ID).NotNull().WithMessage("业务员:不能为空。");

            RuleFor(tb_SaleOrder => tb_SaleOrder.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

            RuleFor(x => x.ForeignFreightIncome).PrecisionScale(19, 4, true).WithMessage("运费收入外币:小数位不能超过4。");

            RuleFor(x => x.FreightIncome).PrecisionScale(19, 4, true).WithMessage("运费收入:小数位不能超过4。");

            //***** 
            RuleFor(tb_SaleOrder => tb_SaleOrder.TotalQty).NotNull().WithMessage("总数量:不能为空。");

            RuleFor(x => x.TotalCost).PrecisionScale(19, 4, true).WithMessage("总成本:小数位不能超过4。");

            RuleFor(x => x.TotalAmount).PrecisionScale(19, 4, true).WithMessage("总金额:小数位不能超过4。");

            RuleFor(x => x.TotalCommissionAmount).PrecisionScale(19, 4, true).WithMessage("佣金金额:小数位不能超过4。");

            RuleFor(x => x.TotalTaxAmount).PrecisionScale(19, 4, true).WithMessage("总税额:小数位不能超过4。");




            RuleFor(tb_SaleOrder => tb_SaleOrder.ShippingAddress).MaximumMixedLength(500).WithMessage("收货地址:不能超过最大长度,500.");

            RuleFor(tb_SaleOrder => tb_SaleOrder.ShippingWay).MaximumMixedLength(50).WithMessage("发货方式:不能超过最大长度,50.");


            RuleFor(x => x.ForeignTotalAmount).PrecisionScale(19, 4, true).WithMessage("金额外币:小数位不能超过4。");


            RuleFor(tb_SaleOrder => tb_SaleOrder.CustomerPONo).MaximumMixedLength(50).WithMessage("客户订单号:不能超过最大长度,50.");

            RuleFor(x => x.ForeignDeposit).PrecisionScale(19, 4, true).WithMessage("订金外币:小数位不能超过4。");

            RuleFor(x => x.Deposit).PrecisionScale(19, 4, true).WithMessage("订金:小数位不能超过4。");


            //RuleFor(x => x.TotalUntaxedAmount).PrecisionScale(19,4,true).WithMessage("未税本位币:小数位不能超过4。");


            RuleFor(tb_SaleOrder => tb_SaleOrder.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


            RuleFor(tb_SaleOrder => tb_SaleOrder.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

            RuleFor(tb_SaleOrder => tb_SaleOrder.CloseCaseOpinions).MaximumMixedLength(200).WithMessage("结案意见:不能超过最大长度,200.");

            RuleFor(tb_SaleOrder => tb_SaleOrder.Notes).MaximumMixedLength(1500).WithMessage("备注:不能超过最大长度,1500.");



            RuleFor(tb_SaleOrder => tb_SaleOrder.ApprovalOpinions).MaximumMixedLength(255).WithMessage("审批意见:不能超过最大长度,255.");

            RuleFor(tb_SaleOrder => tb_SaleOrder.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);




            //***** 
            RuleFor(tb_SaleOrder => tb_SaleOrder.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

            RuleFor(tb_SaleOrder => tb_SaleOrder.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);

            RuleFor(tb_SaleOrder => tb_SaleOrder.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);

            //***** 
            RuleFor(tb_SaleOrder => tb_SaleOrder.OrderPriority).NotNull().WithMessage("紧急程度:不能为空。");

            RuleFor(tb_SaleOrder => tb_SaleOrder.PlatformOrderNo).MaximumMixedLength(100).WithMessage("平台单号:不能超过最大长度,100.");

            //***** 
            RuleFor(tb_SaleOrder => tb_SaleOrder.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");


            RuleFor(tb_SaleOrder => tb_SaleOrder.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);

            RuleFor(tb_SaleOrder => tb_SaleOrder.RefNO).MaximumMixedLength(50).WithMessage("引用单号:不能超过最大长度,50.");

            RuleFor(tb_SaleOrder => tb_SaleOrder.RefBizType).NotEmpty().When(x => x.RefBizType.HasValue);

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

