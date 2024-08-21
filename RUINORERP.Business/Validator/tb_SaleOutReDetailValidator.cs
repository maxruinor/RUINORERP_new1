
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:35
// **************************************
using System;
using SqlSugar;
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
    /// 销售出库退回明细验证类
    /// </summary>
    public partial class tb_SaleOutReDetailValidator : AbstractValidator<tb_SaleOutReDetail>
    {
        public tb_SaleOutReDetailValidator()
        {
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品:下拉选择值不正确。");
            //***** 
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.SaleOutRe_ID).NotNull().WithMessage("销售退回单:不能为空。");
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.property).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
            //***** 
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Quantity).NotNull().WithMessage("退回数量:不能为空。");
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Quantity).GreaterThanOrEqualTo(0).WithMessage("退回数量:不能小于零。");
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Quantity).NotEqual(0).When(c => c.tb_saleoutre.RefundOnly == false).WithMessage("非仅退款时，退回数量不能为0为零。");

            RuleFor(x => x.TransactionPrice).PrecisionScale(19, 4, true).WithMessage("成交单价:小数位不能超过4。");
            RuleFor(x => x.SubtotalTransAmount).PrecisionScale(19, 4, true).WithMessage("小计:小数位不能超过4。");
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.Summary).MaximumLength(255).WithMessage("摘要:不能超过最大长度,255.");
            RuleFor(tb_SaleOutReDetail => tb_SaleOutReDetail.CustomerPartNo).MaximumLength(50).WithMessage("客户型号:不能超过最大长度,50.");
            RuleFor(x => x.Cost).PrecisionScale(19, 4, true).WithMessage("成本:小数位不能超过4。");
            RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19, 4, true).WithMessage("成本小计:小数位不能超过4。");

            RuleFor(x => x.SubtotalUntaxedAmount).PrecisionScale(19, 4, true).WithMessage("未税本位币:小数位不能超过4。");
            RuleFor(x => x.TaxRate).PrecisionScale(5, 2, true).WithMessage(":小数位不能超过2。");
            RuleFor(x => x.SubtotalTaxAmount).PrecisionScale(19, 4, true).WithMessage(":小数位不能超过4。");


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

