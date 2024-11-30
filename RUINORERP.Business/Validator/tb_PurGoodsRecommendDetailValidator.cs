
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:21
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
    /// 采购商品建议验证类
    /// </summary>
    /*public partial class tb_PurGoodsRecommendDetailValidator:AbstractValidator<tb_PurGoodsRecommendDetail>*/
    public partial class tb_PurGoodsRecommendDetailValidator : BaseValidatorGeneric<tb_PurGoodsRecommendDetail>
    {
        public tb_PurGoodsRecommendDetailValidator()
        {
            //主子表时 明细中的主表ID不用设置。这个因为没有适合+DELTAIL规律，手动注释
            //RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.PDID).Must(CheckForeignKeyValue).WithMessage(":下拉选择值不正确。");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.CustomerVendor_ID).Must(CheckForeignKeyValueCanNull).WithMessage("供应商:下拉选择值不正确。");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.CustomerVendor_ID).NotEmpty().When(x => x.CustomerVendor_ID.HasValue);
            RuleFor(x => x.RecommendPurPrice).PrecisionScale(19, 6, true).WithMessage("建议采购价:小数位不能超过6。");
            //***** 
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.ActualRequiredQty).NotNull().WithMessage("需求数量:不能为空。");
            //***** 
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.RecommendQty).NotNull().WithMessage("建议量:不能为空。");
            //***** 
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.RequirementQty).NotNull().WithMessage("请购量:不能为空。");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.Summary).MaximumLength(127).WithMessage("摘要:不能超过最大长度,127.");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.RefBillNO).MaximumLength(50).WithMessage("生成单号:不能超过最大长度,50.");
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.RefBillType).NotEmpty().When(x => x.RefBillType.HasValue);
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);
            RuleFor(tb_PurGoodsRecommendDetail => tb_PurGoodsRecommendDetail.PDCID_RowID).NotEmpty().When(x => x.PDCID_RowID.HasValue);

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

