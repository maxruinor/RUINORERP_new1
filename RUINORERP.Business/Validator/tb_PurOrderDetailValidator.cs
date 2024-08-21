
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：03/06/2024 13:53:34
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
    /// 采购订单明细表验证类
    /// </summary>
    public partial class tb_PurOrderDetailValidator : AbstractValidator<tb_PurOrderDetail>
    {
        public tb_PurOrderDetailValidator()
        {
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("产品详情:下拉选择值不正确。");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.PurOrder_ID).NotNull().WithMessage(":不能为空。");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.Quantity).NotNull().WithMessage("数量:不能为空。");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.Quantity).GreaterThanOrEqualTo(1).WithMessage("数量:不能小于或等于零。");

            RuleFor(x => x.UnitPrice).PrecisionScale(19, 6, true).WithMessage("单价:小数位不能超过6。");
            RuleFor(x => x.Discount).PrecisionScale(5, 3, true).WithMessage("折扣:小数位不能超过3。");
            RuleFor(x => x.TransactionPrice).PrecisionScale(19, 6, true).WithMessage("成交单价:小数位不能超过6。");
            RuleFor(x => x.IsGift).Equal(true).When(c => c.SubtotalAmount == 0).WithMessage("成交金额为0时，必须为赠品。");
            RuleFor(x => x.TaxRate).PrecisionScale(5, 3, true).WithMessage("税率:小数位不能超过3。");
            RuleFor(x => x.TaxAmount).PrecisionScale(19, 6, true).WithMessage("税额:小数位不能超过6。");
            RuleFor(x => x.SubtotalAmount).PrecisionScale(19, 6, true).WithMessage("成交金额:小数位不能超过6。");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.CustomertModel).MaximumLength(50).WithMessage("客户型号:不能超过最大长度,50.");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.DeliveredQuantity).NotNull().WithMessage("已交数量:不能为空。");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.Notes).MaximumLength(255).WithMessage("备注:不能超过最大长度,255.");
            RuleFor(tb_PurOrderDetail => tb_PurOrderDetail.property).MaximumLength(255).WithMessage("属性:不能超过最大长度,255.");
            
            RuleFor(x => x.PreDeliveryDate).GreaterThan(c => c.tb_purorder.PurDate.AddDays(0)).WithMessage("明细中预交日期必须大于采购日期。");
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

