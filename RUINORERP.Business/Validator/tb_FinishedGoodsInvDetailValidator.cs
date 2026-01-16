
// **************************************
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：11/10/2025 23:38:11
// **************************************
using System;
using SqlSugar;
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
    /// 成品入库单明细验证类
    /// </summary>
    /*public partial class tb_FinishedGoodsInvDetailValidator:AbstractValidator<tb_FinishedGoodsInvDetail>*/
    public partial class tb_FinishedGoodsInvDetailValidator : BaseValidatorGeneric<tb_FinishedGoodsInvDetail>
    {


        public tb_FinishedGoodsInvDetailValidator(ApplicationContext appContext = null) : base(appContext)
        {




            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.FG_ID).NotEmpty().When(x => x.FG_ID.HasValue);

            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.Unit_ID).Must(CheckForeignKeyValueCanNull).WithMessage("单位:下拉选择值不正确。");
            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.Unit_ID).NotEmpty().When(x => x.Unit_ID.HasValue);

            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品详情:下拉选择值不正确。");

            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.Rack_ID).Must(CheckForeignKeyValueCanNull).WithMessage("货架:下拉选择值不正确。");
            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.Rack_ID).NotEmpty().When(x => x.Rack_ID.HasValue);

            //***** 
            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.PayableQty).NotNull().WithMessage("应缴数量:不能为空。");

            //***** 
            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.Qty).NotNull().WithMessage("实缴数量:不能为空。");

            RuleFor(x => x.UnitCost).PrecisionScale(19, 4, true).WithMessage("单位成本:小数位不能超过4。");

            //***** 
            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.UnpaidQty).NotNull().WithMessage("未缴数量:不能为空。");

            RuleFor(x => x.NetMachineHours).PrecisionScale(15, 5, true).WithMessage("单位实际机时:小数位不能超过5。");

            RuleFor(x => x.NetWorkingHours).PrecisionScale(15, 5, true).WithMessage("单位实际工时:小数位不能超过5。");

            RuleFor(x => x.ApportionedCost).PrecisionScale(19, 4, true).WithMessage("单位分摊成本:小数位不能超过4。");

            RuleFor(x => x.ManuFee).PrecisionScale(19, 4, true).WithMessage("单位制造费用:小数位不能超过4。");

            RuleFor(x => x.MaterialCost).PrecisionScale(19, 4, true).WithMessage("单位材料成本:小数位不能超过4。");

            RuleFor(x => x.ProductionAllCost).PrecisionScale(19, 4, true).WithMessage("成本小计:小数位不能超过4。");

            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.Summary).MaximumMixedLength(255).WithMessage("摘要:不能超过最大长度,255.");

            RuleFor(tb_FinishedGoodsInvDetail => tb_FinishedGoodsInvDetail.property).MaximumMixedLength(255).WithMessage("属性:不能超过最大长度,255.");


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

