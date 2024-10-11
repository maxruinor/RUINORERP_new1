
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/10/2024 14:15:54
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
    /// 产品转换单明细验证类
    /// </summary>
    /*public partial class tb_ProdConversionDetailValidator:AbstractValidator<tb_ProdConversionDetail>*/
    public partial class tb_ProdConversionDetailValidator : BaseValidatorGeneric<tb_ProdConversionDetail>
    {
        public tb_ProdConversionDetailValidator()
        {
            //***** 
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.ConversionID).NotNull().WithMessage("组合单:不能为空。");

            //***** 
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.ProdDetailID_from).NotNull().WithMessage("来源产品:不能为空。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.ProdDetailID_to).NotNull().WithMessage("目标产品:不能为空。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.ProdDetailID_from).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.ProdDetailID_to).Must(CheckForeignKeyValue).WithMessage("库位:下拉选择值不正确。");

            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.SKU_from).MaximumLength(127).WithMessage("来源SKU:不能超过最大长度,127.");
            //***** 
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.Type_ID_from).NotNull().WithMessage("来源产品类型:不能为空。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.CNName_from).MaximumLength(127).WithMessage("来源品名:不能超过最大长度,127.");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.CNName_from).NotEmpty().WithMessage("来源品名:不能为空。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.Model_from).MaximumLength(25).WithMessage("来源型号:不能超过最大长度,25.");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.Specifications_from).MaximumLength(500).WithMessage("来源规格:不能超过最大长度,500.");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.property_from).MaximumLength(127).WithMessage("来源属性:不能超过最大长度,127.");
            //***** 
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.ConversionQty).NotNull().WithMessage("转换数量:不能为空。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.SKU_to).MaximumLength(127).WithMessage("目标属性:不能超过最大长度,127.");
            //***** 
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.Type_ID_to).NotNull().WithMessage("目标产品类型:不能为空。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.CNName_to).MaximumLength(127).WithMessage("目标品名:不能超过最大长度,127.");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.CNName_to).NotEmpty().WithMessage("目标品名:不能为空。");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.Model_to).MaximumLength(25).WithMessage("目标型号:不能超过最大长度,25.");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.Specifications_to).MaximumLength(500).WithMessage("目标规格:不能超过最大长度,500.");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.property_to).MaximumLength(127).WithMessage("目标属性:不能超过最大长度,127.");
            RuleFor(tb_ProdConversionDetail => tb_ProdConversionDetail.Summary).MaximumLength(500).WithMessage("摘要:不能超过最大长度,500.");

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

