
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/19/2024 11:25:44
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
    /// 缴库明细统计验证类
    /// </summary>
    /*public partial class View_FinishedGoodsInvItemsValidator:AbstractValidator<View_FinishedGoodsInvItems>*/
    public partial class View_FinishedGoodsInvItemsValidator : BaseValidatorGeneric<View_FinishedGoodsInvItems>
    {
        public View_FinishedGoodsInvItemsValidator()
        {

            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.DeliveryBillNo).MaximumLength(25).WithMessage("缴库单号:不能超过最大长度,25.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Employee_ID).NotEmpty().When(x => x.Employee_ID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.DepartmentID).NotEmpty().When(x => x.DepartmentID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.MONo).MaximumLength(25).WithMessage("制令单号:不能超过最大长度,25.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Location_ID).NotEmpty().When(x => x.Location_ID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.PayableQty).NotEmpty().When(x => x.PayableQty.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Qty).NotEmpty().When(x => x.Qty.HasValue);

            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.ProdBaseID).NotEmpty().When(x => x.ProdBaseID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.CNName).MaximumLength(127).WithMessage("品名:不能超过最大长度,127.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Quantity).NotEmpty().When(x => x.Quantity.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.prop).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.ProductNo).MaximumLength(20).WithMessage("品号:不能超过最大长度,20.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Model).MaximumLength(25).WithMessage("型号:不能超过最大长度,25.");
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Category_ID).NotEmpty().When(x => x.Category_ID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Type_ID).NotEmpty().When(x => x.Type_ID.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.DataStatus).NotEmpty().When(x => x.DataStatus.HasValue);
            RuleFor(View_FinishedGoodsInvItems => View_FinishedGoodsInvItems.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");

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

