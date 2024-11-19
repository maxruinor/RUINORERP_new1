
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 18:44:01
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
    /// 包装规格表验证类
    /// </summary>
    /*public partial class tb_PackingValidator:AbstractValidator<tb_Packing>*/
    public partial class tb_PackingValidator : BaseValidatorGeneric<tb_Packing>
    {
        public tb_PackingValidator()
        {
            RuleFor(tb_Packing => tb_Packing.PackagingName).MaximumLength(127).WithMessage("包装名称:不能超过最大长度,127.");
            RuleFor(tb_Packing => tb_Packing.ProdDetailID).NotEmpty().When(x => x.ProdDetailID.HasValue);
            RuleFor(tb_Packing => tb_Packing.BundleID).NotEmpty().When(x => x.BundleID.HasValue);
            RuleFor(tb_Packing => tb_Packing.ProdBaseID).NotEmpty().When(x => x.ProdBaseID.HasValue);
            RuleFor(tb_Packing => tb_Packing.Unit_ID).Must(CheckForeignKeyValue).WithMessage("包装单位:下拉选择值不正确。");
            RuleFor(tb_Packing => tb_Packing.SKU).MaximumLength(40).WithMessage("SKU码:不能超过最大长度,40.");
            RuleFor(tb_Packing => tb_Packing.property).MaximumLength(127).WithMessage("属性:不能超过最大长度,127.");
            RuleFor(tb_Packing => tb_Packing.BoxMaterial).MaximumLength(100).WithMessage("盒子材质:不能超过最大长度,100.");
            RuleFor(x => x.Length).PrecisionScale(8, 2, true).WithMessage("长度(cm):小数位不能超过2。");
            RuleFor(x => x.Width).PrecisionScale(8, 2, true).WithMessage("宽度(cm):小数位不能超过2。");
            RuleFor(x => x.Height).PrecisionScale(8, 2, true).WithMessage("高度(cm):小数位不能超过2。");
            RuleFor(x => x.Volume).PrecisionScale(10, 3, true).WithMessage("体积Vol(cm³):小数位不能超过3。");
            RuleFor(x => x.NetWeight).PrecisionScale(10, 3, true).WithMessage("净重N.Wt.(g):小数位不能超过3。");
            RuleFor(x => x.GrossWeight).PrecisionScale(10, 3, true).WithMessage("毛重G.Wt.(g):小数位不能超过3。");
            RuleFor(tb_Packing => tb_Packing.Notes).MaximumLength(127).WithMessage("备注:不能超过最大长度,127.");
            //有默认值
            RuleFor(tb_Packing => tb_Packing.Created_by).NotEmpty().When(x => x.Created_by.HasValue);
            RuleFor(tb_Packing => tb_Packing.Modified_by).NotEmpty().When(x => x.Modified_by.HasValue);

            //long
            //Pack_ID
            //tb_PackingDetail
            //RuleFor(x => x.tb_PackingDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");
            //视图不需要验证，目前认为无编辑新增操作
            //RuleFor(c => c.tb_PackingDetails).NotNull();
            //RuleForEach(x => x.tb_PackingDetails).NotNull();
            //RuleFor(x => x.tb_PackingDetails).Must(DetailedRecordsNotEmpty).WithMessage("明细不能为空");

            Initialize();
        }




        private bool DetailedRecordsNotEmpty(List<tb_PackingDetail> details)
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

