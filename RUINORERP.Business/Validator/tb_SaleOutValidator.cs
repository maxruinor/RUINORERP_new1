
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/18/2024 17:45:31
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
    /// 销售出库单验证类
    /// </summary>
    /*public partial class tb_SaleOutValidator:AbstractValidator<tb_SaleOut>*/
    public partial class tb_SaleOutValidator : BaseValidatorGeneric<tb_SaleOut>
    {

        //配置全局参数
        public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;

        public tb_SaleOutValidator(IOptionsMonitor<GlobalValidatorConfig> config)
        {

            ValidatorConfig = config;

            RuleFor(tb_SaleOut => tb_SaleOut.Employee_ID).Must(CheckForeignKeyValueCanNull).WithMessage("业务员:下拉选择值不正确。");
            RuleFor(tb_SaleOut => tb_SaleOut.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);

            RuleFor(tb_SaleOut => tb_SaleOut.CustomerVendor_ID).Must(CheckForeignKeyValue).WithMessage("客户:下拉选择值不正确。");
          

            RuleFor(tb_SaleOut => tb_SaleOut.SOrder_ID).Must(CheckForeignKeyValueCanNull).WithMessage("引用订单:下拉选择值不正确。");
            RuleFor(tb_SaleOut => tb_SaleOut.SOrder_ID).NotEmpty().When(x => x.SOrder_ID.HasValue);

            RuleFor(tb_SaleOut => tb_SaleOut.SaleOrderNo).MaximumLength(25).WithMessage("销售订单编号:不能超过最大长度,25.");

            RuleFor(tb_SaleOut => tb_SaleOut.SaleOutNo).MaximumLength(25).WithMessage("出库单号:不能超过最大长度,25.");
            RuleFor(tb_SaleOut => tb_SaleOut.SaleOutNo).NotEmpty().WithMessage("出库单号:不能为空。");

            RuleFor(tb_SaleOut => tb_SaleOut.ProjectGroup_ID).Must(CheckForeignKeyValueCanNull).WithMessage("项目组:下拉选择值不正确。");
            RuleFor(tb_SaleOut => tb_SaleOut.ProjectGroup_ID).NotEmpty().When(x => x.ProjectGroup_ID.HasValue);

            RuleFor(tb_SaleOut => tb_SaleOut.PayStatus).NotEmpty().When(x => x.PayStatus.HasValue);

            RuleFor(tb_SaleOut => tb_SaleOut.Paytype_ID).Must(CheckForeignKeyValue).WithMessage("付款方式:下拉选择值不正确。");
        

            RuleFor(x => x.FreightIncome).PrecisionScale(19, 4, true).WithMessage("运费:小数位不能超过4。");

            //***** 
            RuleFor(tb_SaleOut => tb_SaleOut.TotalQty).NotNull().WithMessage("总数量:不能为空。");

            RuleFor(x => x.TotalAmount).PrecisionScale(19, 4, true).WithMessage("总金额:小数位不能超过4。");


            RuleFor(tb_SaleOut => tb_SaleOut.ShippingAddress).MaximumLength(250).WithMessage("发货地址:不能超过最大长度,250.");

            RuleFor(tb_SaleOut => tb_SaleOut.ShippingWay).MaximumLength(25).WithMessage("发货方式:不能超过最大长度,25.");

            RuleFor(tb_SaleOut => tb_SaleOut.PlatformOrderNo).MaximumLength(50).WithMessage("平台单号:不能超过最大长度,50.");



            RuleFor(x => x.FreightCost).PrecisionScale(19, 4, true).WithMessage("运费成本:小数位不能超过4。");

            RuleFor(x => x.ForeignTotalAmount).PrecisionScale(19, 4, true).WithMessage("总金额外币:小数位不能超过4。");

            RuleFor(tb_SaleOut => tb_SaleOut.Created_by).NotEmpty().When(x => x.Created_by.HasValue);


            RuleFor(tb_SaleOut => tb_SaleOut.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

            RuleFor(tb_SaleOut => tb_SaleOut.Notes).MaximumLength(750).WithMessage("备注:不能超过最大长度,750.");

            RuleFor(tb_SaleOut => tb_SaleOut.ApprovalOpinions).MaximumLength(100).WithMessage("审批意见:不能超过最大长度,100.");

            RuleFor(tb_SaleOut => tb_SaleOut.Approver_by).NotEmpty().When(x => x.Approver_by.HasValue);


            RuleFor(tb_SaleOut => tb_SaleOut.KeepAccountsType).NotEmpty().When(x => x.KeepAccountsType.HasValue);

          

            RuleFor(tb_SaleOut => tb_SaleOut.TaxDeductionType).NotEmpty().When(x => x.TaxDeductionType.HasValue);

            //***** 
            RuleFor(tb_SaleOut => tb_SaleOut.PrintStatus).NotNull().WithMessage("打印状态:不能为空。");

            //***** 
            RuleFor(tb_SaleOut => tb_SaleOut.DataStatus).NotNull().WithMessage("数据状态:不能为空。");

            RuleFor(x => x.TotalCost).PrecisionScale(19, 4, true).WithMessage("总成本:小数位不能超过4。");

            

            RuleFor(x => x.TotalTaxAmount).PrecisionScale(19, 4, true).WithMessage("总税额:小数位不能超过4。");

       
    

        

            //long
            //SaleOut_MainID
            //tb_SaleOutDetail
            //RuleFor(x => x.tb_SaleOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
            //视图不需要验证，目前认为无编辑新增操作
            //RuleFor(c => c.tb_SaleOutDetails).NotNull();
            //RuleForEach(x => x.tb_SaleOutDetails).NotNull();
            //RuleFor(x => x.tb_SaleOutDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
            Initialize();
        }




        private bool DetailedRecordsNotEmpty(List<tb_SaleOutDetail> details)
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

