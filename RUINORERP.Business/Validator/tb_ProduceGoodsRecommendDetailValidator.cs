
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:13
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
    /// 自制成品建议验证类
    /// </summary>
    /*public partial class tb_ProduceGoodsRecommendDetailValidator:AbstractValidator<tb_ProduceGoodsRecommendDetail>*/
    public partial class tb_ProduceGoodsRecommendDetailValidator : BaseValidatorGeneric<tb_ProduceGoodsRecommendDetail>
    {
        public tb_ProduceGoodsRecommendDetailValidator()
        {
            //RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.PDID).Must(CheckForeignKeyValueCanNull).WithMessage(":下拉选择值不正确。");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.PDID).NotEmpty().When(x => x.PDID.HasValue);
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.ProdDetailID).Must(CheckForeignKeyValue).WithMessage("货品:下拉选择值不正确。");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.Location_ID).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.ID).NotEmpty().When(x => x.ID.HasValue);
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.ParentId).NotEmpty().When(x => x.ParentId.HasValue);
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.Specifications).MaximumLength(500).WithMessage("规格:不能超过最大长度,500.");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.BOM_ID).Must(CheckForeignKeyValueCanNull).WithMessage("标准配方:下拉选择值不正确。");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.BOM_ID).NotEmpty().When(x => x.BOM_ID.HasValue);
            RuleFor(x => x.SubtotalCostAmount).PrecisionScale(19, 4, true).WithMessage("成本小计:小数位不能超过4。");
            RuleFor(x => x.UnitCost).PrecisionScale(19, 4, true).WithMessage("单位成本:小数位不能超过4。");
            //***** 
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.RequirementQty).NotNull().WithMessage("请制量:不能为空。");
            //***** 
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.RecommendQty).NotNull().WithMessage("建议量:不能为空。");
            //***** 
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.PlanNeedQty).NotNull().WithMessage("计划需求数:不能为空。");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.RefBillNO).MaximumLength(50).WithMessage("生成单号:不能超过最大长度,50.");
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.RefBillType).NotEmpty().When(x => x.RefBillType.HasValue);
            RuleFor(tb_ProduceGoodsRecommendDetail => tb_ProduceGoodsRecommendDetail.RefBillID).NotEmpty().When(x => x.RefBillID.HasValue);

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

